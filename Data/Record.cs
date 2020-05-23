using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Chart;
using ZoneFiveSoftware.Common.Data.GPS;
using System.Diagnostics;
using CourseScore.UI;

namespace CourseScore.Data
{
    /// <summary>
    /// Record will store 1 individual record of any type
    /// </summary>
    public class Record
    {
        #region Fields

        private int rank;
        private string recValue;
        private IActivity activity;
        private DateTime startDateTime;
        private float startDistance;
        private float maxElevation;
        private float minElevation;
        private RecordCategory category;
        private DateTime trueStartDate;

        #endregion

        #region User Set Properties

        /// <summary>
        /// The name of the record (used for overall record types)
        /// </summary>
        public string Name
        {
            get { return activity.Name; }
        }

        /// <summary>
        /// The rank of the record for the top x list (1st, 2nd, etc)
        /// </summary>
        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        /// <summary>
        /// The value of the record (used for overall record types)
        /// </summary>
        public string RecValue
        {
            get { return recValue; }
            set { recValue = value; }
        }
        #endregion

        #region Class Calculated Properties

        /// <summary>
        /// Activity containing or related to this record
        /// </summary>
        public IActivity Activity
        {
            get { return activity; }
            set { activity = value; }
        }

        public string ActivityCategory
        {
            get
            {
                IActivityCategory category = activity.Category;
                string catString = category.Name;
                if (category.Parent != null)
                {
                    while (category.Parent.Parent != null)
                    {
                        category = category.Parent;
                        catString = category.Name + ": " + catString;
                    }
                }

                return catString;
            }
        }

        /// <summary>
        /// The actual distance for the record (in meters)
        /// </summary>
        public float ActualDistance
        {
            get
            {
                return (float)Length.Convert(ActivityInfoCache.Instance.GetInfo(activity).DistanceMeters, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits);
            }
        }

        /// <summary>
        /// The average heart rate of the duration of the record
        /// </summary>
        public int AvgHR
        {
            get
            {
                return (int)ActivityInfoCache.Instance.GetInfo(activity).AverageHeartRate;
            }
        }

        /// <summary>
        /// The fastest pace of the record
        /// </summary>
        public TimeSpan AvgPace
        {
            get
            {
                if (Speed != 0 && !float.IsNaN(Speed))
                {
                    int seconds = (int)(60 * 60 / Speed);
                    return new TimeSpan(0, 0, 0, seconds);
                }
                else
                {
                    return new TimeSpan(0);
                }
            }
        }

