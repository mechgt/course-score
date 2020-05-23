using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Visuals;

namespace CourseScore.Data
{
    /// <summary>
    /// RecordCategory is used to store the user defined set of categories for records
    /// </summary>
    [XmlRoot("RecordCategory", IsNullable = false)]
    public class RecordCategory : IComparable
    {
        // TODO: Remove unused RecordCategory stuff in this class.  Lots of unused RB remenants here.

        #region Fields

        private RecordType type;
        private string name;
        private float distance;
        private Length.Units lengthMeasure;
        private float time;
        private List<string> categories;
        private Guid id;

        #endregion

        #region Enumerations

        /// <summary>
        /// Enumeration to broadly define what type of record this is.
        /// </summary>
        public enum RecordType
        {
            // TODO: How to override ToString() of this enumeration in order to localize it?
            //[Description(CommonResources.Text.LabelDistance)]
            Distance = 0,
            DistancePace = 1,
            AllActivities = 2,
            MaxTemperature = 3,
            MinTemperature = 4,
            MaxDistance = 5,
            //[Description(CommonResources.Text.LabelTotalTime)]
            TotalTime = 6,
            MaxSpeed = 7,
            MaxHR = 8,
            FastestSpeed = 9,
            FastestPace = 10,
            AvgHR = 11,
            MaxElevation = 12,
            MinElevation = 13,
            MaxCadence = 14,
            ElevationDifference = 15,
            MaxPower = 16,
            AvgPower = 17,
            MaxAscent = 18,
            MaxDescent = 19,
            TotalCalories = 20,
            AvgCadence = 21,
            TotalTimeOneDay = 22,
            TotalDistanceOneDay = 23,
            MaxGrade = 24,
            MinGrade = 25,
            AscentDescent = 26,
            HottestColdest = 27,
            MaxMinElevation = 28,
            MaxMinGrade = 29,
            SpeedPace = 30
        }

        public static string RecordTypeToString(RecordType type)
        {
            // TODO: Localize this enumeration
            // Is this the best way? 
            // Do not override ToString, but rather change what the combobox displays to something else
            switch (type)
            {
                //case RecordType.AllActivities:
                //    return Resources.Strings.Label_AllActivities;
                case RecordType.AscentDescent:
                    return CommonResources.Text.LabelAscending + " / " + CommonResources.Text.LabelDescending;
                case RecordType.AvgCadence:
                    return CommonResources.Text.LabelMaxAvgCadence;
                case RecordType.AvgHR:
                    return CommonResources.Text.LabelMaxAvgHR;
                case RecordType.AvgPower:
                    return CommonResources.Text.LabelMaxAvgPower;
                case RecordType.Distance:
                    return CommonResources.Text.LabelDistance;
                case RecordType.DistancePace:
                    return CommonResources.Text.LabelDistance + " / " + CommonResources.Text.LabelPace;
                case RecordType.ElevationDifference:
                    return CommonResources.Text.LabelElevationChange;
                case RecordType.FastestPace:
                    return CommonResources.Text.LabelFastestPace;
                case RecordType.FastestSpeed:
                    return CommonResources.Text.LabelFastestSpeed;
                case RecordType.HottestColdest:
                    return "" + " / " + "" + CommonResources.Text.LabelTemperature;
                case RecordType.MaxAscent:
                    return CommonResources.Text.LabelAscending;
                case RecordType.MaxCadence:
                    return CommonResources.Text.LabelMaxCadence;
                case RecordType.MaxDescent:
                    return CommonResources.Text.LabelDescending;
                case RecordType.MaxDistance:
                    return "";
                case RecordType.MaxElevation:
                    return "";
                case RecordType.MaxGrade:
                    return CommonResources.Text.LabelMaxGrade;
                case RecordType.MaxHR:
                    return CommonResources.Text.LabelMaxHR;
                case RecordType.MaxMinElevation:
                    return "" + " / " + "" + CommonResources.Text.LabelElevation;
                case RecordType.MaxMinGrade:
                    return "" + " / " + "" + CommonResources.Text.LabelGrade;
                case RecordType.MaxPower:
                    return CommonResources.Text.LabelMaxPower;
                case RecordType.MaxSpeed:
                    return "";
                case RecordType.MaxTemperature:
                    return "";
                case RecordType.MinElevation:
                    return "";
                case RecordType.MinGrade:
                    return "";
                case RecordType.MinTemperature:
                    return "";
                case RecordType.SpeedPace:
                    return CommonResources.Text.LabelPaceOrSpeed;
                case RecordType.TotalCalories:
                    return CommonResources.Text.LabelCalories;
                case RecordType.TotalDistanceOneDay:
                    return "";
                case RecordType.TotalTime:
                    return "";
                default:
                    return "Bad Value!!!";
            }

            //return string.Empty;
        }

