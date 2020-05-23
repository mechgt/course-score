using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Visuals;

namespace CourseScore.Data
{
    /// <summary>
    /// Record Set as TreeListNode for display on a TreeList
    /// </summary>
    class RecordSetWrapper : TreeList.TreeListNode
    {
        #region Fields

        private RecordSet recSet;

        #endregion

        #region Constructor

        public RecordSetWrapper(RecordSetWrapper parent, RecordSet element)
            : base(parent, element)
        {
            recSet = element;
        }

        #endregion

        #region Properties

        public string Rank
        {
            get { return ((RecordSet)Element).CategoryName; }
        }

        public RecordSet RecSet
        {
            get { return recSet; }
        }

        #endregion
    }
}
