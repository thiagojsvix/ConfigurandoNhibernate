using Dominio;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class AnimalMap : ClassMap<Animal>
    {
        public AnimalMap()
        {
            Table("Animal");
            Id(x => x.Id, "AnimalId").GeneratedBy.Identity();
            Map(x => x.Nome).Not.Nullable().Length(50);
        }
    }

    public class CachorroMap : SubclassMap<Cachorro>
    {
        public CachorroMap()
        {
            Table("Cachorro");
            KeyColumn("CachorroId");
            Map(x => x.Pelo).Not.Nullable().Length(20);
        }
    }

    public class PapagaioMap : SubclassMap<Papagaio>
    {
        public PapagaioMap()
        {
            Table("Papagaio");
            KeyColumn("PapagaioId");
            Map(x => x.Plumagem).Not.Nullable().Length(20);
        }
    }
}
