using AtelierTomato.MediaDB.Storage;
using AtelierTomato.MediaDB.Storage.Sqlite;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

if (Debugger.IsAttached)
{
	builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddOptions<SqliteAccessOptions>().Bind(builder.Configuration.GetSection("SqliteAccess"));

builder.Services.AddSingleton<IPartAccess, SqlitePartAccess>()
				.AddSingleton<IPartGroupInfoAccess, SqlitePartGroupInfoAccess>()
				.AddSingleton<IPartGroupNameAccess, SqlitePartGroupNameAccess>()
				.AddSingleton<IPartNameAccess, SqlitePartNameAccess>()
				.AddSingleton<ISeriesAccess, SqliteSeriesAccess>()
				.AddSingleton<ISeriesNameAccess, SqliteSeriesNameAccess>()
				.AddSingleton<ISeriesParentAccess, SqliteSeriesParentAccess>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
