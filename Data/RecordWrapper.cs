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

namespace CourseScore.Data
{
    class RecordWrapper : TreeList.TreeListNode
    {
        #region Constructor

        public RecordWrapper(RecordSetWrapper parent, Record element)
            : base(parent, element)
        {
        }

        #endregion

        public string Name
        {
            get { return ((Record)Element).Name; }
        }

        public string Rank
        {
            get { return ((Record)Element).Rank.ToString(); }
        }

        public string ActualDistance
        {
            get { return ((Record)Element).ActualDistance.ToString("0.00", CultureInfo.CurrentCulture); }
        }

        public string AvgHR
        {
            get { return ((Record)Element).AvgHR.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string AvgPace
        {
            get { return ((Record)Element).AvgPace.ToString(); }
        }

        public string Display_AvgPace
        {
            get { return TimeSpan_RemoveLeadingZeros(((Record)Element).AvgPace); }
        }

        public string CategoryName
        {
            get { return ((Record)Element).CategoryName; }
        }

        public string ElevationChange
        {
            get { return ((Record)Element).ElevationChange.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string EndDateTime
        {
            get { return ((Record)Element).EndDateTime.ToString(); }
        }

        public string EndDistance
        {
            get { return ((Record)Element).EndDistance.ToString("0.00", CultureInfo.CurrentCulture); }
        }

        public string Location
        {
            get { return ((Record)Element).Location; }
        }

        public string MaxHR
        {
            get { return ((Record)Element).MaxHR.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string Speed
        {
            get { return ((Record)Element).Speed.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string StartDateTime
        {
            get
            {
                return ((Record)Element).StartDateTime.ToString(CultureInfo.CurrentCulture);
            }
        }

        public string StartDistance
        {
            get { return ((Record)Element).StartDistance.ToString("0.00", CultureInfo.CurrentCulture); }
        }

        public string Temp
        {
            get { return ((Record)Element).Temp.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string TotalAscend
        {
            get { return ((Record)Element).TotalAscend.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string TotalDescend
        {
            get { return ((Record)Element).TotalDescend.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string RecValue
        {
            get { return ((Record)Element).RecValue; }
        }

        public string TotalTime
        {
            get { return ((Record)Element).TotalTime.ToString(); }
        }

        public string Display_TotalTime
        {
            get { return TimeSpan_RemoveLeadingZeros(((Record)Element).TotalTime); }
        }

        public string MaxSpeed
        {
            get { return ((Record)Element).MaxSpeed.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string MaxElevation
        {
            get { return ((Record)Element).MaxElevation.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string MinElevation
        {
            get { return ((Record)Element).MinElevation.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string MaxCadence
        {
            get { return ((Record)Element).MaxCadence.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string AvgCadence
        {
            get { return ((Record)Element).AvgCadence.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string MaxPower
        {
            get { return ((Record)Element).MaxPower.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string AvgPower
        {
            get { return ((Record)Element).AvgPower.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string MaxGrade
        {
            get { return ((Record)Element).MaxGrade.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string MinGrade
        {
            get { return ((Record)Element).MinGrade.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string TotalCalories
        {
            get { return ((Record)Element).TotalCalories.ToString(); }
        }

        public string ActivityCategory
        {
            get { return ((Record)Element).ActivityCategory; }
        }

        public Speed.Units SpeedUnits
        {
            get { return ((Record)Element).SpeedUnits; }
        }

        public Record Record
        {
            get { return (Record)Element; }
        }

        public string ReferenceId
        {
            get { return ((Record)Element).ReferenceId; }
        }

        public static string TimeSpan_RemoveLeadingZeros(TimeSpan span)
        {
            string displayTime = string.Empty;
            if (span.Hours > 0)
            {
                // Hours & minutes
                displayTime = span.Hours.ToString("#0") + ":" +
                              span.Minutes.ToString("00") + ":";
            }
            else if (span.Minutes < 10)
            {
                // Single digit minutes
                displayTime = span.Minutes.ToString("#0") + ":";
            }
            else
            {
                // Double digit minutes
                displayTime = span.Minutes.ToString("00") + ":";
            }

            displayTime = displayTime +
                          span.Seconds.ToString("00");
            return displayTime;
        }

        #region IComparable Members

        // Default Sort(): Start date (newest first)
        public int CompareTo(object obj)
        {
            RecordWrapper a = (RecordWrapper)obj;
            return DateTime.Compare(Convert.ToDateTime(a.StartDateTime, CultureInfo.InvariantCulture), Convert.ToDateTime(this.StartDateTime, CultureInfo.InvariantCulture));
        }

        // Column specific sort
        public int CompareTo(RecordWrapper a2, RecordComparer.ComparisonType comparisonMethod, RecordComparer.Order sortOrder)
        {
            int result = 0;

            switch (comparisonMethod)
            {
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

                case RecordComparer.ComparisonType.AvgSpeed:
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

        public class RecordComparer : IComparer<RecordWrapper>
        {
            #region Fields

            private ComparisonType comparisonType;
            private Order sortOrder;

            #endregion

            #region Enumerations

            public enum ComparisonType
            {
                // TODO: Not all of these currently exist.  Remove unwanted.
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
                Calories = 18,
                AvgCadence = 19,
                AvgGrade = 20,
                Day = 21,
                FastestPace = 22,
                FastestSpeed = 23,
                MaxHR = 24,
                MaxPower = 25,
                Name = 26,
                StartDistance = 27,
                EndDistance = 28,
                EndDateTime = 29
            }

            #endregion

            #region Properties

            public enum Order
            {
                Ascending = 1,
                Descending = 2
            }

            public ComparisonType ComparisonMethod
            {
                get { return comparisonType; }
                set { comparisonType = value; }
            }

            public Order SortOrder
            {
                get { return sortOrder; }
                set { sortOrder = value; }
            }

            #endregion

            #region IComparer<Record> Members

            public int Compare(RecordWrapper x, RecordWrapper y)
            {
                return x.CompareTo(y, comparisonType, sortOrder);
            }

            #endregion
        }
    }

}
