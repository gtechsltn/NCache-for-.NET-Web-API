
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Common;
using Alachisoft.NCache.Web.Caching;
using Microsoft.Extensions.Options;
using NCacheWebApi.Models;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

// Bind NCache configuration from appsettings.json
builder.Services.Configure<NCacheSettings>(builder.Configuration.GetSection("NCacheConfig"));

// Configure NCache client

// Create new CacheConnectionOptions instance
var options = new CacheConnectionOptions();

// Specify the cache connection options to be set
options.RetryInterval = TimeSpan.FromSeconds(5);
options.ConnectionRetries = 2;
options.EnableKeepAlive = true;
options.KeepAliveInterval = TimeSpan.FromSeconds(30);
// Enter the credentials
//string cacheName = "demoCache";
//string userId = "userid";
//string password = "mypassword";
//options.UserCredentials = new Credentials(userId, password)

builder.Services.AddSingleton<ICache>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<NCacheSettings>>().Value;
    var cacheName1 = settings.CacheName1;
    var clusterIP1 = settings.ClusterIP1;
    var cacheName2 = settings.CacheName2;
    var clusterIP2 = settings.ClusterIP2;

    // Connect to NCache cluster
    // Connect to the caches
    ICache cache1 = CacheManager.GetCache(cacheName1, options);
    ICache cache2 = CacheManager.GetCache(cacheName2, options);

    //ICache cache3 = CacheManager.GetCache("myCache", options);


    return cache1;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
