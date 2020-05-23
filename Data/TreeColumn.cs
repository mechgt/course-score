using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Visuals;
using System.Drawing;
using CourseScore.UI;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Data.Fitness;
using System.Xml.Serialization;

namespace CourseScore.Data
{
    public class TreeColumn : IListColumnDefinition
    {
        public Column treeColumn;
        private int width;
        private string id;

        public enum Column
        {
            HillId,
            Date,
            Start, 
            End,
            Distance,
            Time,
            ElevGain,
            AvgGrade,
            AvgSpeed,
            AvgPace ,
            HR,
            Cadence ,
            Power,
            HillScoreClimbByBike,
            HillScoreCycle2Max,
            HillScoreFiets,
            HillScoreCourseScoreCycling ,
            HillScoreCourseScoreRunning ,
            VAM,
            WKg,
            StoppedTime,
            MaxGrade,
            MinGrade
            //HillCategory
            //AscentDescent
        }

        public TreeColumn()
        {
            // Parameterless constructor for de-serialization
        }

        public TreeColumn(Column thisField, int setWidth)
        {
            treeColumn = thisField;
            width = setWidth;
            id = IdColumnLookup(thisField);
        }

        public static TreeColumnCollection DefaultColumns()
        {
            //IList<Column> columns = new List<Column>();
            TreeColumnCollection columns = new TreeColumnCollection();
            columns.Add(new TreeColumn(Column.HillId, 40));
            columns.Add(new TreeColumn(Column.Date, 120));
            columns.Add(new TreeColumn(Column.Start, 80));
            columns.Add(new TreeColumn(Column.End, 80));
            columns.Add(new TreeColumn(Column.Time, 80));
            columns.Add(new TreeColumn(Column.ElevGain, 80));
            columns.Add(new TreeColumn(Column.AvgGrade, 60));
            columns.Add(new TreeColumn(Column.HillScoreCourseScoreCycling, 60));
            return columns;
        }

        public static ICollection<IListColumnDefinition> ColumnDefs(IActivity activity)
        {
            IList<IListColumnDefinition> columnDefs = new List<IListColumnDefinition>();
            columnDefs.Add(new TreeColumn(Column.HillId, 40));
            columnDefs.Add(new TreeColumn(Column.Date, 120));
            columnDefs.Add(new TreeColumn(Column.Start, 80));
            columnDefs.Add(new TreeColumn(Column.End, 80));
            columnDefs.Add(new TreeColumn(Column.Distance, 80));
            columnDefs.Add(new TreeColumn(Column.Time, 80));
            columnDefs.Add(new TreeColumn(Column.ElevGain, 80));
            columnDefs.Add(new TreeColumn(Column.AvgGrade, 60));
            if (activity.Category.SpeedUnits == Speed.Units.Speed)
            {
                columnDefs.Add(new TreeColumn(Column.AvgSpeed, 80));
            }
            else if (activity.Category.SpeedUnits == Speed.Units.Pace)
            {
                columnDefs.Add(new TreeColumn(Column.AvgPace, 80));
            }
            columnDefs.Add(new TreeColumn(Column.HR, 60));
            columnDefs.Add(new TreeColumn(Column.Cadence, 60));
            columnDefs.Add(new TreeColumn(Column.Power, 60));
            columnDefs.Add(new TreeColumn(Column.HillScoreClimbByBike, 60));
            columnDefs.Add(new TreeColumn(Column.HillScoreCycle2Max, 60));
            columnDefs.Add(new TreeColumn(Column.HillScoreFiets, 60));
            columnDefs.Add(new TreeColumn(Column.HillScoreCourseScoreCycling, 60));
            columnDefs.Add(new TreeColumn(Column.HillScoreCourseScoreRunning, 60));
            columnDefs.Add(new TreeColumn(Column.VAM, 60));
            columnDefs.Add(new TreeColumn(Column.WKg, 60));
            columnDefs.Add(new TreeColumn(Column.StoppedTime, 80));
            columnDefs.Add(new TreeColumn(Column.MaxGrade, 60));
            columnDefs.Add(new TreeColumn(Column.MinGrade, 60));
            //columnDefs.Add(new TreeColumn(Column.HillCategory, 60));
            //columnDefs.Add(new TreeColumn(Column.AscentDescent, 120));
            return columnDefs;
        }

        /// <summary>
        /// Lookup the text for this enum
        /// </summary>
        /// <param name="field">enum to lookup</param>
        /// <returns>Returns the localized text</returns>
        public static string TextColumnLookup(Column field)
        {
            string distUnits = Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.DistanceUnits);
            string elevUnits = Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.ElevationUnits);

