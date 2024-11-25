using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage.Sqlite.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage.Sqlite
{
	public class SqlitePartNameAccess : IPartNameAccess
	{
		private readonly SqliteAccessOptions options;
		public SqlitePartNameAccess(IOptions<SqliteAccessOptions> options)
		{
			this.options = options.Value;
		}

		public async Task DeletePartName(ulong seriesID, PartID partID, CultureInfo language, ScriptType script)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(PartName)} WHERE
{nameof(PartName.SeriesID)} IS @seriesID AND
{nameof(PartName.PartID)} IS @partID AND
{nameof(PartName.Language)} IS @language AND
{nameof(PartName.Script)} IS @script
",
			new
			{
				seriesID,
				partID = partID.ToString(),
				language = language.Name,
				script = script.ToString(),
			});

			connection.Close();
		}

		public async Task DeletePartNameRangeForPart(ulong seriesID, PartID partID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(PartName)} WHERE
{nameof(PartName.SeriesID)} IS @seriesID AND
{nameof(PartName.PartID)} IS @partID
",
			new
			{
				seriesID,
				partID = partID.ToString(),
			});

			connection.Close();
		}

		public async Task DeletePartNameRangeForSeries(ulong seriesID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(PartName)}
WHERE {nameof(PartName.SeriesID)} IS @seriesID
",
			new
			{
				seriesID,
			});

			connection.Close();
		}

		public async Task<IEnumerable<PartName>> ReadAllPartNames()
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartNameRow>($@"
SELECT {nameof(PartName.SeriesID)}, {nameof(PartName.PartID)}, {nameof(PartName.Language)}, {nameof(PartName.Script)}, {nameof(PartName.Name)}
FROM {nameof(PartName)}
			");

			connection.Close();
			return result.Select(r => r.ToPartName());
		}

		public async Task<PartName?> ReadPartName(ulong seriesID, PartID partID, CultureInfo language, ScriptType script)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QuerySingleOrDefaultAsync<PartNameRow>($@"
SELECT {nameof(PartName.SeriesID)}, {nameof(PartName.PartID)}, {nameof(PartName.Language)}, {nameof(PartName.Script)}, {nameof(PartName.Name)}
FROM {nameof(PartName)} WHERE
{nameof(PartName.SeriesID)} IS @seriesID AND
{nameof(PartName.PartID)} IS @partID AND
{nameof(PartName.Language)} IS @language AND
{nameof(PartName.Script)} IS @script
",
			new
			{
				seriesID,
				partID = partID.ToString(),
				language = language.Name,
				script = script.ToString(),
			});

			connection.Close();
			return result?.ToPartName();
		}

		public async Task<IEnumerable<PartName>> ReadPartNameRangeForPart(ulong seriesID, PartID partID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartNameRow>($@"
SELECT {nameof(PartName.SeriesID)}, {nameof(PartName.PartID)}, {nameof(PartName.Language)}, {nameof(PartName.Script)}, {nameof(PartName.Name)}
FROM {nameof(PartName)} WHERE
{nameof(PartName.SeriesID)} IS @seriesID AND
{nameof(PartName.PartID)} IS @partID
",
			new
			{
				seriesID,
				partID = partID.ToString(),
			});

			connection.Close();
			return result.Select(r => r.ToPartName());
		}

		public async Task<IEnumerable<PartName>> ReadPartNameRangeForSeries(ulong seriesID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartNameRow>($@"
SELECT {nameof(PartName.SeriesID)}, {nameof(PartName.PartID)}, {nameof(PartName.Language)}, {nameof(PartName.Script)}, {nameof(PartName.Name)}
FROM {nameof(PartName)}
WHERE {nameof(PartName.SeriesID)} IS @seriesID
",
			new
			{
				seriesID,
			});

			connection.Close();
			return result.Select(r => r.ToPartName());
		}

		public async Task<IEnumerable<PartName>> ReadPartNameRangeForSeriesWithLanguage(ulong seriesID, CultureInfo language, ScriptType script)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartNameRow>($@"
SELECT {nameof(PartName.SeriesID)}, {nameof(PartName.PartID)}, {nameof(PartName.Language)}, {nameof(PartName.Script)}, {nameof(PartName.Name)}
FROM {nameof(PartName)} WHERE
{nameof(PartName.SeriesID)} IS @seriesID AND
{nameof(PartName.Language)} Is @language AND
{nameof(PartName.Script)} IS @script
",
			new
			{
				seriesID,
				language = language.Name,
				script = script.ToString()
			});

			connection.Close();
			return result.Select(r => r.ToPartName());
		}

		public async Task WritePartName(PartName partName) => await WritePartNameRange([partName]);
		public async Task WritePartNameRange(IEnumerable<PartName> partNameRange)
		{
			var partNameRows = partNameRange.Select(p => new PartNameRow(p));
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();
			await using var transaction = await connection.BeginTransactionAsync();

			foreach (PartNameRow partNameRow in partNameRows)
			{
				await connection.ExecuteAsync($@"
INSERT INTO {nameof(PartName)} ( {nameof(PartName.SeriesID)}, {nameof(PartName.PartID)}, {nameof(PartName.Language)}, {nameof(PartName.Script)}, {nameof(PartName.Name)} )
VALUES ( @seriesID, @partID, @language, @script, @name )
ON CONFLICT ({nameof(PartName.SeriesID)}, {nameof(PartName.PartID)}, {nameof(PartName.Language)}, {nameof(PartName.Script)}) DO UPDATE SET
{nameof(PartName.Name)} = excluded.{nameof(PartName.Name)}
",
				new
				{
					seriesID = partNameRow.SeriesID,
					partID = partNameRow.PartID,
					language = partNameRow.Language,
					script = partNameRow.Script,
					name = partNameRow.Name,
				});
			}

			await transaction.CommitAsync();
		}
	}
}
