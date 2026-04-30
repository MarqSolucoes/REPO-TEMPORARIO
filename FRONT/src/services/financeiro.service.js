import axios from "axios";

export default {
  // --- Financeiro ---
  obtemEntradasFinanceiro: (result) => {
    axios
      .get("/Financeiro/ObtemEntradas")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemEntradasRecusadasFinanceiro: (result) => {
    axios
      .get("/Financeiro/ObtemEntradasRecusadas")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  cancelaEntradaFinanceiro: (entrada, result) => {
    axios
      .post("/Financeiro/CancelaEntrada", entrada)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  autorizaEntradaFinanceiro: (entrada, result) => {
    axios
      .post("/Financeiro/AutorizaEntrada", entrada)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  // --- Fluxo de Caixa ---
  obtemFluxoCaixaConsolidado: (data, result) => {
    axios
      .post("/FluxoCaixa/ObtemFluxoCaixaConsolidado", data)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemFluxoCaixaDia: (data, result) => {
    axios
      .post("/FluxoCaixa/ObtemFluxosDoDia", data)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadExcelFluxoCaixa: (parametros, result) => {
    axios
      .post("/FluxoCaixa/Excel/Download/", parametros, { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status, message: "", data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadFluxoCaixaPDF: (parametros, result) => {
    axios
      .post("/FluxoCaixa/PDF/Download", parametros, { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status, message: "", data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemSaldosIniciais: (controller, result) => {
    axios
      .get("/" + controller)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  informarPagamentoOuRecebimentoFluxoCaixa: (idFluxoCaixa, novaData, result) => {
    var objDataPagamento = { id: idFluxoCaixa, data: novaData };
    axios
      .post("/FluxoCaixa/InformarPagamentoOuRecebimento", objDataPagamento)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  cadastraNovosValoresOCPI: (objeto, result) => {
    axios
      .post("/FluxoCaixa/CadastraNovosValoresOCPI", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  // --- Faturamento ---
  obtemFaturamentos: (objParametros, result) => {
    axios
      .post("/Faturamento/GetFiltrado", objParametros)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemFaturamentoObra: (idObra, result) => {
    axios
      .get("/Faturamento/GetObra/" + idObra)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemDetalheClienteFaturamento: (idCliente, result) => {
    axios
      .get("/Faturamento/GetClienteDetalhe/" + idCliente)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadRelatorioFaturamento: (parametros, result) => {
    axios
      .post("/Faturamento/Excel/Download/", parametros, { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status, message: "", data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  alteraValorFaturamento: (_id, _valor, _campo, result) => {
    var objeto = { id: _id, valor: _valor, campo: _campo };
    axios
      .post("/Faturamento/AlteraValor", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  alterarDataFaturamento: (_id, _data, _tipoData, result) => {
    var objeto = { id: _id, data: _data, tipoData: _tipoData };
    axios
      .post("/Faturamento/AlteraData", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemValoresAFaturar: (objParametros, result) => {
    axios
      .post("/Faturamento/ObtemValoresAFaturar", objParametros)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemValoresAFaturarDaObra: (idObra, result) => {
    axios
      .get("/Faturamento/ObtemValoresAFaturar/" + idObra)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  informaFaturamento: (objeto, result) => {
    axios
      .post("/Faturamento/InformarFaturamento", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  salvarAjusteManualAFaturar: (objeto, result) => {
    axios
      .post("/Faturamento/AjustaFaturamento", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  cancelarFaturamento: (objeto, result) => {
    axios
      .post("/Faturamento/CancelarFaturamento", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  // --- Relatório Financeiro ---
  downloadPdfRelatorioFinanceiro: (result) => {
    axios
      .get("/RelatorioFinanceiro/Pdf/Download", { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status, message: "", data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  // --- Notas Fiscais ---
  getItemFiltro: (item, result) => {
    axios
      .get("/NotaFiscal/ItemFiltro?item=" + item)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemNotasFiltradas: (objFiltro, result) => {
    axios
      .post("/NotaFiscal/ObtemNotasFiltradas", objFiltro)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  editarDataVencimentoNF: (objeto, result) => {
    axios
      .post("/NotaFiscal/EditarDataVencimento", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },
};
