import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthApiService } from '../../core/services/auth-api.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly authApi = inject(AuthApiService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  isFetching = false;
  errorMessage = '';

  loginForm = this.fb.nonNullable.group({
    usuario: ['', [Validators.required]],
    senha: ['', [Validators.required]],
  });

  ngOnInit(): void {
    const token = this.route.snapshot.queryParamMap.get('token');
    if (token) {
      this.receiveToken(token);
    } else if (localStorage.getItem('token')) {
      this.receiveLogin();
    }
  }

  async entrar(): Promise<void> {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.errorMessage = '';
    this.isFetching = true;

    const { usuario, senha } = this.loginForm.getRawValue();
    const senhaHash = await this.authApi.sha256(senha);

    this.authApi
      .login(usuario, senhaHash)
      .pipe(finalize(() => (this.isFetching = false)))
      .subscribe({
        next: (token) => this.receiveToken(token),
        error: () => {
          this.errorMessage = 'Erro ao efetuar login. Verifique os dados informados';
        },
      });
  }

  private receiveToken(token: string): void {
    const payload = this.authApi.extrairPayloadJwt(token);
    localStorage.setItem('token', token);

    if (payload) {
      localStorage.setItem('user', JSON.stringify({ id: payload.Id, nome: payload.Nome }));
      this.authApi.obterUsuario(payload.Id).subscribe({
        next: (usuarioDTO) => {
          localStorage.setItem('usuarioDTO', JSON.stringify(usuarioDTO));
          this.receiveLogin();
        },
        error: () => this.receiveLogin(),
      });
      return;
    }

    this.receiveLogin();
  }

  private receiveLogin(): void {
    this.router.navigateByUrl('/dashboard');
  }
}
