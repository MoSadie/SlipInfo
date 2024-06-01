using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace SlipInfo.Handlers
{
    internal interface InfoHandler
    {
        public InfoResponse HandleRequest(NameValueCollection query);

        public string GetPath();
    }

    public class InfoResponse
    {
        public string response;
        public int status;

        public InfoResponse(string response, int status)
        {
            this.response = response;
            this.status = status;
        }
    }
}
