import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Perfil } from '../models/perfil.models';

@Injectable({ providedIn: 'root' })
export class PerfilApiService {
  constructor(private readonly http: HttpClient) {}
  listar(apenasAtivos = true): Observable<Perfil[]> { return this.http.get<Perfil[]>('/Perfil?apenasAtivos=' + apenasAtivos); }
  salvar(payload: Partial<Perfil>): Observable<unknown> { return payload.id ? this.http.put('/Perfil', payload) : this.http.post('/Perfil', payload); }
}
