import axios from "axios";

export default {
  bloquearDesbloquearObra: (idObjeto, valor, result) => {
    axios
      .post("/Obra/BloquearDesbloquear/" + idObjeto + "/" + valor)
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

  cancelarObra: (idObra, result) => {
    axios
      .post("/Obra/Cancelar/" + idObra)
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

  habilitarAprovacaoAutomatica: (idObjeto, result) => {
    axios
      .post("/Obra/HabilitarAprovacaoAutomatica/" + idObjeto)
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

  downloadContaCorrenteExcel: (idObra, result) => {
    axios
      .post("/Obra/ContaCorrenteExcel/Download/" + idObra, null, { responseType: "blob" })
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

  downloadContaCorrentePDF: (idObra, result) => {
    axios
      .get("/Obra/ContaCorrentePDF/Download/" + idObra, { responseType: "blob" })
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

  downloadResumoETOExcel: (result) => {
    axios
      .post("/Obra/ResumoETOExcel/Download/", null, { responseType: "blob" })
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

  downloadResumoETOPDF: (result) => {
    axios
      .get("/Obra/ResumoETOPdf/Download", { responseType: "blob" })
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

  downloadEtoObraPdf: (idObra, result) => {
    axios
      .get("/Obra/EtoObraPdf/Download/" + idObra, { responseType: "blob" })
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

  downloadExcelAjusteETO: (result) => {
    axios
      .get("/Obra/ExcelAjusteETO/Download/", { responseType: "blob" })
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
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
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else
          result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadArquivoAjusteETO: (id, tipo, result) => {
    axios
      .get("/Obra/AjusteETO/Download/" + id + "/" + tipo, { responseType: "blob" })
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
};
