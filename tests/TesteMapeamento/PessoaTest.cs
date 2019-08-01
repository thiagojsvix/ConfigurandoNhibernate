using System;
using System.IO;
using Dominio;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Testing;
using Microsoft.Extensions.Configuration;
using Repositorio.Mapeamento;
using TesteMapeamento.Comparador;
using Xunit;

namespace TesteMapeamento
{
    public class PessoaTest
    {
        private readonly IConfigurationRoot Configuration;

        public PessoaTest()
        {
            var rootDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "src", "ConsoleApp"));

            var builder = new ConfigurationBuilder()
                .SetBasePath(rootDir)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.Configuration = builder.Build();
        }

        [Fact]
        public void MappingTest()
        {
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(this.Configuration.GetConnectionString("DefaultConnection")))
                .Mappings(m =>
                    m.FluentMappings
                        .AddFromAssemblyOf<PessoaMap>())
                .BuildSessionFactory();

            using (var nh = sessionFactory.OpenSession())
            {
                new PersistenceSpecification<PessoaFisica>(nh)
                        .CheckProperty(c => c.CPF, "CPF")
                        .CheckProperty(c => c.Nome, "Nome")
                        .CheckProperty(c => c.Cep, "Cep")
                        .CheckProperty(c => c.Endereco, "Endereco")
                        .CheckProperty(c => c.Numero, "Numero")
                        .CheckProperty(c => c.Bairro, "Bairro")
                        .CheckProperty(c => c.Cidade, "Cidade")
                        .CheckProperty(c => c.Cidade, "Cidade")
                        .CheckProperty(c => c.UF, "UF")
                        .CheckProperty(c => c.Telefone, "Telefone")
                        .CheckProperty(c => c.DataNascimento, DateTime.Now, new ComparadorData())
                        .VerifyTheMappings();
            }
        }
    }
}
