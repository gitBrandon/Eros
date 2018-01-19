using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    class Program
    {
        public static void Main(string[] args)
        {
            var items = Config.GetItems();

            Console.WriteLine("Generate service?");
            string answer = Console.ReadLine();
            if (answer.ToUpper().Contains("Y") || answer.ToUpper().Contains("YES"))
            {
                Console.WriteLine("Generating service...");

                #region Service
                foreach (var item in items)
                {
                    GenTransactions.txGen txGenerator = new GenTransactions.txGen();
                    txGenerator.CreateTx(item);

                    GenService.ServiceProcessGen serviceProcessGen = new GenService.ServiceProcessGen();
                    serviceProcessGen.CreateProcess(item);

                    GenerateWireModels.WireGen wireGen = new GenerateWireModels.WireGen();
                    wireGen.CreateWire(item);
                }

                GenService.ServiceInterfaceGen serviceInterfaceGen = new GenService.ServiceInterfaceGen();
                serviceInterfaceGen.CreateInterface(Config.GetServiceName(), items);

                GenService.ServiceImpGen serviceImpGen = new GenService.ServiceImpGen();
                serviceImpGen.CreateImp(Config.GetServiceName(), items);

                GenerateWireModels.GenBase baseGen = new GenerateWireModels.GenBase();
                baseGen.CreateBase(Config.GetServiceName());

                #endregion Service
            }

            Console.WriteLine("Generate client?");
            answer = Console.ReadLine();
            if (answer.ToUpper().Contains("Y") || answer.ToUpper().Contains("YES"))
            {
                #region Client
                foreach (var item in items)
                {
                    Client.entService.GenService genServiceCaller = new Client.entService.GenService();
                    genServiceCaller.CreateService(item);

                    Client.entViewModel.genViewModel genViewModel = new Client.entViewModel.genViewModel();
                    genViewModel.CreateViewModel(item);
                }
                #endregion Client
            }
            Console.ReadLine();
        }
    }
}
