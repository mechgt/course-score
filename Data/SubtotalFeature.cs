using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using ZoneFiveSoftware.Common.Data.Measurement;
using CourseScore.UI;

namespace CourseScore.Data
{
    public class SubtotalFeature
    {
        double startDistance = 0;
        double endDistance = 0;
        double distance = 0;
        double elevGain = 0;
        double grade = 0;
        double speed = 0;
        double hr = 0;
        double cadence = 0;
        double power = 0;
        double hillScoreClimbByBike = 0;
        double hillScoreCycle2Max = 0;
        double hillScoreFiets = 0;
        double hillScoreCourseScoreCycling = 0;
        double hillScoreCourseScoreRunning = 0;
        double maxGrade = 0;
        double minGrade = 0;
        double vam = 0;
        double wkg = 0;
        TimeSpan totalTime = new TimeSpan();
        TimeSpan stoppedTime = new TimeSpan();
        int featuresCount = 0;

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

        public string Time
        {
            get { return Utilities.TimeSpan_RemoveLeadingZeros(totalTime).ToString(); }
        }

        public string ElevGain
        {
            get
            { return Length.Convert(elevGain, Length.Units.Meter, PluginMain.GetApplication().SystemPreferences.ElevationUnits).ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string AvgGrade
        {
            get { return (grade * 100).ToString("0.0", CultureInfo.CurrentCulture) + "%"; }
        }

        public string AvgSpeed
        {
            get
            {
                return speed.ToString("0.0", CultureInfo.CurrentCulture);
            }
        }

        public string AvgPace
        {
            get
            {
                TimeSpan pace = new TimeSpan();

                if (speed != 0 && !double.IsNaN(speed))
                {
                    int seconds = (int)(60 * 60 / speed);
                    pace = new TimeSpan(0, 0, 0, seconds);
                }
                else
                {
                    pace = new TimeSpan(0);
                }
                return Utilities.TimeSpan_RemoveLeadingZeros(pace);
            }
        }

        public string HR
        {
            get { return hr.ToString("0.", CultureInfo.CurrentCulture); }
        }

        public string Cadence
        {
            get { return cadence.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string Power
        {
            get { return power.ToString("0.0", CultureInfo.CurrentCulture); }
        }

        public string HillScoreClimbByBike
        {
            get
            { return hillScoreClimbByBike.ToString("0.00", CultureInfo.CurrentCulture); }
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

        public string VAM
        {
            get
            { return vam.ToString("0.0", CultureInfo.CurrentCulture);}
        }

        public string WKg
        {
            get
            { return wkg.ToString("0.00", CultureInfo.CurrentCulture);  }
        }

        public string StoppedTime
        {
            get { return Utilities.TimeSpan_RemoveLeadingZeros(stoppedTime).ToString();}
        }

        public string MaxGrade
        {
            get { return (maxGrade).ToString("0.0", CultureInfo.CurrentCulture) + "%";}
        }

        public string MinGrade
        {
            get { return (minGrade).ToString("0.0", CultureInfo.CurrentCulture) + "%";}
        }

        public string HillId
        {
            get { return featuresCount.ToString(); }
        }

        #endregion

        public SubtotalFeature()
        {
        }

        public SubtotalFeature(List<Feature> features)
        {
            foreach (Feature feature in features)
            {
                startDistance += feature.startDistance;
                endDistance += feature.endDistance;
                distance += feature.distance;
                elevGain += (feature.endElevation - feature.startElevation);
                grade += feature.avgGrade;
                speed += feature.record.Speed;
                hr += feature.record.AvgHR;
                cadence += feature.record.AvgCadence;
                power += feature.record.AvgPower;
                hillScoreClimbByBike += feature.hillScoreClimbByBike;
                hillScoreCycle2Max += feature.hillScoreCycle2Max;
                hillScoreFiets += feature.hillScoreFiets;
                hillScoreCourseScoreCycling += feature.hillScoreCourseScoreCycling;
                hillScoreCourseScoreRunning += feature.hillScoreCourseScoreRunning;
                maxGrade += feature.record.MaxGrade;
                minGrade += feature.record.MinGrade;
                vam += feature.vam;
                wkg += feature.wKg;
                totalTime = totalTime.Add(feature.record.TotalTime);
                stoppedTime = stoppedTime.Add(feature.stoppedTime);
                featuresCount++;
            }
        }

        public SubtotalFeature(SubtotalFeature feature)
        {
            startDistance = feature.startDistance;
            endDistance = feature.endDistance;
            distance = feature.distance;
            elevGain = feature.elevGain;
            grade = feature.grade;
            speed = feature.speed;
            hr = feature.hr;
            cadence = feature.cadence;
            power = feature.power;
            hillScoreClimbByBike = feature.hillScoreClimbByBike;
            hillScoreCycle2Max = feature.hillScoreCycle2Max;
            hillScoreFiets = feature.hillScoreFiets;
            hillScoreCourseScoreCycling = feature.hillScoreCourseScoreCycling;
            hillScoreCourseScoreRunning = feature.hillScoreCourseScoreRunning;
            maxGrade = feature.maxGrade;
            minGrade = feature.minGrade;
            vam = feature.vam;
            wkg = feature.wkg;
            totalTime = feature.totalTime;
            stoppedTime = feature.stoppedTime;
            featuresCount++;
        }

        public SubtotalFeature Average()
        {
            SubtotalFeature average = new SubtotalFeature(this);
            average.startDistance = average.startDistance / featuresCount;
            average.endDistance = average.endDistance / featuresCount;
            average.distance = average.distance / featuresCount;
            average.elevGain = average.elevGain / featuresCount;
            average.grade = average.grade / featuresCount;
            average.speed = average.speed / featuresCount;
            average.hr = average.hr / featuresCount;
            average.cadence = average.cadence / featuresCount;
            average.power = average.power / featuresCount;
            average.hillScoreClimbByBike = average.hillScoreClimbByBike / featuresCount;
            average.hillScoreCourseScoreCycling = average.hillScoreCourseScoreCycling / featuresCount;
            average.hillScoreCourseScoreRunning = average.hillScoreCourseScoreRunning / featuresCount;
            average.hillScoreCycle2Max = average.hillScoreCycle2Max / featuresCount;
            average.hillScoreFiets = average.hillScoreFiets / featuresCount;
            average.maxGrade = average.maxGrade / featuresCount;
            average.minGrade = average.minGrade / featuresCount;
            average.vam = average.vam / featuresCount;
            average.wkg = average.wkg / featuresCount;
            average.totalTime = new TimeSpan(average.totalTime.Ticks / featuresCount);
            average.stoppedTime = new TimeSpan(average.stoppedTime.Ticks / featuresCount);
            average.featuresCount = featuresCount;
            return average;

        }

    }
}
