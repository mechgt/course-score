using System;
using System.Globalization;
using System.Windows.Forms;
using ActivityComparer.Util;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Visuals.Util;
using System.Collections.Generic;

namespace ActivityComparer.UI.ReportView
{
    public class ActivityComparerDetail : IDetailPage
    {
        #region Fields

        private ActivityComparerDetailControl control;
        private IDailyActivityView activityView;
        private IActivityReportsView reportView;
        private static bool maximized;
        private static bool visible;
        private static ActivityComparerDetail instance;

        #endregion

        #region Properties

        public static ActivityComparerDetail Instance
        {
            get
            {
                return instance;
            }
        }

        internal bool Visible
        {
            get { return visible; }
        }

        #endregion

        #region Constructor

        public ActivityComparerDetail(IDailyActivityView view)
        {
            this.activityView = view;
            view.SelectionProvider.SelectedItemsChanged += new EventHandler(OnViewSelectedItemsChanged);
            instance = this;
        }

        public ActivityComparerDetail(IActivityReportsView view)
        {
            this.reportView = view;
            view.SelectionProvider.SelectedItemsChanged += new EventHandler(OnReportViewSelectedItemsChanged);
            //view.SelectionProvider.SelectedItemsChanged += new EventHandler(OnViewSelectedItemsChanged);
            instance = this;
        }

        #endregion

        #region Event Handlers

        private void OnViewSelectedItemsChanged(object sender, EventArgs e)
        {
            //IActivity activity = CollectionUtils.GetSingleItemOfType<IActivity>(activityView.SelectionProvider.SelectedItems);
            IList<IActivity> activities = CollectionUtils.GetItemsOfType<IActivity>(activityView.SelectionProvider.SelectedItems);
            //IList<IActivity> activities = new List<IActivity?();
            //if (activity != null)
            //{
            //    activities.Add(activity);
            //}

            SetActivities(activities);

        }

        private void OnReportViewSelectedItemsChanged(object sender, EventArgs e)
        {
            // TODO: Handle report view activity selection
            IList<IActivity> activities = CollectionUtils.GetItemsOfType<IActivity>(reportView.SelectionProvider.SelectedItems);
            SetActivities(activities);
        }

        #endregion

        // TODO: Do I need these members?
        #region IActivityDetailPage Members

        public void SetActivities(IEnumerable<IActivity> activities)
        {
            if (control != null)
            {
                //if (!Licensing.IsActivated && Plugin.GetApplication().Calendar.Selected.AddDays(Plugin.EvalDays) < DateTime.Now)
                //{
                    control.SetActivities(null);
                    control.ScreenLock(true);
                //}
                //else
                //{
                //    control.ScreenLock(false);
                //    control.SetActivities(activities);
                //    control.RefreshPage();
                //}
            }
        }

        public void RefreshPage()
        {
            if (control != null)
            {
                control.RefreshPage();
            }
        }

        #endregion

        #region IDialogPage Members

        public Control CreatePageControl()
        {
            if (control == null)
            {
                control = new ActivityComparerDetailControl();
                SetActivities(null);
            }

            return control;
        }

        public bool HidePage()
        {
            visible = false;
            return true;
        }

        public string PageName
        {
            get { return Resources.Strings.Label_MeanMax; }
        }

        public void ShowPage(string bookmark)
        {
            visible = true;

            if (control != null)
            {
                control.RefreshPage();
            }
        }

        public IPageStatus Status
        {
            get { throw new NotImplementedException(); }
        }

        public void ThemeChanged(ITheme visualTheme)
        {
            if (control != null)
            {
                control.ThemeChanged(visualTheme);
            }
        }

        public string Title
        {
            get { return Resources.Strings.Label_MeanMax; }
        }

        public void UICultureChanged(CultureInfo culture)
        {
            control = CreatePageControl() as ActivityComparerDetailControl;
            control.UICultureChanged(culture);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IDetailPage Members

        public Guid Id
        {
            get { return GUIDs.ActivityComparerDetail; }
        }

        public bool MenuEnabled
        {
            get { return true; }
        }

        public IList<string> MenuPath
        {
            get { return null; }
        }

        public bool MenuVisible
        {
            get
            {
                return true;
            }
        }

        public bool PageMaximized
        {
            get
            {
                return maximized;
            }
            set
            {
                maximized = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs("PageMaximized"));
                }
            }
        }

        #endregion
    }
}
