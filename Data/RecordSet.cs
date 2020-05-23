using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Visuals;

namespace CourseScore.Data
{
    /// <summary>
    /// A set of records representing a record category
    /// </summary>
    class RecordSet
    {
        #region Fields

        private List<Record> records;
        private RecordCategory category;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a set of records for a single specified category
        /// </summary>
        /// <param name="category">Category in which to find records for</param>
        /// <param name="activities">List of activities to search</param>
        public RecordSet(RecordCategory category, IEnumerable<IActivity> activities)
        {
            // Clear private record list
            records = new List<Record>();
            Record record;
            this.category = category;

            if (category.Type == RecordCategory.RecordType.Distance)
            {
                foreach (IActivity activity in activities)
                {
                    if (CheckCategory(activity, category))
                    {
                        // NOTE: Here's a good debug point to investivate specific oddball activities
                        //if (activity.StartTime.Date == new DateTime(2010, 07, 25))
                        //{ }

                        record = GetRecord(activity, category);

                        if (record != null)
                        {
                            records.Add(record);
                        }
                    }
                }

                // Assign rank to each record in this record set
                records = RankRecords(records);
            }
            else if (category.Type == RecordCategory.RecordType.AllActivities)
            {
                foreach (IActivity activity in activities)
                {
                    record = GetRecord(activity, category);
                    records.Add(record);
                }
            }
            else
            {
                // Store Extreme Overall Records
                records = GetExtremeOverallRecordSet(activities, category);
            }

            //WriteRecordsToCSV(records);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The list of records pulled from the supplied activities and categories
        /// </summary>
        public List<Record> Records
        {
            get { return records; }
        }

        /// <summary>
        /// The Category associated with the record
        /// </summary>
        public RecordCategory Category
        {
            get { return category; }
        }

        /// <summary>
        /// The Category name for this record set.
        /// </summary>
        public string CategoryName
        {
            get { return category.Name; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks to see if the activity's category is in the list
        /// </summary>
        /// <param name="act">Activity to check</param>
        /// <param name="cat">RecordCategory to check against</param>
        /// <returns></returns>
        public static bool CheckCategory(IActivity act, RecordCategory cat)
        {
            if (cat.Display_Categories != null && cat.Display_Categories.Count > 0)
            {
                foreach (string refID in cat.Categories)
                {
                    if (refID == act.Category.ReferenceId)
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        public static void WriteRecordsToCSV(List<Record> records)
        {
            System.IO.StreamWriter csvWriter = new System.IO.StreamWriter("records.csv");
            csvWriter.WriteLine("Record_Category, Rec_Cat_Meters, Location, Actual_Rec_Distance, AvgHR, MaxHR, From_Time, To_Time, From_Distance, To_Distance");
            foreach (Record r in records)
            {
                csvWriter.WriteLine(r.CategoryName + "," + r.Category.Meters + "," + r.Location + "," + r.ActualDistance + "," + r.AvgHR + "," + r.MaxHR + "," + r.StartDateTime + "," + r.EndDateTime + "," + r.StartDistance + "," + r.EndDistance);
            }

            csvWriter.Close();
        }

        public void Sort(Record.RecordComparer comparer)
        {
            records.Sort(comparer);
        }

        /// <summary>
        /// Assign rank to records
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private static List<Record> RankRecords(List<Record> records)
        {
            // Rank records
            Record.RecordComparer comparer = new Record.RecordComparer();

            comparer.ComparisonMethod = Record.RecordComparer.ComparisonType.AvgPace;

            comparer.SortOrder = Record.RecordComparer.Order.Ascending;
            if (records != null)
            {
                records.Sort(comparer);

                int rank = 1;
                int index = 0;
                RecordCategory rankCategory = new RecordCategory();

                if (records.Count > index)
                {
                    rankCategory = records[index].Category;
                    rank = 1;

                    // Loop through all records assigning rank
                    while (index < records.Count)
                    {
                        records[index].Rank = rank;
                        index++;
                        rank++;
                    }
                }
            }

            return records;
        }

        /// <summary>
        /// Returns the category record for a single activity
        /// </summary>
        /// <param name="activity">Activity to search for a record in</param>
        /// <param name="category">Type/category of record to search for</param>
        /// <returns></returns>
        private static Record GetRecord(IActivity activity, RecordCategory category)
        {
            switch (category.Type)
            {
                case RecordCategory.RecordType.Distance:
                    return GetDistancePaceRecord(activity, category);
                case RecordCategory.RecordType.AllActivities:
                    return GetAllActivities(activity, category);
                default:
                    // Category should always be defined
                    return null;
            }
        }

        private static Record GetDistancePaceRecord(IActivity activity, RecordCategory category)
        {
            float fastestSpeed = 0;
            float currentSpeed;
            float currentDistance = 0;

            if (activity.GPSRoute != null && activity.GPSRoute.TotalElapsedSeconds > 0 && ActivityInfoCache.Instance.GetInfo(activity).DistanceMetersMoving >= category.Meters)
            {
                int recordStart = 0, recordEnd = 0, startIndex, endIndex = 0;
                List<double> p2pDistance = new List<double>();

                // Go through each starting point
                for (startIndex = 0; startIndex < activity.GPSRoute.Count; startIndex++)
                {
                    // Find end GPS point that's the proper distance away
                    while (currentDistance <= category.Meters)
                    {
                        endIndex += 1;

                        // Typical return point.  End has exceeded route.  Construct and return record for this activity/category.
                        if (endIndex >= activity.GPSRoute.Count)
                        {
                            // Construct record GPS route
                            GPSRoute recordGPS = new GPSRoute();
                            NumericTimeDataSeries recordHRTrack = new NumericTimeDataSeries();
                            NumericTimeDataSeries pwrTrack = new NumericTimeDataSeries();
                            NumericTimeDataSeries elevTrack = new NumericTimeDataSeries();
                            NumericTimeDataSeries cadTrack = new NumericTimeDataSeries();
                            DistanceDataTrack distTrack = new DistanceDataTrack();

                            for (int i = recordStart; i <= recordEnd; i++)
                            {
                                // Record information/statistics
                                DateTime time = activity.GPSRoute.EntryDateTime(activity.GPSRoute[i]);

                                recordGPS.Add(time, activity.GPSRoute[i].Value);
                                if (activity.HeartRatePerMinuteTrack != null
                                    && activity.HeartRatePerMinuteTrack.StartTime <= time
                                    && activity.HeartRatePerMinuteTrack.StartTime.AddSeconds(activity.HeartRatePerMinuteTrack.TotalElapsedSeconds) >= time)
                                {
                                    recordHRTrack.Add(time, activity.HeartRatePerMinuteTrack.GetInterpolatedValue(time).Value);
                                }

                                if (activity.PowerWattsTrack != null
                                    && activity.PowerWattsTrack.StartTime <= time
                                    && activity.PowerWattsTrack.StartTime.AddSeconds(activity.PowerWattsTrack.TotalElapsedSeconds) >= time)
                                {
                                    pwrTrack.Add(time, activity.PowerWattsTrack.GetInterpolatedValue(time).Value);
                                }

                                if (activity.CadencePerMinuteTrack != null
                                    && activity.CadencePerMinuteTrack.StartTime <= time
                                    && activity.CadencePerMinuteTrack.StartTime.AddSeconds(activity.CadencePerMinuteTrack.TotalElapsedSeconds) >= time)
                                {
                                    cadTrack.Add(time, activity.CadencePerMinuteTrack.GetInterpolatedValue(time).Value);
                                }

                                if (activity.DistanceMetersTrack != null
                                    && activity.DistanceMetersTrack.StartTime <= time
                                    && activity.DistanceMetersTrack.StartTime.AddSeconds(activity.DistanceMetersTrack.TotalElapsedSeconds) >= time)
                                {
                                    distTrack.Add(time, activity.DistanceMetersTrack.GetInterpolatedValue(time).Value);
                                }

                                if (activity.ElevationMetersTrack != null
                                    && activity.ElevationMetersTrack.StartTime <= time
                                    && activity.ElevationMetersTrack.StartTime.AddSeconds(activity.ElevationMetersTrack.TotalElapsedSeconds) >= time)
                                {
                                    elevTrack.Add(time, activity.ElevationMetersTrack.GetInterpolatedValue(time).Value);
                                }
                            }

                            // Return record
                            Record record = new Record(activity, category, recordGPS, recordHRTrack, pwrTrack, cadTrack, distTrack, elevTrack);
                            return record;
                        }
                        else
                        {
                            // Add to end of route until category distance is found
                            currentDistance += activity.GPSRoute[endIndex].Value.DistanceMetersToPoint(activity.GPSRoute[endIndex - 1].Value);
                        }
                    }

                    // In meters / second
                    currentSpeed = currentDistance / (activity.GPSRoute[endIndex].ElapsedSeconds - activity.GPSRoute[startIndex].ElapsedSeconds);

                    // Store fastest info (in meters / second)
                    if (fastestSpeed < currentSpeed)
                    {
                        fastestSpeed = currentSpeed;
                        recordStart = startIndex;
                        recordEnd = endIndex;
                    }

                    // Remove first point from routeDistance, and go to next starting point.
                    currentDistance -= activity.GPSRoute[startIndex].Value.DistanceMetersToPoint(activity.GPSRoute[startIndex + 1].Value);
                }
            }

            // Activity does not contain GPS data or not long enough
            return null;
        }

        private static Record GetAllActivities(IActivity activity, RecordCategory category)
        {
            Record record = new Record(activity, category);
            return record;
        }

        /// <summary>
        /// Gets all Extreme Overall type records for a particular activity
        /// </summary>
        /// <param name="activities">Activity list to search in</param>
        /// <param name="category"></param>
        /// <returns></returns>
        private static List<Record> GetExtremeOverallRecordSet(IEnumerable<IActivity> activities, RecordCategory category)
        {
            List<Record> extremeRecords = new List<Record>();
            Record record;
            int i = 0;

            // Build a secondary all activity list to poll for all day records (multiple events on one day)
            // Using an ArrayList for ease of removing records
            ArrayList allAct = new ArrayList();
            foreach (IActivity activity in activities)
            {
                if (CheckCategory(activity, category))
                {
                    allAct.Add(activity);
                }
            }

            foreach (IActivity activity in activities)
            {
                if (CheckCategory(activity, category))
                {
                    switch (category.Type)
                    {
                        #region MaxTemperature
                        case RecordCategory.RecordType.MaxTemperature:
                            {
                                if (!float.IsNaN(activity.Weather.TemperatureCelsius) && activity.GPSRoute != null && activity.GPSRoute.Count > 0)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = Temperature.Convert(activity.Weather.TemperatureCelsius, Temperature.Units.Celsius, PluginMain.GetApplication().SystemPreferences.TemperatureUnits).ToString("N1") + Units.Temp;
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].Activity.Weather.TemperatureCelsius > activity.Weather.TemperatureCelsius)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region MinTemperature
                        case RecordCategory.RecordType.MinTemperature:
                            {
                                if (!float.IsNaN(activity.Weather.TemperatureCelsius) && activity.GPSRoute != null && activity.GPSRoute.Count > 0)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = Temperature.Convert(activity.Weather.TemperatureCelsius, Temperature.Units.Celsius, PluginMain.GetApplication().SystemPreferences.TemperatureUnits).ToString("N1") + Units.Temp;
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].Activity.Weather.TemperatureCelsius < activity.Weather.TemperatureCelsius)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region MaxDistance
                        case RecordCategory.RecordType.MaxDistance:
                            {
                                if (!float.IsNaN(activity.TotalDistanceMetersEntered) && activity.TotalDistanceMetersEntered != 0)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.ActualDistance + Units.Distance;
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].ActualDistance > record.ActualDistance)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region MaxTime
                        case RecordCategory.RecordType.TotalTime:
                            {
                                record = new Record(activity, category);
                                record.RecValue = record.TotalTime.ToString();
                                while (i < extremeRecords.Count)
                                {
                                    if (extremeRecords[i].TotalTime > record.TotalTime)
                                    {
                                        i++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                extremeRecords.Insert(i, record);
                                i = 0;
                                break;
                            }

                        #endregion
                        #region MaxSpeed
                        case RecordCategory.RecordType.MaxSpeed:
                            {
                                record = new Record(activity, category);
                                if (!float.IsNaN(record.MaxSpeed) && !float.IsInfinity(record.MaxSpeed))
                                {
                                    record.RecValue = record.MaxSpeed.ToString("N2") + Units.Speed;
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].MaxSpeed > record.MaxSpeed)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region MaxHR
                        case RecordCategory.RecordType.MaxHR:
                            {
                                if (activity.HeartRatePerMinuteTrack != null)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.MaxHR.ToString("N0") + " bpm";
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].MaxHR > record.MaxHR)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region FastestSpeed
                        case RecordCategory.RecordType.FastestSpeed:
                            {
                                record = new Record(activity, category);
                                if (record.Speed != 0)
                                {
                                    record.RecValue = record.Speed.ToString("N2") + Units.Speed;
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].Speed > record.Speed)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region FastestPace
                        case RecordCategory.RecordType.FastestPace:
                            {
                                record = new Record(activity, category);
                                if (record.AvgPace != TimeSpan.Parse("00:00:00"))
                                {
                                    record.RecValue = record.AvgPace.ToString() + Units.Pace;
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].AvgPace < record.AvgPace)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region AvgHR
                        case RecordCategory.RecordType.AvgHR:
                            {
                                if (activity.HeartRatePerMinuteTrack != null)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.AvgHR.ToString("N0") + " bpm";
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].AvgHR > record.AvgHR)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region MaxElevation
                        case RecordCategory.RecordType.MaxElevation:
                            {
                                if (activity.GPSRoute != null && activity.GPSRoute.Count > 0)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.MaxElevation.ToString("N0") + Units.Elevation;

                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].MaxElevation > record.MaxElevation)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region MinElevation
                        case RecordCategory.RecordType.MinElevation:
                            {
                                if (activity.GPSRoute != null && activity.GPSRoute.Count > 0)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.MinElevation.ToString("N0") + Units.Elevation;
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].MinElevation < record.MinElevation)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region MaxCadence
                        case RecordCategory.RecordType.MaxCadence:
                            {
                                if (activity.CadencePerMinuteTrack != null)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.MaxCadence.ToString("N0") + " rpm";
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].MaxCadence > record.MaxCadence)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region ElevationDifference
                        case RecordCategory.RecordType.ElevationDifference:
                            {
                                if (activity.GPSRoute != null && activity.GPSRoute.Count > 0)
                                {
                                    record = new Record(activity, category);
                                    double minE, maxE;
                                    maxE = record.MaxElevation;
                                    minE = record.MinElevation;

                                    record.RecValue = (maxE - minE).ToString("N0") + Units.Elevation;
                                    while (i < extremeRecords.Count)
                                    {
                                        if ((extremeRecords[i].MaxElevation - extremeRecords[i].MinElevation) > (record.MaxElevation - record.MinElevation))
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region MaxPower
                        case RecordCategory.RecordType.MaxPower:
                            {
                                if (activity.PowerWattsTrack != null)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.MaxPower.ToString("N0") + Units.Power;
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].MaxPower > record.MaxPower)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region AvgPower
                        case RecordCategory.RecordType.AvgPower:
                            {
                                if (activity.PowerWattsTrack != null || activity.AveragePowerWattsEntered != 0)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.AvgPower.ToString("N0") + Units.Power;
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].AvgPower > record.AvgPower)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region MaxAscent
                        case RecordCategory.RecordType.MaxAscent:
                            {
                                if (activity.GPSRoute != null && activity.GPSRoute.Count > 0)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.TotalAscend.ToString("N0") + Units.Elevation;

                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].TotalAscend > record.TotalAscend)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region MaxDescent
                        case RecordCategory.RecordType.MaxDescent:
                            {
                                if (activity.GPSRoute != null && activity.GPSRoute.Count > 0)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.TotalDescend.ToString("N0") + Units.Elevation;

                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].TotalDescend < record.TotalDescend)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region TotalCalories
                        case RecordCategory.RecordType.TotalCalories:
                            {
                                if (activity.TotalCalories != 0)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.TotalCalories.ToString("N0");

                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].TotalCalories > record.TotalCalories)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region AvgCadence
                        case RecordCategory.RecordType.AvgCadence:
                            {
                                if (activity.CadencePerMinuteTrack != null)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.AvgCadence.ToString("N0") + " rpm";
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].AvgCadence > record.AvgCadence)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region TotalTimeOneDay
                        case RecordCategory.RecordType.TotalTimeOneDay:
                            {
                                TimeSpan totTime = new TimeSpan();
                                record = new Record(activity, category);
                                for (int c = 0; c < allAct.Count; c++)
                                {
                                    IActivity act2 = (IActivity)allAct[c];
                                    Record rec2 = new Record(act2, category);
                                    if (act2.StartTime.ToLocalTime().Date == record.StartDateTime.ToLocalTime().Date)
                                    {
                                        totTime += rec2.TotalTime;
                                        allAct.Remove(act2);
                                        c--;
                                    }
                                }

                                if (totTime != TimeSpan.Parse("00:00:00"))
                                {
                                    while (i < extremeRecords.Count)
                                    {
                                        if (TimeSpan.Parse(extremeRecords[i].RecValue) > totTime)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    record.RecValue = totTime.ToString();
                                    DateTime newDt = new DateTime(record.StartDateTime.Year, record.StartDateTime.Month, record.StartDateTime.Day);
                                    record.StartDateTime = newDt;

                                    // TODO: Determine how a record is getting a minDate as it's startdate
                                    if (newDt.Year == 1)
                                    {
                                    }
                                    extremeRecords.Insert(i, record);
                                }

                                i = 0;
                                break;
                            }

                        #endregion
                        #region TotalDistanceOneDay
                        case RecordCategory.RecordType.TotalDistanceOneDay:
                            {
                                double totDist = 0;
                                record = new Record(activity, category);
                                for (int c = 0; c < allAct.Count; c++)
                                {
                                    IActivity act2 = (IActivity)allAct[c];
                                    Record rec2 = new Record(act2, category);
                                    if (act2.StartTime.ToLocalTime().Date == record.StartDateTime.ToLocalTime().Date)
                                    {
                                        totDist += rec2.ActualDistance;
                                        allAct.Remove(act2);
                                        c--;
                                    }
                                }

                                if (totDist != 0)
                                {
                                    while (i < extremeRecords.Count)
                                    {
                                        if (Convert.ToDouble(extremeRecords[i].RecValue) > totDist)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    record.RecValue = totDist.ToString("N2");
                                    DateTime newDt = new DateTime(record.StartDateTime.Year, record.StartDateTime.Month, record.StartDateTime.Day);
                                    record.StartDateTime = newDt;

                                    // TODO: Determine how a record is getting a minDate as it's startdate
                                    if (newDt.Year == 1)
                                    {
                                    }
                                    extremeRecords.Insert(i, record);
                                }

                                i = 0;
                                break;
                            }

                        #endregion
                        #region MaxGrade
                        case RecordCategory.RecordType.MaxGrade:
                            {
                                if (activity.GPSRoute != null && activity.GPSRoute.Count > 0)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.MaxGrade.ToString("N1") + " %";

                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].MaxGrade > record.MaxGrade)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                        #region MinGrade
                        case RecordCategory.RecordType.MinGrade:
                            {
                                if (activity.GPSRoute != null && activity.GPSRoute.Count > 0)
                                {
                                    record = new Record(activity, category);
                                    record.RecValue = record.MinGrade.ToString("N1") + " %";
                                    while (i < extremeRecords.Count)
                                    {
                                        if (extremeRecords[i].MinGrade < record.MinGrade)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    extremeRecords.Insert(i, record);
                                    i = 0;
                                }

                                break;
                            }

                        #endregion
                    }
                }
            }

            for (i = 0; i < extremeRecords.Count; i++)
            {
                extremeRecords[i].Rank = i + 1;
            }

            return extremeRecords;
        }

        #endregion
    }
}
