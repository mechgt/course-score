using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;
using System.Reflection;

using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Chart;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Data.Fitness.CustomData;
using ZoneFiveSoftware.Common.Visuals.Mapping;

using CourseScore.Data;
using CourseScore.Settings;
using CourseScore.UI.Popups;
using CourseScore.UI.MapLayers;
using ZoneFiveSoftware.Common.Visuals.Forms;
using System.Collections;
using System.IO;

namespace CourseScore.UI.DetailPage
{
    public partial class HillsDetailControl : UserControl
    {
        #region Fields

        public IActivityReportsView reportView;
        private IEnumerable<IActivity> activities;
        private IActivity activity;
        private string currentSortColumnId = "Date";
        private bool currentSortDirection = true;
        //private HillsPointsProvider hillsPointsProvider = null; //HillsPointsProvider.Instance;
        private int scoreSmoothing = 10;
        public bool maximized = false;

        private IView currentView;

        private HillsPointsLayer layer = null;
        /*{
            get
            {
                return (HillsPointsLayer)hillsPointsProvider.RouteControlLayer;
            }
        }*/
        private Mode mode;
        private bool zoomed = false;
        private double cyclingFactor = .35f;
        private double cyclingOffset = .1f;

        #endregion

        #region Public enums

        public enum HillChartType
        {
            Overall,
            ClimbDistance,
            ClimbTime,
            DescentDistance,
            DescentTime,
            SplitsDistance,
            SplitsTime
        }

        public enum InfoType
        {
            Overall,
            Details,
            Features
        }

        public enum ScoreType
        {
            Cycling,
            Running
        }

        public enum ColorType
        {
            Basic,
            Longest,
            Hardest,
            Steepest
        }

        public enum RightClickMenu
        {
            Clear,
            ExactHill,
            LikeHill
        }

        public enum RightClickMenuSplits
        {
            Clear,
            ExactSplit,
            Route
        }

        public enum Mode
        {
            OneActivity,
            MultipleActivities
        }

        public enum YAxisLR
        {
            Left,
            Right
        }

        public enum TreeListRightClickMenu
        {
            ListSettings
        }

        /*public enum HillChartTypeMultiMode
        {
            Details,
            Summary
        }*/

        public enum HillChartTypeMultiMode
        {
            Details,
            Cadence,
            Elevation,
            Grade,
            HR,
            Power,
            Speed,
            VAM
        }
        #endregion

        #region Events

        internal event EventHandler Maximize;

        #endregion

        #region Constructors

        public HillsDetailControl()
        {
            InitializeComponent();

            // Remove the horizontal scrollbar from the top control
            treeList1.Controls.Remove(treeList1.HScrollBar);

            // Remove the vertical scrollbar from the bottom control
            treeList_subtotals.Controls.Remove(treeList_subtotals.VScrollBar);

            // Set the subtotals bar to have no header
            treeList_subtotals.HeaderRowHeight = 0;
            treeList_subtotals.NumHeaderRows = 0;

            /* FieldInfo eventsField = typeof(Component).GetField("events", BindingFlags.NonPublic | BindingFlags.Instance);
             EventHandlerList eventHandlerList = (EventHandlerList) eventsField.GetValue(treeList_subtotals);
            
            
             Delegate handlers = eventHandlerList[0];*/

            //foreach(Delegate handler in eventHandlerList.getin

            /* EventHandlerList events = (EventHandlerList)typeof(Component)
                     .GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance)
                     .GetValue(treeList_subtotals, null);*/
            /*if (events != null)
            {
                object key = typeof(TreeList)
                        .GetField("Scroll", BindingFlags.NonPublic | BindingFlags.Static)
                        .GetValue(treeList_subtotals);
            }
            treeList1.*/

            /*treeList_subtotals.HScrollBar.Scroll += new ScrollEventHandler(Scroll);

            EventInfo sc = treeList_subtotals.HScrollBar.GetType().GetEvent("Scroll");
            Type tDelegate = sc.EventHandlerType;
            MethodInfo miHandler = typeof(ScrollBar).GetMethod("EVENT_SCROLL", BindingFlags.NonPublic | BindingFlags.Static);

            Delegate d = Delegate.CreateDelegate(tDelegate, miHandler);
            MethodInfo miAddHandler = sc.GetAddMethod();
            object[] addHandlerArgs = { d };
            miAddHandler.Invoke(treeList1.HScrollBar, addHandlerArgs);*/



            /*treeList1.HScrollBar.Scroll += delegate
            {
                typeof(HScrollBar).GetMethod("Scroll", BindingFlags.NonPublic | BindingFlags.Instance)
                    .Invoke(treeList_subtotals.HScrollBar, new object[] { null });
            };*/

            //treeList_subtotals.Scroll += new ScrollEventHandler(Scroll);

            // Fix button drift
            MaximizeButton.Location = new Point(ChartBanner.Width - 50, 0);
            TreeRefreshButton.Location = new Point(infoBanner.Width - 50, 0);

            // Hide the treelist by default
            treeList1.Visible = false;
            treeList_subtotals.Visible = false;

            // Set up the splitter based on last position
            splitContainer1.SplitterDistance = GlobalSettings.Instance.SplitterDistance;

            mode = Mode.OneActivity;

            SetupMenuItems();
            // TODO: Un-comment axis label selection handler assignment below when API is straightened out
            //MainChart.SelectAxisLabel += new ChartBase.SelectAxisLabelHandler(MainChart_SelectAxisLabel);

            //layer = HillsPointsLayer.Instance;

            // Handler to know when a selction is made on the map
            //if (layer != null)
            //{
            //    layer.RouteControlSelectedItemsChanged += new EventHandler(event_RouteControlSelectedItemsChanged);
            //}
        }

        void MainChart_SelectAxisLabel(object sender, ChartBase.AxisEventArgs e)
        {
            // TODO: API bug.  MainChart.SelectAxisLabel handler doesn't fire.
            // http://www.zonefivesoftware.com/sporttracks/forums/viewtopic.php?p=50702#50702
            // Note that code below is therefore untested.

            IAxis axis = sender as IAxis;
            if (treeList1.RowData != null)
            {
                zoomed = false;
                List<Feature> features = treeList1.RowData as List<Feature>;

                // Loop through all data series
                foreach (ChartDataSeries series in MainChart.DataSeries)
                {
                    // Look for axis of interest
                    if (series.ValueAxis == axis)
                    {
                        // Find matching feature so that a color can be assigned
                        foreach (Feature feature in features)
                        {
                            if (feature.record.Activity.ReferenceId == series.Data as string)
                            {
                                series.LineColor = Color.FromArgb(255, feature.selectedColor);
                                //series.LineWidth = 1;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Reset all non-selected axes (only 1 at a time?)
                        series.LineColor = Color.FromArgb(series.LineColor.A, axis.LabelColor);
                    }
                }

                MainChart.Refresh();
            }
        }

        #endregion

        #region Control Functions

        /// <summary>
        /// Set the current view in ST (Reports, Daily, Athlete, etc)
        /// </summary>
        internal IView CurrentView
        {
            set
            {
                currentView = value;
            }
        }

        /// <summary>
        /// Set Activities to be used for calculation
        /// </summary>
        internal IEnumerable<IActivity> Activities
        {
            set
            {
                activities = value;
                RefreshPage(true);
            }
        }

        /// <summary>
        /// Jumping off point for calcs.  RefreshPage() is called every time the page is refreshed.  Crazy.
        /// </summary>
        internal void RefreshPage(bool autozoom)
        {
            layer = HillsPointsLayer.Instance;

            // Handler to know when a selction is made on the map
            if (layer != null)
            {
                layer.RouteControlSelectedItemsChanged -= new EventHandler(event_RouteControlSelectedItemsChanged);
                layer.RouteControlSelectedItemsChanged += new EventHandler(event_RouteControlSelectedItemsChanged);
            }


            // Check to see what display to show on the top half
            if (GlobalSettings.Instance.InfoType == InfoType.Overall)
            {
                treeList1.Visible = false;
                treeList_subtotals.Visible = false;
                infoBanner.Text = Resources.Strings.Label_CourseScore + " : " + Resources.Strings.Label_Overall;
                TreeRefreshButton.Visible = false;
            }
            else if (GlobalSettings.Instance.InfoType == InfoType.Details)
            {
                RefreshDetailsTreeLists();
                infoBanner.Text = Resources.Strings.Label_CourseScore + " : " + CommonResources.Text.LabelDetails;
                TreeRefreshButton.BackgroundImage = CommonResources.Images.Refresh16;
                TreeRefreshButton.Visible = true;
            }
            else if (GlobalSettings.Instance.InfoType == InfoType.Features)
            {
                // NEW: Find existing features for this route
                RefreshDetailsTreeLists();
                infoBanner.Text = Resources.Strings.Label_CourseScore + " : " + CommonResources.Text.LabelSplits;
            }

            // Mode should be one activity
            mode = HillsDetailControl.Mode.OneActivity;
            EnableSingleModeIcons();

            // If it got through without activities, break
            if (activities == null)
            {
                ClearPage();
                return;
            }

            // We aren't currently zoomed in
            zoomed = false;

            // Iterate through all the activities selected
            foreach (IActivity act in activities)
            {
                // Check to see if the route needs to be redrawn
                //if (!layer.activityMapDrawn)
                //{
                //    layer.DrawRoute(activity);
                //}

                activity = act;
                List<Feature> all_features = CreateAllFeatures(act);
                List<Feature> hills = FindAllHills(act, all_features, GlobalSettings.Instance.ElevationPercent, GlobalSettings.Instance.DistancePercent, GlobalSettings.Instance.GainElevationRequired, GlobalSettings.Instance.HillDistanceRequired, GlobalSettings.Instance.MaxDescentLength, GlobalSettings.Instance.MaxDescentElevation, GlobalSettings.Instance.MinAvgGrade);

                //prog.UpdateProgress(33, string.Empty);
                Application.DoEvents();


                // TODO: Remove Overall from here.  I've left it for users that may have a 
                //if (GlobalSettings.Instance.ChartType == HillChartType.Overall)
                //{
                //    BuildAndDrawHillsForOverall(act, hills, autozoom);
                //    ChartBanner.Text = Resources.Strings.Label_Overall;
                //}
                if (GlobalSettings.Instance.ChartType == HillChartType.ClimbDistance
                        || GlobalSettings.Instance.ChartType == HillChartType.Overall)
                {
                    BuildAndDrawHills(act, hills, autozoom);
                    ChartBanner.Text = Resources.Strings.Label_Climbs + " / " + CommonResources.Text.LabelDistance;
                }
                else if (GlobalSettings.Instance.ChartType == HillChartType.ClimbTime)
                {
                    BuildAndDrawHills(act, hills, autozoom);
                    ChartBanner.Text = Resources.Strings.Label_Climbs + " / " + CommonResources.Text.LabelTime;
                }
                else if (GlobalSettings.Instance.ChartType == HillChartType.DescentDistance)
                {
                    List<Feature> downHills = new List<Feature>();

                    downHills = BuildAndDrawDownHills(act, autozoom);
                    ChartBanner.Text = Resources.Strings.Label_Descents + " / " + CommonResources.Text.LabelDistance;
                }
                else if (GlobalSettings.Instance.ChartType == HillChartType.DescentTime)
                {
                    List<Feature> downHills = new List<Feature>();

                    downHills = BuildAndDrawDownHills(act, autozoom);
                    ChartBanner.Text = Resources.Strings.Label_Descents + " / " + CommonResources.Text.LabelTime;
                }
                else if (GlobalSettings.Instance.ChartType == HillChartType.SplitsDistance)
                {
                    ChartBanner.Text = CommonResources.Text.LabelSplits + " / " + CommonResources.Text.LabelDistance;

                    // If we are looking for matching features
                    if (GlobalSettings.Instance.InfoType == InfoType.Features)
                    {
                        // Find all splits for this category
                        List<MatchingFeature> allSplits = FindAllSplits();

                        // Remove duplicates
                        //allSplits = RemoveDuplicateSplits(allSplits);

                        // Find matches
                        List<Feature> foundSplits = GetSplitsForActivity(activity, allSplits);

                        // Clear the chart
                        MainChart.DataSeries.Clear();

                        // Color the features based hard/steep/long/etc
                        ColorFeatures(foundSplits);

                        // Set the split numbers
                        SetHillNumbers(foundSplits);

                        // Draw the extra data items on the chart
                        List<IActivity> actList = new List<IActivity>();
                        actList.Add(act);
                        DrawChartSelectedRecordsByActivity(actList, autozoom);

                        // Draw the tree
                        RefreshTree(foundSplits);

                        ClearMap();

                        // Draw the map
                        // DrawMap(foundSplits, true);

                        // Draw the chart
                        DrawElevationProfile(foundSplits, autozoom);
                    }
                    else
                    {
                        List<Feature> splits = new List<Feature>();

                        splits = BuildAndDrawSplits(act, autozoom);
                    }
                }
                else if (GlobalSettings.Instance.ChartType == HillChartType.SplitsTime)
                {
                    List<Feature> splits = new List<Feature>();

                    splits = BuildAndDrawSplits(act, autozoom);
                    ChartBanner.Text = CommonResources.Text.LabelSplits + " / " + CommonResources.Text.LabelTime;
                }

                if (GlobalSettings.Instance.ColorType == ColorType.Basic)
                {
                    ChartBanner.Text += ": " + Resources.Strings.Label_Basic;
                }
                else if (GlobalSettings.Instance.ColorType == ColorType.Hardest)
                {
                    ChartBanner.Text += ": " + Resources.Strings.Label_Hardest;
                }
                else if (GlobalSettings.Instance.ColorType == ColorType.Longest)
                {
                    ChartBanner.Text += ": " + Resources.Strings.Label_Longest;
                }
                else if (GlobalSettings.Instance.ColorType == ColorType.Steepest)
                {
                    ChartBanner.Text += ": " + Resources.Strings.Label_Steepest;
                }

                //prog.UpdateProgress(66, string.Empty);
                Application.DoEvents();

                // Populate the overall data screen no matter where you are
                PopulateOverallData(activity, hills);

                //prog.UpdateProgress(100, string.Empty);
                Application.DoEvents();

                // Right now, only parse the first object in the list.  Remove the break to do all
                break;
            }

            //prog.Dispose();
            Application.DoEvents();

        }

        /// <summary>
        /// Clear activity related data from control
        /// </summary>
        public void ClearPage()
        {
            // Clear display
            textBox_HC_distance.Text = string.Empty;
            textBox_HC_score.Text = string.Empty;
            textBox_HC_grade.Text = string.Empty;
            textBox_HC_elevationGain.Text = string.Empty;
            textBox_LC_distance.Text = string.Empty;
            textBox_LC_score.Text = string.Empty;
            textBox_LC_grade.Text = string.Empty;
            textBox_LC_elevationGain.Text = string.Empty;
            textBox_MID_distance.Text = string.Empty;
            textBox_MID_score.Text = string.Empty;
            textBox_MID_grade.Text = string.Empty;
            textBox_MID_elevationGain.Text = string.Empty;
            textBox_climb.Text = string.Empty;
            textBox_courseScore.Text = string.Empty;
            textBox_distance.Text = string.Empty;
            textBox_scoreDistance.Text = string.Empty;

            treeList1.RowData = null;

            MainChart.DataSeries.Clear();
            MainChart.Refresh();
        }

        /// <summary>
        /// BuildAndDrawHillsForOverall will find the extreme hills to display on the Overall view
        /// </summary>
        /// <param name="act">Activity that the hills belong to</param>
        /// <param name="hills">List of feature</param>
        ///// <param name="autozoom">Autozoom the chart?</param>
        //public void BuildAndDrawHillsForOverall(IActivity act, List<Feature> hills, bool autozoom)
        //{
        //    // Clear the chart
        //    MainChart.DataSeries.Clear();

        //    List<Feature> frontPageHills = new List<Feature>();


        //    if (hills.Count != 0)
        //    {
        //        frontPageHills.Add(GetSteepest(hills, true));
        //        frontPageHills.Add(GetLongest(hills, true));
        //        frontPageHills.Add(GetHardest(hills, true));
        //    }

        //    // Draw the chart
        //    DrawElevationProfile(frontPageHills, autozoom);

        //    // Draw the extra data items on the chart
        //    List<IActivity> actList = new List<IActivity>();
        //    actList.Add(act);
        //    DrawChartSelectedRecordsByActivity(actList, autozoom);

        //    // Draw the tree
        //    RefreshTree(hills);

        //    // Draw the map
        //    DrawMap(frontPageHills, true);
        //}

        /// <summary>
        /// BuildAndDrawHills will color this hills based on the 'extreme' selection
        /// </summary>
        /// <param name="act">Activity that the hills belong to</param>
        /// <param name="hills">List of features</param>
        /// <param name="autozoom">Autozoom the chart?</param>
        public void BuildAndDrawHills(IActivity act, List<Feature> hills, bool autozoom)
        {
            int minAlpha = 5;
            int maxAlpha = 255 - minAlpha;

            // Clear the chart
            MainChart.DataSeries.Clear();

            // Color the features based hard/steep/long/etc
            ColorFeatures(hills);

            // Set the hill numbers (4/5/2011)
            SetHillNumbers(hills);

            if (GlobalSettings.Instance.InfoType == InfoType.Details
                || treeList1.RowData == null)
            {
                // Draw the tree
                RefreshTree(hills);
            }

            // Draw the chart
            DrawElevationProfile(hills, autozoom);

            // Draw the extra data items on the chart
            List<IActivity> actList = new List<IActivity>();
            actList.Add(act);
            DrawChartSelectedRecordsByActivity(actList, autozoom);

            // Draw the map
            DrawMap(hills, true);
        }

        /// <summary>
        /// BuildAndDrawDownHills will build the downhills for this activity and return them in a list
        /// </summary>
        /// <param name="act">Activity to parse for downhills</param>
        /// <param name="autozoom">Autozoom the chart?</param>
        /// <returns>Returns a list of downhill features</returns>
        public List<Feature> BuildAndDrawDownHills(IActivity act, bool autozoom)
        {
            // Clear the chart
            MainChart.DataSeries.Clear();

            // Init the variables
            List<Feature> all_features = new List<Feature>();
            List<Feature> downHills = new List<Feature>();

            // Create all features
            all_features = CreateAllFeatures(act);

            // Use the all features list to find the hills
            downHills = FindAllDownHills(act, all_features, GlobalSettings.Instance.ElevationPercent, GlobalSettings.Instance.DistancePercent, GlobalSettings.Instance.GainElevationRequired, GlobalSettings.Instance.HillDistanceRequired, GlobalSettings.Instance.MaxDescentLength, GlobalSettings.Instance.MaxDescentElevation, GlobalSettings.Instance.MinAvgGrade);

            // Set the hill numbers (4/5/2011)
            SetHillNumbers(downHills);

            // Draw the extra data items on the chart
            List<IActivity> actList = new List<IActivity>();
            actList.Add(act);
            DrawChartSelectedRecordsByActivity(actList, autozoom);

            // Draw the tree
            RefreshTree(downHills);

            // Draw the map
            DrawMap(downHills, true);

            // Draw the chart
            DrawElevationProfile(downHills, autozoom);

            return downHills;
        }

        /// <summary>
        /// SetupMenuItems is called to setup the toolstrips for the chartbanner
        /// </summary>
        private void SetupMenuItems()
        {
            ThemedContextMenuStripRenderer themedSTMenu = new ThemedContextMenuStripRenderer(PluginMain.GetApplication().VisualTheme);

            SetupDetailMenuSingleActivity();

            infoMenu.Items.Clear();

            // Setup the overall toolstrip
            ToolStripMenuItem item1 = new ToolStripMenuItem(Resources.Strings.Label_Overall, null, infoMenu_Click);
            item1.Tag = InfoType.Overall;
            infoMenu.Renderer = themedSTMenu;
            infoMenu.Items.Add(item1);

            // Setup the detail toolstrip
            ToolStripMenuItem item2 = new ToolStripMenuItem(CommonResources.Text.LabelDetails, null, infoMenu_Click);
            item2.Tag = InfoType.Details;
            infoMenu.Items.Add(item2);

            // NEW: Find existing features for this route
            // Setup the features toolstrip
            ToolStripMenuItem item3 = new ToolStripMenuItem(CommonResources.Text.LabelSplits, null, infoMenu_Click);
            item3.Tag = InfoType.Features;
            infoMenu.Items.Add(item3);

            // Place a check next to the current selection
            if (GlobalSettings.Instance.InfoType == InfoType.Overall)
            {
                item1.Checked = true;
            }
            else if (GlobalSettings.Instance.InfoType == InfoType.Details)
            {
                item2.Checked = true;
            }
            else if (GlobalSettings.Instance.InfoType == InfoType.Features)
            {
                item3.Checked = true;
            }

            scoreMenu.Items.Clear();

            // Setup the overall toolstrip
            item1 = new ToolStripMenuItem(Resources.Strings.Label_Cycling, null, scoreMenu_Click);
            item1.Tag = ScoreType.Cycling;
            scoreMenu.Renderer = themedSTMenu;
            scoreMenu.Items.Add(item1);

            // Setup the detail toolstrip
            item2 = new ToolStripMenuItem(Resources.Strings.Label_Running, null, scoreMenu_Click);
            item2.Tag = ScoreType.Running;
            scoreMenu.Items.Add(item2);

            // Place a check next to the current selection
            if (GlobalSettings.Instance.ScoreType == ScoreType.Cycling)
            {
                item1.Checked = true;
            }
            else if (GlobalSettings.Instance.ScoreType == ScoreType.Running)
            {
                item2.Checked = true;
            }

            // Setup the color toolstrip
            item1 = new ToolStripMenuItem(Resources.Strings.Label_Basic, null, colorMenu_Click);
            item1.Tag = ColorType.Basic;
            colorMenu.Renderer = themedSTMenu;
            colorMenu.Items.Add(item1);

            item2 = new ToolStripMenuItem(Resources.Strings.Label_Hardest, null, colorMenu_Click);
            item2.Tag = ColorType.Hardest;
            colorMenu.Items.Add(item2);

            item3 = new ToolStripMenuItem(Resources.Strings.Label_Longest, null, colorMenu_Click);
            item3.Tag = ColorType.Longest;
            colorMenu.Items.Add(item3);

            ToolStripMenuItem item4 = new ToolStripMenuItem(Resources.Strings.Label_Steepest, null, colorMenu_Click);
            item4.Tag = ColorType.Steepest;
            colorMenu.Items.Add(item4);

            // Place a check next to the current selection
            if (GlobalSettings.Instance.ColorType == ColorType.Basic)
            {
                item1.Checked = true;
            }
            else if (GlobalSettings.Instance.ColorType == ColorType.Hardest)
            {
                item2.Checked = true;
            }
            else if (GlobalSettings.Instance.ColorType == ColorType.Longest)
            {
                item3.Checked = true;
            }
            else if (GlobalSettings.Instance.ColorType == ColorType.Steepest)
            {
                item4.Checked = true;
            }

            contextMenuStrip.Items.Clear();

            // Setup the clear toolstrip
            item1 = new ToolStripMenuItem(Resources.Strings.Label_Clear, null, contextMenuStrip_Click);
            item1.Tag = RightClickMenu.Clear;
            contextMenuStrip.Renderer = themedSTMenu;
            contextMenuStrip.Items.Add(item1);

            // Setup the Exact Hill toolstrip
            item2 = new ToolStripMenuItem(Resources.Strings.Label_FindThisHillInAllActivites, null, contextMenuStrip_Click);
            item2.Tag = RightClickMenu.ExactHill;
            contextMenuStrip.Items.Add(item2);

            // Setup the Like Hill toolstrip
            item2 = new ToolStripMenuItem(Resources.Strings.Label_FindHillsLikeThis, null, contextMenuStrip_Click);
            item2.Tag = RightClickMenu.LikeHill;
            contextMenuStrip.Items.Add(item2);

            // Setup the splits right click menu
            splitsContextMenuStrip.Items.Clear();

            // Setup the clear toolstrip
            item1 = new ToolStripMenuItem(Resources.Strings.Label_Clear, null, splitsContextMenuStrip_Click);
            item1.Tag = RightClickMenuSplits.Clear;
            splitsContextMenuStrip.Renderer = themedSTMenu;
            splitsContextMenuStrip.Items.Add(item1);

            // Setup the Exact Hill toolstrip
            item2 = new ToolStripMenuItem(Resources.Strings.Label_FindThisSplitInAllActivities, null, splitsContextMenuStrip_Click);
            item2.Tag = RightClickMenuSplits.ExactSplit;
            splitsContextMenuStrip.Items.Add(item2);

            // Setup the Like Hill toolstrip
            item2 = new ToolStripMenuItem(Resources.Strings.Label_FindRoutesLikeThis, null, splitsContextMenuStrip_Click);
            item2.Tag = RightClickMenuSplits.Route;
            splitsContextMenuStrip.Items.Add(item2);

            treeMenu.Items.Clear();
            item1 = new ToolStripMenuItem(Resources.Strings.Label_ListSettings, null, TreeListMenu_Click);
            item1.Tag = TreeListRightClickMenu.ListSettings;
            treeMenu.Items.Add(item1);
        }

        /// <summary>
        /// Sets up the detail menu to show climbs, descents, splits
        /// </summary>
        public void SetupDetailMenuSingleActivity()
        {
            ThemedContextMenuStripRenderer themedSTMenu = new ThemedContextMenuStripRenderer(PluginMain.GetApplication().VisualTheme);
            detailMenu.Renderer = themedSTMenu;
            detailMenu.Items.Clear();

            // Setup the Climb distance toolstrip
            ToolStripMenuItem item1 = new ToolStripMenuItem(CommonResources.Text.LabelClimb
                                      + " / " + CommonResources.Text.LabelDistance, null, detailMenu_Click);
            item1.Tag = HillChartType.ClimbDistance;
            detailMenu.Items.Add(item1);

            // Setup the Climb time toolstrip
            ToolStripMenuItem item2 = new ToolStripMenuItem(CommonResources.Text.LabelClimb
                                      + " / " + CommonResources.Text.LabelTime, null, detailMenu_Click);
            item2.Tag = HillChartType.ClimbTime;
            detailMenu.Items.Add(item2);

            // Setup the Descent distance toolstrip
            ToolStripMenuItem item3 = new ToolStripMenuItem(Resources.Strings.Label_Descents
                                      + " / " + CommonResources.Text.LabelDistance, null, detailMenu_Click);
            item3.Tag = HillChartType.DescentDistance;
            detailMenu.Items.Add(item3);

            // Setup the Descent time toolstrip
            ToolStripMenuItem item4 = new ToolStripMenuItem(Resources.Strings.Label_Descents
                                      + " / " + CommonResources.Text.LabelTime, null, detailMenu_Click);
            item4.Tag = HillChartType.DescentTime;
            detailMenu.Items.Add(item4);

            // TODO: comment to deactivate the splits view
            // Setup the Splits Distance toolstrip
            ToolStripMenuItem item5 = new ToolStripMenuItem(CommonResources.Text.LabelSplits
                                      + " / " + CommonResources.Text.LabelDistance, null, detailMenu_Click);
            item5.Tag = HillChartType.SplitsDistance;
            detailMenu.Items.Add(item5);

            // Setup the Splits Time toolstrip
            ToolStripMenuItem item6 = new ToolStripMenuItem(CommonResources.Text.LabelSplits
                                      + " / " + CommonResources.Text.LabelTime, null, detailMenu_Click);
            item6.Tag = HillChartType.SplitsTime;
            detailMenu.Items.Add(item6);

            if (GlobalSettings.Instance.ChartType == HillChartType.ClimbDistance)
            {
                item1.Checked = true;
            }
            else if (GlobalSettings.Instance.ChartType == HillChartType.ClimbTime)
            {
                item2.Checked = true;
            }
            else if (GlobalSettings.Instance.ChartType == HillChartType.DescentDistance)
            {
                item3.Checked = true;
            }
            else if (GlobalSettings.Instance.ChartType == HillChartType.DescentTime)
            {
                item4.Checked = true;
            }

            // TODO: comment to deactivate the splits view
            else if (GlobalSettings.Instance.ChartType == HillChartType.SplitsDistance)
            {
                item5.Checked = true;
            }
            else if (GlobalSettings.Instance.ChartType == HillChartType.SplitsTime)
            {
                item6.Checked = true;
            }
        }

        /// <summary>
        /// Sets up the detail menu to show summary, details for multplie activities
        /// </summary>
        public void SetupDetailMenuMultipleActivities()
        {
            ThemedContextMenuStripRenderer themedSTMenu = new ThemedContextMenuStripRenderer(PluginMain.GetApplication().VisualTheme);
            detailMenu.Renderer = themedSTMenu;
            detailMenu.Items.Clear();

            // Setup the Climb distance toolstrip
            ToolStripMenuItem item1 = new ToolStripMenuItem(CommonResources.Text.LabelDetails, null, detailMenuMultipleActivities_Click);
            item1.Tag = HillChartTypeMultiMode.Details;
            detailMenu.Items.Add(item1);

            detailMenu.Items.Add(new ToolStripSeparator());

            // Setup the Climb time toolstrip
            /*ToolStripMenuItem item2 = new ToolStripMenuItem(Resources.Strings.Label_Summary, null, detailMenuMultipleActivities_Click);
            item2.Tag = HillChartTypeMultiMode.Summary;
            detailMenu.Items.Add(item2);*/

            ToolStripMenuItem item2 = new ToolStripMenuItem(ChartField.ChartFieldsLookup(ChartField.Field.Cadence), null, detailMenuMultipleActivities_Click);
            item2.Tag = HillChartTypeMultiMode.Cadence;
            detailMenu.Items.Add(item2);

            ToolStripMenuItem item3 = new ToolStripMenuItem(ChartField.ChartFieldsLookup(ChartField.Field.Elevation), null, detailMenuMultipleActivities_Click);
            item3.Tag = HillChartTypeMultiMode.Elevation;
            detailMenu.Items.Add(item3);

            ToolStripMenuItem item4 = new ToolStripMenuItem(ChartField.ChartFieldsLookup(ChartField.Field.Grade), null, detailMenuMultipleActivities_Click);
            item4.Tag = HillChartTypeMultiMode.Grade;
            detailMenu.Items.Add(item4);

            ToolStripMenuItem item5 = new ToolStripMenuItem(ChartField.ChartFieldsLookup(ChartField.Field.HR), null, detailMenuMultipleActivities_Click);
            item5.Tag = HillChartTypeMultiMode.HR;
            detailMenu.Items.Add(item5);

            ToolStripMenuItem item6 = new ToolStripMenuItem(ChartField.ChartFieldsLookup(ChartField.Field.Power), null, detailMenuMultipleActivities_Click);
            item6.Tag = HillChartTypeMultiMode.Power;
            detailMenu.Items.Add(item6);

            ToolStripMenuItem item7 = new ToolStripMenuItem(ChartField.ChartFieldsLookup(ChartField.Field.Speed), null, detailMenuMultipleActivities_Click);
            item7.Tag = HillChartTypeMultiMode.Speed;
            detailMenu.Items.Add(item7);

            ToolStripMenuItem item8 = new ToolStripMenuItem(ChartField.ChartFieldsLookup(ChartField.Field.VAM), null, detailMenuMultipleActivities_Click);
            item8.Tag = HillChartTypeMultiMode.VAM;
            detailMenu.Items.Add(item8);

            if (GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.Details)
            {
                item1.Checked = true;
            }
            else if (GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.Cadence)
            {
                item2.Checked = true;
            }
            else if (GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.Elevation)
            {
                item3.Checked = true;
            }
            else if (GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.Grade)
            {
                item4.Checked = true;
            }
            else if (GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.HR)
            {
                item5.Checked = true;
            }
            else if (GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.Power)
            {
                item6.Checked = true;
            }
            else if (GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.Speed)
            {
                item7.Checked = true;
            }
            else if (GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.VAM)
            {
                item8.Checked = true;
            }
        }

        /// <summary>
        /// HideMapLayer will remove the overlays from the map
        /// </summary>
        public void HideMapLayer()
        {
            layer.ShowPage = false;
            layer.ClearOverlays();
        }

        /// <summary>
        /// PopulateOverallData will fill in the Overall textboxes 
        /// </summary>
        /// <param name="inActivity">This activity</param>
        /// <param name="hills">List of features to parse for extreme hills</param>
        private void PopulateOverallData(IActivity inActivity, List<Feature> hills)
        {
            // Ascend
            ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
            float totalAscend = (float)info.TotalAscendingMeters(PluginMain.GetApplication().Logbook.ClimbZones[0]);

            if (totalAscend == 0 || float.IsNaN(totalAscend))
            {
                totalAscend = info.Activity.TotalAscendMetersEntered;
            }

            totalAscend = (float)Length.Convert(totalAscend, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);

            // Descend
            float totalDescend = (float)info.TotalDescendingMeters(PluginMain.GetApplication().Logbook.ClimbZones[0]);

            if (totalDescend == 0 || float.IsNaN(totalDescend))
            {
                totalDescend = info.Activity.TotalDescendMetersEntered;
            }

            totalDescend = (float)Length.Convert(totalDescend, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);

            // Climb
            textBox_climb.Text = "+" + Math.Round(totalAscend, 0) + " / " + Math.Round(totalDescend, 0);

            // Distance
            textBox_distance.Text = Length.Convert(info.DistanceMeters, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits).ToString("0.00", CultureInfo.CurrentCulture);
            textBox_distance.Text += " " + Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.DistanceUnits);

            // Find the longest and hardest hills
            Feature steepest = null;
            Feature longest = null;
            Feature hardest = null;

            double hardestScore = 0;
            double longestScore = 0;
            double steepestScore = 0;

            if (hills.Count != 0)
            {
                steepest = GetSteepest(hills, false);
                longest = GetLongest(hills, false);
                hardest = GetHardest(hills, false);

                if (GlobalSettings.Instance.ScoreEquation == ScoreEquation.Score.ClimbByBike)
                {
                    hardestScore = hardest.hillScoreClimbByBike;
                    longestScore = longest.hillScoreClimbByBike;
                    steepestScore = steepest.hillScoreClimbByBike;
                }
                else if (GlobalSettings.Instance.ScoreEquation == ScoreEquation.Score.Cycle2Max)
                {
                    hardestScore = hardest.hillScoreCycle2Max;
                    longestScore = longest.hillScoreCycle2Max;
                    steepestScore = steepest.hillScoreCycle2Max;
                }
                else if (GlobalSettings.Instance.ScoreEquation == ScoreEquation.Score.Fiets)
                {
                    hardestScore = hardest.hillScoreFiets;
                    longestScore = longest.hillScoreFiets;
                    steepestScore = steepest.hillScoreFiets;
                }
                else if (GlobalSettings.Instance.ScoreEquation == ScoreEquation.Score.CourseScoreCycling)
                {
                    hardestScore = hardest.hillScoreCourseScoreCycling;
                    longestScore = longest.hillScoreCourseScoreCycling;
                    steepestScore = steepest.hillScoreCourseScoreCycling;
                }
                else if (GlobalSettings.Instance.ScoreEquation == ScoreEquation.Score.CourseScoreRunning)
                {
                    hardestScore = hardest.hillScoreCourseScoreRunning;
                    longestScore = longest.hillScoreCourseScoreRunning;
                    steepestScore = steepest.hillScoreCourseScoreRunning;
                }
            }

            // Format and display the longest and hardest
            if (hardest != null)
            {
                textBox_HC_distance.Text = Length.Convert(hardest.distance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits).ToString("0.00", CultureInfo.CurrentCulture);
                textBox_HC_distance.Text += " " + Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.DistanceUnits);
                textBox_HC_score.Text = hardestScore.ToString("0.00", CultureInfo.CurrentCulture);
                textBox_HC_grade.Text = (hardest.avgGrade * 100).ToString("0.0", CultureInfo.CurrentCulture) + "%";
                textBox_HC_elevationGain.Text = Length.Convert(hardest.elevGain, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits).ToString("0.", CultureInfo.CurrentCulture);
                textBox_HC_elevationGain.Text += " " + Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.ElevationUnits);
            }
            else
            {
                textBox_HC_distance.Text = string.Empty;
                textBox_HC_score.Text = string.Empty;
                textBox_HC_grade.Text = string.Empty;
                textBox_HC_elevationGain.Text = string.Empty;
            }

