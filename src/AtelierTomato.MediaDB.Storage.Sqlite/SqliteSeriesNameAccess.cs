using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage.Sqlite.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage.Sqlite
{
	public class SqliteSeriesNameAccess : ISeriesNameAccess
	{
		private readonly SqliteAccessOptions options;
		public SqliteSeriesNameAccess(IOptions<SqliteAccessOptions> options)
		{
			this.options = options.Value;
		}

		public async Task DeleteSeriesName(ulong ID, CultureInfo language, ScriptType script)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(SeriesName)} WHERE
{nameof(SeriesName.ID)} IS @id AND
{nameof(SeriesName.Language)} IS @language AND
{nameof(SeriesName.Script)} IS @script
",
			new
			{
				id = ID,
				language = language.Name,
				script = script.ToString(),
			});

			connection.Close();
		}

		public async Task DeleteSeriesNameRangeForSeries(ulong ID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(SeriesName)} WHERE
{nameof(Series.ID)} IS @id
",
			new
			{
				id = ID,
			});

			connection.Close();
		}

		public async Task<IEnumerable<SeriesName>> ReadAllSeriesNames()
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<SeriesNameRow>($@"
SELECT {nameof(SeriesName.ID)}, {nameof(SeriesName.Language)}, {nameof(SeriesName.Script)}, {nameof(SeriesName.Name)}
FROM {nameof(SeriesName)}
			");

			connection.Close();
			return result.Select(r => r.ToSeriesName());
		}

		public async Task<SeriesName?> ReadSeriesName(ulong ID, CultureInfo language, ScriptType script) => (await ReadSeriesNameRange([ID], language, script)).FirstOrDefault();
		public async Task<IEnumerable<SeriesName>> ReadSeriesNameRange(IEnumerable<ulong> IDs, CultureInfo language, ScriptType script)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<SeriesNameRow>($@"
SELECT {nameof(SeriesName.ID)}, {nameof(SeriesName.Language)}, {nameof(SeriesName.Script)}, {nameof(SeriesName.Name)}
FROM {nameof(SeriesName)} WHERE
{nameof(SeriesName.ID)} in @ids AND
{nameof(SeriesName.Language)} IS @language AND
{nameof(SeriesName.Script)} IS @script
",
			new
			{
				ids = IDs,
				language = language.Name,
				script = script.ToString(),
			});

			return result.Select(r => r.ToSeriesName());
		}

		public async Task<IEnumerable<SeriesName>> ReadSeriesNameRangeForSeries(ulong ID) => await ReadSeriesNameRangeForSeriesPlural([ID]);
		public async Task<IEnumerable<SeriesName>> ReadSeriesNameRangeForSeriesPlural(IEnumerable<ulong> IDs)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<SeriesNameRow>($@"
SELECT {nameof(SeriesName.ID)}, {nameof(SeriesName.Language)}, {nameof(SeriesName.Script)}, {nameof(SeriesName.Name)}
FROM {nameof(SeriesName)} WHERE
{nameof(SeriesName.ID)} in @ids
",
			new
			{
				ids = IDs,
			});

			return result.Select(r => r.ToSeriesName());
		}

		public async Task WriteSeriesName(SeriesName seriesName) => await WriteSeriesNameRange([]);
		public async Task WriteSeriesNameRange(IEnumerable<SeriesName> seriesNameRange)
		{
			var seriesNameRows = seriesNameRange.Select(s => new SeriesNameRow(s));
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();
			await using var transaction = await connection.BeginTransactionAsync();

			foreach (SeriesNameRow seriesNameRow in seriesNameRows)
			{
				await connection.ExecuteAsync($@"
INSERT INTO {nameof(SeriesName)} ( {nameof(SeriesName.ID)}, {nameof(SeriesName.Language)}, {nameof(SeriesName.Script)}, {nameof(SeriesName.Name)} )
VALUES ( @id, @language, @script, @name )
ON CONFLICT ({nameof(SeriesName.ID)}, {nameof(SeriesName.Language)}, {nameof(SeriesName.Script)}) DO UPDATE SET
{nameof(SeriesName.Name)} = excluded.{nameof(SeriesName.Name)}
",
				new
				{
					id = seriesNameRow.ID,
					language = seriesNameRow.Language,
					script = seriesNameRow.Script,
					name = seriesNameRow.Name,
				});
			}

			await transaction.CommitAsync();
		}
	}
}
