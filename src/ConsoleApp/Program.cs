
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Dominio;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migrator;
using Repositorio.Repositorio;

namespace ConsoleApp
{
    internal class Program
    {
        private static IConfigurationRoot Configuration;

        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            ConsoleWrite.Color("======================================================================================================", ConsoleColor.Yellow);
            ConsoleWrite.Color("Configurando Nhibernate com FluentNHibernate, Teste de Mapeamento do FluentNhibernate e FluentMigrator", ConsoleColor.Yellow);
            ConsoleWrite.Color("======================================================================================================", ConsoleColor.Yellow);

            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            CreateDatabase();
            var serviceProvider = CreateServices();
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }

            await ConnectionPessoa();
            await ConnectionAnimal();
        }

        public static async Task ConnectionPessoa()
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            using (var db = new SessionHelper(connectionString))
            {
                var pessoaFisica = new PessoaFisica();
                await db.Session.SaveOrUpdateAsync(pessoaFisica.New());

                var pessoaJuridica = new PessoaJuridica();
                await db.Session.SaveOrUpdateAsync(pessoaJuridica.New());

                await db.Session.FlushAsync();

                var pessoas = db.Session.Query<Pessoa>().Count();
                ConsoleWrite.Color($"Quantidade de entidades do tipo Pessoa: {pessoas}", ConsoleColor.Green);

                var pessoasFisicas = db.Session.Query<PessoaFisica>().Count();
                ConsoleWrite.Color($"Quantidade de entidades do tipo PessoaFisica: {pessoasFisicas}", ConsoleColor.Green);

                var pessoasJuridicas = db.Session.Query<PessoaJuridica>().Count();
                ConsoleWrite.Color($"Quantidade de entidades do tipo PessoaJuridica: {pessoasJuridicas}", ConsoleColor.Green);
            }
        }

        public static async Task ConnectionAnimal()
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            using (var db = new SessionHelper(connectionString))
            {
                var cachorro = new Cachorro();
                await db.Session.SaveOrUpdateAsync(cachorro.New());

                var papagaio = new Papagaio();
                await db.Session.SaveOrUpdateAsync(papagaio.New());

                await db.Session.FlushAsync();

                var animais = db.Session.Query<Animal>().Count();
                ConsoleWrite.Color($"Quantidade de entidades do tipo Animal: {animais}", ConsoleColor.Green);

                var cachorros = db.Session.Query<Cachorro>().Count();
                ConsoleWrite.Color($"Quantidade de entidades do tipo Cachorros: {cachorros}", ConsoleColor.Green);

                var papagaios = db.Session.Query<Papagaio>().Count();
                ConsoleWrite.Color($"Quantidade de entidades do tipo Papagaios: {papagaios}", ConsoleColor.Green);
            }
        }

        public static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer2016()
                    .WithGlobalConnectionString(Configuration.GetConnectionString("DefaultConnection"))
                    .ScanIn(typeof(CriacaoTabelaPessoa).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        public static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        public static void CreateDatabase()
        {
            var conn = new SqlConnectionStringBuilder(Configuration.GetConnectionString("DefaultConnection"));
            var rootDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", ".."));
            var dbName = conn.InitialCatalog;
            var dbServer = conn.DataSource;
            var dbTrusted = conn.IntegratedSecurity;

            using (var ps = PowerShell.Create())
            {
                ps.AddScript($@"if (!(Test-Path ""{rootDir}\Database"")) {{
                                    mkdir ""{rootDir}\Database""
                                }}");

                var script = $@"
                                IF EXISTS(SELECT * FROM sys.databases WHERE name='{dbName}')
                                BEGIN
                                    ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                    DROP DATABASE [{dbName}];
                                END
                                CREATE DATABASE [{dbName}] On (NAME = '{dbName}', FILENAME = '{rootDir}\Database\{dbName}.mdf') COLLATE SQL_Latin1_General_CP1_CI_AI;
                            ";

                var exec = $@"sqlcmd -S ""{dbServer}"" $(&{{ If({dbTrusted}) {{ ""-E"" }} }}) -Q ""{script}"" ";

                ps.AddScript(exec);

                var results = ps.Invoke();
                foreach (var result in results)
                {
                    Debug.Write(result.ToString());
                    ConsoleWrite.Color(result.ToString());
                }
            }
        }
    }
}
