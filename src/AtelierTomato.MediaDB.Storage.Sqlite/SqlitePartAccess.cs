using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage.Sqlite.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace AtelierTomato.MediaDB.Storage.Sqlite
{
	public class SqlitePartAccess : IPartAccess
	{
		private readonly SqliteAccessOptions options;
		public SqlitePartAccess(IOptions<SqliteAccessOptions> options)
		{
			this.options = options.Value;
		}

		public async Task DeletePart(ulong seriesID, PartID partID) => await DeletePartRangeInSeries(seriesID, [partID]);
		public async Task DeletePartRangeInSeries(ulong seriesID, IEnumerable<PartID> partIDRange)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(Part)} WHERE
{nameof(Part.SeriesID)} IS @seriesID AND
{nameof(Part.PartID)} IN @partIDRange
",
			new
			{
				seriesID,
				partIDRange = partIDRange.Select(p => p.ToString()),
			});

			connection.Close();
		}

		public async Task<IEnumerable<Part>> ReadAllParts()
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartRow>($@"
SELECT {nameof(Part.SeriesID)}, {nameof(Part.PartID)}, {nameof(Part.LengthTime)}, {nameof(Part.LengthPages)}, {nameof(Part.StartTime)}, {nameof(Part.EndTime)}
FROM {nameof(Part)}
			");

			connection.Close();
			return result.Select(r => r.ToPart());
		}

		public async Task<Part?> ReadPart(ulong seriesID, PartID partID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QuerySingleOrDefaultAsync<PartRow>($@"
SELECT {nameof(Part.SeriesID)}, {nameof(Part.PartID)}, {nameof(Part.LengthTime)}, {nameof(Part.LengthPages)}, {nameof(Part.StartTime)}, {nameof(Part.EndTime)}
FROM {nameof(Part)} WHERE
{nameof(Part.SeriesID)} IS @seriesID AND
{nameof(Part.PartID)} IS @partID
",
			new
			{
				seriesID,
				partID = partID.ToString(),
			});

			connection.Close();
			return result?.ToPart();
		}

		public async Task<IEnumerable<Part>> ReadPartRangeBySeries(ulong seriesID) => await ReadPartRangeBySeriesRange([seriesID]);
		public async Task<IEnumerable<Part>> ReadPartRangeBySeriesRange(IEnumerable<ulong> seriesIDRange)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartRow>($@"
SELECT {nameof(Part.SeriesID)}, {nameof(Part.PartID)}, {nameof(Part.LengthTime)}, {nameof(Part.LengthPages)}, {nameof(Part.StartTime)}, {nameof(Part.EndTime)}
FROM {nameof(Part)}
WHERE {nameof(Part.SeriesID)} IN @seriesIDRange
",
			new
			{
				seriesIDRange,
			});

			connection.Close();
			return result.Select(r => r.ToPart());
		}

		public async Task WritePart(Part part) => await WritePartRange([part]);
		public async Task WritePartRange(IEnumerable<Part> partRange)
		{
			var partRows = partRange.Select(p => new PartRow(p));
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();
			await using var transaction = await connection.BeginTransactionAsync();

			foreach (PartRow partRow in partRows)
			{
				await connection.ExecuteAsync($@"
INSERT INTO {nameof(Part)} ( {nameof(Part.SeriesID)}, {nameof(Part.PartID)}, {nameof(Part.LengthTime)}, {nameof(Part.LengthPages)}, {nameof(Part.StartTime)}, {nameof(Part.EndTime)} )
VALUES ( @seriesID, @partID, @lengthTime, @lengthPages, @startTime, @endTime )
ON CONFLICT ({nameof(Part.SeriesID)}, {nameof(Part.PartID)}) DO UPDATE SET
{nameof(Part.LengthTime)} = excluded.{nameof(Part.LengthTime)},
{nameof(Part.LengthPages)} = excluded.{nameof(Part.LengthPages)},
{nameof(Part.StartTime)} = excluded.{nameof(Part.StartTime)},
{nameof(Part.EndTime)} = excluded.{nameof(Part.EndTime)}
",
				new
				{
					seriesID = partRow.SeriesID,
					partID = partRow.PartID,
					lengthTime = partRow.LengthTime,
					lengthPages = partRow.LengthPages,
					startTime = partRow.StartTime,
					endTime = partRow.EndTime,
				});
			}

			await transaction.CommitAsync();
		}
	}
}
