using System;
using System.Collections.Generic;
using System.Text;

namespace CourseScore.Data
{
    /// <summary>
    /// ScoreEquation is used to populate a combobox with an enum and localized
    /// string for display purposes
    /// </summary>
    public class ScoreEquation
    {
        public Score equation;

        public ScoreEquation(Score thisEquation)
        {
            equation = thisEquation;
        }

        public enum Score
        {
            ClimbByBike,
            Cycle2Max,
            Fiets,
            CourseScoreCycling,
            CourseScoreRunning,
            HillCategory
        }

        /// <summary>
        /// Lookup the text for this enum
        /// </summary>
        /// <param name="field">enum to lookup</param>
        /// <returns>Returns the localized text</returns>
        public static string ScoreEquationLookup(Score inEquation)
        {
            switch (inEquation)
            {
                case Score.ClimbByBike:
                    return "ClimbByBike.com";
                case Score.Cycle2Max:
                    return "Cycle2Max.com";
                case Score.Fiets:
                    return "FIETS-index";
                case Score.CourseScoreCycling:
                    return Resources.Strings.Label_CourseScore + " (" + Resources.Strings.Label_Cycling + ")";
                case Score.CourseScoreRunning:
                    return Resources.Strings.Label_CourseScore + " (" + Resources.Strings.Label_Running + ")";
                case Score.HillCategory:
                    return Resources.Strings.Label_HillClassification;
            }
            return "";
        }

        /// <summary>
        /// ToString override to localize the text
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ScoreEquationLookup(equation);
        }
    }
}
