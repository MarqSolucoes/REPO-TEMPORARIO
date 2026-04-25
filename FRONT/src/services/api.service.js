import axios from "axios";
import { isBoolean } from "lodash";

export default {
  get: (controller, idObjeto, result) => {
    axios
      .get("/" + controller + "/" + idObjeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getAll: (controller, apenasAtivos, result) => {
    axios
      .get("/" + controller + "?apenasAtivos=" + apenasAtivos)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  post: (controller, objetoDTO, result) => {
    axios
      .post("/" + controller, objetoDTO)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: apiReturn.data, data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  put: (controller, objetoDTO, result) => {
    axios
      .put("/" + controller, objetoDTO)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  delete: (controller, idObjeto, result) => {
    axios
      .delete("/" + controller + "/" + idObjeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  ativarDesativar: (controller, idObjeto, valor, result) => {
    axios
      .post(
        "/" +
          controller +
          "/AtivarDesativar?id=" +
          idObjeto +
          "&ativarDesativar=" +
          valor
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "" });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  habilitarDesabilitarLogin: (idUsuario, valor, result) => {
    axios
      .post(
        "/Usuario/HabilitaDesabilitaLogin?id=" +
          idUsuario +
          "&habilitaDesabilitaLogin=" +
          valor
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "" });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  habilitarDesabilitarPedidoInterno: (controller, idObjeto, valor, result) => {
    axios
      .post(
        "/" +
          controller +
          "/HabilitarDesabilitarPI?id=" +
          idObjeto +
          "&habilitarDesabilitar=" +
          valor
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "" });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  alterarSenha: (idUsuario, senha, result) => {
    axios
      .post("/Usuario/AlterarSenha?id=" + idUsuario + "&senha=" + senha)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "" });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getEngenheiros: (result) => {
    axios
      .get("/Usuario/Engenheiros")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getDiretores: (result) => {
    axios
      .get("/Usuario/Diretores")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getSolicitacaoCompraParaCotacao: (idStatus, result) => {
    axios
      .get("/SolicitacaoCompra/ParaCotacao/" + idStatus)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getPedidoCompraParaConciliacaoNF: (idPedidoCompra, result) => {
    axios
      .get("/PedidoCompra/ParaConciliacao/" + idPedidoCompra)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  conciliar: (objPedidoCompraConciliacao, idPedidoCompraNotaFiscal, result) => {
    axios
      .put(
        "/PedidoCompra/Conciliar/" + idPedidoCompraNotaFiscal,
        objPedidoCompraConciliacao
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  editarCotacao: (objSolicitacaoCompra, result) => {
    axios
      .put("/SolicitacaoCompra/Cotacao/Editar", objSolicitacaoCompra)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  definirFornecedorPrincipal: (idSolicitacaoCompra, idFornecedor, result) => {
    axios
      .post(
        "/SolicitacaoCompra/Cotacao/DefinirFornecedorPrincipal?idSolicitacaoCompra=" +
          idSolicitacaoCompra +
          "&idFornecedor=" +
          idFornecedor
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  removeFornecedor: (idSolicitacaoCompra, idFornecedor, result) => {
    axios
      .post(
        "/SolicitacaoCompra/Cotacao/ExcluirFornecedor?idSolicitacaoCompra=" +
          idSolicitacaoCompra +
          "&idFornecedor=" +
          idFornecedor
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  definirDataEntregaFornecedor: (
    idSolicitacaoCompra,
    idFornecedor,
    dataEntrega,
    result
  ) => {
    axios
      .post(
        "/SolicitacaoCompra/Cotacao/DefinirDataEntregaFornecedor?idSolicitacaoCompra=" +
          idSolicitacaoCompra +
          "&idFornecedor=" +
          idFornecedor +
          "&dataEntrega=" +
          dataEntrega
      )
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: apiReturn.data,
          data: apiReturn.data,
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getSolicitacoesCompraStatus: (idStatus, result) => {
    axios
      .get("/SolicitacaoCompra/Status/" + idStatus)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getSolicitacoesCompraUsuario: (filtros, result) => {
    axios
      .post("/SolicitacaoCompra/Usuario/", filtros)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getPedidosInternosUsuario: (result) => {
    axios
      .get("/PedidoInterno/Usuario/")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  cancelarSolicitacaoCompra: (idSolicitacaoCompra, motivo, result) => {
    axios
      .post(
        "/SolicitacaoCompra/Cancelar?idSolicitacaoCompra=" +
          idSolicitacaoCompra +
          "&motivoCancelamento=" +
          motivo
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemQuantidades: (result) => {
    axios
      .get("/SolicitacaoCompra/ObtemQuantidades")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  validarSolicitacaoCompra: (idSolicitacaoCompra, idComprador, result) => {
    axios
      .post(`/SolicitacaoCompra/Validar/${idSolicitacaoCompra}/${idComprador}`)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  reprovarSolicitacaoCompra: (idSolicitacaoCompra, result) => {
    axios
      .post("/SolicitacaoCompra/Reprovar/" + idSolicitacaoCompra)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  enviarSolicitacaoParaAprovacao: (idSolicitacaoCompra, result) => {
    axios
      .post("/SolicitacaoCompra/EnviarSolicitacaoParaAprovacao/" + idSolicitacaoCompra)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  reabrirSolicitacaoCompra: (objetoReaberturaSolicitacao, result) => {
    axios
      .post("/SolicitacaoCompra/Reabrir", objetoReaberturaSolicitacao)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  cadastrarCotacao: (objetoCotacao, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao", objetoCotacao)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  cadastraNovoFornecedorCotacao: (
    objeto,
    result
  ) => {
    axios
      .post(
        "/SolicitacaoCompra/Cotacao/Fornecedor", objeto
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemCotacoes: (idSolicitacaoCompraMaterial, result) => {
    axios
      .get(
        "/SolicitacaoCompra/Cotacoes?idSolicitacaoCompraMaterial=" +
          idSolicitacaoCompraMaterial
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  definirCotacaoPrincipal: (controller, idObjeto, result) => {
    axios
      .post("/" + controller + "/" + idObjeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  enviarDiretoria: (objeto, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao/EnviarDiretoria", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  devolverCompras: (objeto, result) => {
    axios
      .post("/SolicitacaoCompra/Cotacao/DevolverCompras", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  finalizarCotacao: (controller, idObjeto, result) => {
    axios
      .post("/" + controller + idObjeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  reprovarCotacao: (idObjeto, comentario, result) => {
    axios
      .post(
        "/SolicitacaoCompra/Cotacao/Reprovar/" +
          idObjeto +
          "?comentario=" +
          comentario
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "" });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getPedidosCompraStatus: (idStatus, result) => {
    axios
      .get("/PedidoCompra/Status/" + idStatus)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  uploadFile: (controller, file, idPedidoCompra, result) => {
    axios
      .post("/" + controller + "/Upload/" + idPedidoCompra, file, {
        headers: { "Content-Type": "multipart/form-data" },
      })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadFile: (controller, id, result) => {
    axios
      .get("/" + controller + "/Download/" + id, { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getArquivos: (controller, idPedidoCompra, result) => {
    axios
      .get("/" + controller + "/Arquivos/" + idPedidoCompra)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  deleteArquivo: (controller, idArquivo, result) => {
    axios
      .delete("/" + controller + "/Arquivos/" + idArquivo)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  enviarComentario: (
    controller,
    idSolicitacaoOuPedidoCompra,
    comentario,
    result
  ) => {
    axios
      .post(
        "/" +
          controller +
          "/Comentario?id=" +
          idSolicitacaoOuPedidoCompra +
          "&comentario=" +
          comentario
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemComentarios: (controller, idSolicitacaoOuPedidoCompra, result) => {
    axios
      .get("/" + controller + "/Comentario/" + idSolicitacaoOuPedidoCompra)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadPdfPedidoCompra: (id, exibirDataPagamento = true, result) => {
    axios
      .get("/PedidoCompra/Pdf/Download/" + id + "/" + exibirDataPagamento, { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadPdfCotacoes: (id, result) => {
    axios
      .get("/SolicitacaoCompra/Pdf/Cotacoes/Download/" + id, { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadPdfArquivoSolicitacaoCompra: (id, result) => {
    axios
      .get("/SolicitacaoCompra/Pdf/Download/" + id, { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  cancelarPedidoCompra: (idPedidoCompra, motivo, result) => {
    axios
      .post(
        "/PedidoCompra/Cancelar?idPedidoCompra=" +
          idPedidoCompra +
          "&motivoCancelamento=" +
          motivo
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  finalizarPedidoCompra: (idPedidoCompra, result) => {
    axios
      .post("/PedidoCompra/Finalizar?idPedidoCompra=" + idPedidoCompra)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  cancelarSaldo: (objCancelamentoSaldo, result) => {
    axios
      .post("/PedidoCompra/CancelarSaldo", objCancelamentoSaldo)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemNotasFiscaisParaAprovacao: (result) => {
    axios
      .get("/PedidoCompra/NotaFiscal/ObtemParaAprovacao")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemCotacoesParaAprovacao: (result) => {
    axios
      .get("/SolicitacaoCompra/Cotacoes/ObtemParaAprovacao")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        console.log(error);
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  aprovarReprovarNotaFiscal: (idPedidoCompraNotaFiscal, aprovada, result) => {
    axios
      .post(
        `/PedidoCompra/NotaFiscal/AprovarReprovar/${idPedidoCompraNotaFiscal}/${aprovada}`
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getItemFiltro: (item, result) => {
    axios
      .get("/NotaFiscal/ItemFiltro?item=" + item)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
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
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemFluxoCaixaConsolidado: (data, result) => {
    axios
      .post("/FluxoCaixa/ObtemFluxoCaixaConsolidado", data)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemFluxoCaixaDia: (data, result) => {
    axios
      .post("/FluxoCaixa/ObtemFluxosDoDia", data)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        console.log(error);
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadPdfRelatorioFinanceiro: (result) => {
    axios
      .get("/RelatorioFinanceiro/Pdf/Download", { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          console.log(error);
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  getAllPedidosInternos: (controller, result) => {
    axios
      .get("/" + controller)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getAllPedidosInternosRecorrentes: (controller, result) => {
    axios
      .get("/" + controller)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  definirParcelaComoPaga: (idParcela, result) => {
    axios
      .post("/PedidoInterno/DefinirParcelaPaga/" + idParcela)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  alterarParcela: (objParcela, result) => {
    axios
      .post("/PedidoInterno/AlterarParcela/", objParcela)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemProximoCodigoPedidoInterno: (result) => {
    axios
      .get("/PedidoInterno/ProximoCodigo")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemFaturamentos: (objParametros, result) => {
    axios
      .post("/Faturamento/GetFiltrado", objParametros)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  bloquearDesbloquearObra: (idObjeto, valor, result) => {
    axios
      .post(
        `/Obra/BloquearDesbloquear/${idObjeto}/${valor}`
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "" });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  cancelarObra: (idObra, result) => {
    axios
      .post(
        `/Obra/Cancelar/${idObra}`
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "" });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
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
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
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
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadRelatorioFaturamento: (parametros, result) => {
    axios
      .post("/Faturamento/Excel/Download/", parametros, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemRelatorioControleETO: (obraBloqueada, result) => {
    axios
      .get("/RelatorioControleETO/" + obraBloqueada)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadRelatorioETOExcelComAjuste: (result) => {
    axios
      .post("/RelatorioControleETO/ExcelComAjuste/Download/", null, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadRelatorioETOExcel: (result) => {
    axios
      .post("/RelatorioControleETO/Excel/Download/", null, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadRelatorioETOPDF: (result) => {
    axios
      .get("/RelatorioControleETO/Pdf/Download", { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemProximasDatasAjuste: (idObra, result) => {
    axios
      .get("/RelatorioControleETO/GetProximasDatasAjuste/" + idObra)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  informaAjustes: (objAjuste, result) => {
    axios
      .post("/RelatorioControleETO/Ajuste", objAjuste)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  visualizaComentarioAjuste: (idAjuste, result) => {
    axios
      .post(`/RelatorioControleETO/VisualizarComentario/${idAjuste}`)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemPedidosInternosParaAprovacao: (idUsuario, result) => {
    axios
      .get("/PedidoInterno/ObtemPedidosInternosParaAprovacao/" + idUsuario)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  aprovarReprovarPedidoInterno: (idPedidoInterno, valor, result) => {
    axios
      .post(
        `/PedidoInterno/AprovarReprovarPedidoInterno/${idPedidoInterno}/${valor}`
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemAgenda: (parametros, result) => {
    axios
      .post(`/Agenda/ObtemAgenda`, parametros)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemAgendaFaturamento: (parametros, result) => {
    axios
      .post(`/Agenda/ObtemAgendaFaturamento`, parametros)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  alteraDefAgenda: (id, idDef, result) => {
    axios
      .post(`/Agenda/AlteraDEF/${id}/${idDef}`)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  alteraValorAgenda: (_id, _valor, result) => {

    var objeto = {
      id: _id,
      valor: _valor
    }

    axios
      .post(`/Agenda/AlteraValor`, objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  alteraValorFaturamento: (_id, _valor, _campo, result) => {

    var objeto = {
      id: _id,
      valor: _valor,
      campo: _campo
    }

    axios
      .post(`/Faturamento/AlteraValor`, objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

   alterarDataAgenda: (_id, _data, result) => {

    var objeto = {
      id: _id,
      data: _data
    }

    axios
      .post(`/Agenda/AlteraDataPagamento`, objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  alterarDataFaturamento: (_id, _data, _tipoData, result) => {

    var objeto = {
      id: _id,
      data: _data,
      tipoData: _tipoData //1 = Recebimento, 2 = Faturamento
    }

    axios
      .post(`/Faturamento/AlteraData`, objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  informarPagamentoRecebimentoEmLote: (objeto, result) => {
    axios
      .post(`/Agenda/InformarPagamentoRecebimentoLote`, objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  alterarDefEmLote: (objeto, result) => {
    axios
      .post(`/Agenda/AlteraDefLote`, objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  alterarDataAgendaEmLote: (objeto, result) => {
    axios
      .post(`/Agenda/AlteraDataPagamentoLote`, objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  cancelarPagamentoRecebimento: (objeto, result) => {
    axios
      .post(`/Agenda/CancelarPagamentoRecebimento`, objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadPDFAgenda: (parametros, result) => {
    axios
      .post("/Agenda/PDF/Download", parametros, { responseType: "blob" })
      .then((apiReturn) => {
        console.log(apiReturn);
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadPDFAgendaFaturamento: (parametros, result) => {
    axios
      .post("/Agenda/PDFFaturamento/Download", parametros, { responseType: "blob" })
      .then((apiReturn) => {
        console.log(apiReturn);
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadExcelAgenda: (parametros, result) => {
    axios
      .post("/Agenda/Excel/Download/", parametros, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadExcelCarimbo: (parametros, result) => {
    axios
      .post("/Agenda/ExcelCarimbos/Download/", parametros, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadExcelAjusteETO: (result) => {
    axios
      .get("/Obra/ExcelAjusteETO/Download/", {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadExcelAgendaFaturamento: (parametros, result) => {
    axios
      .post("/Agenda/ExcelFaturamento/Download/", parametros, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  postNotaFiscalPagamento: (objPagamento, result) => {
    axios
    .post(`/PedidoCompra/NotaFiscal/PostNotaFiscalPagamento`, objPagamento)
    .then((apiReturn) => {
      result({ status: apiReturn.status, message: "", data: apiReturn.data });
    })
    .catch((error) => {
      if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
        result({
          status: 500,
          message: "Erro de comunicação com o servidor",
        });
      else
        result({
          status: error.response.status,
          message: error.response.data,
        });
    });
  },

  getNotaFiscalPagamento: (idPedidoCompraNotaFiscal, result) => {
    axios
    .get(`/PedidoCompra/NotaFiscal/ObtemNotaFiscalPagamento/${idPedidoCompraNotaFiscal}`)
    .then((apiReturn) => {
      console.log(apiReturn);
      result({ status: apiReturn.status, message: "", data: apiReturn.data });
    })
    .catch((error) => {
      if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
        result({
          status: 500,
          message: "Erro de comunicação com o servidor",
        });
      else
        result({
          status: error.response.status,
          message: error.response.data,
        });
    });
  },

  downloadExcelFluxoCaixa: (parametros, result) => {
    axios
      .post("/FluxoCaixa/Excel/Download/", parametros, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
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
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadContaCorrenteExcel: (idObra, result) => {
    axios
      .post("/Obra/ContaCorrenteExcel/Download/" + idObra, null, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadResumoETOExcel: (result) => {
    axios
      .post("/Obra/ResumoETOExcel/Download/", null, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadResumoETOPDF: (result) => {
    axios
      .get("/Obra/ResumoETOPdf/Download", { responseType: "blob" })
      .then((apiReturn) => {
        console.log(apiReturn);
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadContaCorrentePDF: (idObra, result) => {
    axios
      .get("/Obra/ContaCorrentePDF/Download/" + idObra, { responseType: "blob" })
      .then((apiReturn) => {
        console.log(apiReturn);
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadEtoObraPdf: (idObra, result) => {
    axios
      .get("/Obra/EtoObraPdf/Download/" + idObra, { responseType: "blob" })
      .then((apiReturn) => {
        console.log(apiReturn);
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadFluxoCaixaPDF: (parametros, result) => {
    axios
      .post("/FluxoCaixa/PDF/Download",parametros, { responseType: "blob" })
      .then((apiReturn) => {
        console.log(apiReturn);
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemPedidosFinalizadosFiltrados: (objParametros, result) => {
    axios
      .post("/PedidoCompra/ObtemFinalizadosFiltrados", objParametros)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemEntradasFinanceiro: (result) => {
    axios
      .get("/Financeiro/ObtemEntradas")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
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
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
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
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
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
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  habilitarAprovacaoAutomatica: (idObjeto, result) => {
    axios
      .post(
        "/Obra/HabilitarAprovacaoAutomatica/" + idObjeto
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "" });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getUnidades: (result) => {
    axios
      .get("/Material/Unidades")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  removerItensSelecionados: (solicitacaoCompraParaCotacao, result) => {
    axios
      .post(
        "/SolicitacaoCompra/RemoverItensSelecionados", solicitacaoCompraParaCotacao
      )
      .then((apiReturn) => {

        result({ status: apiReturn.status, message: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  duplicarItensSelecionados: (solicitacaoCompraParaCotacao, result) => {
    axios
      .post(
        "/SolicitacaoCompra/DuplicarItensSelecionados", solicitacaoCompraParaCotacao
      )
      .then((apiReturn) => {

        result({ status: apiReturn.status, message: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  getPedidosParaConciliacao: (result) => {
    axios
      .get("/PedidoCompra/ParaConciliacao")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemNotasFiscaisReprovadasDiretoria: (result) => {
    axios
      .get("/PedidoCompra/Conciliacao/ReprovadasDirecao")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  reenviarNotaFiscalDiretoria: (idPedidoCompraNotaFiscal, result) => {
    axios
      .post(`/PedidoCompra/Conciliacao/NotaFiscal/ReenviarDiretoria/${idPedidoCompraNotaFiscal}`)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  reenviarNotaFiscalFinanceiro: (idPedidoCompraNotaFiscal, result) => {
    axios
      .post(`/PedidoCompra/Conciliacao/NotaFiscal/ReenviarFinanceiro/${idPedidoCompraNotaFiscal}`)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemNotasFiscaisReprovadasFinanceiro: (result) => {
    axios
      .get("/PedidoCompra/Conciliacao/ReprovadasFinanceiro")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  cancelarSaldoAbrirPedido: (objeto, result) => {
    axios
      .post("/PedidoCompra/Conciliar/CancelarSaldoAbrirPedido", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  trocarFornecedor: (objeto, result) => {
    axios
      .post("/PedidoCompra/Conciliar/TrocarFornecedor", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
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
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  informarPagamentoOuRecebimentoFluxoCaixa: (idFluxoCaixa, novaData, result) => {

    var objDataPagamento = {
      id: idFluxoCaixa,
      data: novaData
    };

    axios
      .post("/FluxoCaixa/InformarPagamentoOuRecebimento", objDataPagamento)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemNotasConciliadas: (objetoPesquisa, result) => {
    axios
      .post("/PedidoCompra/ObtemNotasConciliadas", objetoPesquisa)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
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
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  downloadExcelCotacoes: (id, result) => {
    axios
      .post("/SolicitacaoCompra/Excel/Cotacoes/Download/"+id, null, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  cancelarNotaFiscal: (idPedidoCompraNotaFiscal, result) => {
    axios
      .post(
        `/PedidoCompra/NotaFiscal/Cancelar/${idPedidoCompraNotaFiscal}`
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  alterarObraNF: (idPedidoCompraNotaFiscal, idObra, result) => {
    axios
      .post(
        `/PedidoCompra/NotaFiscal/AlterarObra/${idPedidoCompraNotaFiscal}/${idObra}`
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemFiliais: (idFornecedor, result) => {
    axios
      .get(
        `/Fornecedor/ObtemFiliais/${idFornecedor}`
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  devolverPedidoParaCotacao: (idPedidoCompra, result) => {
    axios
      .post(
        `/PedidoCompra/DevolverParaCotacao/${idPedidoCompra}`
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  listaFornecedoresFiltrados: (objFiltro, result) => {
    axios
      .post("/Fornecedor/GetFiltrado", objFiltro)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  salvaRascunho: (objeto, result) => {
    axios
      .post("/SolicitacaoCompraRascunho", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  atualizaRascunho: (objeto, result) => {
    axios
      .put("/SolicitacaoCompraRascunho", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  obtemRascunhos: (result) => {
    axios
      .get(
        `/SolicitacaoCompraRascunho`
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  excluiRascunho: (id, result) => {
    axios
      .delete(
        `/SolicitacaoCompraRascunho/${id}`
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemHistorico: (objeto, result) => {
    axios
      .post("/Historico", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  obtemCodigosHistorico: (id, result) => {
    axios
      .get("/Historico/RetornaCodigos/"+id)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  obtemMateriaisPaginados: (objeto, result) => {
    axios
      .post("/Material/ObtemMateriaisPaginados", objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  buscaMaterialPorTexto: (material, result) => {
    axios
      .get(
        `/Material/BuscaMaterialPorTexto/${material}`
      )
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemNotasFiscaisAlteracaoData: (id, result) => {
    axios
      .get("/PedidoCompra/NotaFiscal/ObtemNotasFiscaisAlteracaoData/" + id)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  alteraArquivoNotaFiscal: (controller, file, idPedidoCompraArquivo, result) => {
    axios
      .post("/PedidoCompra/NotaFiscal/Alterar/Upload/" + idPedidoCompraArquivo, file, {
        headers: { "Content-Type": "multipart/form-data" },
      })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  uploadAjusteETO: (file, result) => {
    axios
      .post("/Obra/Upload/AjusteETO", file, {
        headers: { "Content-Type": "multipart/form-data" },
      })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemAjustes: (result) => {
    axios
      .get("/Obra/ObtemAjustes")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadArquivoAjusteETO: (id, tipo, result) => {
    axios
      .get("/Obra/AjusteETO/Download/" + id + "/" + tipo, { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadNotaFiscalComCarimbo: (id, result) => {
    axios
      .get("/Agenda/PDF/Carimbo/Download/" + id, { responseType: "blob" })
      .then((apiReturn) => {
        console.log(apiReturn);
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        console.log(error);
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadZipCarimbo: (parametros, result) => {
    axios
      .post("/Agenda/Carimbo/Download/Zip/", parametros, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadPdfPedidoInterno: (id, result) => {
    axios
      .get("/PedidoInterno/Pdf/Download/" + id, { responseType: "blob" })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemHistoricoDeCompra: (idMaterial, result) => {
    axios
      .get(`/Material/ObtemHistoricoDeCompra/${idMaterial}`)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemValoresAFaturar: (objParametros, result) => {
    axios
      .post(`/Faturamento/ObtemValoresAFaturar`, objParametros)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  obtemValoresAFaturarDaObra: (idObra, result) => {
    axios
      .get(`/Faturamento/ObtemValoresAFaturar/${idObra}`)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
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
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  uploadNovoPdfNotaFiscal: (file, idPedidoCompranotaFiscal, result) => {
    axios
      .post("/PedidoCompra/UploadNotaFiscal/" + idPedidoCompranotaFiscal, file, {
        headers: { "Content-Type": "multipart/form-data" },
      })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  salvarAjusteManual: (objetoDatas, idFluxoCaixa, result) => {
    axios
    .post(`/Agenda/AjusteManual/${idFluxoCaixa}`, objetoDatas)
    .then((apiReturn) => {
      result({ status: apiReturn.status, message: "", data: apiReturn.data });
    })
    .catch((error) => {
      if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
        result({
          status: 500,
          message: "Erro de comunicação com o servidor",
        });
      else
        result({
          status: error.response.status,
          message: error.response.data,
        });
    });
  },

  marcarVisualizacaoNotaFiscalReprovada: (idPedidoCompraNotaFiscal, result) => {
    axios
      .post(`/PedidoCompra/NotaFiscal/MarcarVisualizacaoNotaFiscalReprovada/${idPedidoCompraNotaFiscal}`)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  salvarAjusteManualAFaturar: (objeto, result) => {
    axios
      .post(`/Faturamento/AjustaFaturamento`, objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  cancelarFaturamento: (objeto, result) => {
    axios
      .post(`/Faturamento/CancelarFaturamento`, objeto)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  downloadExcelAgendaFaturamento: (parametros, result) => {
    axios
      .post("/Agenda/Excel/Faturamento/Download/", parametros, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  downloadPDFAgendaFaturamento: (parametros, result) => {
    axios
      .post("/Agenda/PDF/Faturamento/Download/", parametros, {
        responseType: "blob",
      })
      .then((apiReturn) => {
        result({
          status: apiReturn.status,
          message: "",
          data: apiReturn.data,
          contentType: apiReturn.headers["content-type"],
          nomeArquivo: apiReturn.headers["content-disposition"],
        });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

  enviarParaRevisao: (id, result) => {
    axios
      .post(`/SolicitacaoCompra/EnviarParaRevisao/${id}`)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  enviarParaAprovacao: (id, result) => {
    axios
      .post(`/SolicitacaoCompra/EnviarParaAprovacao/${id}`)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn });
      })
      .catch((error) => {
        
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
        {
          result({
            status: error.response.status,
            message: error.response.data,
          });
        }
      });
  },

  excluiMaterialCotacao: (idMaterial, idSolicitacaoCompra, result) => {
    axios
      .post(`/SolicitacaoCompra/Cotacao/Material/${idMaterial}/${idSolicitacaoCompra}`)
      .then((apiReturn) => {
        console.log(apiReturn);
        result({ status: apiReturn.data, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({
            status: 500,
            message: "Erro de comunicação com o servidor",
          });
        else
          result({
            status: error.response.status,
            message: error.response.data,
          });
      });
  },

};
