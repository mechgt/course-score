namespace CourseScore.Settings
{
    partial class SettingsPageControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox_sport = new System.Windows.Forms.ComboBox();
            this.label_sport = new System.Windows.Forms.Label();
            this.textBox_offset = new System.Windows.Forms.TextBox();
            this.label_offset = new System.Windows.Forms.Label();
            this.textBox_factor = new System.Windows.Forms.TextBox();
            this.label_factor = new System.Windows.Forms.Label();
            this.comboBox_hillScore = new System.Windows.Forms.ComboBox();
            this.label_hillScore = new System.Windows.Forms.Label();
            this.scoreChart = new ZoneFiveSoftware.Common.Visuals.Chart.ChartBase();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_sport);
            this.groupBox1.Controls.Add(this.label_sport);
            this.groupBox1.Controls.Add(this.textBox_offset);
            this.groupBox1.Controls.Add(this.label_offset);
            this.groupBox1.Controls.Add(this.textBox_factor);
            this.groupBox1.Controls.Add(this.label_factor);
            this.groupBox1.Controls.Add(this.comboBox_hillScore);
            this.groupBox1.Controls.Add(this.label_hillScore);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(397, 133);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scoring";
            // 
            // comboBox_sport
            // 
            this.comboBox_sport.FormattingEnabled = true;
            this.comboBox_sport.Location = new System.Drawing.Point(244, 45);
            this.comboBox_sport.Name = "comboBox_sport";
            this.comboBox_sport.Size = new System.Drawing.Size(121, 21);
            this.comboBox_sport.TabIndex = 7;
            this.comboBox_sport.SelectedValueChanged += new System.EventHandler(this.sport_SelectedValueChanged);
            // 
            // label_sport
            // 
            this.label_sport.AutoSize = true;
            this.label_sport.Location = new System.Drawing.Point(6, 48);
            this.label_sport.Name = "label_sport";
            this.label_sport.Size = new System.Drawing.Size(32, 13);
            this.label_sport.TabIndex = 6;
            this.label_sport.Text = "Sport";
            // 
            // textBox_offset
            // 
            this.textBox_offset.Location = new System.Drawing.Point(244, 98);
            this.textBox_offset.Name = "textBox_offset";
            this.textBox_offset.Size = new System.Drawing.Size(100, 20);
            this.textBox_offset.TabIndex = 5;
            this.textBox_offset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.digitValidator);
            this.textBox_offset.Leave += new System.EventHandler(this.CyclingOffset_leave);
            // 
            // label_offset
            // 
            this.label_offset.AutoSize = true;
            this.label_offset.Location = new System.Drawing.Point(6, 101);
            this.label_offset.Name = "label_offset";
            this.label_offset.Size = new System.Drawing.Size(142, 13);
            this.label_offset.TabIndex = 4;
            this.label_offset.Text = "Cycling Course Score Offset:";
            // 
            // textBox_factor
            // 
            this.textBox_factor.Location = new System.Drawing.Point(244, 72);
            this.textBox_factor.Name = "textBox_factor";
            this.textBox_factor.Size = new System.Drawing.Size(100, 20);
            this.textBox_factor.TabIndex = 3;
            this.textBox_factor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.digitValidator);
            this.textBox_factor.Leave += new System.EventHandler(this.CyclingFactor_leave);
            // 
            // label_factor
            // 
            this.label_factor.AutoSize = true;
            this.label_factor.Location = new System.Drawing.Point(6, 75);
            this.label_factor.Name = "label_factor";
            this.label_factor.Size = new System.Drawing.Size(144, 13);
            this.label_factor.TabIndex = 2;
            this.label_factor.Text = "Cycling Course Score Factor:";
            // 
            // comboBox_hillScore
            // 
            this.comboBox_hillScore.FormattingEnabled = true;
            this.comboBox_hillScore.Location = new System.Drawing.Point(244, 18);
            this.comboBox_hillScore.Name = "comboBox_hillScore";
            this.comboBox_hillScore.Size = new System.Drawing.Size(121, 21);
            this.comboBox_hillScore.TabIndex = 1;
            this.comboBox_hillScore.SelectedValueChanged += new System.EventHandler(this.hillScore_selectedValueChanged);
            // 
            // label_hillScore
            // 
            this.label_hillScore.AutoSize = true;
            this.label_hillScore.Location = new System.Drawing.Point(6, 21);
            this.label_hillScore.Name = "label_hillScore";
            this.label_hillScore.Size = new System.Drawing.Size(100, 13);
            this.label_hillScore.TabIndex = 0;
            this.label_hillScore.Text = "Hill Score Equation:";
            // 
            // scoreChart
            // 
            this.scoreChart.BackColor = System.Drawing.Color.Transparent;
            this.scoreChart.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.SmallRoundShadow;
            this.scoreChart.Location = new System.Drawing.Point(0, 197);
            this.scoreChart.Name = "scoreChart";
            this.scoreChart.Padding = new System.Windows.Forms.Padding(5);
            this.scoreChart.Size = new System.Drawing.Size(557, 232);
            this.scoreChart.TabIndex = 1;
            // 
            // SettingsPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scoreChart);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingsPageControl";
            this.Size = new System.Drawing.Size(560, 432);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox_hillScore;
        private System.Windows.Forms.Label label_hillScore;
        private ZoneFiveSoftware.Common.Visuals.Chart.ChartBase scoreChart;
        private System.Windows.Forms.TextBox textBox_offset;
        private System.Windows.Forms.Label label_offset;
        private System.Windows.Forms.TextBox textBox_factor;
        private System.Windows.Forms.Label label_factor;
        private System.Windows.Forms.Label label_sport;
        private System.Windows.Forms.ComboBox comboBox_sport;
    }
}
