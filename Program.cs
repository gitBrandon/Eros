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

            Console.ReadLine();
        }
    }
}
