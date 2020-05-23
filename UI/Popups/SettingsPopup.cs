using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using ZoneFiveSoftware.Common.Visuals;

using CourseScore.UI.DetailPage;
using ZoneFiveSoftware.Common.Data.Measurement;
using CourseScore.Settings;

namespace CourseScore.UI.Popups
{
    public partial class SettingsPopup : Form
    {

        public double elevationPercent = double.NaN;
        public double distancePercent = double.NaN;
        public double gainElevationRequired = double.NaN;
        public double hillDistanceRequired = double.NaN;
        public double maxDescentLength = double.NaN;
        public double maxDescentElevation = double.NaN;
        public double minAvgGrade = double.NaN;

        public SettingsPopup()
        {
            
            elevationPercent = GlobalSettings.Instance.ElevationPercent;
            distancePercent = GlobalSettings.Instance.DistancePercent;
            gainElevationRequired = Length.Convert(GlobalSettings.Instance.GainElevationRequired, Length.Units.Meter,PluginMain.GetApplication().SystemPreferences.ElevationUnits);
            hillDistanceRequired = Length.Convert(GlobalSettings.Instance.HillDistanceRequired, Length.Units.Meter,PluginMain.GetApplication().SystemPreferences.DistanceUnits);
            maxDescentElevation = Length.Convert(GlobalSettings.Instance.MaxDescentElevation, Length.Units.Meter,PluginMain.GetApplication().SystemPreferences.ElevationUnits);
            maxDescentLength = Length.Convert(GlobalSettings.Instance.MaxDescentLength, Length.Units.Meter,PluginMain.GetApplication().SystemPreferences.DistanceUnits);
            minAvgGrade = GlobalSettings.Instance.MinAvgGrade;

            InitializeComponent();

            this.Icon = Utilities.MakeIcon(Resources.Images.gear, 16, true);

            ThemeChanged(PluginMain.GetApplication().VisualTheme);
            UICultureChanged(PluginMain.GetApplication().SystemPreferences.UICulture);

            textBox_distancePercent.Text = distancePercent.ToString("0.00", CultureInfo.CurrentCulture);
            textBox_elevationPercent.Text = elevationPercent.ToString("0.00", CultureInfo.CurrentCulture);
            textbox_gainElevationRequired.Text = gainElevationRequired.ToString("0", CultureInfo.CurrentCulture);
            textbox_hillDistanceRequired.Text = hillDistanceRequired.ToString("0.0#", CultureInfo.CurrentCulture);
            textBox_maxDescentElevation.Text = maxDescentElevation.ToString("0", CultureInfo.CurrentCulture);
            textBox_maxDescentLength.Text = maxDescentLength.ToString("0.0#", CultureInfo.CurrentCulture);
            textBox_minAvgGrade.Text = minAvgGrade.ToString("0.00", CultureInfo.CurrentCulture);
        }

        internal void ThemeChanged(ITheme visualTheme)
        {
            textBox_distancePercent.ThemeChanged(visualTheme);
            textBox_elevationPercent.ThemeChanged(visualTheme);
            textbox_gainElevationRequired.ThemeChanged(visualTheme);
            textbox_hillDistanceRequired.ThemeChanged(visualTheme);
            textBox_maxDescentElevation.ThemeChanged(visualTheme);
            textBox_maxDescentLength.ThemeChanged(visualTheme);
            textBox_minAvgGrade.ThemeChanged(visualTheme);
            lblElevReqd.ForeColor = visualTheme.ControlText;
            lblDistReqd.ForeColor = visualTheme.ControlText;
            lblMaxDescent.ForeColor = visualTheme.ControlText;
            lblMaxElevChg.ForeColor = visualTheme.ControlText;
            lblMinAvgGrade.ForeColor = visualTheme.ControlText;
            lblElevPct.ForeColor = visualTheme.ControlText;
            lblDistPct.ForeColor = visualTheme.ControlText;
            label_dist.ForeColor = visualTheme.ControlText;
            label_ele1.ForeColor = visualTheme.ControlText;
            label_ele.ForeColor = visualTheme.ControlText;
            label_dist1.ForeColor = visualTheme.ControlText;
            backPanel.ThemeChanged(visualTheme);
            backPanel.BackColor = visualTheme.Control;
        }

        // TOOD: Not sure how to use this
        internal void UICultureChanged(CultureInfo culture)
        {
            lblElevReqd.Text = Resources.Strings.Label_HillElevationRequired;
            lblDistReqd.Text = Resources.Strings.Label_HillDistanceRequired;
            lblMaxDescent.Text = Resources.Strings.Label_MaxDescentLength;
            lblMaxElevChg.Text = Resources.Strings.Label_MaxDescentElevationChange;
            lblMinAvgGrade.Text = Resources.Strings.Label_MinAvgGrade;
            lblElevPct.Text = Resources.Strings.Label_ElevationPercent;
            lblDistPct.Text = Resources.Strings.Label_DistancePercent;
            button_Defaults.Text = Resources.Strings.Label_Defaults;
            button_Ok.Text = CommonResources.Text.ActionOk;
            button_Cancel.Text = CommonResources.Text.ActionCancel;
            this.Text = Resources.Strings.Label_HillFinderSettings;
            label_dist.Text = Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.DistanceUnits);
            label_ele1.Text = Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.ElevationUnits);
            label_ele.Text = Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.ElevationUnits);
            label_dist1.Text = Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.DistanceUnits);
        }

        private void Ok_click(object sender, EventArgs e)
        {
            try
            {
                elevationPercent = Convert.ToDouble(textBox_elevationPercent.Text);
                distancePercent = Convert.ToDouble(textBox_distancePercent.Text);
                gainElevationRequired = Convert.ToDouble(textbox_gainElevationRequired.Text);
                hillDistanceRequired = Convert.ToDouble(textbox_hillDistanceRequired.Text);
                maxDescentElevation = Convert.ToDouble(textBox_maxDescentElevation.Text);
                maxDescentLength = Convert.ToDouble(textBox_maxDescentLength.Text);
                minAvgGrade = Convert.ToDouble(textBox_minAvgGrade.Text);
                this.DialogResult = DialogResult.OK;
            }
            catch
            {
                MessageBox.Show("Invalid entry.  Use number values only.");
            }
        }

        private void Defaults_click(object sender, EventArgs e)
        {
            double defaultdistancePercent = .33f; 
            double defaultelevationPercent = .33f;
            double defaultgainElevationRequired = Length.Convert(3f, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);
            double defaulthillDistanceRequired = Length.Convert(10f, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits);
            double defaultmaxDescentElevation = Length.Convert(1000f, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);
            double defaultmaxDescentLength = Length.Convert(1000f, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits);
            double defaultminAvgGrade = .01f;

            textBox_distancePercent.Text = defaultdistancePercent.ToString("0.00", CultureInfo.CurrentCulture);
            textBox_elevationPercent.Text = defaultelevationPercent.ToString("0.00", CultureInfo.CurrentCulture);
            textbox_gainElevationRequired.Text = defaultgainElevationRequired.ToString("0", CultureInfo.CurrentCulture);
            textbox_hillDistanceRequired.Text = defaulthillDistanceRequired.ToString("0.0#", CultureInfo.CurrentCulture);
            textBox_maxDescentElevation.Text = defaultmaxDescentElevation.ToString("0", CultureInfo.CurrentCulture);
            textBox_maxDescentLength.Text = defaultmaxDescentLength.ToString("0.0#", CultureInfo.CurrentCulture);
            textBox_minAvgGrade.Text = defaultminAvgGrade.ToString("0.00", CultureInfo.CurrentCulture);
        }

        private void Cancel_click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
