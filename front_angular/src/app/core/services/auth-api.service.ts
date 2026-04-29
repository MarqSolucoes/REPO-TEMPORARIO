import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, switchMap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  constructor(private readonly http: HttpClient) {}

  login(usuario: string, senhaHash: string): Observable<string> {
    return this.http.post('/auth/token?user=' + encodeURIComponent(usuario) + '&password=' + encodeURIComponent(senhaHash), null, {
      responseType: 'text',
    });
  }

  obterUsuario(id: number): Observable<unknown> {
    return this.http.get('/Usuario/' + id);
  }

  async sha256(value: string): Promise<string> {
    const data = new TextEncoder().encode(value);
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    return Array.from(new Uint8Array(hashBuffer))
      .map((b) => b.toString(16).padStart(2, '0'))
      .join('');
  }

  extrairPayloadJwt(token: string): { Id: number; Nome: string } | null {
    try {
      const payloadBase64 = token.split('.')[1];
      const payloadJson = atob(payloadBase64.replace(/-/g, '+').replace(/_/g, '/'));
      return JSON.parse(payloadJson) as { Id: number; Nome: string };
    } catch {
      return null;
    }
  }
}
