using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.GenTransactions
{
    public class txGen
    {
        string _filePath = Path.Combine(Environment.CurrentDirectory, "output", "transactions");
        string _ef = Config.GetEFName();
        string _modelNameSpace = Config.GetModelNamespace();
        string _txNamespace = Config.GetTxNamespace();
        bool _useTableAsID = Config.GetUseTableAsID();
        StringBuilder _contents = new StringBuilder();
        public txGen()
        {

        }

        public void CreateTx(string strName)
        {
            try
            {
                Directory.CreateDirectory(_filePath);
                File.Create(_filePath + "/" + "T" + strName + ".cs").Close();

                GetAll(strName);
                Get(strName);
                Create(strName);
                CreateList(strName);
                Modify(strName);
                ModifyList(strName);
                AddOrUpdate(strName);
                Delete(strName);
                DeleteFlag(strName);

                Save(strName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void GetAll(string strName)
        {
            _contents.AppendLine("public List<" + _modelNameSpace + "." + strName + "> GetAll()");
            _contents.AppendLine("{");
            _contents.AppendLine("try");
            _contents.AppendLine("{");
            _contents.AppendLine("using (" + _ef + " db = new DataContext().Context)");
            _contents.AppendLine("{");
            _contents.AppendLine("return db." + strName + ".ToList();");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            _contents.AppendLine("catch (Exception exc)");
            _contents.AppendLine("{");
            _contents.AppendLine("logMain.Error(\"[" + strName + "] GetAll failed\", exc);");
            _contents.AppendLine("logSpecific.Error(\"[" + strName + "] GetAll failed\", exc);");
            _contents.AppendLine("}");
            _contents.AppendLine("return null;");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void Get(string strName)
        {
            _contents.AppendLine("public " + _modelNameSpace + "." + strName + " Get(int id)");
            _contents.AppendLine("{");
            _contents.AppendLine("try");
            _contents.AppendLine("{");
            _contents.AppendLine("using (" + _ef + " db = new DataContext().Context)");
            _contents.AppendLine("{");
            _contents.AppendLine("return db." + strName + ".Find(id);");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            _contents.AppendLine("catch (Exception exc)");
            _contents.AppendLine("{");
            _contents.AppendLine("logMain.Error(\"[" + strName + "] GetSingle failed\", exc);");
            _contents.AppendLine("logSpecific.Error(\"[" + strName + "] GetSingle failed\", exc);");
            _contents.AppendLine("}");
            _contents.AppendLine("return null;");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void Create(string strName)
        {
            _contents.AppendLine("public " + _modelNameSpace + "." + strName + " Create(" + _modelNameSpace + "." + strName + " createItem)");
            _contents.AppendLine("{");
            _contents.AppendLine("try");
            _contents.AppendLine("{");
            _contents.AppendLine("using (" + _ef + " db = new DataContext().Context)");
            _contents.AppendLine("{");
            _contents.AppendLine("db." + strName + ".Add(createItem);");
            _contents.AppendLine("db.SaveChanges();");
            _contents.AppendLine("return createItem;");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            _contents.AppendLine("catch (Exception exc)");
            _contents.AppendLine("{");
            _contents.AppendLine(ApplyEntityExc(strName, "Create"));
            _contents.AppendLine("}");
            _contents.AppendLine("return null;");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void CreateList(string strName)
        {
            _contents.AppendLine("public bool CreateList(List<" + _modelNameSpace + "." + strName + "> createItems)");
            _contents.AppendLine("{");
            _contents.AppendLine("try");
            _contents.AppendLine("{");
            _contents.AppendLine("using (" + _ef + " db = new DataContext().Context)");
            _contents.AppendLine("{");
            _contents.AppendLine("foreach(var item in createItems)");
            _contents.AppendLine("{");
            _contents.AppendLine("db." + strName + ".Add(item);");
            _contents.AppendLine("}");
            _contents.AppendLine("db.SaveChanges();");
            _contents.AppendLine("return true;");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            _contents.AppendLine("catch (Exception exc)");
            _contents.AppendLine("{");
            _contents.AppendLine(ApplyEntityExc(strName, "CreateList"));
            _contents.AppendLine("}");
            _contents.AppendLine("return false;");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void Modify(string strName)
        {
            _contents.AppendLine("public bool Modify(" + _modelNameSpace + "." + strName + " modifiedItem)");
            _contents.AppendLine("{");
            _contents.AppendLine("try");
            _contents.AppendLine("{");
            _contents.AppendLine("using (" + _ef + " db = new DataContext().Context)");
            _contents.AppendLine("{");
            _contents.AppendLine("db.Entry(modifiedItem).State = System.Data.Entity.EntityState.Modified;");
            _contents.AppendLine("db.SaveChanges();");
            _contents.AppendLine("return true;");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            _contents.AppendLine("catch (Exception exc)");
            _contents.AppendLine("{");
            _contents.AppendLine(ApplyEntityExc(strName, "Modify"));
            _contents.AppendLine("}");
            _contents.AppendLine("return false;");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void AddOrUpdate(string strName)
        {
            _contents.AppendLine("public bool AddOrUpdate(" + _modelNameSpace + "." + strName + " item)");
            _contents.AppendLine("{");
            _contents.AppendLine("try");
            _contents.AppendLine("{");
            _contents.AppendLine("using (" + _ef + " db = new DataContext().Context)");
            _contents.AppendLine("{");
            if (_useTableAsID)
                _contents.AppendLine("var exists = db." + strName + ".Find(item." + strName + "ID);");
            else
                _contents.AppendLine("var exists = db." + strName + ".Find(item.ID);");

            _contents.AppendLine("if (exists != null)");
            _contents.AppendLine("{");
            _contents.AppendLine("db.Entry(item).State = System.Data.Entity.EntityState.Modified;");
            _contents.AppendLine("}");
            _contents.AppendLine("else");
            _contents.AppendLine("{");
            _contents.AppendLine("db." + strName + ".Add(item);");
            _contents.AppendLine("}");
            _contents.AppendLine("db.SaveChanges();");
            _contents.AppendLine("return true;");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            _contents.AppendLine("catch (Exception exc)");
            _contents.AppendLine("{");
            _contents.AppendLine(ApplyEntityExc(strName, "AddOrUpdate"));
            _contents.AppendLine("}");
            _contents.AppendLine("return false;");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void ModifyList(string strName)
        {
            _contents.AppendLine("public bool ModifyList(List<" + _modelNameSpace + "." + strName + "> modifiedItems)");
            _contents.AppendLine("{");
            _contents.AppendLine("try");
            _contents.AppendLine("{");
            _contents.AppendLine("using (" + _ef + " db = new DataContext().Context)");
            _contents.AppendLine("{");
            _contents.AppendLine("foreach(var item in modifiedItems)");
            _contents.AppendLine("{");
            _contents.AppendLine("db.Entry(item).State = System.Data.Entity.EntityState.Modified;");
            _contents.AppendLine("}");
            _contents.AppendLine("db.SaveChanges();");
            _contents.AppendLine("return true;");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            _contents.AppendLine("catch (Exception exc)");
            _contents.AppendLine("{");
            _contents.AppendLine(ApplyEntityExc(strName, "ModifyList"));
            _contents.AppendLine("}");
            _contents.AppendLine("return false;");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void Delete(string strName)
        {
            _contents.AppendLine("public bool Delete(int id)");
            _contents.AppendLine("{");
            _contents.AppendLine("try");
            _contents.AppendLine("{");
            _contents.AppendLine("using (" + _ef + " db = new DataContext().Context)");
            _contents.AppendLine("{");
            _contents.AppendLine("db.Entry(db." + strName + ".Find(id)).State = System.Data.Entity.EntityState.Deleted;");
            _contents.AppendLine("db.SaveChanges();");
            _contents.AppendLine("return true;");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            _contents.AppendLine("catch (Exception exc)");
            _contents.AppendLine("{");
            _contents.AppendLine(ApplyEntityExc(strName, "Delete"));
            _contents.AppendLine("}");
            _contents.AppendLine("return false;");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private void DeleteFlag(string strName)
        {
            _contents.AppendLine("public bool DeleteFlag(int id)");
            _contents.AppendLine("{");
            _contents.AppendLine("try");
            _contents.AppendLine("{");
            _contents.AppendLine("using (" + _ef + " db = new DataContext().Context)");
            _contents.AppendLine("{");
            _contents.AppendLine("var exists = db." + strName + ".Find(id);");
            _contents.AppendLine("if(exists != null)");
            _contents.AppendLine("{");
            _contents.AppendLine("exists.Deleted = true;");
            _contents.AppendLine("db.SaveChanges();");
            _contents.AppendLine("return true;");
            _contents.AppendLine("}");
            _contents.AppendLine("else");
            _contents.AppendLine("{");
            _contents.AppendLine("return false;");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            _contents.AppendLine("}");
            _contents.AppendLine("catch (Exception exc)");
            _contents.AppendLine("{");
            _contents.AppendLine(ApplyEntityExc(strName, "Delete"));
            _contents.AppendLine("}");
            _contents.AppendLine("return false;");
            _contents.AppendLine("}");
            _contents.AppendLine("");
        }

        private string ApplyEntityExc(string strName, string operation)
        {
            string strExc = @"
            #region Entity Valididation Errors
            if (exc.GetType() == typeof(System.Data.Entity.Validation.DbEntityValidationException))
            {
                System.Data.Entity.Validation.DbEntityValidationException valExc = (System.Data.Entity.Validation.DbEntityValidationException)exc;
                if (valExc != null)
                {
                    if (valExc.EntityValidationErrors != null)
                    {
                        foreach (var error in valExc.EntityValidationErrors)
                        {
                            if (error.ValidationErrors != null)
                            {
                                string strError = """";

                                for (int i = 0; i < error.ValidationErrors.Count(); i++)
                                {
                                    strError += ""Property: "" + error.ValidationErrors.ElementAt(i).PropertyName + ""\r\n\"" "";
                                    strError += ""Error : "" + error.ValidationErrors.ElementAt(i).ErrorMessage + ""\r\n\"" "";
                                }

                                logMain.Error(""[" + strName + @"] " + operation + strName + @" failed : "" + strError, exc);
                                logSpecific.Error(""[" + strName + @"] " + operation + strName + @" failed : "" + strError, exc);
                            }
                        }
                    }
                }
            }
            else
            {                
                logMain.Error(""[" + strName + @"] " + operation + strName + @" : "", exc);
                logSpecific.Error(""[" + strName + @"] " + operation + strName + @" : "", exc);
            }
            #endregion Entity Valididation Errors
            ";

            return strExc;
        }

        private void Save(string strName)
        {
            StringBuilder sbFull = new StringBuilder();
            #region usings
            sbFull.AppendLine("using " + _modelNameSpace + ";");
            sbFull.AppendLine("using log4net;");
            sbFull.AppendLine("using System;");
            sbFull.AppendLine("using System.Collections.Generic;");
            sbFull.AppendLine("using System.Linq;");
            sbFull.AppendLine("using System.Text;");
            sbFull.AppendLine("using System.Threading.Tasks;");
            sbFull.AppendLine();
            #endregion usings
            #region namespace
            sbFull.AppendLine("namespace " + _txNamespace + "");
            sbFull.AppendLine("{");
            #region class
            sbFull.AppendLine("public class " + "T" + strName);
            sbFull.AppendLine("{");
            sbFull.AppendLine("private static readonly ILog logMain = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);");
            sbFull.AppendLine("private static readonly ILog logSpecific = log4net.LogManager.GetLogger(\"Database\");");
            sbFull.AppendLine(_contents.ToString());
            sbFull.AppendLine("}");
            #endregion class
            sbFull.AppendLine("}");
            #endregion namespace

            try
            {
                File.WriteAllText(_filePath + "/" + "T" + strName + ".cs", sbFull.ToString());
                Console.WriteLine("Added new transaction file : " + "T" + strName + ".cs");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
