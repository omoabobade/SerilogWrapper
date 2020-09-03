using FlwUtilityClass;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriWrapper
{
    public class Log
    {

        public static string LogPath;
        public static bool Debug;


        private static Logger logger = new LoggerConfiguration()
            .WriteTo.File(LogPath + "log.txt", rollingInterval: RollingInterval.Hour)
            .CreateLogger();
        public static void Error(Exception ex)
        {
            Seri("ERROR", ex.Message, ex);
        }
        private static void Seri(string level, string message, Exception ex, params object[] obj)
        {
            try
            {
                if (!Debug) return;

                var lev = Serilog.Events.LogEventLevel.Information;
                switch (level)
                {
                    case "INFO": lev = Serilog.Events.LogEventLevel.Information; break;
                    case "ERROR": lev = Serilog.Events.LogEventLevel.Error; break;
                    case "WARNING": lev = Serilog.Events.LogEventLevel.Warning; break;
                }
                logger.Write(lev, ex, message, obj);
            }
            catch { }
        }

        public static void Announce(Exception ex)
        {
            try
            {
                //if (Settings.Get("log.verbose", "no") != "yes") return;
                var lev = Serilog.Events.LogEventLevel.Information;
                logger.Write(lev, ex.Message);
            }
            catch { }
        }

        private static void SeriOk(string level, string message, Exception ex, params object[] obj)
        {
            try
            {
                var lev = Serilog.Events.LogEventLevel.Information;
                switch (level)
                {
                    case "INFO": lev = Serilog.Events.LogEventLevel.Information; break;
                    case "ERROR": lev = Serilog.Events.LogEventLevel.Error; break;
                    case "WARNING": lev = Serilog.Events.LogEventLevel.Warning; break;
                }
                logger.Write(lev, ex, message, obj);
            }
            catch { }
        }


        public static void Write(string logMessage, string loc = "", string title = "Default")
        {
            Seri("INFO", title, new Exception(logMessage));
        }

        public static void Info(string v)
        {
            Seri("INFO", v, null);
        }

        public static void IO(object client, object request, object response)
        {
            var k = new
            {
                client,
                request,
                response
            };
            Seri("INFO", FlwUtility.SerializeJSON(k), null);
        }

        public static void Trace(string controller, string actions, Dictionary<string, object> args)
        {
            throw new NotImplementedException();
        }

        public static void Write(object data, string v)
        {
            Seri("INFO", v, new Exception(FlwUtility.SerializeJSON(data)));
        }

        public static void Writeall(object data, string v)
        {
            SeriOk("INFO", v, new Exception(FlwUtility.SerializeJSON(data)));
        }
    }

}
