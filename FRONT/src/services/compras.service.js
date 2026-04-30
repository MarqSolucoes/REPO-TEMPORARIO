import axios from "axios";

export default {
  // --- Solicitação de Compra ---
  getSolicitacaoCompraParaCotacao: (idStatus, result) => {
    axios
      .get("/SolicitacaoCompra/ParaCotacao/" + idStatus)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  getSolicitacoesCompraStatus: (idStatus, result) => {
    axios
      .get("/SolicitacaoCompra/Status/" + idStatus)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  getSolicitacoesCompraUsuario: (filtros, result) => {
    axios
      .post("/SolicitacaoCompra/Usuario/", filtros)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  cancelarSolicitacaoCompra: (idSolicitacaoCompra, motivo, result) => {
    axios
      .post("/SolicitacaoCompra/Cancelar?idSolicitacaoCompra=" + idSolicitacaoCompra + "&motivoCancelamento=" + motivo)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemQuantidades: (result) => {
    axios
      .get("/SolicitacaoCompra/ObtemQuantidades")
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  validarSolicitacaoCompra: (idSolicitacaoCompra, idComprador, result) => {
    axios
      .post("/SolicitacaoCompra/Validar/" + idSolicitacaoCompra + "/" + idComprador)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  reprovarSolicitacaoCompra: (idSolicitacaoCompra, result) => {
    axios
      .post("/SolicitacaoCompra/Reprovar/" + idSolicitacaoCompra)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  enviarSolicitacaoParaAprovacao: (idSolicitacaoCompra, result) => {
    axios
      .post("/SolicitacaoCompra/EnviarSolicitacaoParaAprovacao/" + idSolicitacaoCompra)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  reabrirSolicitacaoCompra: (objetoReaberturaSolicitacao, result) => {
    axios
      .post("/SolicitacaoCompra/Reabrir", objetoReaberturaSolicitacao)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  enviarParaRevisao: (id, result) => {
    axios
      .post("/SolicitacaoCompra/EnviarParaRevisao/" + id)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  enviarParaAprovacao: (id, result) => {
    axios
      .post("/SolicitacaoCompra/EnviarParaAprovacao/" + id)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  removerItensSelecionados: (solicitacaoCompraParaCotacao, result) => {
    axios
      .post("/SolicitacaoCompra/RemoverItensSelecionados", solicitacaoCompraParaCotacao)
      .then((apiReturn) => { result({ status: apiReturn.status, message: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  duplicarItensSelecionados: (solicitacaoCompraParaCotacao, result) => {
    axios
      .post("/SolicitacaoCompra/DuplicarItensSelecionados", solicitacaoCompraParaCotacao)
      .then((apiReturn) => { result({ status: apiReturn.status, message: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  // --- Cotação ---
  cadastrarCotacao: (objetoCotacao, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao", objetoCotacao)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  cadastraNovoFornecedorCotacao: (objeto, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao/Fornecedor", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemCotacoes: (idSolicitacaoCompraMaterial, result) => {
    axios
      .get("/SolicitacaoCompra/Cotacoes?idSolicitacaoCompraMaterial=" + idSolicitacaoCompraMaterial)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  definirCotacaoPrincipal: (controller, idObjeto, result) => {
    axios
      .post("/" + controller + "/" + idObjeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  editarCotacao: (objSolicitacaoCompra, result) => {
    axios
      .put("/SolicitacaoCompra/Cotacao/Editar", objSolicitacaoCompra)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  definirFornecedorPrincipal: (idSolicitacaoCompra, idFornecedor, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao/DefinirFornecedorPrincipal?idSolicitacaoCompra=" + idSolicitacaoCompra + "&idFornecedor=" + idFornecedor)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  removeFornecedor: (idSolicitacaoCompra, idFornecedor, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao/ExcluirFornecedor?idSolicitacaoCompra=" + idSolicitacaoCompra + "&idFornecedor=" + idFornecedor)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  definirDataEntregaFornecedor: (idSolicitacaoCompra, idFornecedor, dataEntrega, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao/DefinirDataEntregaFornecedor?idSolicitacaoCompra=" + idSolicitacaoCompra + "&idFornecedor=" + idFornecedor + "&dataEntrega=" + dataEntrega)
      .then((apiReturn) => { result({ status: apiReturn.status, message: apiReturn.data, data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  enviarDiretoria: (objeto, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao/EnviarDiretoria", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  devolverCompras: (objeto, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao/DevolverCompras", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  finalizarCotacao: (controller, idObjeto, result) => {
    axios
      .post("/" + controller + idObjeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  reprovarCotacao: (idObjeto, comentario, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao/Reprovar/" + idObjeto + "?comentario=" + comentario)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "" }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemCotacoesParaAprovacao: (result) => {
    axios
      .get("/SolicitacaoCompra/Cotacoes/ObtemParaAprovacao")
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadPdfCotacoes: (id, result) => {
    axios
      .get("/SolicitacaoCompra/Pdf/Cotacoes/Download/" + id, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadPdfArquivoSolicitacaoCompra: (id, result) => {
    axios
      .get("/SolicitacaoCompra/Pdf/Download/" + id, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadExcelCotacoes: (id, result) => {
    axios
      .post("/SolicitacaoCompra/Excel/Cotacoes/Download/" + id, null, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  excluiMaterialCotacao: (idMaterial, idSolicitacaoCompra, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao/Material/" + idMaterial + "/" + idSolicitacaoCompra)
      .then((apiReturn) => { result({ status: apiReturn.data, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  // --- Rascunho ---
  salvaRascunho: (objeto, result) => {
    axios
      .post("/SolicitacaoCompraRascunho", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  atualizaRascunho: (objeto, result) => {
    axios
      .put("/SolicitacaoCompraRascunho", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemRascunhos: (result) => {
    axios
      .get("/SolicitacaoCompraRascunho")
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  excluiRascunho: (id, result) => {
    axios
      .delete("/SolicitacaoCompraRascunho/" + id)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  // --- Pedido de Compra ---
  getPedidosCompraStatus: (idStatus, result) => {
    axios
      .get("/PedidoCompra/Status/" + idStatus)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  getPedidoCompraParaConciliacaoNF: (idPedidoCompra, result) => {
    axios
      .get("/PedidoCompra/ParaConciliacao/" + idPedidoCompra)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  uploadFile: (controller, file, idPedidoCompra, result) => {
    axios
      .post("/" + controller + "/Upload/" + idPedidoCompra, file, {
        headers: { "Content-Type": "multipart/form-data" },
      })
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadFile: (controller, id, result) => {
    axios
      .get("/" + controller + "/Download/" + id, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  getArquivos: (controller, idPedidoCompra, result) => {
    axios
      .get("/" + controller + "/Arquivos/" + idPedidoCompra)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  deleteArquivo: (controller, idArquivo, result) => {
    axios
      .delete("/" + controller + "/Arquivos/" + idArquivo)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  enviarComentario: (controller, idSolicitacaoOuPedidoCompra, comentario, result) => {
    axios
      .post("/" + controller + "/Comentario?id=" + idSolicitacaoOuPedidoCompra + "&comentario=" + comentario)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemComentarios: (controller, idSolicitacaoOuPedidoCompra, result) => {
    axios
      .get("/" + controller + "/Comentario/" + idSolicitacaoOuPedidoCompra)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadPdfPedidoCompra: (id, exibirDataPagamento = true, result) => {
    axios
      .get("/PedidoCompra/Pdf/Download/" + id + "/" + exibirDataPagamento, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  cancelarPedidoCompra: (idPedidoCompra, motivo, result) => {
    axios
      .post("/PedidoCompra/Cancelar?idPedidoCompra=" + idPedidoCompra + "&motivoCancelamento=" + motivo)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  finalizarPedidoCompra: (idPedidoCompra, result) => {
    axios
      .post("/PedidoCompra/Finalizar?idPedidoCompra=" + idPedidoCompra)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  cancelarSaldo: (objCancelamentoSaldo, result) => {
    axios
      .post("/PedidoCompra/CancelarSaldo", objCancelamentoSaldo)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemNotasFiscaisParaAprovacao: (result) => {
    axios
      .get("/PedidoCompra/NotaFiscal/ObtemParaAprovacao")
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  aprovarReprovarNotaFiscal: (idPedidoCompraNotaFiscal, aprovada, result) => {
    axios
      .post("/PedidoCompra/NotaFiscal/AprovarReprovar/" + idPedidoCompraNotaFiscal + "/" + aprovada)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemPedidosFinalizadosFiltrados: (objParametros, result) => {
    axios
      .post("/PedidoCompra/ObtemFinalizadosFiltrados", objParametros)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  getPedidosParaConciliacao: (result) => {
    axios
      .get("/PedidoCompra/ParaConciliacao")
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  conciliar: (objPedidoCompraConciliacao, idPedidoCompraNotaFiscal, result) => {
    axios
      .put("/PedidoCompra/Conciliar/" + idPedidoCompraNotaFiscal, objPedidoCompraConciliacao)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemNotasFiscaisReprovadasDiretoria: (result) => {
    axios
      .get("/PedidoCompra/Conciliacao/ReprovadasDirecao")
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  reenviarNotaFiscalDiretoria: (idPedidoCompraNotaFiscal, result) => {
    axios
      .post("/PedidoCompra/Conciliacao/NotaFiscal/ReenviarDiretoria/" + idPedidoCompraNotaFiscal)
      .then((apiReturn) => { result({ status: apiReturn.status, message: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  reenviarNotaFiscalFinanceiro: (idPedidoCompraNotaFiscal, result) => {
    axios
      .post("/PedidoCompra/Conciliacao/NotaFiscal/ReenviarFinanceiro/" + idPedidoCompraNotaFiscal)
      .then((apiReturn) => { result({ status: apiReturn.status, message: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemNotasFiscaisReprovadasFinanceiro: (result) => {
    axios
      .get("/PedidoCompra/Conciliacao/ReprovadasFinanceiro")
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  cancelarSaldoAbrirPedido: (objeto, result) => {
    axios
      .post("/PedidoCompra/Conciliar/CancelarSaldoAbrirPedido", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  trocarFornecedor: (objeto, result) => {
    axios
      .post("/PedidoCompra/Conciliar/TrocarFornecedor", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  postNotaFiscalPagamento: (objPagamento, result) => {
    axios
      .post("/PedidoCompra/NotaFiscal/PostNotaFiscalPagamento", objPagamento)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  getNotaFiscalPagamento: (idPedidoCompraNotaFiscal, result) => {
    axios
      .get("/PedidoCompra/NotaFiscal/ObtemNotaFiscalPagamento/" + idPedidoCompraNotaFiscal)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemNotasConciliadas: (objetoPesquisa, result) => {
    axios
      .post("/PedidoCompra/ObtemNotasConciliadas", objetoPesquisa)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  cancelarNotaFiscal: (idPedidoCompraNotaFiscal, result) => {
    axios
      .post("/PedidoCompra/NotaFiscal/Cancelar/" + idPedidoCompraNotaFiscal)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  alterarObraNF: (idPedidoCompraNotaFiscal, idObra, result) => {
    axios
      .post("/PedidoCompra/NotaFiscal/AlterarObra/" + idPedidoCompraNotaFiscal + "/" + idObra)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  devolverPedidoParaCotacao: (idPedidoCompra, result) => {
    axios
      .post("/PedidoCompra/DevolverParaCotacao/" + idPedidoCompra)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemNotasFiscaisAlteracaoData: (id, result) => {
    axios
      .get("/PedidoCompra/NotaFiscal/ObtemNotasFiscaisAlteracaoData/" + id)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  alteraArquivoNotaFiscal: (controller, file, idPedidoCompraArquivo, result) => {
    axios
      .post("/PedidoCompra/NotaFiscal/Alterar/Upload/" + idPedidoCompraArquivo, file, {
        headers: { "Content-Type": "multipart/form-data" },
      })
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  uploadNovoPdfNotaFiscal: (file, idPedidoCompranotaFiscal, result) => {
    axios
      .post("/PedidoCompra/UploadNotaFiscal/" + idPedidoCompranotaFiscal, file, {
        headers: { "Content-Type": "multipart/form-data" },
      })
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  marcarVisualizacaoNotaFiscalReprovada: (idPedidoCompraNotaFiscal, result) => {
    axios
      .post("/PedidoCompra/NotaFiscal/MarcarVisualizacaoNotaFiscalReprovada/" + idPedidoCompraNotaFiscal)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },
};
