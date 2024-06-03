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
                    return new InfoResponse("{\"error\": \"An error occurred handling ship info. null MpSvc.\"}", HttpStatusCode.InternalServerError);
                }

                MpShipController mpShipController = mpSvc.Ships;

                if (mpShipController == null)
                {
                    return new InfoResponse("{\"error\": \"An error occurred handling ship info. null MpShipController.\"}", HttpStatusCode.InternalServerError);
                }

                ShipInfo info = new ShipInfo(mpShipController);

                string json = JsonConvert.SerializeObject(info);

                return new InfoResponse(json, HttpStatusCode.OK);
            } catch (Exception ex)
            {
                return new InfoResponse($"{{\"error\": \"An exception occurred handling ship info. {ex.Message}\"}}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
