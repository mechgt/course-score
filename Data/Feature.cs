using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Drawing;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Chart;
using ZoneFiveSoftware.Common.Data.Measurement;
using CourseScore.UI;

namespace CourseScore.Data
{
    public class Feature : IEquatable<Feature>
    {
        #region Public Members

        public double startDistance;
        public double endDistance;
        public double distance;
        public double startElevation;
        public double endElevation;
        public double elevGain;
        public Record record;
        public IGPSPoint startPoint;
        public IGPSPoint endPoint;
        public string refId;
        public int hillNumber;
        public bool added;
        public double gradeImpact;
        public double hillScoreClimbByBike;
        public double hillScoreCycle2Max;
        public double hillScoreFiets;
        public double hillScoreCourseScoreRunning;
        public double hillScoreCourseScoreCycling;
        public double startPercentTime;
        public double endPercentTime;
        public double startPercentDistance;
        public double endPercentDistance;
        public double placementImpact;
        //public int start_I;
        //public int end_I;
        public double previousFeatureScore;
        public Color fillColor;
        public Color lineColor;
        public int lineWidth;
        public Color selectedColor;
        public string masterActivityID;
        public int routeWidth;
        public DateTime startTime;
        public DateTime endTime;
        public double vam;
        public double wKg;
        public TimeSpan stoppedTime;
        public string hillCategory;

        public double avgGrade
        {
            get { return (double)((endElevation - startElevation) / (endDistance - startDistance)); }
            set { avgGrade = value; }
        }

        public double totalDistanceMeters
        {
            get { return endDistance - startDistance; }
        }

        public enum feature_type
        {
            descent, ascent, split
        }

        public feature_type _feature_type;

        #endregion

        #region Display Properties

        public string Start
        {
            get { return Length.Convert(startDistance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits).ToString("0.00", CultureInfo.CurrentCulture); }
        }

        public string End
        {
            get { return Length.Convert(endDistance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits).ToString("0.00", CultureInfo.CurrentCulture); }
        }

        public string Distance
        {
            get { return Length.Convert(distance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits).ToString("0.00", CultureInfo.CurrentCulture); }
        }

        public string Distance4Decimals
        {
            get { return Length.Convert(distance, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.DistanceUnits).ToString("0.0000", CultureInfo.CurrentCulture); }
        }

        public string ElevGain
        {
            get
            {
                //double elev = endElevation - startElevation;
                //return elev.ToString("N2");
                return Length.Convert(elevGain, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits).ToString("0.", CultureInfo.CurrentCulture);
            }
        }

        public string AvgGrade
        {
            get { return (avgGrade * 100).ToString("0.0", CultureInfo.CurrentCulture) + "%"; }
        }

