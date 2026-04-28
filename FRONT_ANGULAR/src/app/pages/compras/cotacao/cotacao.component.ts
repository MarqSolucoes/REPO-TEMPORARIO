import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-cotacao',
  standalone: false,
  templateUrl: './cotacao.component.html',
  styleUrls: ['./cotacao.component.scss']
})
export class CotacaoComponent implements OnInit {
  isLoading = false;
  id = 0;
  // Data from GET /SolicitacaoCompra/{id}
  solicitacaoCompra: any = null;
  // Data from GET /SolicitacaoCompra/ParaCotacao/{id}
  solicitacaoCompraParaCotacao: any = null;

  constructor(private api: ApiService, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.id = +this.route.snapshot.params['id'];
    this.obtemSolicitacao();
    this.obtemSolicitacaoParaCotacao();
  }

  obtemSolicitacao(): void {
    this.isLoading = true;
    this.api.get('SolicitacaoCompra', this.id, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.solicitacaoCompra = result.data;
      }
    });
  }

  obtemSolicitacaoParaCotacao(): void {
    this.api.getSolicitacaoCompraParaCotacao(this.id, (result) => {
      if (result.status === 200) {
        this.solicitacaoCompraParaCotacao = result.data;
      }
    });
  }

  centroCusto(): string {
    const s = this.solicitacaoCompra;
    if (!s) return '';
    if (s.idCentroCustoDEF != null) {
      return s.centroCustoDEF ? s.centroCustoDEF.descricao : '';
    }
    const cod = s.centroCustoObra ? s.centroCustoObra.codigo : '';
    const cli = s.centroCustoObra && s.centroCustoObra.cliente ? s.centroCustoObra.cliente.nomeFantasia : '';
    return `${cod} - ${cli}`;
  }

  formataMoeda(v: number): string {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
  }

  formataData(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleDateString('pt-BR');
  }

  trackByIndex(i: number): number { return i; }

  voltar(): void {
    this.router.navigate(['/app/compras/ordemCompra/EmCotacao']);
  }
}

