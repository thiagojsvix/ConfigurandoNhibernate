using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using Repositorio.Mapeamento;

namespace Repositorio.Repositorio
{

    public  class SessionHelper:IDisposable
    {
        public readonly ISession Session;

        public SessionHelper(string conn)
        {
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(conn)
                    .ShowSql().FormatSql())
                .Mappings(m =>
                    m.FluentMappings
                        .AddFromAssemblyOf<PessoaMap>())
                .BuildSessionFactory();

            this.Session = sessionFactory.OpenSession();
        }

        public void Dispose()
        {
            this.Session?.Dispose();
        }
    }


}
