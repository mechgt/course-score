using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;

using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Visuals.Util;
namespace CourseScore.UI.DetailPage
{
    class HillsDetail : IDetailPage
    {

        #region Fields

        private IDailyActivityView view;
        //private static HillsDetail instance;
        private HillsDetailControl control;
        //private static bool visible;
        private static bool maximized;

        #endregion

        #region Constructor

        internal HillsDetail(IDailyActivityView view)
        {
            this.view = view;
        }

        #endregion

        #region Properties

        //public static HillsDetail Instance
        //{
        //    get
        //    {
        //        return instance;
        //    }
        //}

        //internal bool Visible
        //{
        //    get { return visible; }
        //}

        private HillsDetailControl Control
        {
            get
            {
                if (control == null)
                {
                    control = CreatePageControl() as HillsDetailControl;
                }

                return control;
            }
        }

        #endregion

        //public HillsDetail(IDailyActivityView view)
        //{
        //    this.activityView = view;
        //    view.SelectionProvider.SelectedItemsChanged += new EventHandler(OnViewSelectedItemsChanged);
        //    instance = this;
        //    visible = false;
        //}

        #region IDialogPage Members

        public Control CreatePageControl()
        {
            if (control == null)
            {
                control = new HillsDetailControl();
            }

            return control;
        }

        public bool HidePage()
        {
            //visible = false;
            if (control != null)
            {
                control.HideMapLayer();
                view.SelectionProvider.SelectedItemsChanged -= SelectionProvider_SelectedItemsChanged;
                control.Maximize -= control_Maximize;
            }
            return true;
        }

        public string PageName
        {
            get { return Resources.Strings.Label_CourseScore; }
        }

        public void ShowPage(string bookmark)
        {
            //visible = true;

            if (control != null)
            {
                //control.Maximize += new EventHandler(control_Maximize);
                //control.RefreshPage(true);

                view.SelectionProvider.SelectedItemsChanged += new EventHandler(SelectionProvider_SelectedItemsChanged);

                control.Maximize += new EventHandler(control_Maximize);
                SelectionProvider_SelectedItemsChanged(null, null);
            }
        }

        void control_Maximize(object sender, EventArgs e)
        {
            PageMaximized = !PageMaximized;
        }

        public IPageStatus Status
        {
            get { return null; }
        }

        public void ThemeChanged(ITheme visualTheme)
        {
            Control.ThemeChanged(visualTheme);
        }

        public string Title
        {
            get { return Resources.Strings.Label_CourseScore; }
        }

        public void UICultureChanged(System.Globalization.CultureInfo culture)
        {
            //control = CreatePageControl() as HillsDetailControl;
            Control.UICultureChanged(culture);
        }

        #endregion

        #region IActivityDetailPage Members

        public void SetActivities(IEnumerable<IActivity> activities)
        {
            if (control != null)
            {
                control.CurrentView = view;
                control.Activities = activities;
                //RefreshPage();
            }
        }

        //public void RefreshPage()
        //{
        //    if (control != null)
        //    {
        //        if (visible)
        //        {
        //            control.RefreshPage(true);
        //        }
        //    }
        //}

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        #region IDetailPage Members

        public Guid Id
        {
            get { return GUIDs.HillsDetail; }
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
            get { return true; }
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
                if(control != null)
                {
                    control.maximized = value;
                }
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs("PageMaximized"));
                }
            }
        }

        #endregion

        #region Event Handlers

        private void SelectionProvider_SelectedItemsChanged(object sender, EventArgs e)
        {
            if (control != null)
            {
                IList<IActivity> activities = CollectionUtils.GetItemsOfType<IActivity>(view.SelectionProvider.SelectedItems);
                if (activities.Count == 0)
                {
                    SetActivities(null);
                }
                else
                {
                    SetActivities(activities);
                }
            }
        }

        #endregion
    }
}
