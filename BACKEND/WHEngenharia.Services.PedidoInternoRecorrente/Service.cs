using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading;

namespace WHEngenharia.Services.PedidoInternoRecorrente
{
    partial class Service : ServiceBase
    {
        private static readonly HttpClient client = new HttpClient();

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.

            ThreadPool.QueueUserWorkItem(o => GeraPedidosInternos());
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        private static void GeraPedidosInternos()
        {
            while (true)
            {
                var arquivoLog = $"C:\\WH Engenharia\\Servicos\\PedidoInternoRecorrente\\Log\\{DateTime.Now.ToString("dd-MM-yyyy")}.txt";

                try
                {
                    //Realiza uma chamada no endpoint responsável por gerar os pedidos internos todo inicio de mês

                    File.AppendAllText(arquivoLog, $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} Rotina inciada{Environment.NewLine}");

                    var response = client.PostAsync("http://whengenharia.solinski.com.br:5500/api/Services/GeraPedidosInternos", null).Result;

                    File.AppendAllText(arquivoLog, $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} API result: {response.StatusCode}{Environment.NewLine}");
                    File.AppendAllText(arquivoLog, $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} API result: {JsonConvert.SerializeObject(response)}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    File.AppendAllText(arquivoLog, $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} Erro: {JsonConvert.SerializeObject(ex)}{Environment.NewLine}");
                }

                Thread.Sleep(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 1, 0, 0).AddMonths(1) - DateTime.Now);
            }
        }
    }
}
