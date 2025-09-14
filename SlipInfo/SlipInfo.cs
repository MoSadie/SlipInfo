using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using BepInEx.Logging;
using UnityEngine;
using System;
using System.Net;
using SlipInfo.Handlers;
using MoCore;
using System.Threading;

namespace SlipInfo
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.mosadie.mocore", BepInDependency.DependencyFlags.HardDependency)]
    [BepInProcess("Slipstream_Win.exe")]
    public class SlipInfo : BaseUnityPlugin, IMoPlugin, IMoHttpHandler
    {
        private static ConfigEntry<bool> debugLogs;
        internal static ManualLogSource Log;

        private Dictionary<string, IInfoHandler> handlers;

        public static readonly string HTTP_PREFIX = "slipinfo";

        public static readonly string COMPATIBLE_GAME_VERSION = "4.1595";
        public static readonly string GAME_VERSION_URL = "https://raw.githubusercontent.com/MoSadie/SlipInfo/refs/heads/main/versions.json";

        private void Awake()
        {
            try
            {
                SlipInfo.Log = base.Logger;

                if (!MoCore.MoCore.RegisterPlugin(this))
                {
                    Log.LogError("Failed to register plugin with MoCore. Please check the logs for more information.");
                    return;
                }

                debugLogs = Config.Bind("Debug", "DebugLogs", false, "Enable additional logging for debugging");

                // Configure our http handlers
                handlers = new Dictionary<string, IInfoHandler>();
                
                AddHandler(new VersionHandler());

                AddHandler(new CrewListHandler());
                AddHandler(new CrewSearchHandler());
                AddHandler(new CrewSelfHandler());

                AddHandler(new ShipInfoHandler());
                AddHandler(new EnemyShipInfoHandler());

                AddHandler(new RunInfoHandler());

                Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            } catch (PlatformNotSupportedException e)
            {
                Log.LogError("HttpListener is not supported on this platform.");
                Log.LogError(e.Message);
            } catch (Exception e)
            {
                Log.LogError("An error occurred while starting the plugin.");
                Log.LogError(e.Message);
            }

        }

        internal static void DebugLogInfo(string message)
        {
            if (debugLogs.Value)
            {
                Log.LogInfo(message);
            }
        }

        internal static void DebugLogWarn(string message)
        {
            if (debugLogs.Value)
            {
                Log.LogWarning(message);
            }
        }

        internal static void DebugLogError(string message)
        {
            if (debugLogs.Value)
            {
                Log.LogError(message);
            }
        }

        internal static void DebugLogDebug(string message)
        {
            if (debugLogs.Value)
            {
                Log.LogDebug(message);
            }
        }

        private void AddHandler(IInfoHandler handler)
        {
            if (handlers == null || handler == null)
            {
                return;
            }

            string path = $"/{HTTP_PREFIX}/{handler.GetPath()}";

            if (handlers.ContainsKey(path))
            {
                Log.LogWarning($"Duplicate path attempted to register! {path}");
                return;
            }

            Log.LogInfo($"Registered handler for {path}");
            handlers.Add(path, handler);
        }

        public HttpListenerResponse HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            DebugLogInfo("Handling request");
            try
            {

                HttpStatusCode status;
                string responseString;

                string pathUrl = request.RawUrl.Split('?', 2)[0];

                
                if (handlers.ContainsKey(pathUrl))
                {
                    DebugLogInfo($"Handling request with path: {pathUrl}");
                    IInfoHandler handler = handlers[pathUrl];
                    InfoResponse infoResponse = handler.HandleRequest(request.QueryString);

                    status = infoResponse.status;
                    responseString = infoResponse.response;
                } else
                {
                    DebugLogInfo($"No handler found.");
                    status = HttpStatusCode.BadRequest;
                    responseString = "{\"error\": \"Bad Request\"}"; 
                }

                response.StatusCode = (int)status;

                response.Headers.Add("Access-Control-Allow-Origin", "*");

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
                return response;
            } catch (Exception e)
            {
                Log.LogError("An error occurred while handling the request.");
                Log.LogError(e.Message);
                Log.LogError(e.StackTrace);
                return response;
            }
        }

        public string GetPrefix()
        {
            return HTTP_PREFIX;
        }

        public string GetCompatibleGameVersion()
        {
            return COMPATIBLE_GAME_VERSION;
        }

        public string GetVersionCheckUrl()
        {
            return GAME_VERSION_URL;
        }

        public BaseUnityPlugin GetPluginObject()
        {
            return this;
        }

        public IMoHttpHandler GetHttpHandler()
        {
            return this;
        }
    }
}
