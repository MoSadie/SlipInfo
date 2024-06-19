using Newtonsoft.Json;
using SlipInfo.Responses;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace SlipInfo.Handlers
{
    class CrewSelfHandler : InfoHandler
    {
        public string GetPath()
        {
            return "getSelf";
        }

        public InfoResponse HandleRequest(NameValueCollection query)
        {
            MpSvc mpSvc = Svc.Get<MpSvc>();

            if (mpSvc == null)
            {
                Plugin.Log.LogError("An error occurred handling self crew. null MpSvc.");
                return new InfoResponse("{\"error\": \"An error occurred handling self crew. null MpSvc.\"}", HttpStatusCode.InternalServerError);
            }

            MpCrewController mpCrewController = mpSvc.Crew;

            if (mpCrewController == null)
            {
                Plugin.Log.LogError("An error occurred handling self crew. null MpCrewController.");
                return new InfoResponse("{\"error\": \"An error occurred handling self crew. null MpCrewController.\"}", HttpStatusCode.InternalServerError);
            }

            Crewmate self = null;

            foreach (Crewmate potentialCrewmate in mpCrewController.CrewmatesOnBoard)
            {
                if (potentialCrewmate.IsLocalPlayer)
                {
                    self = potentialCrewmate;
                    break;
                }
            }

            if (self == null)
            {
                Plugin.Log.LogError("Could not find the local crewmember. (CrewSelfHandler)");
                return new InfoResponse("{\"error\": \"Could not find the local crewmember.\"}", HttpStatusCode.InternalServerError);
            }

            try
            {
                string json = JsonConvert.SerializeObject(new CrewResponse(self));

                Plugin.Log.LogInfo("Returning self crewmate info.");
                return new InfoResponse(json, HttpStatusCode.OK);
            } catch (Exception ex)
            {
                Plugin.Log.LogError($"An exception occurred handling self crew. {ex.Message}");
                return new InfoResponse($"{{\"error\": \"An exception occurred handling self crew. {ex.Message}\"}}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
