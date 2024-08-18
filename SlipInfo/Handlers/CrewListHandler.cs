using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using SlipInfo.Responses;

namespace SlipInfo.Handlers
{
    class CrewListHandler : InfoHandler
    {
        public string GetPath()
        {
            return "getCrew";
        }

        public InfoResponse HandleRequest(NameValueCollection query)
        {
            MpSvc mpSvc = Svc.Get<MpSvc>();
            if (mpSvc == null) {
                Plugin.Log.LogError("An error occurred in CrewListHandler! mpSvc was null!");
                return new InfoResponse("{\"error\": \"An error occurred in CrewListHandler! mpSvc was null!\"}", HttpStatusCode.InternalServerError);
            } else if (mpSvc.Crew == null) {
                Plugin.Log.LogError("An error occurred in CrewListHandler! MpCrewController was null!");
                return new InfoResponse("{\"error\": \"An error occurred in CrewListHandler: MpCrewController was null!\"}", HttpStatusCode.InternalServerError);
            }

            CrewListResponse response = new CrewListResponse(mpSvc.Crew.CrewmatesOnBoard);

            try
            {
                string json = JsonConvert.SerializeObject(response);

                Plugin.debugLogInfo("Returning crew list.");
                return new InfoResponse(json, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"An error occurred in CrewListHandler: {ex.Message}");
                return new InfoResponse($"{{\"error\": \"An error occured in CrewListHandler: {ex.Message}\"}}", HttpStatusCode.InternalServerError);
            }


            
        }
    }
}
