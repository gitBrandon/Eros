using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Client.entViewModel
{
    public class genViewModel
    {
        public genViewModel()
        {

        }

        public void CreateViewModel(string strName)
        {
            GetData(strName);
        }

        private void GetData(string strName)
        {
            ServiceCaller callService = new ServiceCaller();
            string response = callService.Post("ProcessTableHead", "{\"Action\":99}");

            var kvPairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
        }
    }
}
