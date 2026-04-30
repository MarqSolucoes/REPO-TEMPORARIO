import axios from "axios";

export default {
  getPedidosInternosUsuario: (result) => {
    axios
      .get("/PedidoInterno/Usuario/")
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  habilitarDesabilitarPedidoInterno: (controller, idObjeto, valor, result) => {
    axios
      .post("/" + controller + "/HabilitarDesabilitarPI?id=" + idObjeto + "&habilitarDesabilitar=" + valor)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "" });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  aprovarReprovarPedidoInterno: (idPedidoInterno, valor, result) => {
    axios
      .post("/PedidoInterno/AprovarReprovarPedidoInterno/" + idPedidoInterno + "/" + valor)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadPdfPedidoInterno: (id, result) => {
    axios
      .get("/PedidoInterno/Pdf/Download/" + id, { responseType: "blob" })
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
        else result({ status: error.response.status, message: error.response.data });
      });
  },
};
