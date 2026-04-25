import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  usuario = '';
  senha = '';
  isFetching = false;
  errorMessage = '';
  private subs: Subscription[] = [];

  constructor(private auth: AuthService) {}

  ngOnInit(): void {
    this.subs.push(this.auth.isFetching$.subscribe(v => this.isFetching = v));
    this.subs.push(this.auth.errorMessage$.subscribe(v => this.errorMessage = v));
    const token = localStorage.getItem('token');
    if (this.auth.isAuthenticated(token)) this.auth.receiveLogin();
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }

  login(): void {
    if (this.usuario && this.senha) {
      this.auth.loginUser({ usuario: this.usuario, senha: this.senha });
    }
  }
}
