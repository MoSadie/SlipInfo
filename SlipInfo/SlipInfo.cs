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
    public class SlipInfo : BaseUnityPlugin, MoPlugin
    {
        private static ConfigEntry<int> port;
        private static ConfigEntry<string> prefix;

        private static ConfigEntry<bool> debugLogs;

        private static HttpListener listener;
        private Thread serverThread;

        internal static ManualLogSource Log;

        private Dictionary<string, InfoHandler> handlers;

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

                port = Config.Bind("Server Settings", "Port", 8001, "Port to listen on.");
                prefix = Config.Bind("Server Settings", "Prefix", "slipinfo", "Prefix to have in path. Ex http://localhost:<port>/<prefix>/version");

                debugLogs = Config.Bind("Debug", "DebugLogs", false, "Enable additional logging for debugging");

                if (!HttpListener.IsSupported)
                {
                    Log.LogError("HttpListener is not supported on this platform.");
                    listener = null;
                    return;
                }

                // Start the http server
                listener = new HttpListener();

                listener.Prefixes.Add($"http://127.0.0.1:{port.Value}/{prefix.Value}/");
                listener.Prefixes.Add($"http://localhost:{port.Value}/{prefix.Value}/");

                handlers = new Dictionary<string, InfoHandler>();
                
                AddHandler(new VersionHandler());

                AddHandler(new CrewListHandler());
                AddHandler(new CrewSearchHandler());
                AddHandler(new CrewSelfHandler());

                AddHandler(new ShipInfoHandler());
                AddHandler(new EnemyShipInfoHandler());

                AddHandler(new RunInfoHandler());

                serverThread = new Thread(() => ServerThread(listener));
                serverThread.Start();

                Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");


                Application.quitting += ApplicationQuitting;
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

        private void ServerThread(HttpListener listener)
        {
            try
            {
                listener.Start();

                while (listener.IsListening)
                {
                    HttpListenerContext context = listener.GetContext();
                    HandleRequest(context);
                }
            }
            catch (Exception e)
            {
                Log.LogError("An exception occurred in the http server thread.");
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

        private void AddHandler(InfoHandler handler)
        {
            if (handlers == null || handler == null)
            {
                return;
            }

            string path = $"/{prefix.Value}/{handler.GetPath()}";

            if (handlers.ContainsKey(path))
            {
                Log.LogWarning($"Duplicate path attempted to register! {path}");
                return;
            }

            Log.LogInfo($"Registered handler for {path}");
            handlers.Add(path, handler);
        }

        private void HandleRequest(HttpListenerContext context)
        {
            DebugLogInfo("Handling request");
            try
            {
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                HttpStatusCode status;
                string responseString;

                string pathUrl = request.RawUrl.Split('?', 2)[0];

                
                if (handlers.ContainsKey(pathUrl))
                {
                    DebugLogInfo($"Handling request with path: {pathUrl}");
                    InfoHandler handler = handlers[pathUrl];
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
            } catch (Exception e)
            {
                Log.LogError("An error occurred while handling the request.");
                Log.LogError(e.Message);
                Log.LogError(e.StackTrace);
            }
        }

    private void ApplicationQuitting()
        {
            Logger.LogInfo("Stopping server");
            // Stop server, the thread is looking for the listener to stop listening
            listener.Close();
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
    }
}
