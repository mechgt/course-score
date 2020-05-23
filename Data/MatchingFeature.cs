using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.GPS;

namespace CourseScore.Data
{
    /// <summary>
    /// MatchingFeature is a small class to use when finding features in an activity.
    /// You can use it as a comparison feature to find like/exact features.
    /// </summary>
    public class MatchingFeature : IEquatable<MatchingFeature>
    {
        // Variables
        public double totalDistanceMeters;
        public double avgGrade;
        public TimeSpan totalTime;
        public IGPSPoint startPoint;
        public IGPSPoint endPoint;
        public Guid id;
        public DateTime featureStartTime;

        // Scores
        public double hillScoreClimbByBike;
        public double hillScoreCycle2Max;
        public double hillScoreFiets;
        public double hillScoreCourseScoreRunning;
        public double hillScoreCourseScoreCycling;

        // Activity Fact
        public ActivityFacts activityFact;

        // Activity info that is not directly related to this MatchingFeature
        public struct ActivityFacts
        {
            public DateTime factTime;

            public ActivityFacts(DateTime time)
            {
                factTime = time;
            }

        }

        /// <summary>
        /// Constructor to create a new MatchingFeature based on the stats of the supplied Feature
        /// </summary>
        /// <param name="inFeature">Feature used to create this MatchingFeature</param>
        public MatchingFeature(Feature inFeature)
        {
            totalDistanceMeters = inFeature.totalDistanceMeters;
            avgGrade = inFeature.avgGrade;
            totalTime = inFeature.record.TotalTime;
            startPoint = inFeature.startPoint;
            endPoint = inFeature.endPoint;
            hillScoreClimbByBike = inFeature.hillScoreClimbByBike;
            hillScoreCycle2Max = inFeature.hillScoreCycle2Max;
            hillScoreFiets = inFeature.hillScoreFiets;
            hillScoreCourseScoreRunning = inFeature.hillScoreCourseScoreRunning;
            hillScoreCourseScoreCycling = inFeature.hillScoreCourseScoreCycling;
            id = Guid.NewGuid();
            featureStartTime = inFeature.startTime;
        }

        /// <summary>
        /// Constructor to cloan a MatchingFeature
        /// </summary>
        /// <param name="inMatchingFeature">MatchingFeature to clone</param>
        public MatchingFeature(MatchingFeature inMatchingFeature)
        {
            this.totalDistanceMeters = inMatchingFeature.totalDistanceMeters;
            this.avgGrade = inMatchingFeature.avgGrade;
            this.totalTime = inMatchingFeature.totalTime;
            this.startPoint = inMatchingFeature.startPoint;
            this.endPoint = inMatchingFeature.endPoint;
            this.hillScoreClimbByBike = inMatchingFeature.hillScoreClimbByBike;
            this.hillScoreCycle2Max = inMatchingFeature.hillScoreCycle2Max;
            this.hillScoreFiets = inMatchingFeature.hillScoreFiets;
            this.hillScoreCourseScoreRunning = inMatchingFeature.hillScoreCourseScoreRunning;
            this.hillScoreCourseScoreCycling = inMatchingFeature.hillScoreCourseScoreCycling;
            this.id = inMatchingFeature.id;
            this.activityFact = inMatchingFeature.activityFact;
            this.featureStartTime = inMatchingFeature.featureStartTime;
        }


        #region IEquatable<MatchingFeature> Members

        public bool Equals(MatchingFeature other)
        {
            double distanceBuffer = this.totalDistanceMeters * .15f;
            double gradeBuffer = .02f;

            // Check distance and grade to see if we should look further
            if (Math.Abs(this.totalDistanceMeters - other.totalDistanceMeters) < distanceBuffer &&
                Math.Abs(this.avgGrade - other.avgGrade) < gradeBuffer)
            {
                double proximityConstant = this.totalDistanceMeters / this.totalTime.TotalSeconds * 4f;

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
