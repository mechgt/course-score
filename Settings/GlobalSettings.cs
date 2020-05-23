using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using CourseScore.UI.DetailPage;
using CourseScore.Data;

namespace CourseScore.Settings
{
    /// <summary>
    /// Global settings
    /// </summary>
    [XmlRootAttribute(ElementName = "CourseScore", IsNullable = false)]
    public class GlobalSettings
    {
        private static GlobalSettings instance;
        private static HillsDetailControl.HillChartType chartType = HillsDetailControl.HillChartType.ClimbDistance;
        private static double elevationPercent = .33f;
        private static double distancePercent = .33f;
        private static double gainElevationRequired = 3;
        private static double hillDistanceRequired = 3;
        private static double maxDescentLength = 1000;
        private static double maxDescentElevation = 1000;
        private static double minAvgGrade = .00f;
        private static List<ChartField.Field> chartFields;
        private static List<ChartField.Field> multiChartFields;
        //private static ChartField.Field multiChartSummaryField = ChartField.Field.VAM;
        private static HillsDetailControl.InfoType infoType = HillsDetailControl.InfoType.Overall;
        private static int splitterDistance;
        private static HillsDetailControl.ScoreType scoreType = HillsDetailControl.ScoreType.Cycling;
        private static HillsDetailControl.ColorType colorType = HillsDetailControl.ColorType.Hardest;
        private static ScoreEquation.Score scoreEquation = CourseScore.Data.ScoreEquation.Score.Fiets;
        private static double cyclingFactor = .35f;
        private static double cyclingOffset = .1f;
        private static TreeColumnCollection treeColumns;
        private static int numFixedColumns = 0;
        private static bool hillMarkers = true;
        private static HillsDetailControl.HillChartTypeMultiMode hillChartTypeMultiMode = HillsDetailControl.HillChartTypeMultiMode.Details;

        public HillsDetailControl.HillChartType ChartType
        {
            get { return chartType; }
            set { chartType = value; }
        }

        internal static GlobalSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalSettings();
                }

                return instance;
            }
        }

        /// <summary>
        /// Configurable hill definition criteria
        /// </summary>
        public double ElevationPercent
        {
            get
            {
                return elevationPercent;
            }
            set
            {
                elevationPercent = value;
            }
        }

        /// <summary>
        /// Configurable hill definition criteria
        /// </summary>
        public double DistancePercent
        {
            get
            {
                return distancePercent;
            }
            set
            {
                distancePercent = value;
            }
        }

        /// <summary>
        /// Configurable hill definition criteria
        /// </summary>
        public double GainElevationRequired
        {
            get
            {
                return gainElevationRequired;
            }
            set
            {
                gainElevationRequired = value;
            }
        }

        /// <summary>
        /// Configurable hill definition criteria
        /// </summary>
        public double HillDistanceRequired
        {
            get
            {
                return hillDistanceRequired;
            }
            set
            {
                hillDistanceRequired = value;
            }
        }

        /// <summary>
        /// Configurable hill definition criteria
        /// </summary>
        public double MaxDescentLength
        {
            get
            {
                return maxDescentLength;
            }
            set
            {
                maxDescentLength = value;
            }
        }

        /// <summary>
        /// Configurable hill deffinition criteria
        /// </summary>
        public double MaxDescentElevation
        {
            get
            {
                return maxDescentElevation;
            }
            set
            {
                maxDescentElevation = value;
            }
        }

        /// <summary>
        /// Configurable hill definition criteria
        /// </summary>
        public double MinAvgGrade
        {
            get
            {
                return minAvgGrade;
            }
            set
            {
                minAvgGrade = value;
            }
        }
        
        /// <summary>
        /// List of fields shown in Single activity mode.
        /// Note that first item in list is primary chart (left axis).
        /// </summary>
        public List<ChartField.Field> ChartFields
        {
            get
            {
                if (chartFields == null)
                {
                    chartFields = new List<CourseScore.Data.ChartField.Field>();
                }
                return chartFields;
            }
            set
            {
                chartFields = value;
            }
        }

        /// <summary>
        /// List of fields shown in Multiple activity mode
        /// Note that first item in list is primary chart (left axis).
        /// </summary>
        public List<ChartField.Field> MultiChartFields
        {
            get
            {
                if (multiChartFields == null)
                {
                    multiChartFields = new List<CourseScore.Data.ChartField.Field>();
                }
                return multiChartFields;
            }
            set
            {
                multiChartFields = value;
            }
        }

        /// <summary>
        /// User setting: Top pane view style
        /// </summary>
        public HillsDetailControl.InfoType InfoType
        {
            get
            {
                return infoType;
            }
            set { infoType = value; }
        }

        /// <summary>
        /// User display preference
        /// </summary>
        public int SplitterDistance
        {
            get
            {
                if (splitterDistance <= 30)
                {
                    splitterDistance = 200;
                }

                return splitterDistance;
            }
            set
            {
                splitterDistance = value;
            }
        }

        public HillsDetailControl.ScoreType ScoreType
        {
            get
            {
                return scoreType;
            }
            set { scoreType = value; }
        }

        /// <summary>
        /// User display preference for how to color hills on lower chart for an activity
        /// </summary>
        public HillsDetailControl.ColorType ColorType
        {
            get { return colorType; }
            set { colorType = value; }
        }

        public ScoreEquation.Score ScoreEquation
        {
            get
            {
                return scoreEquation;
            }
            set { scoreEquation = value; }
        }

        public double CyclingFactor
        {
            get
            {
                return cyclingFactor;
            }
            set
            {
                cyclingFactor = value;
            }
        }

        public double CyclingOffset
        {
            get
            {
                return cyclingOffset;
            }
            set
            {
                cyclingOffset = value;
            }
        }

        /// <summary>
        /// Gets a value indicating if the chart should be distance based.
        /// The value is based off of ChartType.
        /// Returns true if chart should be distance based, or false if time based.
        /// </summary>
        public bool IsDistanceChart
        {
            get
            {
                if (ChartType == HillsDetailControl.HillChartType.ClimbDistance ||
                    ChartType == HillsDetailControl.HillChartType.DescentDistance ||
                    ChartType == HillsDetailControl.HillChartType.Overall ||
                    ChartType == HillsDetailControl.HillChartType.SplitsDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public TreeColumnCollection TreeColumns
        {
            get
            {
                if (treeColumns == null)
                {
                    treeColumns = new TreeColumnCollection();
                }

                return treeColumns;
            }
            set
            {
                treeColumns = value;
            }
        }

        /// <summary>
        /// Number of fixed columns in the tree list
        /// </summary>
        public int NumFixedColumns
        {
            get
            {
                return numFixedColumns;
            }
            set
            {
                numFixedColumns = value;
            }
        }

        public bool HillMarkers
        {
            get
            {
                return hillMarkers;
            }
            set
            {
                hillMarkers = value;
            }
        }

        /// <summary>
        /// User setting: Bottom pane view style in multiple activity mode
        /// </summary>
        public HillsDetailControl.HillChartTypeMultiMode HillChartTypeMultiMode
        {
            get
            {
                return hillChartTypeMultiMode;
            }
            set { hillChartTypeMultiMode = value; }
        }

        /// <summary>
        /// User setting: Bottom pane data item when in Summary mode
        /// </summary>
        /*public ChartField.Field MultiChartSummaryField
        {
            get
            {
                return multiChartSummaryField;
            }
            set { multiChartSummaryField = value; }
        }*/
    }
}
