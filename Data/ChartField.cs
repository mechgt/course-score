using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Visuals;
using System.Drawing;
using CourseScore.UI;

namespace CourseScore.Data
{
    /// <summary>
    /// ChartField is used for a field that you put in a list item select box
    /// </summary>
    public class ChartField : IListColumnDefinition
    {
        public const string cadenceID = "bf098136-ff1b-4ccd-89d4-fe2407437a6e";
        public const string elevationID = "382122b0-d9cb-4ec0-a0cf-b6042e1aa2ff";
        public const string gradeID = "ce7e8fb8-bd2f-4ba0-8c3c-6396b21b6e19";
        public const string hrID = "209f292b-044b-42f7-ad47-3c760d3d3be8";
        public const string powerID = "ecb3b972-bb88-4794-99c0-c2a34b54ef9d";
        public const string speedID = "00a34df6-fc29-4cf3-9b87-29c8c0dd931c";
        public const string vamID = "1463f429-445d-41b5-8e9b-c1900439d4de";

        public Field chartField;
        private string id;
        private int width;

        public ChartField(Field thisField)
        {
            chartField = thisField;

        }

        public enum Field
        {
            Power,
            HR,
            Cadence,
            Grade,
            Speed,
            Elevation,
            VAM
        }

        /// <summary>
        /// Lookup the text for this enum
        /// </summary>
        /// <param name="field">enum to lookup</param>
        /// <returns>Returns the localized text</returns>
        public static string ChartFieldsLookup(Field field)
        {
            switch (field)
            {
                case Field.Cadence:
                    return CommonResources.Text.LabelCadence;
                case Field.Grade:
                    return CommonResources.Text.LabelGrade;
                case Field.HR:
                    return CommonResources.Text.LabelHeartRate;
                case Field.Power:
                    return CommonResources.Text.LabelPower;
                case Field.Speed:
                    // TODO: What about pace?
                    return CommonResources.Text.LabelSpeed;
                case Field.Elevation:
                    return CommonResources.Text.LabelElevation;
                case Field.VAM:
                    return Resources.Strings.Label_VAM;
            }
            return "";
        }

        /// <summary>
        /// Lookup the color for this enum
        /// </summary>
        /// <param name="field">enum to lookup</param>
        /// <returns>Returns the appropriate chart color</returns>
        public static Color ChartColorLookup(Field field)
        {
            switch (field)
            {
                case Field.Cadence:
                    return Common.ColorCadence;
                case Field.Grade:
                    return Common.ColorGrade;
                case Field.HR:
                    return Common.ColorHR;
                case Field.Power:
                    return Common.ColorPower;
                case Field.Speed:
                    return Common.ColorSpeed;
                case Field.Elevation:
                    return Common.ColorElevation;
                case Field.VAM:
                    return Common.ColorVAM;
            }

            return Color.Black;
        }

        /// <summary>
        /// ToString override to localize the text
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ChartFieldsLookup(chartField);
        }

        #region IListColumnDefinition Members

        public StringAlignment Align
        {
            get { return StringAlignment.Near; }
        }

        public string GroupName
        {
            get { return null; }
        }

        public string Id
        {
            get { return id; }
        }

        public string Text(string columnId)
        {
            throw new NotImplementedException();
        }

        public int Width
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
