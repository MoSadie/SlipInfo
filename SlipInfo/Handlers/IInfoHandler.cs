using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace SlipInfo.Handlers
{
    internal interface IInfoHandler
    {
        public InfoResponse HandleRequest(NameValueCollection query);

        public string GetPath();
    }

    public class InfoResponse
    {
        public string response;
        public HttpStatusCode status;

        public InfoResponse(string response, HttpStatusCode status)
        {
            this.response = response;
            this.status = status;
        }
    }
}