            switch (field)
            {
                case Column.HillId:
                    return Resources.Strings.Label_HillId;
                case Column.Date:
                    return CommonResources.Text.LabelStartTime;
                case Column.Start:
                    return CommonResources.Text.LabelStart + " (" + distUnits + ")";
                case Column.End:
                    return Resources.Strings.Label_End + " (" + distUnits + ")";
                case Column.Distance:
                    return CommonResources.Text.LabelTotalDistance + " (" + distUnits + ")";
                case Column.Time:
                    return CommonResources.Text.LabelTimeElapsed;
                case Column.ElevGain:
                    return CommonResources.Text.LabelElevationChange + " (" + elevUnits + ")";
                case Column.AvgGrade:
                    return CommonResources.Text.LabelAvgGrade;
                case Column.AvgSpeed:
                    return CommonResources.Text.LabelAvgSpeed;
                case Column.AvgPace:
                    return CommonResources.Text.LabelPace;
                case Column.HR:
                    return CommonResources.Text.LabelAvgHR;
                case Column.Cadence:
                    return CommonResources.Text.LabelAvgCadence;
                case Column.Power:
                    return CommonResources.Text.LabelAvgPower;
                case Column.HillScoreClimbByBike:
                    return ScoreEquation.ScoreEquationLookup(ScoreEquation.Score.ClimbByBike);
                case Column.HillScoreCycle2Max:
                    return ScoreEquation.ScoreEquationLookup(ScoreEquation.Score.Cycle2Max);
                case Column.HillScoreFiets:
                    return ScoreEquation.ScoreEquationLookup(ScoreEquation.Score.Fiets);
                case Column.HillScoreCourseScoreCycling:
                    return ScoreEquation.ScoreEquationLookup(ScoreEquation.Score.CourseScoreCycling);
                case Column.HillScoreCourseScoreRunning:
                    return ScoreEquation.ScoreEquationLookup(ScoreEquation.Score.CourseScoreRunning);
                case Column.VAM:
                    return Resources.Strings.Label_VAM;
                case Column.WKg:
                    return Resources.Strings.Label_wKg;
                case Column.StoppedTime:
                    return CommonResources.Text.LabelStoppedTime;
                case Column.MaxGrade:
                    return CommonResources.Text.LabelMaxGrade;
                case Column.MinGrade:
                    return Resources.Strings.Label_MinGrade;
                //case Column.HillCategory:
                //    return Resources.Strings.Label_HillClassification;
                //case Column.AscentDescent:
                //    return CommonResources.Text.LabelClimb;
            }
            return "";
        }

        /// <summary>
        /// ToString override to localize the text
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return TextColumnLookup(treeColumn);
        }

        /// <summary>
        /// Lookup the id for this enum
        /// </summary>
        /// <param name="field">enum to lookup</param>
        /// <returns>Returns the column id</returns>
        public static string IdColumnLookup(Column field)
        {
            switch (field)
            {
                case Column.HillId:
                    return "HillId";
                case Column.Date:
                    return "Date";
                case Column.Start:
                    return "Start";
                case Column.End:
                    return "End";
                case Column.Distance:
                    return "Distance";
                case Column.Time:
                    return "Time";
                case Column.ElevGain:
                    return "ElevGain";
                case Column.AvgGrade:
                    return "AvgGrade";
                case Column.AvgSpeed:
                    return "AvgSpeed";
                case Column.AvgPace:
                    return "AvgPace";
                case Column.HR:
                    return "HR";
                case Column.Cadence:
                    return "Cadence";
                case Column.Power:
                    return "Power";
                case Column.HillScoreClimbByBike:
                    return "HillScoreClimbByBike";
                case Column.HillScoreCycle2Max:
                    return "HillScoreCycle2Max";
                case Column.HillScoreFiets:
                    return "HillScoreFiets";
                case Column.HillScoreCourseScoreCycling:
                    return "HillScoreCourseScoreCycling";
                case Column.HillScoreCourseScoreRunning:
                    return "HillScoreCourseScoreRunning";
                case Column.VAM:
                    return "VAM";
                case Column.WKg:
                    return "WKg";
                case Column.StoppedTime:
                    return "StoppedTime";
                case Column.MaxGrade:
                    return "MaxGrade";
                case Column.MinGrade:
                    return "MinGrade";
                //case Column.HillCategory:
                //    return "HillCategory";
                //case Column.AscentDescent:
                //    return "AscentDescent";
            }
            return "";
        }

        public static TreeColumn.Column ReverseIdColumnLookup(string field)
        {
            switch (field)
            {
                case "HillId":
                    return Column.HillId;
                case "Date":
                    return Column.Date;
                case "Start":
                    return Column.Start;
                case "End":
                    return Column.End;
                case "Distance":
                    return Column.Distance;
                case "Time":
                    return Column.Time;
                case "ElevGain":
                    return Column.ElevGain;
                case "AvgGrade":
                    return Column.AvgGrade;
                case "AvgSpeed":
                    return Column.AvgSpeed;
                case "AvgPace":
                    return Column.AvgPace;
                case "HR":
                    return Column.HR;
                case "Cadence":
                    return Column.Cadence;
                case "Power":
                    return Column.Power;
                case "HillScoreClimbByBike":
                    return Column.HillScoreClimbByBike;
                case "HillScoreCycle2Max":
                    return Column.HillScoreCycle2Max;
                case "HillScoreFiets":
                    return Column.HillScoreFiets;
                case "HillScoreCourseScoreCycling":
                    return Column.HillScoreCourseScoreCycling;
                case "HillScoreCourseScoreRunning":
                    return Column.HillScoreCourseScoreRunning;
                case "VAM":
                    return Column.VAM;
                case "WKg":
                    return Column.WKg;
                case "StoppedTime":
                    return Column.StoppedTime;
                case "MaxGrade":
                    return Column.MaxGrade;
                case "MinGrade":
                    return Column.MinGrade;
                //case "HillCategory":
                //    return Column.HillCategory;
                //case "AscentDescent":
                //    return Column.AscentDescent;

            }
            return Column.HillId;
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
            set { id = value; }
            get { return id; }
        }

        public string Text(string columnId)
        {
            return TextColumnLookup(treeColumn);
        }

        public int Width
        {
            set { width = value; }
            get { return width; }
        }

        #endregion
    }
}
