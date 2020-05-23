using System;
using System.Collections.Generic;
using System.Globalization;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Visuals;

namespace CourseScore.Data
{
    /// <summary>
    /// DisplayRecord is a class used to display a record's data in a row format instead of a column format
    /// </summary>
    class DisplayRecord : TreeList.TreeListNode
    {
        #region Fields

        private string name;
        private string recValue;
        private string rank;
        private RecordCategory.RecordType type;
        private List<string> categories;

        #endregion

        #region Enumerations

        //public enum recordType
        //{
        //    Distance = 1,
        //    Time = 2,
        //    SpeedPace = 3,
        //    AvgHR = 4,
        //    MaxHR = 5,
        //    ElevationDifference = 6,
        //    AscentDescent = 7,
        //    HottestColdest = 8,
        //    MaxSpeed = 9,
        //    MaxMinElevation = 10,
        //    AvgCadence = 11,
        //    MaxCadence = 12,
        //    AvgPower = 13,
        //    MaxPower = 14,
        //    Calories = 15,
        //    DistanceRecord = 16,
        //    Grade = 17
        //}

        public enum recordTime
        {
            Now,
            Then
        }

        #endregion

        #region Constructors

        public DisplayRecord(RecordWrapper parent, string element)
            : base(parent, element)
        {
        }

        /// <summary>
        /// Constructor to build the display record and rank it 'now' and 'then'
        /// </summary>
        /// <param name="parent">The current record</param>
        /// <param name="rt">What type of record this is</param>
        /// <param name="recList">The list of all the records to compare against</param>
        /// <param name="recT">Is it a 'now' or 'then' record</param>
        public DisplayRecord(RecordWrapper parent, RecordCategory.RecordType recType, List<Record> recList, recordTime recTime, string element, List<string> refIDs, string displayName)
            : base(parent, element)
        {
            int j, total;
            double max, min;

            switch (recType)
            {
                #region Distance

                case RecordCategory.RecordType.Distance:

                    // Set the display name and record value
                    name = displayName;
                    recValue = parent.ActualDistance;

                    int iRank = 1;

                    // If the distance is 0, don't rank it
                    if (recValue == "0.00")
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        // Parse through the record list and find the 'now' or 'then' ranking of the current record
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // 'Then' record.  If the current record occurred before the one in the list
                                if (Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) > recList[i].StartDateTime
                                    && recList[i].ActualDistance != 0
                                    && recTime == recordTime.Then)
                                {
                                    iRank++;
                                    if (Convert.ToDouble(parent.ActualDistance, CultureInfo.CurrentCulture) > recList[i].ActualDistance)
                                    {
                                        iRank--;
                                    }
                                }

                                // 'Now' record.
                                else if (recTime == recordTime.Now
                                        && recList[i].ActualDistance != 0
                                        && Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) != recList[i].StartDateTime)
                                {
                                    iRank++;
                                    if (Convert.ToDouble(parent.ActualDistance, CultureInfo.CurrentCulture) > recList[i].ActualDistance)
                                    {
                                        iRank--;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = iRank.ToString();
                        recValue += " " + Units.Distance;
                    }

                    break;

                #endregion
                #region Total Time
                case RecordCategory.RecordType.TotalTime:
                    // Set the display name and record value
                    name = "Total Time";
                    recValue = parent.TotalTime;
                    j = 0;
                    total = 1;
                    for (int i = 0; i < recList.Count; i++)
                    {
                        if (refIDs.Contains(recList[i].ReferenceId))
                        {
                            // Parse through the record list and find the 'now' or 'then' ranking of the current record
                            if (Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) > recList[i].StartDateTime
                                && recTime == recordTime.Then)
                            {
                                total++;
                                if (TimeSpan.Parse(recValue) > recList[i].TotalTime)
                                {
                                    j++;
                                }
                            }
                            else if (recTime == recordTime.Now
                                     && Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) != recList[i].StartDateTime)
                            {
                                // 'Now' record.
                                total++;
                                if (TimeSpan.Parse(recValue) > recList[i].TotalTime)
                                {
                                    j++;
                                }
                            }
                        }
                    }

