using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public class Config
    {
        public static string GetEFName()
        {
            return ConfigurationManager.AppSettings["EF_Name"];
        }

        public static List<string> GetItems()
        {
            List<string> lsRetVal = null;

            string items = ConfigurationManager.AppSettings["Items"];

            if (string.IsNullOrEmpty(items))
                return lsRetVal;

            lsRetVal = items.Split(',').ToList();

            return lsRetVal;
        }

        public static bool GetUseTableAsID()
        {
            if (ConfigurationManager.AppSettings["UseTableAsID"] == null)
                return false;

            if (ConfigurationManager.AppSettings["UseTableAsID"] == "1")
                return true;
            else
                return false;
        }

        public static string GetServiceName()
        {
            return ConfigurationManager.AppSettings["ServiceName"];
        }

        public static string GetModelNamespace()
        {
            return ConfigurationManager.AppSettings["ModelNamespace"];
        }

        public static string GetTxNamespace()
        {
            return ConfigurationManager.AppSettings["TransactionNamespace"];
        }

        public static string GetServiceProcessNamespace()
        {
            return ConfigurationManager.AppSettings["ServiceProcessNamespace"];
        }

        public static string GetMainNamespace()
        {
            return ConfigurationManager.AppSettings["Namespace"];
        }

        public static string GetServiceEndpoint()
        {
            return ConfigurationManager.AppSettings["ServiceEndpointOnceComplete"];
        }
    }
}
