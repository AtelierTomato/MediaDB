using AtelierTomato.MediaDB.API.ModelBinders;
using AtelierTomato.MediaDB.Model.Converters;
using AtelierTomato.MediaDB.Storage;
using AtelierTomato.MediaDB.Storage.Sqlite;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

if (Debugger.IsAttached)
{
	builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddControllers(options =>
{
	options.ModelBinderProviders.Insert(0, new PartIDModelBinderProvider());
	options.ModelBinderProviders.Insert(1, new CultureInfoModelBinderProvider());
	options.ModelBinderProviders.Insert(2, new RegionInfoModelBinderProvider());
}).AddJsonOptions(options =>
{
	options.JsonSerializerOptions.Converters.Add(new PartIDConverter());
	options.JsonSerializerOptions.Converters.Add(new CultureInfoConverter());
	options.JsonSerializerOptions.Converters.Add(new RegionInfoConverter());
});

builder.Services.AddOptions<SqliteAccessOptions>().Bind(builder.Configuration.GetSection("SqliteAccess"));

builder.Services.AddSingleton<IPartAccess, SqlitePartAccess>()
				.AddSingleton<IPartGroupInfoAccess, SqlitePartGroupInfoAccess>()
				.AddSingleton<IPartGroupNameAccess, SqlitePartGroupNameAccess>()
				.AddSingleton<IPartNameAccess, SqlitePartNameAccess>()
				.AddSingleton<ISeriesAccess, SqliteSeriesAccess>()
				.AddSingleton<ISeriesNameAccess, SqliteSeriesNameAccess>()
				.AddSingleton<ISeriesParentAccess, SqliteSeriesParentAccess>();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowGetFromAnyOrigin", policy =>
	{
		policy.AllowAnyOrigin()
			  .WithMethods("GET")
			  .AllowAnyHeader();
	});

	options.AddPolicy("RestrictLocalAccess", policy =>
	{
		policy.WithOrigins("https://localhost", "http://localhost")
			  .WithMethods("PUT", "POST", "DELETE")
			  .AllowAnyHeader();
	});
});

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
} else
{
	app.UseExceptionHandler("/error");
}

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseWhen(
	context => context.Request.Method == HttpMethods.Get,
	appBuilder => appBuilder.UseCors("AllowGetFromAnyOrigin")
);

app.UseWhen(
	context => context.Request.Method != HttpMethods.Get,
	appBuilder => appBuilder.UseCors("RestrictLocalAccess")
);

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
