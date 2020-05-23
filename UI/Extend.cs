// <copyright file="ExtendActions.cs" company="N/A">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>mechgt</author>
// <email>mechgt@gmail.com</email>
// <date>2008-12-23</date>
namespace CourseScore.UI
{
    using System.Collections.Generic;
    using ZoneFiveSoftware.Common.Data.Fitness;
    using ZoneFiveSoftware.Common.Visuals;
    using ZoneFiveSoftware.Common.Visuals.Fitness;

    class Extend : IExtendViews, IExtendActivityDetailPages
    {
        #region IExtendViews Members

        public IList<IView> Views
        {
            get
            {
                return null;
            }
        }

        #endregion

        #region IExtendActivityDetailPages Members

        public IList<IDetailPage> GetDetailPages(IDailyActivityView view, ExtendViewDetailPages.Location location)
        {
            IList<IDetailPage> list = new List<IDetailPage>();
            list.Add(new DetailPage.HillsDetail(view));
            
            return list;
        }

        #endregion
    }
}
