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
                    return new InfoResponse("{\"error\": \"An error occurred in RunInfoHandler! mpSvc was null!\"}", HttpStatusCode.InternalServerError);
                }
                else if (mpSvc.Campaigns == null)
                {
                    return new InfoResponse("{\"error\": \"An error occurred in RunInfoHandler: MpCampaignController was null!\"}", HttpStatusCode.InternalServerError);
                }

                RunInfo info = new RunInfo(mpSvc.Campaigns);

                string json = JsonConvert.SerializeObject(info);

                return new InfoResponse(json, HttpStatusCode.OK);

            } catch (Exception ex)
            {
                return new InfoResponse($"{{\"error\": \"An exception occurred handling getting run info. {ex.Message}\"}}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
