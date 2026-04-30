import axios from "axios";

export default {
  obtemAgenda: (parametros, result) => {
    axios
      .post("/Agenda/ObtemAgenda", parametros)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  obtemAgendaFaturamento: (parametros, result) => {
    axios
      .post("/Agenda/ObtemAgendaFaturamento", parametros)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  alteraDefAgenda: (id, idDef, result) => {
    axios
      .post("/Agenda/AlteraDEF/" + id + "/" + idDef)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  alteraValorAgenda: (_id, _valor, result) => {
    var objeto = { id: _id, valor: _valor };
    axios
      .post("/Agenda/AlteraValor", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  alterarDataAgenda: (_id, _data, result) => {
    var objeto = { id: _id, data: _data };
    axios
      .post("/Agenda/AlteraDataPagamento", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  informarPagamentoRecebimentoEmLote: (objeto, result) => {
    axios
      .post("/Agenda/InformarPagamentoRecebimentoLote", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  alterarDefEmLote: (objeto, result) => {
    axios
      .post("/Agenda/AlteraDefLote", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  alterarDataAgendaEmLote: (objeto, result) => {
    axios
      .post("/Agenda/AlteraDataPagamentoLote", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  cancelarPagamentoRecebimento: (objeto, result) => {
    axios
      .post("/Agenda/CancelarPagamentoRecebimento", objeto)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadPDFAgenda: (parametros, result) => {
    axios
      .post("/Agenda/PDF/Download", parametros, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadPDFAgendaFaturamento: (parametros, result) => {
    axios
      .post("/Agenda/PDFFaturamento/Download", parametros, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadExcelAgenda: (parametros, result) => {
    axios
      .post("/Agenda/Excel/Download/", parametros, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadExcelCarimbo: (parametros, result) => {
    axios
      .post("/Agenda/ExcelCarimbos/Download/", parametros, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadExcelAgendaFaturamento: (parametros, result) => {
    axios
      .post("/Agenda/ExcelFaturamento/Download/", parametros, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  salvarAjusteManual: (objetoDatas, idFluxoCaixa, result) => {
    axios
      .post("/Agenda/AjusteManual/" + idFluxoCaixa, objetoDatas)
      .then((apiReturn) => { result({ status: apiReturn.status, message: "", data: apiReturn.data }); })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadNotaFiscalComCarimbo: (id, result) => {
    axios
      .get("/Agenda/PDF/Carimbo/Download/" + id, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },

  downloadZipCarimbo: (parametros, result) => {
    axios
      .post("/Agenda/Carimbo/Download/Zip/", parametros, { responseType: "blob" })
      .then((apiReturn) => {
        result({ status: apiReturn.status, message: "", data: apiReturn.data, contentType: apiReturn.headers["content-type"], nomeArquivo: apiReturn.headers["content-disposition"] });
      })
      .catch((error) => {
        if (JSON.parse(JSON.stringify(error)).code == "ECONNABORTED")
          result({ status: 500, message: "Erro de comunicação com o servidor" });
        else result({ status: error.response.status, message: error.response.data });
      });
  },
};