            if (longest != null)
            {
                textBox_LC_distance.Text = Length.Convert(longest.distance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits).ToString("0.00", CultureInfo.CurrentCulture);
                textBox_LC_distance.Text += " " + Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.DistanceUnits);
                textBox_LC_score.Text = longestScore.ToString("0.00", CultureInfo.CurrentCulture);
                textBox_LC_grade.Text = (longest.avgGrade * 100).ToString("0.0", CultureInfo.CurrentCulture) + "%";
                textBox_LC_elevationGain.Text = Length.Convert(longest.elevGain, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits).ToString("0.", CultureInfo.CurrentCulture);
                textBox_LC_elevationGain.Text += " " + Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.ElevationUnits);
            }
            else
            {
                textBox_LC_distance.Text = string.Empty;
                textBox_LC_score.Text = string.Empty;
                textBox_LC_grade.Text = string.Empty;
                textBox_LC_elevationGain.Text = string.Empty;
            }

            if (steepest != null)
            {
                textBox_MID_distance.Text = Length.Convert(steepest.distance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits).ToString("0.00", CultureInfo.CurrentCulture);
                textBox_MID_distance.Text += " " + Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.DistanceUnits);
                textBox_MID_score.Text = steepestScore.ToString("0.00", CultureInfo.CurrentCulture);
                textBox_MID_grade.Text = (steepest.avgGrade * 100).ToString("0.0", CultureInfo.CurrentCulture) + "%";
                textBox_MID_elevationGain.Text = Length.Convert(steepest.elevGain, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits).ToString("0.", CultureInfo.CurrentCulture);
                textBox_MID_elevationGain.Text += " " + Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.ElevationUnits);
            }
            else
            {
                textBox_MID_distance.Text = string.Empty;
                textBox_MID_score.Text = string.Empty;
                textBox_MID_grade.Text = string.Empty;
                textBox_MID_elevationGain.Text = string.Empty;
            }

            // Pre-calculate the course scores
            PopulateCyclingScores(inActivity);
            PopulateRunningScores(inActivity);

            // Only show the correct course score on screen
            if (GlobalSettings.Instance.ScoreType == ScoreType.Cycling)
            {
                label_courseScore.Text = Resources.Strings.Label_CourseScore + " (" + Resources.Strings.Label_Cycling + ")";
                PopulateCyclingScores(inActivity);
            }
            else if (GlobalSettings.Instance.ScoreType == ScoreType.Running)
            {
                label_courseScore.Text = Resources.Strings.Label_CourseScore + " (" + Resources.Strings.Label_Running + ")";
                PopulateRunningScores(inActivity);
            }

        }

        public void RedrawGraph()
        {

            if (mode == Mode.OneActivity)
            {
                // If we double clicked and zoomed, we only want to redraw this hill
                if (zoomed)
                {
                    Feature feature = null;
                    foreach (object o in treeList1.Selected)
                    {
                        feature = ((Feature)o);
                        break;
                    }

                    // If the feature is no longer selected in the tree, get it from the only drawn hill
                    if (feature == null)
                    {
                        List<Feature> rowFeatures = treeList1.RowData as List<Feature>;
                        foreach (Feature f in rowFeatures)
                        {
                            if (f.record.Activity.ReferenceId == (string)MainChart.DataSeries[0].Data)
                            {
                                feature = f;
                            }
                        }
                    }
                    MainChart.DataSeries.Clear();

                    DrawElevationProfile(feature, false);

                    List<IActivity> act = new List<IActivity>();
                    act.Add(feature.record.Activity);

                    DrawChartSelectedRecordsByActivity(act, true);
                }
                // Else we haven't zoomed so draw the whole profile with the all hills highlighted
                else
                {
                    List<Feature> features = new List<Feature>();

                    // Old code to only draw the selected hills.
                    //if (treeList1.Selected.Count > 0)
                    //{
                    //    foreach (object o in treeList1.Selected)
                    //    {
                    //        features.Add((Feature)o);
                    //    }
                    //}
                    //else
                    //{
                    features = (List<Feature>)treeList1.RowData;
                    //}
                    MainChart.DataSeries.Clear();

                    DrawElevationProfile(features, false);

                    List<IActivity> act = new List<IActivity>();
                    act.Add(activity);
                    DrawChartSelectedRecordsByActivity(act, true);
                }
            }
            else if (mode == Mode.MultipleActivities)
            {
                List<Feature> features = treeList1.RowData as List<Feature>;

                // TODO: Update chart based on selected feature.
                MainChart.DataSeries.Clear();

                DrawChartSelectedRecordsByFeature(features, true);
            }
        }

        public void RedrawMap(List<Feature> mustDrawFeatures)
        {
            if (mode == Mode.OneActivity)
            {
                // Check to see if the route needs to be redrawn
                if (!layer.activityMapDrawn)
                {
                    layer.DrawRoute(activity);
                }

                // If mustDrawFeatures is null, draw only what's selected
                if (mustDrawFeatures == null)
                {
                    List<Feature> selectedFeatures = new List<Feature>();
                    foreach (object o in treeList1.Selected)
                    {
                        Feature selected = (Feature)o;
                        selected.routeWidth = PluginMain.GetApplication().SystemPreferences.RouteSettings.RouteWidth * 2;
                        selectedFeatures.Add(selected);
                    }

                    if (selectedFeatures.Count == 1)
                    {
                        DrawMap(selectedFeatures[0], false);
                    }
                    else
                    {
                        DrawMap(selectedFeatures, false);
                    }
                }

                // mustDrawFeatures has data, draw those features
                else
                {
                    if (mustDrawFeatures.Count == 1)
                    {
                        DrawMap(mustDrawFeatures[0], false);
                    }
                    else
                    {
                        DrawMap(mustDrawFeatures, false);
                    }
                }
            }
            else if (mode == Mode.MultipleActivities)
            {
                // If mustDrawFeatures is null, draw only what's selected
                if (mustDrawFeatures == null)
                {
                    List<Feature> features = new List<Feature>();
                    foreach (object o in treeList1.Selected)
                    {
                        features.Add((Feature)o);
                    }

                    if (features.Count == 1)
                    {
                        DrawMap(features[0], true);
                    }
                    else
                    {
                        DrawMap(features, false);
                    }
                }

                // mustDrawFeatures has data, draw those features
                else
                {
                    if (mustDrawFeatures.Count == 1)
                    {
                        DrawMap(mustDrawFeatures[0], true);
                    }
                    else
                    {
                        DrawMap(mustDrawFeatures, false);
                    }
                }
            }
        }

        public void HideUnselectedHills(TreeList inTreeList)
        {
            List<Feature> selectedFeatures = GetSelectedFeatures(inTreeList);
            if (selectedFeatures != null)
            {
                if (mode == Mode.OneActivity)
                {
                    // Single Mode - Hide inactive features by setting alpha to 0
                    foreach (ChartDataSeries series in MainChart.DataSeries)
                    {
                        bool found = false;
                        string id = series.Data as string;

                        foreach (Feature feature in selectedFeatures)
                        {
                            if (!string.IsNullOrEmpty(id) && id == "HILL" + feature.refId)
                            {
                                // Set colors to proper highlight colors
                                series.LineColor = feature.lineColor;
                                series.FillColor = Color.FromArgb(255, feature.fillColor);
                                found = true;
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(id) && !found && id.Substring(0, 4) == "HILL")
                        {
                            // Feature was not selected... hide by setting alpha to 0
                            series.LineColor = Color.FromArgb(0, series.LineColor);
                            series.FillColor = Color.FromArgb(0, series.FillColor);
                        }
                    }
                }
                else if (GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.Details)
                {
                    // Multi-mode details - Highlight selected features
                    HighlightChartLines(selectedFeatures);
                }
                else
                {
                    int seriesNum = 0;

                    // Highlighted the bar for the selected feature
                    foreach (ChartDataSeries series in MainChart.DataSeries)
                    {
                        series.ClearSelectedRegions();
                        series.SetSelectedRange(float.NaN, float.NaN);

                        series.FillColor = Color.FromArgb(75, ChartColorLookup(GlobalSettings.Instance.HillChartTypeMultiMode));

                        foreach (Feature feature in selectedFeatures)
                        {
                            if (feature != null && feature.refId == series.Data as string)
                            {
                                // Set this bad as being selected
                                series.SetSelectedRange(seriesNum, float.NaN);
                                series.FillColor = feature.selectedColor;
                            }
                        }
                        seriesNum++;
                    }
                }
            }
            MainChart.Refresh();
        }

        #endregion

        #region Theme and Culture

        internal void ThemeChanged(ITheme visualTheme)
        {
            treeList_subtotals.ThemeChanged(visualTheme);
            treeList1.ThemeChanged(visualTheme);
            pnlMain.ThemeChanged(visualTheme);
            MainChart.ThemeChanged(visualTheme);
            panelMain.ThemeChanged(visualTheme);
            ButtonPanel.ThemeChanged(visualTheme);
            ChartBanner.ThemeChanged(visualTheme);
            infoBanner.ThemeChanged(visualTheme);
            label_climb.ForeColor = visualTheme.ControlText;
            label_courseScore.ForeColor = visualTheme.ControlText;
            label_distance.ForeColor = visualTheme.ControlText;
            label_hardestClimb.ForeColor = visualTheme.ControlText;
            label_LC_distance.ForeColor = visualTheme.ControlText;
            label_LC_elevationGain.ForeColor = visualTheme.ControlText;
            label_LC_grade.ForeColor = visualTheme.ControlText;
            label_LC_score.ForeColor = visualTheme.ControlText;
            label_longestClimb.ForeColor = visualTheme.ControlText;
            label_scoreDistance.ForeColor = visualTheme.ControlText;
            textBox_climb.ThemeChanged(visualTheme);
            textBox_courseScore.ThemeChanged(visualTheme);
            textBox_distance.ThemeChanged(visualTheme);

            textBox_HC_distance.ThemeChanged(visualTheme);
            textBox_HC_distance.BorderColor = Common.HardestColor;
            textBox_HC_elevationGain.ThemeChanged(visualTheme);
            textBox_HC_elevationGain.BorderColor = Common.HardestColor;
            textBox_HC_grade.ThemeChanged(visualTheme);
            textBox_HC_grade.BorderColor = Common.HardestColor;
            textBox_HC_score.ThemeChanged(visualTheme);
            textBox_HC_score.BorderColor = Common.HardestColor;

            textBox_LC_distance.ThemeChanged(visualTheme);
            textBox_LC_distance.BorderColor = Common.LongestColor;
            textBox_LC_elevationGain.ThemeChanged(visualTheme);
            textBox_LC_elevationGain.BorderColor = Common.LongestColor;
            textBox_LC_grade.ThemeChanged(visualTheme);
            textBox_LC_grade.BorderColor = Common.LongestColor;
            textBox_LC_score.ThemeChanged(visualTheme);
            textBox_LC_score.BorderColor = Common.LongestColor;

            textBox_MID_distance.ThemeChanged(visualTheme);
            textBox_MID_distance.BorderColor = Common.SteepestColor;
            textBox_MID_elevationGain.ThemeChanged(visualTheme);
            textBox_MID_elevationGain.BorderColor = Common.SteepestColor;
            textBox_MID_grade.ThemeChanged(visualTheme);
            textBox_MID_grade.BorderColor = Common.SteepestColor;
            textBox_MID_score.ThemeChanged(visualTheme);
            textBox_MID_score.BorderColor = Common.SteepestColor;

            textBox_scoreDistance.ThemeChanged(visualTheme);
            panel_climb.ThemeChanged(visualTheme);

            ThemedContextMenuStripRenderer themedSTMenu = new ThemedContextMenuStripRenderer(visualTheme);
            detailMenu.Renderer = themedSTMenu;
            infoMenu.Renderer = themedSTMenu;
            scoreMenu.Renderer = themedSTMenu;
            colorMenu.Renderer = themedSTMenu;
            contextMenuStrip.Renderer = themedSTMenu;
            treeMenu.Renderer = themedSTMenu;
        }

        // TOOD: Not sure how to use this
        internal void UICultureChanged(CultureInfo culture)
        {
            // The 2 lines below errors out the form builder due to the getApp portion.  Putting it here instead
            this.label_scoreDistance.Text = Resources.Strings.Label_Score + "/" + Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.DistanceUnits) + ":";

            // Setup the chart banner
            if (GlobalSettings.Instance.ChartType == HillChartType.ClimbDistance
                || GlobalSettings.Instance.ChartType == HillChartType.DescentDistance
                || GlobalSettings.Instance.ChartType == HillChartType.Overall)
            {
                if (activity == null)
                {
                    MainChart.XAxis.Label = CommonResources.Text.LabelDistance;
                }
                else
                {
                    MainChart.XAxis.Label = CommonResources.Text.LabelDistance + " (" + activity.Category.DistanceUnits + ")";
                }
            }
            else if (GlobalSettings.Instance.ChartType == HillChartType.ClimbTime
                || GlobalSettings.Instance.ChartType == HillChartType.DescentTime)
            {
                MainChart.XAxis.Label = CommonResources.Text.LabelTime;
            }

            MainChart.YAxis.Label = CommonResources.Text.LabelElevation;

            label_courseScore.Text = Resources.Strings.Label_CourseScore + " (" + Resources.Strings.Label_Cycling + ")";
            label_climb.Text = CommonResources.Text.LabelClimb;
            label_distance.Text = CommonResources.Text.LabelDistance;
            label_hardestClimb.Text = Resources.Strings.Label_HardestClimb;
            label_LC_distance.Text = CommonResources.Text.LabelDistance;
            label_LC_elevationGain.Text = CommonResources.Text.LabelElevationChange;
            label_LC_grade.Text = CommonResources.Text.LabelGrade;
            label_LC_score.Text = Resources.Strings.Label_Score;
            label_longestClimb.Text = Resources.Strings.Label_LongestClimb;
            label_steepestClimb.Text = Resources.Strings.Label_SteepestClimb;

            if (GlobalSettings.Instance.InfoType == InfoType.Overall)
            {
                infoBanner.Text = Resources.Strings.Label_CourseScore + " : " + Resources.Strings.Label_Overall;
            }
            else if (GlobalSettings.Instance.InfoType == InfoType.Details)
            {
                infoBanner.Text = Resources.Strings.Label_CourseScore + " : " + CommonResources.Text.LabelDetails;
            }
            else if (GlobalSettings.Instance.InfoType == InfoType.Features)
            {
                if (GlobalSettings.Instance.ChartType == HillChartType.ClimbDistance ||
                    GlobalSettings.Instance.ChartType == HillChartType.ClimbTime ||
                    GlobalSettings.Instance.ChartType == HillChartType.Overall)
                {
                    infoBanner.Text = Resources.Strings.Label_CourseScore + " : " + Resources.Strings.Label_Climbs;
                }
                else if (GlobalSettings.Instance.ChartType == HillChartType.DescentDistance ||
                    GlobalSettings.Instance.ChartType == HillChartType.DescentTime)
                {
                    infoBanner.Text = Resources.Strings.Label_CourseScore + " : " + Resources.Strings.Label_Descents;
                }
                else if (GlobalSettings.Instance.ChartType == HillChartType.SplitsDistance ||
                    GlobalSettings.Instance.ChartType == HillChartType.SplitsTime)
                {
                    infoBanner.Text = Resources.Strings.Label_CourseScore + " : " + CommonResources.Text.LabelSplits;
                }
            }

            if (GlobalSettings.Instance.ChartType == HillChartType.Overall)
            {
                ChartBanner.Text = Resources.Strings.Label_Overall;
            }
            else if (GlobalSettings.Instance.ChartType == HillChartType.ClimbDistance)
            {
                ChartBanner.Text = Resources.Strings.Label_Climbs + " / " + CommonResources.Text.LabelDistance;
            }
            else if (GlobalSettings.Instance.ChartType == HillChartType.ClimbTime)
            {
                ChartBanner.Text = Resources.Strings.Label_Climbs + " / " + CommonResources.Text.LabelTime;
            }
            else if (GlobalSettings.Instance.ChartType == HillChartType.DescentDistance)
            {
                ChartBanner.Text = Resources.Strings.Label_Descents + " / " + CommonResources.Text.LabelDistance;
            }
            else if (GlobalSettings.Instance.ChartType == HillChartType.DescentTime)
            {
                ChartBanner.Text = Resources.Strings.Label_Descents + " / " + CommonResources.Text.LabelTime;
            }

            if (GlobalSettings.Instance.ColorType == ColorType.Basic)
            {
                ChartBanner.Text += ": " + Resources.Strings.Label_Basic;
            }
            else if (GlobalSettings.Instance.ColorType == ColorType.Hardest)
            {
                ChartBanner.Text += ": " + Resources.Strings.Label_Hardest;
            }
            else if (GlobalSettings.Instance.ColorType == ColorType.Longest)
            {
                ChartBanner.Text += ": " + Resources.Strings.Label_Longest;
            }
            else if (GlobalSettings.Instance.ColorType == ColorType.Steepest)
            {
                ChartBanner.Text += ": " + Resources.Strings.Label_Steepest;
            }
            // TODO: Toolstrip menu items should be here also for localization, but it might be messier, and it'd only matter if you change languages in the middle of an ST session
        }

        #endregion

        #region Hill Finder Functions

        /// <summary>
        /// CreateAllFeatures will find all ascents and descents.  Only constant up or constant down
        /// </summary>
        /// <param name="activity">Activity to search</param>
        /// <returns>Returns a list of features from this activity</returns>
        public static List<Feature> CreateAllFeatures(IActivity activity)
        {
            //float min, max;
            List<Feature> features = new List<Feature>();
            ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(activity);
            INumericTimeDataSeries distanceTrack = ai.MovingDistanceMetersTrack;
            INumericTimeDataSeries smoothedElevation = ai.SmoothedElevationTrack; // Utilities.STSmooth(Utilities.GetElevationTrack(activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.ElevationSmoothingSeconds, out min, out max);


            if (smoothedElevation.Count != 0 && distanceTrack.Count != 0)
            {
                double start_distance = 0;
                double end_distance = 0;

                double start_elevation = smoothedElevation[0].Value;
                double end_elevation = smoothedElevation[0].Value;

                int start_i = 0;
                int end_i = 0;

                DateTime startTime = new DateTime();
                DateTime endTime = new DateTime();

                bool areWeClimbing = false;

                double this_elevation = smoothedElevation[0].Value;
                double last_elevation = smoothedElevation[0].Value;

                //Parse through all the smoothed elevation points
                for (int i = 0; i < smoothedElevation.Count; i++)
                {
                    this_elevation = smoothedElevation[i].Value;
                    // Find the start time of the track and use it to find the current grade and distance
                    DateTime start = smoothedElevation.StartTime;
                    // I can't use grade, don't try it again.  It's not correct.
                    //float grade = ai.SmoothedGradeTrack.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i].ElapsedSeconds)).Value;

                    double distance = 0;
                    try
                    {
                        // Try GetInterpolatedValue.  If it fails, distance will be 0
                        distance = (double)distanceTrack.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i].ElapsedSeconds)).Value;
                    }
                    catch
                    {
                    }
                    // If we are current going uphill and climbing
                    if ((this_elevation - last_elevation) > 0 && areWeClimbing == true)
                    {
                        end_i = i;
                        endTime = start.AddSeconds(smoothedElevation[i].ElapsedSeconds);
                        end_elevation = smoothedElevation[i].Value;
                        end_distance = distance;
                    }

                    // If we just switched from descending to climbing
                    else if ((this_elevation - last_elevation) > 0 && areWeClimbing == false)
                    {
                        // Make sure we aren't sending a blank record
                        if (start_i > end_i)
                        {
                            // Add the descent record (start and end will flipped due to descent)
                            features.Add(new Feature(activity, Feature.feature_type.descent, endTime, startTime));
                        }

                        // If this isn't the first record, step back 1
                        if (i != 0)
                        {
                            start_i = i - 1;
                            startTime = start.AddSeconds(smoothedElevation[i - 1].ElapsedSeconds);
                            start_elevation = smoothedElevation[i - 1].Value;
                            start_distance = 0;
                            try
                            {
                                //start_distance = (double)ai.MovingDistanceMetersTrack.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i - 1].ElapsedSeconds)).Value;
                                start_distance = (double)distanceTrack.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i - 1].ElapsedSeconds)).Value;
                            }
                            catch
                            {
                            }
                        }

                        // If this is the first record, we can't step back
                        else
                        {
                            start_i = i;
                            startTime = start.AddSeconds(smoothedElevation[i].ElapsedSeconds);
                            start_elevation = smoothedElevation[i].Value;
                            start_distance = distance;
                        }
                        areWeClimbing = true;
                        i--;
                    }

                    // If we just switched from climbing to descending
                    else if ((this_elevation - last_elevation) <= 0 && areWeClimbing == true)
                    {
                        // Make sure we aren't sending a blank track
                        if (start_i < end_i)
                        {
                            // Add the ascent record 
                            features.Add(new Feature(activity, Feature.feature_type.ascent, startTime, endTime));
                        }

                        // The start point is now the old end.  Gotta rewind 1 step
                        start_i = end_i;
                        startTime = endTime;
                        start_elevation = end_elevation;
                        start_distance = end_distance;
                        areWeClimbing = false;
                        i--;
                    }

                    // If we are currently descending and going downhill
                    else if ((this_elevation - last_elevation) <= 0 && areWeClimbing == false)
                    {
                        start_i = i;
                        startTime = start.AddSeconds(smoothedElevation[i].ElapsedSeconds);
                        start_elevation = smoothedElevation[i].Value;
                        start_distance = distance;
                    }

                    // If we are on the last point, check it to see if it is the end of a climb
                    if (i == smoothedElevation.Count - 1)
                    {
                        // If we ended on a climb, add it
                        if (areWeClimbing == true)
                        {
                            if (start_i < end_i)
                            {
                                features.Add(new Feature(activity, Feature.feature_type.ascent, startTime, endTime));
                            }
                        }

                        // If we ended on a descent, add it
                        else if (areWeClimbing == false)
                        {
                            if (start_i > end_i)
                            {
                                features.Add(new Feature(activity, Feature.feature_type.descent, endTime, startTime));
                            }
                        }
                    }
                    last_elevation = smoothedElevation[i].Value;
                }
            }
            return features;
        }

        /// <summary>
        /// CreateAllFeatures will find all ascents and descents.  Only constant up or constant down
        /// </summary>
        /// <param name="activity">Activity to search</param>
        /// <returns>Returns a list of features from this activity</returns>
        //public static List<Feature> CreateAllFeatures(IActivity activity)
        //{
        //    //float min, max;
        //    List<Feature> features = new List<Feature>();
        //    ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(activity);
        //    INumericTimeDataSeries distanceTrack = ai.MovingDistanceMetersTrack;
        //    INumericTimeDataSeries smoothedElevation = ai.SmoothedElevationTrack; // Utilities.STSmooth(Utilities.GetElevationTrack(activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.ElevationSmoothingSeconds, out min, out max);


        //    if (smoothedElevation.Count != 0 && distanceTrack.Count != 0)
        //    {
        //        double start_distance = 0;
        //        double end_distance = 0;


        //        DateTime elevationEntryTime = smoothedElevation.EntryDateTime(distanceTrack[0]);

        //        double start_elevation = smoothedElevation.GetInterpolatedValue(elevationEntryTime).Value;
        //        double end_elevation = start_elevation;

        //        int start_i = 0;
        //        int end_i = 0;

        //        DateTime startTime = new DateTime();
        //        DateTime endTime = new DateTime();

        //        bool areWeClimbing = false;

        //        double this_elevation = start_elevation;
        //        double last_elevation = start_elevation;

        //        //Parse through all the smoothed elevation points
        //        for (int i = 0; i < distanceTrack.Count; i++)
        //        {
        //            elevationEntryTime = smoothedElevation.EntryDateTime(distanceTrack[i]);

        //            this_elevation = smoothedElevation.GetInterpolatedValue(elevationEntryTime).Value;
        //            // Find the start time of the track and use it to find the current grade and distance
        //            DateTime start = smoothedElevation.StartTime;
        //            // I can't use grade, don't try it again.  It's not correct.
        //            //float grade = ai.SmoothedGradeTrack.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i].ElapsedSeconds)).Value;

        //            double distance = 0;
        //            try
        //            {
        //                // Try GetInterpolatedValue.  If it fails, distance will be 0
        //                distance = distanceTrack[i].Value; // (double)distanceTrack.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i].ElapsedSeconds)).Value;
        //            }
        //            catch
        //            {
        //            }
        //            // If we are current going uphill and climbing
        //            if ((this_elevation - last_elevation) > 0 && areWeClimbing == true)
        //            {
        //                end_i = i;
        //                elevationEntryTime = smoothedElevation.EntryDateTime(distanceTrack[i]);
        //                endTime = start.AddSeconds(distanceTrack[i].ElapsedSeconds);
        //                end_elevation = smoothedElevation.GetInterpolatedValue(elevationEntryTime).Value;
        //                end_distance = distance;
        //            }

        //            // If we just switched from descending to climbing
        //            else if ((this_elevation - last_elevation) > 0 && areWeClimbing == false)
        //            {
        //                // Make sure we aren't sending a blank record
        //                if (start_i > end_i)
        //                {
        //                    // Add the descent record (start and end will flipped due to descent)
        //                    features.Add(new Feature(activity, Feature.feature_type.descent, endTime, startTime));
        //                }

        //                // If this isn't the first record, step back 1
        //                if (i != 0)
        //                {
        //                    start_i = i - 1;
        //                    elevationEntryTime = smoothedElevation.EntryDateTime(distanceTrack[i-1]);
        //                    startTime = start.AddSeconds(distanceTrack[i - 1].ElapsedSeconds);
        //                    start_elevation = smoothedElevation.GetInterpolatedValue(elevationEntryTime).Value;
        //                    start_distance = 0;
        //                    try
        //                    {
        //                        //start_distance = (double)ai.MovingDistanceMetersTrack.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i - 1].ElapsedSeconds)).Value;
        //                        start_distance = distanceTrack[i - 1].Value; // (double)distanceTrack.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i - 1].ElapsedSeconds)).Value;
        //                    }
        //                    catch
        //                    {
        //                    }
        //                }

        //                // If this is the first record, we can't step back
        //                else
        //                {
        //                    start_i = i;
        //                    elevationEntryTime = smoothedElevation.EntryDateTime(distanceTrack[i]);
        //                    startTime = start.AddSeconds(distanceTrack[i].ElapsedSeconds);
        //                    start_elevation = smoothedElevation.GetInterpolatedValue(elevationEntryTime).Value;
        //                    start_distance = distance;
        //                }
        //                areWeClimbing = true;
        //                i--;
        //            }

        //            // If we just switched from climbing to descending
        //            else if ((this_elevation - last_elevation) <= 0 && areWeClimbing == true)
        //            {
        //                // Make sure we aren't sending a blank track
        //                if (start_i < end_i)
        //                {
        //                    // Add the ascent record 
        //                    features.Add(new Feature(activity, Feature.feature_type.ascent, startTime, endTime));
        //                }

        //                // The start point is now the old end.  Gotta rewind 1 step
        //                start_i = end_i;
        //                startTime = endTime;
        //                start_elevation = end_elevation;
        //                start_distance = end_distance;
        //                areWeClimbing = false;
        //                i--;
        //            }

        //            // If we are currently descending and going downhill
        //            else if ((this_elevation - last_elevation) <= 0 && areWeClimbing == false)
        //            {
        //                start_i = i;
        //                elevationEntryTime = smoothedElevation.EntryDateTime(distanceTrack[i]);
        //                startTime = start.AddSeconds(distanceTrack[i].ElapsedSeconds);
        //                start_elevation = smoothedElevation.GetInterpolatedValue(elevationEntryTime).Value;
        //                start_distance = distance;
        //            }

        //            // If we are on the last point, check it to see if it is the end of a climb
        //            if (i == smoothedElevation.Count - 1)
        //            {
        //                // If we ended on a climb, add it
        //                if (areWeClimbing == true)
        //                {
        //                    if (start_i < end_i)
        //                    {
        //                        features.Add(new Feature(activity, Feature.feature_type.ascent, startTime, endTime));
        //                    }
        //                }

        //                // If we ended on a descent, add it
        //                else if (areWeClimbing == false)
        //                {
        //                    if (start_i > end_i)
        //                    {
        //                        features.Add(new Feature(activity, Feature.feature_type.descent, endTime, startTime));
        //                    }
        //                }
        //            }
        //            elevationEntryTime = smoothedElevation.EntryDateTime(distanceTrack[i]);
        //            last_elevation = smoothedElevation.GetInterpolatedValue(elevationEntryTime).Value;
        //        }
        //    }
        //    return features;
        //}


        /// <summary>
        /// FindAllHills will look through a list of features and pull out all stand alone hills
        /// as well as compound hills (hills with some descents in them)
        /// </summary>
        /// <param name="activity">Activity that the features belong to</param>
        /// <param name="allFeatures">The allFeatures list should be ascent, descent, ascent, descent, etc</param>
        /// <param name="finderPercent">Percent to use to determine if a descent is part of the greater hill</param>
        /// <param name="elevationRequired">Elevation required to call this a hill (meters)</param>
        /// <param name="distanceRequired">Distance required to call this a hill (meters)</param>
        /// <param name="maxDescentLength">Max length of a descent before it breaks the hill up (meters)</param>
        /// <param name="maxDescentElevation">Max elevation change during a descent before it breaks the hill up (meters)</param>
        /// <returns>Returns a list of hills</returns>
        public List<Feature> FindAllHills(IActivity activity, List<Feature> allFeatures, double elevationPercent, double distancePercent, double elevationRequired, double distanceRequired, double maxDescentLength, double maxDescentElevation, double minAvgGrade)
        {
            List<Feature> hills = new List<Feature>();

            // If there's only 1 ascent, no need to do math on it
            if (allFeatures.Count <= 2)
            {
                foreach (Feature feature in allFeatures)
                {
                    if (feature._feature_type == Feature.feature_type.ascent)
                    {
                        if (feature.distance >= distanceRequired && feature.elevGain >= elevationRequired)
                        {
                            hills.Add(feature);
                        }
                    }
                }
            }
            else
            {
                // More than 1 ascent, iterate through all features
                for (int i = 0; i < allFeatures.Count; i++)
                {
                    // Start calculating the climb on the first ascent
                    if (allFeatures[i]._feature_type == Feature.feature_type.ascent)
                    {
                        // Init the 2 features we will use to compare to the climb
                        Feature lastAscent = null;
                        Feature currentDescent = null;

                        // Init the list of features that will make up this climb
                        List<Feature> currentClimb = new List<Feature>();

                        // Now that we have our first ascent found, see if is more detailed
                        for (int j = i; j < allFeatures.Count; j++)
                        {
                            #region Ascent
                            if (allFeatures[j]._feature_type == Feature.feature_type.ascent)
                            {
                                // If this is part of a compound climb
                                if (lastAscent != null)
                                {
                                    // Check to make sure this ascent adds elevation to the climb
                                    if (allFeatures[j].endElevation > FindHigestElevation(currentClimb)
                                        || ((allFeatures[j].endElevation - currentClimb[0].startElevation) / (FindHigestElevation(currentClimb) - currentClimb[0].startElevation) >= elevationPercent)) // Added 3/30/2011
                                    {
                                        lastAscent = allFeatures[j];
                                        currentClimb.Add(lastAscent);
                                    }
                                    else
                                    {
                                        // Since this ascent isn't counted, the last Feature in currentClimb is a descent, remove it
                                        currentClimb.RemoveAt(currentClimb.Count - 1);

                                        // Check to make sure you end on the peak of the climb
                                        if (currentClimb[currentClimb.Count - 1].endElevation < FindHigestElevation(currentClimb))
                                        {
                                            // Roll back until you hit the high point
                                            do
                                            {
                                                currentClimb.RemoveAt(currentClimb.Count - 1);
                                                j--;
                                            }
                                            while (currentClimb[currentClimb.Count - 1].endElevation < FindHigestElevation(currentClimb));
                                        }

                                        // Combine the current climb
                                        Feature combinedFeature = CombineFeatures(activity, currentClimb);

                                        // Check the current climb
                                        if (combinedFeature.distance >= distanceRequired
                                            &&
                                            combinedFeature.elevGain >= elevationRequired
                                            &&
                                            combinedFeature.avgGrade >= minAvgGrade)
                                        {
                                            // Score the hill before we add it
                                            ScoreHillClimbByBike(combinedFeature);
                                            ScoreHillCycle2Max(combinedFeature);
                                            ScoreHillFiets(combinedFeature);
                                            ScoreHillCourseScoreRunning(combinedFeature);
                                            ScoreHillCourseScoreCycling(combinedFeature);
                                            ScoreHillCategory(combinedFeature);

                                            // Add this hill
                                            hills.Add(combinedFeature);
                                        }

                                        // Reset the outer loop to where we ended and break
                                        i = j - 1;
                                        break;
                                    }
                                }
                                else
                                {
                                    // This is the first instance of the lastAscent in this subsection
                                    lastAscent = allFeatures[j];
                                    currentClimb.Add(lastAscent);
                                }
                            }
                            #endregion

                            #region Descent
                            else if (allFeatures[j]._feature_type == Feature.feature_type.descent)
                            {
                                currentDescent = allFeatures[j];
                                // Check to see if the current descent is part of the climb
                                if (
                                    (Math.Abs(currentDescent.elevGain) < maxDescentElevation) // Not too much drop
                                    &&
                                    (currentDescent.distance < maxDescentLength) // Not too long of a descent
                                    &&
                                    //(currentDescent.distance / FindTotalDistance(currentClimb) <= distancePercent) // Not too long of descent compared to the hill
                                    (((currentDescent.endDistance - FindHigestElevationDistance(currentClimb)) / FindTotalDistance(currentClimb)) <= distancePercent)
                                    &&
                                    //(Math.Abs(currentDescent.elevGain) / (FindHigestElevation(currentClimb) - currentClimb[0].startElevation) <= elevationPercent)) // Not too much drop compared to the hill
                                    ((currentDescent.endElevation - currentClimb[0].startElevation) / (FindHigestElevation(currentClimb) - currentClimb[0].startElevation)) >= elevationPercent)
                                {
                                    currentClimb.Add(currentDescent);
                                }
                                else
                                {
                                    // Check to make sure you end on the peak of the climb
                                    if (currentClimb[currentClimb.Count - 1].endElevation < FindHigestElevation(currentClimb))
                                    {
                                        Debug.WriteLine(FindHigestElevation(currentClimb));
                                        // Roll back until you hit the high point
                                        do
                                        {
                                            currentClimb.RemoveAt(currentClimb.Count - 1);
                                            j--;
                                            Debug.WriteLine(currentClimb[currentClimb.Count - 1].endElevation);
                                        }
                                        while (currentClimb[currentClimb.Count - 1].endElevation < FindHigestElevation(currentClimb));
                                    }

                                    // Combine the current climb
                                    Feature combinedFeature = CombineFeatures(activity, currentClimb);

                                    // Check the current climb
                                    if (combinedFeature.distance >= distanceRequired
                                        &&
                                        combinedFeature.elevGain >= elevationRequired
                                        &&
                                        combinedFeature.avgGrade >= minAvgGrade)
                                    {
                                        // Score the hill before we add it
                                        ScoreHillClimbByBike(combinedFeature);
                                        ScoreHillCycle2Max(combinedFeature);
                                        ScoreHillFiets(combinedFeature);
                                        ScoreHillCourseScoreRunning(combinedFeature);
                                        ScoreHillCourseScoreCycling(combinedFeature);
                                        ScoreHillCategory(combinedFeature);

                                        // Add this hill
                                        hills.Add(combinedFeature);
                                    }

                                    // Reset the outer loop to where we ended and break
                                    i = j - 1;
                                    break;
                                }
                            }
                            #endregion
                            // Check to see if we have hit the last point
                            if (j == allFeatures.Count - 1)
                            {
                                // Since this is the last point, we can't end on a descent
                                /*if (currentClimb[currentClimb.Count - 1]._feature_type == Feature.feature_type.descent)
                                {
                                    currentClimb.RemoveAt(currentClimb.Count - 1);
                                }*/

                                // Check to make sure you end on the peak of the climb
                                if (currentClimb[currentClimb.Count - 1].endElevation < FindHigestElevation(currentClimb))
                                {
                                    // Roll back until you hit the high point
                                    do
                                    {
                                        currentClimb.RemoveAt(currentClimb.Count - 1);
                                        //j--;
                                    }
                                    while (currentClimb[currentClimb.Count - 1].endElevation < FindHigestElevation(currentClimb));
                                }

                                // Combine the current climb
                                Feature combinedFeature = CombineFeatures(activity, currentClimb);

                                // Check the current climb
                                if (combinedFeature.distance >= distanceRequired
                                    &&
                                    combinedFeature.elevGain >= elevationRequired
                                    &&
                                    combinedFeature.avgGrade >= minAvgGrade)
                                {
                                    // Score the hill before we add it
                                    ScoreHillClimbByBike(combinedFeature);
                                    ScoreHillCycle2Max(combinedFeature);
                                    ScoreHillFiets(combinedFeature);
                                    ScoreHillCourseScoreRunning(combinedFeature);
                                    ScoreHillCourseScoreCycling(combinedFeature);
                                    ScoreHillCategory(combinedFeature);

                                    // Add this hill
                                    hills.Add(combinedFeature);

                                    // Since we found the last hill in the route, set the outer loop to break
                                    i = j;
                                    break;
                                }
                            }
                        }
                    }
                }
            }


            return hills;
        }

        /// <summary>
        /// FindAllDownHills will look through a list of features and pull out all stand alone downhills
        /// as well as compound downhills (downhills with some ascents in them)
        /// </summary>
        /// <param name="activity">Activity that the features belong to</param>
        /// <param name="allFeatures">The allFeatures list should be ascent, descent, ascent, descent, etc</param>
        /// <param name="finderPercent">Percent to use to determine if a ascent is part of the greater downhill</param>
        /// <param name="elevationRequired">Elevation required to call this a downhill (meters)</param>
        /// <param name="distanceRequired">Distance required to call this a downhill (meters)</param>
        /// <param name="maxDescentLength">Max length of a ascent before it breaks the downhill up (meters)</param>
        /// <param name="maxDescentElevation">Max elevation change during a ascent before it breaks the downhill up (meters)</param>
        /// <returns>Returns a list of downhills</returns>
        public List<Feature> FindAllDownHills(IActivity activity, List<Feature> allFeatures, double elevationPercent, double distancePercent, double elevationRequired, double distanceRequired, double maxDescentLength, double maxDescentElevation, double minAvgGrade)
        {
            List<Feature> downHills = new List<Feature>();

            // If there's only 1 ascent, no need to do math on it
            if (allFeatures.Count <= 2)
            {
                foreach (Feature feature in allFeatures)
                {
                    if (feature._feature_type == Feature.feature_type.descent)
                    {
                        if (feature.distance >= distanceRequired && feature.elevGain <= (elevationRequired * -1))
                        {
                            downHills.Add(feature);
                        }
                    }
                }
            }
            else
            {
                // More than 1 ascent, iterate through all features
                for (int i = 0; i < allFeatures.Count; i++)
                {
                    // Start calculating the climb on the first ascent
                    if (allFeatures[i]._feature_type == Feature.feature_type.descent)
                    {
                        // Init the 2 features we will use to compare to the climb
                        Feature lastDescent = null;
                        Feature currentAscent = null;

                        // Init the list of features that will make up this climb
                        List<Feature> currentDownHill = new List<Feature>();

                        // Now that we have our first descent found, see if is more detailed
                        for (int j = i; j < allFeatures.Count; j++)
                        {
                            if (allFeatures[j]._feature_type == Feature.feature_type.descent)
                            {
                                // If this is part of a compound downhill
                                if (lastDescent != null)
                                {
                                    // Check to make sure this descent adds elevation to the downhill
                                    if (allFeatures[j].endElevation < FindLowestElevation(currentDownHill))
                                    {
                                        lastDescent = allFeatures[j];
                                        currentDownHill.Add(lastDescent);
                                    }
                                    else
                                    {
                                        // Since this descent isn't counted, the last Feature in currentDownHill is a ascent, remove it
                                        currentDownHill.RemoveAt(currentDownHill.Count - 1);

                                        // Combine the current descent
                                        Feature combinedFeature = CombineFeatures(activity, currentDownHill);
                                        combinedFeature._feature_type = Feature.feature_type.descent;
                                        // Check the current descent
                                        if (combinedFeature.distance >= distanceRequired
                                            &&
                                            combinedFeature.elevGain <= (elevationRequired * -1)
                                            &&
                                            combinedFeature.avgGrade <= (minAvgGrade * -1))
                                        {
                                            // Score the downHill before we add it
                                            ScoreHillClimbByBike(combinedFeature);
                                            ScoreHillCycle2Max(combinedFeature);
                                            ScoreHillFiets(combinedFeature);
                                            ScoreHillCourseScoreRunning(combinedFeature);
                                            ScoreHillCourseScoreCycling(combinedFeature);
                                            ScoreHillCategory(combinedFeature);

                                            // Add this downHill
                                            downHills.Add(combinedFeature);
                                        }

                                        // Reset the outer loop to where we ended and break
                                        i = j - 1;
                                        break;
                                    }
                                }
                                else
                                {
                                    // This is the first instance of the lastAscent in this subsection
                                    lastDescent = allFeatures[j];
                                    currentDownHill.Add(lastDescent);
                                }
                            }
                            else if (allFeatures[j]._feature_type == Feature.feature_type.ascent)
                            {
                                currentAscent = allFeatures[j];
                                // Check to see if the current ascent is part of the downHill
                                if (
                                    (currentAscent.elevGain < maxDescentElevation) // Not too much climb
                                    &&
                                    (currentAscent.distance < maxDescentLength) // Not too long of a descent
                                    &&
                                    (currentAscent.distance / FindTotalDistance(currentDownHill) <= distancePercent) // Not too long of descent compared to the hill
                                    &&
                                    (Math.Abs(currentAscent.elevGain) / (FindHigestElevation(currentDownHill) - currentDownHill[0].startElevation) <= elevationPercent)) // Not too much drop compared to the hill
                                {
                                    currentDownHill.Add(currentAscent);
                                }
                                else
                                {
                                    // Combine the current downHill
                                    Feature combinedFeature = CombineFeatures(activity, currentDownHill);
                                    combinedFeature._feature_type = Feature.feature_type.descent;

                                    // Check the current climb
                                    if (combinedFeature.distance >= distanceRequired
                                        &&
                                        combinedFeature.elevGain <= (elevationRequired * -1)
                                        &&
                                        combinedFeature.avgGrade <= (minAvgGrade * -1))
                                    {
                                        // Score the hill before we add it
                                        ScoreHillClimbByBike(combinedFeature);
                                        ScoreHillCycle2Max(combinedFeature);
                                        ScoreHillFiets(combinedFeature);
                                        ScoreHillCourseScoreRunning(combinedFeature);
                                        ScoreHillCourseScoreCycling(combinedFeature);
                                        ScoreHillCategory(combinedFeature);

                                        // Add this hill
                                        downHills.Add(combinedFeature);
                                    }

                                    // Reset the outer loop to where we ended and break
                                    i = j - 1;
                                    break;
                                }
                            }

                            // Check to see if we have hit the last point
                            if (j == allFeatures.Count - 1)
                            {
                                // Since this is the last point, we can't end on a ascent
                                if (currentDownHill[currentDownHill.Count - 1]._feature_type == Feature.feature_type.ascent)
                                {
                                    currentDownHill.RemoveAt(currentDownHill.Count - 1);
                                }

                                // Combine the current downHill
                                Feature combinedFeature = CombineFeatures(activity, currentDownHill);
                                combinedFeature._feature_type = Feature.feature_type.descent;

                                // Check the current climb
                                if (combinedFeature.distance >= distanceRequired
                                    &&
                                    combinedFeature.elevGain <= (elevationRequired * -1)
                                    &&
                                    combinedFeature.avgGrade <= (minAvgGrade * -1))
                                {
                                    // Score the hill before we add it
                                    ScoreHillClimbByBike(combinedFeature);
                                    ScoreHillCycle2Max(combinedFeature);
                                    ScoreHillFiets(combinedFeature);
                                    ScoreHillCourseScoreRunning(combinedFeature);
                                    ScoreHillCourseScoreCycling(combinedFeature);
                                    ScoreHillCategory(combinedFeature);

                                    // Add this hill
                                    downHills.Add(combinedFeature);
                                }

                            }
                        }
                    }
                }
            }
            return downHills;
        }

        /// <summary>
        /// CombineFeatures will combine numerous sequential featurs for an activity to one feature
        /// </summary>
        /// <param name="activity">Master activity for all the features</param>
        /// <param name="featuresToCombine">A sequential (no gaps in nodes) list </param>
        /// <returns>Returns 1 feature</returns>
        public static Feature CombineFeatures(IActivity activity, List<Feature> featuresToCombine)
        {
            // Start by sorting the list by start distance
            Feature.FeatureComparer comparer = new Feature.FeatureComparer();
            comparer.ComparisonMethod = Feature.FeatureComparer.ComparisonType.Start;
            comparer.SortOrder = Feature.FeatureComparer.Order.Ascending;
            featuresToCombine.Sort(comparer);

            // All features are assumed to be sequential.  
            // That way, we can take the first and last data to combine
            Feature newFeature = new Feature(activity,
                                             Feature.feature_type.ascent,
                                             featuresToCombine[0].startTime,
                                             featuresToCombine[featuresToCombine.Count - 1].endTime);


            return newFeature;
        }

        /// <summary>
        /// FindHighestElevation will look through a list of features and find the highest point
        /// </summary>
        /// <param name="features">List of features</param>
        /// <returns>Returns highest elevation</returns>
        public static double FindHigestElevation(List<Feature> features)
        {
            double highest = double.NegativeInfinity;
            foreach (Feature f in features)
            {
                if (f.endElevation >= highest)
                {
                    highest = f.endElevation;
                }
            }
            return highest;
        }

        /// <summary>
        /// FindHighestElevationDistance will look through a list of features and find the highest point's distance
        /// </summary>
        /// <param name="features">List of features</param>
        /// <returns>Returns highest elevation</returns>
        public static double FindHigestElevationDistance(List<Feature> features)
        {
            double highest = double.NegativeInfinity;
            double distance = 0;

            foreach (Feature f in features)
            {
                if (f.endElevation >= highest)
                {
                    highest = f.endElevation;
                    distance = f.endDistance;
                }
            }
            return distance;
        }

        /// <summary>
        /// FindTotalDistance will find the total distance covered in the list of features
        /// </summary>
        /// <param name="features">List of features</param>
        /// <returns>Returns distance covered by the features</returns>
        public static double FindTotalDistance(List<Feature> features)
        {
            double distance = 0;
            foreach (Feature f in features)
            {
                distance += f.distance;
            }
            return distance;
        }

        /// <summary>
        /// CompareFeatures will compare features for matches.  The List of a list is used 
        /// if you want to compare features over multiple activities.  The outer list being the activity
        /// and the inner list is that activity's features
        /// </summary>
        /// <param name="allFeatures">List built like -> Activity/Features/individual Feature</param>
        /// <returns>Return a list of matching features</returns>
        public static List<List<Feature>> CompareFeatures(List<List<Feature>> allFeatures)
        {
            for (int i = 0; i < allFeatures.Count; i++)
            {
                Feature.FeatureComparer comparer = new Feature.FeatureComparer();
                comparer.ComparisonMethod = Feature.FeatureComparer.ComparisonType.Date;
                comparer.SortOrder = Feature.FeatureComparer.Order.Ascending;
                allFeatures[i].Sort(comparer);
            }

            List<List<Feature>> equalFeatures = new List<List<Feature>>();

            //Cycle through this list of all activities and their features
            for (int i = 0; i < allFeatures.Count; i++)
            {
                //Pull out 1 activity to check
                List<Feature> oneActivityFeatures = allFeatures[i];
                for (int j = 0; j < oneActivityFeatures.Count; j++)
                {
                    //Check to make sure the list item isn't blank
                    if (oneActivityFeatures[j] != null)
                    {
                        //Check to make sure if this feature for this activity hasn't been previously added
                        if (allFeatures[i][j].added == false)
                        {
                            List<Feature> currentEqualFeature = new List<Feature>();
                            currentEqualFeature.Add(oneActivityFeatures[j]);
                            allFeatures[i][j].added = true;
                            //Cycle through all the activities and features to find similiar features
                            for (int k = i; k < allFeatures.Count; k++)
                            {
                                List<Feature> compareActivityFeatures = allFeatures[k];

                                for (int m = 0; m < compareActivityFeatures.Count; m++)
                                {
                                    if (compareActivityFeatures[m] != null)
                                    {
                                        // Constant that defines how close a point need to be to the feature points.  Measured in meters.
                                        // This may need to be a large number for cycling... approx 40-50ish!!!
                                        // Worst case scenario:  travel 40 mph (high end of bike speed) = 17m/s.  5 sec. device recording -> 85 meters in 5 seconds.
                                        // Divide by 2 (midpoint... farthest spot from 2 consecutive points ~40 meter radius)
                                        // large number like this should still be OK because 130 feet will easily separate routes and points, etc.
                                        double proximityConstant = oneActivityFeatures[j].totalDistanceMeters / oneActivityFeatures[j].record.TotalTime.TotalSeconds * 8f;
                                        float startDist = oneActivityFeatures[j].startPoint.DistanceMetersToPoint(compareActivityFeatures[m].startPoint);
                                        float endDist = oneActivityFeatures[j].endPoint.DistanceMetersToPoint(compareActivityFeatures[m].endPoint);

                                        //Check to see if the 2 features start and end near each other +-.0005 degrees
                                        if (compareActivityFeatures[m].startPoint == null
                                            || oneActivityFeatures[j].startPoint == null)
                                        {
                                            break;
                                        }
                                        // TODO: Get rid of Lat/Lon comparison and make this like FindFeatures(...) code.
                                        /*if (compareActivityFeatures[m].startPoint.LatitudeDegrees <= oneActivityFeatures[j].startPoint.LatitudeDegrees + .00025
                                           && compareActivityFeatures[m].startPoint.LatitudeDegrees >= oneActivityFeatures[j].startPoint.LatitudeDegrees - .00025
                                           && compareActivityFeatures[m].startPoint.LongitudeDegrees <= oneActivityFeatures[j].startPoint.LongitudeDegrees + .00025
                                           && compareActivityFeatures[m].startPoint.LongitudeDegrees >= oneActivityFeatures[j].startPoint.LongitudeDegrees - .00025
                                           && compareActivityFeatures[m].endPoint.LatitudeDegrees <= oneActivityFeatures[j].endPoint.LatitudeDegrees + .00025
                                           && compareActivityFeatures[m].endPoint.LatitudeDegrees >= oneActivityFeatures[j].endPoint.LatitudeDegrees - .00025
                                           && compareActivityFeatures[m].endPoint.LongitudeDegrees <= oneActivityFeatures[j].endPoint.LongitudeDegrees + .00025
                                           && compareActivityFeatures[m].endPoint.LongitudeDegrees >= oneActivityFeatures[j].endPoint.LongitudeDegrees - .00025
                                           && compareActivityFeatures[m].refId != oneActivityFeatures[j].refId
                                           && allFeatures[k][m].added == false)*/
                                        if (compareActivityFeatures[m].refId != oneActivityFeatures[j].refId
                                            && allFeatures[k][m].added == false
                                            && startDist < proximityConstant
                                            && endDist < proximityConstant)
                                        {

                                            allFeatures[k][m].added = true;
                                            currentEqualFeature.Add(compareActivityFeatures[m]);
                                        }
                                    }
                                }
                            }
                            equalFeatures.Add(currentEqualFeature);
                        }
                    }
                }
            }
            return equalFeatures;
        }

        /// <summary>
        /// FindLowestElevation will look through a list of features and find the lowest point
        /// </summary>
        /// <param name="features">List of features</param>
        /// <returns>Returns loswet elevation</returns>
        public static double FindLowestElevation(List<Feature> features)
        {
            double lowest = double.PositiveInfinity;
            foreach (Feature f in features)
            {
                if (f.endElevation <= lowest)
                {
                    lowest = f.endElevation;
                }
            }
            return lowest;
        }

        /// <summary>
        /// GetSteepest will look at all this hills in the list and return the one with the highest grade
        /// </summary>
        /// <param name="hills">List of features to look through</param>
        /// <returns>Returns the feature with the highest grade</returns>
        private Feature GetSteepest(List<Feature> hills, bool newColor)
        {
            Feature steepest = null;

            double steepestGrade = double.NegativeInfinity;

            foreach (Feature hill in hills)
            {
                if (hill.avgGrade > steepestGrade)
                {
                    steepest = hill;
                    steepestGrade = hill.avgGrade;
                }
            }

            // If you want to recolor this hill
            if (steepest != null && newColor)
            {
                steepest.fillColor = Color.FromArgb(125, Common.SteepestColor);
                steepest.lineColor = Common.SteepestColor;
            }

            return steepest;
        }

        /// <summary>
        /// GetLongest will look at all this hills in the list and return the longest one
        /// </summary>
        /// <param name="hills">List of features to look through</param>
        /// <returns>Returns the feature with the longest distance</returns>
        private Feature GetLongest(List<Feature> hills, bool newColor)
        {
            Feature longest = null;
            double distance = double.NegativeInfinity;

            foreach (Feature hill in hills)
            {
                if (hill.distance > distance)
                {
                    longest = hill;
                    distance = hill.distance;
                }
            }

            // If you want to recolor this hill
            if (longest != null && newColor)
            {
                longest.fillColor = Color.FromArgb(125, Common.LongestColor);
                longest.lineColor = Common.LongestColor;
            }

            return longest;
        }

        /// <summary>
        /// GetHardest will look at all this hills in the list and return the one with the highest score
        /// </summary>
        /// <param name="hills">List of features to look through</param>
        /// <returns>Returns the feature with the highest score</returns>
        private Feature GetHardest(List<Feature> hills, bool newColor)
        {
            Feature hardest = null;
            double score = double.NegativeInfinity;

            ScoreEquation.Score scoreType = GlobalSettings.Instance.ScoreEquation;

            foreach (Feature hill in hills)
            {
                if (scoreType == ScoreEquation.Score.ClimbByBike)
                {
                    if (hill.hillScoreClimbByBike > score)
                    {
                        hardest = hill;
                        score = hill.hillScoreClimbByBike;
                    }
                }
                else if (scoreType == ScoreEquation.Score.Cycle2Max)
                {
                    if (hill.hillScoreCycle2Max > score)
                    {
                        hardest = hill;
                        score = hill.hillScoreCycle2Max;
                    }
                }
                else if (scoreType == ScoreEquation.Score.Fiets)
                {
                    if (hill.hillScoreFiets > score)
                    {
                        hardest = hill;
                        score = hill.hillScoreFiets;
                    }
                }
                else if (scoreType == ScoreEquation.Score.CourseScoreCycling)
                {
                    if (hill.hillScoreCourseScoreCycling > score)
                    {
                        hardest = hill;
                        score = hill.hillScoreCourseScoreCycling;
                    }
                }
                else if (scoreType == ScoreEquation.Score.CourseScoreRunning)
                {
                    if (hill.hillScoreCourseScoreRunning > score)
                    {
                        hardest = hill;
                        score = hill.hillScoreCourseScoreRunning;
                    }
                }
            }

            // If you want to recolor this hill
            if (hardest != null && newColor)
            {
                hardest.fillColor = Color.FromArgb(125, Common.HardestColor);
                hardest.lineColor = Common.HardestColor;
            }

            return hardest;
        }

        /// <summary>
        /// FindThisFeature will try to find this exact feature based on start and end GPS points
        /// </summary>
        /// <param name="feature">The master feature to which the others must match</param>
        /// <param name="searchActivity">Activity to look through for this feature</param>
        /// <returns>Returns a list of features that match the master feature</returns>
        public List<Feature> FindThisFeature(Feature feature, IActivity searchActivity)
        {
            List<Feature> equalFeatures = new List<Feature>();
            ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(searchActivity);

            // Constant that defines how close a point need to be to the feature points.  Measured in meters.
            // This may need to be a large number for cycling... approx 40-50ish!!!
            // Worst case scenario:  travel 40 mph (high end of bike speed) = 17m/s.  5 sec. device recording -> 85 meters in 5 seconds.
            // Divide by 2 (midpoint... farthest spot from 2 consecutive points ~40 meter radius)
            // large number like this should still be OK because 130 feet will easily separate routes and points, etc.
            double proximityConstant = feature.totalDistanceMeters / feature.record.TotalTime.TotalSeconds * 4f;

            if (searchActivity.GPSRoute != null && ai.SmoothedElevationTrack != null && ai.SmoothedElevationTrack.Count > 0)
            {
                if (searchActivity.GPSRoute.Count > 0)
                {
                    double start_distance = 0;
                    double end_distance = 0;

                    double start_elevation = ai.SmoothedElevationTrack[0].Value;
                    double end_elevation = ai.SmoothedElevationTrack[0].Value;

                    int start_i = 0;
                    int end_i = 0;

                    DateTime startTime = new DateTime();
                    DateTime endTime = new DateTime();

                    bool startFound = false;
                    float start_delta = float.MaxValue; // distance from start point during search
                    DateTime start = searchActivity.StartTime;
                    INumericTimeDataSeries meters = Utilities.GetDistanceMovingTrack(searchActivity);

                    for (int i = 0; i < searchActivity.GPSRoute.Count; i++)
                    {
                        // If the index is outside the distance track, we are done.  Break.
                        if (i >= meters.Count)
                        {
                            break;
                        }

                        GPSPoint p = (GPSPoint)searchActivity.GPSRoute[i].Value;

                        // Start point
                        float dist = feature.startPoint.DistanceMetersToPoint(p);
                        if (dist < proximityConstant && dist < start_delta)
                        {
                            start_i = i;
                            startTime = start.AddSeconds(searchActivity.GPSRoute[i].ElapsedSeconds);
                            startFound = true;
                            start_distance = meters[i].Value;
                            start_elevation = p.ElevationMeters;
                        }

                        // End point
                        if (feature.endPoint.DistanceMetersToPoint(p) < proximityConstant)
                        {
                            // zero in on the best end point instead of taking first closest match
                            // continue searching until we start getting farther away (or get to the track end)
                            dist = feature.endPoint.DistanceMetersToPoint(p);
                            while (i + 2 < searchActivity.GPSRoute.Count && feature.endPoint.DistanceMetersToPoint(p) <= dist)
                            {
                                dist = feature.endPoint.DistanceMetersToPoint(p);
                                i++;
                                p = (GPSPoint)searchActivity.GPSRoute[i + 1].Value;
                            }

                            end_i = i;

                            endTime = start.AddSeconds(searchActivity.GPSRoute[i].ElapsedSeconds);
                            end_distance = meters[i].Value;
                            end_elevation = p.ElevationMeters;

                            // Check to make sure we found a start to go with this end
                            if (startFound)
                            {
                                Feature match = new Feature(searchActivity, feature._feature_type, startTime, endTime);

                                // Score hill
                                ScoreHillClimbByBike(match);
                                ScoreHillCycle2Max(match);
                                ScoreHillFiets(match);
                                ScoreHillCourseScoreRunning(match);
                                ScoreHillCourseScoreCycling(match);
                                ScoreHillCategory(match);

                                double distanceBuffer = feature.distance * .15f;
                                double gradeBuffer = .02f;

                                // Check and make sure key stats are close (make sure it's the same route)
                                if (Math.Abs(feature.distance - match.distance) < distanceBuffer &&
                                    Math.Abs(feature.avgGrade - match.avgGrade) < gradeBuffer)
                                {
                                    equalFeatures.Add(match);
                                }
                                else
                                {
                                    // Filtered
                                }
                            }

                            // Reset to continue to look for this hill in this activity
                            startFound = false;
                            start_delta = float.MaxValue;
                        }
                    }
                }
            }

            return equalFeatures;
        }

        /// <summary>
        /// FindLikeFeatures will try to find features like this one's elevation and distance
        /// </summary>
        /// <param name="feature">The master feature to which the others must match</param>
        /// <param name="searchActivity">Activity to look through for this feature</param>
        /// <returns>Returns a list of features that are like the master feature</returns>
        public List<Feature> FindLikeFeatures(Feature feature, IActivity searchActivity)
        {
            // Create all features
            List<Feature> all_features = CreateAllFeatures(searchActivity);
            List<Feature> features = new List<Feature>();

            // Find all features for this event
            if (feature._feature_type == Feature.feature_type.ascent)
            {
                features = FindAllHills(searchActivity, all_features, GlobalSettings.Instance.ElevationPercent, GlobalSettings.Instance.DistancePercent, GlobalSettings.Instance.GainElevationRequired, GlobalSettings.Instance.HillDistanceRequired, GlobalSettings.Instance.MaxDescentLength, GlobalSettings.Instance.MaxDescentElevation, GlobalSettings.Instance.MinAvgGrade);
            }
            else if (feature._feature_type == Feature.feature_type.descent)
            {
                features = FindAllHills(searchActivity, all_features, GlobalSettings.Instance.ElevationPercent, GlobalSettings.Instance.DistancePercent, GlobalSettings.Instance.GainElevationRequired, GlobalSettings.Instance.HillDistanceRequired, GlobalSettings.Instance.MaxDescentLength, GlobalSettings.Instance.MaxDescentElevation, GlobalSettings.Instance.MinAvgGrade);
            }

            // TODO: turn these into settings
            double distanceBuffer = feature.distance * .05f;
            double elevationBuffer = feature.elevGain * .05f;
            List<Feature> matches = new List<Feature>();

            foreach (Feature f in features)
            {
                if (feature.distance <= f.distance + distanceBuffer
                   && feature.distance >= f.distance - distanceBuffer
                   && feature.elevGain <= f.elevGain + elevationBuffer
                   && feature.elevGain >= f.elevGain - elevationBuffer)
                {
                    matches.Add(f);
                }
            }
            return matches;
        }

        /// <summary>
        /// SetHillNumbers will set the HillID for the supplied features
        /// </summary>
        /// <param name="features">Features to number</param>
        public void SetHillNumbers(List<Feature> features)
        {
            // If the features list actually contains features
            if (features != null)
            {
                // Init your Lists of Lists of Features
                List<List<Feature>> allFeatures = new List<List<Feature>>();
                List<List<Feature>> equalFeatures = new List<List<Feature>>();

                // Add the master list of features to allFeatures (this activity only)
                allFeatures.Add(features);

                // Find all the features that match
                if (features.Count > 0)
                {
                    if (features[0].startPoint != null)
                    {
                        equalFeatures = CompareFeatures(allFeatures);
                    }
                    else
                    {
                        equalFeatures = allFeatures;
                    }
                }
                else
                {
                    equalFeatures = allFeatures;
                }

                // Parse through the equalFeatures list to number the features
                for (int i = 0; i < equalFeatures.Count; i++)
                {
                    List<Feature> oneFeature = equalFeatures[i];
                    for (int j = 0; j < oneFeature.Count; j++)
                    {
                        if (oneFeature[j] != null)
                        {
                            oneFeature[j].hillNumber = i + 1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ColorFeatures will set up the color scheme and color the features for the colortype you select
        /// (steepest, longest, hardest)
        /// </summary>
        /// <param name="features">List of features to apply the color settings to</param>
        public void ColorFeatures(List<Feature> features)
        {
            int minAlpha = 5;
            int maxAlpha = 255 - minAlpha;

            if (GlobalSettings.Instance.ColorType == ColorType.Hardest)
            {
                // Pull the score method from settings and sort the hills list by that score
                Feature.FeatureComparer comparer = new Feature.FeatureComparer();
                ScoreEquation.Score scoreType = GlobalSettings.Instance.ScoreEquation;

                if (scoreType == ScoreEquation.Score.ClimbByBike)
                {
                    comparer.ComparisonMethod = Feature.FeatureComparer.ComparisonType.HillScoreClimbByBike;
                }
                else if (scoreType == ScoreEquation.Score.Cycle2Max)
                {
                    comparer.ComparisonMethod = Feature.FeatureComparer.ComparisonType.HillScoreCycle2Max;
                }
                else if (scoreType == ScoreEquation.Score.Fiets)
                {
                    comparer.ComparisonMethod = Feature.FeatureComparer.ComparisonType.HillScoreFiets;
                }
                else if (scoreType == ScoreEquation.Score.CourseScoreCycling)
                {
                    comparer.ComparisonMethod = Feature.FeatureComparer.ComparisonType.HillScoreCourseScoreCycling;
                }
                else if (scoreType == ScoreEquation.Score.CourseScoreRunning)
                {
                    comparer.ComparisonMethod = Feature.FeatureComparer.ComparisonType.HillScoreCourseScoreRunning;
                }

                comparer.SortOrder = Feature.FeatureComparer.Order.Descending;
                features.Sort(comparer);

                int alpha = 255;
                double highScore = double.NegativeInfinity;

                List<Color> hillColors = new List<Color>();

                for (int i = 0; i < features.Count; i++)
                {
                    if (i == 0)
                    {
                        features[i].lineColor = Color.FromArgb(alpha, Common.HardestColor);
                        features[i].fillColor = Color.FromArgb(alpha, Common.HardestColor);

                        // Set the highscore for the appropriate score type
                        if (scoreType == ScoreEquation.Score.ClimbByBike)
                        {
                            highScore = features[i].hillScoreClimbByBike;
                        }
                        else if (scoreType == ScoreEquation.Score.Cycle2Max)
                        {
                            highScore = features[i].hillScoreCycle2Max;
                        }
                        else if (scoreType == ScoreEquation.Score.Fiets)
                        {
                            highScore = features[i].hillScoreFiets;
                        }
                        else if (scoreType == ScoreEquation.Score.CourseScoreCycling)
                        {
                            highScore = features[i].hillScoreCourseScoreCycling;
                        }
                        else if (scoreType == ScoreEquation.Score.CourseScoreRunning)
                        {
                            highScore = features[i].hillScoreCourseScoreRunning;
                        }
                    }
                    else
                    {
                        // Set the alpha for the appropriate score type
                        if (scoreType == ScoreEquation.Score.ClimbByBike)
                        {
                            alpha = (int)((features[i].hillScoreClimbByBike / highScore) * maxAlpha) + minAlpha;
                        }
                        else if (scoreType == ScoreEquation.Score.Cycle2Max)
                        {
                            alpha = (int)((features[i].hillScoreCycle2Max / highScore) * maxAlpha) + minAlpha;
                        }
                        else if (scoreType == ScoreEquation.Score.Fiets)
                        {
                            alpha = (int)((features[i].hillScoreFiets / highScore) * maxAlpha) + minAlpha;
                        }
                        else if (scoreType == ScoreEquation.Score.CourseScoreCycling)
                        {
                            alpha = (int)((features[i].hillScoreCourseScoreCycling / highScore) * maxAlpha) + minAlpha;
                        }
                        else if (scoreType == ScoreEquation.Score.CourseScoreRunning)
                        {
                            alpha = (int)((features[i].hillScoreCourseScoreRunning / highScore) * maxAlpha) + minAlpha;
                        }

                        // Bad score values can cause alpha to go out of range.
                        alpha = Math.Max(minAlpha, Math.Min(maxAlpha + minAlpha, alpha));

                        features[i].lineColor = Color.FromArgb(alpha, Common.HardestColor);
                        features[i].fillColor = Color.FromArgb(alpha, Common.HardestColor);
                    }
                }
            }
            else if (GlobalSettings.Instance.ColorType == ColorType.Longest)
            {
                // Sort the hills list by score
                Feature.FeatureComparer comparer = new Feature.FeatureComparer();
                comparer.ComparisonMethod = Feature.FeatureComparer.ComparisonType.Distance;
                comparer.SortOrder = Feature.FeatureComparer.Order.Descending;
                features.Sort(comparer);

                int alpha = 255;
                double longest = double.NegativeInfinity;

                List<Color> hillColors = new List<Color>();

                for (int i = 0; i < features.Count; i++)
                {
                    if (i == 0)
                    {
                        features[i].lineColor = Color.FromArgb(alpha, Common.LongestColor);
                        features[i].fillColor = Color.FromArgb(alpha, Common.LongestColor);
                        longest = features[i].distance;
                    }
                    else
                    {
                        alpha = (int)((features[i].distance / longest) * maxAlpha) + minAlpha;

                        // Bad score values can cause alpha to go out of range.
                        alpha = Math.Max(minAlpha, Math.Min(maxAlpha + minAlpha, alpha));

                        features[i].lineColor = Color.FromArgb(alpha, Common.LongestColor);
                        features[i].fillColor = Color.FromArgb(alpha, Common.LongestColor);
                    }
                }
            }
            else if (GlobalSettings.Instance.ColorType == ColorType.Steepest)
            {
                // Sort the hills list by score
                Feature.FeatureComparer comparer = new Feature.FeatureComparer();
                comparer.ComparisonMethod = Feature.FeatureComparer.ComparisonType.AvgGrade;
                comparer.SortOrder = Feature.FeatureComparer.Order.Descending;
                features.Sort(comparer);

                int alpha = maxAlpha;
                double steepest = double.NegativeInfinity;

                List<Color> hillColors = new List<Color>();

                for (int i = 0; i < features.Count; i++)
                {
                    if (i == 0)
                    {
                        features[i].lineColor = Color.FromArgb(alpha, Common.SteepestColor);
                        features[i].fillColor = Color.FromArgb(alpha, Common.SteepestColor);
                        steepest = features[i].avgGrade;
                    }
                    else
                    {
                        alpha = (int)((features[i].avgGrade / steepest) * maxAlpha) + minAlpha;

                        // Bad score values can cause alpha to go out of range.
                        alpha = Math.Max(minAlpha, Math.Min(maxAlpha + minAlpha, alpha));

                        features[i].lineColor = Color.FromArgb(alpha, Common.SteepestColor);
                        features[i].fillColor = Color.FromArgb(alpha, Common.SteepestColor);
                    }
                }
            }
            else if (GlobalSettings.Instance.ColorType == ColorType.Basic)
            {
                Feature steepest = null;
                Feature longest = null;
                Feature hardest = null;

                if (features.Count != 0)
                {
                    steepest = GetSteepest(features, true);
                    longest = GetLongest(features, true);
                    hardest = GetHardest(features, true);
                }

                for (int i = 0; i < features.Count; i++)
                {
                    if (features[i].refId == hardest.refId)
                    {
                        features[i].lineColor = Color.FromArgb(255, Common.HardestColor);
                        features[i].fillColor = Color.FromArgb(235, Common.HardestColor);
                    }
                    else if (features[i].refId == longest.refId)
                    {
                        features[i].lineColor = Color.FromArgb(255, Common.LongestColor);
                        features[i].fillColor = Color.FromArgb(235, Common.LongestColor);
                    }
                    else if (features[i].refId == steepest.refId)
                    {
                        features[i].lineColor = Color.FromArgb(255, Common.SteepestColor);
                        features[i].fillColor = Color.FromArgb(235, Common.SteepestColor);
                    }


                    else
                    {
                        features[i].lineColor = Color.FromArgb(255, Common.ColorElevation); //255
                        features[i].fillColor = Color.FromArgb(125, Common.ColorElevation); //125
                    }
                }
            }
        }
        #endregion

        #region Graph Function

        /// <summary>
        /// Draw a dataseries on the the chart.
        /// </summary>
        /// <param name="series">Series to draw.</param>
        /// <param name="distanceBase">True if this is a distance based chart, false if time based.</param>
        /// <param name="ai">ActivityInfo associated with this activity.</param>
        /// <param name="axis">Associated axis to plot against.</param>
        /// <param name="totalRecs">Used for determining alpha of fill color</param>
        /// <param name="color">Line color.</param>
        /// <param name="fill">True to fill the chart, false for a simple line.</param>
        /// <returns>Returns index of dataseries as it was added to the chart, or -1 if nothing was added</returns>
        private int DrawSeriesGraph(INumericTimeDataSeries series, bool distanceBase, ActivityInfo ai, IAxis axis,
                                    int totalRecs, Color lineColor, bool fill)
        {
            // Bad or empty data
            if (series == null || series.Count == 0)
            {
                return -1;
            }

            // Remove any paused time from the incoming track
            series = Utilities.RemovePausedTimesInTrack(series, ai.Activity);

            ChartDataSeries ds = new ChartDataSeries(MainChart, axis);

            float distanceTotal = 0;
            float secondsLast = 0;

            // Draw the track
            if (distanceBase)
            {

                float min;
                float max;
                INumericTimeDataSeries distanceTrack = null;
                INumericTimeDataSeries smoothedElevation = null;

                // If we are in one mode, we can use the ActivityInfo tracks
                if (mode == Mode.OneActivity && !zoomed)
                {
                    distanceTrack = Utilities.RemovePausedTimesInTrack(ai.MovingDistanceMetersTrack, ai.Activity);
                    smoothedElevation = Utilities.RemovePausedTimesInTrack(ai.SmoothedElevationTrack, ai.Activity); // Utilities.RemovePausedTimesInTrack(Utilities.STSmooth(Utilities.GetElevationTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.ElevationSmoothingSeconds, out min, out max), ai.Activity);
                }
                // MovingDistanceMetersTrack and SmoothedElevationTrack aren't created for subActivities, you need to make it yourself
                else if (mode == Mode.MultipleActivities || zoomed)
                {
                    distanceTrack = Utilities.RemovePausedTimesInTrack(ai.Activity.DistanceMetersTrack, ai.Activity);
                    smoothedElevation = Utilities.RemovePausedTimesInTrack(Utilities.STSmooth(ai.Activity.ElevationMetersTrack, PluginMain.GetApplication().SystemPreferences.AnalysisSettings.ElevationSmoothingSeconds, out min, out max), ai.Activity);
                    distanceTrack = Utilities.SetTrackStartValueToZero(distanceTrack);
                }


                for (int i = 0; i < distanceTrack.Count; i++)
                {
                    //float value = series[i].Value;
                    DateTime time = series.EntryDateTime(distanceTrack[i]);


                    ITimeValueEntry<float> distancePoint = distanceTrack.GetInterpolatedValue(time);
                    if (distancePoint != null)
                    {
                        // Get the value in m/s
                        //float value = series.GetInterpolatedValue(dista
                        ITimeValueEntry<float> elevationEntry = series.GetInterpolatedValue(time);
                        // Convert the xaxis to be distance
                        distanceTotal = distancePoint.Value;
                        secondsLast = distancePoint.ElapsedSeconds;

                        if (elevationEntry != null)
                        {
                            float distance = (float)Length.Convert(distanceTotal, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits);
                            PointF point = new PointF(distance, elevationEntry.Value);
                            if (!ds.Points.ContainsKey(point.X))
                            {
                                ds.Points.Add(point.X, point);
                            }
                        }
                    }
                }
            }
            else
            {
                // Draw time based chart
                foreach (TimeValueEntry<float> item in series)
                {
                    PointF point = new PointF(item.ElapsedSeconds, item.Value);
                    ds.Points.Add(point.X, point);
                }
            }

            // Check to see if the right vertical axis already exists or not
            bool found = false;
            foreach (RightVerticalAxis rva in MainChart.YAxisRight)
            {
                if (rva.Label == axis.Label)
                {
                    found = true;
                    ds.ValueAxis = rva;
                    break;
                }
            }

            if (!found)
            {
                if (axis.GetType() == typeof(RightVerticalAxis))
                {
                    MainChart.YAxisRight.Add(axis);
                    ds.ValueAxis = axis;
                }
                else
                {
                    ds.ValueAxis = axis;
                }
            }

            // Draw the chart
            if (fill)
            {
                ds.ChartType = ChartDataSeries.Type.Fill;
            }
            else
            {
                ds.ChartType = ChartDataSeries.Type.Line;
            }

            ds.LineColor = lineColor;
            ds.FillColor = Color.FromArgb(20 + (100 / totalRecs), lineColor);
            ds.Data = ai.Activity.ReferenceId;
            //ds.FillColor = Color.FromArgb(20 + (100 / totalRecs), fillColor);
            //ds.LineWidth = lineWidth;

            MainChart.DataSeries.Add(ds);

            MainChart.XAxis.ChartLines = true;
            return MainChart.DataSeries.IndexOf(ds);
        }

        /// <summary>
        /// DrawElevationProfile will draw all the features to the graph
        /// </summary>
        /// <param name="features">All features you wish to draw</param>
        private void DrawElevationProfile(List<Feature> features, bool autozoom)
        {
            if (activity != null)
            {
                HillChartType chartType = GlobalSettings.Instance.ChartType;
                ChartDataSeries activityData = new ChartDataSeries(MainChart, MainChart.YAxis);
                ChartDataSeries featuresData = new ChartDataSeries(MainChart, MainChart.YAxis);
                float distanceTotal = 0;
                float secondsLast = 0;

                List<ChartDataSeries> cds = new List<ChartDataSeries>();
                if (features != null)
                {
                    for (int i = 0; i < features.Count; i++)
                    {
                        ChartDataSeries cd = new ChartDataSeries(MainChart, MainChart.YAxis);
                        cd.ChartType = ChartDataSeries.Type.Fill;
                        cd.LineColor = features[i].lineColor;
                        cd.LineWidth = features[i].lineWidth;
                        cd.FillColor = features[i].fillColor;

                        cd.ValueAxis = MainChart.YAxis;
                        cd.Data = "HILL" + features[i].refId;
                        cds.Add(cd);
                    }
                }

                ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(activity);
                //INumericTimeDataSeries distanceTrack = Utilities.GetDistanceTrack(activity);
                MainChart.YAxis.Label = CommonResources.Text.LabelElevation;
                MainChart.YAxis.LabelColor = Color.Black;

                if (chartType == HillChartType.ClimbDistance
                    || chartType == HillChartType.DescentDistance
                    || chartType == HillChartType.Overall
                    || chartType == HillChartType.SplitsDistance)
                {
                    #region Distance

                    /*if (ai.SmoothedElevationTrack.Count != 0)
                    {
                        for (int i = 0; i < ai.SmoothedElevationTrack.Count; i++)
                        {
                            DateTime start = ai.ActualTrackStart;

                            // Get the value in m/s
                            float value = (float)Length.Convert(ai.SmoothedElevationTrack[i].Value, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);
                            float distance = 0;
                            secondsLast = 0;
                            // Convert the xaxis to be distance
                            try
                            {
                                distanceTotal = (distanceTrack.GetInterpolatedValue(start.AddSeconds(ai.SmoothedElevationTrack[i].ElapsedSeconds)).Value * (distanceTrack.GetInterpolatedValue(start.AddSeconds(ai.SmoothedElevationTrack[i].ElapsedSeconds)).ElapsedSeconds - secondsLast)) + distanceTotal;
                                secondsLast = ai.SmoothedSpeedTrack.GetInterpolatedValue(start.AddSeconds(ai.SmoothedElevationTrack[i].ElapsedSeconds)).ElapsedSeconds;
                                distance = (float)distanceTrack.GetInterpolatedValue(start.AddSeconds(ai.SmoothedElevationTrack[i].ElapsedSeconds)).Value;
                            }
                            catch
                            {

                            }

                            PointF point = new PointF((float)Length.Convert(distance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits), value);
                            try
                            {
                                activityData.Points.Add((float)Length.Convert(distance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits), point);
                            }
                            catch { }
                            //distance = (float)Length.Convert(distance, Length.Units.Meter, Plugin.GetApplication().SystemPreferences.DistanceUnits);
                            if (features != null)
                            {
                                for (int j = 0; j < features.Count; j++)
                                {
                                    if (distance >= features[j].startDistance && distance <= features[j].endDistance)
                                    {
                                        try
                                        {
                                            cds[j].Points.Add((float)Length.Convert(distanceTotal, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits), point);
                                        }
                                        catch { }
                                        break;
                                    }
                                }
                            }
                        }
                    }*/

                    // Draw Distance based chart
                    //float min, max;
                    //INumericTimeDataSeries smoothedSpeed = Utilities.RemovePausedTimesInTrack(ai.SmoothedSpeedTrack, activity); // Utilities.RemovePausedTimesInTrack(Utilities.STSmooth(Utilities.GetSpeedTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.SpeedSmoothingSeconds, out min, out max), ai.Activity);
                    INumericTimeDataSeries distanceTrack = Utilities.RemovePausedTimesInTrack(ai.MovingDistanceMetersTrack, ai.Activity);
                    INumericTimeDataSeries smoothedElevation = Utilities.RemovePausedTimesInTrack(ai.SmoothedElevationTrack, activity); // Utilities.RemovePausedTimesInTrack(Utilities.STSmooth(Utilities.GetElevationTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.ElevationSmoothingSeconds, out min, out max), ai.Activity);
                    if (smoothedElevation != null && smoothedElevation.Count > 0)
                    {
                        for (int i = 0; i < distanceTrack.Count; i++)
                        {

                            DateTime time = smoothedElevation.EntryDateTime(distanceTrack[i]);
                            ITimeValueEntry<float> elevationEntry = smoothedElevation.GetInterpolatedValue(time);

                            // Get the value in m/s
                            //float value = smoothedElevation[i].Value;         //THIS WAS CRASHING ON UWE'S DELETED ELEVATION TRACK
                            float value = 0;
                            if (elevationEntry != null)
                            {
                                value = elevationEntry.Value;
                            }

                            ITimeValueEntry<float> distancePoint = distanceTrack.GetInterpolatedValue(time);
                            if (distancePoint != null)
                            {
                                // Convert the xaxis to be distance
                                distanceTotal = distancePoint.Value;
                                secondsLast = distancePoint.ElapsedSeconds;

                                float distance = (float)Length.Convert(distanceTotal, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits);
                                PointF point = new PointF(distance, (float)Length.Convert(value, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits));
                                if (!activityData.Points.ContainsKey(point.X))
                                {
                                    activityData.Points.Add(point.X, point);
                                }
                                if (features != null)
                                {
                                    for (int j = 0; j < features.Count; j++)
                                    {
                                        if (distanceTotal >= features[j].startDistance && distanceTotal <= features[j].endDistance)
                                        {
                                            try
                                            {
                                                cds[j].Points.Add(point.X, point);
                                            }
                                            catch { }
                                            //break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (chartType == HillChartType.ClimbTime
                    || chartType == HillChartType.DescentTime
                    || chartType == HillChartType.SplitsTime)
                {
                    #region Time
                    INumericTimeDataSeries hillSeries = Utilities.RemovePausedTimesInTrack(ai.SmoothedElevationTrack, activity);
                    INumericTimeDataSeries distanceSeries = Utilities.RemovePausedTimesInTrack(ai.MovingDistanceMetersTrack, activity);
                    if (hillSeries.Count != 0)
                    {
                        for (int i = 0; i < hillSeries.Count; i++)
                        {
                            DateTime start = ai.ActualTrackStart;
                            float value = (float)Length.Convert(hillSeries[i].Value, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);
                            PointF point = new PointF(hillSeries[i].ElapsedSeconds, value);
                            try
                            {
                                activityData.Points.Add(hillSeries[i].ElapsedSeconds, point);
                            }
                            catch { }

                            float distance = 0;
                            try
                            {
                                distance = (float)distanceSeries.GetInterpolatedValue(start.AddSeconds(hillSeries[i].ElapsedSeconds)).Value;
                            }
                            catch
                            {
                            }

                            if (features != null)
                            {
                                for (int j = 0; j < features.Count; j++)
                                {
                                    //if (start.AddSeconds(hillSeries[i].ElapsedSeconds).ToLocalTime() >= features[j].record.StartTimeNoPause && start.AddSeconds(hillSeries[i].ElapsedSeconds).ToLocalTime() <= features[j].record.StartTimeNoPause)
                                    if (distance >= features[j].startDistance && distance <= features[j].endDistance)
                                    {
                                        try
                                        {
                                            cds[j].Points.Add(hillSeries[i].ElapsedSeconds, point);
                                        }
                                        catch { }
                                        //break;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

                activityData.ChartType = ChartDataSeries.Type.Fill;
                activityData.LineColor = Color.FromArgb(255, 146, 94, 9);
                activityData.FillColor = Color.FromArgb(75, 146, 94, 9);
                activityData.ValueAxis = MainChart.YAxis;

                featuresData.ChartType = ChartDataSeries.Type.Bar;
                featuresData.LineColor = Color.Black;
                featuresData.FillColor = Color.Black;
                featuresData.ValueAxis = MainChart.YAxis;


                if (chartType == HillChartType.ClimbTime
                    || chartType == HillChartType.DescentTime
                    || chartType == HillChartType.SplitsTime)
                {
                    MainChart.XAxis.Formatter = new Formatter.SecondsToTime();
                    MainChart.XAxis.Label = CommonResources.Text.LabelTime;
                }
                else if (chartType == HillChartType.ClimbDistance
                    || chartType == HillChartType.DescentDistance
                    || chartType == HillChartType.Overall
                    || chartType == HillChartType.SplitsDistance)
                {
                    MainChart.XAxis.Formatter = new Formatter.General();
                    MainChart.XAxis.Label = CommonResources.Text.LabelDistance + " (" + activity.Category.DistanceUnits + ")";
                }

                MainChart.DataSeries.Add(activityData);

                // Clear the hill markers
                MainChart.XAxis.Markers.Clear();

                foreach (ChartDataSeries c in cds)
                {
                    MainChart.DataSeries.Add(c);

                    if (GlobalSettings.Instance.HillMarkers == true)
                    {
                        // Find the first point value in this series and draw a lap marker there
                        if (c.Points.Count > 0)
                        {
                            PointF startPoint = c.Points.Values[0];
                            PointF endPoint = c.Points.Values[c.Points.Count - 1];

                            // Setup and display the diamond with text to the screen
                            Image blankImage = Resources.Images.blank24;

                            // Build the graphic object from the image
                            Graphics g = Graphics.FromImage(blankImage);

                            // Set up the text alignment
                            StringFormat strFormat = new StringFormat();
                            strFormat.Alignment = StringAlignment.Center;
                            strFormat.LineAlignment = StringAlignment.Center;

                            string hillNum = "";
                            //Brush hillBrush = new SolidBrush(Color.White);

                            // Hunt throught the features to find this one's hill number
                            foreach (Feature f in features)
                            {
                                if ("HILL" + f.refId == c.Data.ToString())
                                {
                                    hillNum = f.HillId;
                                    //hillBrush = new SolidBrush(f.fillColor);
                                    break;
                                }
                            }

                            // Init the outlines for the symbol
                            int x = 0;
                            int y = 0;
                            int width = 24;
                            int height = 12;
                            PointF[] triangle = new PointF[3];

                            // Draw an upside down triangle for descents
                            if (chartType == HillChartType.DescentDistance || chartType == HillChartType.DescentTime)
                            {
                                triangle[0].X = x;
                                triangle[0].Y = y;
                                triangle[1].X = x + width;
                                triangle[1].Y = y;
                                triangle[2].X = (x + width) / 2f;
                                triangle[2].Y = y + height;
                                g.FillPolygon(new SolidBrush(PluginMain.GetApplication().VisualTheme.Control), triangle);
                                g.DrawPolygon(new Pen(PluginMain.GetApplication().VisualTheme.ControlText), triangle);
                                g.DrawString(hillNum.ToString(), new Font("Arial", 5, FontStyle.Regular), new SolidBrush(PluginMain.GetApplication().VisualTheme.ControlText), new RectangleF(x, y, width, height), strFormat);

                            }
                            // Draw a triange for ascents
                            else if (chartType == HillChartType.ClimbDistance || chartType == HillChartType.ClimbTime)
                            {
                                triangle[0].X = (x + width) / 2f;
                                triangle[0].Y = y;
                                triangle[1].X = x;
                                triangle[1].Y = y + height;
                                triangle[2].X = x + width;
                                triangle[2].Y = y + height;
                                g.FillPolygon(new SolidBrush(PluginMain.GetApplication().VisualTheme.Control), triangle);
                                g.DrawPolygon(new Pen(PluginMain.GetApplication().VisualTheme.ControlText), triangle);
                                g.DrawString(hillNum.ToString(), new Font("Arial", 5, FontStyle.Regular), new SolidBrush(PluginMain.GetApplication().VisualTheme.ControlText), new RectangleF(x, y + 2, width, height), strFormat);
                            }

                            // Draw the marker to the screen
                            AxisMarker lapMarker = new AxisMarker(((endPoint.X - startPoint.X) / 2) + startPoint.X, blankImage);
                            MainChart.XAxis.Markers.Add(lapMarker);
                        }
                    }
                }

                if (autozoom)
                {
                    MainChart.AutozoomToData(true);
                }

                MainChart.XAxis.ChartLines = true;
            }
        }

        private void DrawElevationProfile(Feature feature, bool autozoom)
        {
            if (feature.record.Activity != null && feature != null)
            {
                //List<Feature> features = ExportElevation(activity);
                HillChartType chartType = GlobalSettings.Instance.ChartType;
                MainChart.XAxis.Label = CommonResources.Text.LabelDistance + " (" + activity.Category.DistanceUnits + ")";
                MainChart.YAxis.Label = CommonResources.Text.LabelElevation;
                MainChart.YAxis.LabelColor = Color.Black;
                MainChart.XAxis.ChartLines = true;

                ChartDataSeries activityData = new ChartDataSeries(MainChart, MainChart.YAxis);
                //ChartDataSeries featuresData = new ChartDataSeries(detailPaneChart.Chart, detailPaneChart.Chart.YAxis);
                float distanceTotal = 0;
                float secondsLast = 0;

                //int x = 125;
                List<ChartDataSeries> cds = new List<ChartDataSeries>();
                if (feature != null)
                {
                    ChartDataSeries cd = new ChartDataSeries(MainChart, MainChart.YAxis);
                    cd.ChartType = ChartDataSeries.Type.Fill;
                    cd.LineColor = feature.lineColor;
                    cd.LineWidth = feature.lineWidth;
                    cd.FillColor = feature.fillColor;
                    cd.ValueAxis = MainChart.YAxis;
                    //x+=10;
                    cds.Add(cd);
                }

                ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(feature.record.Activity);
                float min;
                float max;
                // Commented out line will start the track at the distance of the start of the hill
                //INumericTimeDataSeries smoothedElevation = feature.GetSmoothedElevationTrack(PluginMain.GetApplication().SystemPreferences.AnalysisSettings.ElevationSmoothingSeconds, out min, out max);

                //INumericTimeDataSeries smoothedElevation = Utilities.RemovePausedTimesInTrack(Utilities.STSmooth(Utilities.GetElevationTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.ElevationSmoothingSeconds, out min, out max), ai.Activity);
                //INumericTimeDataSeries distanceTrack = Utilities.GetDistanceMovingTrack(feature.record.Activity);
                //distanceTrack = Utilities.SetTrackStartValueToZero(distanceTrack);


                if (chartType == HillChartType.ClimbDistance
                    || chartType == HillChartType.DescentDistance
                    || chartType == HillChartType.Overall
                    || chartType == HillChartType.SplitsDistance)
                {
                    #region Distance
                    /*if (smoothedElevation.Count != 0)
                    {
                        for (int i = 0; i < smoothedElevation.Count; i++)
                        {
                            DateTime start = ActivityInfoCache.Instance.GetInfo(feature.record.Activity).ActualTrackStart;
                            // Get the value in m/s
                            float value = (float)Length.Convert(smoothedElevation[i].Value, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);
                            float distance = 0;
                            // Convert the xaxis to be distance
                            try
                            {
                                //distanceTotal = (smoothedSpeed.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i].ElapsedSeconds)).Value * (smoothedSpeed.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i].ElapsedSeconds)).ElapsedSeconds - secondsLast)) + distanceTotal;
                                //secondsLast = smoothedSpeed.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i].ElapsedSeconds)).ElapsedSeconds;
                                distance = (float)distanceTrack.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i].ElapsedSeconds)).Value;
                            }
                            catch
                            {
                            }
                            PointF point = new PointF((float)Length.Convert(distance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits), value);
                            try
                            {
                                activityData.Points.Add((float)Length.Convert(distance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits), point);
                            }
                            catch { }
                        }
                    }*/

                    // Draw Distance based chart
                    //float min, max;
                    //INumericTimeDataSeries smoothedSpeed = Utilities.RemovePausedTimesInTrack(ai.SmoothedSpeedTrack, activity); // Utilities.RemovePausedTimesInTrack(Utilities.STSmooth(Utilities.GetSpeedTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.SpeedSmoothingSeconds, out min, out max), ai.Activity);
                    INumericTimeDataSeries distanceTrack = Utilities.RemovePausedTimesInTrack(Utilities.GetDistanceMovingTrack(ai.Activity), ai.Activity);
                    //INumericTimeDataSeries smoothedElevation = Utilities.RemovePausedTimesInTrack(ai.SmoothedElevationTrack, activity); // Utilities.RemovePausedTimesInTrack(Utilities.STSmooth(Utilities.GetElevationTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.ElevationSmoothingSeconds, out min, out max), ai.Activity);
                    INumericTimeDataSeries smoothedElevation = Utilities.RemovePausedTimesInTrack(Utilities.STSmooth(Utilities.GetElevationTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.ElevationSmoothingSeconds, out min, out max), ai.Activity);

                    distanceTrack = Utilities.SetTrackStartValueToZero(distanceTrack);

                    if (smoothedElevation != null && smoothedElevation.Count > 0)
                    {
                        for (int i = 0; i < distanceTrack.Count; i++)
                        {

                            DateTime time = smoothedElevation.EntryDateTime(distanceTrack[i]);
                            ITimeValueEntry<float> elevationEntry = smoothedElevation.GetInterpolatedValue(time);

                            // Get the value in m/s
                            //float value = smoothedElevation[i].Value;         //THIS WAS CRASHING ON UWE'S DELETED ELEVATION TRACK
                            float value = 0;
                            if (elevationEntry != null)
                            {
                                value = elevationEntry.Value;
                            }
                            ITimeValueEntry<float> distancePoint = distanceTrack.GetInterpolatedValue(time);
                            if (distancePoint != null)
                            {
                                // Convert the xaxis to be distance
                                distanceTotal = distancePoint.Value;
                                secondsLast = distancePoint.ElapsedSeconds;

                                float distance = (float)Length.Convert(distanceTotal, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits);
                                PointF point = new PointF(distance, value);
                                if (!activityData.Points.ContainsKey(point.X))
                                {
                                    activityData.Points.Add(point.X, point);
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (chartType == HillChartType.ClimbTime
                    || chartType == HillChartType.DescentTime
                    || chartType == HillChartType.SplitsTime)
                {
                    #region Time
                    INumericTimeDataSeries hillSeries = Utilities.RemovePausedTimesInTrack(ai.SmoothedElevationTrack, activity);
                    INumericTimeDataSeries distanceSeries = Utilities.RemovePausedTimesInTrack(ai.MovingDistanceMetersTrack, activity);
                    if (hillSeries.Count != 0)
                    {
                        for (int i = 0; i < hillSeries.Count; i++)
                        {
                            DateTime start = ai.ActualTrackStart;
                            float value = (float)Length.Convert(hillSeries[i].Value, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);
                            PointF point = new PointF(hillSeries[i].ElapsedSeconds, value);
                            try
                            {
                                activityData.Points.Add(hillSeries[i].ElapsedSeconds, point);
                            }
                            catch { }

                            float distance = 0;
                            try
                            {
                                distance = (float)distanceSeries.GetInterpolatedValue(start.AddSeconds(hillSeries[i].ElapsedSeconds)).Value;
                            }
                            catch { }
                        }
                    }
                    #endregion
                }


                activityData.ChartType = ChartDataSeries.Type.Fill;
                activityData.LineColor = Color.FromArgb(255, 146, 94, 9);
                activityData.FillColor = Color.FromArgb(75, 146, 94, 9);
                activityData.ValueAxis = MainChart.YAxis;

                if (chartType == HillChartType.ClimbTime
                    || chartType == HillChartType.DescentTime
                    || chartType == HillChartType.SplitsTime)
                {
                    MainChart.XAxis.Formatter = new Formatter.SecondsToTime();
                    MainChart.XAxis.Label = CommonResources.Text.LabelTime;
                }
                else if (chartType == HillChartType.ClimbDistance
                    || chartType == HillChartType.DescentDistance
                    || chartType == HillChartType.Overall
                    || chartType == HillChartType.SplitsDistance)
                {
                    MainChart.XAxis.Formatter = new Formatter.General();
                    MainChart.XAxis.Label = CommonResources.Text.LabelDistance + " (" + activity.Category.DistanceUnits + ")";
                }

                MainChart.DataSeries.Add(activityData);

                // Clear the hill markers
                MainChart.XAxis.Markers.Clear();

                foreach (ChartDataSeries c in cds)
                {
                    MainChart.DataSeries.Add(c);
                }

                if (autozoom)
                {
                    MainChart.AutozoomToData(true);
                }
            }
        }

        /// <summary>
        /// Draws a single highlighted hill on the MainChart.  Draws only the single hill.  
        /// Will remove it first if it already exists.
        /// Note that the ChartDataSeries will be tagged with HILL + (feature guid) for identification
        /// </summary>
        /// <param name="feature">Hill to draw</param>
        private void DrawHillProfile(Feature feature)
        {
            // TODO: THIS METHOD DOES NOT WORK AS-IS RIGHT NOW!!!
            ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(activity);

            // Setup data series
            ChartDataSeries cd = new ChartDataSeries(MainChart, MainChart.YAxis);
            cd.ChartType = ChartDataSeries.Type.Fill;
            cd.LineColor = feature.lineColor;
            cd.LineWidth = feature.lineWidth;
            cd.FillColor = feature.fillColor;
            cd.ValueAxis = MainChart.YAxis;
            cd.Data = "HILL" + feature.refId;

            float distanceTotal = 0;
            float secondsLast = 0;

            if (GlobalSettings.Instance.IsDistanceChart)
            {
                #region Distance

                INumericTimeDataSeries distanceTrack = Utilities.GetDistanceMovingTrack(activity);

                if (ai.SmoothedElevationTrack.Count != 0 && distanceTrack != null && distanceTrack.Count > 0)
                {
                    for (int i = 0; i < ai.SmoothedElevationTrack.Count; i++)
                    {
                        DateTime start = ai.SmoothedElevationTrack.StartTime;

                        // Get the value in m/s
                        float value = (float)Length.Convert(ai.SmoothedElevationTrack[i].Value, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);
                        float distance = 0;
                        secondsLast = 0;

                        // Convert the xaxis to be distance
                        ITimeValueEntry<float> speedPoint = ai.SmoothedSpeedTrack.GetInterpolatedValue(ai.SmoothedElevationTrack.EntryDateTime(ai.SmoothedElevationTrack[i]));
                        ITimeValueEntry<float> distancePoint = distanceTrack.GetInterpolatedValue(start.AddSeconds(ai.SmoothedElevationTrack[i].ElapsedSeconds));
                        if (speedPoint != null && distancePoint != null)
                        {
                            distanceTotal += speedPoint.Value * (speedPoint.ElapsedSeconds - secondsLast);
                            secondsLast = speedPoint.ElapsedSeconds;
                            distance = distancePoint.Value;
                        }

                        PointF point = new PointF((float)Length.Convert(distance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits), value);

                        if (distance >= feature.startDistance && distance <= feature.endDistance)
                        {
                            // Add points for hill
                            cd.Points.Add((float)Length.Convert(distanceTotal, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits), point);
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region Time
                INumericTimeDataSeries elevSeries = Utilities.RemovePausedTimesInTrack(ai.SmoothedElevationTrack, activity);
                INumericTimeDataSeries distanceSeries = Utilities.RemovePausedTimesInTrack(ai.MovingDistanceMetersTrack, activity);
                if (elevSeries.Count != 0)
                {
                    // Draw time based chart
                    foreach (TimeValueEntry<float> item in elevSeries)
                    {
                        ITimeValueEntry<float> distancePoint = distanceSeries.GetInterpolatedValue(elevSeries.EntryDateTime(item));

                        if (distancePoint != null)
                        {
                            float distance = (float)distanceSeries.GetInterpolatedValue(elevSeries.EntryDateTime(item)).Value;
                            if (distance >= feature.startDistance && distance <= feature.endDistance)
                            {
                                float elevation = (float)Length.Convert(item.Value, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);
                                PointF point = new PointF(item.ElapsedSeconds, elevation);
                                cd.Points.Add(point.X, point);
                            }
                        }
                    }
                }
                #endregion
            }


            // Clear the hill markers
            MainChart.XAxis.Markers.Clear();

            // Add dataseries
            MainChart.DataSeries.Add(cd);
            MainChart.XAxis.ChartLines = true;
        }

        /// <summary>
        /// Draw charts in normal activity mode
        /// </summary>
        /// <param name="inActivities"></param>
        /// <param name="autozoom"></param>
        private void DrawChartSelectedRecordsByActivity(IEnumerable<IActivity> inActivities, bool autozoom)
        {
            // Count the number of activities that use pace vs speed
            // We will display units based on the most popular
            int paceCount = 0;
            int speedCount = 0;

            // Find the most popular speed unit
            foreach (IActivity r in inActivities)
            {
                if (r != null)
                {
                    if (r.Category.SpeedUnits == Speed.Units.Pace)
                    {
                        paceCount++;
                    }
                    else if (r.Category.SpeedUnits == Speed.Units.Speed)
                    {
                        speedCount++;
                    }
                }
            }

            List<ChartField.Field> fields = new List<ChartField.Field>();
            if (mode == Mode.OneActivity)
            {
                fields = GlobalSettings.Instance.ChartFields;
            }
            else if (mode == Mode.MultipleActivities)
            {
                fields = GlobalSettings.Instance.MultiChartFields;
            }

            // Clear the YAxisRight before we add to it.
            // Elevation is always primary
            MainChart.YAxisRight.Clear();

            foreach (IActivity activity in inActivities)
            {
                if (activity != null)
                {
                    ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(activity);
                    foreach (ChartField.Field field in fields)
                    {
                        // Next iteration is a secondary axis
                        INumericTimeDataSeries series = null;
                        IAxis axis = new RightVerticalAxis(MainChart);
                        axis.Label = ChartField.ChartFieldsLookup(field);
                        axis.LabelColor = ChartField.ChartColorLookup(field);

                        switch (field)
                        {
                            case ChartField.Field.Elevation:
                                // NOTE: Elevation is not added here because it's always primary, and always added
                                break;

                            case ChartField.Field.Cadence:
                                series = ai.SmoothedCadenceTrack;
                                break;

                            case ChartField.Field.Grade:
                                // Grade isn't created properly in ActivityInfo for subActivities
                                // Check it and create it if necessary
                                if (ai.SmoothedGradeTrack.Count == 0)
                                {
                                    float min;
                                    float max;
                                    series = Utilities.GetPercentTrack(Utilities.STSmooth(Utilities.GetGradeTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.ElevationSmoothingSeconds, out min, out max));
                                }
                                else
                                {
                                    series = Utilities.GetPercentTrack(ai.SmoothedGradeTrack);
                                }
                                break;

                            case ChartField.Field.HR:
                                series = ai.SmoothedHeartRateTrack;
                                break;

                            case ChartField.Field.Power:
                                series = ai.SmoothedPowerTrack;
                                break;

                            case ChartField.Field.Speed:
                                // Speed isn't created properly in ActivityInfo for subActivities
                                // Check it and create it if necessary
                                if (ai.SmoothedSpeedTrack.Count == 0)
                                {
                                    float min;
                                    float max;
                                    series = Utilities.STSmooth(Utilities.GetSpeedTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.SpeedSmoothingSeconds, out min, out max);
                                }
                                else
                                {
                                    series = ai.SmoothedSpeedTrack;
                                }

                                // Convert track to local units (mph, pace, etc.)
                                series = Utilities.ConvertDistanceUnits(series, PluginMain.GetApplication().SystemPreferences.DistanceUnits);

                                if (paceCount > speedCount)
                                {
                                    // Convert to pace if required
                                    series = Utilities.SpeedToPace(series);
                                }
                                break;
                            case ChartField.Field.VAM:
                                //series = Utilities.Smooth(Utilities.GetVAMTrack(ai.Activity),
                                {
                                    float min;
                                    float max;
                                    series = Utilities.STSmooth(Utilities.GetVAMTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.SpeedSmoothingSeconds, out min, out max);
                                }
                                break;
                        }

                        // Draw series
                        int index = DrawSeriesGraph(series, GlobalSettings.Instance.IsDistanceChart, ai, axis, (paceCount + speedCount), ChartField.ChartColorLookup(field), false);
                        // MainChart.DataSeries[index].FillColor...

                        if (index != -1 && field == ChartField.Field.Speed && paceCount > speedCount)
                        {
                            // Setup Pace axis if appropriate
                            MainChart.DataSeries[index].ValueAxis.Formatter = new Formatter.SecondsToTime();
                            MainChart.DataSeries[index].ValueAxis.Label = CommonResources.Text.LabelPace;
                        }
                        else if (index != -1)
                        {
                            MainChart.DataSeries[index].ValueAxis.Formatter = new Formatter.General();
                        }
                    }
                }
            }

            if (autozoom)
            {
                MainChart.AutozoomToData(true);
            }
            MainChart.XAxis.ChartLines = true;
            MainChart.Refresh();
        }

        /// <summary>
        /// Draw charts in multi mode (search results from finding matches)
        /// </summary>
        /// <param name="features"></param>
        /// <param name="autozoom"></param>
        private void DrawChartSelectedRecordsByFeature(List<Feature> features, bool autozoom)
        {
            // Count the number of activities that use pace vs speed
            // We will display units based on the most popular
            int paceCount = 0;
            int speedCount = 0;

            // Find the most popular speed unit
            foreach (Feature f in features)
            {
                if (f.record.Activity != null)
                {
                    if (f.record.Activity.Category.SpeedUnits == Speed.Units.Pace)
                    {
                        paceCount++;
                    }
                    else if (f.record.Activity.Category.SpeedUnits == Speed.Units.Speed)
                    {
                        speedCount++;
                    }
                }
            }

            List<ChartField.Field> fields = new List<ChartField.Field>();

            if (mode == Mode.OneActivity)
            {
                fields = GlobalSettings.Instance.ChartFields;
            }
            else if (mode == Mode.MultipleActivities)
            {
                fields = GlobalSettings.Instance.MultiChartFields;
            }

            //***********************************************************************
            // Clear the YAxisRight before we add to it.  
            // First field listed is primary, so set axis to main YAxis and fill it.
            // It's later set to a new YRightAxis for secondary charts.
            MainChart.YAxisRight.Clear();
            IAxis axis = MainChart.YAxis;
            bool filled = true;

            foreach (ChartField.Field field in fields)
            {
                foreach (Feature feature in features)
                {
                    ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(feature.record.Activity);
                    if (feature.record.Activity != null)
                    {
                        INumericTimeDataSeries series = null;
                        axis.Label = ChartField.ChartFieldsLookup(field);
                        axis.LabelColor = ChartField.ChartColorLookup(field);

                        switch (field)
                        {
                            case ChartField.Field.Elevation:
                                series = new NumericTimeDataSeries(ai.SmoothedElevationTrack);
                                foreach (TimeValueEntry<float> item in series)
                                {
                                    item.Value = (float)Length.Convert(item.Value, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);
                                }
                                break;

                            case ChartField.Field.Cadence:
                                series = ai.SmoothedCadenceTrack;
                                break;

                            case ChartField.Field.Grade:
                                // Grade isn't created properly in ActivityInfo for subActivities
                                // Check it and create it if necessary
                                if (ai.SmoothedGradeTrack.Count == 0)
                                {
                                    float min;
                                    float max;
                                    series = Utilities.GetPercentTrack(Utilities.STSmooth(Utilities.GetGradeTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.ElevationSmoothingSeconds, out min, out max));
                                }
                                else
                                {
                                    series = Utilities.GetPercentTrack(ai.SmoothedGradeTrack);
                                }
                                break;

                            case ChartField.Field.HR:
                                series = ai.SmoothedHeartRateTrack;
                                break;

                            case ChartField.Field.Power:
                                series = ai.SmoothedPowerTrack;
                                break;

                            case ChartField.Field.Speed:
                                // Speed isn't created properly in ActivityInfo for subActivities
                                // Check it and create it if necessary
                                if (ai.SmoothedSpeedTrack.Count == 0)
                                {
                                    float min;
                                    float max;
                                    series = Utilities.STSmooth(Utilities.GetSpeedTrack(ai.Activity), PluginMain.GetApplication().SystemPreferences.AnalysisSettings.SpeedSmoothingSeconds, out min, out max);
                                }
                                else
                                {
                                    series = ai.SmoothedSpeedTrack;
                                }

                                // Convert track to local units (mph, pace, etc.)
                                series = Utilities.ConvertDistanceUnits(series, PluginMain.GetApplication().SystemPreferences.DistanceUnits);

                                if (paceCount > speedCount)
                                {
                                    // Convert to pace if required
                                    series = Utilities.SpeedToPace(series);
                                }

                                break;
                            case ChartField.Field.VAM:
                                series = Utilities.GetVAMTrack(ai.Activity);
                                break;
                        }

                        // Draw the graph
                        int index = DrawSeriesGraph(series, GlobalSettings.Instance.IsDistanceChart, ai, axis, (paceCount + speedCount), Color.FromArgb(100, ChartField.ChartColorLookup(field)), filled);
                        if (index != -1 && filled)
                        {
                            // Dim the fill color
                            MainChart.DataSeries[index].FillColor = Color.FromArgb(1 + 10 / (paceCount + speedCount), MainChart.DataSeries[index].FillColor);
                        }

                        if (index != -1 && field == ChartField.Field.Speed && paceCount > speedCount)
                        {
                            // Setup Pace axis if appropriate
                            MainChart.DataSeries[index].ValueAxis.Formatter = new Formatter.SecondsToTime();
                            MainChart.DataSeries[index].ValueAxis.Label = CommonResources.Text.LabelPace;
                        }

                        else if (index != -1)
                        {
                            MainChart.DataSeries[index].ValueAxis.Formatter = new Formatter.General();
                        }

                    }
                }

                // Next iteration is a secondary axis
                filled = false;
                axis = new RightVerticalAxis(MainChart);
            }

            if (autozoom)
            {
                MainChart.AutozoomToData(true);
            }
            MainChart.XAxis.ChartLines = true;
            MainChart.Refresh();
        }

        /// <summary>
        /// Highlight multimode chart lines for the selected activities
        /// </summary>
        /// <param name="features">Selected features to highlight.  All others will be dimmed.</param>
        private void HighlightChartLines(List<Feature> features)
        {
            // Multi-mode - Highlight data lines: ST colors if 1 activity selected, Rainbow colors if 2 or more selected
            foreach (ChartDataSeries series in MainChart.DataSeries)
            {
                bool found = false;

                foreach (Feature feature in features)
                {
                    // series.Data contains associated Activity's ReferenceId (Activity Id, not Feature Id)
                    if (feature != null && feature.record.Activity.ReferenceId == series.Data as string)
                    {
                        // NOTE: Selected feature using ST colors, or activity/feature colors
                        // Highlight selected activity features
                        if (treeList1.SelectedItems.Count > 1)
                        {
                            // Activity color (multiple selected)
                            series.LineColor = Color.FromArgb(255, feature.selectedColor);
                            series.LineWidth = 3;
                        }
                        else
                        {
                            // ST color (single feature selected)
                            series.LineColor = Color.FromArgb(255, series.ValueAxis.LabelColor);
                            series.LineWidth = 2;
                        }
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    // Reset/Dim unselected items
                    series.LineColor = Color.FromArgb(40, series.ValueAxis.LabelColor);
                    series.LineWidth = 1;
                }
            }

        }

        #endregion

        #region Tree Function

        private void RefreshColumns(TreeList tree)
        {
            tree.Columns.Clear();

            TreeColumnCollection savedColumns = new TreeColumnCollection();
            savedColumns = GlobalSettings.Instance.TreeColumns;

            // If no columns were saved, show the defaults
            if (savedColumns.Count == 0)
            {
                savedColumns = TreeColumn.DefaultColumns();
                GlobalSettings.Instance.TreeColumns = savedColumns;
            }

            // Build the column list
            IList<TreeList.Column> columns = new List<TreeList.Column>();

            for (int i = 0; i < savedColumns.Count; i++)
            {
                // If this is a speed/pace column, display the correct value
                if (savedColumns[i].treeColumn == TreeColumn.Column.AvgSpeed
                    || savedColumns[i].treeColumn == TreeColumn.Column.AvgPace)
                {
                    if (activity.Category.SpeedUnits == Speed.Units.Speed)
                    {
                        // Set this column as speed
                        savedColumns[i] = new TreeColumn(TreeColumn.Column.AvgSpeed, savedColumns[i].Width);
                    }
                    else if (activity.Category.SpeedUnits == Speed.Units.Pace)
                    {
                        // Set this column as pace
                        savedColumns[i] = new TreeColumn(TreeColumn.Column.AvgPace, savedColumns[i].Width);
                    }
                }

                // Add the column
                columns.Add(new TreeList.Column(savedColumns[i].Id, savedColumns[i].ToString(), savedColumns[i].Width, savedColumns[i].Align));
            }


            // Allow the columns to be selected
            foreach (TreeList.Column column in columns)
            {
                column.CanSelect = true;
            }

            // Display the columns 
            foreach (TreeList.Column column in columns)
            {
                tree.Columns.Add(column);
            }

            // Set the locked columns
            tree.NumLockedColumns = GlobalSettings.Instance.NumFixedColumns;

        }

        //private IList<TreeList.Column> GetTreeColumns()
        //{
        //    TreeColumn hillId = new TreeColumn(TreeColumn.Column.HillId, 40);

        //    // Display prefs for units (distance, pace, speed units, etc.)
        //    string distUnits = Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.DistanceUnits);
        //    string elevUnits = Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.ElevationUnits);

        //    IList<TreeList.Column> columns = new List<TreeList.Column>();

        //    columns.Add(new TreeList.Column(hillId.Id, hillId.ToString(), hillId.Width, hillId.Align));

        //    //columns.Add(new TreeList.Column("HillId", Resources.Strings.Label_HillId, 40, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("Date", CommonResources.Text.LabelStartTime, 120, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("Start", CommonResources.Text.LabelStart + " (" + distUnits + ")", 80, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("End", Resources.Strings.Label_End + " (" + distUnits + ")", 80, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("Distance", CommonResources.Text.LabelTotalDistance + " (" + distUnits + ")", 80, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("Time", CommonResources.Text.LabelTimeElapsed, 80, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("ElevGain", CommonResources.Text.LabelElevationChange + " (" + elevUnits + ")", 80, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("AvgGrade", CommonResources.Text.LabelAvgGrade, 60, StringAlignment.Near));

        //    if (activity.Category.SpeedUnits == Speed.Units.Speed)
        //    {
        //        columns.Add(new TreeList.Column("AvgSpeed", CommonResources.Text.LabelAvgSpeed, 80, StringAlignment.Near));
        //    }
        //    else if (activity.Category.SpeedUnits == Speed.Units.Pace)
        //    {
        //        columns.Add(new TreeList.Column("AvgPace", CommonResources.Text.LabelPace, 80, StringAlignment.Near));
        //    }

        //    columns.Add(new TreeList.Column("HR", CommonResources.Text.LabelAvgHR, 60, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("Cadence", CommonResources.Text.LabelAvgCadence, 60, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("Power", CommonResources.Text.LabelAvgPower, 60, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("HillScoreClimbByBike", ScoreEquation.ScoreEquationLookup(ScoreEquation.Score.ClimbByBike), 60, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("HillScoreCycle2Max", ScoreEquation.ScoreEquationLookup(ScoreEquation.Score.Cycle2Max), 60, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("HillScoreFiets", ScoreEquation.ScoreEquationLookup(ScoreEquation.Score.Fiets), 60, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("HillScoreCourseScoreCycling", ScoreEquation.ScoreEquationLookup(ScoreEquation.Score.CourseScoreCycling), 60, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("HillScoreCourseScoreRunning", ScoreEquation.ScoreEquationLookup(ScoreEquation.Score.CourseScoreRunning), 60, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("VAM", Resources.Strings.Label_VAM, 60, StringAlignment.Near));
        //    columns.Add(new TreeList.Column("WKg", Resources.Strings.Label_wKg, 60, StringAlignment.Near));

        //    foreach (TreeList.Column column in columns)
        //    {
        //        column.CanSelect = true;
        //    }

        //    return columns;
        //}

        /// <summary>
        /// RefreshTree will refresh the tree list with the provided features
        /// It will calcuate a hill id based on starting position.  If you iterate the same feature
        /// more than once, you will be able to tell based on hill id
        /// </summary>
        /// <param name="features">List of all features</param>
        private void RefreshTree(List<Feature> features)
        {
            // If the features list actually contains features
            if (features != null)
            {
                // Clear the columns and re-add them
                RefreshColumns(treeList1);
                RefreshColumns(treeList_subtotals);

                // Sort the tree list
                SortHillsTreeList(currentSortColumnId, false);

                // Remove the handler
                treeList1.SelectedItemsChanged -= treeList_SelectedChanged;

                // Add the custom handler
                treeList1.RowDataRenderer = new FeatureListRenderer(treeList1);

                // Add data to the treelist
                treeList1.RowData = features;

                // Build the subtotal row
                PopulateSubtotalRow();

                // Add the handler back
                treeList1.SelectedItemsChanged += treeList_SelectedChanged;

            }
        }

        /// <summary>
        /// Sort activities in TreeActivity table.
        /// </summary>
        /// <param name="columnId">Column to sort by.</param>
        /// <param name="reSort">Re-sort/invert sort of table.  For example, TRUE if column clicked, FALSE if maintain existing sort order.</param>
        private void SortHillsTreeList(string columnId, bool reSort)
        {
            // Exit if chart is empty
            if (treeList1.RowData != null)
            {
                // Sort only the data currently in the treeList
                List<Feature> data = (List<Feature>)treeList1.RowData;

                // Create a comparer instance
                Feature.FeatureComparer comparer = new Feature.FeatureComparer();

                // Set comparison method
                try
                {
                    comparer.ComparisonMethod = (Feature.FeatureComparer.ComparisonType)Enum.Parse(typeof(Feature.FeatureComparer.ComparisonType), columnId);
                }
                catch
                {
                    // Default to sort by start time if there's an error for some reason
                    comparer.ComparisonMethod = Feature.FeatureComparer.ComparisonType.Date;
                }

                // Set sort direction
                if ((reSort && !currentSortDirection && columnId == currentSortColumnId) || (!reSort && currentSortDirection))
                {
                    // Sort ascending - same column clicked, invert sort order OR don't resort and already Ascending
                    currentSortColumnId = columnId;
                    currentSortDirection = true; // Ascending = True, Descending = False
                    comparer.SortOrder = Feature.FeatureComparer.Order.Ascending;
                    treeList1.SetSortIndicator(columnId, true);
                }
                else
                {
                    // Sort descending
                    currentSortColumnId = columnId;
                    currentSortDirection = false; // Ascending = True, Descending = False
                    comparer.SortOrder = Feature.FeatureComparer.Order.Descending;
                    treeList1.SetSortIndicator(columnId, false);
                }

                // Sort and display activities
                data.Sort(comparer);
                treeList1.RowData = data;
            }
        }

        /// <summary>
        /// Gets a list of features selected in a treelist.  Returns an empty list if nothing selected.
        /// </summary>
        /// <param name="tree"></param>
        /// <returns>Returns a list of features or null if nothing selected.</returns>
        private List<Feature> GetSelectedFeatures(TreeList tree)
        {
            if (tree.SelectedItems == null || tree.SelectedItems.Count == 0)
            {
                return null;
            }

            List<Feature> features = new List<Feature>();
            foreach (object obj in tree.SelectedItems)
            {
                Feature feature = obj as Feature;
                features.Add(feature);
            }

            return features;
        }

        /// <summary>
        /// ClearChart will clear and refresh the chart.  Called from right click --> Clear
        /// </summary>
        public void ClearChart()
        {
            this.Cursor = Cursors.WaitCursor;
            zoomed = false;
            if (mode == Mode.OneActivity)
            {
                // Commented code below only works to clear when unzoomed.  When you try to clear from
                // "double click zoom" it doesn't work
                // Leaving the code here for now.  I may use it elsewhere.

                // Single Activity Clear - Reset hill highlight colors
                //List<Feature> features = treeList1.RowData as List<Feature>;

                //foreach (ChartDataSeries series in MainChart.DataSeries)
                //{
                //    string id = series.Data as string;

                //    foreach (Feature feature in features)
                //    {
                //        if (!string.IsNullOrEmpty(id) && id == "HILL" + feature.refId)
                //        {
                //            // Set colors to proper highlight colors
                //            series.LineColor = feature.lineColor;
                //            series.FillColor = feature.fillColor;
                //            break;
                //        }
                //    }
                //}

                //MainChart.Refresh();


                treeList1.SelectedItemsChanged -= treeList_SelectedChanged;
                treeList1.Selected = null;
                treeList1.SelectedItemsChanged += treeList_SelectedChanged;

                RefreshPage(true);
            }
            else
            {
                // Switch to single activity mode
                SwitchToSingleMode();
            }
            this.Cursor = Cursors.Default;
        }

        #endregion

        #region Map Functions

        /// <summary>
        /// DrawMap will draw the supplied feature to the map based on the feature's linecolor
        /// </summary>
        /// <param name="feature">Feature to draw to the map</param>
        private void DrawMap(Feature feature, bool clear)
        {
            if (feature != null && feature.startPoint != null)
            {
                // Move the map to this feature's location
                GPSLocation location = new GPSLocation(feature.startPoint.LatitudeDegrees, feature.startPoint.LongitudeDegrees);
                layer.MoveMap((IGPSLocation)location);

                // If you want to clear the features, do so
                if (clear)
                {
                    layer.ClearFeatures();

                    // Add and draw the feature
                    layer.HighlightRadius = 3;
                    layer.AddFeature(feature);
                    layer.DrawFeatures(clear);
                    layer.ShowPage = true;
                }
                else
                {
                    layer.HighlightFeature(feature);
                    layer.ShowPage = true;
                }
            }
        }

        /// <summary>
        /// DrawMap will draw the supplied features to the map based on the features' linecolor
        /// </summary>
        /// <param name="features">Features to draw to the map</param>
        private void DrawMap(List<Feature> features, bool clear)
        {
            if (features != null)
            {
                // If you want to clear the features, do so
                if (clear)
                {
                    layer.ClearFeatures();

                    // Add and draw the features
                    layer.HighlightRadius = 3;
                    foreach (Feature feature in features)
                    {
                        layer.AddFeature(feature);
                    }
                    layer.DrawFeatures(clear);
                    layer.ShowPage = true;

                }
                else
                {
                    layer.HighlightFeature(features);
                    layer.ShowPage = true;
                }
            }
        }

        private void ClearMap()
        {
            layer.ClearFeatures();
        }

        #endregion

        #region Event Handlers

        private void ChartBanner_MenuClicked(object sender, EventArgs e)
        {
            ChartBanner.ContextMenuStrip.Width = 100;
            ChartBanner.ContextMenuStrip.Show(ChartBanner.Parent.PointToScreen(new System.Drawing.Point(ChartBanner.Right - ChartBanner.ContextMenuStrip.Width - 2, ChartBanner.Bottom + 1)));
        }

        private void InfoBanner_MenuClicked(object sender, EventArgs e)
        {
            ThemedContextMenuStripRenderer contextMenu = new ThemedContextMenuStripRenderer(PluginMain.GetApplication().VisualTheme);
            infoBanner.ContextMenuStrip.Width = 100;
            infoBanner.ContextMenuStrip.Show(infoBanner.Parent.PointToScreen(new System.Drawing.Point(infoBanner.Right - infoBanner.ContextMenuStrip.Width - 2, infoBanner.Bottom + 1)));
        }

        /// <summary>
        /// Menu Item selected from chart action banner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detailMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem selected = sender as ToolStripMenuItem;

            for (int i = 0; i < detailMenu.Items.Count; i++)
            {
                ToolStripMenuItem item = detailMenu.Items[i] as ToolStripMenuItem;

                if (item != null)
                {
                    if (item != selected)
                    {
                        item.Checked = false;
                    }
                    else
                    {
                        item.Checked = true;
                    }
                }
                else
                {
                    // ToolStrip Separator encountered.  Stop evaluating
                    break;
                }
            }

            GlobalSettings.Instance.ChartType = (HillChartType)selected.Tag;
            detailMenu.Text = selected.Text;

            if (mode == Mode.MultipleActivities)
            {
                // Switch to single activity mode
                SwitchToSingleMode();
            }
            else if (mode == Mode.OneActivity)
            {
                RefreshPage(true);
            }
        }

        /// <summary>
        /// Menu item selected from upper action banner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void infoMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem selected = sender as ToolStripMenuItem;

            for (int i = 0; i < infoMenu.Items.Count; i++)
            {
                ToolStripMenuItem item = infoMenu.Items[i] as ToolStripMenuItem;

                if (item != null)
                {
                    if (item != selected)
                    {
                        item.Checked = false;
                    }
                    else
                    {
                        item.Checked = true;
                    }
                }
                else
                {
                    // ToolStrip Separator encountered.  Stop evaluating
                    break;
                }
            }
            GlobalSettings.Instance.InfoType = (InfoType)selected.Tag;

            if (mode == Mode.MultipleActivities)
            {
                // Switch to single activity mode
                SwitchToSingleMode();
            }
            else if (mode == Mode.OneActivity)
            {
                RefreshPage(false);
            }
        }

        private void treeList_ColumnClicked(object sender, TreeList.ColumnEventArgs e)
        {
            SortHillsTreeList(e.Column.Id, true);
        }

        private void Settings_click(object sender, EventArgs e)
        {

            SettingsPopup popup = new SettingsPopup();
            if (popup.ShowDialog() == DialogResult.OK)
            {
                GlobalSettings.Instance.DistancePercent = popup.distancePercent;
                GlobalSettings.Instance.ElevationPercent = popup.elevationPercent;
                GlobalSettings.Instance.GainElevationRequired = Length.Convert(popup.gainElevationRequired, PluginMain.GetApplication().SystemPreferences.ElevationUnits, Length.Units.Meter);
                GlobalSettings.Instance.HillDistanceRequired = Length.Convert(popup.hillDistanceRequired, PluginMain.GetApplication().SystemPreferences.DistanceUnits, Length.Units.Meter);
                GlobalSettings.Instance.MaxDescentElevation = Length.Convert(popup.maxDescentElevation, PluginMain.GetApplication().SystemPreferences.ElevationUnits, Length.Units.Meter);
                GlobalSettings.Instance.MaxDescentLength = Length.Convert(popup.maxDescentLength, PluginMain.GetApplication().SystemPreferences.DistanceUnits, Length.Units.Meter);
                GlobalSettings.Instance.MinAvgGrade = popup.minAvgGrade;
                RefreshPage(true);
            }
        }

        private void treeList_click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            TreeList tree = (TreeList)sender;

            if (mouse.Button == MouseButtons.Right && mouse.Y > tree.HeaderRowHeight)
            {
                HillChartType chartType = GlobalSettings.Instance.ChartType;

                if (chartType == HillChartType.ClimbDistance
                    || chartType == HillChartType.ClimbTime
                    || chartType == HillChartType.DescentDistance
                    || chartType == HillChartType.DescentTime
                    || chartType == HillChartType.Overall)
                {
                    // If the right click menu is hill based
                    contextMenuStrip.Show(this, new Point(mouse.X, mouse.Y));
                }
                else if (chartType == HillChartType.SplitsDistance
                    || chartType == HillChartType.SplitsTime)
                {
                    // If the right click menu is split based
                    splitsContextMenuStrip.Show(this, new Point(mouse.X, mouse.Y));
                }
            }
            else if (mouse.Button == MouseButtons.Right && mouse.Y <= tree.HeaderRowHeight)
            {
                treeMenu.Show(this, new Point(mouse.X, mouse.Y));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            TreeList tree = (TreeList)sender;

            if (mouse.Button == MouseButtons.Left && (mouse.Y > tree.HeaderRowHeight || Cursor.Current != Cursors.Hand))
            {
                if (tree.SelectedItems.Count > 0 && tree.SelectedItems[0] != null)
                {
                    if (mode == Mode.MultipleActivities)
                    {
                        Feature feature = tree.SelectedItems[0] as Feature;
                        if (feature != null)
                        {
                            string bookmark = "id=" + feature.masterActivityID;
                            PluginMain.GetApplication().ShowView(GUIDs.DailyActivityView, bookmark);
                        }
                    }
                    else
                    {
                        zoomed = true;
                        MainChart.DataSeries.Clear();
                        Feature oneFeature = (Feature)tree.Selected[0];
                        DrawElevationProfile(oneFeature, true);
                        List<IActivity> actList = new List<IActivity>();
                        actList.Add(oneFeature.record.Activity);
                        DrawChartSelectedRecordsByActivity(actList, true);

                        List<Feature> oneFeatureList = new List<Feature>();
                        oneFeatureList.Add(oneFeature);
                        RedrawMap(oneFeatureList);
                    }
                }
            }
        }

        private void treeList_SelectedChanged(object sender, EventArgs e)
        {
            TreeList tree = (TreeList)sender;

            if (tree.Selected[0] != null)
            {
                zoomed = false;

                // TODO: Filter this so that the map isn't refreshed on every click in 'Find this exact feature'
                RedrawMap(null);

                HideUnselectedHills(tree);

            }
        }

        private void contextMenuStrip_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem selected = sender as ToolStripMenuItem;
            if ((RightClickMenu)selected.Tag == RightClickMenu.Clear)
            {
                ClearChart();
            }
            else if ((RightClickMenu)selected.Tag == RightClickMenu.ExactHill)
            {
                #region Exact Hill
                // Close this context strip
                contextMenuStrip.Close();

                // Double check that something is selected, if not, exit
                if (treeList1.SelectedItems.Count == 0)
                {
                    return;
                }

                // Build the progress bar
                PopupProgress prog = new PopupProgress();
                prog.UpdateProgress(0, string.Empty);
                prog.Show();
                prog.BringToFront();

                // Setup variable to adjust progress bar's value
                int totalSteps = 0;
                int currentStep = 1;

                // Get all activities
                IEnumerable<IActivity> allActivities = PluginMain.GetApplication().Logbook.Activities;

                // Setup variable to pull out only activities that belong to the selected category
                IActivityCategory category = PluginMain.GetApplication().DisplayOptions.SelectedCategoryFilter;
                IList<IActivity> filteredActivities = new List<IActivity>();
                ActivityInfoCache info = ActivityInfoCache.Instance;
                IActivityCategory activityCategory;

                // Parse all activities 
                foreach (IActivity thisActivity in allActivities)
                {
                    activityCategory = info.GetInfo(thisActivity).Activity.Category;

                    // Search through the categories for a match
                    while (true)
                    {
                        if (activityCategory == category)
                        {
                            filteredActivities.Add(info.GetInfo(thisActivity).Activity);
                            break;
                        }
                        else if (activityCategory.Parent != null)
                        {
                            activityCategory = activityCategory.Parent;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // Update progress bar's total steps
                totalSteps = filteredActivities.Count;

                // Set the selected feature based on the current selection              
                Feature selectedFeature = treeList1.Selected[0] as Feature;

                // If we have something selected
                if (selectedFeature != null)
                {
                    // Check to make sure we aren't trying to match a hill without GPS
                    if (selectedFeature.startPoint == null)
                    {
                        prog.Dispose();
                        Application.DoEvents();
                        MessageBox.Show(Resources.Strings.Message_NoGPS);
                        return;
                    }
                    List<Feature> allFeatures = new List<Feature>();
                    foreach (IActivity act in filteredActivities)
                    {
                        prog.UpdateProgress((float)currentStep / (float)totalSteps, Resources.Strings.Message_ExportingElevation + " " + act.StartTime.ToString());
                        currentStep++;
                        Application.DoEvents();

                        // Let's not blow up if we hit an activity we can't parse.  Display an error instead
                        try
                        {
                            List<Feature> foundFeatures = FindThisFeature(selectedFeature, act);
                            allFeatures.AddRange(foundFeatures);
                        }
                        catch
                        {
                            MessageBox.Show(Resources.Strings.Message_ErrorExporting + " " + info.GetInfo(act).ActualTrackStart.ToString());
                            break;
                        }
                    }

                    // Switch to mulitple activity mode
                    SwitchToMultiMode(allFeatures);

                    prog.Dispose();
                    Application.DoEvents();
                    MainChart.Refresh();
                }

                #endregion
            }
            else if ((RightClickMenu)selected.Tag == RightClickMenu.LikeHill)
            {
                #region Like Hill

                // Close this context strip
                contextMenuStrip.Close();

                // Double check that something is selected, if not, exit
                if (treeList1.SelectedItems.Count == 0)
                {
                    return;
                }

                // Build the progress bar
                PopupProgress prog = new PopupProgress();
                prog.UpdateProgress(0, string.Empty);
                prog.Show();
                prog.BringToFront();

                // Setup variable to adjust progress bar's value
                int totalSteps = 0;
                int currentStep = 1;

                // Get all activities
                IEnumerable<IActivity> allActivities = PluginMain.GetApplication().Logbook.Activities;

                // Setup variable to pull out only activities that belong to the selected category
                IActivityCategory category = PluginMain.GetApplication().DisplayOptions.SelectedCategoryFilter;
                IList<IActivity> filteredActivities = new List<IActivity>();
                ActivityInfoCache info = ActivityInfoCache.Instance;
                IActivityCategory activityCategory;

                // Parse all activities 
                foreach (IActivity thisActivity in allActivities)
                {
                    activityCategory = info.GetInfo(thisActivity).Activity.Category;

                    // Search through the categories for a match
                    while (true)
                    {
                        if (activityCategory == category)
                        {
                            filteredActivities.Add(info.GetInfo(thisActivity).Activity);
                            break;
                        }
                        else if (activityCategory.Parent != null)
                        {
                            activityCategory = activityCategory.Parent;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // Update progress bar's total steps
                totalSteps = filteredActivities.Count;

                // Set the selected feature based on the current selection              
                Feature selectedFeature = (Feature)treeList1.Selected[0];

                // If we have something selected
                if (selectedFeature != null)
                {
                    List<Feature> allFeatures = new List<Feature>();
                    foreach (IActivity act in filteredActivities)
                    {
                        prog.UpdateProgress((float)currentStep / (float)totalSteps, Resources.Strings.Message_ExportingElevation + " " + info.GetInfo(act).ActualTrackStart.ToString());
                        currentStep++;
                        Application.DoEvents();

                        // Let's not blow up if we hit an activity we can't parse.  Display an error instead
                        try
                        {
                            List<Feature> foundFeatures = FindLikeFeatures(selectedFeature, act);
                            allFeatures.AddRange(foundFeatures);
                        }
                        catch
                        {
                            MessageBox.Show(Resources.Strings.Message_ErrorExporting + " " + info.GetInfo(act).ActualTrackStart.ToString());
                            break;
                        }
                    }

                    // Switch to mulitple activity mode
                    SwitchToMultiMode(allFeatures);

                    prog.Dispose();
                    Application.DoEvents();
                    MainChart.Refresh();
                }
                #endregion
            }
        }

        private void zoomIn_Click(object sender, EventArgs e)
        {
            MainChart.ZoomIn();
            MainChart.Focus();
        }

        private void zoomOut_Click(object sender, EventArgs e)
        {
            MainChart.ZoomOut();
            MainChart.Focus();
        }

        private void ZoomFitButton_Click(object sender, EventArgs e)
        {
            MainChart.AutozoomToData(false);
            MainChart.Focus();
        }

        private void SaveImageButton_Click(object sender, EventArgs e)
        {
            ITheme theme = PluginMain.GetApplication().VisualTheme;

            SaveImageDialog save = new SaveImageDialog();
            save.ThemeChanged(theme);

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filename = Resources.Strings.Label_CourseScore + " " + DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            save.FileName = Path.Combine(folder, filename);

            if (save.ShowDialog() == DialogResult.OK)
            {
                if (System.IO.File.Exists(save.FileName))
                {
                    if (MessageDialog.Show("File exists.  Overwrite?", "File Save", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                MainChart.SaveImage(save.ImageSizes[save.ImageSize], save.FileName, save.ImageFormat);
            }

            save.Dispose();
        }

        /// <summary>
        /// Handles the "More Charts" button from chart toolbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExtraCharts_click(object sender, EventArgs e)
        {
            List<ChartField> selected = new List<ChartField>();
            List<ChartField> unselected = new List<ChartField>();
            List<ChartField.Field> savedFields = new List<ChartField.Field>();

            if (mode == Mode.OneActivity)
            {
                savedFields = GlobalSettings.Instance.ChartFields;
            }
            else if (mode == Mode.MultipleActivities)
            {
                savedFields = GlobalSettings.Instance.MultiChartFields;
            }

            foreach (ChartField.Field field in System.Enum.GetValues(typeof(ChartField.Field)))
            {
                // If we are on oneActivity mode, don't include elevation.  It must be charted
                if (mode == Mode.OneActivity
                    && field != ChartField.Field.Elevation)
                {
                    if (!savedFields.Contains(field))
                    {
                        unselected.Add(new ChartField(field));
                    }
                }
                else if (mode == Mode.MultipleActivities)
                {
                    if (!savedFields.Contains(field))
                    {
                        unselected.Add(new ChartField(field));
                    }
                }
            }

            foreach (ChartField.Field field in savedFields)
            {
                // If we are on oneActivity mode, don't include elevation.  It must be charted
                if (mode == Mode.OneActivity && field != ChartField.Field.Elevation)
                {
                    selected.Add(new ChartField(field));
                }
                else if (mode == Mode.MultipleActivities)
                {
                    selected.Add(new ChartField(field));
                }
            }

            DataSelectForm data = new DataSelectForm(PluginMain.GetApplication().VisualTheme, selected, unselected);
            if (data.ShowDialog() == DialogResult.OK)
            {
                selected = data.Items;
                List<ChartField.Field> storeFields = new List<ChartField.Field>();
                foreach (ChartField field in selected)
                {
                    storeFields.Add(field.chartField);
                }
                if (mode == Mode.OneActivity)
                {
                    GlobalSettings.Instance.ChartFields = storeFields;
                }
                else if (mode == Mode.MultipleActivities)
                {
                    GlobalSettings.Instance.MultiChartFields = storeFields;
                }

                RedrawGraph();

                //RefreshPage(false);
                data.Dispose();
            }
        }

        private void splitterMoved(object sender, SplitterEventArgs e)
        {
            SplitContainer sc = (SplitContainer)sender;
            GlobalSettings.Instance.SplitterDistance = sc.SplitterDistance;
        }

        private void splitter_mouseDown(object sender, MouseEventArgs e)
        {
            // Don't add the splitterMoved handler until we click
            splitContainer1.SplitterMoved += splitterMoved;

            // We've clicked, we can get rid of this
            splitContainer1.MouseDown -= splitter_mouseDown;
        }

        private void score_click(object sender, EventArgs e)
        {
            button_courseScore.ContextMenuStrip.Width = 100;
            button_courseScore.ContextMenuStrip.Show(button_courseScore.Parent.PointToScreen(new System.Drawing.Point(button_courseScore.Right, button_courseScore.Bottom + 1)));
        }

        private void scoreMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem selected = sender as ToolStripMenuItem;

            for (int i = 0; i < scoreMenu.Items.Count; i++)
            {
                ToolStripMenuItem item = scoreMenu.Items[i] as ToolStripMenuItem;

                if (item != null)
                {
                    if (item != selected)
                    {
                        item.Checked = false;
                    }
                    else
                    {
                        item.Checked = true;
                    }
                }
                else
                {
                    // ToolStrip Separator encountered.  Stop evaluating
                    break;
                }
            }
            GlobalSettings.Instance.ScoreType = (ScoreType)selected.Tag;

            // Only show the correct course score on screen
            if (GlobalSettings.Instance.ScoreType == ScoreType.Cycling)
            {
                label_courseScore.Text = Resources.Strings.Label_CourseScore + " (" + Resources.Strings.Label_Cycling + ")";
                PopulateCyclingScores(activity);
            }
            else if (GlobalSettings.Instance.ScoreType == ScoreType.Running)
            {
                label_courseScore.Text = Resources.Strings.Label_CourseScore + " (" + Resources.Strings.Label_Running + ")";
                PopulateRunningScores(activity);
            }

            //RefreshPage(false);
        }

        /// <summary>
        /// Open Color Wheel Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Color_click(object sender, EventArgs e)
        {
            button_color.ContextMenuStrip.Width = 100;
            button_color.ContextMenuStrip.Show(button_color.Parent.PointToScreen(new System.Drawing.Point(button_color.Right, button_color.Bottom + 1)));
        }

        private void colorMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem selected = sender as ToolStripMenuItem;

            for (int i = 0; i < colorMenu.Items.Count; i++)
            {
                ToolStripMenuItem item = colorMenu.Items[i] as ToolStripMenuItem;

                if (item != null)
                {
                    if (item != selected)
                    {
                        item.Checked = false;
                    }
                    else
                    {
                        item.Checked = true;
                    }
                }
                else
                {
                    // ToolStrip Separator encountered.  Stop evaluating
                    break;
                }
            }

            GlobalSettings.Instance.ColorType = (ColorType)selected.Tag;

            RefreshPage(true);
        }

        private void MaximizeButton_Click(object sender, EventArgs e)
        {
            Maximize.Invoke(sender, e);
            if (maximized)
            {
                this.MaximizeButton.CenterImage = CommonResources.Images.View3PaneLowerLeft16;
            }
            else
            {
                this.MaximizeButton.CenterImage = CommonResources.Images.View2PaneLowerHalf16;

            }
        }

        private void MainChart_click(object sender, EventArgs e)
        {
            Type t = sender.GetType();
            System.Reflection.FieldInfo o = t.GetField("selectRangeData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetField);
            ChartDataSeries rangeSeries = (ChartDataSeries)o.GetValue(sender);

            if (rangeSeries != null)
            {
                t = rangeSeries.GetType();
                System.Reflection.FieldInfo start = t.GetField("xSelectedStart", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetField);
                System.Reflection.FieldInfo end = t.GetField("xSelectedEnd", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetField);
                float chartIndexLeft = (float)start.GetValue(rangeSeries);
                float chartIndexRight = (float)end.GetValue(rangeSeries);

                if (!float.IsNaN(chartIndexLeft))
                {
                    // Convert the left side to meters
                    chartIndexLeft = (float)Length.Convert(chartIndexLeft, PluginMain.GetApplication().SystemPreferences.DistanceUnits, Length.Units.Meter);

                    bool onePoint = false;

                    if (float.IsNaN(chartIndexRight))
                    {
                        // Single point selected
                        chartIndexRight = chartIndexLeft;
                        onePoint = true;
                    }
                    else
                    {
                        // Convert the right side to meters
                        chartIndexRight = (float)Length.Convert(chartIndexRight, PluginMain.GetApplication().SystemPreferences.DistanceUnits, Length.Units.Meter);
                        onePoint = false;
                    }

                    List<Feature> features = treeList1.RowData as List<Feature>;
                    List<Feature> selectedFeatures = new List<Feature>();

                    if (mode == Mode.OneActivity)
                    {
                        foreach (Feature feature in features)
                        {
                            // If only 1 point is clicked, add that feature
                            if (onePoint)
                            {
                                if (feature.startDistance <= chartIndexLeft
                                    && feature.endDistance >= chartIndexRight)
                                {
                                    selectedFeatures.Add(feature);
                                }
                            }
                            else
                            {
                                // Handle incomplete hill selections

                                // If this is the left most hill in the selection
                                if (feature.startDistance <= chartIndexLeft
                                    && feature.endDistance >= chartIndexLeft)
                                {
                                    selectedFeatures.Add(feature);
                                }

                                // If this is the right most hill in the selection
                                if (feature.startDistance <= chartIndexRight
                                    && feature.endDistance >= chartIndexRight)
                                {
                                    selectedFeatures.Add(feature);
                                }

                                // If this is a hill in between the selection
                                if (feature.startDistance >= chartIndexLeft
                                    && feature.endDistance <= chartIndexRight)
                                {
                                    selectedFeatures.Add(feature);
                                }
                            }
                        }
                    }
                    else if (mode == Mode.MultipleActivities && GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.Details)
                    {
                        foreach (Feature feature in features)
                        {
                            if ((string)rangeSeries.Data == feature.record.Activity.ReferenceId)
                            {
                                selectedFeatures.Add(feature);
                            }
                        }
                    }
                    else if (mode == Mode.MultipleActivities)
                    {
                        foreach (Feature feature in features)
                        {
                            if ((string)rangeSeries.Data == feature.refId)
                            {
                                selectedFeatures.Add(feature);
                            }
                        }
                    }

                    // Add the selected features
                    treeList1.SelectedItemsChanged -= treeList_SelectedChanged;
                    treeList1.Selected = selectedFeatures;
                    RedrawMap(null);
                    HideUnselectedHills(treeList1);
                    treeList1.SelectedItemsChanged += treeList_SelectedChanged;
                }
            }
            else
            {
                // Select nothing
                treeList1.SelectedItemsChanged -= treeList_SelectedChanged;
                treeList1.Selected = null;
                treeList1.SelectedItemsChanged += treeList_SelectedChanged;
                RedrawMap(null);
            }
        }

        private void TreeListMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem selected = sender as ToolStripMenuItem;
            if ((TreeListRightClickMenu)selected.Tag == TreeListRightClickMenu.ListSettings)
            {
                // Pull the saved columns from the settings
                TreeColumnCollection savedColumns = new TreeColumnCollection();
                savedColumns = GlobalSettings.Instance.TreeColumns;

                // Add the column ids to an array to show in the dialog
                List<string> selectedColumns = new List<string>();
                foreach (TreeColumn col in savedColumns)
                {
                    selectedColumns.Add(col.Id);
                }

                // Pull the list of all available columns
                List<IListColumnDefinition> allColumns = new List<IListColumnDefinition>();
                allColumns = (List<IListColumnDefinition>)TreeColumn.ColumnDefs(activity);

                // Create the dialog to show the List Settings options
                ListSettingsDialog dialog = new ListSettingsDialog();
                dialog.AvailableColumns = allColumns;
                dialog.SelectedColumns = selectedColumns;
                dialog.AllowFixedColumnSelect = true;
                dialog.NumFixedColumns = GlobalSettings.Instance.NumFixedColumns;
                dialog.ThemeChanged(PluginMain.GetApplication().VisualTheme);

                // Display the dialog and wait for a return value
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    IList<string> userSelectedColumns = dialog.SelectedColumns;
                    GlobalSettings.Instance.NumFixedColumns = dialog.NumFixedColumns;
                    TreeColumnCollection userSaveColumns = new TreeColumnCollection();
                    foreach (string str in userSelectedColumns)
                    {
                        bool found = false;
                        foreach (TreeColumn col in savedColumns)
                        {
                            if (col.Id == str)
                            {
                                userSaveColumns.Add(col);
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            userSaveColumns.Add(new TreeColumn(TreeColumn.ReverseIdColumnLookup(str), 120));
                        }
                    }
                    GlobalSettings.Instance.TreeColumns = userSaveColumns;
                    RefreshColumns(treeList1);
                    RefreshColumns(treeList_subtotals);
                }
            }
        }

        private void treeList_ColumnResized(object sender, TreeList.ColumnEventArgs e)
        {
            // Pull the saved columns from the settings
            TreeColumnCollection savedColumns = new TreeColumnCollection();
            savedColumns = GlobalSettings.Instance.TreeColumns;
            for (int i = 0; i < savedColumns.Count; i++)
            {
                // Find the column that was adjusted and reset it's width
                if (savedColumns[i].Id == e.Column.Id)
                {
                    savedColumns[i].Width = e.Column.Width;
                    break;
                }
            }

            // Write the columns list back to the settings
            GlobalSettings.Instance.TreeColumns = savedColumns;

            // Refresh the trees
            RefreshDetailsTreeLists();
            //PopulateSubtotalRow();

        }

        void event_RouteControlSelectedItemsChanged(object sender, EventArgs e)
        {
            IRouteControl route = sender as IRouteControl;
            List<Feature> selectedFeatures = new List<Feature>();

            if (route != null)
            {
                IList<IRouteControlSelection> selected = route.SelectedItems;
                foreach (IRouteControlSelection item in selected)
                {
                    IList<IValueRange<double>> ranges = item.DistanceMetersRanges;
                    foreach (IValueRange<double> range in ranges)
                    {
                        bool onePoint = false;
                        if (range.Lower == range.Upper)
                        {
                            onePoint = true;
                        }

                        List<Feature> features = treeList1.RowData as List<Feature>;
                        foreach (Feature feature in features)
                        {
                            // If only 1 point is clicked, add that feature
                            if (onePoint)
                            {
                                if (feature.startDistance <= range.Lower
                                    && feature.endDistance >= range.Upper)
                                {
                                    selectedFeatures.Add(feature);
                                }
                            }
                            else
                            {
                                // Handle incomplete hill selections

                                // If this is the left most hill in the selection
                                if (feature.startDistance <= range.Lower
                                    && feature.endDistance >= range.Lower)
                                {
                                    selectedFeatures.Add(feature);
                                }

                                // If this is the right most hill in the selection
                                if (feature.startDistance <= range.Upper
                                    && feature.endDistance >= range.Upper)
                                {
                                    selectedFeatures.Add(feature);
                                }

                                // If this is a hill in between the selection
                                if (feature.startDistance >= range.Lower
                                    && feature.endDistance <= range.Upper)
                                {
                                    selectedFeatures.Add(feature);
                                }
                            }
                        }
                    }
                }

                // Add the selected features
                treeList1.SelectedItemsChanged -= treeList_SelectedChanged;
                treeList1.Selected = selectedFeatures;
                RedrawMap(null);
                //RedrawGraph();
                HideUnselectedHills(treeList1);
                treeList1.SelectedItemsChanged += treeList_SelectedChanged;
            }
        }

        private void HillMarkersButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            bool markers = GlobalSettings.Instance.HillMarkers;

            if (markers == true)
            {
                GlobalSettings.Instance.HillMarkers = false;
                // Clear the hill markers
                if (MainChart.DataSeries != null)
                {
                    if (MainChart.XAxis.Markers.Count > 0)
                    {
                        MainChart.XAxis.Markers.Clear();
                    }
                }
            }
            else
            {
                GlobalSettings.Instance.HillMarkers = true;
                if (MainChart.DataSeries.Count > 0)
                {
                    // Draw the hill markers
                    RefreshPage(true);
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void TreeRefreshButton_Click(object sender, EventArgs e)
        {
            ClearChart();
        }

        #endregion

        #region Hill Scoring Algorithms

        // Cycle2Max algorithm
        public void ScoreHillCycle2Max(Feature feature)
        {
            feature.hillScoreCycle2Max = (feature.elevGain / feature.distance) * 400 + ((feature.elevGain * feature.elevGain) / feature.distance) + (feature.distance / 1000);
        }

        // ClimbByBike algorithm
        public void ScoreHillClimbByBike(Feature feature)
        {
            feature.hillScoreClimbByBike = (feature.elevGain / feature.distance) * 400 + ((feature.elevGain * feature.elevGain) / feature.distance) + (feature.distance / 1000) + ((feature.endElevation - 1000) / 100);
        }

        // FIETS-index
        public void ScoreHillFiets(Feature feature)
        {
            if (feature.endElevation > 1000)
            {
                feature.hillScoreFiets = ((feature.elevGain * feature.elevGain) / (feature.distance * 10)) + ((feature.endElevation - 1000) / 1000);
            }
            else
            {
                feature.hillScoreFiets = ((feature.elevGain * feature.elevGain) / (feature.distance * 10));
            }
        }

        // TRIMP style for running
        public void ScoreHillCourseScoreRunning(Feature feature)
        {
            feature.hillScoreCourseScoreRunning = CalculateCourseScoreRunning(feature.record.Activity);
        }

        // TRIMP style for cycling
        public void ScoreHillCourseScoreCycling(Feature feature)
        {
            feature.hillScoreCourseScoreCycling = CalculateCourseScoreCycling(feature.record.Activity);
        }

        // Hill Category finder
        public void ScoreHillCategory(Feature feature)
        {
            double stravaMath = 0;
            stravaMath = (feature.avgGrade * 100) * feature.distance;

            if (stravaMath >= 80000)
            {
                feature.hillCategory = "HC";
            }
            else if (stravaMath >= 64000)
            {
                feature.hillCategory = "1";
            }
            else if (stravaMath >= 32000)
            {
                feature.hillCategory = "2";
            }
            else if (stravaMath >= 16000)
            {
                feature.hillCategory = "3";
            }
            else if (stravaMath >= 8000)
            {
                feature.hillCategory = "4";
            }
            else
            {
                feature.hillCategory = "-";
            }


            // Previous attempt at categorizing
            //// HC
            //if ((feature.distance > 10000 /*&& feature.avgGrade > .06f*/)
            //    && (feature.distance > 25000
            //        || feature.elevGain > 1500
            //        || feature.avgGrade >.1f ))
            //{
            //    feature.hillCategory = "HC";
            //}
            //// Cat1
            //else if (feature.distance >= 7000 /*&& feature.avgGrade >= .045f*/ && feature.elevGain >= 800)
            //{
            //    feature.hillCategory = "1";
            //}
            //// Cat2
            //else if (feature.distance >= 5000 /*&& feature.avgGrade >= .035f*/ && feature.elevGain >= 500)
            //{
            //    feature.hillCategory = "2";
            //}
            //// Cat3
            //else if (feature.distance >= 1000 /*&& feature.avgGrade >= .035f*/ && feature.elevGain >= 150)
            //{
            //    feature.hillCategory = "3";
            //}
            //// Cat4
            //else if (feature.distance >= 1000 /*&& feature.avgGrade >= .03f*/ && feature.elevGain >= 70)
            //{
            //    feature.hillCategory = "4";
            //}
            //// Uncat
            //else
            //{
            //    feature.hillCategory = "-";
            //}
        }
        #endregion

        #region Course Scoring Algorithms

        /// <summary>
        /// PopulateCyclingScores will fill in the Overall textboxes for course score
        /// </summary>
        /// <param name="inActivity">This activity</param>
        private void PopulateCyclingScores(IActivity inActivity)
        {
            ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
            ICustomDataFieldDefinition cyclingScore = CustomDataFields.GetCustomProperty(CustomDataFields.CSCustomFields.CourseScoreCycling);
            ICustomDataFieldDefinition cyclingScoreDistance = CustomDataFields.GetCustomProperty(CustomDataFields.CSCustomFields.ScoreDistanceCycling);

            double? cs = inActivity.GetCustomDataValue(cyclingScore) as double?;
            double? csDist = inActivity.GetCustomDataValue(cyclingScoreDistance) as double?;

            // Course score
            // Calcuate the course score
            double? newScore = CalculateCourseScoreCycling(inActivity);
            if (newScore != cs)
            {
                // Put the cs value in a custom field
                inActivity.SetCustomDataValue(cyclingScore, newScore);
            }

            // Course score per distance unit
            double? newcsDist = newScore / Length.Convert(info.DistanceMeters, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits);
            if (newcsDist != csDist)
            {
                // Put the cs/distance value in a customField
                inActivity.SetCustomDataValue(cyclingScoreDistance, newcsDist);
            }

            double courseScore = (double)newScore;
            double scoreDistance = (double)newcsDist;
            textBox_courseScore.Text = courseScore.ToString("0.00", CultureInfo.CurrentCulture);
            textBox_scoreDistance.Text = scoreDistance.ToString("0.00", CultureInfo.CurrentCulture);


        }

        /// <summary>
        /// PopulateCyclingScores will fill in the Overall textboxes for course score
        /// </summary>
        /// <param name="inActivity">This activity</param>
        private void PopulateRunningScores(IActivity inActivity)
        {
            ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
            ICustomDataFieldDefinition runningScore = CustomDataFields.GetCustomProperty(CustomDataFields.CSCustomFields.CourseScoreRunning);
            ICustomDataFieldDefinition runningScoreDistance = CustomDataFields.GetCustomProperty(CustomDataFields.CSCustomFields.ScoreDistanceRunning);

            double? cs = inActivity.GetCustomDataValue(runningScore) as double?;
            double? csDist = inActivity.GetCustomDataValue(runningScoreDistance) as double?;

            // Course score
            // Calcuate the course score
            double? newScore = CalculateCourseScoreRunning(inActivity);

            if (newScore != cs)
            {
                // Put the cs value in a custom field
                inActivity.SetCustomDataValue(runningScore, newScore);
            }

            // Course score per distance unit
            double? newcsDist = newScore / Length.Convert(info.DistanceMeters, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits);
            if (newcsDist != csDist)
            {
                // Put the cs/distance value in a customField
                inActivity.SetCustomDataValue(runningScoreDistance, newcsDist);
            }

            double courseScore = (double)newScore;
            double scoreDistance = (double)newcsDist;
            textBox_courseScore.Text = courseScore.ToString("0.00", CultureInfo.CurrentCulture);
            textBox_scoreDistance.Text = scoreDistance.ToString("0.00", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// CalcuateCourseScoreCycling will apply a TRIMP style scoring algorithm to score the course
        /// </summary>
        /// <param name="act">Activity to score</param>
        /// <returns>Returns the score</returns>
        private double CalculateCourseScoreCycling(IActivity act)
        {
            SortedList<double, double> gradeDistance = CalcuateGradeDistance(act);
            if (gradeDistance != null)
            {
                // Prep the variables
                double score = 0;
                //double factor = 0;
                //double offset = 0;

                // Pull the factor and offset from settings
                //factor = GlobalSettings.Instance.CyclingFactor;
                //offset = GlobalSettings.Instance.CyclingOffset;

                // Calculate the course score
                foreach (KeyValuePair<double, double> pair in gradeDistance)
                {
                    if (pair.Key < 0)
                    {
                    }
                    else
                    {
                        score += (pair.Value / 1000) * ((cyclingFactor * pair.Key) + cyclingOffset);
                    }
                }

                return score;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// CalcuateCourseScoreRunning will apply a TRIMP style scoring algorithm to score the course
        /// </summary>
        /// <param name="act">Activity to score</param>
        /// <returns>Returns the score</returns>
        private double CalculateCourseScoreRunning(IActivity act)
        {
            SortedList<double, double> gradeDistance = CalcuateGradeDistance(act);
            if (gradeDistance != null)
            {
                Double score = 0;

                double g = 0;
                double g0 = .184f;
                double a0 = 1.68f;
                double a1 = 54.9f;
                double a2 = -102f;
                double a3 = 200f;
                double de = 0;

                foreach (KeyValuePair<double, double> pair in gradeDistance)
                {
                    g = (pair.Key / 100) / Math.Sqrt(1 + (pair.Key / 100) * (pair.Key / 100));
                    de = a0 + a1 * Math.Pow(g + g0, 2) + a2 * Math.Pow(g + g0, 4) + a3 * Math.Pow(g + g0, 6);
                    de = de - 1.5f;
                    score += (pair.Value / 1000) * de;
                }

                return score;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// CalcuateGradeDistance will return a sorted list of grades and the distance travelled for that grade for the supplied activity
        /// </summary>
        /// <param name="act">Activity</param>
        /// <returns>Returns a sorted list of grades and the distance travelled for that grade</returns>
        private SortedList<double, double> CalcuateGradeDistance(IActivity act)
        {
            ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(act);
            SortedList<double, double> gradeDistance = new SortedList<double, double>();
            double lastDistance = 0;

            // My smoothed values to assure the same data each time (no users settings)
            float min;
            float max;
            INumericTimeDataSeries smoothedElevation = Utilities.STSmooth(Utilities.GetElevationTrack(act), scoreSmoothing, out min, out max);
            INumericTimeDataSeries distanceTrack = Utilities.GetDistanceMovingTrack(act);
            //INumericTimeDataSeries gradeTrack = Utilities.STSmooth(Utilities.GetGradeTrack(act), scoreSmoothing, out min, out max);

            // Pull the grade track, remove bad points, then apply smoothing.  Bad points cause the smoothing algo to make the bad point spread out.
            INumericTimeDataSeries gradeTrack = Utilities.GetGradeTrack(act);
            gradeTrack = Utilities.RemoveBadTrackData(gradeTrack, -1, 1);
            gradeTrack = Utilities.STSmooth(gradeTrack, scoreSmoothing, out min, out max);

            if (smoothedElevation != null && distanceTrack != null && gradeTrack != null)
            {
                if (smoothedElevation.Count != 0 && distanceTrack.Count != 0)
                {
                   DateTime start = smoothedElevation.StartTime;

                    for (int i = 0; i < smoothedElevation.Count; i++)
                    {
                        double grade = 0;
                        double distance = 0;
                        ITimeValueEntry<float> point = gradeTrack.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i].ElapsedSeconds));
                        if (point != null)
                        {
                            grade = Math.Round(point.Value, 2);
                        }

                        point = distanceTrack.GetInterpolatedValue(start.AddSeconds(smoothedElevation[i].ElapsedSeconds));
                        if (point != null)
                        {
                            distance = point.Value;
                        }

                        if (i == 0)
                        {
                            lastDistance = distance;
                        }
                        // If the grade is in the list, add distance to it
                        if (gradeDistance.ContainsKey(grade * 100))
                        {
                            gradeDistance[grade * 100] += (distance - lastDistance);
                        }

                        // If the grade isn't in the list, add it
                        else
                        {
                            gradeDistance.Add(grade * 100, (distance - lastDistance));
                        }

                        lastDistance = distance;
                    }
                }

                return gradeDistance;
            }
            else
            {
                return null;
            }
        }

        #endregion

        /// <summary>
        /// BuildAndDrawSplits will build the splits for this activity and return them in a list
        /// </summary>
        /// <param name="act">Activity to parse for splits</param>
        /// <param name="autozoom">Autozoom the chart?</param>
        /// <returns>Returns a list of split features</returns>
        public List<Feature> BuildAndDrawSplits(IActivity act, bool autozoom)
        {
            // Clear the chart
            MainChart.DataSeries.Clear();

            // Get the elevation track to use to find the end time
            INumericTimeDataSeries eleTrack = Utilities.GetElevationTrack(act);

            // If the elevation track is crap, exit
            if (eleTrack == null || eleTrack.Count <= 0)
            {
                return null;
            }

            // Init the variables
            List<Feature> splits = new List<Feature>();

            IActivityLaps recordedLaps = act.Laps;

            for (int i = 0; i < recordedLaps.Count; i++)
            {
                Feature split = new Feature();

                if (i == recordedLaps.Count - 1)
                {
                    // If this is the last lap, the end time is the track end time
                    split = new Feature(activity, Feature.feature_type.split, recordedLaps[i].StartTime, recordedLaps[0].StartTime.AddSeconds(eleTrack[eleTrack.Count - 1].ElapsedSeconds));
                }
                else
                {
                    // This split starts at the split time and ends at the next split's start
                    split = new Feature(activity, Feature.feature_type.split, recordedLaps[i].StartTime, recordedLaps[i + 1].StartTime);
                }


                // Score the split before we add it
                ScoreHillClimbByBike(split);
                ScoreHillCycle2Max(split);
                ScoreHillFiets(split);
                ScoreHillCourseScoreRunning(split);
                ScoreHillCourseScoreCycling(split);
                ScoreHillCategory(split);

                // Add the split to the list
                splits.Add(split);
            }

            // Color the features based hard/steep/long/etc
            ColorFeatures(splits);

            // Set the split numbers
            SetHillNumbers(splits);

            // Draw the extra data items on the chart
            List<IActivity> actList = new List<IActivity>();
            actList.Add(act);
            DrawChartSelectedRecordsByActivity(actList, autozoom);

            // Draw the tree
            RefreshTree(splits);

            // Draw the map
            DrawMap(splits, true);

            // Draw the chart
            DrawElevationProfile(splits, autozoom);

            return splits;
        }


        private void splitsContextMenuStrip_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem selected = sender as ToolStripMenuItem;
            if ((RightClickMenu)selected.Tag == RightClickMenu.Clear)
            {
                ClearChart();
            }
            else if ((RightClickMenuSplits)selected.Tag == RightClickMenuSplits.ExactSplit)
            {
                #region Exact Split
                // Close this context strip
                contextMenuStrip.Close();

                // Double check that something is selected, if not, exit
                if (treeList1.SelectedItems.Count == 0)
                {
                    return;
                }

                // Build the progress bar
                PopupProgress prog = new PopupProgress();
                prog.UpdateProgress(0, string.Empty);
                prog.Show();
                prog.BringToFront();

                // Setup variable to adjust progress bar's value
                int totalSteps = 0;
                int currentStep = 1;

                // Get all activities
                IEnumerable<IActivity> allActivities = PluginMain.GetApplication().Logbook.Activities;

                // Setup variable to pull out only activities that belong to the selected category
                IActivityCategory category = PluginMain.GetApplication().DisplayOptions.SelectedCategoryFilter;
                IList<IActivity> filteredActivities = new List<IActivity>();
                ActivityInfoCache info = ActivityInfoCache.Instance;
                IActivityCategory activityCategory;

                // Parse all activities 
                foreach (IActivity thisActivity in allActivities)
                {
                    activityCategory = info.GetInfo(thisActivity).Activity.Category;

                    // Search through the categories for a match
                    while (true)
                    {
                        if (activityCategory == category)
                        {
                            filteredActivities.Add(info.GetInfo(thisActivity).Activity);
                            break;
                        }
                        else if (activityCategory.Parent != null)
                        {
                            activityCategory = activityCategory.Parent;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // Update progress bar's total steps
                totalSteps = filteredActivities.Count;

                // Set the selected feature based on the current selection              
                Feature selectedSplit = treeList1.Selected[0] as Feature;

                // If we have something selected
                if (selectedSplit != null)
                {
                    // Check to make sure we aren't trying to match a hill without GPS
                    if (selectedSplit.startPoint == null)
                    {
                        prog.Dispose();
                        Application.DoEvents();
                        MessageBox.Show(Resources.Strings.Message_NoGPS);
                        return;
                    }
                    List<Feature> allSplits = new List<Feature>();
                    foreach (IActivity act in filteredActivities)
                    {
                        prog.UpdateProgress((float)currentStep / (float)totalSteps, Resources.Strings.Message_ExportingElevation + " " + act.StartTime.ToString());
                        currentStep++;
                        Application.DoEvents();

                        // Let's not blow up if we hit an activity we can't parse.  Display an error instead
                        try
                        {
                            List<Feature> foundFeatures = FindThisFeature(selectedSplit, act);
                            allSplits.AddRange(foundFeatures);
                        }
                        catch
                        {
                            MessageBox.Show(Resources.Strings.Message_ErrorExporting + " " + info.GetInfo(act).ActualTrackStart.ToString());
                            break;
                        }
                    }

                    // Switch to mulitple activity mode
                    SwitchToMultiMode(allSplits);

                    prog.Dispose();
                    Application.DoEvents();
                    MainChart.Refresh();
                }

                #endregion
            }
            else if ((RightClickMenuSplits)selected.Tag == RightClickMenuSplits.Route)
            {
                #region Like Hill

                // Close this context strip
                contextMenuStrip.Close();

                // Double check that something is selected, if not, exit
                if (treeList1.SelectedItems.Count == 0)
                {
                    return;
                }

                // Build the progress bar
                PopupProgress prog = new PopupProgress();
                prog.UpdateProgress(0, string.Empty);
                prog.Show();
                prog.BringToFront();

                // Setup variable to adjust progress bar's value
                int totalSteps = 0;
                int currentStep = 1;

                // Get all activities
                IEnumerable<IActivity> allActivities = PluginMain.GetApplication().Logbook.Activities;

                // Setup variable to pull out only activities that belong to the selected category
                IActivityCategory category = PluginMain.GetApplication().DisplayOptions.SelectedCategoryFilter;
                IList<IActivity> filteredActivities = new List<IActivity>();
                ActivityInfoCache info = ActivityInfoCache.Instance;
                IActivityCategory activityCategory;

                // Parse all activities 
                foreach (IActivity thisActivity in allActivities)
                {
                    activityCategory = info.GetInfo(thisActivity).Activity.Category;

                    // Search through the categories for a match
                    while (true)
                    {
                        if (activityCategory == category)
                        {
                            filteredActivities.Add(info.GetInfo(thisActivity).Activity);
                            break;
                        }
                        else if (activityCategory.Parent != null)
                        {
                            activityCategory = activityCategory.Parent;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // Update progress bar's total steps
                totalSteps = filteredActivities.Count;

                // Set the selected feature based on the current selection              
                Feature selectedFeature = (Feature)treeList1.Selected[0];

                // If we have something selected
                if (selectedFeature != null)
                {
                    List<Feature> allFeatures = new List<Feature>();
                    foreach (IActivity act in filteredActivities)
                    {
                        prog.UpdateProgress((float)currentStep / (float)totalSteps, Resources.Strings.Message_ExportingElevation + " " + info.GetInfo(act).ActualTrackStart.ToString());
                        currentStep++;
                        Application.DoEvents();

                        // Let's not blow up if we hit an activity we can't parse.  Display an error instead
                        try
                        {
                            List<Feature> foundFeatures = FindLikeFeatures(selectedFeature, act);
                            allFeatures.AddRange(foundFeatures);
                        }
                        catch
                        {
                            MessageBox.Show(Resources.Strings.Message_ErrorExporting + " " + info.GetInfo(act).ActualTrackStart.ToString());
                            break;
                        }
                    }

                    // Switch to mulitple activity mode
                    SwitchToMultiMode(allFeatures);

                    prog.Dispose();
                    Application.DoEvents();
                    MainChart.Refresh();
                }
                #endregion
            }
        }

        private void score_doubleclick(object sender, EventArgs e)
        {
            SortedList<double, double> gradeDistance = CalcuateGradeDistance(activity);

            List<Feature> features = new List<Feature>();

            if (gradeDistance != null)
            {
                // Calculate the course score
                foreach (KeyValuePair<double, double> pair in gradeDistance)
                {
                    if (pair.Key < 0)
                    {
                    }
                    else
                    {
                        Feature f = new Feature();
                        f.vam = pair.Key;
                        f.distance = pair.Value;
                        f.hillScoreClimbByBike = (pair.Value / 1000) * ((cyclingFactor * pair.Key) + cyclingOffset);
                        features.Add(f);
                    }
                }
            }

            GridPopup gp = new GridPopup();
            gp.treeList.Columns.Clear();

            // Build the column list
            IList<TreeList.Column> columns = new List<TreeList.Column>();

            gp.treeList.Columns.Add(new TreeList.Column("VAM", "Grade", 120, StringAlignment.Near));
            gp.treeList.Columns.Add(new TreeList.Column("Distance4Decimals", "Distance", 120, StringAlignment.Near));
            gp.treeList.Columns.Add(new TreeList.Column("HillScoreClimbByBike", "Score", 120, StringAlignment.Near));
            gp.treeList.RowData = features;
            gp.Show();

        }


        public List<MatchingFeature> FindAllSplits()
        {
            // Build the progress bar
            PopupProgress prog = new PopupProgress();
            prog.UpdateProgress(0, string.Empty);
            prog.Show();
            prog.BringToFront();

            // Setup variable to adjust progress bar's value
            int totalSteps = 0;
            int currentStep = 1;

            // Get all activities
            IEnumerable<IActivity> allActivities = PluginMain.GetApplication().Logbook.Activities;

            // Setup variable to pull out only activities that belong to the selected category
            IActivityCategory category = PluginMain.GetApplication().DisplayOptions.SelectedCategoryFilter;
            IList<IActivity> filteredActivities = new List<IActivity>();
            ActivityInfoCache info = ActivityInfoCache.Instance;
            IActivityCategory activityCategory;

            // Parse all activities 
            foreach (IActivity thisActivity in allActivities)
            {
                activityCategory = info.GetInfo(thisActivity).Activity.Category;

                // Search through the categories for a match
                while (true)
                {
                    if (activityCategory == category)
                    {
                        filteredActivities.Add(info.GetInfo(thisActivity).Activity);
                        break;
                    }
                    else if (activityCategory.Parent != null)
                    {
                        activityCategory = activityCategory.Parent;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // Update progress bar's total steps
            totalSteps = filteredActivities.Count;

            // Find all splits from all filtered activities
            List<MatchingFeature> allSplits = new List<MatchingFeature>();
            foreach (IActivity act in filteredActivities)
            {
                prog.UpdateProgress((float)currentStep / (float)totalSteps, Resources.Strings.Message_ExportingElevation + " " + act.StartTime.ToString());
                currentStep++;
                Application.DoEvents();

                // Let's not blow up if we hit an activity we can't parse.  Display an error instead
                //try
                //{
                IActivityLaps recordedLaps = act.Laps;
                foreach (ILapInfo lapInfo in recordedLaps)
                {
                    // Add the split as a feature 
                    Feature split = new Feature(act, Feature.feature_type.split, lapInfo.StartTime, lapInfo.StartTime.Add(lapInfo.TotalTime));

                    // Score the split before we add it
                    ScoreHillClimbByBike(split);
                    ScoreHillCycle2Max(split);
                    ScoreHillFiets(split);
                    ScoreHillCourseScoreRunning(split);
                    ScoreHillCourseScoreCycling(split);
                    ScoreHillCategory(split);

                    // I don't need to store all the track information in memory for the split, just basic info I use in my comparisons
                    MatchingFeature splitShell = new MatchingFeature(split);

                    // Check to make sure we have start/end GPS points.  No point to add it if it doesn't
                    if (splitShell.startPoint != null && splitShell.endPoint != null)
                    {
                        // Add the split to the list
                        allSplits.Add(splitShell);
                    }
                }
                //}
                //catch
                //{
                //    MessageBox.Show(Resources.Strings.Message_ErrorExporting + " " + info.GetInfo(act).ActualTrackStart.ToString());
                //    break;
                //}
            }

            prog.Dispose();
            Application.DoEvents();
            return allSplits;
        }

        public List<MatchingFeature> RemoveDuplicateSplits(List<MatchingFeature> allSplits)
        {
            List<MatchingFeature> uniqueFeatures = new List<MatchingFeature>();
            foreach (MatchingFeature currentFeature in allSplits)
            {
                if (!uniqueFeatures.Contains(currentFeature))
                {
                    uniqueFeatures.Add(currentFeature);
                }
            }
            return uniqueFeatures;
        }

        public List<Feature> GetSplitsForActivity(IActivity act, List<MatchingFeature> allSplits)
        {
            // First, let's remove splits that are longer than this activity
            List<MatchingFeature> splits = new List<MatchingFeature>();
            double totalProximityConstant = act.TotalDistanceMetersEntered / act.TotalTimeEntered.TotalSeconds * 4f;
            foreach (MatchingFeature feature in allSplits)
            {
                if (feature.totalDistanceMeters < act.TotalDistanceMetersEntered + totalProximityConstant)
                {
                    splits.Add(feature);
                }
            }


            // Next, walk through the activity and find any existing split in it
            List<Feature> equalFeatures = new List<Feature>();
            ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(act);

            if (act.GPSRoute != null && ai.SmoothedElevationTrack != null && ai.SmoothedElevationTrack.Count > 0)
            {
                if (act.GPSRoute.Count > 0)
                {
                    DateTime endTime = new DateTime();

                    //float start_delta = float.MaxValue; // distance from start point during search
                    DateTime start = act.StartTime;
                    INumericTimeDataSeries meters = Utilities.GetDistanceMovingTrack(act);

                    List<MatchingFeature> matchedStartPoints = new List<MatchingFeature>();
                    //List<MatchingFeature> matchedEndPoints = new List<MatchingFeature>();

                    for (int i = 0; i < act.GPSRoute.Count; i++)
                    {
                        // If the index is outside the distance track, we are done.  Break.
                        if (i >= meters.Count)
                        {
                            break;
                        }

                        GPSPoint p = (GPSPoint)act.GPSRoute[i].Value;

                        List<MatchingFeature> featuresToCheck = new List<MatchingFeature>(splits);

                        // Start point
                        foreach (MatchingFeature feature in featuresToCheck)
                        {
                            // Constant that defines how close a point need to be to the feature points.  Measured in meters.
                            // This may need to be a large number for cycling... approx 40-50ish!!!
                            // Worst case scenario:  travel 40 mph (high end of bike speed) = 17m/s.  5 sec. device recording -> 85 meters in 5 seconds.
                            // Divide by 2 (midpoint... farthest spot from 2 consecutive points ~40 meter radius)
                            // large number like this should still be OK because 130 feet will easily separate routes and points, etc.
                            double proximityConstant = feature.totalDistanceMeters / feature.totalTime.TotalSeconds * 4f;

                            if (feature.startPoint.DistanceMetersToPoint(p) < proximityConstant)
                            {
                                // zero in on the best end point instead of taking first closest match
                                // continue searching until we start getting farther away (or get to the track end)
                                int thisPoint = i;

                                // Search backwards for a better point
                                while (thisPoint - 2 > 0 &&
                                        feature.startPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[thisPoint].Value) >=
                                        feature.startPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[thisPoint - 1].Value))
                                {
                                    thisPoint--;
                                }

                                // Search forwards for a better point
                                while (thisPoint + 2 < act.GPSRoute.Count &&
                                        feature.startPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[thisPoint].Value) >=
                                        feature.startPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[thisPoint + 1].Value))
                                {
                                    thisPoint++;
                                }

                                DateTime sTime = start.AddSeconds(act.GPSRoute[thisPoint].ElapsedSeconds);
                                feature.activityFact = new MatchingFeature.ActivityFacts(sTime);

                                // Check to see if we have added this start point
                                bool newStartPoint = true;
                                foreach (MatchingFeature sFeature in matchedStartPoints)
                                {
                                    if (sFeature.activityFact.factTime.Equals(feature.activityFact.factTime))
                                    {
                                        newStartPoint = false;
                                        break;
                                    }
                                    else
                                    {
                                        newStartPoint = true;
                                    }
                                }

                                // If it's not in our list, add it
                                if (newStartPoint)
                                {
                                    matchedStartPoints.Add(new MatchingFeature(feature));
                                }
                            }
                        }

                    }

                    for (int i = 0; i < act.GPSRoute.Count; i++)
                    {
                        // If the index is outside the distance track, we are done.  Break.
                        if (i >= meters.Count)
                        {
                            break;
                        }

                        GPSPoint p = (GPSPoint)act.GPSRoute[i].Value;

                        // End point
                        // Only check for end points if we have found a start point
                        foreach (MatchingFeature feature in matchedStartPoints)
                        {
                            // Constant that defines how close a point need to be to the feature points.  Measured in meters.
                            // This may need to be a large number for cycling... approx 40-50ish!!!
                            // Worst case scenario:  travel 40 mph (high end of bike speed) = 17m/s.  5 sec. device recording -> 85 meters in 5 seconds.
                            // Divide by 2 (midpoint... farthest spot from 2 consecutive points ~40 meter radius)
                            // large number like this should still be OK because 130 feet will easily separate routes and points, etc.
                            double proximityConstant = feature.totalDistanceMeters / feature.totalTime.TotalSeconds * 4f;
                            p = (GPSPoint)act.GPSRoute[i].Value;

                            if (feature.endPoint.DistanceMetersToPoint(p) < proximityConstant)
                            {
                                // zero in on the best end point instead of taking first closest match
                                // continue searching until we start getting farther away (or get to the track end)
                                int thisPoint = i;

                                // Search backwards for a better point
                                while (thisPoint - 2 > 0 &&
                                        feature.endPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[thisPoint].Value) >=
                                        feature.endPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[thisPoint - 1].Value))
                                {
                                    thisPoint--;
                                }

                                // Search forwards for a better point
                                while (thisPoint + 2 < act.GPSRoute.Count &&
                                        feature.endPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[thisPoint].Value) >=
                                        feature.endPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[thisPoint + 1].Value))
                                {
                                    thisPoint++;
                                }

                                endTime = start.AddSeconds(act.GPSRoute[thisPoint].ElapsedSeconds);

                                Feature potentialMatch = new Feature(act, Feature.feature_type.split, feature.activityFact.factTime, endTime);
                                double distanceBuffer = feature.totalDistanceMeters * .15f;
                                double gradeBuffer = .02f;

                                if (Math.Abs(feature.totalDistanceMeters - potentialMatch.distance) < distanceBuffer &&
                                     Math.Abs(feature.avgGrade - potentialMatch.avgGrade) < gradeBuffer)
                                {
                                    // Check and make sure we haven't already added this feature to our equalFeatures list
                                    if (!equalFeatures.Contains(potentialMatch))
                                    {
                                        // Score the split before we add it
                                        ScoreHillClimbByBike(potentialMatch);
                                        ScoreHillCycle2Max(potentialMatch);
                                        ScoreHillFiets(potentialMatch);
                                        ScoreHillCourseScoreRunning(potentialMatch);
                                        ScoreHillCourseScoreCycling(potentialMatch);
                                        ScoreHillCategory(potentialMatch);

                                        equalFeatures.Add(potentialMatch);
                                        Debug.WriteLine(feature.activityFact.factTime + " - " + endTime + " = " + feature.featureStartTime);
                                    }
                                }
                            }
                        }// End of matched start points
                    }//End of for loop walking the gps route
                }
            }

            return equalFeatures;
        }

        // This is a simplier version of GetSplitsForActivity but has issue
        // It does not do well with out and back routes
        // It refinds the start point before it gets to the end point and misses matches
        // Commenting out incase I need to revisit in the future
        //public List<Feature> GetSplitsForActivity2(IActivity act, List<MatchingFeature> allSplits)
        //{
        //    // First, let's remove splits that are longer than this activity
        //    List<MatchingFeature> splits = new List<MatchingFeature>();
        //    double totalProximityConstant = act.TotalDistanceMetersEntered / act.TotalTimeEntered.TotalSeconds * 4f;
        //    foreach (MatchingFeature feature in allSplits)
        //    {
        //        if (feature.totalDistanceMeters < act.TotalDistanceMetersEntered + totalProximityConstant)
        //        {
        //            splits.Add(feature);
        //        }
        //    }


        //    // Next, walk through the activity and find any existing split in it
        //    List<Feature> equalFeatures = new List<Feature>();
        //    ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(act);


        //    // ************************
        //    // Check each feature to see if it is on the route
        //    foreach (MatchingFeature feature in splits)
        //    {
        //        // If the activity track is good, proceed
        //        if (act.GPSRoute != null && ai.SmoothedElevationTrack != null && ai.SmoothedElevationTrack.Count > 0)
        //        {
        //            // Make sure the route has points
        //            if (act.GPSRoute.Count > 0)
        //            {
        //                DateTime startTime = new DateTime();
        //                DateTime endTime = new DateTime();

        //                // Set the activity start
        //                DateTime start = act.StartTime;

        //                // Pull the distanceMovingTrack
        //                INumericTimeDataSeries meters = Utilities.GetDistanceMovingTrack(act);

        //                // Boolean value to indicate if we have found a matching start point
        //                bool startFound = false;

        //                // Walk the activity's route
        //                for (int i = 0; i < act.GPSRoute.Count; i++)
        //                {
        //                    // If the index is outside the distance track, we are done.  Break.
        //                    // We need this since we are adding to i in places besides the for loop
        //                    if (i >= meters.Count)
        //                    {
        //                        break;
        //                    }

        //                    // Get this point
        //                    GPSPoint p = (GPSPoint)act.GPSRoute[i].Value;

        //                    // Set our proxConstant to use in checking points
        //                    double proximityConstant = feature.totalDistanceMeters / feature.totalTime.TotalSeconds * 6f;

        //                    // Check to see if this point is in the start proximity bounds
        //                    if (feature.startPoint.DistanceMetersToPoint(p) < proximityConstant && startFound)
        //                    {
        //                        // We found a start point
        //                        startFound = true;

        //                        // Now zero in on the best end point instead of taking first closest match
        //                        // Search forwards for a better point
        //                        while (i + 2 < act.GPSRoute.Count &&
        //                                feature.startPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[i].Value) >=
        //                                feature.startPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[i + 1].Value))
        //                        {
        //                            i++;
        //                        }

        //                        // Pull the startTime for this point.  We'll use it to construct the final Feature
        //                        startTime = start.AddSeconds(act.GPSRoute[i].ElapsedSeconds);
        //                    }

        //                    // If we have found a start point for this feature
        //                    // Check and see if this point is in the end proximity bounds
        //                    if (startFound)
        //                    {
        //                        if (i == act.GPSRoute.Count - 1)
        //                        {
        //                            int david = 0;
        //                        }

        //                        if (feature.endPoint.DistanceMetersToPoint(p) < proximityConstant)
        //                        {
        //                            // Now zero in on the best end point instead of taking first closest match
        //                            // Search forwards for a better point
        //                            while (i + 2 < act.GPSRoute.Count &&
        //                                    feature.endPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[i].Value) >=
        //                                    feature.endPoint.DistanceMetersToPoint((GPSPoint)act.GPSRoute[i + 1].Value))
        //                            {
        //                                i++;
        //                            }

        //                            // Pull the endTime for this point.  We'll use it to construct the final Feature
        //                            endTime = start.AddSeconds(act.GPSRoute[i].ElapsedSeconds);

        //                            // Create the matching feature
        //                            Feature match = new Feature(act, Feature.feature_type.split, startTime, endTime);
        //                            ScoreHillClimbByBike(match);
        //                            ScoreHillCycle2Max(match);
        //                            ScoreHillFiets(match);
        //                            ScoreHillCourseScoreRunning(match);
        //                            ScoreHillCourseScoreCycling(match);
        //                            ScoreHillCategory(match);

        //                            // Create buffers to use to test to see if our match meets the distance and grade of the existing feature
        //                            double distanceBuffer = feature.totalDistanceMeters * .15f;
        //                            double gradeBuffer = .02f;

        //                            // Check and make sure key stats are close (make sure it's the same route)
        //                            if (Math.Abs(feature.totalDistanceMeters - match.distance) < distanceBuffer &&
        //                                Math.Abs(feature.avgGrade - match.avgGrade) < gradeBuffer)
        //                            {
        //                                equalFeatures.Add(match);
        //                                break;
        //                            }
        //                            else
        //                            {
        //                                // Filtered
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //    }


        //    // ************************


        //    return equalFeatures;
        //}

        public void SwitchToMultiMode(List<Feature> allFeatures)
        {
            // Switch modes
            mode = Mode.MultipleActivities;

            // Clear the markers from the xaxis
            MainChart.XAxis.Markers.Clear();

            // Setup the details menu
            SetupDetailMenuMultipleActivities();

            PopulateBottomPane(allFeatures);

        }

        public void SwitchToSingleMode()
        {
            mode = Mode.OneActivity;
            SetupDetailMenuSingleActivity();
            List<IActivity> currentActs = new List<IActivity>();
            currentActs.Add(activity);
            activities = currentActs;
            RefreshPage(true);
        }

        /// <summary>
        /// Menu Item selected from chart action banner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detailMenuMultipleActivities_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem selected = sender as ToolStripMenuItem;

            for (int i = 0; i < detailMenu.Items.Count; i++)
            {
                ToolStripMenuItem item = detailMenu.Items[i] as ToolStripMenuItem;

                if (item != null)
                {
                    if (item != selected)
                    {
                        item.Checked = false;
                    }
                    else
                    {
                        item.Checked = true;
                    }
                }
                else
                {
                    // ToolStrip Separator encountered.  Stop evaluating
                    break;
                }
            }

            GlobalSettings.Instance.HillChartTypeMultiMode = (HillChartTypeMultiMode)selected.Tag;
            detailMenu.Text = selected.Text;

            if (treeList1.RowData != null)
            {
                List<Feature> allFeatures = treeList1.RowData as List<Feature>;
                PopulateBottomPane(allFeatures);
            }

        }

        private void PopulateBottomPane(List<Feature> allFeatures)
        {
            // Build the tree list in the above pane
            MultiModeBuildTreeList(allFeatures);

            // Figure out what is selected and show that bottom pane
            if (GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.Details)
            {
                MultiModeDetails(allFeatures);
                ChartBanner.Text = CommonResources.Text.LabelDetails;
            }
            else /*if (GlobalSettings.Instance.HillChartTypeMultiMode == HillChartTypeMultiMode.Summary)*/
            {
                MultiModeSummary(allFeatures);
                ChartBanner.Text = Resources.Strings.Label_Summary;
            }
        }


        private void MultiModeDetails(List<Feature> allFeatures)
        {

            DrawChartSelectedRecordsByFeature(allFeatures, true);
        }

        private void MultiModeBuildTreeList(List<Feature> allFeatures)
        {

            // Set up the colors for the distinct chart
            List<Color> colors = Utilities.Rainbow(allFeatures.Count, 255);
            for (int i = 0; i < allFeatures.Count; i++)
            {
                allFeatures[i].selectedColor = colors[i];
            }

            treeList1.RowData = null;
            treeList1.MultiSelect = true;
            MainChart.DataSeries.Clear();
            RefreshTree(allFeatures);

            List<IActivity> featureActs = new List<IActivity>();
            foreach (Feature f in allFeatures)
            {
                featureActs.Add(f.record.Activity);
            }
            activities = featureActs;

            // Sort the tree list
            SortHillsTreeList(currentSortColumnId, false);
        }

        private void MultiModeSummary(List<Feature> allFeatures)
        {

            // Remove the buttons that don't apply
            DisableSingleModeIcons();

            // Clear the chart
            MainChart.DataSeries.Clear();

            IAxis axis = MainChart.YAxis;
            //ChartDataSeries ds = new ChartDataSeries(MainChart, axis);

            ArrayList xLabels = new ArrayList();

            // Sort the features by start date
            Feature.FeatureComparer comparer = new Feature.FeatureComparer();
            comparer.ComparisonMethod = Feature.FeatureComparer.ComparisonType.Date;
            allFeatures.Sort(comparer);

            // Loop the features and create points
            for (int j = 0; j < allFeatures.Count; j++)
            {
                ChartDataSeries ds = new ChartDataSeries(MainChart, axis);
                PointF point = new PointF();
                switch (GlobalSettings.Instance.HillChartTypeMultiMode)
                {
                    case HillChartTypeMultiMode.Cadence:
                        point = new PointF(j, float.Parse(allFeatures[j].Cadence));
                        break;
                    case HillChartTypeMultiMode.Elevation:
                        point = new PointF(j, float.Parse(allFeatures[j].ElevGain));
                        break;
                    case HillChartTypeMultiMode.Grade:
                        point = new PointF(j, (float)allFeatures[j].avgGrade * 100);
                        break;
                    case HillChartTypeMultiMode.HR:
                        point = new PointF(j, float.Parse(allFeatures[j].HR));
                        break;
                    case HillChartTypeMultiMode.Power:
                        point = new PointF(j, float.Parse(allFeatures[j].Power));
                        break;
                    case HillChartTypeMultiMode.Speed:
                        point = new PointF(j, float.Parse(allFeatures[j].AvgSpeed));
                        break;
                    case HillChartTypeMultiMode.VAM:
                        point = new PointF(j, float.Parse(allFeatures[j].VAM));
                        break;
                }
                xLabels.Add(allFeatures[j].startTime.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture));
                ds.Points.Add(j, point);
                ds.Data = allFeatures[j].refId;
                ds.ChartType = ChartDataSeries.Type.Bar;
                ds.FillColor = Color.FromArgb(75, ChartColorLookup(GlobalSettings.Instance.HillChartTypeMultiMode));
                ds.SelectedColor = allFeatures[j].selectedColor;
                //ds.LineColor = ChartColorLookup(GlobalSettings.Instance.HillChartTypeMultiMode);
                //ds.LineWidth = 1;
                //ds.SelectedColor = ChartColorLookup(GlobalSettings.Instance.HillChartTypeMultiMode);

                MainChart.DataSeries.Add(ds);
            }
            //ds.Points.Add(ds.Points.Count, new PointF(ds.Points.Count, 0));

            // Set the formatter to a my custom labels
            Formatter.Category xformatter = new Formatter.Category(xLabels);

            MainChart.XAxis.Formatter = xformatter;

            Formatter.General yformatter = new Formatter.General();
            MainChart.YAxis.Formatter = yformatter;

            // Format and display the chart
            //ds.ChartType = ChartDataSeries.Type.Bar;
            MainChart.XAxis.Label = CommonResources.Text.LabelDate;

            // Label and color the chart for this field
            //ds.FillColor = Color.FromArgb(75, ChartColorLookup(GlobalSettings.Instance.HillChartTypeMultiMode));
            //ds.LineColor = ChartColorLookup(GlobalSettings.Instance.HillChartTypeMultiMode);
            //ds.LineWidth = 1;
            //ds.SelectedColor = ChartColorLookup(GlobalSettings.Instance.HillChartTypeMultiMode);
            MainChart.YAxis.LabelColor = ChartColorLookup(GlobalSettings.Instance.HillChartTypeMultiMode);
            MainChart.YAxis.Label = ChartFieldsLookup(GlobalSettings.Instance.HillChartTypeMultiMode);

            MainChart.XAxis.ChartLines = false;

            MainChart.YAxisRight.Clear();
            //MainChart.DataSeries.Add(ds);
            MainChart.AutozoomToData(true);
            MainChart.XAxis.MinOriginValue = -1;
            MainChart.XAxis.MaxOriginFarValue = allFeatures.Count;
            MainChart.ZoomOut();
            MainChart.Focus();
        }

        private void DisableSingleModeIcons()
        {
            HillMarkersButton.Visible = false;
            button_color.Visible = false;
            button1.Visible = false;
        }

        private void EnableSingleModeIcons()
        {
            HillMarkersButton.Visible = true;
            button_color.Visible = true;
            button1.Visible = true;
        }

        /// <summary>
        /// Lookup the color for this enum
        /// </summary>
        /// <param name="field">enum to lookup</param>
        /// <returns>Returns the appropriate chart color</returns>
        public static Color ChartColorLookup(HillChartTypeMultiMode field)
        {
            switch (field)
            {
                case HillChartTypeMultiMode.Cadence:
                    return Common.ColorCadence;
                case HillChartTypeMultiMode.Grade:
                    return Common.ColorGrade;
                case HillChartTypeMultiMode.HR:
                    return Common.ColorHR;
                case HillChartTypeMultiMode.Power:
                    return Common.ColorPower;
                case HillChartTypeMultiMode.Speed:
                    return Common.ColorSpeed;
                case HillChartTypeMultiMode.Elevation:
                    return Common.ColorElevation;
                case HillChartTypeMultiMode.VAM:
                    return Common.ColorVAM;
            }

            return Color.Black;
        }

        /// <summary>
        /// Lookup the text for this enum
        /// </summary>
        /// <param name="field">enum to lookup</param>
        /// <returns>Returns the localized text</returns>
        public static string ChartFieldsLookup(HillChartTypeMultiMode field)
        {
            switch (field)
            {
                case HillChartTypeMultiMode.Cadence:
                    return CommonResources.Text.LabelCadence;
                case HillChartTypeMultiMode.Grade:
                    return CommonResources.Text.LabelGrade;
                case HillChartTypeMultiMode.HR:
                    return CommonResources.Text.LabelHeartRate;
                case HillChartTypeMultiMode.Power:
                    return CommonResources.Text.LabelPower;
                case HillChartTypeMultiMode.Speed:
                    // TODO: What about pace?
                    return CommonResources.Text.LabelSpeed;
                case HillChartTypeMultiMode.Elevation:
                    return CommonResources.Text.LabelElevation;
                case HillChartTypeMultiMode.VAM:
                    return Resources.Strings.Label_VAM;
            }
            return "";
        }

        private void MainChart_SelectData(object sender, ChartBase.SelectDataEventArgs e)
        {
            Debug.Write("A");

        }

        private void RefreshDetailsTreeLists()
        {
            treeList1.Dock = DockStyle.Top;
            treeList1.Visible = true;
            treeList1.BringToFront();

            treeList_subtotals.Dock = DockStyle.Bottom;
            treeList_subtotals.Visible = true;
            treeList_subtotals.BringToFront();
            treeList_subtotals.Refresh();
            Application.DoEvents();

            // Pull the total column width to see if there will be a scroll bar
            int totalWidth = 0;
            foreach (TreeList.Column col in treeList1.Columns)
            {
                totalWidth += col.Width;
            }

            // TODO: Hard coded numbers can't be the best.  Figure out why these work and how to pull them dynamically
            if (totalWidth > treeList1.Width)
            {
                treeList1.Height = splitContainer1.Panel1.Height - 56 + System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight;
                treeList_subtotals.Height = 35;
            }
            else
            {
                treeList1.Height = splitContainer1.Panel1.Height - 56 + System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight;
                treeList_subtotals.Height = 35 - System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight;
                int x = System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight;
            }



            treeList1.Refresh();
            treeList_subtotals.Refresh();


        }

        private void PopulateSubtotalRow()
        {
            List<SubtotalFeature> subtotalTest = new List<SubtotalFeature>();
            List<Feature> allData = new List<Feature>();
            allData = (List<Feature>)treeList1.RowData;
            if (allData != null && 0 < allData.Count)
            {
                subtotalTest.Add(CalculateAverageRow());
            }

            treeList_subtotals.RowData = subtotalTest;
            treeList_subtotals.Refresh();
        }

        private SubtotalFeature CalculateAverageRow()
        {
            List<Feature> features = new List<Feature>();
            features = (List<Feature>)treeList1.RowData;

            if (features != null && features.Count > 0)
            {
                SubtotalFeature subtotal = new SubtotalFeature(features);
                return subtotal.Average();
                //return subtotal;
            }
            else
            {
                return null;
            }
        }
    }
}
