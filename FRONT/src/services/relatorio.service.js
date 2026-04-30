import axios from "axios";

export default {
  // --- Relatório Controle ETO ---
  obtemRelatorioControleETO: (obraBloqueada, result) => {
    axios
      .get("/RelatorioControleETO/" + obraBloqueada)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadRelatorioETOExcelComAjuste: (result) => {
    axios
      .post("/RelatorioControleETO/ExcelComAjuste/Download/", null, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadRelatorioETOExcel: (result) => {
    axios
      .post("/RelatorioControleETO/Excel/Download/", null, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadRelatorioETOPDF: (result) => {
    axios
      .get("/RelatorioControleETO/Pdf/Download", { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemProximasDatasAjuste: (idObra, result) => {
    axios
      .get("/RelatorioControleETO/GetProximasDatasAjuste/" + idObra)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  informaAjustes: (objAjuste, result) => {
    axios
      .post("/RelatorioControleETO/Ajuste", objAjuste)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  visualizaComentarioAjuste: (idAjuste, result) => {
    axios
      .post("/RelatorioControleETO/VisualizarComentario/" + idAjuste)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  // --- Histórico ---
  obtemHistorico: (objeto, result) => {
    axios
      .post("/Historico", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemCodigosHistorico: (id, result) => {
    axios
      .get("/Historico/RetornaCodigos/" + id)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },
};
