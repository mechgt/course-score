using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ZoneFiveSoftware.Common.Visuals;
using System.Diagnostics;

namespace CourseScore.Data
{
    /// <summary>
    /// FeatureListRenderer is used to override the DrawCell method of the TreeList's default row data renderer
    /// enabling us to custom color a cell
    /// </summary>
    class FeatureListRenderer : TreeList.DefaultRowDataRenderer
    {
        public FeatureListRenderer(TreeList tree)
            : base(tree)
        {

        }

        protected override void DrawCell(Graphics graphics, TreeList.DrawItemState rowState, object element, TreeList.Column column, Rectangle cellRect)
        {
            Feature feature = element as Feature;

            if (feature != null)
            {
                if (feature.selectedColor != Color.Empty)
                {
                    // If this is the ID column and the feature.selectedColor isn't empty, color the cell and return.  We don't need an id
                    if (column.Id == "HillId")
                    {
                        Brush selectedBrush = new SolidBrush(feature.selectedColor);
                        graphics.FillRectangle(selectedBrush, cellRect);
                        return;

                    }
                }
            }

            base.DrawCell(graphics, rowState, element, column, cellRect);
        }
    }
}
