using System.Collections.Generic;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
namespace CourseScore.Settings
{
    class ExtendSettingsPages: IExtendSettingsPages
    {
        #region IExtendSettingsPages Members

        public IList<ISettingsPage> SettingsPages
        {
            get
            {
                // Create & return the new menu item under 'Select View'
                IList<ISettingsPage> views = new List<ISettingsPage>();

                // Removed the settings page for inital release
                // Uncomment this line to add it back
                //views.Add(new SettingsPage());

                return views;
            }
        }

        #endregion
    }
}
