namespace Ecommerce.Domain.Entities
{
    public class Endereco : EntidadeBase
    {
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string CEP { get; private set; }
        public string Cidade { get; private set; }
        public int ClienteId { get; set; }

        protected Endereco() { }

        public Endereco(string logradouro, string numero, string cep, string cidade)
        {
            Logradouro = logradouro;
            Numero = numero;
            CEP = cep;
            Cidade = cidade;
        }
    }
}