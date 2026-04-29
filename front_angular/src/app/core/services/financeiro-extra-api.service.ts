import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CustoFixoResumo, FaturamentoResumo, FolhaPagamentoResumo } from '../models/financeiro-extra.models';

@Injectable({ providedIn: 'root' })
export class FinanceiroExtraApiService {
  constructor(private readonly http: HttpClient) {}

  obtemFaturamentos(): Observable<FaturamentoResumo[]> {
    return this.http.post<FaturamentoResumo[]>('/Faturamento/GetFiltrado', {});
  }

  obtemFolhaPagamento(): Observable<FolhaPagamentoResumo[]> {
    return this.http.get<FolhaPagamentoResumo[]>('/FolhaPagamento?apenasAtivos=true');
  }

  obtemCustosFixos(): Observable<CustoFixoResumo[]> {
    return this.http.get<CustoFixoResumo[]>('/CustoFixo?apenasAtivos=true');
  }
}
