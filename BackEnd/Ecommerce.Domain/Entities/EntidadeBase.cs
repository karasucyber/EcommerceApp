using System;

namespace Ecommerce.Domain.Entities
{
    public abstract class EntidadeBase
    {
        public int Id { get; protected set; }
        public bool Ativo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime? DataAtualizacao { get; private set; }

        protected EntidadeBase()
        {
            Ativo = true; 
            DataCadastro = DateTime.Now;
        }
        protected void AtualizarData()
        {
            DataAtualizacao = DateTime.Now;
        }

        public void Desativar()
        {
            if (Ativo)
            {
                Ativo = false;
                AtualizarData();
            }
        }
        public void Ativar()
        {
            if (!Ativo)
            {
                Ativo = true;
                AtualizarData();
            }
        }

    }
}