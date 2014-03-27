using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HRE.Attributes {

        public class RequiresSsl : ActionFilterAttribute {
        
            public override void OnActionExecuting(ActionExecutingContext filterContext) {
            HttpRequestBase req = filterContext.HttpContext.Request;
            HttpResponseBase res = filterContext.HttpContext.Response;

            //Check if secure is required if we're on the localhost.
            if (!req.IsSecureConnection) {
                var builder = new UriBuilder(req.Url) {
                    Scheme = Uri.UriSchemeHttps,
                    Port = req.IsLocal ? 59162 : 443
                };
                res.Redirect(builder.Uri.ToString());
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
