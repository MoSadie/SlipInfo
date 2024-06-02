using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json;

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
                return new InfoResponse("{\"error\": \"An error occurred in CrewListHandler! mpSvc was null!\"}", HttpStatusCode.InternalServerError);
            } else if (mpSvc.Crew == null) {
                return new InfoResponse("{\"error\": \"An error occurred in CrewListHandler: MpCrewController was null!\"}", HttpStatusCode.InternalServerError);
            }

            CrewListResponse response = new CrewListResponse(mpSvc.Crew.CrewmatesOnBoard);

            try
            {
                string json = JsonConvert.SerializeObject(response);

                return new InfoResponse(json, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new InfoResponse($"{{\"error\": \"An error occured in CrewListHandler: {ex.Message}\"}}", HttpStatusCode.InternalServerError);
            }


            
        }
    }
}
