﻿using Newtonsoft.Json;
using SlipInfo.Responses;
using Subpixel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace SlipInfo.Handlers
{
    class CrewSearchHandler : InfoHandler
    {
        public string GetPath()
        {
            return "getCrewByUsername";
        }

        public InfoResponse HandleRequest(NameValueCollection query)
        {
            if (query == null || query.Get("username") == null)
            {
                Plugin.debugLogError("Invalid query! (CrewSearchHandler)");
                return new InfoResponse("{\"error\": \"Invalid query!\"}", HttpStatusCode.BadRequest);
            }

            CrewResponse response = new CrewResponse(query.Get("username"));

            if (response.crewmate == null)
            {
                Plugin.debugLogError("User not found! (CrewSearchHandler)");
                return new InfoResponse("{\"error\": \"User not found!\"}", HttpStatusCode.NotFound);
            }

            try
            {
                string json = JsonConvert.SerializeObject(response);

                Plugin.debugLogInfo("Returning crewmate info.");
                return new InfoResponse(json, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"An error occurred in CrewSearchHandler: {ex.Message}");
                return new InfoResponse($"{{\"error\": \"An error occured in CrewSearchHandler: {ex.Message}\"}}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
