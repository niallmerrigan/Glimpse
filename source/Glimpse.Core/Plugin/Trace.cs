﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plumbing;

namespace Glimpse.Core.Plugin
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    internal class Trace : IGlimpsePlugin, IProvideGlimpseHelp
    {
        public const string TraceMessageStoreKey = "Glimpse.Trace.Messages";
        public const string FirstWatchStoreKey = "Glimpse.Trace.FirstWatch";
        public const string LastWatchStoreKey = "Glimpse.Trace.LastWatch";

        public string Name
        {
            get { return "Trace"; }
        }

        public object GetData(HttpContextBase context)
        {
            var messages = context.Items[TraceMessageStoreKey] as IList<IList<string>>;
            if (messages == null) return null;

            foreach (var message in messages)
            {
                //Add style if the category is recognized
                switch (message[1].ToLower())
                {
                    case "warning":
                    case "warn":
                        message.Add("warn");
                        break;
                    case "information":
                    case "info":
                        message.Add("info");
                        break;
                    case "error":
                        message.Add("error");
                        break;
                    case "fail":
                        message.Add("fail");
                        break;
                    case "quiet":
                        message.Add("quiet");
                        break;
                    case "timing":
                        message.Add("loading");
                        break;
                    case "selected":
                        message.Add("selected");
                        break;
                    case "aspx.page":
                        message.Add("ms");
                        break;
                }
            }

            return messages;
        }

        public void SetupInit()
        {
            var traceListeners = System.Diagnostics.Trace.Listeners;
            if (!traceListeners.OfType<GlimpseTraceListener>().Any())
                traceListeners.Add(new GlimpseTraceListener()); //Add trace listener if it isn't already configured
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/Trace"; }
        }
    }
}