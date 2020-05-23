using ZoneFiveSoftware.Common.Visuals;

namespace CourseScore.UI.DetailPage
{
    partial class HillsDetailControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HillsDetailControl));
            this.pnlMain = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_courseScore = new ZoneFiveSoftware.Common.Visuals.Button();
            this.scoreMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel_climb = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.textBox_HC_distance = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_HC_score = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_HC_elevationGain = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_HC_grade = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.label_hardestClimb = new System.Windows.Forms.Label();
            this.label_steepestClimb = new System.Windows.Forms.Label();
            this.label_longestClimb = new System.Windows.Forms.Label();
            this.textBox_MID_distance = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.label_LC_distance = new System.Windows.Forms.Label();
            this.textBox_MID_score = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_LC_distance = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_LC_score = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.label_LC_elevationGain = new System.Windows.Forms.Label();
            this.textBox_MID_elevationGain = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.label_LC_score = new System.Windows.Forms.Label();
            this.textBox_LC_elevationGain = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_MID_grade = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.label_LC_grade = new System.Windows.Forms.Label();
            this.textBox_LC_grade = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.textBox_scoreDistance = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.label_scoreDistance = new System.Windows.Forms.Label();
            this.textBox_courseScore = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.label_courseScore = new System.Windows.Forms.Label();
            this.textBox_climb = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.label_climb = new System.Windows.Forms.Label();
            this.textBox_distance = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.label_distance = new System.Windows.Forms.Label();
            this.infoBanner = new ZoneFiveSoftware.Common.Visuals.ActionBanner();
            this.infoMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TreeRefreshButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.treeList1 = new ZoneFiveSoftware.Common.Visuals.TreeList();
            this.treeList_subtotals = new ZoneFiveSoftware.Common.Visuals.TreeList();
            this.panelMain = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.MainChart = new ZoneFiveSoftware.Common.Visuals.Chart.ChartBase();
            this.ButtonPanel = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.HillMarkersButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.colorMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.button_color = new ZoneFiveSoftware.Common.Visuals.Button();
            this.button1 = new ZoneFiveSoftware.Common.Visuals.Button();
            this.ZoomInButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.ZoomOutButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.ZoomChartButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.ExtraChartsButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.SaveImageButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.ChartBanner = new ZoneFiveSoftware.Common.Visuals.ActionBanner();
            this.detailMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MaximizeButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.allActs = new System.Windows.Forms.ToolStripMenuItem();
            this.treeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.splitsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnlMain.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel_climb.SuspendLayout();
            this.infoBanner.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.ButtonPanel.SuspendLayout();
            this.ChartBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.pnlMain.BorderColor = System.Drawing.Color.Gray;
            this.pnlMain.Controls.Add(this.splitContainer1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.pnlMain.HeadingFont = null;
            this.pnlMain.HeadingLeftMargin = 0;
            this.pnlMain.HeadingText = null;
            this.pnlMain.HeadingTextColor = System.Drawing.Color.Black;
            this.pnlMain.HeadingTopMargin = 3;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(515, 558);
            this.pnlMain.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button_courseScore);
            this.splitContainer1.Panel1.Controls.Add(this.panel_climb);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_scoreDistance);
            this.splitContainer1.Panel1.Controls.Add(this.label_scoreDistance);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_courseScore);
            this.splitContainer1.Panel1.Controls.Add(this.label_courseScore);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_climb);
            this.splitContainer1.Panel1.Controls.Add(this.label_climb);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_distance);
            this.splitContainer1.Panel1.Controls.Add(this.label_distance);
            this.splitContainer1.Panel1.Controls.Add(this.infoBanner);
            this.splitContainer1.Panel1.Controls.Add(this.treeList1);
            this.splitContainer1.Panel1.Controls.Add(this.treeList_subtotals);
            this.splitContainer1.Panel1MinSize = 190;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelMain);
            this.splitContainer1.Size = new System.Drawing.Size(515, 558);
            this.splitContainer1.SplitterDistance = 190;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.splitter_mouseDown);
            // 
            // button_courseScore
            // 
            this.button_courseScore.BackColor = System.Drawing.Color.Transparent;
            this.button_courseScore.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_courseScore.BackgroundImage")));
            this.button_courseScore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_courseScore.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.button_courseScore.CenterImage = null;
            this.button_courseScore.ContextMenuStrip = this.scoreMenu;
            this.button_courseScore.DialogResult = System.Windows.Forms.DialogResult.None;
            this.button_courseScore.HyperlinkStyle = false;
            this.button_courseScore.ImageMargin = 2;
            this.button_courseScore.LeftImage = null;
            this.button_courseScore.Location = new System.Drawing.Point(232, 53);
            this.button_courseScore.Name = "button_courseScore";
            this.button_courseScore.PushStyle = true;
            this.button_courseScore.RightImage = null;
            this.button_courseScore.Size = new System.Drawing.Size(10, 16);
            this.button_courseScore.TabIndex = 49;
            this.button_courseScore.TextAlign = System.Drawing.StringAlignment.Center;
            this.button_courseScore.TextLeftMargin = 2;
            this.button_courseScore.TextRightMargin = 2;
            this.button_courseScore.Click += new System.EventHandler(this.score_click);
            // 
            // scoreMenu
            // 
            this.scoreMenu.Name = "scoreMenu";
            this.scoreMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // panel_climb
            // 
            this.panel_climb.BackColor = System.Drawing.Color.Transparent;
            this.panel_climb.BorderColor = System.Drawing.Color.Gray;
            this.panel_climb.Controls.Add(this.textBox_HC_distance);
            this.panel_climb.Controls.Add(this.textBox_HC_score);
            this.panel_climb.Controls.Add(this.textBox_HC_elevationGain);
            this.panel_climb.Controls.Add(this.textBox_HC_grade);
            this.panel_climb.Controls.Add(this.label_hardestClimb);
            this.panel_climb.Controls.Add(this.label_steepestClimb);
            this.panel_climb.Controls.Add(this.label_longestClimb);
            this.panel_climb.Controls.Add(this.textBox_MID_distance);
            this.panel_climb.Controls.Add(this.label_LC_distance);
            this.panel_climb.Controls.Add(this.textBox_MID_score);
            this.panel_climb.Controls.Add(this.textBox_LC_distance);
            this.panel_climb.Controls.Add(this.textBox_LC_score);
            this.panel_climb.Controls.Add(this.label_LC_elevationGain);
            this.panel_climb.Controls.Add(this.textBox_MID_elevationGain);
            this.panel_climb.Controls.Add(this.label_LC_score);
            this.panel_climb.Controls.Add(this.textBox_LC_elevationGain);
            this.panel_climb.Controls.Add(this.textBox_MID_grade);
            this.panel_climb.Controls.Add(this.label_LC_grade);
            this.panel_climb.Controls.Add(this.textBox_LC_grade);
            this.panel_climb.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.panel_climb.HeadingFont = null;
            this.panel_climb.HeadingLeftMargin = 0;
            this.panel_climb.HeadingText = null;
            this.panel_climb.HeadingTextColor = System.Drawing.Color.Black;
            this.panel_climb.HeadingTopMargin = 3;
            this.panel_climb.Location = new System.Drawing.Point(6, 80);
            this.panel_climb.Name = "panel_climb";
            this.panel_climb.Size = new System.Drawing.Size(506, 106);
            this.panel_climb.TabIndex = 48;
            // 
            // textBox_HC_distance
            // 
            this.textBox_HC_distance.AcceptsReturn = false;
            this.textBox_HC_distance.AcceptsTab = false;
            this.textBox_HC_distance.BackColor = System.Drawing.Color.White;
            this.textBox_HC_distance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_HC_distance.ButtonImage = null;
            this.textBox_HC_distance.Enabled = false;
            this.textBox_HC_distance.Location = new System.Drawing.Point(92, 19);
            this.textBox_HC_distance.MaxLength = 32767;
            this.textBox_HC_distance.Multiline = false;
            this.textBox_HC_distance.Name = "textBox_HC_distance";
            this.textBox_HC_distance.ReadOnly = true;
            this.textBox_HC_distance.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_HC_distance.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_HC_distance.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_HC_distance.Size = new System.Drawing.Size(120, 19);
            this.textBox_HC_distance.TabIndex = 20;
            this.textBox_HC_distance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_HC_score
            // 
            this.textBox_HC_score.AcceptsReturn = false;
            this.textBox_HC_score.AcceptsTab = false;
            this.textBox_HC_score.BackColor = System.Drawing.Color.White;
            this.textBox_HC_score.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_HC_score.ButtonImage = null;
            this.textBox_HC_score.Enabled = false;
            this.textBox_HC_score.Location = new System.Drawing.Point(92, 61);
            this.textBox_HC_score.MaxLength = 32767;
            this.textBox_HC_score.Multiline = false;
            this.textBox_HC_score.Name = "textBox_HC_score";
            this.textBox_HC_score.ReadOnly = true;
            this.textBox_HC_score.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_HC_score.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_HC_score.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_HC_score.Size = new System.Drawing.Size(120, 19);
            this.textBox_HC_score.TabIndex = 32;
            this.textBox_HC_score.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_HC_elevationGain
            // 
            this.textBox_HC_elevationGain.AcceptsReturn = false;
            this.textBox_HC_elevationGain.AcceptsTab = false;
            this.textBox_HC_elevationGain.BackColor = System.Drawing.Color.White;
            this.textBox_HC_elevationGain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_HC_elevationGain.ButtonImage = null;
            this.textBox_HC_elevationGain.Enabled = false;
            this.textBox_HC_elevationGain.Location = new System.Drawing.Point(92, 40);
            this.textBox_HC_elevationGain.MaxLength = 32767;
            this.textBox_HC_elevationGain.Multiline = false;
            this.textBox_HC_elevationGain.Name = "textBox_HC_elevationGain";
            this.textBox_HC_elevationGain.ReadOnly = true;
            this.textBox_HC_elevationGain.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_HC_elevationGain.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_HC_elevationGain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_HC_elevationGain.Size = new System.Drawing.Size(120, 19);
            this.textBox_HC_elevationGain.TabIndex = 22;
            this.textBox_HC_elevationGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_HC_grade
            // 
            this.textBox_HC_grade.AcceptsReturn = false;
            this.textBox_HC_grade.AcceptsTab = false;
            this.textBox_HC_grade.BackColor = System.Drawing.Color.White;
            this.textBox_HC_grade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_HC_grade.ButtonImage = null;
            this.textBox_HC_grade.Enabled = false;
            this.textBox_HC_grade.Location = new System.Drawing.Point(92, 82);
            this.textBox_HC_grade.MaxLength = 32767;
            this.textBox_HC_grade.Multiline = false;
            this.textBox_HC_grade.Name = "textBox_HC_grade";
            this.textBox_HC_grade.ReadOnly = true;
            this.textBox_HC_grade.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_HC_grade.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_HC_grade.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_HC_grade.Size = new System.Drawing.Size(120, 19);
            this.textBox_HC_grade.TabIndex = 24;
            this.textBox_HC_grade.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label_hardestClimb
            // 
            this.label_hardestClimb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_hardestClimb.Location = new System.Drawing.Point(89, 3);
            this.label_hardestClimb.Name = "label_hardestClimb";
            this.label_hardestClimb.Size = new System.Drawing.Size(123, 13);
            this.label_hardestClimb.TabIndex = 18;
            this.label_hardestClimb.Text = "Hardest Climb";
            // 
            // label_steepestClimb
            // 
            this.label_steepestClimb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_steepestClimb.Location = new System.Drawing.Point(377, 3);
            this.label_steepestClimb.Name = "label_steepestClimb";
            this.label_steepestClimb.Size = new System.Drawing.Size(123, 13);
            this.label_steepestClimb.TabIndex = 11;
            this.label_steepestClimb.Text = "Steepest Climb";
            // 
            // label_longestClimb
            // 
            this.label_longestClimb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_longestClimb.Location = new System.Drawing.Point(237, 3);
            this.label_longestClimb.Name = "label_longestClimb";
            this.label_longestClimb.Size = new System.Drawing.Size(120, 13);
            this.label_longestClimb.TabIndex = 11;
            this.label_longestClimb.Text = "Longest Climb";
            // 
            // textBox_MID_distance
            // 
            this.textBox_MID_distance.AcceptsReturn = false;
            this.textBox_MID_distance.AcceptsTab = false;
            this.textBox_MID_distance.BackColor = System.Drawing.Color.White;
            this.textBox_MID_distance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_MID_distance.ButtonImage = null;
            this.textBox_MID_distance.Enabled = false;
            this.textBox_MID_distance.Location = new System.Drawing.Point(380, 19);
            this.textBox_MID_distance.MaxLength = 32767;
            this.textBox_MID_distance.Multiline = false;
            this.textBox_MID_distance.Name = "textBox_MID_distance";
            this.textBox_MID_distance.ReadOnly = true;
            this.textBox_MID_distance.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_MID_distance.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_MID_distance.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_MID_distance.Size = new System.Drawing.Size(120, 19);
            this.textBox_MID_distance.TabIndex = 13;
            this.textBox_MID_distance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label_LC_distance
            // 
            this.label_LC_distance.Location = new System.Drawing.Point(3, 23);
            this.label_LC_distance.Name = "label_LC_distance";
            this.label_LC_distance.Size = new System.Drawing.Size(83, 13);
            this.label_LC_distance.TabIndex = 12;
            this.label_LC_distance.Text = "Distance:";
            this.label_LC_distance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBox_MID_score
            // 
            this.textBox_MID_score.AcceptsReturn = false;
            this.textBox_MID_score.AcceptsTab = false;
            this.textBox_MID_score.BackColor = System.Drawing.Color.White;
            this.textBox_MID_score.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_MID_score.ButtonImage = null;
            this.textBox_MID_score.Enabled = false;
            this.textBox_MID_score.Location = new System.Drawing.Point(380, 61);
            this.textBox_MID_score.MaxLength = 32767;
            this.textBox_MID_score.Multiline = false;
            this.textBox_MID_score.Name = "textBox_MID_score";
            this.textBox_MID_score.ReadOnly = true;
            this.textBox_MID_score.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_MID_score.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_MID_score.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_MID_score.Size = new System.Drawing.Size(120, 19);
            this.textBox_MID_score.TabIndex = 30;
            this.textBox_MID_score.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_LC_distance
            // 
            this.textBox_LC_distance.AcceptsReturn = false;
            this.textBox_LC_distance.AcceptsTab = false;
            this.textBox_LC_distance.BackColor = System.Drawing.Color.White;
            this.textBox_LC_distance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_LC_distance.ButtonImage = null;
            this.textBox_LC_distance.Enabled = false;
            this.textBox_LC_distance.Location = new System.Drawing.Point(237, 19);
            this.textBox_LC_distance.MaxLength = 32767;
            this.textBox_LC_distance.Multiline = false;
            this.textBox_LC_distance.Name = "textBox_LC_distance";
            this.textBox_LC_distance.ReadOnly = true;
            this.textBox_LC_distance.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_LC_distance.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_LC_distance.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_LC_distance.Size = new System.Drawing.Size(120, 19);
            this.textBox_LC_distance.TabIndex = 13;
            this.textBox_LC_distance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_LC_score
            // 
            this.textBox_LC_score.AcceptsReturn = false;
            this.textBox_LC_score.AcceptsTab = false;
            this.textBox_LC_score.BackColor = System.Drawing.Color.White;
            this.textBox_LC_score.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_LC_score.ButtonImage = null;
            this.textBox_LC_score.Enabled = false;
            this.textBox_LC_score.Location = new System.Drawing.Point(237, 61);
            this.textBox_LC_score.MaxLength = 32767;
            this.textBox_LC_score.Multiline = false;
            this.textBox_LC_score.Name = "textBox_LC_score";
            this.textBox_LC_score.ReadOnly = true;
            this.textBox_LC_score.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_LC_score.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_LC_score.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_LC_score.Size = new System.Drawing.Size(120, 19);
            this.textBox_LC_score.TabIndex = 30;
            this.textBox_LC_score.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label_LC_elevationGain
            // 
            this.label_LC_elevationGain.Location = new System.Drawing.Point(3, 45);
            this.label_LC_elevationGain.Name = "label_LC_elevationGain";
            this.label_LC_elevationGain.Size = new System.Drawing.Size(83, 13);
            this.label_LC_elevationGain.TabIndex = 14;
            this.label_LC_elevationGain.Text = "Elev. chg.:";
            this.label_LC_elevationGain.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBox_MID_elevationGain
            // 
            this.textBox_MID_elevationGain.AcceptsReturn = false;
            this.textBox_MID_elevationGain.AcceptsTab = false;
            this.textBox_MID_elevationGain.BackColor = System.Drawing.Color.White;
            this.textBox_MID_elevationGain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_MID_elevationGain.ButtonImage = null;
            this.textBox_MID_elevationGain.Enabled = false;
            this.textBox_MID_elevationGain.Location = new System.Drawing.Point(380, 40);
            this.textBox_MID_elevationGain.MaxLength = 32767;
            this.textBox_MID_elevationGain.Multiline = false;
            this.textBox_MID_elevationGain.Name = "textBox_MID_elevationGain";
            this.textBox_MID_elevationGain.ReadOnly = true;
            this.textBox_MID_elevationGain.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_MID_elevationGain.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_MID_elevationGain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_MID_elevationGain.Size = new System.Drawing.Size(120, 19);
            this.textBox_MID_elevationGain.TabIndex = 15;
            this.textBox_MID_elevationGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label_LC_score
            // 
            this.label_LC_score.Location = new System.Drawing.Point(3, 66);
            this.label_LC_score.Name = "label_LC_score";
            this.label_LC_score.Size = new System.Drawing.Size(83, 13);
            this.label_LC_score.TabIndex = 29;
            this.label_LC_score.Text = "Score:";
            this.label_LC_score.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBox_LC_elevationGain
            // 
            this.textBox_LC_elevationGain.AcceptsReturn = false;
            this.textBox_LC_elevationGain.AcceptsTab = false;
            this.textBox_LC_elevationGain.BackColor = System.Drawing.Color.White;
            this.textBox_LC_elevationGain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_LC_elevationGain.ButtonImage = null;
            this.textBox_LC_elevationGain.Enabled = false;
            this.textBox_LC_elevationGain.Location = new System.Drawing.Point(237, 40);
            this.textBox_LC_elevationGain.MaxLength = 32767;
            this.textBox_LC_elevationGain.Multiline = false;
            this.textBox_LC_elevationGain.Name = "textBox_LC_elevationGain";
            this.textBox_LC_elevationGain.ReadOnly = true;
            this.textBox_LC_elevationGain.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_LC_elevationGain.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_LC_elevationGain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_LC_elevationGain.Size = new System.Drawing.Size(120, 19);
            this.textBox_LC_elevationGain.TabIndex = 15;
            this.textBox_LC_elevationGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_MID_grade
            // 
            this.textBox_MID_grade.AcceptsReturn = false;
            this.textBox_MID_grade.AcceptsTab = false;
            this.textBox_MID_grade.BackColor = System.Drawing.Color.White;
            this.textBox_MID_grade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_MID_grade.ButtonImage = null;
            this.textBox_MID_grade.Enabled = false;
            this.textBox_MID_grade.Location = new System.Drawing.Point(380, 82);
            this.textBox_MID_grade.MaxLength = 32767;
            this.textBox_MID_grade.Multiline = false;
            this.textBox_MID_grade.Name = "textBox_MID_grade";
            this.textBox_MID_grade.ReadOnly = true;
            this.textBox_MID_grade.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_MID_grade.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_MID_grade.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_MID_grade.Size = new System.Drawing.Size(120, 19);
            this.textBox_MID_grade.TabIndex = 17;
            this.textBox_MID_grade.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label_LC_grade
            // 
            this.label_LC_grade.Location = new System.Drawing.Point(3, 87);
            this.label_LC_grade.Name = "label_LC_grade";
            this.label_LC_grade.Size = new System.Drawing.Size(83, 13);
            this.label_LC_grade.TabIndex = 16;
            this.label_LC_grade.Text = "Grade:";
            this.label_LC_grade.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBox_LC_grade
            // 
            this.textBox_LC_grade.AcceptsReturn = false;
            this.textBox_LC_grade.AcceptsTab = false;
            this.textBox_LC_grade.BackColor = System.Drawing.Color.White;
            this.textBox_LC_grade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_LC_grade.ButtonImage = null;
            this.textBox_LC_grade.Enabled = false;
            this.textBox_LC_grade.Location = new System.Drawing.Point(237, 82);
            this.textBox_LC_grade.MaxLength = 32767;
            this.textBox_LC_grade.Multiline = false;
            this.textBox_LC_grade.Name = "textBox_LC_grade";
            this.textBox_LC_grade.ReadOnly = true;
            this.textBox_LC_grade.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_LC_grade.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_LC_grade.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_LC_grade.Size = new System.Drawing.Size(120, 19);
            this.textBox_LC_grade.TabIndex = 17;
            this.textBox_LC_grade.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // textBox_scoreDistance
            // 
            this.textBox_scoreDistance.AcceptsReturn = false;
            this.textBox_scoreDistance.AcceptsTab = false;
            this.textBox_scoreDistance.BackColor = System.Drawing.Color.White;
            this.textBox_scoreDistance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_scoreDistance.ButtonImage = null;
            this.textBox_scoreDistance.Enabled = false;
            this.textBox_scoreDistance.Location = new System.Drawing.Point(340, 52);
            this.textBox_scoreDistance.MaxLength = 32767;
            this.textBox_scoreDistance.Multiline = false;
            this.textBox_scoreDistance.Name = "textBox_scoreDistance";
            this.textBox_scoreDistance.ReadOnly = true;
            this.textBox_scoreDistance.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_scoreDistance.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_scoreDistance.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_scoreDistance.Size = new System.Drawing.Size(98, 19);
            this.textBox_scoreDistance.TabIndex = 43;
            this.textBox_scoreDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_scoreDistance
            // 
            this.label_scoreDistance.AutoSize = true;
            this.label_scoreDistance.Location = new System.Drawing.Point(259, 55);
            this.label_scoreDistance.Name = "label_scoreDistance";
            this.label_scoreDistance.Size = new System.Drawing.Size(53, 13);
            this.label_scoreDistance.TabIndex = 42;
            this.label_scoreDistance.Text = "Score/mi:";
            // 
            // textBox_courseScore
            // 
            this.textBox_courseScore.AcceptsReturn = false;
            this.textBox_courseScore.AcceptsTab = false;
            this.textBox_courseScore.BackColor = System.Drawing.Color.White;
            this.textBox_courseScore.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_courseScore.ButtonImage = null;
            this.textBox_courseScore.Enabled = false;
            this.textBox_courseScore.Location = new System.Drawing.Point(137, 52);
            this.textBox_courseScore.MaxLength = 32767;
            this.textBox_courseScore.Multiline = false;
            this.textBox_courseScore.Name = "textBox_courseScore";
            this.textBox_courseScore.ReadOnly = true;
            this.textBox_courseScore.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_courseScore.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_courseScore.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_courseScore.Size = new System.Drawing.Size(95, 19);
            this.textBox_courseScore.TabIndex = 41;
            this.textBox_courseScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_courseScore.DoubleClick += new System.EventHandler(this.score_doubleclick);
            // 
            // label_courseScore
            // 
            this.label_courseScore.AutoSize = true;
            this.label_courseScore.Location = new System.Drawing.Point(3, 55);
            this.label_courseScore.Name = "label_courseScore";
            this.label_courseScore.Size = new System.Drawing.Size(117, 13);
            this.label_courseScore.TabIndex = 40;
            this.label_courseScore.Text = "Course Score (Cycling):";
            // 
            // textBox_climb
            // 
            this.textBox_climb.AcceptsReturn = false;
            this.textBox_climb.AcceptsTab = false;
            this.textBox_climb.BackColor = System.Drawing.Color.White;
            this.textBox_climb.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_climb.ButtonImage = null;
            this.textBox_climb.Enabled = false;
            this.textBox_climb.Location = new System.Drawing.Point(340, 30);
            this.textBox_climb.MaxLength = 32767;
            this.textBox_climb.Multiline = false;
            this.textBox_climb.Name = "textBox_climb";
            this.textBox_climb.ReadOnly = true;
            this.textBox_climb.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_climb.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_climb.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_climb.Size = new System.Drawing.Size(98, 19);
            this.textBox_climb.TabIndex = 39;
            this.textBox_climb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_climb
            // 
            this.label_climb.AutoSize = true;
            this.label_climb.Location = new System.Drawing.Point(259, 34);
            this.label_climb.Name = "label_climb";
            this.label_climb.Size = new System.Drawing.Size(35, 13);
            this.label_climb.TabIndex = 38;
            this.label_climb.Text = "Climb:";
            // 
            // textBox_distance
            // 
            this.textBox_distance.AcceptsReturn = false;
            this.textBox_distance.AcceptsTab = false;
            this.textBox_distance.BackColor = System.Drawing.Color.White;
            this.textBox_distance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.textBox_distance.ButtonImage = null;
            this.textBox_distance.Enabled = false;
            this.textBox_distance.Location = new System.Drawing.Point(137, 30);
            this.textBox_distance.MaxLength = 32767;
            this.textBox_distance.Multiline = false;
            this.textBox_distance.Name = "textBox_distance";
            this.textBox_distance.ReadOnly = true;
            this.textBox_distance.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.textBox_distance.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.textBox_distance.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox_distance.Size = new System.Drawing.Size(95, 19);
            this.textBox_distance.TabIndex = 37;
            this.textBox_distance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label_distance
            // 
            this.label_distance.AutoSize = true;
            this.label_distance.Location = new System.Drawing.Point(3, 34);
            this.label_distance.Name = "label_distance";
            this.label_distance.Size = new System.Drawing.Size(52, 13);
            this.label_distance.TabIndex = 35;
            this.label_distance.Text = "Distance:";
            // 
            // infoBanner
            // 
            this.infoBanner.BackColor = System.Drawing.Color.Transparent;
            this.infoBanner.ContextMenuStrip = this.infoMenu;
            this.infoBanner.Controls.Add(this.TreeRefreshButton);
            this.infoBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoBanner.HasMenuButton = true;
            this.infoBanner.Location = new System.Drawing.Point(0, 0);
            this.infoBanner.Name = "infoBanner";
            this.infoBanner.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.infoBanner.Size = new System.Drawing.Size(515, 24);
            this.infoBanner.Style = ZoneFiveSoftware.Common.Visuals.ActionBanner.BannerStyle.Header2;
            this.infoBanner.TabIndex = 1;
            this.infoBanner.UseStyleFont = true;
            this.infoBanner.MenuClicked += new System.EventHandler(this.InfoBanner_MenuClicked);
            // 
            // infoMenu
            // 
            this.infoMenu.Name = "infoMenu";
            this.infoMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // TreeRefreshButton
            // 
            this.TreeRefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeRefreshButton.BackColor = System.Drawing.Color.Transparent;
            this.TreeRefreshButton.BackgroundImage = global::CourseScore.Resources.Images.ZoomIn;
            this.TreeRefreshButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.TreeRefreshButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.TreeRefreshButton.CenterImage = null;
            this.TreeRefreshButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.TreeRefreshButton.HyperlinkStyle = false;
            this.TreeRefreshButton.ImageMargin = 2;
            this.TreeRefreshButton.LeftImage = null;
            this.TreeRefreshButton.Location = new System.Drawing.Point(2670, 0);
            this.TreeRefreshButton.Margin = new System.Windows.Forms.Padding(0);
            this.TreeRefreshButton.Name = "TreeRefreshButton";
            this.TreeRefreshButton.PushStyle = true;
            this.TreeRefreshButton.RightImage = null;
            this.TreeRefreshButton.Size = new System.Drawing.Size(24, 24);
            this.TreeRefreshButton.TabIndex = 1;
            this.TreeRefreshButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.TreeRefreshButton.TextLeftMargin = 2;
            this.TreeRefreshButton.TextRightMargin = 2;
            this.TreeRefreshButton.Click += new System.EventHandler(this.TreeRefreshButton_Click);
            // 
            // treeList1
            // 
            this.treeList1.BackColor = System.Drawing.Color.Transparent;
            this.treeList1.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.SmallRoundShadow;
            this.treeList1.CheckBoxes = false;
            this.treeList1.DefaultIndent = 15;
            this.treeList1.DefaultRowHeight = -1;
            this.treeList1.HeaderRowHeight = 21;
            this.treeList1.Location = new System.Drawing.Point(386, 200);
            this.treeList1.MultiSelect = true;
            this.treeList1.Name = "treeList1";
            this.treeList1.NumHeaderRows = ZoneFiveSoftware.Common.Visuals.TreeList.HeaderRows.Auto;
            this.treeList1.NumLockedColumns = 0;
            this.treeList1.RowAlternatingColors = true;
            this.treeList1.RowHotlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(147)))), ((int)(((byte)(160)))), ((int)(((byte)(112)))));
            this.treeList1.RowHotlightColorText = System.Drawing.SystemColors.HighlightText;
            this.treeList1.RowHotlightMouse = true;
            this.treeList1.RowSelectedColor = System.Drawing.SystemColors.Highlight;
            this.treeList1.RowSelectedColorText = System.Drawing.SystemColors.HighlightText;
            this.treeList1.RowSeparatorLines = true;
            this.treeList1.ShowLines = false;
            this.treeList1.ShowPlusMinus = false;
            this.treeList1.Size = new System.Drawing.Size(85, 35);
            this.treeList1.TabIndex = 0;
            this.treeList1.SelectedItemsChanged += new System.EventHandler(this.treeList_SelectedChanged);
            this.treeList1.ColumnResized += new ZoneFiveSoftware.Common.Visuals.TreeList.ColumnEventHandler(this.treeList_ColumnResized);
            this.treeList1.ColumnClicked += new ZoneFiveSoftware.Common.Visuals.TreeList.ColumnEventHandler(this.treeList_ColumnClicked);
            this.treeList1.Click += new System.EventHandler(this.treeList_click);
            this.treeList1.DoubleClick += new System.EventHandler(this.treeList_DoubleClick);
            // 
            // treeList_subtotals
            // 
            this.treeList_subtotals.BackColor = System.Drawing.Color.Transparent;
            this.treeList_subtotals.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.SmallRoundShadow;
            this.treeList_subtotals.CheckBoxes = false;
            this.treeList_subtotals.DefaultIndent = 15;
            this.treeList_subtotals.DefaultRowHeight = -1;
            this.treeList_subtotals.HeaderRowHeight = 0;
            this.treeList_subtotals.Location = new System.Drawing.Point(209, -17);
            this.treeList_subtotals.MultiSelect = false;
            this.treeList_subtotals.Name = "treeList_subtotals";
            this.treeList_subtotals.NumHeaderRows = ZoneFiveSoftware.Common.Visuals.TreeList.HeaderRows.Auto;
            this.treeList_subtotals.NumLockedColumns = 0;
            this.treeList_subtotals.RowAlternatingColors = true;
            this.treeList_subtotals.RowHotlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(147)))), ((int)(((byte)(160)))), ((int)(((byte)(112)))));
            this.treeList_subtotals.RowHotlightColorText = System.Drawing.SystemColors.HighlightText;
            this.treeList_subtotals.RowHotlightMouse = true;
            this.treeList_subtotals.RowSelectedColor = System.Drawing.SystemColors.Highlight;
            this.treeList_subtotals.RowSelectedColorText = System.Drawing.SystemColors.HighlightText;
            this.treeList_subtotals.RowSeparatorLines = true;
            this.treeList_subtotals.ShowLines = false;
            this.treeList_subtotals.ShowPlusMinus = false;
            this.treeList_subtotals.Size = new System.Drawing.Size(85, 35);
            this.treeList_subtotals.TabIndex = 3;
            this.treeList_subtotals.Visible = false;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.Transparent;
            this.panelMain.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.Round;
            this.panelMain.BorderColor = System.Drawing.Color.Gray;
            this.panelMain.Controls.Add(this.MainChart);
            this.panelMain.Controls.Add(this.ButtonPanel);
            this.panelMain.Controls.Add(this.ChartBanner);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.panelMain.HeadingFont = null;
            this.panelMain.HeadingLeftMargin = 0;
            this.panelMain.HeadingText = null;
            this.panelMain.HeadingTextColor = System.Drawing.Color.Black;
            this.panelMain.HeadingTopMargin = 0;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(515, 364);
            this.panelMain.TabIndex = 3;
            // 
            // MainChart
            // 
            this.MainChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainChart.BackColor = System.Drawing.Color.Transparent;
            this.MainChart.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.MainChart.Location = new System.Drawing.Point(1, 47);
            this.MainChart.Name = "MainChart";
            this.MainChart.Padding = new System.Windows.Forms.Padding(5);
            this.MainChart.Size = new System.Drawing.Size(513, 314);
            this.MainChart.TabIndex = 6;
            this.MainChart.SelectData += new ZoneFiveSoftware.Common.Visuals.Chart.ChartBase.SelectDataHandler(this.MainChart_SelectData);
            this.MainChart.Click += new System.EventHandler(this.MainChart_click);
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonPanel.BackColor = System.Drawing.Color.Transparent;
            this.ButtonPanel.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.Square;
            this.ButtonPanel.BorderColor = System.Drawing.Color.Gray;
            this.ButtonPanel.Controls.Add(this.HillMarkersButton);
            this.ButtonPanel.Controls.Add(this.button_color);
            this.ButtonPanel.Controls.Add(this.button1);
            this.ButtonPanel.Controls.Add(this.ZoomInButton);
            this.ButtonPanel.Controls.Add(this.ZoomOutButton);
            this.ButtonPanel.Controls.Add(this.ZoomChartButton);
            this.ButtonPanel.Controls.Add(this.ExtraChartsButton);
            this.ButtonPanel.Controls.Add(this.SaveImageButton);
            this.ButtonPanel.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.ButtonPanel.HeadingFont = null;
            this.ButtonPanel.HeadingLeftMargin = 0;
            this.ButtonPanel.HeadingText = null;
            this.ButtonPanel.HeadingTextColor = System.Drawing.Color.Black;
            this.ButtonPanel.HeadingTopMargin = 0;
            this.ButtonPanel.Location = new System.Drawing.Point(0, 23);
            this.ButtonPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(515, 24);
            this.ButtonPanel.TabIndex = 1;
            // 
            // HillMarkersButton
            // 
            this.HillMarkersButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HillMarkersButton.BackColor = System.Drawing.Color.Transparent;
            this.HillMarkersButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("HillMarkersButton.BackgroundImage")));
            this.HillMarkersButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.HillMarkersButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.HillMarkersButton.CenterImage = null;
            this.HillMarkersButton.ContextMenuStrip = this.colorMenu;
            this.HillMarkersButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.HillMarkersButton.HyperlinkStyle = false;
            this.HillMarkersButton.ImageMargin = 2;
            this.HillMarkersButton.LeftImage = null;
            this.HillMarkersButton.Location = new System.Drawing.Point(321, 0);
            this.HillMarkersButton.Margin = new System.Windows.Forms.Padding(0);
            this.HillMarkersButton.Name = "HillMarkersButton";
            this.HillMarkersButton.PushStyle = true;
            this.HillMarkersButton.RightImage = null;
            this.HillMarkersButton.Size = new System.Drawing.Size(24, 24);
            this.HillMarkersButton.TabIndex = 52;
            this.HillMarkersButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.HillMarkersButton.TextLeftMargin = 2;
            this.HillMarkersButton.TextRightMargin = 2;
            this.HillMarkersButton.Click += new System.EventHandler(this.HillMarkersButton_Click);
            // 
            // colorMenu
            // 
            this.colorMenu.Name = "colorMenu";
            this.colorMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // button_color
            // 
            this.button_color.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_color.BackColor = System.Drawing.Color.Transparent;
            this.button_color.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_color.BackgroundImage")));
            this.button_color.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_color.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.button_color.CenterImage = null;
            this.button_color.ContextMenuStrip = this.colorMenu;
            this.button_color.DialogResult = System.Windows.Forms.DialogResult.None;
            this.button_color.HyperlinkStyle = false;
            this.button_color.ImageMargin = 2;
            this.button_color.LeftImage = null;
            this.button_color.Location = new System.Drawing.Point(345, 0);
            this.button_color.Margin = new System.Windows.Forms.Padding(0);
            this.button_color.Name = "button_color";
            this.button_color.PushStyle = true;
            this.button_color.RightImage = null;
            this.button_color.Size = new System.Drawing.Size(24, 24);
            this.button_color.TabIndex = 2;
            this.button_color.TextAlign = System.Drawing.StringAlignment.Center;
            this.button_color.TextLeftMargin = 2;
            this.button_color.TextRightMargin = 2;
            this.button_color.Click += new System.EventHandler(this.Color_click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.button1.CenterImage = null;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.button1.HyperlinkStyle = false;
            this.button1.ImageMargin = 2;
            this.button1.LeftImage = null;
            this.button1.Location = new System.Drawing.Point(369, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.PushStyle = true;
            this.button1.RightImage = null;
            this.button1.Size = new System.Drawing.Size(24, 24);
            this.button1.TabIndex = 1;
            this.button1.TextAlign = System.Drawing.StringAlignment.Center;
            this.button1.TextLeftMargin = 2;
            this.button1.TextRightMargin = 2;
            this.button1.Click += new System.EventHandler(this.Settings_click);
            // 
            // ZoomInButton
            // 
            this.ZoomInButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomInButton.BackColor = System.Drawing.Color.Transparent;
            this.ZoomInButton.BackgroundImage = global::CourseScore.Resources.Images.ZoomIn;
            this.ZoomInButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ZoomInButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.ZoomInButton.CenterImage = null;
            this.ZoomInButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ZoomInButton.HyperlinkStyle = false;
            this.ZoomInButton.ImageMargin = 2;
            this.ZoomInButton.LeftImage = null;
            this.ZoomInButton.Location = new System.Drawing.Point(489, 0);
            this.ZoomInButton.Margin = new System.Windows.Forms.Padding(0);
            this.ZoomInButton.Name = "ZoomInButton";
            this.ZoomInButton.PushStyle = true;
            this.ZoomInButton.RightImage = null;
            this.ZoomInButton.Size = new System.Drawing.Size(24, 24);
            this.ZoomInButton.TabIndex = 0;
            this.ZoomInButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.ZoomInButton.TextLeftMargin = 2;
            this.ZoomInButton.TextRightMargin = 2;
            this.ZoomInButton.Click += new System.EventHandler(this.zoomIn_Click);
            // 
            // ZoomOutButton
            // 
            this.ZoomOutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomOutButton.BackColor = System.Drawing.Color.Transparent;
            this.ZoomOutButton.BackgroundImage = global::CourseScore.Resources.Images.ZoomOut;
            this.ZoomOutButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ZoomOutButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.ZoomOutButton.CenterImage = null;
            this.ZoomOutButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ZoomOutButton.HyperlinkStyle = false;
            this.ZoomOutButton.ImageMargin = 2;
            this.ZoomOutButton.LeftImage = null;
            this.ZoomOutButton.Location = new System.Drawing.Point(465, 0);
            this.ZoomOutButton.Margin = new System.Windows.Forms.Padding(0);
            this.ZoomOutButton.Name = "ZoomOutButton";
            this.ZoomOutButton.PushStyle = true;
            this.ZoomOutButton.RightImage = null;
            this.ZoomOutButton.Size = new System.Drawing.Size(24, 24);
            this.ZoomOutButton.TabIndex = 0;
            this.ZoomOutButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.ZoomOutButton.TextLeftMargin = 2;
            this.ZoomOutButton.TextRightMargin = 2;
            this.ZoomOutButton.Click += new System.EventHandler(this.zoomOut_Click);
            // 
            // ZoomChartButton
            // 
            this.ZoomChartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomChartButton.BackColor = System.Drawing.Color.Transparent;
            this.ZoomChartButton.BackgroundImage = global::CourseScore.Resources.Images.ZoomFit;
            this.ZoomChartButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ZoomChartButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.ZoomChartButton.CenterImage = null;
            this.ZoomChartButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ZoomChartButton.HyperlinkStyle = false;
            this.ZoomChartButton.ImageMargin = 2;
            this.ZoomChartButton.LeftImage = null;
            this.ZoomChartButton.Location = new System.Drawing.Point(441, 0);
            this.ZoomChartButton.Margin = new System.Windows.Forms.Padding(0);
            this.ZoomChartButton.Name = "ZoomChartButton";
            this.ZoomChartButton.PushStyle = true;
            this.ZoomChartButton.RightImage = null;
            this.ZoomChartButton.Size = new System.Drawing.Size(24, 24);
            this.ZoomChartButton.TabIndex = 0;
            this.ZoomChartButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.ZoomChartButton.TextLeftMargin = 2;
            this.ZoomChartButton.TextRightMargin = 2;
            this.ZoomChartButton.Click += new System.EventHandler(this.ZoomFitButton_Click);
            // 
            // ExtraChartsButton
            // 
            this.ExtraChartsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExtraChartsButton.BackColor = System.Drawing.Color.Transparent;
            this.ExtraChartsButton.BackgroundImage = global::CourseScore.Resources.Images.Charts;
            this.ExtraChartsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ExtraChartsButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.ExtraChartsButton.CenterImage = null;
            this.ExtraChartsButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ExtraChartsButton.HyperlinkStyle = false;
            this.ExtraChartsButton.ImageMargin = 2;
            this.ExtraChartsButton.LeftImage = null;
            this.ExtraChartsButton.Location = new System.Drawing.Point(393, 0);
            this.ExtraChartsButton.Margin = new System.Windows.Forms.Padding(0);
            this.ExtraChartsButton.Name = "ExtraChartsButton";
            this.ExtraChartsButton.PushStyle = true;
            this.ExtraChartsButton.RightImage = null;
            this.ExtraChartsButton.Size = new System.Drawing.Size(24, 24);
            this.ExtraChartsButton.TabIndex = 0;
            this.ExtraChartsButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.ExtraChartsButton.TextLeftMargin = 2;
            this.ExtraChartsButton.TextRightMargin = 2;
            this.ExtraChartsButton.Click += new System.EventHandler(this.ExtraCharts_click);
            // 
            // SaveImageButton
            // 
            this.SaveImageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveImageButton.BackColor = System.Drawing.Color.Transparent;
            this.SaveImageButton.BackgroundImage = global::CourseScore.Resources.Images.Save;
            this.SaveImageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SaveImageButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.SaveImageButton.CenterImage = null;
            this.SaveImageButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.SaveImageButton.HyperlinkStyle = false;
            this.SaveImageButton.ImageMargin = 2;
            this.SaveImageButton.LeftImage = null;
            this.SaveImageButton.Location = new System.Drawing.Point(417, 0);
            this.SaveImageButton.Margin = new System.Windows.Forms.Padding(0);
            this.SaveImageButton.Name = "SaveImageButton";
            this.SaveImageButton.PushStyle = true;
            this.SaveImageButton.RightImage = null;
            this.SaveImageButton.Size = new System.Drawing.Size(24, 24);
            this.SaveImageButton.TabIndex = 0;
            this.SaveImageButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.SaveImageButton.TextLeftMargin = 2;
            this.SaveImageButton.TextRightMargin = 2;
            this.SaveImageButton.Click += new System.EventHandler(this.SaveImageButton_Click);
            // 
            // ChartBanner
            // 
            this.ChartBanner.BackColor = System.Drawing.Color.Transparent;
            this.ChartBanner.ContextMenuStrip = this.detailMenu;
            this.ChartBanner.Controls.Add(this.MaximizeButton);
            this.ChartBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChartBanner.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ChartBanner.HasMenuButton = true;
            this.ChartBanner.Location = new System.Drawing.Point(0, 0);
            this.ChartBanner.Name = "ChartBanner";
            this.ChartBanner.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ChartBanner.Size = new System.Drawing.Size(515, 24);
            this.ChartBanner.Style = ZoneFiveSoftware.Common.Visuals.ActionBanner.BannerStyle.Header2;
            this.ChartBanner.TabIndex = 5;
            this.ChartBanner.Text = "Detail Pane Chart";
            this.ChartBanner.UseStyleFont = true;
            this.ChartBanner.MenuClicked += new System.EventHandler(this.ChartBanner_MenuClicked);
            // 
            // detailMenu
            // 
            this.detailMenu.Name = "detailMenu";
            this.detailMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // MaximizeButton
            // 
            this.MaximizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaximizeButton.BackColor = System.Drawing.Color.Transparent;
            this.MaximizeButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.MaximizeButton.CenterImage = ((System.Drawing.Image)(resources.GetObject("MaximizeButton.CenterImage")));
            this.MaximizeButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.MaximizeButton.HyperlinkStyle = false;
            this.MaximizeButton.ImageMargin = 2;
            this.MaximizeButton.LeftImage = null;
            this.MaximizeButton.Location = new System.Drawing.Point(4434, 1);
            this.MaximizeButton.Name = "MaximizeButton";
            this.MaximizeButton.PushStyle = true;
            this.MaximizeButton.RightImage = null;
            this.MaximizeButton.Size = new System.Drawing.Size(24, 24);
            this.MaximizeButton.TabIndex = 1;
            this.MaximizeButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.MaximizeButton.TextLeftMargin = 2;
            this.MaximizeButton.TextRightMargin = 2;
            this.MaximizeButton.Click += new System.EventHandler(this.MaximizeButton_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // allActs
            // 
            this.allActs.Name = "allActs";
            this.allActs.Size = new System.Drawing.Size(32, 19);
            // 
            // treeMenu
            // 
            this.treeMenu.Name = "treeMenu";
            this.treeMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // splitsContextMenuStrip
            // 
            this.splitsContextMenuStrip.Name = "contextMenuStrip";
            this.splitsContextMenuStrip.Size = new System.Drawing.Size(181, 26);
            // 
            // HillsDetailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Name = "HillsDetailControl";
            this.Size = new System.Drawing.Size(515, 558);
            this.pnlMain.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel_climb.ResumeLayout(false);
            this.infoBanner.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.ButtonPanel.ResumeLayout(false);
            this.ChartBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ZoneFiveSoftware.Common.Visuals.Panel pnlMain;
        private ZoneFiveSoftware.Common.Visuals.TreeList treeList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem allActs;
        private ZoneFiveSoftware.Common.Visuals.Panel panelMain;
        private ZoneFiveSoftware.Common.Visuals.Chart.ChartBase MainChart;
        private ZoneFiveSoftware.Common.Visuals.Panel ButtonPanel;
        private ZoneFiveSoftware.Common.Visuals.Button ZoomInButton;
        private ZoneFiveSoftware.Common.Visuals.Button ZoomOutButton;
        private ZoneFiveSoftware.Common.Visuals.Button ZoomChartButton;
        private ZoneFiveSoftware.Common.Visuals.Button ExtraChartsButton;
        private ZoneFiveSoftware.Common.Visuals.Button SaveImageButton;
        private ZoneFiveSoftware.Common.Visuals.ActionBanner ChartBanner;
        private System.Windows.Forms.ContextMenuStrip detailMenu;
        private ZoneFiveSoftware.Common.Visuals.Button button1;
        private ZoneFiveSoftware.Common.Visuals.ActionBanner infoBanner;
        private ZoneFiveSoftware.Common.Visuals.Panel panel_climb;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_HC_distance;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_HC_score;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_HC_elevationGain;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_HC_grade;
        private System.Windows.Forms.Label label_hardestClimb;
        private System.Windows.Forms.Label label_longestClimb;
        private System.Windows.Forms.Label label_LC_distance;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_LC_distance;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_LC_score;
        private System.Windows.Forms.Label label_LC_elevationGain;
        private System.Windows.Forms.Label label_LC_score;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_LC_elevationGain;
        private System.Windows.Forms.Label label_LC_grade;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_LC_grade;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_scoreDistance;
        private System.Windows.Forms.Label label_scoreDistance;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_courseScore;
        private System.Windows.Forms.Label label_courseScore;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_climb;
        private System.Windows.Forms.Label label_climb;
        private ZoneFiveSoftware.Common.Visuals.TextBox textBox_distance;
        private System.Windows.Forms.Label label_distance;
        private System.Windows.Forms.ContextMenuStrip infoMenu;
        private Button button_courseScore;
        private System.Windows.Forms.ContextMenuStrip scoreMenu;
        private System.Windows.Forms.Label label_steepestClimb;
        private TextBox textBox_MID_distance;
        private TextBox textBox_MID_score;
        private TextBox textBox_MID_elevationGain;
        private TextBox textBox_MID_grade;
        private Button button_color;
        private System.Windows.Forms.ContextMenuStrip colorMenu;
        private Button MaximizeButton;
        private System.Windows.Forms.ContextMenuStrip treeMenu;
        private Button HillMarkersButton;
        private Button TreeRefreshButton;
        private System.Windows.Forms.ContextMenuStrip splitsContextMenuStrip;
        private TreeList treeList_subtotals;



    }
}
