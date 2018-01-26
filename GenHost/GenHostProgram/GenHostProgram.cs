using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.GenHost.GenHostProgram
{
    public class GenHostProgram
    {
        string _filePath = Path.Combine(Environment.CurrentDirectory, "output", "Host");
        string _Namespace = Config.GetMainNamespace();
        string _serviceName = Config.GetServiceName();
        string _modelNameSpace = Config.GetModelNamespace();
        string _txNamespace = Config.GetTxNamespace();
        string _mainNamespace = Config.GetMainNamespace();
        StringBuilder _contents = new StringBuilder();

        public GenHostProgram()
        {

        }

        public void CreateProgram()
        {
            try
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + "/Program.cs").Close();

                CreateProgramClass();

                Save();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void CreateProgramClass()
        {
            _contents.AppendLine("public static void Main(string[] args)");
            _contents.AppendLine("{");
            _contents.AppendLine(_modelNameSpace + ".Mapping.MapModels.StartMapper();");
            _contents.AppendLine("ServiceStarter.StartServices();");
            _contents.AppendLine("Console.ReadLine();");
            _contents.AppendLine("}");
        }

        private void Save()
        {
            StringBuilder sbFull = new StringBuilder();

            #region usings
            sbFull.AppendLine("using System;");
            sbFull.AppendLine("using System.Collections.Generic;");
            sbFull.AppendLine("using System.Linq;");
            sbFull.AppendLine("using System.ServiceModel;");
            sbFull.AppendLine("using System.ServiceModel.Web;");
            sbFull.AppendLine("using System.ServiceModel.Description;");
            sbFull.AppendLine("using System.Text;");
            sbFull.AppendLine("using System.Threading.Tasks;");
            sbFull.AppendLine("using " + _Namespace + ".Service;");
            sbFull.AppendLine("using " + _Namespace + ".Contracts;");
            sbFull.AppendLine("using " + _Namespace + ".Host.CORS;");
            sbFull.AppendLine("");
            #endregion usings

            #region namespace
            sbFull.AppendLine("namespace " + _Namespace + ".Host");
            sbFull.AppendLine("{");
            #region class/interface
            sbFull.AppendLine("public class Program");
            sbFull.AppendLine("{");
            sbFull.AppendLine(_contents.ToString());
            sbFull.AppendLine("}");
            #endregion class/interface
            sbFull.AppendLine("}");
            #endregion namespace

            string result = sbFull.ToString();

            try
            {
                File.WriteAllText(_filePath + "/Program.cs", sbFull.ToString());
                Console.WriteLine("Added new Program file: Program.cs");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