        #endregion

        #region Constructors
        public RecordCategory()
        {
            this.id = Guid.NewGuid();
        }

        public RecordCategory(string name, RecordType type, List<string> refIds)
        {
            this.name = name;
            this.type = type;
            this.categories = refIds;
            this.id = Guid.NewGuid();
        }

        public RecordCategory(string name, RecordType type, List<string> refIds, float distance, Length.Units units)
        {
            this.name = name;
            this.type = type;
            this.categories = refIds;
            this.Distance = distance;
            this.LengthMeasure = units;
            this.id = Guid.NewGuid();
        }


        #endregion

        #region Properties

        public string RefId
        {
            get
            {
                if (id == null || id == Guid.Empty)
                {
                    id = Guid.NewGuid();
                }

                return id.ToString("D");
            }

            set
            {
                try
                {
                    // Try to parse, if not at least set it to a new valid guid
                    id = new Guid(value);
                }
                catch
                {
                    id = Guid.NewGuid();
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of record stored as an enumeration
        /// </summary>
        public RecordType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets or sets the name of the record category
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating the distance for the record category
        /// </summary>
        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        /// <summary>
        /// Gets or sets the unit used to measure the length of this record
        /// </summary>
        public Length.Units LengthMeasure
        {
            get { return lengthMeasure; }
            set { lengthMeasure = value; }
        }

        /// <summary>
        /// Gets a string of the unit to be displayed
        /// </summary>
        public string Display_Unit
        {
            get { return lengthMeasure.ToString(); }
        }

        public double Centimeters
        {
            get { return Length.Convert(distance, lengthMeasure, Length.Units.Centimeter); }
        }

        public double Meters
        {
            get { return Length.Convert(distance, lengthMeasure, Length.Units.Meter); }
        }

        /// <summary>
        /// Gets or sets the time for the record category
        /// </summary>
        public float Time
        {
            get { return time; }
            set { time = value; }
        }

        /// <summary>
        /// Gets or sets the categories.
        /// Categories will store an ArrayList of ST categories for which this record type applies
        /// </summary>
        public List<string> Categories
        {
            get
            {
                if (categories == null)
                {
                    categories = new List<string>();
                }

                return categories;
            }
            set
            {
                categories = value;
            }
        }

        [XmlIgnore]
        public List<string> Display_Categories
        {
            get
            {
                List<string> display = new List<string>();
                foreach (IActivityCategory cat in PluginMain.GetApplication().Logbook.ActivityCategories)
                {
                    GetActivityCategories(cat, ref display);
                }

                return display;
            }
        }

        public string Display_RecordType
        {
            get
            {
                return type.ToString();
            }
        }

        #endregion

        /// <summary>
        /// Get a list of the categories for display
        /// </summary>
        /// <param name="cat"></param>
        /// <param name="display"></param>
        public void GetActivityCategories(IActivityCategory cat, ref List<string> display)
        {
            if (cat.SubCategories.Count != 0)
            {
                for (int i = 0; i < cat.SubCategories.Count; i++)
                {
                    GetActivityCategories(cat.SubCategories[i], ref display);
                }
            }
            else
            {
                if (categories.Contains(cat.ReferenceId))
                {
                    if (cat.Parent != null)
                    {
                        display.Add(cat.Parent.Name + ": " + cat.Name);
                    }
                    else
                    {
                        display.Add(cat.Name);
                    }
                }
            }
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            RecordCategory rc = (RecordCategory)obj;
            if (rc.Centimeters > this.Centimeters)
            {
                return -1;
            }
            else if (rc.Centimeters == this.Centimeters)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        #endregion

    }
}
