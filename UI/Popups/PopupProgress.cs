using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Visuals;

namespace CourseScore.UI.Popups
{
    public partial class PopupProgress : Form
    {
        public PopupProgress()
        {
            InitializeComponent();
            status.Text = string.Empty;
            progress.Percent = 0f;
            ThemeChanged(PluginMain.GetApplication().VisualTheme);
        }

        /// <summary>
        /// UpdateProgress will update the progress bar to the supplied percentage
        /// </summary>
        /// <param name="percent">The percentage to show on the progress bar</param>
        /// <param name="text">The text to display along side the progress bar</param>
        public void UpdateProgress(float percent, string text)
        {
            progress.Percent = percent;
            status.Text = text;
            this.Update();
            Application.DoEvents();
        }

        /// <summary>
        /// ThemeChanged will change the color theme to match the user's selection
        /// </summary>
        /// <param name="visualTheme">ITheme to apply to the progress bar</param>
        public void ThemeChanged(ITheme visualTheme)
        {
            progress.ThemeChanged(visualTheme);
            status.ForeColor = visualTheme.ControlText;
        }
    }
}
