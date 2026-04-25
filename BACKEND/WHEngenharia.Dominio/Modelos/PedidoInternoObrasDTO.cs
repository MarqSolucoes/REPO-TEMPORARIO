using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoInternoObrasDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoInterno { get; set; }
        public Int64 IdObra { get; set; }
        public double Valor { get; set; }

        public virtual ObraDTO Obra { get; set; }
    }
}
