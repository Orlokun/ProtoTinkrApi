using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using TinkrCommon.Settings;

namespace TinkrCommon.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            //Format how to store Dates and Guids
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            
            //GetService settings to construct mongo client
                        
            //Retrieve DataBase
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var _serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var settings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(settings.ConnectionString);
                return mongoClient.GetDatabase(_serviceSettings.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
            where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                var dataBase = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(dataBase, collectionName);
            });
            
            return services;
        }
    }
}