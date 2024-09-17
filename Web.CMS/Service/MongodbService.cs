using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace WEB.Adavigo.CMS.Service
{
    public class MongodbService
    {
        public static IMongoDatabase GetDatabase()
        {
            var host = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("DataBaseConfig").GetSection("MongoServer")["Host"];
            var port = int.Parse(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("DataBaseConfig").GetSection("MongoServer")["Port"]);
            var catalog_log = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("DataBaseConfig").GetSection("MongoServer")["catalog_log"];
            var user = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("DataBaseConfig").GetSection("MongoServer")["user"];
            var pwd = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("DataBaseConfig").GetSection("MongoServer")["pwd"];
            var cred = MongoCredential.CreateCredential(catalog_log, user, pwd);
            var client = new MongoClient(
                    new MongoClientSettings()
                    {
                        Server = new MongoServerAddress(host, port),
                        ClusterConfigurator = cb =>
                        {
                            //var textWriter = TextWriter.Synchronized(new StreamWriter("mylogfile.txt"));
                            cb.Subscribe<CommandStartedEvent>(e =>
                            {
                                //log.Debug(e.Command.ToString());
                                //LogHelper.InsertLogTelegram(e.Command.ToString());
                            });
                        },
                        Credential = cred
                    });

            //them tinh nang bo qua cac truong co trong db nhung khong co trong mo ta class
            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
            var db = client.GetDatabase(catalog_log);
            return db;
        }
    }
}
