using Dominio;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PessoaMap : ClassMap<Pessoa>
    {
        public PessoaMap()
        {
            Table("Pessoa");
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Bairro).Not.Nullable().Length(50);
            Map(x => x.Cidade).Not.Nullable().Length(50);
            Map(x => x.Nome).Not.Nullable().Length(50);
            Map(x => x.Numero).Not.Nullable().Length(10);
            Map(x => x.Cep).Not.Nullable().Length(255);
            Map(x => x.Endereco).Not.Nullable().Length(255);
            Map(x => x.Telefone).Not.Nullable().Length(10);
            Map(x => x.UF).Not.Nullable().Length(2);

            DiscriminateSubClassesOnColumn("Tipo").Not.Nullable().Length(50);
        }
    }

    public class PessoaFisicaMap : SubclassMap<PessoaFisica>
    {
        public PessoaFisicaMap()
        {
            Map(x => x.CPF).Not.Nullable().Length(11);
            Map(x => x.DataNascimento).Not.Nullable();
        }
    }

    public class PessoaJuridicaMap : SubclassMap<PessoaJuridica>
    {
        public PessoaJuridicaMap()
        {
            Map(x => x.CNPJ).Not.Nullable().Length(14);
            Map(x => x.RazaoSocial).Not.Nullable().Length(50);
        }
    }
}
