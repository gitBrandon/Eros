using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.GenService
{
    public class ServiceProcessGen
    {
        string _filePath = Path.Combine(Environment.CurrentDirectory, "output", "ServiceProcesses");
        string _modelNameSpace = Config.GetModelNamespace();
        string _txNamespace = Config.GetTxNamespace();
        string _serviceProcessNamespace = Config.GetServiceProcessNamespace();
        bool _UseTableID = Config.GetUseTableAsID();
        StringBuilder _contents = new StringBuilder();
        string _strDb = "";
        string _strResponse = "";
        string _strRequest = "";
        public ServiceProcessGen()
        {

        }

        public void CreateProcess(string strName)
        {
            try
            {
                _strDb = "" + _txNamespace + ".T" + strName;
                _strResponse = "" + _modelNameSpace + ".Wires." + strName + "Response";
                _strRequest = "" + _modelNameSpace + ".Wires." + strName + "Request";

                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + "/" + strName + "Process.cs").Close();

                GetAll(strName);
                Get(strName);
                Create(strName);
                CreateList(strName);
                Modify(strName);
                ModifyList(strName);
                Delete(strName);
                DeleteFlag(strName);
                GenTemplateGet(strName);

                Save(strName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void CreateHeader(string strName)
        {

        }

        private void GetAll(string strName)
        {
            _contents.AppendLine("case " + _modelNameSpace + ".Base.Action.GetAll:");
            _contents.AppendLine("{");
            _contents.AppendLine("logMain.Info(\"Inside process " + strName + " GetAll \");");
            _contents.AppendLine("retVal." + strName + "List = new List<" + _modelNameSpace + ".Wires." + strName + "Item>();");
            _contents.AppendLine("var dtoList = db.GetAll();");
            _contents.AppendLine("if (dtoList != null)");
            _contents.AppendLine("{");
            _contents.AppendLine("foreach(var item in dtoList)");
            _contents.AppendLine("{");
            _contents.AppendLine("var mappedResult = Mapper.Map<" + _modelNameSpace + ".Wires." + strName + "Item>(item);");
            _contents.AppendLine("retVal." + strName + "List.Add(mappedResult);");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            _contents.AppendLine("} break;");
        }

        private void Get(string strName)
        {
            _contents.AppendLine("case " + _modelNameSpace + ".Base.Action.Get:");
            _contents.AppendLine("{");
            _contents.AppendLine("logMain.Info(\"Inside process " + strName + " Get \");");
            _contents.AppendLine("retVal." + strName + "List = new List<" + _modelNameSpace + ".Wires." + strName + "Item>();");
            #region Test custom fields
            _contents.AppendLine("if (string.IsNullOrEmpty(request.CustomField))");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("retVal.ErrorHint = \"Custom field is empty\";");
            _contents.AppendLine("retVal.TechnicalError = \"if (string.IsNullOrEmpty(request.CustomField)) is empty\";");
            _contents.AppendLine("logMain.Warn(\"CustomField is empty for " + strName + " Get \");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");

            _contents.AppendLine("if (string.IsNullOrEmpty(request.CustomValue))");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("retVal.ErrorHint = \"Custom value is empty\";");
            _contents.AppendLine("retVal.TechnicalError = \"if (string.IsNullOrEmpty(request.CustomValue)) is empty\";");
            _contents.AppendLine("logMain.Warn(\"CustomValue is empty for " + strName + " Get \");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");
            #endregion Test custom fields

            #region switch custom field/value
            _contents.AppendLine("switch(request.CustomField.ToUpper())");
            _contents.AppendLine("{");
            _contents.AppendLine("case \"ID\":");
            _contents.AppendLine("{");

            #region Get int value from custom value
            _contents.AppendLine("int id = -1;");
            _contents.AppendLine("if (!Int32.TryParse(request.CustomValue, out id))");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("retVal.ErrorHint = \"Cannot cast Custom value to int\";");
            _contents.AppendLine("retVal.TechnicalError = \"if (!Int32.TryParse(request.CustomValue, out id))\";");
            _contents.AppendLine("logMain.Warn(\"Failed to cast ID for " + strName + " Get \");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");
            #endregion Get int value from custom value

            #region actual call
            _contents.AppendLine("var dtoItem = db.Get(id);");
            _contents.AppendLine("if (dtoItem != null)");
            _contents.AppendLine("{");
            _contents.AppendLine("var mappedResult = Mapper.Map<" + _modelNameSpace + ".Wires." + strName + "Item>(dtoItem);");
            _contents.AppendLine("retVal." + strName + "List.Add(mappedResult);");
            _contents.AppendLine("}");
            #endregion actual call

            _contents.AppendLine("} break;");
            _contents.AppendLine("}");
            #endregion switch custom field/value
            _contents.AppendLine("} break;");
        }

        private void Create(string strName)
        {
            _contents.AppendLine("case " + _modelNameSpace + ".Base.Action.Create:");
            _contents.AppendLine("{");

            _contents.AppendLine("retVal." + strName + "List = new List<" + _modelNameSpace + ".Wires." + strName + "Item>();");

            #region validate
            _contents.AppendLine("if (!request." + strName + "Item.IsValid())");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("retVal.ErrorHint = request." + strName + "Item.Error;");
            _contents.AppendLine("retVal.TechnicalError = \"request." + strName + "Item.IsValid() is not valid\";");
            _contents.AppendLine("logMain.Warn(\"Failed to create " + strName + " " + " item.Error \");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");
            #endregion validate

            #region map
            _contents.AppendLine("// Automap to db model");
            _contents.AppendLine("" + _modelNameSpace + "." + strName + " db" + strName + "Item = Mapper.Map<" + _modelNameSpace + "." + strName + ">(request." + strName + "Item);");
            _contents.AppendLine("// END mapping");
            #endregion map

            if (_UseTableID)
            {
                _contents.AppendLine("db" + strName + "Item." + strName + "UID = Guid.NewGuid();");
            }
            else
            {
                _contents.AppendLine("db" + strName + "Item.UID = Guid.NewGuid();");
            }
            _contents.AppendLine("var successItem = db.Create(db" + strName + "Item);");
            _contents.AppendLine("if (successItem == null)");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.ErrorHint = \"Failed to create item\";");
            _contents.AppendLine("retVal.TechnicalError = \"db.Create(db" + strName + "Item); failed.\";");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("logMain.Error(\"Failed to create for " + strName + " db.Create(db" + strName + "Item); failed\");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");

            #region map back
            _contents.AppendLine("// Automap to db model");
            _contents.AppendLine("" + _modelNameSpace + ".Wires." + strName + "Item return" + strName + "Item = Mapper.Map<" + _modelNameSpace + ".Wires." + strName + "Item>(successItem);");
            _contents.AppendLine("// END mapping");
            #endregion map back

            _contents.AppendLine("retVal." + strName + "List = new List<" + _modelNameSpace + ".Wires." + strName + "Item>();");
            _contents.AppendLine("retVal." + strName + "List.Add(return" + strName + "Item);");

            _contents.AppendLine("} break;");

        }

        private void CreateList(string strName)
        {
            _contents.AppendLine("case " + _modelNameSpace + ".Base.Action.CreateList:");
            _contents.AppendLine("{");

            _contents.AppendLine("retVal." + strName + "List = new List<" + _modelNameSpace + ".Wires." + strName + "Item>();");

            //#region validate
            //_contents.AppendLine("if (!request." + strName + "Item.IsValid())");
            //_contents.AppendLine("{");
            //_contents.AppendLine("retVal.Success = false;");
            //_contents.AppendLine("retVal.ErrorHint = request." + strName + "Item.Error;");
            //_contents.AppendLine("retVal.TechnicalError = \"request." + strName + "Item.IsValid() is not valid\";");
            //_contents.AppendLine("return retVal;");
            //_contents.AppendLine("}");
            //#endregion validate

            #region map
            _contents.AppendLine("List<" + _modelNameSpace + "." + strName + "> lsItemsToCreate = new List<" + _modelNameSpace + "." + strName + ">();");
            #region if has items
            _contents.AppendLine("if (request." + strName + "List != null)");
            _contents.AppendLine("{");
            #region each item
            _contents.AppendLine("foreach (var item in request." + strName + "List)");
            _contents.AppendLine("{");
            _contents.AppendLine("// Automap to db model");
            _contents.AppendLine("" + _modelNameSpace + "." + strName + " db" + strName + "Item = Mapper.Map<" + _modelNameSpace + "." + strName + ">(request." + strName + "Item);");
            _contents.AppendLine("lsItemsToCreate.Add(db" + strName + "Item);");
            _contents.AppendLine("// END mapping");
            _contents.AppendLine("}");
            #endregion each item
            #region actual add
            _contents.AppendLine("var successItem = db.CreateList(lsItemsToCreate);");
            _contents.AppendLine("if (successItem == false)");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.ErrorHint = \"Failed to create item list\";");
            _contents.AppendLine("retVal.TechnicalError = \"db.CreateList(lsItemsToCreate); failed.\";");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("logMain.Error(\"Failed to create for " + strName + " db.CreateList(lsItemsToCreate); failed\");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");
            #endregion actual add
            #endregion if has items
            _contents.AppendLine("}");
            #endregion map

            _contents.AppendLine("} break;");
        }

        private void Modify(string strName)
        {
            _contents.AppendLine("case " + _modelNameSpace + ".Base.Action.Modify:");
            _contents.AppendLine("{");

            _contents.AppendLine("retVal." + strName + "List = new List<" + _modelNameSpace + ".Wires." + strName + "Item>();");

            #region validate
            _contents.AppendLine("if (!request." + strName + "Item.IsValid())");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("retVal.ErrorHint = request." + strName + "Item.Error;");
            _contents.AppendLine("retVal.TechnicalError = \"request." + strName + "Item.IsValid() is not valid\";");
            _contents.AppendLine("logMain.Warn(\"Failed to modify " + strName + " " + " item.Error \");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");
            #endregion validate

            #region map
            _contents.AppendLine("// Automap to db model");
            _contents.AppendLine("" + _modelNameSpace + "." + strName + " db" + strName + "Item = Mapper.Map<" + _modelNameSpace + "." + strName + ">(request." + strName + "Item);");
            _contents.AppendLine("// END mapping");
            #endregion map


            _contents.AppendLine("var successItem = db.Modify(db" + strName + "Item);");
            _contents.AppendLine("if (successItem == false)");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.ErrorHint = \"Failed to modify item\";");
            _contents.AppendLine("retVal.TechnicalError = \"db.Modify(db" + strName + "Item); failed.\";");
            _contents.AppendLine("logMain.Error(\"Failed to modify for " + strName + " db.Modify(db" + strName + "Item); failed\");");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");

            _contents.AppendLine("} break;");
        }

        private void ModifyList(string strName)
        {
            _contents.AppendLine("case " + _modelNameSpace + ".Base.Action.ModifyList:");
            _contents.AppendLine("{");

            _contents.AppendLine("retVal." + strName + "List = new List<" + _modelNameSpace + ".Wires." + strName + "Item>();");

            //#region validate
            //_contents.AppendLine("if (!request." + strName + "Item.IsValid())");
            //_contents.AppendLine("{");
            //_contents.AppendLine("retVal.Success = false;");
            //_contents.AppendLine("retVal.ErrorHint = request." + strName + "Item.Error;");
            //_contents.AppendLine("retVal.TechnicalError = \"request." + strName + "Item.IsValid() is not valid\";");
            //_contents.AppendLine("return retVal;");
            //_contents.AppendLine("}");
            //#endregion validate

            #region map
            _contents.AppendLine("List<" + _modelNameSpace + "." + strName + "> lsItemsToModify = new List<" + _modelNameSpace + "." + strName + ">();");
            #region if has items
            _contents.AppendLine("if (request." + strName + "List != null)");
            _contents.AppendLine("{");
            #region each item
            _contents.AppendLine("foreach (var item in request." + strName + "List)");
            _contents.AppendLine("{");
            _contents.AppendLine("// Automap to db model");
            _contents.AppendLine("" + _modelNameSpace + "." + strName + " db" + strName + "Item = Mapper.Map<" + _modelNameSpace + "." + strName + ">(request." + strName + "Item);");
            _contents.AppendLine("lsItemsToModify.Add(db" + strName + "Item);");
            _contents.AppendLine("// END mapping");
            _contents.AppendLine("}");
            #endregion each item
            #region actual add
            _contents.AppendLine("var successItem = db.ModifyList(lsItemsToModify);");
            _contents.AppendLine("if (successItem == false)");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.ErrorHint = \"Failed to modify item list\";");
            _contents.AppendLine("retVal.TechnicalError = \"db.ModifyList(lsItemsToModify); failed.\";");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("logMain.Error(\"Failed to modify for " + strName + " db.ModifyList(lsItemsToModify); failed\");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");
            #endregion actual add
            #endregion if has items
            _contents.AppendLine("}");
            #endregion map

            _contents.AppendLine("} break;");
        }

        private void Delete(string strName)
        {
            _contents.AppendLine("case " + _modelNameSpace + ".Base.Action.Delete:");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal." + strName + "List = new List<" + _modelNameSpace + ".Wires." + strName + "Item>();");
            #region Test custom fields
            _contents.AppendLine("if (string.IsNullOrEmpty(request.CustomField))");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("retVal.ErrorHint = \"Custom field is empty\";");
            _contents.AppendLine("retVal.TechnicalError = \"if (string.IsNullOrEmpty(request.CustomField)) is empty\";");
            _contents.AppendLine("logMain.Error(\"Failed to delete for " + strName + " CustomField is empty\");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");

            _contents.AppendLine("if (string.IsNullOrEmpty(request.CustomValue))");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("retVal.ErrorHint = \"Custom value is empty\";");
            _contents.AppendLine("retVal.TechnicalError = \"if (string.IsNullOrEmpty(request.CustomValue)) is empty\";");
            _contents.AppendLine("logMain.Error(\"Failed to delete for " + strName + " CustomValue is empty\");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");
            #endregion Test custom fields

            #region switch custom field/value
            _contents.AppendLine("switch(request.CustomField.ToUpper())");
            _contents.AppendLine("{");
            _contents.AppendLine("case \"ID\":");
            _contents.AppendLine("{");

            #region Get int value from custom value
            _contents.AppendLine("int id = -1;");
            _contents.AppendLine("if (!Int32.TryParse(request.CustomValue, out id))");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("retVal.ErrorHint = \"Cannot cast Custom value to int\";");
            _contents.AppendLine("retVal.TechnicalError = \"if (!Int32.TryParse(request.CustomValue, out id))\";");
            _contents.AppendLine("logMain.Error(\"Failed to delete for " + strName + " Could not cast ID from CustomValue\");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");
            #endregion Get int value from custom value

            #region actual call
            _contents.AppendLine("var successItem = db.Delete(id);");
            _contents.AppendLine("if (successItem == false)");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.ErrorHint = \"Failed to delete\";");
            _contents.AppendLine("retVal.TechnicalError = \"db.Delete(id); failed.\";");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("logMain.Error(\"Failed to delete for " + strName + " db.Delete(id); failed.\");");
            _contents.AppendLine("}");
            #endregion actual call

            _contents.AppendLine("} break;");
            _contents.AppendLine("}");
            #endregion switch custom field/value
            _contents.AppendLine("} break;");
        }

        private void DeleteFlag(string strName)
        {
            _contents.AppendLine("case " + _modelNameSpace + ".Base.Action.FlagDeleted:");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal." + strName + "List = new List<" + _modelNameSpace + ".Wires." + strName + "Item>();");
            #region Test custom fields
            _contents.AppendLine("if (string.IsNullOrEmpty(request.CustomField))");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("retVal.ErrorHint = \"Custom field is empty\";");
            _contents.AppendLine("retVal.TechnicalError = \"if (string.IsNullOrEmpty(request.CustomField)) is empty\";");
            _contents.AppendLine("logMain.Error(\"Failed to delete for " + strName + " CustomField is empty\");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");

            _contents.AppendLine("if (string.IsNullOrEmpty(request.CustomValue))");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("retVal.ErrorHint = \"Custom value is empty\";");
            _contents.AppendLine("retVal.TechnicalError = \"if (string.IsNullOrEmpty(request.CustomValue)) is empty\";");
            _contents.AppendLine("logMain.Error(\"Failed to delete for " + strName + " CustomValue is empty\");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");
            #endregion Test custom fields

            #region switch custom field/value
            _contents.AppendLine("switch(request.CustomField.ToUpper())");
            _contents.AppendLine("{");
            _contents.AppendLine("case \"ID\":");
            _contents.AppendLine("{");

            #region Get int value from custom value
            _contents.AppendLine("int id = -1;");
            _contents.AppendLine("if (!Int32.TryParse(request.CustomValue, out id))");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("retVal.ErrorHint = \"Cannot cast Custom value to int\";");
            _contents.AppendLine("retVal.TechnicalError = \"if (!Int32.TryParse(request.CustomValue, out id))\";");
            _contents.AppendLine("logMain.Error(\"Failed to delete for " + strName + " Could not cast ID from CustomValue\");");
            _contents.AppendLine("return retVal;");
            _contents.AppendLine("}");
            #endregion Get int value from custom value

            #region actual call
            _contents.AppendLine("var successItem = db.DeleteFlag(id);");
            _contents.AppendLine("if (successItem == false)");
            _contents.AppendLine("{");
            _contents.AppendLine("retVal.ErrorHint = \"Failed to delete\";");
            _contents.AppendLine("retVal.TechnicalError = \"db.Delete(id); failed.\";");
            _contents.AppendLine("retVal.Success = false;");
            _contents.AppendLine("logMain.Error(\"Failed to delete for " + strName + " db.Delete(id); failed.\");");
            _contents.AppendLine("}");
            #endregion actual call

            _contents.AppendLine("} break;");
            _contents.AppendLine("}");
            #endregion switch custom field/value
            _contents.AppendLine("} break;");
        }

        private void GenTemplateGet(string strName)
        {
            _contents.AppendLine("case " + _modelNameSpace + ".Base.Action.TemplateGen:");
            _contents.AppendLine("{");
            _contents.AppendLine("logMain.Info(\"Inside process " + strName + " GetTemplateGen \");");
            _contents.AppendLine("retVal." + strName + "List = new List<" + _modelNameSpace + ".Wires." + strName + "Item>();");
            _contents.AppendLine("");
            _contents.AppendLine("retVal." + strName + "List.Add(new " + _modelNameSpace + ".Wires." + strName + "Item();");
            _contents.AppendLine("} break;");
        }

        private void Save(string strName)
        {
            StringBuilder sbFull = new StringBuilder();
            #region usings
            sbFull.AppendLine(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Threading;
using System.Net;
using log4net;
using " + _modelNameSpace + @";
using " + _txNamespace + ";");
            #endregion usings
            #region namespace
            sbFull.AppendLine("namespace " + _serviceProcessNamespace);
            sbFull.AppendLine("{");
            #region class
            sbFull.AppendLine("public class " + strName + "Process");
            sbFull.AppendLine("{");
            sbFull.AppendLine("private static readonly ILog logMain = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);");
            #region Function
            sbFull.AppendLine("public " + _strResponse + " Process" + strName + "(" + _strRequest + " request)");
            sbFull.AppendLine("{");
            sbFull.AppendLine("WebOperationContext.Current.OutgoingResponse.Format = WebMessageFormat.Json;");
            sbFull.AppendLine("" + _strResponse + " retVal = new " + _strResponse + "();");
            sbFull.AppendLine(_strDb + " db = new " + _strDb + "();");
            #region try
            sbFull.AppendLine("try");
            sbFull.AppendLine("{");
            #region Switch/Case
            sbFull.AppendLine("switch(request.Action)");
            sbFull.AppendLine("{");
            sbFull.AppendLine(_contents.ToString());
            sbFull.AppendLine("}");
            #endregion Switch/Case
            sbFull.AppendLine("}");
            sbFull.AppendLine("catch (Exception exc)");
            sbFull.AppendLine("{");
            sbFull.AppendLine(AddExceptionResponse(strName, "Root Exception"));
            sbFull.AppendLine("}");
            #endregion try
            sbFull.AppendLine("return retVal;");
            sbFull.AppendLine("}");
            #endregion Function
            sbFull.AppendLine("}");
            #endregion class
            sbFull.AppendLine("}");
            #endregion namespace

            try
            {
                File.WriteAllText(_filePath + "/" + strName + "Process.cs", sbFull.ToString());
                Console.WriteLine("Added new service process file : " + strName + "Process.cs");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            string strResult = sbFull.ToString();
        }

        private string AddExceptionResponse(string strName, string operation)
        {
            StringBuilder exc = new StringBuilder();
            exc.AppendLine("retVal.Success = false;");
            exc.AppendLine("retVal.ErrorHint = \"Failed to Process " + strName + " @ " + operation + "\";");
            exc.AppendLine("retVal.TechnicalError = exc.Message;");
            return exc.ToString();
        }
    }
}
