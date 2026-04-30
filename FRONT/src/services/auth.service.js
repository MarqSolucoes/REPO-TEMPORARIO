import axios from "axios";

export default {
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },
};
