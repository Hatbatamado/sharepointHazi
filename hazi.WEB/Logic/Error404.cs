using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class Error404
    {
        /// <summary>
        /// 404-es hibakód dobása
        /// </summary>
        /// <param name="Response"></param>
        public static void HibaDobas(HttpResponse Response)
        {
            Response.Clear();
            Response.StatusCode = 404;
            Response.End();
        }
    }
}