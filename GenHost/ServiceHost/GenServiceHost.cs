using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.GenHost.ServiceHost
{
    public class GenServiceHost
    {
        string _filePath = Path.Combine(Environment.CurrentDirectory, "output", "Host");
        string _Namespace = Config.GetMainNamespace();
        string _serviceName = Config.GetServiceName();
        StringBuilder _contents = new StringBuilder();

        public GenServiceHost()
        {

        }

        public void CreateHoster()
        {
            try
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + "/ServiceStarter.cs").Close();

                CreateServiceHelper();

                Save();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void CreateServiceHelper()
        {
            _contents.AppendLine("private static readonly log4net.ILog logMain = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);");
            _contents.AppendLine("private static readonly log4net.ILog log = log4net.LogManager.GetLogger(\"ServiceHost\");");
            _contents.AppendLine();
            _contents.AppendLine("private static WebServiceHost s_ManagementServiceREST = null;");
            _contents.AppendLine();

            #region function
            _contents.AppendLine("public static void StartServices()");
            _contents.AppendLine("{");
            _contents.AppendLine("logMain.Info(\"Starting services...\");");
            _contents.AppendLine("try");
            _contents.AppendLine("{");
            _contents.AppendLine("s_ManagementServiceREST = new WebServiceHost(typeof(" + _Namespace + ".Service." + _serviceName + "Imp));");
            _contents.AppendLine(@"foreach (ServiceEndpoint EP in s_ManagementServiceREST.Description.Endpoints)
                    EP.Behaviors.Add(new BehaviorAttribute());
                s_ManagementServiceREST.Open();
                logMain.Info(\""Service REST Started: \"" + s_ManagementServiceREST.BaseAddresses.ElementAt(0).AbsoluteUri.ToString());");
            _contents.AppendLine("}");
            _contents.AppendLine("catch(Exception exc)");
            _contents.AppendLine("{");
            _contents.AppendLine(@"log.Error(\""[StartServices] Failed to start Service REST\"", exc);
                logMain.Error(\""[StartServices] Failed to start Management Service REST\"", exc);
            EventLog.WriteEntry(\""" + _Namespace + @"\"", \""Failed to start EXCEPTION: \"" + exc.Message + \"" INNER EXCEPTION: \"" + exc.InnerException, EventLogEntryType.Error); ");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            #endregion function
        }

        private void Save()
        {
            StringBuilder sbFull = new StringBuilder();

            #region usings
            sbFull.AppendLine("using System;");
            sbFull.AppendLine("using System.Collections.Generic;");
            sbFull.AppendLine("using System.Diagnostics;");
            sbFull.AppendLine("using System.Globalization;");
            sbFull.AppendLine("using System.IO;");
            sbFull.AppendLine("using System.Linq;");
            sbFull.AppendLine("using System.ServiceModel;");
            sbFull.AppendLine("using System.ServiceProcess;");
            sbFull.AppendLine("using System.Text;");
            sbFull.AppendLine("using System.Threading;");
            sbFull.AppendLine("using System.Threading.Tasks;");
            sbFull.AppendLine("");
            #endregion usings

            #region namespace
            sbFull.AppendLine("namespace " + _Namespace + ".Host");
            sbFull.AppendLine("{");
            #region class/interface
            sbFull.AppendLine("public class ServiceStarter");
            sbFull.AppendLine("{");
            sbFull.AppendLine(_contents.ToString());
            sbFull.AppendLine("}");
            #endregion class/interface
            sbFull.AppendLine("}");
            #endregion namespace

            string result = sbFull.ToString();
            try
            {
                File.WriteAllText(_filePath + "/ServiceStarter.cs", sbFull.ToString());
                Console.WriteLine("Added new Service handler file: ServiceStarter.cs");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
