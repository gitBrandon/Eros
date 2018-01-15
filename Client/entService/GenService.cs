using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Client.entService
{
    public class GenService
    {
        string _filePath = "";
        string _modelNameSpace = Config.GetModelNamespace();
        string _txNamespace = Config.GetTxNamespace();
        string _serviceProcessNamespace = Config.GetServiceProcessNamespace();
        bool _UseTableID = Config.GetUseTableAsID();
        StringBuilder _contents = new StringBuilder();

        public GenService()
        {

        }

        public void CreateService(string strName)
        {
            _filePath = Path.Combine(Environment.CurrentDirectory, "output", "client", strName, "js");

            Directory.CreateDirectory(_filePath);
            File.Create(_filePath + "/" + strName + "Service.js").Close();

            StringBuilder(strName);

            Save(strName);
        }

        private void StringBuilder(string strName)
        {
            _contents.AppendLine("if (" + strName.ToUpper() + " === undefined)");
            _contents.AppendLine("{");
            _contents.AppendLine("var " + strName.ToUpper() + " = { }");
            _contents.AppendLine("}");
            _contents.AppendLine("");
            _contents.AppendLine("" + strName.ToUpper() + ".Service = function() {");
            _contents.AppendLine("");
            _contents.AppendLine("var _service = new SERVICE.Caller();");
            _contents.AppendLine("");
            _contents.AppendLine("this.GetAll = function(request, callback) {");
            _contents.AppendLine("Call(request, callback);");
            _contents.AppendLine("}");
            _contents.AppendLine("this.GetSingle = function(request, callback) {");
            _contents.AppendLine("Call(request, callback);");
            _contents.AppendLine("}");
            _contents.AppendLine("this.Create = function(request, callback) {");
            _contents.AppendLine("var newRequest = {");
            _contents.AppendLine("\"Action\" : 2,");
            _contents.AppendLine("\"GameAreaItem\": JSON.parse(ko.toJSON(request))");
            _contents.AppendLine("}");
            _contents.AppendLine("Call(newRequest, callback);");
            _contents.AppendLine("}");
            _contents.AppendLine("this.Edit = function(request, callback) {");
            _contents.AppendLine("var newRequest = {");
            _contents.AppendLine("\"Action\" : 4,");
            _contents.AppendLine("\"GameAreaItem\": JSON.parse(ko.toJSON(request))");
            _contents.AppendLine("}");
            _contents.AppendLine("Call(newRequest, callback);");
            _contents.AppendLine("}");
            _contents.AppendLine("this.Delete = function(id, callback)");
            _contents.AppendLine("{");
            _contents.AppendLine("var request = {");
            _contents.AppendLine("\"Action\": 6,");
            _contents.AppendLine("\"CustomField\": \"ID\",");
            _contents.AppendLine("\"CustomValue\": id");
            _contents.AppendLine("}");
            _contents.AppendLine("");
            _contents.AppendLine("Call(request, callback);");
            _contents.AppendLine("}");
            _contents.AppendLine("");
            _contents.AppendLine("function Call(request, callback)");
            _contents.AppendLine("{");
            _contents.AppendLine("_service.Call(\"Process" + strName + "\", request, callback);");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
        }

        private void Save(string strName)
        {
            try
            {
                File.WriteAllText(_filePath + "/" + strName + "Service.js", _contents.ToString());
                Console.WriteLine("Added new Client service file : " + strName + "Process.cs");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
