import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EntradaFinanceira, NotaFiscalFinanceira } from '../models/financeiro.models';

@Injectable({ providedIn: 'root' })
export class FinanceiroApiService {
  constructor(private readonly http: HttpClient) {}

  obtemEntradas(): Observable<EntradaFinanceira[]> {
    return this.http.get<EntradaFinanceira[]>('/Financeiro/ObtemEntradas');
  }

  obtemNotasFiscaisFinanceiro(): Observable<NotaFiscalFinanceira[]> {
    return this.http.get<NotaFiscalFinanceira[]>('/Financeiro/ObtemEntradasRecusadas');
  }

  downloadRelatorioPdf(): Observable<Blob> {
    return this.http.get('/RelatorioFinanceiro/Pdf/Download', { responseType: 'blob' });
  }
}
