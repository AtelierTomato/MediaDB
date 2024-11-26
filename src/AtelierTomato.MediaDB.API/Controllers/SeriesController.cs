using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AtelierTomato.MediaDB.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SeriesController : ControllerBase
	{
		private readonly ISeriesAccess _seriesAccess;
		public SeriesController(ISeriesAccess seriesAccess)
		{
			_seriesAccess = seriesAccess;
		}

		[HttpPost]
		public async Task<IActionResult> WriteNewSeries([FromBody] Series series)
		{
			if (series is null)
				return BadRequest("Series cannot be null.");

			var seriesID = await _seriesAccess.WriteNewSeries(series);
			return CreatedAtAction(nameof(ReadSeries), new { ID = seriesID }, seriesID);
		}

		[HttpPost("range")]
		public async Task<IActionResult> WriteNewSeriesRange([FromBody] IEnumerable<Series> seriesRange)
		{
			if (seriesRange is null || !seriesRange.Any())
				return BadRequest("Series range cannot be null.");

			var seriesIDs = await _seriesAccess.WriteNewSeriesRange(seriesRange);
			return Ok(seriesIDs);
		}

		[HttpPut]
		public async Task<IActionResult> WriteSeries([FromBody] Series series)
		{
			if (series is null)
				return BadRequest("Series cannot be null.");

			await _seriesAccess.WriteSeries(series);
			return NoContent();
		}

		[HttpPut("range")]
		public async Task<IActionResult> WriteSeriesRange([FromBody] IEnumerable<Series> seriesRange)
		{
			if (seriesRange is null || !seriesRange.Any())
				return BadRequest("Series range cannot be null.");

			await _seriesAccess.WriteSeriesRange(seriesRange);
			return NoContent();
		}

		[HttpGet("{ID}")]
		public async Task<ActionResult<Series>> ReadSeries(ulong ID)
		{
			var result = await _seriesAccess.ReadSeries(ID);

			if (result is null)
				return NotFound();

			return Ok(result);
		}

		[HttpGet("range")]
		public async Task<ActionResult<IEnumerable<Series>>> ReadSeriesRange([FromQuery] IEnumerable<ulong> IDs)
		{
			var result = await _seriesAccess.ReadSeriesRange(IDs);

			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("all")]
		public async Task<ActionResult<IEnumerable<Series>>> ReadAllSeries()
		{
			var result = await _seriesAccess.ReadAllSeries();

			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("search/name/{name}")]
		public async Task<ActionResult<IEnumerable<Series>>> SearchSeriesByName(string name)
		{
			var result = await _seriesAccess.SearchSeriesByName(name);

			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpDelete("{ID}")]
		public async Task<IActionResult> DeleteSeries(ulong ID)
		{
			await _seriesAccess.DeleteSeries(ID);
			return NoContent();
		}

		[HttpGet("count")]
		public async Task<ActionResult<int>> CountSeries()
		{
			var result = await _seriesAccess.CountSeries();
			return Ok(result);
		}
	}
}

