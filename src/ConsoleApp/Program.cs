
using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using Dominio;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migrator;
using Repositorio.Repositorio;
using System.Data.SqlClient;

namespace ConsoleApp
{
    internal class Program
    {
        private static IConfigurationRoot Configuration;

        static async Task Main(string[] args)
        {
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
            await Connection();
        }

        public static async Task Connection()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("======================================================================================================");
            Console.WriteLine("Configurando Nhibernate com FluentNHibernate, Teste de Mapeamento do FluentNhibernate e FluentMigrator");
            Console.WriteLine("======================================================================================================");
            Console.WriteLine("");
            
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Abrindo Conexão com o Banco de Dados");

            using (var db = new SessionHelper(connectionString))
            {
                //Criar Pessoa Fisica
                var pessoaFisica = new PessoaFisica();
                await db.Session.SaveOrUpdateAsync(pessoaFisica.New());

                var pessoaJuridica = new PessoaJuridica();
                await db.Session.SaveOrUpdateAsync(pessoaJuridica.New());

                await db.Session.FlushAsync();
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
            var rootDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..","..",".."));
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
                    Console.WriteLine(result.ToString());
                }
            }
        }
    }
}
