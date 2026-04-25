import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  isFetching$ = new BehaviorSubject<boolean>(false);
  errorMessage$ = new BehaviorSubject<string>('');

  constructor(private http: HttpClient, private router: Router) {}

  private decodeJwt(token: string): any {
    try {
      const parts = token.split('.');
      if (parts.length < 2) return null;
      return JSON.parse(atob(parts[1]));
    } catch { return null; }
  }

  isAuthenticated(token: string | null): boolean {
    if (!token) return false;
    const payload = this.decodeJwt(token);
    if (!payload) return false;
    return Date.now() / 1000 < payload.exp;
  }

  loginUser(creds: { usuario: string; senha: string }): void {
    this.isFetching$.next(true);
    this.errorMessage$.next('');
    this.http.post(
      `${environment.baseURLApi}/auth/token?user=${creds.usuario}&password=${creds.senha}`,
      {},
      { responseType: 'text' }
    ).subscribe({
      next: (token) => this.receiveToken(token),
      error: () => {
        this.isFetching$.next(false);
        this.errorMessage$.next('Erro ao efetuar login. Verifique os dados informados');
      }
    });
  }

  receiveToken(token: string): void {
    const payload = this.decodeJwt(token);
    const user = payload ? { id: payload.Id, nome: payload.Nome } : {};
    localStorage.setItem('token', token);
    localStorage.setItem('user', JSON.stringify(user));
    this.receiveLogin();
  }

  receiveLogin(): void {
    this.isFetching$.next(false);
    this.errorMessage$.next('');
    this.router.navigate(['/app/dashboard']);
  }

  logoutUser(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    localStorage.removeItem('usuarioDTO');
    this.router.navigate(['/login']);
  }
}
