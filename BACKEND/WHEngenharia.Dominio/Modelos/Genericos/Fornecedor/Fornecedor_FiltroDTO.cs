using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Fornecedor
{
    public class Fornecedor_FiltroDTO
    {
        public string CNPJ { get; set; }
        public string Endereco { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
    }
}
