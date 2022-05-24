using Basket.API.GrpServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

//Add Redis service
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

//Register Dependency injected classes
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
//Activate AutoMapper in the startup (i.e. Program )
builder.Services.AddAutoMapper(typeof(Program));

//Register DiscountGrpcService and Client
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (options => options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));
builder.Services.AddScoped<DiscountGrpcService>();

//Add service for MassTransit - RabbitMQ
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg)=>
    {
        //Creates host in RabbitMQ
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});

#region Important Info
//builder.Services.AddMassTransitHostedService(); ---  This is outdated and only below packages should be used with specific version

//< PackageReference Include = "MassTransit" Version = "8.0.2" />
//< PackageReference Include = "MassTransit.RabbitMQ" Version = "8.0.2" />
 // link - https://stackoverflow.com/questions/70187422/addmasstransithostedservice-cannot-be-found
#endregion


// Add services to the container.
builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
