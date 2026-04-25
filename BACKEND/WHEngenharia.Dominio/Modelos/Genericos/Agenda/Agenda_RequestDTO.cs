using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Agenda
{
    public class Agenda_RequestDTO
    {
        public int tipoRelatorio { get; set; }
        public List<Int64> idsObras { get; set; }
        public List<Int64> idsClientes { get; set; }
        public List<Int64> idsDefs { get; set; }
        public List<Int64> idsFornecedores { get; set; }
        public string notaFiscal { get; set;}
        public string pedidoInterno { get; set; }
        public string ordemCompra { get; set; }
        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }
    }
}
