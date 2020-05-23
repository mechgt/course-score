using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Chart;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Data.Measurement;

namespace CourseScore.UI.Popups
{
    partial class SettingsPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.help1 = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.panel1 = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.panel2 = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.panel3 = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.panel4 = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.panel5 = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.panel6 = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.textbox_gainElevationRequired = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textbox_hillDistanceRequired = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_maxDescentLength = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_maxDescentElevation = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_minAvgGrade = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_elevationPercent = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_distancePercent = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.lblElevReqd = new System.Windows.Forms.Label();
            this.lblDistReqd = new System.Windows.Forms.Label();
            this.lblMaxDescent = new System.Windows.Forms.Label();
            this.lblMaxElevChg = new System.Windows.Forms.Label();
            this.lblMinAvgGrade = new System.Windows.Forms.Label();
            this.lblElevPct = new System.Windows.Forms.Label();
            this.lblDistPct = new System.Windows.Forms.Label();
            this.backPanel = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.label_ele1 = new System.Windows.Forms.Label();
            this.label_dist1 = new System.Windows.Forms.Label();
            this.label_ele = new System.Windows.Forms.Label();
            this.label_dist = new System.Windows.Forms.Label();
            this.button_Ok = new ZoneFiveSoftware.Common.Visuals.Button();
            this.button_Cancel = new ZoneFiveSoftware.Common.Visuals.Button();
            this.button_Defaults = new ZoneFiveSoftware.Common.Visuals.Button();
            this.backPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            // 
            // help1
            // 
            this.help1.BackColor = System.Drawing.Color.Transparent;
            this.help1.BackgroundImage = global::CourseScore.Resources.Images.help;
            this.help1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.help1.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.help1.BorderColor = System.Drawing.Color.Gray;
            this.help1.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.help1.HeadingFont = null;
            this.help1.HeadingLeftMargin = 0;
            this.help1.HeadingText = null;
            this.help1.HeadingTextColor = System.Drawing.Color.Black;
            this.help1.HeadingTopMargin = 3;
            this.help1.Location = new System.Drawing.Point(336, 34);
            this.help1.Name = "help1";
            this.help1.Size = new System.Drawing.Size(19, 19);
            this.help1.TabIndex = 26;
            this.toolTip.SetToolTip(this.help1, global::CourseScore.Resources.Strings.Tooltip_HillElevationRequired);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::CourseScore.Resources.Images.help;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.panel1.BorderColor = System.Drawing.Color.Gray;
            this.panel1.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.panel1.HeadingFont = null;
            this.panel1.HeadingLeftMargin = 0;
            this.panel1.HeadingText = null;
            this.panel1.HeadingTextColor = System.Drawing.Color.Black;
            this.panel1.HeadingTopMargin = 3;
            this.panel1.Location = new System.Drawing.Point(336, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(19, 19);
            this.panel1.TabIndex = 27;
            this.toolTip.SetToolTip(this.panel1, global::CourseScore.Resources.Strings.Tooltip_HillDistanceRequired);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::CourseScore.Resources.Images.help;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.panel2.BorderColor = System.Drawing.Color.Gray;
            this.panel2.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.panel2.HeadingFont = null;
            this.panel2.HeadingLeftMargin = 0;
            this.panel2.HeadingText = null;
            this.panel2.HeadingTextColor = System.Drawing.Color.Black;
            this.panel2.HeadingTopMargin = 3;
            this.panel2.Location = new System.Drawing.Point(336, 110);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(19, 19);
            this.panel2.TabIndex = 27;
            this.toolTip.SetToolTip(this.panel2, global::CourseScore.Resources.Strings.Tooltip_MaxDescentLength);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BackgroundImage = global::CourseScore.Resources.Images.help;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel3.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.panel3.BorderColor = System.Drawing.Color.Gray;
            this.panel3.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.panel3.HeadingFont = null;
            this.panel3.HeadingLeftMargin = 0;
            this.panel3.HeadingText = null;
            this.panel3.HeadingTextColor = System.Drawing.Color.Black;
            this.panel3.HeadingTopMargin = 3;
            this.panel3.Location = new System.Drawing.Point(336, 148);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(19, 19);
            this.panel3.TabIndex = 27;
            this.toolTip.SetToolTip(this.panel3, global::CourseScore.Resources.Strings.Tooltip_MaxDescentElevationChange);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.BackgroundImage = global::CourseScore.Resources.Images.help;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel4.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.panel4.BorderColor = System.Drawing.Color.Gray;
            this.panel4.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.panel4.HeadingFont = null;
            this.panel4.HeadingLeftMargin = 0;
            this.panel4.HeadingText = null;
            this.panel4.HeadingTextColor = System.Drawing.Color.Black;
            this.panel4.HeadingTopMargin = 3;
            this.panel4.Location = new System.Drawing.Point(336, 186);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(19, 19);
            this.panel4.TabIndex = 27;
            this.toolTip.SetToolTip(this.panel4, global::CourseScore.Resources.Strings.Tooltip_MinAvgGrade);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.BackgroundImage = global::CourseScore.Resources.Images.help;
            this.panel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel5.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.panel5.BorderColor = System.Drawing.Color.Gray;
            this.panel5.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.panel5.HeadingFont = null;
            this.panel5.HeadingLeftMargin = 0;
            this.panel5.HeadingText = null;
            this.panel5.HeadingTextColor = System.Drawing.Color.Black;
            this.panel5.HeadingTopMargin = 3;
            this.panel5.Location = new System.Drawing.Point(336, 224);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(19, 19);
            this.panel5.TabIndex = 27;
            this.toolTip.SetToolTip(this.panel5, global::CourseScore.Resources.Strings.Tooltip_ElevationPercent);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Transparent;
            this.panel6.BackgroundImage = global::CourseScore.Resources.Images.help;
            this.panel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel6.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.panel6.BorderColor = System.Drawing.Color.Gray;
            this.panel6.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.panel6.HeadingFont = null;
            this.panel6.HeadingLeftMargin = 0;
            this.panel6.HeadingText = null;
            this.panel6.HeadingTextColor = System.Drawing.Color.Black;
            this.panel6.HeadingTopMargin = 3;
            this.panel6.Location = new System.Drawing.Point(336, 262);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(19, 19);
            this.panel6.TabIndex = 27;
            this.toolTip.SetToolTip(this.panel6, global::CourseScore.Resources.Strings.Tooltip_DistancePercent);
            // 
            // textbox_gainElevationRequired
            // 
            this.textbox_gainElevationRequired.AcceptsReturn = false;
            this.textbox_gainElevationRequired.AcceptsTab = false;
            this.textbox_gainElevationRequired.BackColor = System.Drawing.Color.White;
            this.textbox_gainElevationRequired.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textbox_gainElevationRequired.ButtonImage = null;
            this.textbox_gainElevationRequired.Location = new System.Drawing.Point(15, 34);
            this.textbox_gainElevationRequired.MaxLength = 32767;
            this.textbox_gainElevationRequired.Multiline = false;
            this.textbox_gainElevationRequired.Name = "textbox_gainElevationRequired";
            this.textbox_gainElevationRequired.ReadOnly = false;
            this.textbox_gainElevationRequired.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textbox_gainElevationRequired.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textbox_gainElevationRequired.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textbox_gainElevationRequired.Size = new System.Drawing.Size(285, 19);
            this.textbox_gainElevationRequired.TabIndex = 8;
            this.textbox_gainElevationRequired.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textbox_hillDistanceRequired
            // 
            this.textbox_hillDistanceRequired.AcceptsReturn = false;
            this.textbox_hillDistanceRequired.AcceptsTab = false;
            this.textbox_hillDistanceRequired.BackColor = System.Drawing.Color.White;
            this.textbox_hillDistanceRequired.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textbox_hillDistanceRequired.ButtonImage = null;
            this.textbox_hillDistanceRequired.Location = new System.Drawing.Point(15, 72);
            this.textbox_hillDistanceRequired.MaxLength = 32767;
            this.textbox_hillDistanceRequired.Multiline = false;
            this.textbox_hillDistanceRequired.Name = "textbox_hillDistanceRequired";
            this.textbox_hillDistanceRequired.ReadOnly = false;
            this.textbox_hillDistanceRequired.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textbox_hillDistanceRequired.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textbox_hillDistanceRequired.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textbox_hillDistanceRequired.Size = new System.Drawing.Size(285, 19);
            this.textbox_hillDistanceRequired.TabIndex = 9;
            this.textbox_hillDistanceRequired.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_maxDescentLength
            // 
            this.textBox_maxDescentLength.AcceptsReturn = false;
            this.textBox_maxDescentLength.AcceptsTab = false;
            this.textBox_maxDescentLength.BackColor = System.Drawing.Color.White;
            this.textBox_maxDescentLength.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_maxDescentLength.ButtonImage = null;
            this.textBox_maxDescentLength.Location = new System.Drawing.Point(15, 110);
            this.textBox_maxDescentLength.MaxLength = 32767;
            this.textBox_maxDescentLength.Multiline = false;
            this.textBox_maxDescentLength.Name = "textBox_maxDescentLength";
            this.textBox_maxDescentLength.ReadOnly = false;
            this.textBox_maxDescentLength.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_maxDescentLength.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_maxDescentLength.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_maxDescentLength.Size = new System.Drawing.Size(285, 19);
            this.textBox_maxDescentLength.TabIndex = 10;
            this.textBox_maxDescentLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_maxDescentElevation
            // 
            this.textBox_maxDescentElevation.AcceptsReturn = false;
            this.textBox_maxDescentElevation.AcceptsTab = false;
            this.textBox_maxDescentElevation.BackColor = System.Drawing.Color.White;
            this.textBox_maxDescentElevation.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_maxDescentElevation.ButtonImage = null;
            this.textBox_maxDescentElevation.Location = new System.Drawing.Point(15, 148);
            this.textBox_maxDescentElevation.MaxLength = 32767;
            this.textBox_maxDescentElevation.Multiline = false;
            this.textBox_maxDescentElevation.Name = "textBox_maxDescentElevation";
            this.textBox_maxDescentElevation.ReadOnly = false;
            this.textBox_maxDescentElevation.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_maxDescentElevation.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_maxDescentElevation.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_maxDescentElevation.Size = new System.Drawing.Size(285, 19);
            this.textBox_maxDescentElevation.TabIndex = 11;
            this.textBox_maxDescentElevation.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_minAvgGrade
            // 
            this.textBox_minAvgGrade.AcceptsReturn = false;
            this.textBox_minAvgGrade.AcceptsTab = false;
            this.textBox_minAvgGrade.BackColor = System.Drawing.Color.White;
            this.textBox_minAvgGrade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_minAvgGrade.ButtonImage = null;
            this.textBox_minAvgGrade.Location = new System.Drawing.Point(15, 186);
            this.textBox_minAvgGrade.MaxLength = 32767;
            this.textBox_minAvgGrade.Multiline = false;
            this.textBox_minAvgGrade.Name = "textBox_minAvgGrade";
            this.textBox_minAvgGrade.ReadOnly = false;
            this.textBox_minAvgGrade.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_minAvgGrade.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_minAvgGrade.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_minAvgGrade.Size = new System.Drawing.Size(285, 19);
            this.textBox_minAvgGrade.TabIndex = 12;
            this.textBox_minAvgGrade.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_elevationPercent
            // 
            this.textBox_elevationPercent.AcceptsReturn = false;
            this.textBox_elevationPercent.AcceptsTab = false;
            this.textBox_elevationPercent.BackColor = System.Drawing.Color.White;
            this.textBox_elevationPercent.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_elevationPercent.ButtonImage = null;
            this.textBox_elevationPercent.Location = new System.Drawing.Point(15, 224);
            this.textBox_elevationPercent.MaxLength = 32767;
            this.textBox_elevationPercent.Multiline = false;
            this.textBox_elevationPercent.Name = "textBox_elevationPercent";
            this.textBox_elevationPercent.ReadOnly = false;
            this.textBox_elevationPercent.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_elevationPercent.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_elevationPercent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_elevationPercent.Size = new System.Drawing.Size(285, 19);
            this.textBox_elevationPercent.TabIndex = 13;
            this.textBox_elevationPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_distancePercent
            // 
            this.textBox_distancePercent.AcceptsReturn = false;
            this.textBox_distancePercent.AcceptsTab = false;
            this.textBox_distancePercent.BackColor = System.Drawing.Color.White;
            this.textBox_distancePercent.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_distancePercent.ButtonImage = null;
            this.textBox_distancePercent.Location = new System.Drawing.Point(15, 262);
            this.textBox_distancePercent.MaxLength = 32767;
            this.textBox_distancePercent.Multiline = false;
            this.textBox_distancePercent.Name = "textBox_distancePercent";
            this.textBox_distancePercent.ReadOnly = false;
            this.textBox_distancePercent.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_distancePercent.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_distancePercent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_distancePercent.Size = new System.Drawing.Size(285, 19);
            this.textBox_distancePercent.TabIndex = 14;
            this.textBox_distancePercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblElevReqd
            // 
            this.lblElevReqd.Location = new System.Drawing.Point(12, 18);
            this.lblElevReqd.Name = "lblElevReqd";
            this.lblElevReqd.Size = new System.Drawing.Size(343, 13);
            this.lblElevReqd.TabIndex = 15;
            this.lblElevReqd.Text = "Hill Elevation Required";
            // 
            // lblDistReqd
            // 
            this.lblDistReqd.Location = new System.Drawing.Point(12, 56);
            this.lblDistReqd.Name = "lblDistReqd";
            this.lblDistReqd.Size = new System.Drawing.Size(343, 13);
            this.lblDistReqd.TabIndex = 16;
            this.lblDistReqd.Text = "Hill Distance Required";
            // 
            // lblMaxDescent
            // 
            this.lblMaxDescent.Location = new System.Drawing.Point(12, 94);
            this.lblMaxDescent.Name = "lblMaxDescent";
            this.lblMaxDescent.Size = new System.Drawing.Size(343, 13);
            this.lblMaxDescent.TabIndex = 17;
            this.lblMaxDescent.Text = "Max Descent Length";
            // 
            // lblMaxElevChg
            // 
            this.lblMaxElevChg.Location = new System.Drawing.Point(12, 132);
            this.lblMaxElevChg.Name = "lblMaxElevChg";
            this.lblMaxElevChg.Size = new System.Drawing.Size(343, 13);
            this.lblMaxElevChg.TabIndex = 18;
            this.lblMaxElevChg.Text = "Max Descent Elevation Change";
            // 
            // lblMinAvgGrade
            // 
            this.lblMinAvgGrade.Location = new System.Drawing.Point(12, 170);
            this.lblMinAvgGrade.Name = "lblMinAvgGrade";
            this.lblMinAvgGrade.Size = new System.Drawing.Size(343, 13);
            this.lblMinAvgGrade.TabIndex = 19;
            this.lblMinAvgGrade.Text = "Min Average Grade";
            // 
            // lblElevPct
            // 
            this.lblElevPct.Location = new System.Drawing.Point(12, 208);
            this.lblElevPct.Name = "lblElevPct";
            this.lblElevPct.Size = new System.Drawing.Size(343, 13);
            this.lblElevPct.TabIndex = 20;
            this.lblElevPct.Text = "Elevation Percent";
            // 
            // lblDistPct
            // 
            this.lblDistPct.Location = new System.Drawing.Point(12, 246);
            this.lblDistPct.Name = "lblDistPct";
            this.lblDistPct.Size = new System.Drawing.Size(343, 13);
            this.lblDistPct.TabIndex = 21;
            this.lblDistPct.Text = "Distance Percent";
            // 
            // backPanel
            // 
            this.backPanel.BackColor = System.Drawing.Color.Transparent;
            this.backPanel.BorderColor = System.Drawing.Color.Gray;
            this.backPanel.Controls.Add(this.label_ele1);
            this.backPanel.Controls.Add(this.label_dist1);
            this.backPanel.Controls.Add(this.label_ele);
            this.backPanel.Controls.Add(this.label_dist);
            this.backPanel.Controls.Add(this.panel6);
            this.backPanel.Controls.Add(this.panel5);
            this.backPanel.Controls.Add(this.panel4);
            this.backPanel.Controls.Add(this.panel3);
            this.backPanel.Controls.Add(this.panel2);
            this.backPanel.Controls.Add(this.panel1);
            this.backPanel.Controls.Add(this.help1);
            this.backPanel.Controls.Add(this.button_Ok);
            this.backPanel.Controls.Add(this.button_Cancel);
            this.backPanel.Controls.Add(this.button_Defaults);
            this.backPanel.Controls.Add(this.textbox_gainElevationRequired);
            this.backPanel.Controls.Add(this.lblDistPct);
            this.backPanel.Controls.Add(this.lblElevPct);
            this.backPanel.Controls.Add(this.textbox_hillDistanceRequired);
            this.backPanel.Controls.Add(this.lblMinAvgGrade);
            this.backPanel.Controls.Add(this.textBox_maxDescentLength);
            this.backPanel.Controls.Add(this.lblMaxElevChg);
            this.backPanel.Controls.Add(this.textBox_maxDescentElevation);
            this.backPanel.Controls.Add(this.lblMaxDescent);
            this.backPanel.Controls.Add(this.textBox_minAvgGrade);
            this.backPanel.Controls.Add(this.lblDistReqd);
            this.backPanel.Controls.Add(this.textBox_elevationPercent);
            this.backPanel.Controls.Add(this.lblElevReqd);
            this.backPanel.Controls.Add(this.textBox_distancePercent);
            this.backPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backPanel.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.backPanel.HeadingFont = null;
            this.backPanel.HeadingLeftMargin = 0;
            this.backPanel.HeadingText = null;
            this.backPanel.HeadingTextColor = System.Drawing.Color.Black;
            this.backPanel.HeadingTopMargin = 3;
            this.backPanel.Location = new System.Drawing.Point(0, 0);
            this.backPanel.Name = "backPanel";
            this.backPanel.Size = new System.Drawing.Size(375, 352);
            this.backPanel.TabIndex = 22;
            // 
            // label_ele1
            // 
            this.label_ele1.AutoSize = true;
            this.label_ele1.Location = new System.Drawing.Point(306, 154);
            this.label_ele1.Name = "label_ele1";
            this.label_ele1.Size = new System.Drawing.Size(12, 13);
            this.label_ele1.TabIndex = 31;
            this.label_ele1.Text = "x";
            // 
            // label_dist1
            // 
            this.label_dist1.AutoSize = true;
            this.label_dist1.Location = new System.Drawing.Point(306, 116);
            this.label_dist1.Name = "label_dist1";
            this.label_dist1.Size = new System.Drawing.Size(12, 13);
            this.label_dist1.TabIndex = 30;
            this.label_dist1.Text = "x";
            // 
            // label_ele
            // 
            this.label_ele.AutoSize = true;
            this.label_ele.Location = new System.Drawing.Point(306, 40);
            this.label_ele.Name = "label_ele";
            this.label_ele.Size = new System.Drawing.Size(12, 13);
            this.label_ele.TabIndex = 29;
            this.label_ele.Text = "x";
            // 
            // label_dist
            // 
            this.label_dist.AutoSize = true;
            this.label_dist.Location = new System.Drawing.Point(306, 78);
            this.label_dist.Name = "label_dist";
            this.label_dist.Size = new System.Drawing.Size(12, 13);
            this.label_dist.TabIndex = 28;
            this.label_dist.Text = "x";
            // 
            // button_Ok
            // 
            this.button_Ok.BackColor = System.Drawing.Color.Transparent;
            this.button_Ok.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.button_Ok.CenterImage = null;
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.None;
            this.button_Ok.HyperlinkStyle = false;
            this.button_Ok.ImageMargin = 2;
            this.button_Ok.LeftImage = null;
            this.button_Ok.Location = new System.Drawing.Point(280, 307);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.PushStyle = true;
            this.button_Ok.RightImage = null;
            this.button_Ok.Size = new System.Drawing.Size(75, 23);
            this.button_Ok.TabIndex = 25;
            this.button_Ok.Text = "OK";
            this.button_Ok.TextAlign = System.Drawing.StringAlignment.Center;
            this.button_Ok.TextLeftMargin = 2;
            this.button_Ok.TextRightMargin = 2;
            this.button_Ok.Click += new System.EventHandler(this.Ok_click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.BackColor = System.Drawing.Color.Transparent;
            this.button_Cancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.button_Cancel.CenterImage = null;
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.button_Cancel.HyperlinkStyle = false;
            this.button_Cancel.ImageMargin = 2;
            this.button_Cancel.LeftImage = null;
            this.button_Cancel.Location = new System.Drawing.Point(199, 307);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.PushStyle = true;
            this.button_Cancel.RightImage = null;
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 24;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.TextAlign = System.Drawing.StringAlignment.Center;
            this.button_Cancel.TextLeftMargin = 2;
            this.button_Cancel.TextRightMargin = 2;
            this.button_Cancel.Click += new System.EventHandler(this.Cancel_click);
            // 
            // button_Defaults
            // 
            this.button_Defaults.BackColor = System.Drawing.Color.Transparent;
            this.button_Defaults.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.button_Defaults.CenterImage = null;
            this.button_Defaults.DialogResult = System.Windows.Forms.DialogResult.None;
            this.button_Defaults.HyperlinkStyle = false;
            this.button_Defaults.ImageMargin = 2;
            this.button_Defaults.LeftImage = null;
            this.button_Defaults.Location = new System.Drawing.Point(12, 307);
            this.button_Defaults.Name = "button_Defaults";
            this.button_Defaults.PushStyle = true;
            this.button_Defaults.RightImage = null;
            this.button_Defaults.Size = new System.Drawing.Size(75, 23);
            this.button_Defaults.TabIndex = 22;
            this.button_Defaults.Text = "Defaults";
            this.button_Defaults.TextAlign = System.Drawing.StringAlignment.Center;
            this.button_Defaults.TextLeftMargin = 2;
            this.button_Defaults.TextRightMargin = 2;
            this.button_Defaults.Click += new System.EventHandler(this.Defaults_click);
            // 
            // SettingsPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 352);
            this.Controls.Add(this.backPanel);
            this.Name = "SettingsPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hill Finder Settings";
            this.backPanel.ResumeLayout(false);
            this.backPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        private ZoneFiveSoftware.Common.Visuals.TextBox textbox_gainElevationRequired;
        private ZoneFiveSoftware.Common.Visuals.TextBox textbox_hillDistanceRequired;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_maxDescentLength;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_maxDescentElevation;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_minAvgGrade;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_elevationPercent;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_distancePercent;
        private System.Windows.Forms.Label lblElevReqd;
        private System.Windows.Forms.Label lblDistReqd;
        private System.Windows.Forms.Label lblMaxDescent;
        private System.Windows.Forms.Label lblMaxElevChg;
        private System.Windows.Forms.Label lblMinAvgGrade;
        private System.Windows.Forms.Label lblElevPct;
        private System.Windows.Forms.Label lblDistPct;
        private ZoneFiveSoftware.Common.Visuals.Panel backPanel;
        private ZoneFiveSoftware.Common.Visuals.Button button_Defaults;
        private ZoneFiveSoftware.Common.Visuals.Button button_Cancel;
        private ZoneFiveSoftware.Common.Visuals.Button button_Ok;
        private ZoneFiveSoftware.Common.Visuals.Panel help1;
        private ZoneFiveSoftware.Common.Visuals.Panel panel6;
        private ZoneFiveSoftware.Common.Visuals.Panel panel5;
        private ZoneFiveSoftware.Common.Visuals.Panel panel4;
        private ZoneFiveSoftware.Common.Visuals.Panel panel3;
        private ZoneFiveSoftware.Common.Visuals.Panel panel2;
        private ZoneFiveSoftware.Common.Visuals.Panel panel1;
        private System.Windows.Forms.Label label_dist;
        private System.Windows.Forms.Label label_ele1;
        private System.Windows.Forms.Label label_dist1;
        private System.Windows.Forms.Label label_ele;
    }
}