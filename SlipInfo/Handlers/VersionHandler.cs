using Newtonsoft.Json;
using SlipInfo.Responses;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace SlipInfo.Handlers
{
    internal class VersionHandler : InfoHandler
    {
        public string GetPath()
        {
            return "version";
        }

        public InfoResponse HandleRequest(NameValueCollection query)
        {
            VersionResponse response = new VersionResponse(typeof(VersionHandler).Assembly.GetName().Version);

            string json = JsonConvert.SerializeObject(response);

            Plugin.Log.LogInfo("Returning SlipInfo version info.");
            return new InfoResponse(json, HttpStatusCode.OK);
        }
    }
}