                    // Set the rank
                    rank = (total - j).ToString();
                    break;

                #endregion
                #region SpeedPace
                case RecordCategory.RecordType.SpeedPace:

                    // Set the display name and record value
                    name = "Speed / Pace";
                    recValue = Convert.ToDouble(parent.Speed, CultureInfo.CurrentCulture).ToString("N2") + " " + Units.Speed + " / " + parent.AvgPace + " " + Units.Pace;
                    j = 0;
                    total = 1;

                    // If the speed is 0, don't rank it
                    if (parent.Speed == "0" && parent.AvgPace == "00:00:00")
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                if (Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) > recList[i].StartDateTime
                                    && recList[i].Speed != 0 && parent.AvgPace != "00:00:00"
                                    && recTime == recordTime.Then)
                                {
                                    total++;
                                    if (Convert.ToDouble(parent.Speed, CultureInfo.CurrentCulture) > recList[i].Speed)
                                    {
                                        j++;
                                    }
                                }

                                // 'Now' record.
                                else if (recTime == recordTime.Now
                                        && recList[i].Speed != 0 && parent.AvgPace != "00:00:00"
                                        && Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) != recList[i].StartDateTime)
                                {
                                    total++;
                                    if (Convert.ToDouble(parent.Speed, CultureInfo.CurrentCulture) > recList[i].Speed)
                                    {
                                        j++;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = (total - j).ToString();
                    }

                    break;

                #endregion
                #region AvgHR
                case RecordCategory.RecordType.AvgHR:

                    // Set the display name and record value
                    name = "Avg HR";
                    recValue = parent.AvgHR;
                    j = 0;
                    total = 1;

                    // If the average HR is 0, don't rank it
                    if (parent.AvgHR == "0")
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                if (Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) > recList[i].StartDateTime
                                    && recList[i].AvgHR != 0
                                    && recTime == recordTime.Then)
                                {
                                    total++;
                                    if (Convert.ToDouble(recValue, CultureInfo.CurrentCulture) > recList[i].AvgHR)
                                    {
                                        j++;
                                    }
                                }

                                // 'Now' record.
                                else if (recTime == recordTime.Now
                                        && recList[i].AvgHR != 0
                                        && Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) != recList[i].StartDateTime)
                                {
                                    total++;
                                    if (Convert.ToDouble(recValue, CultureInfo.CurrentCulture) > recList[i].AvgHR)
                                    {
                                        j++;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = (total - j).ToString();
                        recValue += " bpm";
                    }

                    break;

                #endregion
                #region MaxHR
                case RecordCategory.RecordType.MaxHR:

                    // Set the display name and record value
                    name = "Max HR";
                    recValue = parent.MaxHR;
                    j = 0;
                    total = 1;

                    // If the max HR is 0, don't rank it
                    if (parent.MaxHR == "0")
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                if (Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) > recList[i].StartDateTime
                                    && recList[i].MaxHR != 0
                                    && recTime == recordTime.Then)
                                {
                                    total++;
                                    if (Convert.ToDouble(recValue, CultureInfo.CurrentCulture) > recList[i].MaxHR)
                                    {
                                        j++;
                                    }
                                }

                                // 'Now' record.
                                else if (recTime == recordTime.Now
                                        && recList[i].MaxHR != 0
                                        && Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) != recList[i].StartDateTime)
                                {
                                    total++;
                                    if (Convert.ToDouble(recValue, CultureInfo.CurrentCulture) > recList[i].MaxHR)
                                    {
                                        j++;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = (total - j).ToString();
                        recValue += " bpm";
                    }

                    break;

                #endregion
                #region ElevationDifference
                case RecordCategory.RecordType.ElevationDifference:

                    // Set the display name and record value
                    name = "Elevation Difference";
                    string maxE, minE;
                    maxE = parent.MaxElevation;
                    minE = parent.MinElevation;
                    recValue = (Convert.ToDouble(maxE, CultureInfo.CurrentCulture) - Convert.ToDouble(minE, CultureInfo.CurrentCulture)).ToString("N0");
                    j = 0;
                    total = 1;

                    // If the elevation is 0, don't rank it
                    if (parent.MaxElevation == "0" && parent.MinElevation == "0")
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                if (Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) > recList[i].StartDateTime
                                    && recList[i].MaxElevation != 0 && recList[i].MinElevation != 0
                                    && recTime == recordTime.Then)
                                {
                                    total++;
                                    float recMaxE, recMinE;
                                    recMaxE = recList[i].MaxElevation;
                                    recMinE = recList[i].MinElevation;
                                    if ((Convert.ToDouble(maxE, CultureInfo.CurrentCulture) - Convert.ToDouble(minE, CultureInfo.CurrentCulture)) > (recMaxE - recMinE))
                                    {
                                        j++;
                                    }
                                }

                                // 'Now' record.
                                else if (recTime == recordTime.Now
                                        && recList[i].MaxElevation != 0 && recList[i].MinElevation != 0
                                        && Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) != recList[i].StartDateTime)
                                {
                                    total++;
                                    float recMaxE, recMinE;
                                    recMaxE = recList[i].MaxElevation;
                                    recMinE = recList[i].MinElevation;
                                    if ((Convert.ToDouble(maxE, CultureInfo.CurrentCulture) - Convert.ToDouble(minE, CultureInfo.CurrentCulture)) > (recMaxE - recMinE))
                                    {
                                        j++;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = (total - j).ToString();
                        recValue += " " + Units.Elevation;
                    }

                    break;

                #endregion
                #region AscentDescent
                case RecordCategory.RecordType.AscentDescent:

                    // Set the display name and record value
                    name = "Ascent / Descent";
                    recValue = parent.TotalAscend + " / " + parent.TotalDescend;
                    int ascend = 0, descend = 0;
                    total = 1;

                    // If the ascend or descend is NaN
                    if (double.IsNaN(Convert.ToDouble(parent.TotalAscend, CultureInfo.CurrentCulture))
                        || double.IsNaN(Convert.ToDouble(parent.TotalDescend, CultureInfo.CurrentCulture))
                        || Convert.ToDouble(parent.TotalAscend, CultureInfo.CurrentCulture) == 0
                        || Convert.ToDouble(parent.TotalDescend, CultureInfo.CurrentCulture) == 0)
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                if (Convert.ToDateTime(parent.StartDateTime) > recList[i].StartDateTime
                                    && (!double.IsNaN(Convert.ToDouble(parent.TotalAscend, CultureInfo.CurrentCulture))
                                    || !double.IsNaN(Convert.ToDouble(recList[i].TotalDescend, CultureInfo.CurrentCulture))
                                    || Convert.ToDouble(recList[i].TotalAscend, CultureInfo.CurrentCulture) == 0
                                    || Convert.ToDouble(recList[i].TotalDescend, CultureInfo.CurrentCulture) == 0)
                                    && recTime == recordTime.Then)
                                {
                                    total++;
                                    if (Convert.ToDouble(parent.TotalAscend, CultureInfo.CurrentCulture) > recList[i].TotalAscend)
                                    {
                                        ascend++;
                                    }

                                    if (Convert.ToDouble(parent.TotalDescend, CultureInfo.CurrentCulture) < recList[i].TotalDescend)
                                    {
                                        descend++;
                                    }
                                }

                                // 'Now' record.
                                else if (recTime == recordTime.Now
                                    && (!double.IsNaN(Convert.ToDouble(parent.TotalAscend, CultureInfo.CurrentCulture))
                                    || !double.IsNaN(Convert.ToDouble(recList[i].TotalDescend, CultureInfo.CurrentCulture))
                                    || Convert.ToDouble(recList[i].TotalAscend, CultureInfo.CurrentCulture) == 0
                                    || Convert.ToDouble(recList[i].TotalDescend, CultureInfo.CurrentCulture) == 0)
                                    && Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) != recList[i].StartDateTime)
                                {
                                    total++;
                                    if (Convert.ToDouble(parent.TotalAscend, CultureInfo.CurrentCulture) > recList[i].TotalAscend)
                                    {
                                        ascend++;
                                    }

                                    if (Convert.ToDouble(parent.TotalDescend, CultureInfo.CurrentCulture) < recList[i].TotalDescend)
                                    {
                                        descend++;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = (total - ascend).ToString("N0") + " / " + (total - descend).ToString("N0");
                        recValue += " " + Units.Elevation;
                    }

                    break;

                #endregion
                #region HottestColdest
                case RecordCategory.RecordType.HottestColdest:

                    // Set the display name and record value
                    name = "Hottest / Coldest";
                    recValue = parent.Temp;
                    int hot = 0, cold = 0;
                    total = 1;

                    // If the average HR is 0, don't rank it
                    if (double.IsNaN(Convert.ToDouble(parent.Temp, CultureInfo.CurrentCulture)))
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                if (Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) > recList[i].StartDateTime
                                    && !double.IsNaN(recList[i].Temp)
                                    && recTime == recordTime.Then)
                                {
                                    total++;
                                    if (Convert.ToDouble(recValue, CultureInfo.CurrentCulture) > recList[i].Temp)
                                    {
                                        hot++;
                                    }

                                    if (Convert.ToDouble(recValue, CultureInfo.CurrentCulture) < recList[i].Temp)
                                    {
                                        cold++;
                                    }
                                }

                                // 'Now' record.
                                else if (recTime == recordTime.Now
                                        && !double.IsNaN(recList[i].Temp)
                                        && Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) != recList[i].StartDateTime)
                                {
                                    total++;
                                    if (Convert.ToDouble(recValue, CultureInfo.CurrentCulture) > recList[i].Temp)
                                    {
                                        hot++;
                                    }

                                    if (Convert.ToDouble(recValue, CultureInfo.CurrentCulture) < recList[i].Temp)
                                    {
                                        cold++;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = (total - hot).ToString("N0") + " / " + (total - cold).ToString("N0");
                        recValue += " " + Units.Temp;
                    }

                    break;

                #endregion
                #region MaxSpeed
                case RecordCategory.RecordType.MaxSpeed:

                    // Set the display name and record value
                    name = "Fastest Speed";
                    recValue = Convert.ToDouble(parent.MaxSpeed, CultureInfo.CurrentCulture).ToString("N2");
                    j = 0;
                    total = 1;

                    // If the max speed is 0, infinity, -infinity, NaN - don't rank it
                    if (parent.MaxSpeed == "0"
                        || Double.IsNaN(Convert.ToDouble(parent.MaxSpeed, CultureInfo.CurrentCulture))
                        || Double.IsInfinity(Convert.ToDouble(parent.MaxSpeed, CultureInfo.CurrentCulture)))
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                if (Convert.ToDateTime(parent.StartDateTime, CultureInfo.CurrentCulture) > recList[i].StartDateTime
                                    && recList[i].MaxSpeed != 0
                                    && !Double.IsNaN(Convert.ToDouble(recList[i].MaxSpeed, CultureInfo.CurrentCulture))
                                    && !Double.IsInfinity(Convert.ToDouble(recList[i].MaxSpeed, CultureInfo.CurrentCulture))
                                    && recTime == recordTime.Then)
                                {
                                    total++;
                                    if (Convert.ToDouble(recValue, CultureInfo.CurrentCulture) > recList[i].MaxSpeed)
                                    {
                                        j++;
                                    }
                                }
                                else if (recTime == recordTime.Now
                                        && recList[i].MaxSpeed != 0
                                        && !Double.IsNaN(Convert.ToDouble(recList[i].MaxSpeed, CultureInfo.CurrentCulture))
                                        && !Double.IsInfinity(Convert.ToDouble(recList[i].MaxSpeed, CultureInfo.CurrentCulture))
                                        && Convert.ToDateTime(parent.StartDateTime) != recList[i].StartDateTime)
                                {
                                    // 'Now' record.
                                    total++;
                                    if (Convert.ToDouble(recValue, CultureInfo.CurrentCulture) > recList[i].MaxSpeed)
                                    {
                                        j++;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = (total - j).ToString();
                        recValue += " " + Units.Speed;
                    }

                    break;

                #endregion
                #region MaxMinElevation
                case RecordCategory.RecordType.MaxMinElevation:

                    // Set the display name and record value
                    name = "Max / Min Elevation";
                    max = 0;
                    min = 0;
                    max = Convert.ToDouble(parent.MaxElevation);
                    min = Convert.ToDouble(parent.MinElevation);
                    recValue = max.ToString("N0") + " / " + min.ToString("N0");
                    int high = 0, low = 0;
                    total = 1;

                    // If the ascend or descend is NaN
                    if (double.IsNaN(Convert.ToDouble(parent.MaxElevation))
                        || double.IsNaN(Convert.ToDouble(parent.MinElevation))
                        || Convert.ToDouble(parent.MaxElevation) == 0
                        || Convert.ToDouble(parent.MinElevation) == 0)
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                if (Convert.ToDateTime(parent.StartDateTime) > recList[i].StartDateTime
                                    && (!double.IsNaN(Convert.ToDouble(recList[i].MaxElevation)) || !double.IsNaN(Convert.ToDouble(recList[i].MinElevation))
                                    || Convert.ToDouble(recList[i].MaxElevation) != 0 || Convert.ToDouble(recList[i].MinElevation) != 0)
                                    && recTime == recordTime.Then)
                                {
                                    total++;
                                    if (Convert.ToDouble(parent.MaxElevation) > recList[i].MaxElevation)
                                    {
                                        high++;
                                    }

                                    if (Convert.ToDouble(parent.MinElevation) < recList[i].MinElevation)
                                    {
                                        low++;
                                    }
                                }
                                else if (recTime == recordTime.Now
                                        && (!double.IsNaN(Convert.ToDouble(parent.MaxElevation)) || !double.IsNaN(Convert.ToDouble(recList[i].MinElevation))
                                        || Convert.ToDouble(recList[i].MaxElevation) != 0 || Convert.ToDouble(recList[i].MinElevation) != 0)
                                        && Convert.ToDateTime(parent.StartDateTime) != recList[i].StartDateTime)
                                {
                                    // 'Now' record.
                                    total++;
                                    if (Convert.ToDouble(parent.MaxElevation) > recList[i].MaxElevation)
                                    {
                                        high++;
                                    }

                                    if (Convert.ToDouble(parent.MinElevation) < recList[i].MinElevation)
                                    {
                                        low++;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = (total - high).ToString("N0") + " / " + (total - low).ToString("N0");
                        recValue += " " + Units.Elevation;
                    }

                    break;

                #endregion
                #region AvgCadence
                case RecordCategory.RecordType.AvgCadence:
                    // Set the display name and record value
                    name = "Average Cadence";
                    recValue = Convert.ToDouble(parent.AvgCadence).ToString("N0");
                    j = 0;
                    total = 1;

                    // If the average cadence is 0, don't rank it
                    if (parent.AvgCadence == "0")
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                if (Convert.ToDateTime(parent.StartDateTime) > recList[i].StartDateTime
                                    && recList[i].AvgCadence != 0
                                    && recTime == recordTime.Then)
                                {
                                    total++;
                                    if (Convert.ToDouble(recValue) > recList[i].AvgCadence)
                                    {
                                        j++;
                                    }
                                }
                                else if (recTime == recordTime.Now
                                        && recList[i].AvgCadence != 0
                                        && Convert.ToDateTime(parent.StartDateTime) != recList[i].StartDateTime)
                                {
                                    // 'Now' record.
                                    total++;
                                    if (Convert.ToDouble(recValue) > recList[i].AvgCadence)
                                    {
                                        j++;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = (total - j).ToString();
                        recValue += " rpm";
                    }

                    break;

                #endregion
                #region MaxCadence
                case RecordCategory.RecordType.MaxCadence:

                    // Set the display name and record value
                    name = "Max Cadence";
                    recValue = Convert.ToDouble(parent.MaxCadence).ToString("N0");
                    j = 0;
                    total = 1;

                    // If the max cadence is 0, don't rank it
                    if (parent.MaxCadence == "0")
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                if (Convert.ToDateTime(parent.StartDateTime) > recList[i].StartDateTime
                                    && recList[i].MaxCadence != 0
                                    && recTime == recordTime.Then)
                                {
                                    total++;
                                    if (Convert.ToDouble(recValue) > recList[i].MaxCadence)
                                    {
                                        j++;
                                    }
                                }

                                // 'Now' record.
                                else if (recTime == recordTime.Now
                                        && recList[i].MaxCadence != 0
                                        && Convert.ToDateTime(parent.StartDateTime) != recList[i].StartDateTime)
                                {
                                    total++;
                                    if (Convert.ToDouble(recValue) > recList[i].MaxCadence)
                                    {
                                        j++;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = (total - j).ToString();
                        recValue += " rpm";
                    }

                    break;

                case RecordCategory.RecordType.MaxPower:
                    {
                        // Set the display name and record value
                        name = "Max Power";
                        recValue = Convert.ToDouble(parent.MaxPower).ToString("N0");
                        j = 0;
                        total = 1;

                        // If the max Power is 0, don't rank it
                        if (parent.MaxPower == "0")
                        {
                            rank = "-";
                            recValue = "-";
                        }
                        else
                        {
                            for (int i = 0; i < recList.Count; i++)
                            {
                                if (refIDs.Contains(recList[i].ReferenceId))
                                {
                                    // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                    if (Convert.ToDateTime(parent.StartDateTime) > recList[i].StartDateTime
                                        && recList[i].MaxPower != 0
                                        && recTime == recordTime.Then)
                                    {
                                        total++;
                                        if (Convert.ToDouble(recValue) > recList[i].MaxPower)
                                        {
                                            j++;
                                        }
                                    }
                                    else if (recTime == recordTime.Now
                                            && recList[i].MaxPower != 0
                                            && Convert.ToDateTime(parent.StartDateTime) != recList[i].StartDateTime)
                                    {
                                        // 'Now' record.
                                        total++;
                                        if (Convert.ToDouble(recValue) > recList[i].MaxPower)
                                        {
                                            j++;
                                        }
                                    }
                                }
                            }

                            // Set the rank
                            rank = (total - j).ToString();
                            recValue += " " + Units.Power;
                        }

                        break;
                    }
                #endregion
                #region AvgPower
                case RecordCategory.RecordType.AvgPower:
                    {
                        // Set the display name and record value
                        name = "Avg Power";
                        recValue = Convert.ToDouble(parent.AvgPower).ToString("N0");
                        j = 0;
                        total = 1;

                        // If the average HR is 0, don't rank it
                        if (parent.AvgPower == "0")
                        {
                            rank = "-";
                            recValue = "-";
                        }
                        else
                        {
                            for (int i = 0; i < recList.Count; i++)
                            {
                                if (refIDs.Contains(recList[i].ReferenceId))
                                {
                                    // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                    if (Convert.ToDateTime(parent.StartDateTime) > recList[i].StartDateTime
                                        && recList[i].AvgPower != 0
                                        && recTime == recordTime.Then)
                                    {
                                        total++;
                                        if (Convert.ToDouble(recValue) > recList[i].AvgPower)
                                        {
                                            j++;
                                        }
                                    }
                                    else if (recTime == recordTime.Now
                                            && recList[i].AvgPower != 0
                                            && Convert.ToDateTime(parent.StartDateTime) != recList[i].StartDateTime)
                                    {
                                        // 'Now' record.
                                        total++;
                                        if (Convert.ToDouble(recValue) > recList[i].AvgPower)
                                        {
                                            j++;
                                        }
                                    }
                                }
                            }

                            // Set the rank
                            rank = (total - j).ToString();
                            recValue += " " + Units.Power;
                        }

                        break;
                    }
                #endregion
                #region Calories
                case RecordCategory.RecordType.TotalCalories:
                    {
                        // Set the display name and record value
                        name = "Calories";
                        recValue = Convert.ToDouble(parent.TotalCalories).ToString("N0");
                        j = 0;
                        total = 1;

                        // If the total calories are 0, don't rank it
                        if (parent.TotalCalories == "0")
                        {
                            rank = "-";
                            recValue = "-";
                        }
                        else
                        {
                            for (int i = 0; i < recList.Count; i++)
                            {
                                if (refIDs.Contains(recList[i].ReferenceId))
                                {
                                    // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                    if (Convert.ToDateTime(parent.StartDateTime) > recList[i].StartDateTime
                                        && recList[i].TotalCalories != 0
                                        && recTime == recordTime.Then)
                                    {
                                        total++;
                                        if (Convert.ToDouble(recValue) > recList[i].TotalCalories)
                                        {
                                            j++;
                                        }
                                    }
                                    else if (recTime == recordTime.Now
                                            && recList[i].TotalCalories != 0
                                            && Convert.ToDateTime(parent.StartDateTime) != recList[i].StartDateTime)
                                    {
                                        // 'Now' record.
                                        total++;
                                        if (Convert.ToDouble(recValue) > recList[i].TotalCalories)
                                        {
                                            j++;
                                        }
                                    }
                                }
                            }

                            // Set the rank
                            rank = (total - j).ToString();
                        }

                        break;
                    }
                #endregion
                #region Grade
                case RecordCategory.RecordType.MaxMinGrade:

                    // Set the display name and record value
                    name = "Max / Min Grade";
                    recValue = parent.MaxGrade + " / " + parent.MinGrade;
                    max = 0;
                    min = 0;
                    total = 1;

                    // If the ascend or descend is NaN
                    if (double.IsNaN(Convert.ToDouble(parent.MaxGrade))
                        || double.IsNaN(Convert.ToDouble(parent.MinGrade))
                        || Convert.ToDouble(parent.MaxGrade) == 0
                        || Convert.ToDouble(parent.MinGrade) == 0)
                    {
                        rank = "-";
                        recValue = "-";
                    }
                    else
                    {
                        for (int i = 0; i < recList.Count; i++)
                        {
                            if (refIDs.Contains(recList[i].ReferenceId))
                            {
                                // Parse through the record list and find the 'now' or 'then' ranking of the current record
                                if (Convert.ToDateTime(parent.StartDateTime) > recList[i].StartDateTime
                                    && (!double.IsNaN(Convert.ToDouble(parent.MaxGrade))
                                    && !double.IsNaN(Convert.ToDouble(recList[i].MinGrade)))
                                    && recTime == recordTime.Then)
                                {
                                    total++;
                                    if (Convert.ToDouble(parent.MaxGrade) > recList[i].MaxGrade)
                                    {
                                        max++;
                                    }

                                    if (Convert.ToDouble(parent.MinGrade) < recList[i].MinGrade)
                                    {
                                        min++;
                                    }
                                }

                                // 'Now' record.
                                else if (recTime == recordTime.Now
                                    && (!double.IsNaN(Convert.ToDouble(parent.MaxGrade))
                                    && !double.IsNaN(Convert.ToDouble(recList[i].MinGrade)))
                                    && Convert.ToDateTime(parent.StartDateTime) != recList[i].StartDateTime)
                                {
                                    total++;
                                    if (Convert.ToDouble(parent.MaxGrade) > recList[i].MaxGrade)
                                    {
                                        max++;
                                    }

                                    if (Convert.ToDouble(parent.MinGrade) < recList[i].MinGrade)
                                    {
                                        min++;
                                    }
                                }
                            }
                        }

                        // Set the rank
                        rank = (total - max).ToString("N0") + " / " + (total - min).ToString("N0");
                    }

                    break;

                #endregion
            }
        }

        public DisplayRecord(RecordWrapper parent, recordTime recT, List<RecordSet> master_recordset, int listIndex, string element)
            : base(parent, element)
        {
            // Display prefs for distance, pace, speed units\
            string spdPaceUnits = string.Empty;
            switch (PluginMain.GetApplication().SystemPreferences.DistanceUnits)
            {
                case Length.Units.Meter:
                    if (parent.SpeedUnits == Speed.Units.Pace)
                    {
                        spdPaceUnits = " min/m";
                    }
                    else
                    {
                        spdPaceUnits = " m/hr";
                    }

                    break;

                case Length.Units.Kilometer:
                    if (parent.SpeedUnits == Speed.Units.Pace)
                    {
                        spdPaceUnits = " min/km";
                    }
                    else
                    {
                        spdPaceUnits = " km/hr";
                    }

                    break;

                case Length.Units.Mile:
                    if (parent.SpeedUnits == Speed.Units.Pace)
                    {
                        spdPaceUnits = " min/mi";
                    }
                    else
                    {
                        spdPaceUnits = " mph";
                    }

                    break;

                case Length.Units.Yard:
                    if (parent.SpeedUnits == Speed.Units.Pace)
                    {
                        spdPaceUnits = " min/yd";
                    }
                    else
                    {
                        spdPaceUnits = " yd/hr";
                    }

                    break;
            }

            name = master_recordset[listIndex].CategoryName;
            recValue = "-";
            rank = "-";
            if (recT == recordTime.Now)
            {
                for (int i = 0; i < master_recordset[listIndex].Records.Count; i++)
                {
                    if (Convert.ToDateTime(parent.StartDateTime) == master_recordset[listIndex].Records[i].TrueStartDate)
                    {
                        if (parent.SpeedUnits == Speed.Units.Pace)
                        {
                            recValue = "(" + RecordWrapper.TimeSpan_RemoveLeadingZeros(master_recordset[listIndex].Records[i].TotalTime)
                                        + ") - " + RecordWrapper.TimeSpan_RemoveLeadingZeros(master_recordset[listIndex].Records[i].AvgPace) + spdPaceUnits;
                        }
                        else
                        {
                            recValue = "(" + RecordWrapper.TimeSpan_RemoveLeadingZeros(master_recordset[listIndex].Records[i].TotalTime)
                                        + ") - " + master_recordset[listIndex].Records[i].Speed.ToString("N1") + spdPaceUnits;
                        }

                        rank = master_recordset[listIndex].Records[i].Rank.ToString();
                        break;
                    }
                }
            }
            else if (recT == recordTime.Then)
            {
                int recRank = -1;
                int recsBefore = 1;
                for (int i = 0; i < master_recordset[listIndex].Records.Count; i++)
                {
                    if (Convert.ToDateTime(parent.StartDateTime) == master_recordset[listIndex].Records[i].TrueStartDate)
                    {
                        if (parent.SpeedUnits == Speed.Units.Pace)
                        {
                            recValue = "(" + RecordWrapper.TimeSpan_RemoveLeadingZeros(master_recordset[listIndex].Records[i].TotalTime)
                                        + ") - " + RecordWrapper.TimeSpan_RemoveLeadingZeros(master_recordset[listIndex].Records[i].AvgPace) + spdPaceUnits;
                        }
                        else
                        {
                            recValue = "(" + RecordWrapper.TimeSpan_RemoveLeadingZeros(master_recordset[listIndex].Records[i].TotalTime)
                                        + ") - " + master_recordset[listIndex].Records[i].Speed.ToString("N1") + spdPaceUnits;
                        }

                        recRank = master_recordset[listIndex].Records[i].Rank;
                        break;
                    }
                    else if (Convert.ToDateTime(parent.StartDateTime) > master_recordset[listIndex].Records[i].TrueStartDate)
                    {
                        recsBefore++;
                    }
                }

                if (recRank != -1)
                {
                    rank = recsBefore.ToString();
                }
            }
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string RecValue
        {
            get { return recValue; }
        }

        public string Rank
        {
            get { return rank; }
        }

        /// <summary>
        /// The type of record stored as an enumeration
        /// </summary>
        public RecordCategory.RecordType Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Display_RecordType
        {
            get
            {
                return type.ToString();
            }
        }

        /// <summary>
        /// Categories will store an ArrayList of ST categories for which this record type applies
        /// </summary>
        public List<string> Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        public string Display_Categories
        {
            get
            {
                string display = string.Empty;
                foreach (IActivityCategory cat in PluginMain.GetApplication().Logbook.ActivityCategories)
                {
                    GetActivityCategories(cat, ref display);
                }

                if (display.Length > 2)
                {
                    display = display.Substring(2);
                }

                return display;
            }
        }

        #endregion

        public void GetActivityCategories(IActivityCategory cat, ref string display)
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
                    display = display + ", " + cat.Parent.Name + ": " + cat.Name;
                }
            }
        }
    }
}
