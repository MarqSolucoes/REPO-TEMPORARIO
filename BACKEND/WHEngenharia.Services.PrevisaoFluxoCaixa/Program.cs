using System.Net.Http;
using System.ServiceProcess;

namespace WHEngenharia.Services.PrevisaoFluxoCaixa
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            using (var service = new Service())
                ServiceBase.Run(service);
        }
    }
}
