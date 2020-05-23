using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Chart;
using CourseScore.Data;
using System.Diagnostics;

namespace CourseScore.Settings
{
    public partial class SettingsPageControl : UserControl
    {
        #region Fields

        #endregion

        #region Constructor

        public SettingsPageControl()
        {
            InitializeComponent();
            
            foreach (ScoreEquation.Score equation in System.Enum.GetValues(typeof(ScoreEquation.Score)))
            {
                comboBox_hillScore.Items.Add(new ScoreEquation(equation));
            }
            textBox_factor.Text = GlobalSettings.Instance.CyclingFactor.ToString("0.000", CultureInfo.CurrentCulture);
            textBox_offset.Text = GlobalSettings.Instance.CyclingOffset.ToString("0.000", CultureInfo.CurrentCulture);

            comboBox_sport.Items.Add(Resources.Strings.Label_Cycling);
            comboBox_sport.Items.Add(Resources.Strings.Label_Running);
            comboBox_sport.SelectedItem = Resources.Strings.Label_Cycling;

            label_factor.Text = "Cycling Course Score Factor";
            label_offset.Text = "Cycling Course Score Offset";
            label_hillScore.Text = Resources.Strings.Label_HillScoreEquation;
            label_sport.Text = Resources.Strings.Label_Sport;
        }

        #endregion

        #region Control Functions

        public void RefreshPage()
        {
            for(int i=0; i<comboBox_hillScore.Items.Count; i++)
            {
                ScoreEquation current = (ScoreEquation)comboBox_hillScore.Items[i];
                if (current.equation == GlobalSettings.Instance.ScoreEquation)
                {
                    comboBox_hillScore.SelectedIndex = i;
                    break;
                }
            }
            if (comboBox_sport.SelectedItem == Resources.Strings.Label_Cycling)
            {
                RefreshChartCycling();
            }
            else if (comboBox_sport.SelectedItem == Resources.Strings.Label_Running)
            {
                RefreshChartRunning();
            }
        }

        internal void ThemeChanged(ITheme visualTheme)
        {
        }

        internal void UICultureChanged(CultureInfo culture)
        {
            label_factor.Text = "Cycling Course Score Factor";
            label_offset.Text = "Cycling Course Score Offset";
            label_hillScore.Text = Resources.Strings.Label_HillScoreEquation;
            label_sport.Text = Resources.Strings.Label_Sport;
        }

        #endregion

        #region Graph

        private void RefreshChartCycling()
        {
            // Initialize the charts, dataseries, and variables
            IAxis axis = scoreChart.YAxis;
            ChartDataSeries dsFactor = new ChartDataSeries(scoreChart, axis);
            ChartDataSeries dsFactorLine = new ChartDataSeries(scoreChart, axis);
            PointF point = new PointF();

            double factor = GlobalSettings.Instance.CyclingFactor;
            double offset = GlobalSettings.Instance.CyclingOffset;

            // Populate the data series.  Cycling only scores on uphills
            for (int i = 0; i <= 100; i++)
            {
                point.X = (float)i;
                point.Y = (float)((factor * i) + offset);
                dsFactor.Points.Add(i, point);
            }

            // Prepare the axis and the dataseries
            axis.Label = Resources.Strings.Label_Score + "/km";
            scoreChart.XAxis.Label = CommonResources.Text.LabelGrade;
            scoreChart.DataSeries.Clear();
            dsFactor.ChartType = ChartDataSeries.Type.Line;
            dsFactor.LineColor = Color.Blue;
            dsFactor.ValueAxis = axis;

            // Add the data series to the chart
            scoreChart.DataSeries.Add(dsFactor);

            // Autozoom from 0-15
            scoreChart.AutozoomToData(true);
            double ratio = (scoreChart.XAxis.MaxOriginFarValue - scoreChart.XAxis.OriginValue) / 15;
            scoreChart.XAxis.PixelsPerValue = scoreChart.XAxis.PixelsPerValue * ratio;

            ratio = (((factor * 100) + offset) - scoreChart.YAxis.OriginValue) / ((factor * 15) + offset);
            scoreChart.YAxis.PixelsPerValue = scoreChart.YAxis.PixelsPerValue * ratio;
        }

