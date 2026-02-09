import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClienteService } from '../../services/cliente.service';
@Component({
  selector: 'app-cadastro-cliente',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cadastro-cliente.component.html',
  styleUrl: './cadastro-cliente.componente.scss' 
})
export class CadastroClienteComponent implements OnInit {
  clienteService = inject(ClienteService);

  nome = '';
  email = '';
  cpf = '';
  loading = signal(false);
  clientesLista = signal<any[]>([]);

  ngOnInit() {
    this.carregarClientes();
  }

  carregarClientes() {
    this.clienteService.listar().subscribe({
      next: (dados) => this.clientesLista.set(dados),
      error: (e) => console.error('Erro ao listar clientes', e)
    });
  }

  cadastrar() {
    this.loading.set(true);
    const dto = { nome: this.nome, email: this.email, cpf: this.cpf };

    this.clienteService.cadastrar(dto).subscribe({
      next: () => {
        alert('Cliente cadastrado!');
        this.nome = ''; this.email = ''; this.cpf = '';
        this.loading.set(false);
        this.carregarClientes(); // Atualiza lista
      },
      error: (err) => {
        alert('Erro: ' + (err.error || 'Falha'));
        this.loading.set(false);
      }
    });
  }
}