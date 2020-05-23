// <copyright file="Plugin.cs" company="N/A">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>mechgt</author>
// <email>mechgt@gmail.com</email>
// <date>2008-12-23</date>
namespace CourseScore
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using ZoneFiveSoftware.Common.Data.Fitness;
    using ZoneFiveSoftware.Common.Visuals.Fitness;
    using CourseScore.UI;
    using CourseScore.Settings;
    using CourseScore.Data;

    /// <summary>
    /// Plugin class provides interface to the main application and logbook.
    /// </summary>
    class PluginMain : IPlugin
    {
        #region Private fields

        private static IApplication application;
        private static ILogbook logbook;

        #endregion

        /// <summary>
        /// Plugin product Id as listed in license application
        /// </summary>
        internal static string ProductId
        {
            get
            {
                return "cs";
            }
        }

        /// <summary>
        /// Number of days in history that data will be displayed.
        /// This is the amount of data that will be displayed, not a number of days for a trial version.
        /// </summary>
        internal static int EvalDays
        {
            get { return 30; }
        }

        /// <summary>
        /// E-mail address shown to user related to activation
        /// </summary>
        internal static string SupportEmail
        {
            get
            {
                return "mechgt@gmail.com";
            }
        }

        #region IPlugin Members

        /// <summary>
        /// Sets interface to SportTracks application
        /// </summary>
        public IApplication Application
        {
            set { application = value; }
        }

        /// <summary>
        /// Gets plugin GUID
        /// </summary>
        public Guid Id
        {
            // This GUID is currently being used to store/retreive settings data from logbook
            get { return GUIDs.PluginMain; }
        }

        /// <summary>
        /// Gets plugin Name
        /// </summary>
        public string Name
        {
            get
            {
                return "Course Score";
            }
        }

        /// <summary>
        /// Gets plugin Version
        /// </summary>
        public string Version
        {
            get { return GetType().Assembly.GetName().Version.ToString(3); }
        }

        public void ReadOptions(XmlDocument xmlDoc, XmlNamespaceManager nsmgr, XmlElement pluginNode)
        {
            try
            {
                // Deserialization
                GlobalSettings settings;
                XmlSerializer xs = new XmlSerializer(typeof(GlobalSettings));
                MemoryStream memoryStream = new MemoryStream(Utilities.StringToUTF8ByteArray(pluginNode.InnerText));

                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                object deserialize = xs.Deserialize(memoryStream);

                settings = (GlobalSettings)deserialize;
            }
            catch
            { }

        }

        public void WriteOptions(XmlDocument xmlDoc, XmlElement pluginNode)
        {
            // Serialization
            string xmlizedString;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(GlobalSettings));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, GlobalSettings.Instance);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            xmlizedString = Utilities.UTF8ByteArrayToString(memoryStream.ToArray());

            pluginNode.InnerText = xmlizedString;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Main SportTracks application
        /// </summary>
        /// <returns>Returns application interface</returns>
        internal static IApplication GetApplication()
        {
            return application;
        }

        /// <summary>
        /// Currently loaded logbook
        /// </summary>
        /// <returns>Returns currently loaded logbook</returns>
        internal static ILogbook GetLogbook()
        {
            logbook = GetApplication().Logbook;
            return logbook;
        }

        #endregion
    }
}
