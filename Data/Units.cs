using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Data.Measurement;

namespace CourseScore.Data
{
    public static partial class Units
    {
        private static bool unitsLoaded;
        private static string distUnits;
        private static string paceUnits;
        private static string spdUnits;
        private static string elevUnits;
        private static string tempUnits;

        /// <summary>
        /// Distance Units to be displayed
        /// </summary>
        public static string Distance
        {
            get
            {
                if (!unitsLoaded)
                {
                    LoadUnits();
                }

                return distUnits;
            }
        }

        /// <summary>
        /// Pace Units to be displayed
        /// </summary>
        public static string Pace
        {
            get
            {
                if (!unitsLoaded)
                {
                    LoadUnits();
                }

                return paceUnits;
            }
        }

        /// <summary>
        /// Speed Units to be displayed
        /// </summary>
        public static string Speed
        {
            get
            {
                if (!unitsLoaded)
                {
                    LoadUnits();
                }

                return spdUnits;
            }
        }

        /// <summary>
        /// Elevation Units to be displayed
        /// </summary>
        public static string Elevation
        {
            get
            {
                if (!unitsLoaded)
                {
                    LoadUnits();
                }

                return elevUnits;
            }
        }

        /// <summary>
        /// Temperature Units to be displayed
        /// </summary>
        public static string Temp
        {
            get
            {
                if (!unitsLoaded)
                {
                    LoadUnits();
                }

                return tempUnits;
            }
        }

        public static string Power
        {
            get
            {
                return "watts";
            }
        }
        private static void LoadUnits()
        {
            // Setup all the units text
            switch (PluginMain.GetApplication().SystemPreferences.DistanceUnits)
            {
                case Length.Units.Meter:
                    distUnits = "m";
                    paceUnits = "min/m";
                    spdUnits = "m/hr";
                    break;

                case Length.Units.Kilometer:
                    distUnits = "km";
                    paceUnits = "min/km";
                    spdUnits = "km/hr";
                    break;

                case Length.Units.Mile:
                    distUnits = "mi";
                    paceUnits = "min/mi";
                    spdUnits = "mph";
                    break;

                case Length.Units.Yard:
                    distUnits = "yd";
                    paceUnits = "min/yd";
                    spdUnits = "yd/hr";
                    break;

            }
            switch (PluginMain.GetApplication().SystemPreferences.ElevationUnits)
            {
                case Length.Units.Centimeter:
                    elevUnits = "cm";
                    break;

                case Length.Units.Foot:
                    elevUnits = "feet";
                    break;

                case Length.Units.Inch:
                    elevUnits = "in";
                    break;

                case Length.Units.Kilometer:
                    elevUnits = "km";
                    break;

                case Length.Units.Meter:
                    elevUnits = "m";
                    break;

                case Length.Units.Mile:
                    elevUnits = "mi";
                    break;

                case Length.Units.Yard:
                    elevUnits = "yd";
                    break;
            }

            //string tempUnits = string.Empty;
            switch (PluginMain.GetApplication().SystemPreferences.TemperatureUnits)
            {
                case Temperature.Units.Celsius:
                    tempUnits = "°C";
                    break;

                case Temperature.Units.Fahrenheit:
                    tempUnits = "°F";
                    break;
            }

            unitsLoaded = true;

        }
    }
}
