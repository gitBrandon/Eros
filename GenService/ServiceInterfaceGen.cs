using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.GenService
{
    public class ServiceInterfaceGen
    {
        string _filePath = Path.Combine(Environment.CurrentDirectory, "output", "ServiceInterfaces");
        string _modelNameSpace = Config.GetModelNamespace();
        string _Namespace = Config.GetMainNamespace();
        StringBuilder _contents = new StringBuilder();

        public ServiceInterfaceGen()
        {

        }

        public void CreateInterface(string strName, List<string> items)
        {
            try
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + "/" + "I" + strName + ".cs").Close();

                foreach (var item in items)
                {
                    CreateOperation(item);
                }

                Save(strName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void CreateOperation(string strName)
        {
            _contents.AppendLine("[OperationContract]");
            _contents.AppendLine("[WebInvoke(Method = \"POST\", UriTemplate = \"Process" + strName + "\", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]");
            _contents.AppendLine("" + _modelNameSpace + ".Wires." + strName + "Response Process"+strName+" (" + _modelNameSpace + ".Wires."+strName + "Request request);");
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
            sbFull.AppendLine("using " + _modelNameSpace + ".Wires;");
            sbFull.AppendLine("");
            #endregion usings

            #region namespace
            sbFull.AppendLine("namespace "+ _Namespace + ".Contracts");
            sbFull.AppendLine("{");
            #region class/interface
            sbFull.AppendLine("[ServiceContract(Name = \"" + strName + "\")]");
            sbFull.AppendLine("public interface I" + strName);
            sbFull.AppendLine("{");
            sbFull.AppendLine(_contents.ToString());
            sbFull.AppendLine("}");
            #endregion class/interface
            sbFull.AppendLine("}");
            #endregion namespace

            string result = sbFull.ToString();
            try
            {
                File.WriteAllText(_filePath + "/" + "I" + strName + ".cs", sbFull.ToString());
                Console.WriteLine("Added new Service interface file: " + "I" + strName + ".cs");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
