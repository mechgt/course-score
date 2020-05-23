using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Visuals.Mapping;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Data;
using CourseScore.Data;
using System.Diagnostics;

namespace CourseScore.UI.MapLayers
{
    class HillsPointsLayer : RouteControlLayerBase, IRouteControlLayer
    {
        private bool scalingChanged = false;
        private bool routeSettingsChanged = false;
        public bool activityMapDrawn = true;
        private List<IMapOverlay> featurePolylines = new List<IMapOverlay>();
        private List<IMapOverlay> highlightFeatures = new List<IMapOverlay>();
        private float highlightRadius;
        private static bool showPage;
        private List<IMapOverlay> originalOverlay = new List<IMapOverlay>();
        private static HillsPointsLayer dailyLayer = null;
        private static HillsPointsLayer reportsLayer = null;
        //private static List<Feature> features = new List<Feature>();

        public HillsPointsLayer(IRouteControlLayerProvider provider, IRouteControl control)
            : base(provider, control, 1)
        {
            // HACK: This can't be the best way to get the reports Layer
            // There are 2 layers on the reports view and I don't know how to separate them
            if (PluginMain.GetApplication().ActiveView.Id == GUIDs.ActivityReportsView)
            {
                if (reportsLayer == null)
                {
                    reportsLayer = this;
                }
            }
            else if(PluginMain.GetApplication().ActiveView.Id == GUIDs.DailyActivityView)
            {
                dailyLayer = this;
            }
        }

        public void AddFeature(Feature feature)
        {
            MapPolyline newLine = new MapPolyline(feature.record.GPSPoints, feature.routeWidth, Color.FromArgb(255, feature.lineColor));
            featurePolylines.Add(newLine);
            //features.Add(feature);
        }

        public void DrawFeatures(bool clear)
        {
            if (clear)
            {
                ClearOverlays();
            }

            MapControl.AddOverlays(featurePolylines);
        }

        public void HighlightFeature(List<Feature> features)
        {
            MapControl.RemoveOverlays(highlightFeatures);
            highlightFeatures.Clear();

            foreach (Feature feature in features)
            {
                MapPolyline newLine = new MapPolyline(feature.record.GPSPoints, feature.routeWidth, Color.FromArgb(255, feature.lineColor));
                highlightFeatures.Add(newLine);
            }
            MapControl.AddOverlays(highlightFeatures);
        }

        public void HighlightFeature(Feature feature)
        {
            MapControl.RemoveOverlays(highlightFeatures);
            highlightFeatures.Clear();

            MapPolyline newLine = new MapPolyline(feature.record.GPSPoints, feature.routeWidth, Color.FromArgb(255, feature.lineColor));
            highlightFeatures.Add(newLine);

            MapControl.AddOverlays(highlightFeatures);
        }

        public void ClearFeatures()
        {
            ClearOverlays();
            featurePolylines.Clear();
            //features.Clear();
        }

        public void RefreshOverlays()
        {

            if (MapControlChanged)
            {
                ResetMapControl();
            }

            if (!showPage)
            {
                return;
            }

            //ResetMapControl();
            //ClearOverlays();
            //MapControl.AddOverlays(featurePolylines);
        }

        public void ClearOverlays()
        {
            MapControl.RemoveOverlays(featurePolylines);
            MapControl.RemoveOverlays(highlightFeatures);
        }

        public void MoveMap(IGPSLocation newLocation)
        {
            Debug.Write(showPage);
            base.MapControl.PanTo(newLocation);
        }

        public void DrawRoute(IActivity activity)
        {
            if (activity.GPSRoute != null)
            {
                List<IGPSPoint> points = new List<IGPSPoint>();
                for (int i = 0; i < activity.GPSRoute.Count; i++)
                {
                    GPSPoint p = (GPSPoint)activity.GPSRoute[i].Value;
                    points.Add(p);
                }

                MapPolyline newLine = new MapPolyline(points,
                                                        PluginMain.GetApplication().SystemPreferences.RouteSettings.RouteWidth,
                                                        PluginMain.GetApplication().SystemPreferences.RouteSettings.RouteColor);

                MapControl.AddOverlay(newLine);

                activityMapDrawn = true;
            }
        }

        public float HighlightRadius
        {
            set
            {
                if (highlightRadius != value)
                {
                    scalingChanged = true;
                }
                highlightRadius = value;
            }
        }

        public bool ShowPage
        {
            get { return showPage; }
            set
            {
                bool changed = (value != showPage);
                showPage = value;
                if (changed)
                {
                    RefreshOverlays();
                }
            }
        }

        protected override void OnMapControlZoomChanged(object sender, EventArgs e)
        {
            if (showPage)
            {
                scalingChanged = true;
                RefreshOverlays();
            }
        }

        protected override void OnMapControlCenterMoveEnd(object sender, EventArgs e)
        {
            if (showPage)
            {
                RefreshOverlays();
            }
        }

        protected override void OnRouteControlResize(object sender, EventArgs e)
        {
            if (showPage)
            {
                RefreshOverlays();
            }
        }

        protected override void OnRouteControlVisibleChanged(object sender, EventArgs e)
        {
            if (showPage)
            {
                if (RouteControl.Visible && routeSettingsChanged)
                {
                    ClearOverlays();
                    routeSettingsChanged = false;
                    RefreshOverlays();
                }
            }
        }

        protected override void OnRouteControlMapControlChanged(object sender, EventArgs e)
        {
            if (showPage)
            {
                RefreshOverlays();
            }
        }

        protected override void OnRouteControlItemsChanged(object sender, EventArgs e)
        {
            if (showPage)
            {
                RefreshOverlays();
            }
        }

        protected override void OnRouteControlSelectedItemsChanged(object sender, EventArgs e)
        {
            if (showPage)
            {
                if (RouteControl.Visible)
                {
                    RouteControlSelectedItemsChanged.Invoke(sender, e);
                }
            }
        }

        internal event EventHandler RouteControlSelectedItemsChanged;

        public static HillsPointsLayer Instance
        {
            get 
            {
                // HACK: This can't be the best way to get the reports Layer
                // There are 2 layers on the reports view and I don't know how to separate them
                if (PluginMain.GetApplication().ActiveView.Id == GUIDs.ActivityReportsView)
                {
                    return reportsLayer;
                }
                else if (PluginMain.GetApplication().ActiveView.Id == GUIDs.DailyActivityView)
                {
                    return dailyLayer;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
