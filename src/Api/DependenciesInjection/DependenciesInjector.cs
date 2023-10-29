namespace Core.DependenciesInjection;

public static class DependenciesInjector
{
    public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        //Add cache
        AddRedisCache(services, configuration);

        //Add cache pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

        //Add mediators
        services.AddMediatR(new[]
        {
            typeof(CargoByAirportPagedQueryHandler).Assembly,
        });

        //Add Mongo Repositories
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.AddSingleton<IMongoDbSettings>(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
        services.AddScoped<ICargoMongoRepository, CargoMongoRepository>();
        services.AddScoped<IAirportMongoRepository, AirportMongoRepository>();

        //Add serialization for Mongo
        BsonSerializer.RegisterSerializationProvider(new MongoSerializationProvider());

        //Add Sql Repositories
        var sqlConnectionString = configuration.GetConnectionString("SqlConnectionString");
        services.AddScoped(sp => new SqlSession(sqlConnectionString));
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<ICargoSqlRepository, CargoSqlRepository>();
        
        FluentMapper.Initialize(config =>
        {
            //config.AddMap(new CargoSqlMap());
            config.ForDommel();
        });
    }

    private static void AddRedisCache(IServiceCollection services, IConfiguration configuration)
    {
        var redisHost = configuration["Cache:Redis:ServerConfiguration"];
        var password = configuration["Cache:Redis:Password"];
        var options = new ConfigurationOptions()
        {
            AllowAdmin = true,
            EndPoints = { redisHost },
            ConnectTimeout = 5000,
            ConnectRetry = 5,
            AsyncTimeout = 5000,
            ReconnectRetryPolicy = new LinearRetry(300),
            Ssl = true,
            AbortOnConnectFail = false,
            Password = password
        };

        services.AddSingleton<ICache>(new RedisCache(ConnectionMultiplexer.Connect(options)));
    }
}
