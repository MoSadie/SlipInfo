using Newtonsoft.Json;
using SlipInfo.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace SlipInfo.Handlers
{
    class ShipInfoHandler : InfoHandler
    {
        public string GetPath()
        {
            return "getShipInfo";
        }

        public InfoResponse HandleRequest(NameValueCollection query)
        {
            try
            {
                MpSvc mpSvc = Svc.Get<MpSvc>();

                if (mpSvc == null)
                {
                    Plugin.Log.LogError("An error occurred in ShipInfoHandler! mpSvc was null!");
                    return new InfoResponse("{\"error\": \"An error occurred handling ship info. null MpSvc.\"}", HttpStatusCode.InternalServerError);
                }

                MpShipController mpShipController = mpSvc.Ships;

                if (mpShipController == null)
                {
                    Plugin.Log.LogError("An error occurred in ShipInfoHandler! MpShipController was null!");
                    return new InfoResponse("{\"error\": \"An error occurred handling ship info. null MpShipController.\"}", HttpStatusCode.InternalServerError);
                }

                ShipInfo info = new ShipInfo(mpShipController);

                string json = JsonConvert.SerializeObject(info);

                Plugin.Log.LogInfo("Returning ship info.");
                return new InfoResponse(json, HttpStatusCode.OK);
            } catch (Exception ex)
            {
                Plugin.Log.LogError($"An exception occurred handling ship info. {ex.Message}");
                return new InfoResponse($"{{\"error\": \"An exception occurred handling ship info. {ex.Message}\"}}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
