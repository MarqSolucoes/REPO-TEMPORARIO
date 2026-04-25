using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Configuration;
using System.Threading.Tasks;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.Services.AjusteDataFaturamento
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            var options = new DbContextOptionsBuilder<WHEngenhariaContext>().UseSqlServer(connectionString).Options;

            var context = new WHEngenhariaContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            });

            var mapper = config.CreateMapper();

            var servico = new ServicoFaturamento(context, mapper);

            await servico.AjustaDataFaturamento();
        }
    }
}