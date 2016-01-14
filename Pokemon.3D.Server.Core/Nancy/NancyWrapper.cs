using System;
using System.Collections.Generic;
using Aragas.Core.Wrappers;

using Nancy;
using Nancy.ErrorHandling;
using Nancy.Hosting.Self;

namespace Pokemon_3D_Server_Core.Nancy
{
    public class OnlineResponseJson
    {
        public class PlayerJson
        {
            public string Name { get; set; }
            public int Ping { get; set; }
            public bool Online { get; set; }

            public PlayerJson(string name, int ping, bool online) { Name = name; Ping = ping; Online = online; }
        }
        public List<PlayerJson> Players { get; }

        public OnlineResponseJson(IEnumerable<PlayerJson> players) { Players = new List<PlayerJson>(players); }
    }

    public class CustomStatusCode : IStatusCodeHandler
    {
        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            context.Response.StatusCode = HttpStatusCode.Forbidden;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context) =>
            statusCode == HttpStatusCode.NotFound || statusCode == HttpStatusCode.InternalServerError ||
            statusCode == HttpStatusCode.Forbidden || statusCode == HttpStatusCode.Unauthorized;
    }

    public class ApiNancyModule : NancyModule
    {
        public ApiNancyModule() : base("/api")
        {
            foreach (var pageAction in NancyImpl.DataApi.List)
                Get[$"/{pageAction.Page}"] = pageAction.Action;
        }
    }


    public static class NancyImpl
    {
        public static NancyData DataApi { get; private set; }

        private static NancyHost Server { get; set; }


        public static void SetDataApi(NancyData data) { DataApi = data; }

        public static void Start(string url, ushort port)
        {
            var config = new HostConfiguration {RewriteLocalhost = false};

            Server?.Stop();
            Server = new NancyHost(config, new Uri($"http://{url}:{port}/"));
            Server.Start();
        }

        public static void Stop() { Server?.Dispose(); }
    }
}