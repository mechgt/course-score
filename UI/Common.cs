namespace CourseScore.UI
{
    using System.Drawing;
    using System;

    public class Common
    {
        public static readonly Color ColorCadence = Color.FromArgb(78, 154, 6);
        public static readonly Color ColorElevation = Color.FromArgb(143, 89, 2);
        public static readonly Color ColorGrade = Color.FromArgb(193, 125, 17);
        public static readonly Color ColorHR = Color.FromArgb(204, 0, 0);
        public static readonly Color ColorPower = Color.FromArgb(92, 53, 102);
        public static readonly Color ColorSpeed = Color.FromArgb(32, 74, 135);
        public static readonly Color ColorVAM = Color.OrangeRed;

        public static readonly UInt16 SecondsPerMinute = 60;
        public static readonly UInt16 MinutesPerHour = 60;
        public static readonly UInt16 SecondsPerHour = (UInt16)(MinutesPerHour * SecondsPerMinute);

        public static readonly Color HardestColor = Color.DarkRed;
        public static readonly Color SteepestColor = Color.Olive;
        public static readonly Color LongestColor = Color.Navy;


        public enum ColorIndex
        {
            Cadence = 0,
            Elevation = 1,
            HeartRateBPM = 2,
            HeartRatePercentMax = 3,
            Power = 4,
            Grade = 5,
            Speed = 6,
            VAM = 7
        }

        private static readonly Color[] STDataColor = new Color[]
                    { ColorCadence,       // Cadence = 0
                      ColorElevation,     // Elevation = 1
                      ColorHR,            // HeartRateBPM = 2
                      ColorHR,            // HeartRatePercentMax = 3
                      ColorPower,         // Power
                      ColorGrade,         // Grade = 5
                      ColorSpeed,         // Speed = 6
                      ColorVAM};          // VAM = 7

        public static Color GetDataColor(ColorIndex index)
        {
            int i = (int)index;
            return STDataColor[i];
        }
    }
}
