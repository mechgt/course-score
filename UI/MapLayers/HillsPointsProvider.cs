using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Visuals.Mapping;

namespace CourseScore.UI.MapLayers
{
    class HillsPointsProvider : IRouteControlLayerProvider
    {
        private IRouteControlLayer layer = null;

        public IRouteControlLayer RouteControlLayer
        {
            get { return layer; }
        }

        #region IRouteControlLayerProvider Members

        public IRouteControlLayer CreateControlLayer(IRouteControl control)
        {
            if (layer == null)
            {
                layer = new HillsPointsLayer(this, control);
            }
            return layer;
        }

        public Guid Id
        {
            get { return GUIDs.HillsPointsProvider; }
        }

        public string Name
        {
            get
            {
                return "Course Score";
            }
        }

        #endregion
    }
}
