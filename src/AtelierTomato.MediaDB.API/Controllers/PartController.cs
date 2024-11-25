using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AtelierTomato.MediaDB.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PartController : ControllerBase
	{
		private readonly IPartAccess _partAccess;
		public PartController(IPartAccess partAccess)
		{
			_partAccess = partAccess;
		}

		[HttpPost]
		public async Task<IActionResult> WritePart([FromBody] Part part)
		{
			if (part is null)
				return BadRequest("Part cannot be null.");

			await _partAccess.WritePart(part);
			return NoContent();
		}

		[HttpPost("range")]
		public async Task<IActionResult> WritePartRange([FromBody] IEnumerable<Part> partRange)
		{
			if (partRange is null || !partRange.Any())
				return BadRequest("Part range cannot be null.");

			await _partAccess.WritePartRange(partRange);
			return NoContent();
		}

		[HttpGet("{seriesID}/{partID}")]
		public async Task<ActionResult<Part>> ReadPart(ulong seriesID, PartID partID)
		{
			var result = await _partAccess.ReadPart(seriesID, partID);
			if (result is null)
				return NotFound();

			return Ok(result);
		}

		[HttpGet("{seriesID}")]
		public async Task<ActionResult<IEnumerable<Part>>> ReadPartRangeBySeries(ulong seriesID)
		{
			var result = await _partAccess.ReadPartRangeBySeries(seriesID);
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("byseriesrange")]
		public async Task<ActionResult<IEnumerable<Part>>> ReadPartRangeBySeriesRange([FromQuery] IEnumerable<ulong> seriesIDRange)
		{
			var result = await _partAccess.ReadPartRangeBySeriesRange(seriesIDRange);
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("all")]
		public async Task<ActionResult<IEnumerable<Part>>> ReadAllParts()
		{
			var result = await _partAccess.ReadAllParts();
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpDelete("{seriesID}/{partID}")]
		public async Task<IActionResult> DeletePart(ulong seriesID, PartID partID)
		{
			await _partAccess.DeletePart(seriesID, partID);
			return NoContent();
		}

		[HttpDelete("{seriesID}")]
		public async Task<IActionResult> DeletePartRangeInSeries(ulong seriesID, [FromBody] IEnumerable<PartID> partIDRange)
		{
			await _partAccess.DeletePartRangeInSeries(seriesID, partIDRange);
			return NoContent();
		}
	}
}
