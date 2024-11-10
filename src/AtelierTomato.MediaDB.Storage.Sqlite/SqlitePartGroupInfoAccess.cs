using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage.Sqlite.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace AtelierTomato.MediaDB.Storage.Sqlite
{
	public class SqlitePartGroupInfoAccess : IPartGroupInfoAccess
	{
		private readonly SqliteAccessOptions options;
		public SqlitePartGroupInfoAccess(IOptions<SqliteAccessOptions> options)
		{
			this.options = options.Value;
		}

		public async Task DeletePartGroupInfo(ulong seriesID, PartID? parentPartID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@" 
DELETE FROM {nameof(PartGroupInfo)} WHERE
{nameof(PartGroupInfo.SeriesID)} IS @seriesID AND
{nameof(PartGroupInfo.ParentPartID)} IS @parentPartID
",
			new
			{
				seriesID,
				parentPartID = parentPartID?.ToString() ?? string.Empty,
			});

			connection.Close();
		}

		public async Task DeletePartGroupInfoRangeForSeries(ulong seriesID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@" 
DELETE FROM {nameof(PartGroupInfo)}
WHERE {nameof(PartGroupInfo.SeriesID)} IS @seriesID
",
			new
			{
				seriesID,
			});

			connection.Close();
		}

		public async Task<IEnumerable<PartGroupInfo>> ReadAllPartGroupInfos()
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartGroupInfoRow>($@"
SELECT {nameof(PartGroupInfo.SeriesID)}, {nameof(PartGroupInfo.ParentPartID)}, {nameof(PartGroupInfo.AverageLengthTime)}, {nameof(PartGroupInfo.AverageLengthPages)}, {nameof(PartGroupInfo.MediaType)}, {nameof(PartGroupInfo.ReleaseType)}
FROM {nameof(PartGroupInfo)}
			");

			connection.Close();
			return result.Select(r => r.ToPartGroupInfo());
		}

		public async Task<PartGroupInfo?> ReadPartGroupInfo(ulong seriesID, PartID? parentPartID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QuerySingleOrDefaultAsync<PartGroupInfoRow>($@"
SELECT {nameof(PartGroupInfo.SeriesID)}, {nameof(PartGroupInfo.ParentPartID)}, {nameof(PartGroupInfo.AverageLengthTime)}, {nameof(PartGroupInfo.AverageLengthPages)}, {nameof(PartGroupInfo.MediaType)}, {nameof(PartGroupInfo.ReleaseType)}
FROM {nameof(PartGroupInfo)} WHERE
{nameof(PartGroupInfo.SeriesID)} IS @seriesID AND
{nameof(PartGroupInfo.ParentPartID)} IS @parentPartID
",
			new
			{
				seriesID,
				parentPartID = parentPartID?.ToString() ?? string.Empty,
			});

			connection.Close();
			return result?.ToPartGroupInfo();
		}

		public async Task<IEnumerable<PartGroupInfo>> ReadPartGroupInfoRangeForSeries(ulong seriesID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<PartGroupInfoRow>($@"
SELECT {nameof(PartGroupInfo.SeriesID)}, {nameof(PartGroupInfo.ParentPartID)}, {nameof(PartGroupInfo.AverageLengthTime)}, {nameof(PartGroupInfo.AverageLengthPages)}, {nameof(PartGroupInfo.MediaType)}, {nameof(PartGroupInfo.ReleaseType)}
FROM {nameof(PartGroupInfo)}
WHERE {nameof(PartGroupInfo.SeriesID)} IS @seriesID
",
			new
			{
				seriesID,
			});

			connection.Close();
			return result.Select(r => r.ToPartGroupInfo());
		}

		public async Task WritePartGroupInfo(PartGroupInfo partGroupInfo) => await WritePartGroupInfoRange([partGroupInfo]);
		public async Task WritePartGroupInfoRange(IEnumerable<PartGroupInfo> partGroupInfoRange)
		{
			var partGroupInfoRows = partGroupInfoRange.Select(p => new PartGroupInfoRow(p));
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();
			await using var transaction = await connection.BeginTransactionAsync();

			foreach (PartGroupInfoRow partGroupInfoRow in partGroupInfoRows)
			{
				await connection.ExecuteAsync($@"
INSERT INTO {nameof(PartGroupInfo)} ( {nameof(PartGroupInfo.SeriesID)}, {nameof(PartGroupInfo.ParentPartID)}, {nameof(PartGroupInfo.AverageLengthTime)}, {nameof(PartGroupInfo.AverageLengthPages)}, {nameof(PartGroupInfo.MediaType)}, {nameof(PartGroupInfo.ReleaseType)} )
VALUIES ( @seriesID, @parentPartID, @averageLengthTime, @averageLengthPages, @mediaType, @releaseType )
ON CONFLICT ({nameof(PartGroupInfo.SeriesID)}, {nameof(PartGroupInfo.ParentPartID)}) DO UPDATE SET
{nameof(PartGroupInfo.AverageLengthTime)} = excluded.{nameof(PartGroupInfo.AverageLengthTime)},
{nameof(PartGroupInfo.AverageLengthPages)} = excluded.{nameof(PartGroupInfo.AverageLengthPages)},
{nameof(PartGroupInfo.MediaType)} = excluded.{nameof(PartGroupInfo.MediaType)},
{nameof(PartGroupInfo.ReleaseType)} = excluded.{nameof(PartGroupInfo.ReleaseType)},
",
				new
				{
					seriesID = partGroupInfoRow.SeriesID,
					parentPartID = partGroupInfoRow.ParentPartID,
					averageLengthTime = partGroupInfoRow.AverageLengthTime,
					averageLengthPages = partGroupInfoRow.AverageLengthPages,
					mediaType = partGroupInfoRow.MediaType,
					releaseType = partGroupInfoRow.ReleaseType,
				});
			}

			await transaction.CommitAsync();
		}
	}
}
