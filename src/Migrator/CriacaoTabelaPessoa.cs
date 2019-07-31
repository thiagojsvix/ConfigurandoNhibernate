using System;
using FluentMigrator;

namespace Migrator
{
    [Migration(1, "Criando Tabela de Pessoa")]
    public class CriacaoTabelaPessoa : MigrationBase
    {
        public override void Up()
        {
            Create.Table("Pessoa")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Nome").AsString(100).NotNullable()
                .WithColumn("Cep").AsString(10).Nullable()
                .WithColumn("Endereco").AsString(100).Nullable()
                .WithColumn("Numero").AsString(10).Nullable()
                .WithColumn("Bairro").AsString(50).Nullable()
                .WithColumn("Cidade").AsString(50).Nullable()
                .WithColumn("UF").AsString(2).Nullable()
                .WithColumn("Telefone").AsString(15).Nullable()
                .WithColumn("DataNascimento").AsDateTime().Nullable()
                .WithColumn("Cpf").AsString(15).Nullable()
                .WithColumn("CNPJ").AsString(20).Nullable()
                .WithColumn("RazaoSocial").AsString(100).Nullable()
                .WithColumn("Tipo").AsString(20).NotNullable().Nullable()
                ;
        }

        public override void Down() => throw new NotImplementedException();
    }

}
