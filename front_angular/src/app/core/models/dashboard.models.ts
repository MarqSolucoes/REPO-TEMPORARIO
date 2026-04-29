export interface NotaFiscalVencimento {
  nome: string;
  dataVencimento: string;
}

export interface RascunhoSolicitacao {
  id: number;
  dataCadastro: string;
  titulo: string;
  objetoSerializado?: string;
}

export interface ApiEnvelope<T> {
  status: number;
  data: T;
  message?: string;
}
