namespace Ecommerce.Domain.Entities
{
    public class Cliente : EntidadeBase
    {
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string Email { get; private set; }

        private readonly List<Endereco> _enderecos = new();
        public IReadOnlyCollection<Endereco> Enderecos => _enderecos;

        protected Cliente() { }

        public Cliente(string nome, string cpf, string email)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
                throw new ArgumentException("CPF deve ter 11 dígitos."); // Validação simples

            Nome = nome;
            CPF = cpf;
            Email = email;
        }

        public void AdicionarEndereco(Endereco endereco) => _enderecos.Add(endereco);
    }
}