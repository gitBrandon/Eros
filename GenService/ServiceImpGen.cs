using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.GenService
{
    public class ServiceImpGen
    {
        string _filePath = Path.Combine(Environment.CurrentDirectory, "output", "ServiceImplementations");
        string _modelNameSpace = Config.GetModelNamespace();
        string _Namespace = Config.GetMainNamespace();
        string _ServiceNamespace = Config.GetServiceProcessNamespace();
        string _ServiceName = Config.GetServiceName();
        StringBuilder _contents = new StringBuilder();

        public ServiceImpGen()
        {

        }

        public void CreateImp(string strName, List<string> items)
        {
            try
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + "/" + strName + "Imp.cs").Close();

                foreach (var item in items)
                {
                    CreateImplementation(item);
                }

                Save(strName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void CreateImplementation(string strName)
        {
            _contents.AppendLine("public " + _modelNameSpace + ".Wires." + strName + "Response Process" + strName + "(" + _modelNameSpace + ".Wires." + strName + "Request request)");
            _contents.AppendLine("{");
            _contents.AppendLine(_ServiceNamespace + ".ServiceProcess." + strName + "Process processObj = new " + _ServiceNamespace + ".ServiceProcess." + strName + "Process();");
            _contents.AppendLine("return processObj.Process" + strName + "(request);");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void Save(string strName)
        {
            StringBuilder sbFull = new StringBuilder();

            #region usings
            sbFull.AppendLine("using System;");
            sbFull.AppendLine("using System.Collections.Generic;");
            sbFull.AppendLine("using System.Linq;");
            sbFull.AppendLine("using System.ServiceModel;");
            sbFull.AppendLine("using System.ServiceModel.Web;");
            sbFull.AppendLine("using System.Text;");
            sbFull.AppendLine("using System.Threading.Tasks;");
            sbFull.AppendLine("using " + _modelNameSpace + ";");
            sbFull.AppendLine("using " + _modelNameSpace + ".Wires;");
            sbFull.AppendLine("");
            #endregion usings

            #region namespace
            sbFull.AppendLine("namespace " + _Namespace + ".Service");
            sbFull.AppendLine("{");
            #region class/interface
            sbFull.AppendLine("[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false)]");
            sbFull.AppendLine("public class " + strName + "Imp : " + _Namespace + ".Contracts.I" + _ServiceName);
            sbFull.AppendLine("{");
            sbFull.AppendLine(_contents.ToString());
            sbFull.AppendLine("}");
            #endregion class/interface
            sbFull.AppendLine("}");
            #endregion namespace

            string result = sbFull.ToString();
            try
            {
                File.WriteAllText(_filePath + "/" + strName + "Imp.cs", sbFull.ToString());
                Console.WriteLine("Added new Service implementation file: " + strName + ".cs");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

    }
}
