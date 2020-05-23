using System;
using System.Collections.Generic;
using System.Text;

using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Visuals.Mapping;

namespace CourseScore.UI.MapLayers
{
    class ExtendRouteControl : IExtendRouteControlLayerProviders
    {
        #region IExtendRouteControlLayerProviders Members

        public IList<IRouteControlLayerProvider> RouteControlLayerProviders
        {
            get 
            {
                return new IRouteControlLayerProvider[] { new HillsPointsProvider()  };
            }
        }

        #endregion
    }
}
