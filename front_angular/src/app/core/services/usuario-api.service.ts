import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Usuario } from '../models/usuario.models';

@Injectable({ providedIn: 'root' })
export class UsuarioApiService {
  constructor(private readonly http: HttpClient) {}
  listar(apenasAtivos = true): Observable<Usuario[]> { return this.http.get<Usuario[]>('/Usuario?apenasAtivos=' + apenasAtivos); }
  salvar(payload: Partial<Usuario>): Observable<unknown> { return payload.id ? this.http.put('/Usuario', payload) : this.http.post('/Usuario', payload); }
}
