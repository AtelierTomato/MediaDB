using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AtelierTomato.MediaDB.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SeriesParentController : ControllerBase
	{
		private readonly ISeriesParentAccess _seriesParentAccess;
		public SeriesParentController(ISeriesParentAccess seriesParentAccess)
		{
			_seriesParentAccess = seriesParentAccess;
		}

		[HttpPost]
		public async Task<IActionResult> WriteSeriesParent([FromBody] SeriesParent seriesParent)
		{
			if (seriesParent is null)
				return BadRequest("SeriesParent cannot be null.");

			await _seriesParentAccess.WriteSeriesParent(seriesParent);
			return NoContent();
		}

		[HttpPost("range")]
		public async Task<IActionResult> WriteSeriesParentRange([FromBody] IEnumerable<SeriesParent> seriesParentRange)
		{
			if (seriesParentRange is null || !seriesParentRange.Any())
				return BadRequest("SeriesParent range cannot be null.");

			await _seriesParentAccess.WriteSeriesParentRange(seriesParentRange);
			return NoContent();
		}

		[HttpGet("{ID}/{parentID}")]
		public async Task<ActionResult<SeriesParent>> ReadSeriesParent(ulong ID, ulong parentID)
		{
			var result = await _seriesParentAccess.ReadSeriesParent(ID, parentID);
			if (result is null)
				return NotFound();

			return Ok(result);
		}

		[HttpGet("byseries/{ID}")]
		public async Task<ActionResult<IEnumerable<SeriesParent>>> ReadSeriesParentRangeBySeries(ulong ID)
		{
			var result = await _seriesParentAccess.ReadSeriesParentRangeBySeries(ID);
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("byparent/{parentID}")]
		public async Task<ActionResult<IEnumerable<SeriesParent>>> ReadSeriesParentRangeByParentSeries(ulong parentID)
		{
			var result = await _seriesParentAccess.ReadSeriesParentRangeByParentSeries(parentID);
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("all")]
		public async Task<ActionResult<IEnumerable<SeriesParent>>> ReadAllSeriesParents()
		{
			var result = await _seriesParentAccess.ReadAllSeriesParents();
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpDelete("{ID}/{parentID}")]
		public async Task<IActionResult> DeleteSeriesParent(ulong ID, ulong parentID)
		{
			await _seriesParentAccess.DeleteSeriesParent(ID, parentID);
			return NoContent();
		}

		[HttpDelete("byseries/{ID}")]
		public async Task<IActionResult> DeleteSeriesParentRangeBySeries(ulong ID)
		{
			await _seriesParentAccess.DeleteSeriesParentRangeBySeries(ID);
			return NoContent();
		}

		[HttpDelete("byparent/{parentID}")]
		public async Task<IActionResult> DeleteSeriesParentRangeByParentSeries(ulong parentID)
		{
			await _seriesParentAccess.DeleteSeriesParentRangeByParentSeries(parentID);
			return NoContent();
		}
	}
}
