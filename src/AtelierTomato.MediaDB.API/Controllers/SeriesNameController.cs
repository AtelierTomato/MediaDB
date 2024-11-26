using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AtelierTomato.MediaDB.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SeriesNameController : ControllerBase
	{
		private readonly ISeriesNameAccess _seriesNameAccess;
		public SeriesNameController(ISeriesNameAccess seriesNameAccess)
		{
			_seriesNameAccess = seriesNameAccess;
		}

		[HttpPost]
		public async Task<IActionResult> WriteSeriesName([FromBody] SeriesName seriesName)
		{
			if (seriesName is null)
				return BadRequest("SeriesName cannot be null.");

			await _seriesNameAccess.WriteSeriesName(seriesName);
			return NoContent();
		}

		[HttpPost("range")]
		public async Task<IActionResult> WriteSeriesNameRange([FromBody] IEnumerable<SeriesName> seriesNameRange)
		{
			if (seriesNameRange is null || !seriesNameRange.Any())
				return BadRequest("SeriesName range cannot be null.");

			await _seriesNameAccess.WriteSeriesNameRange(seriesNameRange);
			return NoContent();
		}

		[HttpGet("{ID}/{language}/{script}")]
		public async Task<ActionResult<SeriesName>> ReadSeriesName(ulong ID, CultureInfo language, ScriptType script)
		{
			var result = await _seriesNameAccess.ReadSeriesName(ID, language, script);

			if (result is null)
				return NotFound();

			return Ok(result);
		}

		[HttpGet("range")]
		public async Task<ActionResult<IEnumerable<SeriesName>>> ReadSeriesNameRange(
			[FromQuery] IEnumerable<ulong> IDs,
			[FromQuery] CultureInfo language,
			[FromQuery] ScriptType script)
		{
			var result = await _seriesNameAccess.ReadSeriesNameRange(IDs, language, script);

			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("{ID}")]
		public async Task<ActionResult<IEnumerable<SeriesName>>> ReadSeriesNameRangeForSeries(ulong ID)
		{
			var result = await _seriesNameAccess.ReadSeriesNameRangeForSeries(ID);

			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("range/forseries")]
		public async Task<ActionResult<IEnumerable<SeriesName>>> ReadSeriesNameRangeForSeriesPlural([FromQuery] IEnumerable<ulong> IDs)
		{
			var result = await _seriesNameAccess.ReadSeriesNameRangeForSeriesPlural(IDs);

			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("all")]
		public async Task<ActionResult<IEnumerable<SeriesName>>> ReadAllSeriesNames()
		{
			var result = await _seriesNameAccess.ReadAllSeriesNames();

			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpDelete("{ID}/{language}/{script}")]
		public async Task<IActionResult> DeleteSeriesName(ulong ID, CultureInfo language, ScriptType script)
		{
			await _seriesNameAccess.DeleteSeriesName(ID, language, script);
			return NoContent();
		}

		[HttpDelete("{ID}")]
		public async Task<IActionResult> DeleteSeriesNameRangeForSeries(ulong ID)
		{
			await _seriesNameAccess.DeleteSeriesNameRangeForSeries(ID);
			return NoContent();
		}

		[HttpGet("count")]
		public async Task<ActionResult<int>> CountSeriesNames()
		{
			var result = await _seriesNameAccess.CountSeriesNames();
			return Ok(result);
		}
	}
}
