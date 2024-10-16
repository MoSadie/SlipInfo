﻿using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using BepInEx.Logging;
using UnityEngine;
using System;
using System.Net;
using SlipInfo.Handlers;
using MoCore;

namespace SlipInfo
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.mosadie.mocore", BepInDependency.DependencyFlags.HardDependency)]
    [BepInProcess("Slipstream_Win.exe")]
    public class Plugin : BaseUnityPlugin, MoPlugin
    {
        private static ConfigEntry<int> port;
        private static ConfigEntry<string> prefix;

        private static ConfigEntry<bool> debugLogs;

        private static HttpListener listener;

        internal static ManualLogSource Log;

        private Dictionary<string, InfoHandler> handlers;

        public static readonly string COMPATIBLE_GAME_VERSION = "4.1579";
        public static readonly string GAME_VERSION_URL = "https://raw.githubusercontent.com/MoSadie/SlipInfo/refs/heads/main/versions.json";

        private void Awake()
        {
            try
            {
                Plugin.Log = base.Logger;

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
                
                addHandler(new VersionHandler());

                addHandler(new CrewListHandler());
                addHandler(new CrewSearchHandler());
                addHandler(new CrewSelfHandler());

                addHandler(new ShipInfoHandler());
                addHandler(new EnemyShipInfoHandler());

                addHandler(new RunInfoHandler());

                listener.Start();

                listener.BeginGetContext(new AsyncCallback(HandleRequest), listener);

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

        internal static void debugLogInfo(string message)
        {
            if (debugLogs.Value)
            {
                Log.LogInfo(message);
            }
        }

        internal static void debugLogWarn(string message)
        {
            if (debugLogs.Value)
            {
                Log.LogWarning(message);
            }
        }

        internal static void debugLogError(string message)
        {
            if (debugLogs.Value)
            {
                Log.LogError(message);
            }
        }

        internal static void debugLogDebug(string message)
        {
            if (debugLogs.Value)
            {
                Log.LogDebug(message);
            }
        }

        private void addHandler(InfoHandler handler)
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

        private void HandleRequest(IAsyncResult result)
        {
            debugLogInfo("Handling request");
            try
            {
                HttpListener listener = (HttpListener)result.AsyncState;

                HttpListenerContext context = listener.EndGetContext(result);

                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                HttpStatusCode status;
                string responseString;

                string pathUrl = request.RawUrl.Split('?', 2)[0];

                
                if (handlers.ContainsKey(pathUrl))
                {
                    debugLogInfo($"Handling request with path: {pathUrl}");
                    InfoHandler handler = handlers[pathUrl];
                    InfoResponse infoResponse = handler.HandleRequest(request.QueryString);

                    status = infoResponse.status;
                    responseString = infoResponse.response;
                } else
                {
                    debugLogInfo($"No handler found.");
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


                // Start listening for the next request
                listener.BeginGetContext(new AsyncCallback(HandleRequest), listener);
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
            // Stop server
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
