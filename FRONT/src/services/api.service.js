import authService from "./auth.service";
import cadastroService from "./cadastro.service";
import obraService from "./obra.service";
import financeiroService from "./financeiro.service";
import comprasService from "./compras.service";
import pedidoInternoService from "./pedidointerno.service";
import agendaService from "./agenda.service";
import relatorioService from "./relatorio.service";

export default {
  ...authService,
  ...cadastroService,
  ...obraService,
  ...financeiroService,
  ...comprasService,
  ...pedidoInternoService,
  ...agendaService,
  ...relatorioService,
};
