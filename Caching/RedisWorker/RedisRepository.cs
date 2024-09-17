using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caching.RedisWorker
{
    public class RedisRepository : IRedisRepository
    {
        private static string _RedisHost;
        public RedisRepository(string redisConfig)
        {
            _RedisHost = redisConfig;
        }

        public string Get(string key)
        {
            try
            {
                using (var _redis = ConnectionMultiplexer.Connect(_RedisHost))
                {
                    var db = _redis.GetDatabase();
                    return db.StringGet(key);
                }
            }
            catch
            {

            }
            return null;
        }

        public void Remove(string key)
        {
            try
            {
                using (var _redis = ConnectionMultiplexer.Connect(_RedisHost))
                {
                    var db = _redis.GetDatabase();
                    db.KeyDelete(key);
                }
            }
            catch
            {

            }
        }

        public void Set(string key, string value)
        {
            try
            {
                using (var _redis = ConnectionMultiplexer.Connect(_RedisHost))
                {
                    var db = _redis.GetDatabase();
                    db.StringSet(key, value);
                }
            }
            catch
            {

            }
        }
    }
}
