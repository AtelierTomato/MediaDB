using AtelierTomato.MediaDB.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace AtelierTomato.MediaDB.Storage.Sqlite
{
	public class SqliteSeriesParentAccess : ISeriesParentAccess
	{
		private readonly SqliteAccessOptions options;
		public SqliteSeriesParentAccess(IOptions<SqliteAccessOptions> options)
		{
			this.options = options.Value;
		}

		public async Task DeleteSeriesParent(ulong ID, ulong parentID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(SeriesParent)} WHERE
{nameof(SeriesParent.ID)} IS @id AND
{nameof(SeriesParent.ParentID)} IS @parentID
",
			new
			{
				id = ID,
				parentID,
			});

			connection.Close();
		}

		public async Task DeleteSeriesParentRangeByParentSeries(ulong parentID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(SeriesParent)} WHERE
{nameof(SeriesParent.ParentID)} IS @parentID
",
			new
			{
				parentID,
			});

			connection.Close();
		}

		public async Task DeleteSeriesParentRangeBySeries(ulong ID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			await connection.ExecuteAsync($@"
DELETE FROM {nameof(SeriesParent)} WHERE
{nameof(SeriesParent.ID)} IS @id
",
			new
			{
				id = ID,
			});

			connection.Close();
		}

		public async Task<IEnumerable<SeriesParent>> ReadAllSeriesParents()
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<SeriesParent>($@"
SELECT {nameof(SeriesParent.ID)}, {nameof(SeriesParent.ParentID)}
FROM {nameof(SeriesParent)}
			");

			connection.Close();
			return result;
		}

		public async Task<SeriesParent?> ReadSeriesParent(ulong ID, ulong parentID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QuerySingleOrDefaultAsync<SeriesParent>($@"
SELECT {nameof(SeriesParent.ID)}, {nameof(SeriesParent.ParentID)}
FROM {nameof(SeriesParent)} WHERE
{nameof(SeriesParent.ID)} IS @id AND
{nameof(SeriesParent.ParentID)} IS @parentID
",
			new
			{
				id = ID,
				parentID,
			});

			connection.Close();
			return result;
		}

		public async Task<IEnumerable<SeriesParent>> ReadSeriesParentRangeByParentSeries(ulong parentID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<SeriesParent>($@"
SELECT {nameof(SeriesParent.ID)}, {nameof(SeriesParent.ParentID)}
FROM {nameof(SeriesParent)}
WHERE {nameof(SeriesParent.ParentID)} IS @parentID
",
			new
			{
				parentID,
			});

			connection.Close();
			return result;
		}

		public async Task<IEnumerable<SeriesParent>> ReadSeriesParentRangeBySeries(ulong ID)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();

			var result = await connection.QueryAsync<SeriesParent>($@"
SELECT {nameof(SeriesParent.ID)}, {nameof(SeriesParent.ParentID)}
FROM {nameof(SeriesParent)}
WHERE {nameof(SeriesParent.ID)} IS @id
",
			new
			{
				id = ID,
			});

			connection.Close();
			return result;
		}

		public async Task WriteSeriesParent(SeriesParent seriesParent) => await WriteSeriesParentRange([seriesParent]);
		public async Task WriteSeriesParentRange(IEnumerable<SeriesParent> seriesParentRange)
		{
			await using var connection = new SqliteConnection(options.ConnectionString);
			connection.Open();
			await using var transaction = await connection.BeginTransactionAsync();

			foreach (SeriesParent seriesParent in seriesParentRange)
			{
				await connection.ExecuteAsync($@"
INSERT INTO {nameof(SeriesParent)} ( {nameof(SeriesParent.ID)}, {nameof(SeriesParent.ParentID)} )
VALUES ( @id, @parentID )
ON CONFLICT ({nameof(SeriesParent.ID)}, {nameof(SeriesParent.ParentID)}) DO NOTHING
",
				new
				{
					id = seriesParent.ID,
					parentID = seriesParent.ParentID,
				});
			}

			await transaction.CommitAsync();
		}
	}
}
