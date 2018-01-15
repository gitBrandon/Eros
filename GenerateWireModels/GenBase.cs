using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.GenerateWireModels
{
    public class GenBase
    {
        string _filePath = Path.Combine(Environment.CurrentDirectory, "output", "transactions");
        string _modelNameSpace = Config.GetModelNamespace();
        string _txNamespace = Config.GetTxNamespace();
        string _serviceProcessNamespace = Config.GetServiceProcessNamespace();
        bool _UseTableID = Config.GetUseTableAsID();
        StringBuilder _contents = new StringBuilder();

        public GenBase()
        {

        }

        public void CreateBase(string strName)
        {
            try
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + "/Base.cs").Close();

                CreateRequest(strName);
                CreateResponse(strName);
                CreateActionType(strName);

                Save(strName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void CreateRequest(string strName)
        {
            _contents.AppendLine("[DataContract]");
            _contents.AppendLine("public class RequestBase");
            _contents.AppendLine("{");
            _contents.AppendLine("[DataMember]");
            _contents.AppendLine("public Action Action { get; set; }");
            _contents.AppendLine("[DataMember]");
            _contents.AppendLine("public string CustomField { get; set; }");
            _contents.AppendLine("[DataMember]");
            _contents.AppendLine("public string CustomValue { get; set; }");

            _contents.AppendLine("// add later if necessary");
            _contents.AppendLine("//public string CustomAction { get; set; }");
            _contents.AppendLine("//public string CustomActionValue { get; set; }");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void CreateResponse(string strName)
        {
            _contents.AppendLine("[DataContract]");
            _contents.AppendLine("public class ResponseBase");
            _contents.AppendLine("{");
            _contents.AppendLine("public ResponseBase()");
            _contents.AppendLine("{");
            _contents.AppendLine("this.Success = true;");
            _contents.AppendLine("}");

            _contents.AppendLine("[DataMember]");
            _contents.AppendLine("public bool Success { get; set; }");
            _contents.AppendLine("[DataMember]");
            _contents.AppendLine("public string ErrorHint { get; set; }");
            _contents.AppendLine("[DataMember]");
            _contents.AppendLine("public string TechnicalError { get; set; }");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void CreateActionType(string strName)
        {
            _contents.AppendLine("[DataContract]");
            _contents.AppendLine("public enum Action");
            _contents.AppendLine("{");
            _contents.AppendLine("[EnumMember]");
            _contents.AppendLine("GetAll,");
            _contents.AppendLine("[EnumMember]");
            _contents.AppendLine("Get,");
            _contents.AppendLine("[EnumMember]");
            _contents.AppendLine("Create,");
            _contents.AppendLine("[EnumMember]");
            _contents.AppendLine("CreateList,");
            _contents.AppendLine("[EnumMember]");
            _contents.AppendLine("Modify,");
            _contents.AppendLine("[EnumMember]");
            _contents.AppendLine("ModifyList,");
            _contents.AppendLine("[EnumMember]");
            _contents.AppendLine("Delete,");
            _contents.AppendLine("[EnumMember]");
            _contents.AppendLine("FlagDeleted,");
            _contents.AppendLine("[EnumMember]");
            _contents.AppendLine("TemplateGen");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void Save(string strName)
        {
            StringBuilder sbFull = new StringBuilder();
            sbFull.AppendLine("using System;");
            sbFull.AppendLine("using System.Collections.Generic;");
            sbFull.AppendLine("using System.Linq;");
            sbFull.AppendLine("using System.Runtime.Serialization;");
            sbFull.AppendLine("using System.Text;");
            sbFull.AppendLine("using System.Threading.Tasks;");

            sbFull.AppendLine("");

            sbFull.AppendLine("namespace " + _modelNameSpace + ".Base");
            sbFull.AppendLine("{");
            #region classes
            _contents.AppendLine("");
            sbFull.AppendLine(_contents.ToString());
            _contents.AppendLine("");
            #endregion classes
            sbFull.AppendLine("}");

            try
            {
                File.WriteAllText(_filePath + "/Base.cs", sbFull.ToString());
                Console.WriteLine("Added new Base file : Base.cs");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            string strResult = sbFull.ToString();
        }
    }
}
