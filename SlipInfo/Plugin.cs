using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx.Logging;
using UnityEngine;
using Newtonsoft.Json;
using Subpixel.Events;
using System;
using System.Net;

namespace SlipInfo
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Slipstream_Win.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private static ConfigEntry<int> port;

        private static HttpListener listener;

        internal static ManualLogSource Log;

        private void Awake()
        {
            try
            {
                Plugin.Log = base.Logger;

                port = Config.Bind("Server Settings", "Port", 8001);

                if (!HttpListener.IsSupported)
                {
                    Log.LogError("HttpListener is not supported on this platform.");
                    listener = null;
                    return;
                }

                // Start the http server
                listener = new HttpListener();

                listener.Prefixes.Add($"http://127.0.0.1:{port.Value}/");
                listener.Prefixes.Add($"http://localhost:{port.Value}/");

                listener.Start();

                listener.BeginGetContext(new AsyncCallback(HandleRquest), listener);

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

        private void HandleRquest(IAsyncResult result)
        {
            Logger.LogInfo("Handling request");
            try
            {
                HttpListener listener = (HttpListener)result.AsyncState;

                HttpListenerContext context = listener.EndGetContext(result);

                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                string responseString = $"Hello World!\n URL: {request.RawUrl}\n Query:";

                foreach (string key in request.QueryString.AllKeys)
                {
                    responseString += $"\n - {key}: {request.QueryString[key]}";
                }

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();


                // Start listening for the next request
                listener.BeginGetContext(new AsyncCallback(HandleRquest), listener);
            } catch (Exception e)
            {
                Log.LogError("An error occurred while handling the request.");
                Log.LogError(e.Message);
            }
        }

    private void ApplicationQuitting()
        {
            Logger.LogInfo("Stopping server");
            // Stop server
            listener.Close();
        }
    }
}
