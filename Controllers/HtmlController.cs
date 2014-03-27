using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRE.Models;
using HRE.Business;

namespace HRE.Controllers {
    public class HtmlController : BaseController {

        public ActionResult Index() {
            string path = Url.Content("~/Content/html/index.html");
            return Redirect(path);
        }


        // Alias.
        public ActionResult Rest(string urlPart) {
            return new FilePathResult(urlPart, "text/html");
        }

        
        public ActionResult GetHtml() {
            string path = Request.FilePath;
            var result = new FilePathResult("~/"+ path + "/index.html", "text/html");
            return result;
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderAndOrFile">Pathname; for instance "/Solution/Html"/</param>
        public void GetHtml(string folderOrFile) {
            var encoding = new System.Text.UTF8Encoding();
            var htm = System.IO.File.ReadAllText(Server.MapPath(folderOrFile) + "index.html", encoding);
            byte[] data = encoding.GetBytes(htm);
            Response.OutputStream.Write(data, 0, data.Length);
            Response.OutputStream.Flush();
        }
         * */
    }
}
