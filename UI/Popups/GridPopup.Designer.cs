namespace CourseScore.UI.Popups
{
    partial class GridPopup
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
            this.treeList = new ZoneFiveSoftware.Common.Visuals.TreeList();
            this.SuspendLayout();
            // 
            // treeList
            // 
            this.treeList.BackColor = System.Drawing.Color.Transparent;
            this.treeList.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.SmallRoundShadow;
            this.treeList.CheckBoxes = false;
            this.treeList.DefaultIndent = 15;
            this.treeList.DefaultRowHeight = -1;
            this.treeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList.HeaderRowHeight = 21;
            this.treeList.Location = new System.Drawing.Point(0, 0);
            this.treeList.MultiSelect = true;
            this.treeList.Name = "treeList";
            this.treeList.NumHeaderRows = ZoneFiveSoftware.Common.Visuals.TreeList.HeaderRows.Auto;
            this.treeList.NumLockedColumns = 0;
            this.treeList.RowAlternatingColors = true;
            this.treeList.RowHotlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(147)))), ((int)(((byte)(160)))), ((int)(((byte)(112)))));
            this.treeList.RowHotlightColorText = System.Drawing.SystemColors.HighlightText;
            this.treeList.RowHotlightMouse = true;
            this.treeList.RowSelectedColor = System.Drawing.SystemColors.Highlight;
            this.treeList.RowSelectedColorText = System.Drawing.SystemColors.HighlightText;
            this.treeList.RowSeparatorLines = true;
            this.treeList.ShowLines = false;
            this.treeList.ShowPlusMinus = false;
            this.treeList.Size = new System.Drawing.Size(344, 528);
            this.treeList.TabIndex = 1;
            // 
            // GridPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 528);
            this.Controls.Add(this.treeList);
            this.Name = "GridPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cycling Score Breakdown";
            this.ResumeLayout(false);

        }

        #endregion

        public ZoneFiveSoftware.Common.Visuals.TreeList treeList;
    }
}