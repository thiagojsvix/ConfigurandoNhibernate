using System;
using FluentMigrator;

namespace Migrator
{
    [Migration(2, "Criando Tabela de Animal")]
    public class CriacaoTabelaAnimal : MigrationBase
    {
        public override void Up()
        {
            Create.Table("Animal")
                .WithColumn("AnimalId").AsInt32().PrimaryKey().Identity()
                .WithColumn("Nome").AsString(100).NotNullable()
                ;

            Create.Table("Cachorro")
                .WithColumn("CachorroId").AsInt32().PrimaryKey()
                .WithColumn("Pelo").AsString(20).NotNullable()
                ;

            Create.Table("Papagaio")
                .WithColumn("PapagaioId").AsInt32().PrimaryKey()
                .WithColumn("Plumagem").AsString(20).NotNullable()
                ;
        }

        public override void Down() => throw new NotImplementedException();
    }

}