        public float AvgCadence
        {
            get
            {
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                if (info.SmoothedCadenceTrack != null)
                {
                    return info.AverageCadence;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float AvgPower
        {
            get
            {
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                return info.AveragePower;
            }
        }

        public string CategoryName
        {
            get { return category.Name; }
        }

        public RecordCategory Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// The amount of elevation change for the record
        /// </summary>
        public float ElevationChange
        {
            get
            {
                float a = TotalAscend;
                float d = TotalDescend;
                return a + d;
            }
        }

        /// <summary>
        /// The date and time that the record ends
        /// </summary>
        public DateTime EndDateTime
        {
            get
            {
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                DateTime endDateTime = info.ActualTrackEnd.ToLocalTime();

                if (endDateTime == DateTime.MinValue)
                {
                    endDateTime = activity.StartTime + info.Time;
                }

                return endDateTime;
            }
        }

        /// <summary>
        /// The distance from the starting point of the activity to the ending point of the record 
        /// </summary>
        public float EndDistance
        {
            get
            {
                return ActualDistance + (float)Length.Convert(startDistance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits);
            }
        }

        public List<IGPSLocation> GPSLocation
        {
            get
            {
                List<IGPSLocation> locations = new List<IGPSLocation>();
                for (int i = 0; i < activity.GPSRoute.Count; i++)
                {
                    GPSPoint p = (GPSPoint)activity.GPSRoute[i].Value;
                    GPSLocation l = new GPSLocation(p.LatitudeDegrees, p.LongitudeDegrees);
                    locations.Add(l);
                }
                return locations;
            }
        }

        public List<IGPSPoint> GPSPoints
        {
            get
            {
                List<IGPSPoint> points = new List<IGPSPoint>();
                for (int i = 0; i < activity.GPSRoute.Count; i++)
                {
                    GPSPoint p = (GPSPoint)activity.GPSRoute[i].Value;
                    points.Add(p);
                }
                return points;
            }
        }

        /// <summary>
        /// The location of the record
        /// </summary>
        public string Location
        {
            get { return activity.Location; }
        }

        /// <summary>
        /// Maximum Grade
        /// </summary>
        public float MaxGrade
        {
            get
            {
                return ActivityInfoCache.Instance.GetInfo(activity).SmoothedGradeTrack.Max * 100;
            }
        }

        /// <summary>
        /// The maximum heart rate for the record
        /// </summary>
        public int MaxHR
        {
            get
            {
                return (int)ActivityInfoCache.Instance.GetInfo(activity).MaximumHeartRate;
            }
        }

        public float MaxCadence
        {
            get
            {
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                if (info.SmoothedCadenceTrack != null)
                {
                    return info.MaximumCadence;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float MaxPower
        {
            get
            {
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                return info.MaximumPower;
            }
        }

        /// <summary>
        /// Maximum Average Speed
        /// </summary>
        public float MaxSpeed
        {
            get
            {
                // TODO: Separate MaxAvgSpeed/Pace from Fastest (instantaneous) speed/pace
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                double speed = info.FastestSpeedMetersPerSecond; // meter/sec
                speed = Length.Convert(speed, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits); // (ST Units)/sec
                speed = speed * 60 * 60; // Convert sec. to hours (mph or km/hr)

                return (float)speed;
            }
        }

        public float MaxElevation
        {
            get { return (float)Length.Convert(maxElevation, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits); }
            set { maxElevation = value; }
        }

        /// <summary>
        /// Minimum Grade
        /// </summary>
        public float MinGrade
        {
            get
            {
                return ActivityInfoCache.Instance.GetInfo(activity).SmoothedGradeTrack.Min * 100;
            }
        }

        public float MinElevation
        {
            get { return (float)Length.Convert(minElevation, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits); }
            set { minElevation = value; }
        }

        public string ReferenceId
        {
            get
            {
                return activity.Category.ReferenceId;
            }
        }

        /// <summary>
        /// The fastest speed of the record
        /// </summary>
        public float Speed
        {
            get
            {
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                double speed = info.AverageSpeedMetersPerSecond; // meter/sec

                // Sometimes on nonGPS tracks, the ActivityInfo is incomplete.  In that case, you'll need to figure out speed
                if (double.IsNaN(speed))
                {
                    try
                    {
                        // Build a new track that starts at distance=0m
                        INumericTimeDataSeries zeroDistanceTrack = Utilities.SetTrackStartValueToZero(activity.DistanceMetersTrack);
                        speed = zeroDistanceTrack[zeroDistanceTrack.Count - 1].Value / TotalTime.TotalSeconds; // meter/sec
                    }
                    catch
                    {
                        speed = 0;
                    }
                }
                    speed = Length.Convert(speed, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits); // (ST Units)/sec
                    speed = speed * 60 * 60; // Convert sec. to hours (mph or km/hr)

                return (float)speed;
            }
        }

        /// <summary>
        /// The date and time that the record starts
        /// </summary>
        public DateTime StartDateTime
        {
            get
            {
                if (startDateTime != DateTime.MinValue)
                {
                    return startDateTime;
                }
                else
                {
                    DateTime actualTrackStart = ActivityInfoCache.Instance.GetInfo(activity).ActualTrackStart.ToLocalTime();

                    if (actualTrackStart == DateTime.MinValue)
                    {
                        actualTrackStart = activity.StartTime.ToLocalTime();
                    }

                    return actualTrackStart;
                }
            }

            set
            {
                startDateTime = value;
            }
        }

        /// <summary>
        /// The distance from the starting point of the activity to the starting point of the record
        /// </summary>
        public float StartDistance
        {
            get
            {
                return (float)Length.Convert(startDistance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits);
            }
        }

        /// <summary>
        /// The total distance ascended for the record
        /// </summary>
        public float TotalAscend
        {
            get
            {
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                float totalAscend = (float)info.TotalAscendingMeters(PluginMain.GetApplication().Logbook.ClimbZones[0]);

                if (totalAscend == 0 || float.IsNaN(totalAscend))
                {
                    totalAscend = info.Activity.TotalAscendMetersEntered;
                }

                //totalAscend = (float)Length.Convert(totalAscend, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);

                return totalAscend;
            }
        }

        /// <summary>
        /// The total distance descended for the record
        /// </summary>
        public float TotalDescend
        {
            get
            {
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                float totalDescend = (float)info.TotalDescendingMeters(PluginMain.GetApplication().Logbook.ClimbZones[0]);

                if (totalDescend == 0 || float.IsNaN(totalDescend))
                {
                    totalDescend = info.Activity.TotalDescendMetersEntered;
                }

                //totalDescend = (float)Length.Convert(totalDescend, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);

                return totalDescend;
            }
        }

        /// <summary>
        /// The temperature for the record in the correct format according to preferences
        /// </summary>
        public float Temp
        {
            get
            {
                return (float)Temperature.Convert(activity.Weather.TemperatureCelsius, Temperature.Units.Celsius, PluginMain.GetApplication().SystemPreferences.TemperatureUnits);
            }
        }

        /// <summary>
        /// Activity Total Time
        /// </summary>
        public TimeSpan TotalTime
        {
            get
            {
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);

                if (info.TimeMoving == TimeSpan.Parse("00:00:00"))
                {
                    if (info.TimeNonPaused != TimeSpan.Parse("00:00:00"))
                    {
                        return info.TimeNonPaused;
                    }
                    else
                    {
                        return activity.TotalTimeEntered;
                    }
                }
                else
                {
                    return info.TimeMoving;
                }
            }
        }

        public Speed.Units SpeedUnits
        {
            get
            {
                return activity.Category.SpeedUnits;
            }
        }

        public float TotalCalories
        {
            get
            {
                return activity.TotalCalories;
            }
        }

        public DateTime TrueStartDate
        {
            get { return trueStartDate.Add(activity.TimeZoneUtcOffset); }
            set { trueStartDate = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Used to generate the record properties of the supplied activity
        /// </summary>
        /// <param name="activity">The full activity for the record</param>
        /// <param name="category">The Record Category for this record</param>
        /// <param name="gpsTrack">The GPS route of the actual record</param>
        /// <param name="hrTrack">The HR track of the actual record</param>
        /// <param name="pwrTrack">The power track of the actual record</param>
        /// <param name="cadTrack">The cadence track of the actual record</param>
        public Record(IActivity activity, RecordCategory category, IGPSRoute gpsTrack, INumericTimeDataSeries hrTrack, INumericTimeDataSeries pwrTrack, INumericTimeDataSeries cadTrack, IDistanceDataTrack distTrack, INumericTimeDataSeries elevTrack, DateTime activityStartTime)
        {
            // Create new activity from template
            IActivity recActivity = (IActivity)Activator.CreateInstance(activity.GetType());

            // HACK: Manually Clone 'activity' until a better way is found
            recActivity.Category = activity.Category;
            recActivity.DistanceMetersTrack = distTrack;
            recActivity.ElevationMetersTrack = elevTrack;
            recActivity.GPSRoute = gpsTrack;
            recActivity.HasStartTime = activity.HasStartTime;
            recActivity.HeartRatePerMinuteTrack = hrTrack;
            recActivity.Intensity = activity.Intensity;
            recActivity.Location = activity.Location;
            recActivity.Name = activity.Name;
            recActivity.PowerWattsTrack = pwrTrack;
            recActivity.CadencePerMinuteTrack = cadTrack;
            recActivity.Weather.Conditions = activity.Weather.Conditions;
            recActivity.Weather.CurentDirectionDegrees = activity.Weather.CurentDirectionDegrees;
            recActivity.Weather.CurentSpeedKilometersPerHour = activity.Weather.CurentSpeedKilometersPerHour;
            recActivity.Weather.HumidityPercent = activity.Weather.HumidityPercent;
            recActivity.Weather.TemperatureCelsius = activity.Weather.TemperatureCelsius;
            recActivity.Weather.WindDirectionDegrees = activity.Weather.WindDirectionDegrees;
            recActivity.Weather.WindSpeedKilometersPerHour = activity.Weather.WindSpeedKilometersPerHour;

            // Set the start time for the record activity
            recActivity.StartTime = activityStartTime;

            // Set up the activity info for pulling summary data
            ActivityInfo info = ActivityInfoCache.Instance.GetInfo(recActivity);

            // Set the record category
            this.category = category;

            // Max and Min elevation seen over the route
            float maxE = float.NegativeInfinity;
            float minE = float.PositiveInfinity;

            if (activity.GPSRoute != null && activity.GPSRoute.Count > 0)
            {
                GPSRoute startRoute = new GPSRoute();
                for (int i = 0; i < activity.GPSRoute.Count; i++)
                {
                    GPSPoint p = (GPSPoint)activity.GPSRoute[i].Value;
                    if (p.ElevationMeters > maxE)
                    {
                        maxE = p.ElevationMeters;
                    }

                    if (p.ElevationMeters < minE)
                    {
                        minE = p.ElevationMeters;
                    }

                    if (gpsTrack.Count == 0)
                    {
                        break;
                    }

                    if (p.Equals((GPSPoint)gpsTrack[0].Value))
                    {
                        break;
                    }
                    else
                    {
                        startRoute.Add(activity.GPSRoute.EntryDateTime(activity.GPSRoute[i]), p);
                    }
                }
                startDistance = startRoute.TotalDistanceMeters;
            }
            else if (activity.ElevationMetersTrack != null)
            {
                for (int i = 0; i < activity.ElevationMetersTrack.Count; i++)
                {
                    if (activity.ElevationMetersTrack[i].Value > maxE)
                    {
                        maxE = activity.ElevationMetersTrack[i].Value;
                    }

                    if (activity.ElevationMetersTrack[i].Value < maxE)
                    {
                        minE = activity.ElevationMetersTrack[i].Value;
                    }
                }
                startDistance = 0;
            }

            this.maxElevation = maxE;
            this.minElevation = minE;
            this.trueStartDate = activity.StartTime;
            this.activity = recActivity;
        }

        //public Record(IActivity activity, RecordCategory category)
        //{
        //    // TODO: WTF? Why does this only collect min and max elevation???
        //    this.activity = activity;
        //    this.category = category;

        //    if (activity != null)
        //    {
        //        // Create new activity from template
        //        IActivity recActivity = (IActivity)Activator.CreateInstance(activity.GetType());

        //        // Max and Min elevation seen over the route
        //        if (activity.GPSRoute != null && activity.GPSRoute.Count > 0)
        //        {
        //            GPSPoint px = (GPSPoint)activity.GPSRoute[0].Value;
        //            float maxE = px.ElevationMeters;
        //            float minE = px.ElevationMeters;
        //            for (int i = 0; i < activity.GPSRoute.Count; i++)
        //            {
        //                GPSPoint p = (GPSPoint)activity.GPSRoute[i].Value;
        //                if (p.ElevationMeters > maxE)
        //                {
        //                    maxE = p.ElevationMeters;
        //                }

        //                if (p.ElevationMeters < minE)
        //                {
        //                    minE = p.ElevationMeters;
        //                }
        //            }

        //            this.maxElevation = maxE;
        //            this.minElevation = minE;
        //        }
        //    }
        //}

        #endregion

        #region IComparable Members

        // Default Sort(): Start date (newest first)
        public int CompareTo(object obj)
        {
            Record a = (Record)obj;
            return DateTime.Compare(a.StartDateTime, this.StartDateTime);
        }

        // Column specific sort
        public int CompareTo(Record a2, RecordComparer.ComparisonType comparisonMethod, RecordComparer.Order sortOrder)
        {
            int result = 0;

            switch (comparisonMethod)
            {
                // Define all different sort methods
                case RecordComparer.ComparisonType.AvgPower:
                    if (AvgPower != a2.AvgPower)
                    {
                        result = AvgPower.CompareTo(a2.AvgPower);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.MaxPower:
                    if (MaxPower != a2.MaxPower)
                    {
                        result = MaxPower.CompareTo(a2.MaxPower);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.Category:
                    if (CategoryName != a2.CategoryName)
                    {
                        result = CategoryName.CompareTo(a2.CategoryName);
                    }
                    else
                    {
                        // TODO: WTF is this?
                        switch (Category.Type)
                        {
                            case RecordCategory.RecordType.DistancePace:
                                result = AvgPace.CompareTo(a2.AvgPace);
                                break;
                            case RecordCategory.RecordType.MaxTemperature:
                                result = Temp.CompareTo(a2.Temp);
                                break;
                        }
                    }

                    break;

                case RecordComparer.ComparisonType.Distance:
                    if (ActualDistance != a2.ActualDistance)
                    {
                        result = ActualDistance.CompareTo(a2.ActualDistance);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.StartDistance:
                    if (StartDistance != a2.StartDistance)
                    {
                        result = StartDistance.CompareTo(a2.StartDistance);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.EndDistance:
                    if (EndDistance != a2.EndDistance)
                    {
                        result = EndDistance.CompareTo(a2.EndDistance);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.Location:
                    if (Location != a2.Location)
                    {
                        result = Location.CompareTo(a2.Location);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.ActivityCategory:
                    if (ActivityCategory != a2.ActivityCategory)
                    {
                        result = ActivityCategory.CompareTo(a2.ActivityCategory);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.Name:
                    if (Name != a2.Name)
                    {
                        result = Name.CompareTo(a2.Name);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.TotalTime:
                    if (TotalTime != a2.TotalTime)
                    {
                        result = TotalTime.CompareTo(a2.TotalTime);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.ElevationChange:
                    if (ElevationChange != a2.ElevationChange)
                    {
                        result = ElevationChange.CompareTo(a2.ElevationChange);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.TotalAscend:
                    if (TotalAscend != a2.TotalAscend)
                    {
                        result = TotalAscend.CompareTo(a2.TotalAscend);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.TotalDescend:
                    if (TotalDescend != a2.TotalDescend)
                    {
                        result = TotalDescend.CompareTo(a2.TotalDescend);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.Temp:
                    if (Temp != a2.Temp)
                    {
                        result = Temp.CompareTo(a2.Temp);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.Rank:
                    if (Rank != a2.Rank)
                    {
                        result = Rank.CompareTo(a2.Rank);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.AvgPace:
                    if (AvgPace != a2.AvgPace)
                    {
                        result = AvgPace.CompareTo(a2.AvgPace);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.Speed:
                    if (Speed != a2.Speed)
                    {
                        result = Speed.CompareTo(a2.Speed);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.AvgHR:
                    if (AvgHR != a2.AvgHR)
                    {
                        result = AvgHR.CompareTo(a2.AvgHR);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.TotalCalories:
                    if (TotalCalories != a2.TotalCalories)
                    {
                        result = TotalCalories.CompareTo(a2.TotalCalories);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.AvgCadence:
                    if (AvgCadence != a2.AvgCadence)
                    {
                        result = AvgCadence.CompareTo(a2.AvgCadence);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.MaxSpeed:
                    if (MaxSpeed != a2.MaxSpeed)
                    {
                        result = MaxSpeed.CompareTo(a2.MaxSpeed);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.MaxHR:
                    if (MaxHR != a2.MaxHR)
                    {
                        result = MaxHR.CompareTo(a2.MaxHR);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.MaxElevation:
                    if (MaxElevation != a2.MaxElevation)
                    {
                        result = MaxElevation.CompareTo(a2.MaxElevation);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.MinElevation:
                    if (MinElevation != a2.MinElevation)
                    {
                        result = MinElevation.CompareTo(a2.MinElevation);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.MaxCadence:
                    if (MaxCadence != a2.MaxCadence)
                    {
                        result = MaxCadence.CompareTo(a2.MaxCadence);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.MaxGrade:
                    if (MaxGrade != a2.MaxGrade)
                    {
                        result = MaxGrade.CompareTo(a2.MaxGrade);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.MinGrade:
                    if (MinGrade != a2.MinGrade)
                    {
                        result = MinGrade.CompareTo(a2.MinGrade);
                    }
                    else
                    {
                        result = StartDateTime.CompareTo(a2.StartDateTime);
                    }

                    break;

                case RecordComparer.ComparisonType.StartDateTime:
                default:
                    result = StartDateTime.CompareTo(a2.StartDateTime);
                    break;
            }

            if (sortOrder == RecordComparer.Order.Descending)
            {
                return result * -1;
            }
            else
            {
                return result;
            }
        }

        #endregion

        public class RecordComparer : IComparer<Record>
        {
            #region Fields

            private ComparisonType _comparisonType;
            private Order _sortOrder;

            #endregion

            #region Enumerations

            public enum ComparisonType
            {
                StartDateTime = 1,
                ElevationChange = 2,
                NormPower = 3,
                AvgPower = 4,
                Location = 5,
                Category = 6,
                Distance = 7,
                TotalTime = 8,
                TotalAscend = 9,
                TotalDescend = 10,
                Temp = 11,
                Rank = 12,
                ATL = 13,
                CTL = 14,
                AvgPace = 15,
                AvgSpeed = 16,
                AvgHR = 17,
                TotalCalories = 18,
                AvgCadence = 19,
                AvgGrade = 20,
                Day = 21,
                FastestPace = 22,
                MaxSpeed = 23,
                MaxHR = 24,
                MaxPower = 25,
                Name = 26,
                StartDistance = 27,
                EndDistance = 28,
                EndDateTime = 29,
                ActivityCategory = 30,
                Speed = 31,
                MaxElevation = 32,
                MinElevation = 33,
                MaxCadence = 34,
                MinCadence = 35,
                MaxGrade = 36,
                MinGrade = 37
            }

            public enum Order
            {
                Ascending = 1,
                Descending = 2
            }

            #endregion

            #region Properties

            public ComparisonType ComparisonMethod
            {
                get { return _comparisonType; }
                set { _comparisonType = value; }
            }

            public Order SortOrder
            {
                get { return _sortOrder; }
                set { _sortOrder = value; }
            }

            #endregion

            #region IComparer<Record> Members

            public int Compare(Record x, Record y)
            {
                return x.CompareTo(y, _comparisonType, _sortOrder);
            }

            #endregion
        }
    }
}
