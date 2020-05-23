using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness.CustomData;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using System.Diagnostics;
using ZoneFiveSoftware.Common.Data.Measurement;

namespace CourseScore.Data
{
    class CustomDataFields
    {
        private static bool warningMsgBadField;

        public enum CSCustomFields
        {
            CourseScoreCycling, 
            ScoreDistanceCycling, 
            CourseScoreRunning, 
            ScoreDistanceRunning
        }

        /// <summary>
        /// Get a Training Load related custom parameter
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static ICustomDataFieldDefinition GetCustomProperty(CSCustomFields field)
        {
            // All data types so far are numbers
            ICustomDataFieldDefinition fieldDef = null;
            ICustomDataFieldDataType dataType = CustomDataFieldDefinitions.StandardDataType(CustomDataFieldDefinitions.StandardDataTypes.NumberDataTypeId);
            ICustomDataFieldObjectType objType;

            Guid id;
            string name;
            string options = "";

            switch (field)
            {
                case CSCustomFields.CourseScoreCycling:
                    objType = CustomDataFieldDefinitions.StandardObjectType(typeof(IActivity));
                    id = GUIDs.customCSCycling;
                    name = Resources.Strings.Label_CourseScore 
                            + ": " + Resources.Strings.Label_Cycling;
                    fieldDef = GetCustomProperty(dataType, objType, id, name);
                    options = "2";
                    break;

                case CSCustomFields.CourseScoreRunning:
                    objType = CustomDataFieldDefinitions.StandardObjectType(typeof(IActivity));
                    id = GUIDs.customCSRunning;
                    name = Resources.Strings.Label_CourseScore 
                            + ": " + Resources.Strings.Label_Running;
                    fieldDef = GetCustomProperty(dataType, objType, id, name);
                    break;

                case CSCustomFields.ScoreDistanceCycling:
                    objType = CustomDataFieldDefinitions.StandardObjectType(typeof(IActivity));
                    id = GUIDs.customCSCyclingDistance;
                    name = Resources.Strings.Label_CourseScore + "/" + 
                            Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.DistanceUnits)
                            + ": " + Resources.Strings.Label_Cycling;
                    fieldDef = GetCustomProperty(dataType, objType, id, name);
                    break;

                case CSCustomFields.ScoreDistanceRunning:
                    objType = CustomDataFieldDefinitions.StandardObjectType(typeof(IActivity));
                    id = GUIDs.customCSRunningDistance;
                    name = Resources.Strings.Label_CourseScore + "/" + 
                            Length.LabelAbbr(PluginMain.GetApplication().SystemPreferences.DistanceUnits)
                            + ": " +Resources.Strings.Label_Running;
                    fieldDef = GetCustomProperty(dataType, objType, id, name);
                    break;
            }

            return fieldDef;
        }

        /// <summary>
        /// Private helper to dig the logbook for a custom parameter
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static ICustomDataFieldDefinition GetCustomProperty(ICustomDataFieldDataType dataType, ICustomDataFieldObjectType objType, Guid id, string name)
        {
            // Dig all of the existing custom params looking for a match.
            foreach (ICustomDataFieldDefinition customDef in PluginMain.GetLogbook().CustomDataFieldDefinitions)
            {
                if (customDef.Id == id)
                {
                    // Is this really necessary...?
                    if (customDef.DataType != dataType)
                    {
                        // Invalid match found!!! Bad news.
                        // This might occur if a user re-purposes a field.
                        if (!warningMsgBadField)
                        {
                            warningMsgBadField = true;
                            MessageDialog.Show("Invalid " + name + " Custom Field.  Was this field data type modified? (" + customDef.Name + ")", Resources.Strings.Label_CourseScore);
                        }

                        return null;
                    }
                    else
                    {
                        // Return custom def
                        return customDef;
                    }
                }
            }

            // No match found, create it
            return PluginMain.GetLogbook().CustomDataFieldDefinitions.Add(id, objType, dataType, name);
        }

    }
}
