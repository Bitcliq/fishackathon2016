using Bitcliq.BIR.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bitcliq.BIR.Portal
{
    /// <summary>
    /// Summary description for GetImage
    /// </summary>
    public class GetImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            if (context.Request["id"] + "" != "")
            {
                try
                {
                    Issue i = new Issue(Convert.ToInt32(context.Request["id"]));

                    if(i.ID > 0)
                    {
                        IssueJson ij = new IssueJson(i, false);

                        context.Response.Write(ij.PhotoThumbnailUrl);
                    }
                    
                }
                catch (Exception ex)
                {
                    //context.Response.Write()
                    //{
                    //}
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}