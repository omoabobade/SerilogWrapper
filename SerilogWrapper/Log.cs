using FlwUtilityClass;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriWrapper
{
    public class Log
    {

        public Logger logger;

        public Log()
        {
            var logPath = ConfigurationManager.AppSettings["log.path"];
              Logger logger = new LoggerConfiguration()
            .WriteTo.File(logPath + "log.txt", rollingInterval: RollingInterval.Hour)
            .CreateLogger();
        }


        public  void Error(Exception ex)
        {
            Seri("ERROR", ex.Message, ex);
        }
        private  void Seri(string level, string message, Exception ex, params object[] obj)
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

        public void Announce(Exception ex)
        {
            try
            {
                //if (Settings.Get("log.verbose", "no") != "yes") return;
                var lev = Serilog.Events.LogEventLevel.Information;
                logger.Write(lev, ex.Message);
            }
            catch { }
        }

        private  void SeriOk(string level, string message, Exception ex, params object[] obj)
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


        public void Write(string logMessage, string loc = "", string title = "Default")
        {
            Seri("INFO", title, new Exception(logMessage));
        }

        public  void Info(string v)
        {
            Seri("INFO", v, null);
        }

        public  void IO(object client, object request, object response)
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

        public void Write(object data, string v)
        {
            Seri("INFO", v, new Exception(FlwUtility.SerializeJSON(data)));
        }

        public void Writeall(object data, string v)
        {
            SeriOk("INFO", v, new Exception(FlwUtility.SerializeJSON(data)));
        }
    }

}
