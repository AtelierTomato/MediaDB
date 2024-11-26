using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage.Sqlite.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace AtelierTomato.MediaDB.Storage.Sqlite
{
	public class SqlitePartGroupNameAccess : IPartGroupNameAccess
	{
		private readonly SqliteAccessOptions options;
		public SqlitePartGroupNameAccess(IOptions<SqliteAccessOptions> options)
		{
			this.options = options.Value;
		}

		public async Task<int> CountPartGroupNames()
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.ExecuteScalarAsync<int>($@"SELECT COUNT(*) FROM {nameof(PartGroupName)}");

			connection.Close();
			return result;
		}

		public async Task DeletePartGroupName(ulong seriesID, PartID? parentPartID, CultureInfo language, ScriptType script)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(PartGroupName)} WHERE
{nameof(PartGroupName.SeriesID)} IS @seriesID AND
{nameof(PartGroupName.ParentPartID)} IS @parentPartID AND
{nameof(PartGroupName.Language)} IS @language AND
{nameof(PartGroupName.Script)} IS @script
",
			new
			{
				seriesID,
				parentPartID = parentPartID?.ToString() ?? string.Empty,
				language = language.Name,
				script = script.ToString(),
			});

			connection.Close();
		}

		public async Task DeletePartGroupNameRangeForPart(ulong seriesID, PartID? parentPartID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(PartGroupName)} WHERE
{nameof(PartGroupName.SeriesID)} IS @seriesID AND
{nameof(PartGroupName.ParentPartID)} IS @parentPartID
",
			new
			{
				seriesID,
				parentPartID = parentPartID?.ToString() ?? string.Empty,
			});

			connection.Close();
		}

		public async Task DeletePartGroupNameRangeForSeries(ulong seriesID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(PartGroupName)}
WHERE {nameof(PartGroupName.SeriesID)} IS @seriesID
",
			new
			{
				seriesID,
			});

			connection.Close();
		}

		public async Task<IEnumerable<PartGroupName>> ReadAllPartGroupNames()
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartGroupNameRow>($@"
SELECT {nameof(PartGroupName.SeriesID)}, {nameof(PartGroupName.ParentPartID)}, {nameof(PartGroupName.Language)}, {nameof(PartGroupName.Script)}, {nameof(PartGroupName.Name)}
FROM {nameof(PartGroupName)}
			");

			connection.Close();
			return result.Select(r => r.ToPartGroupName());
		}

		public async Task<PartGroupName?> ReadPartGroupName(ulong seriesID, PartID? parentPartID, CultureInfo language, ScriptType script)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QuerySingleOrDefaultAsync<PartGroupNameRow>($@"
SELECT {nameof(PartGroupName.SeriesID)}, {nameof(PartGroupName.ParentPartID)}, {nameof(PartGroupName.Language)}, {nameof(PartGroupName.Script)}, {nameof(PartGroupName.Name)}
FROM {nameof(PartName)} WHERE
{nameof(PartGroupName.SeriesID)} IS @seriesID AND
{nameof(PartGroupName.ParentPartID)} IS @parentPartID AND
{nameof(PartGroupName.Language)} IS @language AND
{nameof(PartGroupName.Script)} IS @script
",
			new
			{
				seriesID,
				parentPartID = parentPartID?.ToString() ?? string.Empty,
				language = language.Name,
				script = script.ToString(),
			});

			connection.Close();
			return result?.ToPartGroupName();
		}

		public async Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForParentPart(ulong seriesID, PartID? parentPartID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartGroupNameRow>($@"
SELECT {nameof(PartGroupName.SeriesID)}, {nameof(PartGroupName.ParentPartID)}, {nameof(PartGroupName.Language)}, {nameof(PartGroupName.Script)}, {nameof(PartGroupName.Name)}
FROM {nameof(PartGroupName)} WHERE
{nameof(PartGroupName.SeriesID)} IS @seriesID AND
{nameof(PartGroupName.ParentPartID)} IS @partID
",
			new
			{
				seriesID,
				parentPartID = parentPartID?.ToString() ?? string.Empty,
			});

			connection.Close();
			return result.Select(r => r.ToPartGroupName());
		}

		public async Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForSeries(ulong seriesID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartGroupNameRow>($@"
SELECT {nameof(PartGroupName.SeriesID)}, {nameof(PartGroupName.ParentPartID)}, {nameof(PartGroupName.Language)}, {nameof(PartGroupName.Script)}, {nameof(PartGroupName.Name)}
FROM {nameof(PartGroupName)}
WHERE {nameof(PartGroupName.SeriesID)} IS @seriesID
",
			new
			{
				seriesID,
			});

			connection.Close();
			return result.Select(r => r.ToPartGroupName());
		}

		public async Task<IEnumerable<PartGroupName>> ReadPartGroupNameRangeForSeriesWithLanguage(ulong seriesID, CultureInfo language, ScriptType script)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartGroupNameRow>($@"
SELECT {nameof(PartGroupName.SeriesID)}, {nameof(PartGroupName.ParentPartID)}, {nameof(PartGroupName.Language)}, {nameof(PartGroupName.Script)}, {nameof(PartGroupName.Name)}
FROM {nameof(PartGroupName)} WHERE
{nameof(PartGroupName.SeriesID)} IS @seriesID AND
{nameof(PartGroupName.Language)} Is @language AND
{nameof(PartGroupName.Script)} IS @script
",
			new
			{
				seriesID,
				language = language.Name,
				script = script.ToString()
			});

			connection.Close();
			return result.Select(r => r.ToPartGroupName());
		}

		public async Task WritePartGroupName(PartGroupName partGroupName) => await WritePartGroupNameRange([partGroupName]);
		public async Task WritePartGroupNameRange(IEnumerable<PartGroupName> partGroupNameRange)
		{
			var partGroupNameRows = partGroupNameRange.Select(p => new PartGroupNameRow(p));
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();
			await using var transaction = await connection.BeginTransactionAsync();

			foreach (PartGroupNameRow partGroupNameRow in partGroupNameRows)
			{
				await connection.ExecuteAsync($@"
INSERT INTO {nameof(PartGroupName)} ( {nameof(PartGroupName.SeriesID)}, {nameof(PartGroupName.ParentPartID)}, {nameof(PartGroupName.Language)}, {nameof(PartGroupName.Script)}, {nameof(PartGroupName.Name)} )
VALUES ( @seriesID, @partID, @language, @script, @name )
ON CONFLICT ({nameof(PartGroupName.SeriesID)}, {nameof(PartGroupName.ParentPartID)}, {nameof(PartGroupName.Language)}, {nameof(PartGroupName.Script)}) DO UPDATE SET
{nameof(PartGroupName.Name)} = excluded.{nameof(PartGroupName.Name)}
",
				new
				{
					seriesID = partGroupNameRow.SeriesID,
					partID = partGroupNameRow.ParentPartID,
					language = partGroupNameRow.Language,
					script = partGroupNameRow.Script,
					name = partGroupNameRow.Name,
				});
			}

			await transaction.CommitAsync();
		}
	}
}
