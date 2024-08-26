﻿using Api.Users.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Api.Users.Infrastructure.Database;

public static class Setup
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        // Changes Id format from "ObjectID with 96 bits" to "GUID with 128 bits"
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

        services.AddSingleton(sp =>
            new MongoUrl(sp.GetRequiredService<IConfiguration>()
                .GetConnectionString("Default")!));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var mongoUrl = sp.GetRequiredService<MongoUrl>();
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);

            return new MongoClient(mongoClientSettings);
        });

        services.AddSingleton(sp =>
        {
            var mongoUrl = sp.GetRequiredService<MongoUrl>();
            var mongoClient = sp.GetRequiredService<IMongoClient>();

            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

            return database.GetCollection<User>(nameof(User));
        });

        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }
}
