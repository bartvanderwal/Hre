using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace HRE.Common {

    public class TransferToRouteResult : ActionResult {
        public string RouteName { get;set; }
        public RouteValueDictionary RouteValues { get; set; }

        public TransferToRouteResult(RouteValueDictionary routeValues)
            : this(null, routeValues) { }


        public TransferToRouteResult(string routeName, RouteValueDictionary routeValues) {
            this.RouteName = routeName ?? string.Empty;
            this.RouteValues = routeValues ?? new RouteValueDictionary();
        }


        public override void ExecuteResult(ControllerContext context) {
            if (context == null)
                throw new ArgumentNullException("context");

            var urlHelper = new UrlHelper(context.RequestContext);
            var url = urlHelper.RouteUrl(this.RouteName, this.RouteValues);

            var actualResult = new MvcTransferResult(url);
            actualResult.ExecuteResult(context);
        }
    }

}