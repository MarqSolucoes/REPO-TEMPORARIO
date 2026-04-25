using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoInternoDTO
    {
        public Int64 Id { get; set; }
        public Int64? IdPedidoInternoRecorrente { get; set; }
        public Int64 IdUsuarioAprovacao { get; set; }
        public Int64? IdFornecedorBeneficiario { get; set; }
        public Int64? IdUsuarioBeneficiario { get; set; }
        public Int64 IdDef { get; set; }
        public Int64? IdObra { get; set; }
        public Int64 Codigo { get; set; }
        public string CodigoFormatado { get; set; }
        public string Descricao { get; set; }
        public int NumeroTotalParcelas { get; set; }
        public double ValorTotal { get; set; }
        public bool? Aprovado { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public bool? ImportadoParaFinanceiro { get; set; }
        public bool Cancelado {  get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        public UsuarioDTO UsuarioAprovacao { get; set; }
        public FornecedorDTO FornecedorBeneficiario { get; set; }
        public UsuarioDTO UsuarioBeneficiario { get; set; }
        public UsuarioDTO UsuarioCadastro { get; set; }
        public DEFDTO DEF { get; set; }
        public ObraDTO Obra { get; set; }   

        public List<PedidoInternoArquivosDTO> Arquivos { get; set; }
        public List<PedidoInternoParcelasDTO> Parcelas { get; set; }
        public List<PedidoInternoObrasDTO> Obras { get; set; } 
    }
}
