using Newtonsoft.Json;
using SlipInfo.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace SlipInfo.Handlers
{
    class RunInfoHandler : InfoHandler
    {
        public string GetPath()
        {
            return "getRunInfo";
        }

        public InfoResponse HandleRequest(NameValueCollection query)
        {
            try
            {
                MpSvc mpSvc = Svc.Get<MpSvc>();

                if (mpSvc == null)
                {
                    Plugin.Log.LogError("An error occurred in RunInfoHandler! mpSvc was null!");
                    return new InfoResponse("{\"error\": \"An error occurred in RunInfoHandler! mpSvc was null!\"}", HttpStatusCode.InternalServerError);
                }
                else if (mpSvc.Campaigns == null)
                {
                    Plugin.Log.LogError("An error occurred in RunInfoHandler! MpCampaignController was null!");
                    return new InfoResponse("{\"error\": \"An error occurred in RunInfoHandler: MpCampaignController was null!\"}", HttpStatusCode.InternalServerError);
                }

                RunInfo info = new RunInfo(mpSvc.Campaigns);

                string json = JsonConvert.SerializeObject(info);

                Plugin.Log.LogInfo("Returning run info.");
                return new InfoResponse(json, HttpStatusCode.OK);

            } catch (Exception ex)
            {
                Plugin.Log.LogError($"An exception occurred handling getting run info. {ex.Message}");
                return new InfoResponse($"{{\"error\": \"An exception occurred handling getting run info. {ex.Message}\"}}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
