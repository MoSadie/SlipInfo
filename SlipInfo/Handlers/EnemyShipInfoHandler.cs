using Newtonsoft.Json;
using SlipInfo.Responses;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace SlipInfo.Handlers
{
    class EnemyShipInfoHandler : InfoHandler
    {
        public string GetPath()
        {
            return "getEnemyShipInfo";
        }

        public InfoResponse HandleRequest(NameValueCollection query)
        {
            try
            {
                MpSvc mpSvc = Svc.Get<MpSvc>();

                if (mpSvc == null)
                {
                    return new InfoResponse("{\"error\": \"An error occurred handling getting enemy ship info. null MpSvc.\"}", HttpStatusCode.InternalServerError);
                }

                MpShipController mpShipController = mpSvc.Ships;
                MpScenarioController mpScenarioController = mpSvc.Scenarios;

                if (mpShipController == null)
                {
                    return new InfoResponse("{\"error\": \"An error occurred handling getting enemy ship info. null MpShipController.\"}", HttpStatusCode.InternalServerError);
                }

                if (mpScenarioController == null)
                {
                    return new InfoResponse("{\"error\": \"An error occurred handling getting enemy ship info. null MpScenarioController.\"}", HttpStatusCode.InternalServerError);
                }

                EnemyShipResponse response = new EnemyShipResponse(mpShipController, mpScenarioController);

                string json = JsonConvert.SerializeObject(response);

                Plugin.debugLogInfo("Returning enemy ship info.");
                return new InfoResponse(json, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"An exception occurred handling getting enemy ship info. {ex.Message}");
                return new InfoResponse($"{{\"error\": \"An exception occurred handling getting enemy ship info. {ex.Message}\"}}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
