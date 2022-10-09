﻿using System.IO;
using System.Net;
using Autofac;
using BusinessLogic.BusinessObjects;
using BusinessLogic.Repo.Domain;
using BusinessLogic.SQLDriver;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace BusinessLogic;

internal static class ConnectionHelper
{
    private static readonly object lockObject = new object();
    private static ISessionFactory _sessionFactory;
    private static XTradeConfig config;

    private static void InitConfig()
    {
        if (config == null)
            config = MainService.thisGlobal.Container.Resolve<XTradeConfig>();
    }

    public static ISession CreateNewSession()
    {
        if (_sessionFactory == null)
        {
            InitConfig();
            var connection = config.ConnectionString();

            if (config.ConnectionStringName().Contains("SQLite"))
            {
                
                // http://qaru.site/questions/754091/getting-fluent-nhibernate-to-work-with-sqlite
                var dbConfig = MsSqliteConfiguration.Standard.ConnectionString(connection).Dialect<CustomDialect>(); 
                //.Driver<SQLiteCustomDriver>();
                
                // SqliteConnection
                _sessionFactory = Fluently.Configure().Database(dbConfig)
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DBAdviser>())
                    .BuildSessionFactory();
                
            }
            else
            {
                var dbConfig = MySQLConfiguration.Standard.ConnectionString(connection);
                _sessionFactory = Fluently.Configure().Database(dbConfig)
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DBAdviser>())
                    .BuildSessionFactory();
            }
        }

        lock (lockObject) // Session is not thread safe thus - should be locked.
        {
            return _sessionFactory.OpenSession();
        }
    }

    private static string GetComputer_InternetIP()
    {
        // check IP using DynDNS's service
        var request = WebRequest.Create("http://checkip.dyndns.org");
        // IMPORTANT: set Proxy to null, to drastically INCREASE the speed of request
        request.Proxy = null;
        var response = request.GetResponse();
        var stream = new StreamReader(response.GetResponseStream());


        // read complete response
        var html = stream.ReadToEnd();

        // replace everything and keep only IP
        var ipAddress = html.Replace(
                "<html><head><title>Current IP Check</title></head><body>Current IP Address: ", string.Empty)
            .Replace("</body></html>", string.Empty);
        char[] trim = {'\r', '\n'};
        ipAddress = ipAddress.TrimEnd(trim);
        return ipAddress;
    }
}