        public string HR
        {
            get { return record.AvgHR.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string Cadence
        {
            get { return record.AvgCadence.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string Power
        {
            get { return record.AvgPower.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string Time
        {
            get { return Utilities.TimeSpan_RemoveLeadingZeros(record.TotalTime).ToString(); }
        }

        /// <summary>
        /// Speed in the user's defined units
        /// </summary>
        public string AvgSpeed
        {
            get
            {
                return record.Speed.ToString("0.0", CultureInfo.CurrentCulture);
            }
        }

        public string AvgPace
        {
            get
            {
                return Utilities.TimeSpan_RemoveLeadingZeros(record.AvgPace);
            }
        }

        public string HillId
        {
            get { return hillNumber.ToString(); }
        }

        public string Date
        {
            get { return record.StartDateTime.ToString(); }
        }

        public string GradeImpact
        {
            get { return gradeImpact.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string HillScoreClimbByBike
        {
            get
            {
                // TODO: Somehow (not sure yet) but these 'Display Member' string conversions can be moved to a DefaultTreeRenderer
                //  instead of having to manually create all these
                return hillScoreClimbByBike.ToString("0.00", CultureInfo.CurrentCulture);
            }
        }

        public string HillScoreCycle2Max
        {
            get { return hillScoreCycle2Max.ToString("0.00", CultureInfo.CurrentCulture); }
        }

        public string HillScoreFiets
        {
            get { return hillScoreFiets.ToString("0.00", CultureInfo.CurrentCulture); }
        }

        public string HillScoreCourseScoreRunning
        {
            get { return hillScoreCourseScoreRunning.ToString("0.00", CultureInfo.CurrentCulture); }
        }

        public string HillScoreCourseScoreCycling
        {
            get { return hillScoreCourseScoreCycling.ToString("0.00", CultureInfo.CurrentCulture); }
        }

        public string PreviousScore
        {
            get { return previousFeatureScore.ToString("0.00", CultureInfo.CurrentCulture); }
        }

        public string StartPercentTime
        {
            get { return startPercentTime.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string EndPercentTime
        {
            get { return endPercentTime.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string StartPercentDistance
        {
            get { return startPercentDistance.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string EndPercentDistance
        {
            get { return endPercentDistance.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string PlacementImpact
        {
            get { return placementImpact.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string VAM
        {
            get
            {
                return vam.ToString("0.0", CultureInfo.CurrentCulture);
            }
        }

        public string WKg
        {
            get
            {
                return wKg.ToString("0.00", CultureInfo.CurrentCulture);
            }
        }

        // Ascent and Descent seem to be buggy due to smoothing.  
        //public string AscentDescent
        //{
        //    get
        //    {
        //        double totalAscend = (float)Length.Convert(record.TotalAscend, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);
        //        double totalDescend = (float)Length.Convert(record.TotalDescend, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits);

        //        return "+" + totalAscend.ToString("0", CultureInfo.CurrentCulture) + " / " + totalDescend.ToString("0", CultureInfo.CurrentCulture);
        //    }
        //}

        public string StoppedTime
        {
            get
            {
                return Utilities.TimeSpan_RemoveLeadingZeros(stoppedTime).ToString();
            }
        }

        public string MaxGrade
        {
            get
            {
                return (record.MaxGrade).ToString("0.0", CultureInfo.CurrentCulture) + "%";
            }
        }

        public string MinGrade
        {
            get
            {
                return (record.MinGrade).ToString("0.0", CultureInfo.CurrentCulture) + "%";
            }
        }

        public string HillCategory
        {
            get
            {
                return hillCategory;
            }
        }

        #endregion

        public INumericTimeDataSeries GetSmoothedElevationTrack(int seconds, out float min, out float max)
        {
            return Utilities.STSmooth(Utilities.GetElevationTrack(record.Activity), seconds, out min, out max);
        }

        public INumericTimeDataSeries GetSmoothedSpeedTrack(int seconds, out float min, out float max)
        {
            return Utilities.STSmooth(Utilities.GetSpeedTrack(record.Activity), seconds, out min, out max);
        }

        //public INumericTimeDataSeries GetSmoothedDistanceTrack(int seconds, out float min, out float max)
        //{
        //    return Utilities.STSmooth(Utilities.GetDistanceTrack(record.Activity), seconds, out min, out max);
        //}

        public INumericTimeDataSeries GetSmoothedPowerTrack(int seconds, out float min, out float max)
        {
            return Utilities.STSmooth(record.Activity.PowerWattsTrack, seconds, out min, out max);
        }

        public INumericTimeDataSeries GetSmoothedCadenceTrack(int seconds, out float min, out float max)
        {
            return Utilities.STSmooth(record.Activity.CadencePerMinuteTrack, seconds, out min, out max);
        }

        //public IDistanceDataTrack GetSmoothedDistanceTrack(int seconds, out float min, out float max)
        //{
        //    IDistanceDataTrack distanceTrack;
        //    if (record.Activity.DistanceMetersTrack != null)
        //    {
        //        // #1 Use Distance track from activity
        //        distanceTrack = record.Activity.DistanceMetersTrack;
        //    }
        //    else
        //    {
        //        if (record.Activity.GPSRoute != null)
        //        {
        //            // #2 Otherwise create a distance track from GPS
        //            distanceTrack = Utilities.CreateDistanceDataTrack(record.Activity);
        //            return Utilities.STSmooth(distanceTrack, seconds, min, max);
        //        }
        //        else
        //        {
        //            // Else, no distance track, and cannot create one.
        //            distanceTrack = new DistanceDataTrack();
        //        }
        //    }
        //}

        //public INumericTimeDataSeries GetSmoothedGradeTrack(int seconds, out float min, out float max)
        //{
        //    NumericTimeDataSeries gradeTrack = new NumericTimeDataSeries();
        //    for (int i = 0; i < record.Activity.ElevationMetersTrack.Count; i++)
        //    {
        //        if (i == 0)
        //        {
        //            gradeTrack.Add(record.Activity.ElevationMetersTrack[i].ElapsedSeconds, 0);
        //        }
        //    }
        //}

        #region Constructors

        public Feature(IActivity activity, feature_type type, DateTime inStartTime, DateTime inEndTime)
        {
            startTime = inStartTime;
            endTime = inEndTime;
            added = false;
            hillNumber = 0;
            _feature_type = type;
            masterActivityID = activity.ReferenceId;

            // Default fill and line color
            fillColor = Color.FromArgb(125, 146, 94, 9);
            lineColor = Color.FromArgb(255, 146, 94, 9);
            lineWidth = 1;
            selectedColor = Color.Empty;
            routeWidth = PluginMain.GetApplication().SystemPreferences.RouteSettings.RouteWidth;

            IGPSRoute recordGPS = new GPSRoute();
            INumericTimeDataSeries recordHRTrack = new NumericTimeDataSeries();
            INumericTimeDataSeries pwrTrack = new NumericTimeDataSeries();
            INumericTimeDataSeries elevTrack = new NumericTimeDataSeries();
            INumericTimeDataSeries cadTrack = new NumericTimeDataSeries();
            IDistanceDataTrack distTrack = new DistanceDataTrack();
            RecordCategory category = new RecordCategory();

            ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(activity);
            DateTime start = activity.StartTime;

            if (activity.GPSRoute != null)
            {
                // Check and make sure the route has points
                if (activity.GPSRoute.Count > 0)
                {
                    // If the time passed in is before the start of the gps track, get the first value
                    if (activity.GPSRoute.StartTime > inStartTime)
                    {
                        startPoint = activity.GPSRoute[0].Value;
                    }
                    else
                    {
                        // Set the start point
                        ITimeValueEntry<IGPSPoint> sPoint = activity.GPSRoute.GetInterpolatedValue(inStartTime);
                        if (sPoint != null)
                        {
                            startPoint = sPoint.Value;
                        }
                    }

                    // If the time passed in is after the end of the gps track, get the last value
                    if (activity.GPSRoute.StartTime.AddSeconds(activity.GPSRoute[activity.GPSRoute.Count - 1].ElapsedSeconds) < inEndTime)
                    {
                        endPoint = activity.GPSRoute[activity.GPSRoute.Count - 1].Value;
                    }
                    else
                    {
                        // Set the end point
                        ITimeValueEntry<IGPSPoint> ePoint = activity.GPSRoute.GetInterpolatedValue(inEndTime);
                        if (ePoint != null)
                        {
                            endPoint = ePoint.Value;
                        }
                    }
                }
                

                // Create the GPSRoute
                for (int i=0; i < activity.GPSRoute.Count; i++)
                {
                    if (activity.GPSRoute.StartTime.AddSeconds(activity.GPSRoute[i].ElapsedSeconds) >= inStartTime
                        && activity.GPSRoute.StartTime.AddSeconds(activity.GPSRoute[i].ElapsedSeconds) <= inEndTime)
                    {
                        recordGPS.Add(activity.GPSRoute.StartTime.AddSeconds(activity.GPSRoute[i].ElapsedSeconds), activity.GPSRoute[i].Value);
                    }
                }
            }

            // Create the Distance Track
            INumericTimeDataSeries allDistanceTrack = ai.MovingDistanceMetersTrack; // Utilities.GetDistanceTrack(activity);
            INumericTimeDataSeries allElevationTrack = ai.SmoothedElevationTrack; // Utilities.GetElevationTrack(activity); 

            // Work your way through the moving meters track to create all others
            if(allDistanceTrack != null)
            {
                for (int i = 0; i < allDistanceTrack.Count; i++)
                {
                    DateTime time = allDistanceTrack.StartTime.AddSeconds(allDistanceTrack[i].ElapsedSeconds);
                    if (time >= inStartTime
                        && time <= inEndTime)
                    {
                        // Add the distance point
                        distTrack.Add(time, allDistanceTrack[i].Value);
                        ITimeValueEntry<float> point = null;

                        // Find the elevation point at this time and add it
                        if (allElevationTrack != null && allElevationTrack.Count > 0)
                        {
                            point = allElevationTrack.GetInterpolatedValue(time);
                            if(point != null)
                            {
                                elevTrack.Add(time, point.Value);
                            }
                        }

                        // Find the HR point at this time and add it
                        if (activity.HeartRatePerMinuteTrack != null && activity.HeartRatePerMinuteTrack.Count > 0)
                        {
                            point = activity.HeartRatePerMinuteTrack.GetInterpolatedValue(time);
                            if (point != null)
                            {
                                recordHRTrack.Add(time, point.Value);
                            }
                        }

                        // Find the power point at this time and add it
                        if (activity.PowerWattsTrack != null && activity.PowerWattsTrack.Count > 0)
                        {
                            point = activity.PowerWattsTrack.GetInterpolatedValue(time);
                            if (point != null)
                            {
                                pwrTrack.Add(time, point.Value);
                            }
                        }

                        // Find the cadence point at this time and add it
                        if (activity.CadencePerMinuteTrack != null && activity.CadencePerMinuteTrack.Count > 0)
                        {
                            point = activity.CadencePerMinuteTrack.GetInterpolatedValue(time);
                            if (point != null)
                            {
                                cadTrack.Add(time, point.Value);
                            }
                        }
                    }
                    else if (allDistanceTrack.StartTime.AddSeconds(allDistanceTrack[i].ElapsedSeconds) > inEndTime)
                    {
                        break;
                    }
                }
            }

            // Get the start/end distance
            if (distTrack != null && distTrack.Count > 0)
            {
                startDistance = distTrack[0].Value;
                endDistance = distTrack[distTrack.Count - 1].Value;
            }
            else
            {
                startDistance = 0;
                endDistance = 0;
            }

            // Get the start/end elevation
            if (elevTrack != null && elevTrack.Count > 0)
            {
                startElevation = elevTrack[0].Value;
                endElevation = elevTrack[elevTrack.Count - 1].Value;
            }
            else
            {
                startElevation = 0;
                endElevation = 0;
            }

            // Build the record
            record = new Record(activity, category, recordGPS, recordHRTrack, pwrTrack, cadTrack, distTrack, elevTrack, inStartTime);
            
            // Create a reference id for this hill
            refId = Guid.NewGuid().ToString("D");

            double distanceX = endDistance - startDistance;
            distance = distanceX;

            double elev = endElevation - startElevation;
            elevGain = elev;

            // Find the start percents from the distance track
            if (allDistanceTrack != null && allDistanceTrack.Count > 0)
            {
                startPercentDistance = startDistance / allDistanceTrack[allDistanceTrack.Count - 1].Value;
                endPercentDistance = endDistance / allDistanceTrack[allDistanceTrack.Count - 1].Value;

                startPercentTime = ((inStartTime - allDistanceTrack.StartTime).TotalSeconds / allDistanceTrack[allDistanceTrack.Count - 1].ElapsedSeconds);
                endPercentTime = ((inEndTime - allDistanceTrack.StartTime).TotalSeconds / allDistanceTrack[allDistanceTrack.Count - 1].ElapsedSeconds);
            }

            // Calculate the VAM (Velocity Ascended, Meters per hour)
            // Calculate the W/kg (Relative power)
            vam = 0;
            wKg = 0;
            if (elevGain > 0)
            {
                vam = (elevGain * 60f * 60f) / record.TotalTime.TotalSeconds;
                wKg = vam / ((2 + (avgGrade * 10f)) * 100f);
            }

            ActivityInfo aiRec = ActivityInfoCache.Instance.GetInfo(record.Activity);
            stoppedTime = aiRec.TimeNotMoving;
        }

        public Feature()
        { }

        #endregion

        #region Functions

        public int CompareTo(Feature feature, FeatureComparer.ComparisonType comparisonMethod, FeatureComparer.Order sortOrder)
        {
            int result = 0;

            switch (comparisonMethod)
            {//Define all different sort methods
                case FeatureComparer.ComparisonType.HillId:
                    if (hillNumber != feature.hillNumber)
                        result = hillNumber.CompareTo(feature.hillNumber);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.End:
                    if (endDistance != feature.endDistance)
                        result = endDistance.CompareTo(feature.endDistance);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.Distance:
                    if (distance != feature.distance)
                        result = distance.CompareTo(feature.distance);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.Time:
                    if (record.TotalTime != feature.record.TotalTime)
                        result = record.TotalTime.CompareTo(feature.record.TotalTime);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.ElevGain:
                    if (elevGain != feature.elevGain)
                        result = elevGain.CompareTo(feature.elevGain);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.AvgSpeed:
                    if (record.Speed != feature.record.Speed)
                        result = record.Speed.CompareTo(feature.record.Speed);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.AvgGrade:
                    if (avgGrade != feature.avgGrade)
                        result = avgGrade.CompareTo(feature.avgGrade);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.HR:
                    if (record.AvgHR != feature.record.AvgHR)
                        result = record.AvgHR.CompareTo(feature.record.AvgHR);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.Cadence:
                    if (record.AvgCadence != feature.record.AvgCadence)
                        result = record.AvgCadence.CompareTo(feature.record.AvgCadence);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.Power:
                    if (record.AvgPower != feature.record.AvgPower)
                        result = record.AvgPower.CompareTo(feature.record.AvgPower);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.Date:
                    if (record.StartDateTime != feature.record.StartDateTime)
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    else
                        result = startDistance.CompareTo(feature.startDistance);
                    break;
                case FeatureComparer.ComparisonType.GradeImpact:
                    if (gradeImpact != feature.gradeImpact)
                        result = gradeImpact.CompareTo(feature.gradeImpact);
                    else
                        result = startDistance.CompareTo(feature.startDistance);
                    break;
                case FeatureComparer.ComparisonType.HillScoreClimbByBike:
                    if (hillScoreClimbByBike != feature.hillScoreClimbByBike)
                        result = hillScoreClimbByBike.CompareTo(feature.hillScoreClimbByBike);
                    else
                        result = startDistance.CompareTo(feature.startDistance);
                    break;
                case FeatureComparer.ComparisonType.HillScoreCycle2Max:
                    if (hillScoreCycle2Max != feature.hillScoreCycle2Max)
                        result = hillScoreCycle2Max.CompareTo(feature.hillScoreCycle2Max);
                    else
                        result = startDistance.CompareTo(feature.startDistance);
                    break;
                case FeatureComparer.ComparisonType.HillScoreFiets:
                    if (hillScoreFiets != feature.hillScoreFiets)
                        result = hillScoreFiets.CompareTo(feature.hillScoreFiets);
                    else
                        result = startDistance.CompareTo(feature.startDistance);
                    break;
                case FeatureComparer.ComparisonType.HillScoreCourseScoreRunning:
                    if (hillScoreCourseScoreRunning != feature.hillScoreCourseScoreRunning)
                        result = hillScoreCourseScoreRunning.CompareTo(feature.hillScoreCourseScoreRunning);
                    else
                        result = startDistance.CompareTo(feature.startDistance);
                    break;
                case FeatureComparer.ComparisonType.HillScoreCourseScoreCycling:
                    if (hillScoreCourseScoreCycling != feature.hillScoreCourseScoreCycling)
                        result = hillScoreCourseScoreCycling.CompareTo(feature.hillScoreCourseScoreCycling);
                    else
                        result = startDistance.CompareTo(feature.startDistance);
                    break;
                case FeatureComparer.ComparisonType.PreviousScore:
                    if (previousFeatureScore != feature.previousFeatureScore)
                        result = previousFeatureScore.CompareTo(feature.previousFeatureScore);
                    else
                        result = startDistance.CompareTo(feature.startDistance);
                    break;
                case FeatureComparer.ComparisonType.PlacementImpact:
                    if (placementImpact != feature.placementImpact)
                        result = placementImpact.CompareTo(feature.placementImpact);
                    else
                        result = startDistance.CompareTo(feature.startDistance);
                    break;
                case FeatureComparer.ComparisonType.AvgPace:
                    if (record.Speed != feature.record.Speed)
                        result = record.Speed.CompareTo(feature.record.Speed);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.VAM:
                    if (vam != feature.vam)
                        result = vam.CompareTo(feature.vam);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.WKg:
                    if (wKg != feature.wKg)
                        result = wKg.CompareTo(feature.wKg);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.Ascent:
                    if (record.TotalAscend != feature.record.TotalAscend)
                        result = record.TotalAscend.CompareTo(feature.record.TotalAscend);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.StoppedTime:
                    if (stoppedTime != feature.stoppedTime)
                        result = stoppedTime.CompareTo(feature.stoppedTime);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.MaxGrade:
                    if (record.MaxGrade != feature.record.MaxGrade)
                        result = record.MaxGrade.CompareTo(feature.record.MaxGrade);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.MinGrade:
                    if (record.MinGrade != feature.record.MinGrade)
                        result = record.MinGrade.CompareTo(feature.record.MinGrade);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                case FeatureComparer.ComparisonType.HillCategory:
                    if (HillCategory != feature.HillCategory)
                        result = HillCategory.CompareTo(feature.HillCategory);
                    else
                        result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
                default:
                    result = record.StartDateTime.CompareTo(feature.record.StartDateTime);
                    break;
            }
            if (sortOrder == FeatureComparer.Order.Descending)
            { return (result * -1); }
            else
            { return result; }
        }

        #endregion

        #region FeatureComparer

        public class FeatureComparer : IComparer<Feature>
        {
            public enum ComparisonType
            {
                HillId = 1,
                Start = 2,
                End = 3,
                Distance = 4,
                Time = 5,
                ElevGain = 6,
                AvgSpeed = 7,
                AvgGrade = 8,
                HR = 9,
                Cadence = 10,
                Power = 11,
                Date = 12,
                GradeImpact = 13,
                HillScoreClimbByBike = 14,
                PlacementImpact = 15,
                HillScoreCycle2Max = 16,
                HillScoreFiets = 17,
                HillScoreCourseScoreRunning = 18,
                HillScoreCourseScoreCycling = 19,
                PreviousScore = 20,
                AvgPace = 21,
                VAM = 22,
                WKg = 23,
                Ascent = 24,
                StoppedTime = 25,
                MaxGrade = 26,
                MinGrade = 27,
                HillCategory = 28
            }

            public enum Order
            { Ascending = 1, Descending = 2 }

            private ComparisonType _comparisonType;
            public ComparisonType ComparisonMethod
            {
                get { return _comparisonType; }
                set { _comparisonType = value; }
            }

            private Order _sortOrder;
            public Order SortOrder
            {
                get { return _sortOrder; }
                set { _sortOrder = value; }
            }

            public int Compare(Feature x, Feature y)
            {
                return x.CompareTo(y, _comparisonType, _sortOrder);
            }
        }
        
        #endregion

        #region IEquatable<MatchingFeature> Members

        //public bool Equals(Feature other)
        //{
        //    if (this.startTime == other.startTime &&
        //        this.endTime == other.endTime)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public bool Equals(Feature other)
        {
            double distanceBuffer = this.totalDistanceMeters * .15f;
            double gradeBuffer = .02f;

            // Check distance and grade to see if we should look further
            if (Math.Abs(this.totalDistanceMeters - other.totalDistanceMeters) < distanceBuffer &&
                Math.Abs(this.avgGrade - other.avgGrade) < gradeBuffer)
            {
                double proximityConstant = this.totalDistanceMeters / this.record.TotalTime.TotalSeconds * 4f;

                // Check start points
                float startDist = this.startPoint.DistanceMetersToPoint(other.startPoint);
                if (startDist < proximityConstant)
                {
                    // Check end points
                    float endDist = this.endPoint.DistanceMetersToPoint(other.endPoint);
                    if (endDist < proximityConstant)
                    {
                        // We're good to go
                        return true;
                    }
                }
            }

            // Not a match
            return false;

        }

        #endregion
    }
}
