import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cliente } from '../models/cliente.models';

@Injectable({ providedIn: 'root' })
export class ClienteApiService {
  constructor(private readonly http: HttpClient) {}

  listar(apenasAtivos = true): Observable<Cliente[]> {
    return this.http.get<Cliente[]>('/Cliente?apenasAtivos=' + apenasAtivos);
  }

  salvar(payload: Partial<Cliente>): Observable<unknown> {
    return payload.id ? this.http.put('/Cliente', payload) : this.http.post('/Cliente', payload);
  }
}
