using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.GenerateWireModels
{
    public class Mapping
    {
        string _filePath = Path.Combine(Environment.CurrentDirectory, "output", "Mapping");
        string _ef = Config.GetEFName();
        string _modelNameSpace = Config.GetModelNamespace();
        string _txNamespace = Config.GetTxNamespace();
        string _mainNamespace = Config.GetMainNamespace();
        StringBuilder _contents = new StringBuilder();

        public Mapping()
        {

        }

        public void CreateMapping(List<string> items)
        {
            try
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + "/MapModels.cs").Close();

                foreach (var item in items)
                {
                    AddMapping(item);
                }

                Save("");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void AddMapping(string strName)
        {
            _contents.AppendLine("// " + strName);
            _contents.AppendLine("x.CreateMap<" + _modelNameSpace + "." + strName + ", " + _modelNameSpace + ".Wires." + strName + "Item>();");
            _contents.AppendLine("x.CreateMap<" + _modelNameSpace + ".Wires." + strName + "Item, " + _modelNameSpace + "." + strName + ">();");
            _contents.AppendLine("");
        }

        private void Save(string strName)
        {
            StringBuilder sbFull = new StringBuilder();
            #region usings
            sbFull.AppendLine("using " + _modelNameSpace + ";");
            sbFull.AppendLine("using System;");
            sbFull.AppendLine("using System.Collections.Generic;");
            sbFull.AppendLine("using System.Runtime.Serialization;");
            sbFull.AppendLine("using System.Linq;");
            sbFull.AppendLine("using System.Text;");
            sbFull.AppendLine("using System.Threading.Tasks;");
            sbFull.AppendLine("using AutoMapper;");
            sbFull.AppendLine();
            #endregion usings
            #region namespace
            sbFull.AppendLine("namespace " + _modelNameSpace + ".Mapping");
            sbFull.AppendLine("{");
            #region classes
            sbFull.AppendLine("public class MapModels");
            sbFull.AppendLine("{");
            sbFull.AppendLine("public static void StartMapper()");
            sbFull.AppendLine("{");
            sbFull.AppendLine("Mapper.Initialize(x =>");
            sbFull.AppendLine("{");
            sbFull.AppendLine(_contents.ToString());
            sbFull.AppendLine("});");
            sbFull.AppendLine("}");
            sbFull.AppendLine("}");

            #endregion classes
            sbFull.AppendLine("}");
            #endregion namespace

            try
            {
                string str = sbFull.ToString();
                File.WriteAllText(_filePath + "/MapModels.cs", sbFull.ToString());
                Console.WriteLine("Added new mapping file : MapModels.cs");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
