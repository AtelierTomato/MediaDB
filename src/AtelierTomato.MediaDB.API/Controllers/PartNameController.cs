using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AtelierTomato.MediaDB.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PartNameController : ControllerBase
	{
		private readonly IPartNameAccess _partNameAccess;
		public PartNameController(IPartNameAccess partNameAccess)
		{
			_partNameAccess = partNameAccess;
		}

		[HttpPost]
		public async Task<IActionResult> WritePartName([FromBody] PartName partName)
		{
			if (partName is null)
				return BadRequest("PartName cannot be null.");

			await _partNameAccess.WritePartName(partName);
			return NoContent();
		}

		[HttpPost("range")]
		public async Task<IActionResult> WritePartNameRange([FromBody] IEnumerable<PartName> partNameRange)
		{
			if (partNameRange is null || !partNameRange.Any())
				return BadRequest("PartName range cannot be null.");

			await _partNameAccess.WritePartNameRange(partNameRange);
			return NoContent();
		}

		[HttpGet("{seriesID}/{partID}/{language}/{script}")]
		public async Task<ActionResult<PartName>> ReadPartName(ulong seriesID, PartID partID, CultureInfo language, ScriptType script)
		{
			var result = await _partNameAccess.ReadPartName(seriesID, partID, language, script);
			if (result is null)
				return NotFound();

			return Ok(result);
		}

		[HttpGet("{seriesID}/all/{language}/{script}")]
		public async Task<ActionResult<IEnumerable<PartName>>> ReadPartNameRangeForSeriesWithLanguage(ulong seriesID, CultureInfo language, ScriptType script)
		{
			var result = await _partNameAccess.ReadPartNameRangeForSeriesWithLanguage(seriesID, language, script);
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("{seriesID}/{partID}")]
		public async Task<ActionResult<IEnumerable<PartName>>> ReadPartNameRangeForPart(ulong seriesID, PartID partID)
		{
			var result = await _partNameAccess.ReadPartNameRangeForPart(seriesID, partID);
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("{seriesID}")]
		public async Task<ActionResult<IEnumerable<PartName>>> ReadPartNameRangeForSeries(ulong seriesID)
		{
			var result = await _partNameAccess.ReadPartNameRangeForSeries(seriesID);
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("all")]
		public async Task<ActionResult<IEnumerable<PartName>>> ReadAllPartNames()
		{
			var result = await _partNameAccess.ReadAllPartNames();
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpDelete("{seriesID}/{partID}/{language}/{script}")]
		public async Task<IActionResult> DeletePartName(ulong seriesID, PartID partID, CultureInfo language, ScriptType script)
		{
			await _partNameAccess.DeletePartName(seriesID, partID, language, script);
			return NoContent();
		}

		[HttpDelete("{seriesID}/{partID}")]
		public async Task<IActionResult> DeletePartNameRangeForPart(ulong seriesID, PartID partID)
		{
			await _partNameAccess.DeletePartNameRangeForPart(seriesID, partID);
			return NoContent();
		}

		[HttpDelete("{seriesID}")]
		public async Task<IActionResult> DeletePartNameRangeForSeries(ulong seriesID)
		{
			await _partNameAccess.DeletePartNameRangeForSeries(seriesID);
			return NoContent();
		}

		[HttpGet("count")]
		public async Task<ActionResult<int>> CountPartNames()
		{
			var result = await _partNameAccess.CountPartNames();
			return Ok(result);
		}
	}
}
