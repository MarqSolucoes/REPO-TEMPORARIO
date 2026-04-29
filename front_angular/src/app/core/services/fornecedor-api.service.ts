import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Fornecedor } from '../models/fornecedor.models';

@Injectable({ providedIn: 'root' })
export class FornecedorApiService {
  constructor(private readonly http: HttpClient) {}
  listar(apenasAtivos = true): Observable<Fornecedor[]> { return this.http.get<Fornecedor[]>('/Fornecedor?apenasAtivos=' + apenasAtivos); }
  salvar(payload: Partial<Fornecedor>): Observable<unknown> { return payload.id ? this.http.put('/Fornecedor', payload) : this.http.post('/Fornecedor', payload); }
}