        private void RefreshChartRunning()
        {
            // Initialize the charts, dataseries, and variables
            IAxis axis = scoreChart.YAxis;
            ChartDataSeries dsFactor = new ChartDataSeries(scoreChart, axis);
            ChartDataSeries dsFactorLine = new ChartDataSeries(scoreChart, axis);
            PointF point = new PointF();

            // Prep the variables for the running equation
            double grade = 0;
            double g = 0;
            double g0 = .184f;
            double a0 = 1.68f;
            double a1 = 54.9f;
            double a2 = -102f;
            double a3 = 200f;
            double de = 0;
            double pos15 = 0;

            // Running scores from -100% to 100%, do the math
            for (float i = -100; i <= 100; i++)
            {
                grade = i/100f;
                g = grade / Math.Sqrt(1 + grade * grade);
                de = a0 + a1 * Math.Pow(g + g0, 2) + a2 * Math.Pow(g + g0, 4) + a3 * Math.Pow(g + g0, 6);
                de = de - 1.5f;
                point.X = i;
                point.Y = (float)de;
                dsFactor.Points.Add(i, point);

                // Find the value at grade = 15 for our autozoom
                if (i == 15)
                {
                    pos15 = de;
                }
            }

            // Set up the axis and the chart
            axis.Label = Resources.Strings.Label_Score + "/km";
            scoreChart.XAxis.Label = CommonResources.Text.LabelGrade;
            scoreChart.DataSeries.Clear();
            dsFactor.ChartType = ChartDataSeries.Type.Line;
            dsFactor.LineColor = Color.Blue;
            dsFactor.ValueAxis = axis;

            // Add the dataseries to the chart
            scoreChart.DataSeries.Add(dsFactor);

            // Autozoom from -15 to 15
            scoreChart.AutozoomToData(true);
            double ratio = (scoreChart.XAxis.MaxOriginFarValue - scoreChart.XAxis.OriginValue) / 30;
            scoreChart.XAxis.PixelsPerValue = scoreChart.XAxis.PixelsPerValue * ratio;
            scoreChart.XAxis.OriginValue = -15;

            ratio = (de - scoreChart.YAxis.OriginValue) / pos15;
            scoreChart.YAxis.PixelsPerValue = scoreChart.YAxis.PixelsPerValue * ratio;
        }

        #endregion

        #region Event Handlers

        private void hillScore_selectedValueChanged(object sender, EventArgs e)
        {
            ComboBox score = (ComboBox)sender;
            ScoreEquation current = (ScoreEquation)score.SelectedItem;
            GlobalSettings.Instance.ScoreEquation = current.equation;
        }

        private void CyclingOffset_leave(object sender, EventArgs e)
        {
            try
            {
                GlobalSettings.Instance.CyclingOffset = Convert.ToDouble(textBox_offset.Text);
                RefreshChartCycling();
            }
            catch
            {
                MessageBox.Show(Resources.Strings.Message_InvalidCharacter);
            }
        }

        private void CyclingFactor_leave(object sender, EventArgs e)
        {
            try
            {
                GlobalSettings.Instance.CyclingFactor = Convert.ToDouble(textBox_factor.Text);
                RefreshChartCycling();
            }
            catch
            {
                MessageBox.Show(Resources.Strings.Message_InvalidCharacter);
            }
        }

        private void sport_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox_sport.SelectedItem == Resources.Strings.Label_Cycling)
            {
                RefreshChartCycling();
                textBox_factor.Visible = true;
                label_factor.Visible = true;
                textBox_offset.Visible = true;
                label_offset.Visible = true;
            }
            else if (comboBox_sport.SelectedItem == Resources.Strings.Label_Running)
            {
                RefreshChartRunning();
                textBox_factor.Visible = false;
                label_factor.Visible = false;
                textBox_offset.Visible = false;
                label_offset.Visible = false;
            }
        }

        /// <summary>
        /// DigitValidator will make sure what was typed in the box is a digit or decimal
        /// </summary>
        private void digitValidator(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true; // input is not passed on to the control(TextBox)`
            }
        }

        #endregion

    }
}
