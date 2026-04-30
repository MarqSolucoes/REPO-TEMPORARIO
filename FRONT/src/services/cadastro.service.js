import axios from "axios";

export default {
  get: (controller, idObjeto, result) => {
    axios
      .get("/" + controller + "/" + idObjeto)
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

  getAll: (controller, apenasAtivos, result) => {
    axios
      .get("/" + controller + "?apenasAtivos=" + apenasAtivos)
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

  post: (controller, objetoDTO, result) => {
    axios
      .post("/" + controller, objetoDTO)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: apiReturn.data, data: apiReturn.data });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  ativarDesativar: (controller, idObjeto, valor, result) => {
    axios
      .post("/" + controller + "/AtivarDesativar?id=" + idObjeto + "&ativarDesativar=" + valor)
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "" });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemFiliais: (idFornecedor, result) => {
    axios
      .get("/Fornecedor/ObtemFiliais/" + idFornecedor)
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

  buscaMaterialPorTexto: (material, result) => {
    axios
      .get("/Material/BuscaMaterialPorTexto/" + material)
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

  obtemMateriaisPaginados: (objeto, result) => {
    axios
      .post("/Material/ObtemMateriaisPaginados", objeto)
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

  obtemHistoricoDeCompra: (idMaterial, result) => {
    axios
      .get("/Material/ObtemHistoricoDeCompra/" + idMaterial)
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
};
