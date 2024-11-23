using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage.Sqlite.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace AtelierTomato.MediaDB.Storage.Sqlite
{
	public class SqliteSeriesAccess : ISeriesAccess
	{
		private readonly SqliteAccessOptions options;
		public SqliteSeriesAccess(IOptions<SqliteAccessOptions> options)
		{
			this.options = options.Value;
		}

		public async Task DeleteSeries(ulong ID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(Series)} WHERE
{nameof(Series.ID)} IS @id
",
			new
			{
				id = ID,
			});

			connection.Close();
		}

		public async Task<IEnumerable<Series>> ReadAllSeries()
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<SeriesRow>($@"
SELECT {nameof(Series.ID)}, {nameof(Series.OriginCountries)}, {nameof(Series.OriginLanguage)}, {nameof(Series.OriginScript)}, {nameof(Series.StartTime)}, {nameof(Series.EndTime)}
FROM {nameof(Series)}
			");

			connection.Close();
			return result.Select(r => r.ToSeries());
		}

		public async Task<Series?> ReadSeries(ulong ID) => (await ReadSeriesRange([ID])).FirstOrDefault();
		public async Task<IEnumerable<Series>> ReadSeriesRange(IEnumerable<ulong> IDs)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<SeriesRow>($@"
SELECT {nameof(Series.ID)}, {nameof(Series.OriginCountries)}, {nameof(Series.OriginLanguage)}, {nameof(Series.OriginScript)}, {nameof(Series.StartTime)}, {nameof(Series.EndTime)}
FROM {nameof(Series)}
WHERE {nameof(Series.ID)} IN @ids
",
			new
			{
				ids = IDs,
			});

			connection.Close();

			return result.Select(r => r.ToSeries());
		}

		public async Task<ulong> WriteNewSeries(Series series) => (await WriteNewSeriesRange([series])).FirstOrDefault();
		public async Task<IEnumerable<ulong>> WriteNewSeriesRange(IEnumerable<Series> seriesRange)
		{
			var seriesRows = seriesRange.Select(s => new SeriesRow(s));
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();
			await using var transaction = await connection.BeginTransactionAsync();

			List<ulong> insertedIDs = [];

			foreach (SeriesRow seriesRow in seriesRows)
			{
				var id = await connection.ExecuteScalarAsync<ulong>($@"
INSERT INTO {nameof(Series)} ( {nameof(Series.OriginCountries)}, {nameof(Series.OriginLanguage)}, {nameof(Series.OriginScript)}, {nameof(Series.StartTime)}, {nameof(Series.EndTime)} )
VALUES ( @originCountries, @originLanguage, @originScript, @startTime, @endTime )
SELECT last_insert_rowid()
",
				new
				{
					originCountries = seriesRow.OriginCountries,
					originLanguage = seriesRow.OriginLanguage,
					originScript = seriesRow.OriginScript,
					startTime = seriesRow.StartTime,
					endTime = seriesRow.EndTime,
				});

				insertedIDs.Add(id);
			}

			await transaction.CommitAsync();
			return insertedIDs;
		}

		public async Task WriteSeries(Series series) => await WriteSeriesRange([series]);
		public async Task WriteSeriesRange(IEnumerable<Series> seriesRange)
		{
			var seriesRows = seriesRange.Select(s => new SeriesRow(s));
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();
			await using var transaction = await connection.BeginTransactionAsync();

			foreach (SeriesRow seriesRow in seriesRows)
			{
				await connection.ExecuteAsync($@"
INSERT INTO {nameof(Series)} ( {nameof(Series.ID)}, {nameof(Series.OriginCountries)}, {nameof(Series.OriginLanguage)}, {nameof(Series.OriginScript)}, {nameof(Series.StartTime)}, {nameof(Series.EndTime)} )
VALUES ( @id, @originCountries, @originLanguage, @originScript, @startTime, @endTime )
ON CONFLICT ({nameof(Series.ID)}) DO UPDATE SET
{nameof(Series.OriginCountries)} = excluded.{nameof(Series.OriginCountries)},
{nameof(Series.OriginLanguage)} = excluded.{nameof(Series.OriginLanguage)},
{nameof(Series.OriginScript)} = excluded.{nameof(Series.OriginScript)},
{nameof(Series.StartTime)} = excluded.{nameof(Series.StartTime)},
{nameof(Series.EndTime)} = excluded.{nameof(Series.EndTime)}
",
				new
				{
					id = seriesRow.ID,
					originCountries = seriesRow.OriginCountries,
					originLanguage = seriesRow.OriginLanguage,
					originScript = seriesRow.OriginScript,
					startTime = seriesRow.StartTime,
					endTime = seriesRow.EndTime,
				});
			}

			await transaction.CommitAsync();
		}
	}
}
