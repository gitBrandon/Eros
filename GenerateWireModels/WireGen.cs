using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.GenerateWireModels
{
    public class WireGen
    {
        string _filePath = Path.Combine(Environment.CurrentDirectory, "output", "WireModelBasic");
        string _ef = Config.GetEFName();
        string _modelNameSpace = Config.GetModelNamespace();
        string _txNamespace = Config.GetTxNamespace();
        string _mainNamespace = Config.GetMainNamespace();
        StringBuilder _contents = new StringBuilder();

        public WireGen()
        {

        }

        public void CreateWire(string strName)
        {
            try
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + "/" + strName + "Item.cs").Close();

                Model(strName);
                Request(strName);
                Response(strName);
                Save(strName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void Model(string strName)
        {
            _contents.AppendLine("[DataContract]");
            _contents.AppendLine("public class " + strName + "Item");
            _contents.AppendLine("{");
            _contents.AppendLine("[DataMember]");
            _contents.AppendLine("public string Error { get; set; }");

            _contents.AppendLine("public bool IsValid()");
            _contents.AppendLine("{");
            _contents.AppendLine("return true;");
            _contents.AppendLine("}");
            _contents.AppendLine("");
            _contents.AppendLine("public string IsValid(string propertyName)");
            _contents.AppendLine("{");
            _contents.AppendLine("return null;");
            _contents.AppendLine("}");
            _contents.AppendLine("");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void Request(string strName)
        {
            _contents.AppendLine("[DataContract]");
            _contents.AppendLine("public class " + strName + "Request : Base.RequestBase");
            _contents.AppendLine("{");
            _contents.AppendLine("[DataMember]");
            _contents.AppendLine("public " + strName + "Item " + strName + "Item { get; set; }");
            _contents.AppendLine("[DataMember]");
            _contents.AppendLine("public List<" + strName + "Item> " + strName + "List { get; set; }");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void Response(string strName)
        {
            _contents.AppendLine("[DataContract]");
            _contents.AppendLine("public class " + strName + "Response : Base.ResponseBase");
            _contents.AppendLine("{");
            _contents.AppendLine("[DataMember]");
            _contents.AppendLine("public List<" + strName + "Item> " + strName + "List { get; set; }");
            _contents.AppendLine("}");
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
            sbFull.AppendLine();
            #endregion usings
            #region namespace
            sbFull.AppendLine("namespace " + _mainNamespace + ".Models.Wires");
            sbFull.AppendLine("{");
            #region classes
            sbFull.AppendLine(_contents.ToString());
            #endregion classes
            sbFull.AppendLine("}");
            #endregion namespace

            try
            {
                File.WriteAllText(_filePath + "/" + strName + "Item.cs", sbFull.ToString());
                Console.WriteLine("Added new Wire Model file : " + strName + "Item.cs");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
