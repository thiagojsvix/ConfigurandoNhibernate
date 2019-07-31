using System;

namespace Dominio
{
    public abstract class Pessoa
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Cep { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string UF { get; set; }
        public virtual string Telefone { get; set; }
    }

    public class PessoaFisica : Pessoa
    {
        public virtual PessoaFisica New()
        {
            var entity = new PessoaFisica()
            {
                Nome = "Alessandra Laura Julia da Cunha",
                Cep = "96830-150",
                Endereco = "Rua Dario Barbosa",
                Numero = "792",
                Bairro = "Bonfim",
                Cidade = "Santa Cruz do Sul",
                UF = "RS",
                Telefone = "(51) 2849-6061",
                DataNascimento = new DateTime(1961, 4, 4),
                CPF = "478.126.449-28"
            };

            return entity;
        }

        public virtual DateTime DataNascimento { get; set; }
        public virtual string CPF { get; set; }

    }

    public class PessoaJuridica : Pessoa
    {
        public virtual string RazaoSocial { get; set; }
        public virtual string CNPJ { get; set; }
        public virtual PessoaJuridica New()
        {
            var entity = new PessoaJuridica()
            {
                Nome = "Inova Consultoria Financeira",
                CNPJ = "33.465.180/0001-36",
                RazaoSocial = "Carlos Eduardo e Ian Consultoria Financeira ME",
                Cep = "29141-752",
                Endereco = "Avenida Principal",
                Numero = "161",
                Bairro = "Rio Marinho",
                Cidade = "Cariacica",
                UF = "ES",
                Telefone = "(27) 2812-9123"
            };

            return entity;
        }
    }
}
