using System.ServiceProcess;

namespace WHEngenharia.Services.PedidoInternoRecorrente
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var service = new Service())
                ServiceBase.Run(service);
        }
    }
}
