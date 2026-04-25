using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Helpers
{
    public static class Helper_RepositorioSolicitacaoCompra
    {
        public static void AtualizaMaterial(
    SolicitacaoCompraMateriais existente,
    SolicitacaoCompraMateriaisDTO dto,
    SolicitacaoCompraDTO solicitacaoDto,
    string nomeUsuario,
    IReadOnlyDictionary<long, string> descricaoCondPgto,
    List<Historico> historicos)
        {
            var descMaterial = existente.Material.Descricao;

            if (dto.Quantidade != existente.Quantidade)
            {
                historicos.Add(NovoHistorico(solicitacaoDto, nomeUsuario,
                    $"Material: {descMaterial} - Quantidade solicitada",
                    existente.Quantidade.ToString(), dto.Quantidade.ToString()));
                existente.Quantidade = dto.Quantidade;
            }

            if (dto.ValorUnitarioEstimado != existente.ValorUnitarioEstimado)
            {
                historicos.Add(NovoHistorico(solicitacaoDto, nomeUsuario,
                    $"Material: {descMaterial} - Valor unitário estimado",
                    existente.ValorUnitarioEstimado?.ToString() ?? "",
                    dto.ValorUnitarioEstimado?.ToString() ?? ""));
                existente.ValorUnitarioEstimado = dto.ValorUnitarioEstimado;
            }

            foreach (var cotDto in dto.Cotacoes ?? new())
            {
                var cot = existente.Cotacoes.FirstOrDefault(c => c.IdFornecedor == cotDto.IdFornecedor);
                if (cot == null) continue; // comportamento preservado do original

                var prefixo = $"Material: {descMaterial}, Fornecedor {cot.Fornecedor.NomeFantasia}";

                if (cotDto.DataEntrega != cot.DataEntrega)
                {
                    historicos.Add(NovoHistorico(solicitacaoDto, nomeUsuario, $"{prefixo} - Data entrega material",
                        cot.DataEntrega?.ToString("dd/MM/yyyy") ?? "", cotDto.DataEntrega?.ToString("dd/MM/yyyy") ?? ""));
                    cot.DataEntrega = cotDto.DataEntrega;
                }
                if (cotDto.Frete != cot.Frete)
                {
                    historicos.Add(NovoHistorico(solicitacaoDto, nomeUsuario, $"{prefixo} - Frete",
                        cot.Frete?.ToString() ?? "", cotDto.Frete?.ToString() ?? ""));
                    cot.Frete = cotDto.Frete;
                }
                if (cotDto.IdCondicaoPagamento != cot.IdCondicaoPagamento)
                {
                    historicos.Add(NovoHistorico(solicitacaoDto, nomeUsuario, $"{prefixo} - Condição pagamento",
                        descricaoCondPgto.GetValueOrDefault(cot.IdCondicaoPagamento, ""),
                        descricaoCondPgto.GetValueOrDefault(cotDto.IdCondicaoPagamento, "")));
                    cot.IdCondicaoPagamento = cotDto.IdCondicaoPagamento;
                }
                if (cotDto.Imposto != cot.Imposto)
                {
                    historicos.Add(NovoHistorico(solicitacaoDto, nomeUsuario, $"{prefixo} - Imposto",
                        cot.Imposto?.ToString() ?? "", cotDto.Imposto?.ToString() ?? ""));
                    cot.Imposto = cotDto.Imposto;
                }
                if (cotDto.Desconto != cot.Desconto)
                {
                    historicos.Add(NovoHistorico(solicitacaoDto, nomeUsuario, $"{prefixo} - Desconto",
                        cot.Desconto?.ToString() ?? "", cotDto.Desconto?.ToString() ?? ""));
                    cot.Desconto = cotDto.Desconto;
                }
                if (cotDto.QuantidadeCotado != cot.QuantidadeCotado)
                {
                    historicos.Add(NovoHistorico(solicitacaoDto, nomeUsuario, $"{prefixo} - Quantidade cotada",
                        cot.QuantidadeCotado.ToString(), cotDto.QuantidadeCotado.ToString()));
                    cot.QuantidadeCotado = cotDto.QuantidadeCotado;
                }
                if (cotDto.ValorUnitarioCotado != cot.ValorUnitarioCotado)
                {
                    historicos.Add(NovoHistorico(solicitacaoDto, nomeUsuario, $"{prefixo} - Valor unitário cotado",
                        cot.ValorUnitarioCotado.ToString(), cotDto.ValorUnitarioCotado.ToString()));
                    cot.ValorUnitarioCotado = cotDto.ValorUnitarioCotado;
                    cot.ValorUnitarioComDesconto = cotDto.ValorUnitarioCotado * (1 - ((cotDto.Desconto ?? 0) / 100));
                }
            }
        }

        public static Historico NovoHistorico(
            SolicitacaoCompraDTO solicitacaoDto,
            string nomeUsuario,
            string campo,
            string valorAntigo = null,
            string valorNovo = null) => new()
            {
                Id = Guid.NewGuid(),
                Campo = campo,
                Data = DateTime.Now,
                IdPedidoCompra = null,
                IdPedidoInterno = null,
                IdSolicitacaoCompra = solicitacaoDto.Id,
                IdUsuario = solicitacaoDto.IdUsuarioAlteracao,
                Usuario = nomeUsuario,
                ValorAntigo = valorAntigo,
                ValorNovo = valorNovo,
            };
    }
}
