import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProdutoService } from '../../../core/services/produto.service';

@Component({
  selector: 'app-cadastro-produto',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cadastro-produto.component.html',
  styleUrl: './cadastro-produto.component.scss'
})
export class CadastroProdutoComponent implements OnInit {
  private produtoService = inject(ProdutoService);

  // Formulário
  nome = '';
  sku = '';
  precoVenda = 0;
  estoque = 10;
  
  loading = signal(false);
  
  // Lista de Produtos Existentes
  produtosLista = signal<any[]>([]);

  ngOnInit() {
    this.carregarProdutos();
  }

  carregarProdutos() {
    this.produtoService.obterTodos().subscribe({
      next: (res: any) => {
        this.produtosLista.set(res.itens || res); 
      },
      error: (err) => console.error('Erro ao listar produtos', err)
    });
  }

  salvar() {
    if (!this.nome || !this.sku || this.precoVenda <= 0) {
      alert('Preencha os campos corretamente.');
      return;
    }

    this.loading.set(true);
    
    const dto = {
      nome: this.nome,
      sku: this.sku,
      precoCusto: this.precoVenda * 0.5,
      precoVenda: this.precoVenda,
      estoqueInicial: this.estoque
    };

    this.produtoService.cadastrar(dto).subscribe({
      next: () => {
        alert('Produto cadastrado com sucesso! 📦');
        
        this.nome = ''; this.sku = ''; this.precoVenda = 0;
        
        this.loading.set(false);
        
        this.carregarProdutos();
      },
      error: (err) => {
        alert('Erro: ' + (err.error?.message || 'Falha ao cadastrar'));
        this.loading.set(false);
      }
    });
  }
}