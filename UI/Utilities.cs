// <copyright file="Utilities.cs" company="N/A">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>mechgt</author>
// <email>mechgt@gmail.com</email>
// <date>2008-12-23</date>
namespace CourseScore.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using ZoneFiveSoftware.Common.Data.Fitness;
    using ZoneFiveSoftware.Common.Visuals;
    using ZoneFiveSoftware.Common.Data;
    using System.Drawing;
    using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Data.Measurement;

    /// <summary>
    /// Generic utilities class that can be used on many projects
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Refresh the SportTracks Calendar with the selected activities
        /// </summary>
        /// <param name="activities">These activity dates will be highlighted on the calendar</param>
        internal static void RefreshCalendar(IEnumerable<IActivity> activities)
        {
            IList<DateTime> dates = new List<DateTime>();
            foreach (IActivity activity in activities)
            {
                dates.Add(activity.StartTime.ToLocalTime().Date);
            }

            PluginMain.GetApplication().Calendar.SetHighlightedDates(dates);
        }

        /// <summary>
        /// Filters activities for a category (and it's subcategories), and includes only those activities with a HeartRateTrack.
        /// </summary>
        /// <param name="category">Category to filter for</param>
        /// <param name="activityList">List of activities to be filtered from.</param>
        /// <returns>Returns a list of activities filtered by category</returns>
        internal static IEnumerable<IActivity> FilterActivities(IActivityCategory category, IList<IActivity> activityList)
        {
            IList<IActivity> filteredActivities = new List<IActivity>();
            ActivityInfoCache info = ActivityInfoCache.Instance;
            IActivityCategory activityCategory;
            foreach (IActivity activity in activityList)
            {
                activityCategory = info.GetInfo(activity).Activity.Category;
                while (true)
                {
                    if (activityCategory == category)
                    {
                        // Include Activity
                        filteredActivities.Add(info.GetInfo(activity).Activity);
                        break;
                    }
                    else if (activityCategory.Parent != null)
                    {
                        // Keep searching
                        activityCategory = activityCategory.Parent;
                    }
                    else
                    {
                        // Exclude Activity
                        break;
                    }
                }
            }

            return filteredActivities;
        }

        /// <summary>
        /// Perform a smoothing operation using a moving average on the data series
        /// </summary>
        /// <param name="track">The data series to smooth</param>
        /// <param name="period">The range to smooth.  This is the total number of seconds to smooth across (slightly different than the ST method.)</param>
        /// <param name="min">An out parameter set to the minimum value of the smoothed data series</param>
        /// <param name="max">An out parameter set to the maximum value of the smoothed data series</param>
        /// <returns></returns>
        internal static INumericTimeDataSeries Smooth(INumericTimeDataSeries track, uint period, out float min, out float max)
        {
            min = float.NaN;
            max = float.NaN;
            INumericTimeDataSeries smooth = new NumericTimeDataSeries();

            if (track != null && track.Count > 0 && period > 1)
            {
                //min = float.NaN;
                //max = float.NaN;
                int start = 0;
                int index = 0;
                float value = 0;
                float delta;

                float per = period;

                // Iterate through track
                // For each point, create average starting with 'start' index and go forward averaging 'period' seconds.
                // Stop when last 'full' period can be created ([start].ElapsedSeconds + 'period' seconds >= TotalElapsedSeconds)
                while (track[start].ElapsedSeconds + period < track.TotalElapsedSeconds)
                {
                    while (track[index].ElapsedSeconds < track[start].ElapsedSeconds + period)
                    {
                        delta = track[index + 1].ElapsedSeconds - track[index].ElapsedSeconds;
                        value += track[index].Value * delta;
                        index++;
                    }

                    // Finish value calculation
                    per = track[index].ElapsedSeconds - track[start].ElapsedSeconds;
                    value = value / per;

                    // Add value to track
                    // TODO: I really don't need the smoothed track... really just need max.  Kill this for efficiency?
                    //smooth.Add(track.StartTime.AddSeconds(start), value);
                    smooth.Add(track.EntryDateTime(track[index]), value);

                    // Remove beginning point for next cycle
                    delta = track[start + 1].ElapsedSeconds - track[start].ElapsedSeconds;
                    value = (per * value - delta * track[start].Value);

                    // Next point
                    start++;
                }

                max = smooth.Max;
                min = smooth.Min;
            }
            else if (track != null && track.Count > 0 && period == 1)
            {
                min = track.Min;
                max = track.Max;
                return track;
            }

            return smooth;
        }

        internal static INumericTimeDataSeries STSmooth(INumericTimeDataSeries data, int seconds, out float min, out float max)
        {
            min = float.NaN;
            max = float.NaN;
            if (data != null)
            {
                if (data.Count == 0)
                {
                    // Special case, no data
                    return new ZoneFiveSoftware.Common.Data.NumericTimeDataSeries();
                }
                else if (data.Count == 1 || seconds < 1)
                {
                    // Special case
                    INumericTimeDataSeries copyData = new ZoneFiveSoftware.Common.Data.NumericTimeDataSeries();
                    min = data[0].Value;
                    max = data[0].Value;
                    foreach (ITimeValueEntry<float> entry in data)
                    {
                        copyData.Add(data.StartTime.AddSeconds(entry.ElapsedSeconds), entry.Value);
                        min = Math.Min(min, entry.Value);
                        max = Math.Max(max, entry.Value);
                    }
                    return copyData;
                }
                min = float.MaxValue;
                max = float.MinValue;
                int smoothWidth = Math.Max(0, seconds * 2); // Total width/period.  'seconds' is the half-width... seconds on each side to smooth
                int denom = smoothWidth * 2; // Final value to divide by.  It's divide by 2 because we're double-adding everything
                INumericTimeDataSeries smoothedData = new ZoneFiveSoftware.Common.Data.NumericTimeDataSeries();

                // Loop through entire dataset
                for (int nEntry = 0; nEntry < data.Count; nEntry++)
                {
                    ITimeValueEntry<float> entry = data[nEntry];
                    // TODO: Don't reset value & index markers, instead continue data here...
                    double value = 0;
                    double delta;
                    // Data prior to entry
                    long secondsRemaining = seconds;
                    ITimeValueEntry<float> p1, p2;
                    int increment = -1;
                    int pos = nEntry - 1;
                    p2 = data[nEntry];


                    while (secondsRemaining > 0 && pos >= 0)
                    {
                        p1 = data[pos];
                        if (SumValues(p2, p1, ref value, ref secondsRemaining))
                        {
                            pos += increment;
                            p2 = p1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (secondsRemaining > 0)
                    {
                        // Occurs at beginning of track when period extends before beginning of track.
                        delta = data[0].Value * secondsRemaining * 2;
                        value += delta;
                    }
                    // Data after entry
                    secondsRemaining = seconds;
                    increment = 1;
                    pos = nEntry;
                    p1 = data[nEntry];
                    while (secondsRemaining > 0 && pos < data.Count - 1)
                    {
                        p2 = data[pos + 1];
                        if (SumValues(p1, p2, ref value, ref secondsRemaining))
                        {
                            // Move to next point
                            pos += increment;
                            p1 = p2;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (secondsRemaining > 0)
                    {
                        // Occurs at end of track when period extends past end of track
                        value += data[data.Count - 1].Value * secondsRemaining * 2;
                    }
                    float entryValue = (float)(value / denom);
                    smoothedData.Add(data.StartTime.AddSeconds(entry.ElapsedSeconds), entryValue);
                    min = Math.Min(min, entryValue);
                    max = Math.Max(max, entryValue);

                    // TODO: Remove 'first' p1 & p2 SumValues from 'value'
                    if (data[nEntry].ElapsedSeconds - seconds < 0)
                    {
                        // Remove 1 second worth of first data point (multiply by 2 because everything is double here)
                        value -= data[0].Value * 2;
                    }
                    else
                    {
                        // Remove data in middle of track (typical scenario)
                        //value -= 
                    }
                }
                return smoothedData;
            }
            else
            {
                return null;
            }
        }

        private static bool SumValues(ITimeValueEntry<float> p1, ITimeValueEntry<float> p2, ref double value, ref long secondsRemaining)
        {
            double spanSeconds = Math.Abs((double)p2.ElapsedSeconds - (double)p1.ElapsedSeconds);
            if (spanSeconds <= secondsRemaining)
            {
                value += (p1.Value + p2.Value) * spanSeconds;
                secondsRemaining -= (long)spanSeconds;
                return true;
            }
            else
            {
                double percent = (double)secondsRemaining / (double)spanSeconds;
                value += (p1.Value * ((float)2 - percent) + p2.Value * percent) * secondsRemaining;
                secondsRemaining = 0;
                return false;
            }
        }

        /// <summary>
        /// Removes paused (but not stopped?) times in track.
        /// </summary>
        /// <param name="sourceTrack">Source data track to remove paused times</param>
        /// <param name="activity"></param>
        /// <returns>Returns an INumericTimeDataSeries with the paused times removed.</returns>
        internal static INumericTimeDataSeries RemovePausedTimesInTrack(INumericTimeDataSeries sourceTrack, IActivity activity)
        {
            ActivityInfo activityInfo = ActivityInfoCache.Instance.GetInfo(activity);

            if (activityInfo != null && sourceTrack != null)
            {
                if (activityInfo.NonMovingTimes.Count == 0)
                {
                    return sourceTrack;
                }
                else
                {
                    INumericTimeDataSeries result = new NumericTimeDataSeries();
                    DateTime currentTime = sourceTrack.StartTime;
                    IEnumerator<ITimeValueEntry<float>> sourceEnumerator = sourceTrack.GetEnumerator();
                    IEnumerator<IValueRange<DateTime>> pauseEnumerator = activityInfo.NonMovingTimes.GetEnumerator();
                    double totalPausedTimeToDate = 0;
                    bool sourceEnumeratorIsValid;
                    bool pauseEnumeratorIsValid;

                    pauseEnumeratorIsValid = pauseEnumerator.MoveNext();
                    sourceEnumeratorIsValid = sourceEnumerator.MoveNext();

                    while (sourceEnumeratorIsValid)
                    {
                        bool addCurrentSourceEntry = true;
                        bool advanceCurrentSourceEntry = true;

                        // Loop to handle all pauses up to this current track point
                        if (pauseEnumeratorIsValid)
                        {
                            if (currentTime >= pauseEnumerator.Current.Lower &&
                                currentTime <= pauseEnumerator.Current.Upper)
                            {
                                addCurrentSourceEntry = false;
                            }
                            else if (currentTime > pauseEnumerator.Current.Upper)
                            {
                                // Advance pause enumerator
                                totalPausedTimeToDate += (pauseEnumerator.Current.Upper - pauseEnumerator.Current.Lower).TotalSeconds;
                                pauseEnumeratorIsValid = pauseEnumerator.MoveNext();

                                // Make sure we retry with the next pause
                                addCurrentSourceEntry = false;
                                advanceCurrentSourceEntry = false;
                            }
                        }

                        if (addCurrentSourceEntry)
                        {
                            result.Add(currentTime - new TimeSpan(0, 0, (int)totalPausedTimeToDate), sourceEnumerator.Current.Value);
                        }

                        if (advanceCurrentSourceEntry)
                        {
                            sourceEnumeratorIsValid = sourceEnumerator.MoveNext();
                            currentTime = sourceTrack.StartTime + new TimeSpan(0, 0, (int)sourceEnumerator.Current.ElapsedSeconds);
                        }
                    }

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Temporary output routine.  Eventually should be deleted.
        /// </summary>
        /// <param name="track"></param>
        internal static void ExportTrack(INumericTimeDataSeries track, string name)
        {
            try
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(name);

                // Write Header
                writer.WriteLine("Seconds, Value");

                foreach (ITimeValueEntry<float> item in track)
                {
                    // Write data
                    writer.WriteLine(item.ElapsedSeconds + ", " + item.Value);
                }

                writer.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Open a popup treelist.
        /// </summary>
        /// <typeparam name="T">The type of items to be listed</typeparam>
        /// <param name="theme">Visual Theme</param>
        /// <param name="items">Items to be listed</param>
        /// <param name="control">The control that the list will appear attached to</param>
        /// <param name="selected">selected item</param>
        /// <param name="selectHandler">Handler that will handle when an item is clicked</param>
        internal static void OpenListPopup<T>(ITheme theme, IList<T> items, System.Windows.Forms.Control control, T selected, TreeListPopup.ItemSelectedEventHandler selectHandler)
        {
            TreeListPopup popup = new TreeListPopup();
            popup.ThemeChanged(theme);
            popup.Tree.Columns.Add(new TreeList.Column());
            popup.Tree.RowData = items;
            if (selected != null)
            {
                popup.Tree.Selected = new object[] { selected };
            }

            popup.ItemSelected += delegate(object sender, TreeListPopup.ItemSelectedEventArgs e)
            {
                if (e.Item is T)
                {
                    selectHandler((T)e.Item, e);
                }
            };
            popup.Popup(control.Parent.RectangleToScreen(control.Bounds));
        }

        /// <summary>
        /// Open a context menu.
        /// </summary>
        /// <param name="theme">Visual Theme</param>
        /// <param name="items">Items to be listed</param>
        /// <param name="mouse"></param>
        /// <param name="selectHandler">Handler that will handle when an item is clicked</param>
        internal static void OpenContextPopup(ITheme theme, ToolStripItemCollection items, MouseEventArgs mouse, ToolStripItemClickedEventHandler selectHandler)
        {
            ContextMenuStrip menuStrip = new ContextMenuStrip();

            menuStrip.Items.AddRange(items);

            menuStrip.ItemClicked += delegate(object sender, ToolStripItemClickedEventArgs e)
            {
                selectHandler(e.ClickedItem, e);
            };
            menuStrip.Show(mouse.Location);
        }

        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        internal static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return constructedString;
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        internal static byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        /// <summary>
        /// Converts an image into an icon.
        /// http://www.dreamincode.net/code/snippet1684.htm
        /// </summary>
        /// <param name="img">The image that shall become an icon</param>
        /// <param name="size">The width and height of the icon. Standard
        /// sizes are 16x16, 32x32, 48x48, 64x64.</param>
        /// <param name="keepAspectRatio">Whether the image should be squashed into a
        /// square or whether whitespace should be put around it.</param>
        /// <returns>An icon!!</returns>
        internal static Icon MakeIcon(Image img, int size, bool keepAspectRatio)
        {
            Bitmap square = new Bitmap(size, size); // create new bitmap
            Graphics g = Graphics.FromImage(square); // allow drawing to it

            int x, y, w, h; // dimensions for new image

            if (!keepAspectRatio || img.Height == img.Width)
            {
                // just fill the square
                x = y = 0; // set x and y to 0
                w = h = size; // set width and height to size
            }
            else
            {
                // work out the aspect ratio
                float r = (float)img.Width / (float)img.Height;

                // set dimensions accordingly to fit inside size^2 square
                if (r > 1)
                { // w is bigger, so divide h by r
                    w = size;
                    h = (int)((float)size / r);
                    x = 0; y = (size - h) / 2; // center the image
                }
                else
                { // h is bigger, so multiply w by r
                    w = (int)((float)size * r);
                    h = size;
                    y = 0; x = (size - w) / 2; // center the image
                }
            }

            // make the image shrink nicely by using HighQualityBicubic mode
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, x, y, w, h); // draw image with specified dimensions
            g.Flush(); // make sure all drawing operations complete before we get the icon

            // following line would work directly on any image, but then
            // it wouldn't look as nice.
            return Icon.FromHandle(square.GetHicon());
        }

        /// <summary>
        /// Rainbow will return a distinct list of colors based on ROYGBIV
        /// </summary>
        /// <param name="totalItems">Number of colors to generate</param>
        /// <returns>Returns a distinct list of colors</returns>
        internal static List<Color> Rainbow(int totalItems, int alpha)
        {
            List<Color> colors = new List<Color>();
            double red;
            double green;
            double blue;
            double scaleFactor;

            // Harshness of the color. Max is 255
            int harshness = 150;

            // Manually add the colors if there are less than 6 items
            if (totalItems == 1)
            {
                colors.Add(Color.FromArgb(alpha, harshness, 0, 0));
            }
            else if (totalItems == 2)
            {
                colors.Add(Color.FromArgb(alpha, harshness, 0, 0));
                colors.Add(Color.FromArgb(alpha, 0, harshness, 0));
            }
            else if (totalItems == 3)
            {
                colors.Add(Color.FromArgb(alpha, harshness, 0, 0));
                colors.Add(Color.FromArgb(alpha, 0, harshness, 0));
                colors.Add(Color.FromArgb(alpha, 0, 0, harshness));
            }
            else if (totalItems == 4)
            {
                colors.Add(Color.FromArgb(alpha, harshness, 0, 0));
                colors.Add(Color.FromArgb(alpha, harshness, harshness, 0));
                colors.Add(Color.FromArgb(alpha, 0, harshness, 0));
                colors.Add(Color.FromArgb(alpha, 0, 0, harshness));
            }
            else if (totalItems == 5)
            {
                colors.Add(Color.FromArgb(alpha, harshness, 0, 0));
                colors.Add(Color.FromArgb(alpha, harshness, harshness, 0));
                colors.Add(Color.FromArgb(alpha, 0, harshness, 0));
                colors.Add(Color.FromArgb(alpha, 0, harshness, harshness));
                colors.Add(Color.FromArgb(alpha, 0, 0, harshness));
            }

            // Make sure we have a multiple of 6 to rainbow
            while (totalItems % 6 != 0)
            {
                totalItems += 1;
            }

            // Find the factor to which we will scale the colors
            scaleFactor = ((double)harshness / totalItems) * 6f;

            // Red is our starting point
            red = harshness;
            green = 0;
            blue = 0;

            // Add red to the list
            colors.Add(Color.FromArgb(alpha, (int)red, (int)green, (int)blue));

            // Work your way through the spectrum to build the colors
            while (green < harshness)
            {
                green += scaleFactor;

                // Catch any potential rounding issues
                if (green > harshness)
                {
                    green = harshness;
                }
                colors.Add(Color.FromArgb(alpha, (int)red, (int)green, (int)blue));
            }

            while (red > 0)
            {
                red -= scaleFactor;

                // Catch any potential rounding issues
                if (red < 0)
                {
                    red = 0;
                }

                colors.Add(Color.FromArgb(alpha, (int)red, (int)green, (int)blue));
            }

            while (blue < harshness)
            {
                blue += scaleFactor;

                // Catch any potential rounding issues
                if (blue > harshness)
                {
                    blue = harshness;
                }

                colors.Add(Color.FromArgb(alpha, (int)red, (int)green, (int)blue));
            }

            while (green > 0)
            {
                green -= scaleFactor;

                // Catch any potential rounding issues
                if (green < 0)
                {
                    green = 0;
                }

                colors.Add(Color.FromArgb(alpha, (int)red, (int)green, (int)blue));
            }

            while (red < harshness)
            {
                red += scaleFactor;

                // Catch any potential rounding issues
                if (red > harshness)
                {
                    red = harshness;
                }

                colors.Add(Color.FromArgb(alpha, (int)red, (int)green, (int)blue));
            }

            while (blue > 0)
            {
                blue -= scaleFactor;

                // Catch any potential rounding issues
                if (blue < 0)
                {
                    blue = 0;
                }

                colors.Add(Color.FromArgb(alpha, (int)red, (int)green, (int)blue));
            }

            // The last color and the first color should be the same.  Remove the last color
            colors.RemoveAt(colors.Count - 1);

            // Return the colors list
            return colors;
        }

        /// <summary>
        /// Create a distance track from an activity's GPS Route
        /// </summary>
        /// <param name="gpsActivity"></param>
        /// <returns>A distance track created from the GPS route</returns>
        public static IDistanceDataTrack CreateDistanceDataTrack(IActivity gpsActivity)
        {
            IDistanceDataTrack distanceTrack = new DistanceDataTrack();

            if (gpsActivity.GPSRoute != null && gpsActivity.GPSRoute.Count > 0)
            {
                float distance = 0;

                // First Point
                distanceTrack.Add(gpsActivity.GPSRoute.StartTime, 0);

                for (int i = 1; i < gpsActivity.GPSRoute.Count; i++)
                {
                    DateTime pointTime = gpsActivity.GPSRoute.StartTime.AddSeconds(gpsActivity.GPSRoute[i].ElapsedSeconds);
                    distance += gpsActivity.GPSRoute[i].Value.DistanceMetersToPoint(gpsActivity.GPSRoute[i - 1].Value);
                    distanceTrack.Add(pointTime, distance);
                }
            }

            return distanceTrack;
        }

        /// <summary>
        /// Create a grade track from an activity's elevation track
        /// </summary>
        /// <param name="activity">Activity that needs an grade track</param>
        /// <returns>A grade track created from the elevation track</returns>
        public static INumericTimeDataSeries GetGradeTrack(IActivity activity)
        {
            INumericTimeDataSeries gradeTrack = new NumericTimeDataSeries();
            INumericTimeDataSeries elevationTrack = new NumericTimeDataSeries();
            INumericTimeDataSeries distanceTrack = new NumericTimeDataSeries();
            elevationTrack = GetElevationTrack(activity);
            distanceTrack = GetDistanceMovingTrack(activity);
            float grade = 0;
            float lastElevation = 0;
            float lastDistance = 0;
            float currentElevation = 0;
            float currentDistance = 0;
            DateTime startTime = activity.StartTime;

            if (gradeTrack != null && elevationTrack != null && distanceTrack != null)
            {
                for (int i = 0; i < elevationTrack.Count; i++)
                {
                    if (i == 0)
                    {
                        gradeTrack.Add(startTime, grade);
                        lastElevation = elevationTrack[i].Value;
                        ITimeValueEntry<float> point = distanceTrack.GetInterpolatedValue(startTime.AddSeconds(elevationTrack[i].ElapsedSeconds));
                        if (point != null)
                        {
                            lastDistance = point.Value;
                        }

                    }
                    else
                    {
                        ITimeValueEntry<float> point = distanceTrack.GetInterpolatedValue(startTime.AddSeconds(elevationTrack[i].ElapsedSeconds));
                        if (point != null)
                        {
                            currentDistance = point.Value;
                        }
                        else
                        {
                            currentDistance = lastDistance;
                        }

                        currentElevation = elevationTrack[i].Value;
                        grade = (currentElevation - lastElevation) / (currentDistance - lastDistance);
                        if (float.IsNaN(grade) || float.IsInfinity(grade))
                        {
                            grade = 0;
                        }
                        gradeTrack.Add(startTime.AddSeconds(elevationTrack[i].ElapsedSeconds), grade);

                        lastDistance = currentDistance;
                        lastElevation = currentElevation;
                    }
                }

                return gradeTrack;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Create an elevation track if one doesn't exists.
        /// </summary>
        /// <param name="activity">Activity that needs an elevation track</param>
        /// <returns>If an elevation track exists, return it.  Otherwise, create it from GPS</returns>
        public static INumericTimeDataSeries GetElevationTrack(IActivity activity)
        {
            if (activity.GPSRoute != null && activity.GPSRoute.Count > 0)
            {
                // Get the track from GPS data
                INumericTimeDataSeries newSeries = new NumericTimeDataSeries();
                for (int i = 0; i < activity.GPSRoute.Count; i++)
                {
                    //DateTime time = activity.StartTime.AddSeconds(activity.GPSRoute[i].ElapsedSeconds);
                    // Changed from activity.StartTime to GPSRoute.EntryDateTime to catch start time differences (old line above)
                    DateTime time = activity.GPSRoute.EntryDateTime(activity.GPSRoute[i]);
                    newSeries.Add(time, activity.GPSRoute[i].Value.ElevationMeters);
                }
                return newSeries;
            }
            else
            {
                // Return the elevation track
                return activity.ElevationMetersTrack;
            }
        }

        // Commented out to use GetDistanceMovingTrack for features.  This code is still valid, I just don't want to confuse myself
        /// <summary>
        /// Create an distance track if one doesn't exists.
        /// </summary>
        /// <param name="activity">Activity that needs an distance track</param>
        /// <returns>If an distance track exists, return it.  Otherwise, create it from GPS</returns>
        //public static INumericTimeDataSeries GetDistanceTrack(IActivity activity)
        //{
        //    // Get the track from GPS data
        //    if (activity.GPSRoute != null && activity.GPSRoute.Count > 0)
        //    {
        //        INumericTimeDataSeries newSeries = new NumericTimeDataSeries();
        //        IGPSPoint lastPoint = null;
        //        float distanceTotal = 0;
        //        for (int i = 0; i < activity.GPSRoute.Count; i++)
        //        {
        //            DateTime time = activity.StartTime.AddSeconds(activity.GPSRoute[i].ElapsedSeconds);

        //            if (i == 0)
        //            {
        //                newSeries.Add(time, distanceTotal);
        //            }
        //            else
        //            {
        //                distanceTotal += activity.GPSRoute[i].Value.DistanceMetersToPoint(lastPoint);
        //                newSeries.Add(time, distanceTotal);
        //            }

        //            lastPoint = activity.GPSRoute[i].Value;
        //        }
        //        return newSeries;
        //    }
        //    else if (activity.DistanceMetersTrack != null)
        //    {
        //        // If this track was joined, there's a chance the distance data is screwy.  I'll fix it as not to worry about it later.
        //        INumericTimeDataSeries newSeries = new NumericTimeDataSeries();
        //        float lastDistance = 0;
        //        float totalDistance = 0;
        //        float deltaDistance = 0;

        //        DateTime time = activity.DistanceMetersTrack.StartTime;

        //        for (int i = 0; i < activity.DistanceMetersTrack.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                totalDistance += activity.DistanceMetersTrack[i].Value;
        //            }
        //            else
        //            {
        //                deltaDistance = activity.DistanceMetersTrack[i].Value - lastDistance;
        //                if (deltaDistance < 0)
        //                {
        //                    // Should i set it to 0 here or to the value of the current track?
        //                    // I lean towards 0 since the track could really have anything in it.
        //                    // I don't want to take the value and it be huge due to joined activities 
        //                    // that were parts of other activities (not starting at distance=0)
        //                    deltaDistance = 0;
        //                }
        //                totalDistance += deltaDistance;
        //            }

        //            newSeries.Add(time.AddSeconds(activity.DistanceMetersTrack[i].ElapsedSeconds), totalDistance);
        //            lastDistance = activity.DistanceMetersTrack[i].Value;
        //        }

        //        // Return the distance track
        //        return newSeries;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Create a speed track from an activity's distance track.
        /// Speed is returned in m/s
        /// </summary>
        /// <param name="activity">Activity that needs an speed track</param>
        /// <returns>A speed track created from the distance track</returns>
        public static INumericTimeDataSeries GetSpeedTrack(IActivity activity)
        {
            INumericTimeDataSeries distanceTrack = new NumericTimeDataSeries();
            distanceTrack = GetDistanceMovingTrack(activity);

            INumericTimeDataSeries speedTrack = new NumericTimeDataSeries();

            float speed = 0;
            float lastTime = 0;
            float lastDistance = 0;
            float currentTime = 0;
            float currentDistance = 0;
            DateTime startTime = activity.StartTime;

            for (int i = 0; i < distanceTrack.Count; i++)
            {
                if (i == 0)
                {
                    speedTrack.Add(startTime, speed);
                    lastTime = distanceTrack[i].ElapsedSeconds;
                    lastDistance = distanceTrack[i].Value;
                }
                else
                {
                    currentTime = distanceTrack[i].ElapsedSeconds;
                    currentDistance = distanceTrack[i].Value;

                    speed = (currentDistance - lastDistance) / (currentTime - lastTime);
                    speedTrack.Add(startTime.AddSeconds(distanceTrack[i].ElapsedSeconds), speed);
                    lastTime = currentTime;
                    lastDistance = currentDistance;
                }
            }

            return speedTrack;
        }

        /// <summary>
        /// Convert a track data from meters to (toUnits)
        /// </summary>
        /// <param name="track">Input track</param>
        /// <param name="toUnits">Desired units</param>
        /// <returns>Converted track</returns>
        public static INumericTimeDataSeries ConvertDistanceUnits(INumericTimeDataSeries track, Length.Units toUnits)
        {
            INumericTimeDataSeries result = new NumericTimeDataSeries(track);

            foreach (TimeValueEntry<float> item in result)
            {
                // Convert to proper distance units from m/s
                item.Value = (float)Length.Convert(item.Value, Length.Units.Meter, toUnits) * Common.SecondsPerHour;
            }

            return result;
        }

        /// <summary>
        /// Convert a track from speed to pace.  Length units are maintained.  Assumes input is in hours (mph, km/hr), and converts to seconds base (sec/mile, sec/km).
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static INumericTimeDataSeries SpeedToPace(INumericTimeDataSeries track)
        {
            INumericTimeDataSeries result = new NumericTimeDataSeries(track);

            foreach (TimeValueEntry<float> item in result)
            {
                // Convert to pace if required
                item.Value = (float)Utilities.SpeedToPace(item.Value) * Common.SecondsPerMinute;
            }

            return result;
        }

        /// <summary>
        /// Multiple each value in a data series by 100.
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static INumericTimeDataSeries GetPercentTrack(INumericTimeDataSeries track)
        {
            if (track == null || track.Count == 0)
            {
                return track;
            }

            NumericTimeDataSeries percent = new NumericTimeDataSeries(track);

            foreach (TimeValueEntry<float> point in percent)
            {
                point.Value = point.Value * 100f;
            }

            return percent;
        }

        public static INumericTimeDataSeries SetTrackStartValueToZero(INumericTimeDataSeries inTrack)
        {
            if (inTrack == null || inTrack.Count == 0)
            {
                return inTrack;
            }

            NumericTimeDataSeries newTrack = new NumericTimeDataSeries(inTrack);
            float firstPoint = inTrack[0].Value;

            foreach (TimeValueEntry<float> point in newTrack)
            {
                point.Value = point.Value - firstPoint;
            }

            return newTrack;
        }

        /// <summary>
        /// Convert a speed (length/hour) value, to pace (minutes/length).
        /// Distance units (mile, km, etc.) are retained.
        /// </summary>
        /// <param name="speed">Speed value in distance PER HOUR</param>
        /// <returns>Minutes / Length</returns>
        public static double SpeedToPace(double speed)
        {
            return Common.SecondsPerMinute / speed;
        }

        public static INumericTimeDataSeries GetSectionOfTrack(INumericTimeDataSeries track, DateTime start, DateTime end)
        {
            if (track != null && track.Count > 0)
            {
                INumericTimeDataSeries newSeries = new NumericTimeDataSeries();
                for (int i = 0; i < track.Count; i++)
                {
                    if (track.StartTime.AddSeconds(track[i].ElapsedSeconds) >= start
                        && track.StartTime.AddSeconds(track[i].ElapsedSeconds) <= end)
                    {
                        newSeries.Add(track.StartTime.AddSeconds(track[i].ElapsedSeconds), track[i].Value);
                    }
                    else if (track.StartTime.AddSeconds(track[i].ElapsedSeconds) >= end)
                    {
                        break;
                    }
                }
                return newSeries;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Create an distance track if one doesn't exists.
        /// </summary>
        /// <param name="activity">Activity that needs an distance track</param>
        /// <returns>If an distance track exists, return it.  Otherwise, create it from GPS</returns>
        public static INumericTimeDataSeries GetDistanceMovingTrack(IActivity activity)
        {
            ActivityInfo ai = ActivityInfoCache.Instance.GetInfo(activity);
            // Get the track from GPS data
            if (ai.MovingDistanceMetersTrack != null && ai.MovingDistanceMetersTrack.Count > 0)
            {
                // If this track was joined, there's a chance the distance data is screwy.  I'll fix it as not to worry about it later.
                INumericTimeDataSeries newSeries = new NumericTimeDataSeries();
                float lastDistance = 0;
                float totalDistance = 0;
                float deltaDistance = 0;

                DateTime time = ai.MovingDistanceMetersTrack.StartTime;

                for (int i = 0; i < ai.MovingDistanceMetersTrack.Count; i++)
                {
                    if (i == 0)
                    {
                        totalDistance += ai.MovingDistanceMetersTrack[i].Value;
                    }
                    else
                    {
                        deltaDistance = ai.MovingDistanceMetersTrack[i].Value - lastDistance;
                        if (deltaDistance < 0)
                        {
                            // Should i set it to 0 here or to the value of the current track?
                            // I lean towards 0 since the track could really have anything in it.
                            // I don't want to take the value and it be huge due to joined activities 
                            // that were parts of other activities (not starting at distance=0)
                            deltaDistance = 0;
                        }
                        totalDistance += deltaDistance;
                    }

                    newSeries.Add(time.AddSeconds(ai.MovingDistanceMetersTrack[i].ElapsedSeconds), totalDistance);
                    lastDistance = ai.MovingDistanceMetersTrack[i].Value;
                }

                // Return the distance track
                return newSeries;
            }
            else if (activity.DistanceMetersTrack != null)
            {
                // If this track was joined, there's a chance the distance data is screwy.  I'll fix it as not to worry about it later.
                INumericTimeDataSeries newSeries = new NumericTimeDataSeries();
                float lastDistance = 0;
                float totalDistance = 0;
                float deltaDistance = 0;

                DateTime time = activity.DistanceMetersTrack.StartTime;

                for (int i = 0; i < activity.DistanceMetersTrack.Count; i++)
                {
                    if (i == 0)
                    {
                        totalDistance += activity.DistanceMetersTrack[i].Value;
                    }
                    else
                    {
                        deltaDistance = activity.DistanceMetersTrack[i].Value - lastDistance;
                        if (deltaDistance < 0)
                        {
                            // Should i set it to 0 here or to the value of the current track?
                            // I lean towards 0 since the track could really have anything in it.
                            // I don't want to take the value and it be huge due to joined activities 
                            // that were parts of other activities (not starting at distance=0)
                            deltaDistance = 0;
                        }
                        totalDistance += deltaDistance;
                    }

                    newSeries.Add(time.AddSeconds(activity.DistanceMetersTrack[i].ElapsedSeconds), totalDistance);
                    lastDistance = activity.DistanceMetersTrack[i].Value;
                }

                // Return the distance track
                return newSeries;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Create a vam track from an activity's elevation track.
        /// vam is returned in Vm/h
        /// </summary>
        /// <param name="activity">Activity that needs an vam track</param>
        /// <returns>A vam track created from the elevation track</returns>
        public static INumericTimeDataSeries GetVAMTrack(IActivity activity)
        {
            INumericTimeDataSeries vamTrack = new NumericTimeDataSeries();
            INumericTimeDataSeries elevationTrack = new NumericTimeDataSeries();
            elevationTrack = GetElevationTrack(activity);
            float vam = 0;
            float lastElevation = 0;
            float currentElevation = 0;

            DateTime startTime = elevationTrack.StartTime;
            uint currentSeconds = 0;
            uint lastSeconds = 0;

            if (vamTrack != null && elevationTrack != null)
            {
                /*for (int i = 0; i < elevationTrack.Count; i++)
                {
                    vam = ((elevationTrack[i].Value - startElevation) * 60f * 60f) / elevationTrack[i].ElapsedSeconds;
                    if (vam < 0 || float.IsNaN(vam) || float.IsNegativeInfinity(vam) || float.IsPositiveInfinity(vam))
                    {
                        vam = 0;
                    }
                    vamTrack.Add(startTime.AddSeconds(elevationTrack[i].ElapsedSeconds), vam);
                }*/

                for (int i = 0; i < elevationTrack.Count; i++)
                {
                    if (i == 0)
                    {
                        vamTrack.Add(startTime, vam);
                        lastElevation = elevationTrack[i].Value;
                        lastSeconds = elevationTrack[i].ElapsedSeconds;
                    }
                    else
                    {
                        currentElevation = elevationTrack[i].Value;
                        currentSeconds = elevationTrack[i].ElapsedSeconds;
                        vam = ((currentElevation - lastElevation) * 60f * 60f) / (currentSeconds - lastSeconds);
                        if (vam < 0 || float.IsNaN(vam) || float.IsNegativeInfinity(vam) || float.IsPositiveInfinity(vam))
                        {
                            vam = 0;
                        }
                        vamTrack.Add(startTime.AddSeconds(elevationTrack[i].ElapsedSeconds), vam);
                        lastSeconds = currentSeconds;
                        lastElevation = currentElevation;
                    }
                }

                return vamTrack;
            }
            else
            {
                return null;
            }
        }

        public static INumericTimeDataSeries RemoveBadTrackData(INumericTimeDataSeries track, double lowValue, double highValue)
        {
            if (track != null && track.Count > 0)
            {
                INumericTimeDataSeries newSeries = new NumericTimeDataSeries();
                for (int i = 0; i < track.Count; i++)
                {
                    // If the track data is in the high/low bounds, add it to the new series
                    if (track[i].Value < highValue && track[i].Value > lowValue)
                    {
                        newSeries.Add(track.StartTime.AddSeconds(track[i].ElapsedSeconds), track[i].Value);
                    }
                }
                return newSeries;
            }
            else
            {
                return null;
            }
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
    }
}
