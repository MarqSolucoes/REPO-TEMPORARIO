import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cidade } from '../models/cidade.models';

@Injectable({ providedIn: 'root' })
export class CidadeApiService {
  constructor(private readonly http: HttpClient) {}
  listar(apenasAtivos = true): Observable<Cidade[]> { return this.http.get<Cidade[]>('/Cidade?apenasAtivos=' + apenasAtivos); }
  salvar(payload: Partial<Cidade>): Observable<unknown> { return payload.id ? this.http.put('/Cidade', payload) : this.http.post('/Cidade', payload); }
}
