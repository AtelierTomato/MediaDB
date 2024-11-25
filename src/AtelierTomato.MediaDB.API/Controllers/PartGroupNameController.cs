using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AtelierTomato.MediaDB.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PartGroupNameController : ControllerBase
	{
		private readonly IPartGroupNameAccess _partGroupNameAccess;
		public PartGroupNameController(IPartGroupNameAccess partGroupNameAccess)
		{
			_partGroupNameAccess = partGroupNameAccess;
		}

		[HttpPost]
		public async Task<IActionResult> WritePartGroupName([FromBody] PartGroupName partGroupName)
		{
			if (partGroupName is null)
				return BadRequest("PartGroupName cannot be null.");

			await _partGroupNameAccess.WritePartGroupName(partGroupName);
			return NoContent();
		}

		[HttpPost("range")]
		public async Task<IActionResult> WritePartGroupNameRange([FromBody] IEnumerable<PartGroupName> partGroupNameRange)
		{
			if (partGroupNameRange is null || !partGroupNameRange.Any())
				return BadRequest("PartGroupName range cannot be null.");

			await _partGroupNameAccess.WritePartGroupNameRange(partGroupNameRange);
			return NoContent();
		}

		[HttpGet("{seriesID}/null/{language}/{script}")]
		public async Task<ActionResult<PartGroupName>> ReadPartGroupNameNull(ulong seriesID, CultureInfo language, ScriptType script) => await ReadPartGroupNameCore(seriesID, null, language, script);
		[HttpGet("{seriesID}/{parentPartID}/{language}/{script}")]
		public async Task<ActionResult<PartGroupName>> ReadPartGroupName(ulong seriesID, PartID parentPartID, CultureInfo language, ScriptType script) => await ReadPartGroupNameCore(seriesID, parentPartID, language, script);
		private async Task<ActionResult<PartGroupName>> ReadPartGroupNameCore(ulong seriesID, PartID? parentPartID, CultureInfo language, ScriptType script)
		{
			var result = await _partGroupNameAccess.ReadPartGroupName(seriesID, parentPartID, language, script);
			if (result is null)
				return NotFound();

			return Ok(result);
		}

		[HttpGet("{seriesID}/all/{language}/{script}")]
		public async Task<ActionResult<IEnumerable<PartGroupName>>> ReadPartGroupNameRangeForSeriesWithLanguage(ulong seriesID, CultureInfo language, ScriptType script)
		{
			var result = await _partGroupNameAccess.ReadPartGroupNameRangeForSeriesWithLanguage(seriesID, language, script);
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("{seriesID}/null")]
		public async Task<ActionResult<IEnumerable<PartGroupName>>> ReadPartGroupNameRangeForPartNull(ulong seriesID) => await ReadPartGroupNameRangeForPartCore(seriesID, null);
		[HttpGet("{seriesID}/{parentPartID}")]
		public async Task<ActionResult<IEnumerable<PartGroupName>>> ReadPartGroupNameRangeForPart(ulong seriesID, PartID parentPartID) => await ReadPartGroupNameRangeForPartCore(seriesID, parentPartID);
		private async Task<ActionResult<IEnumerable<PartGroupName>>> ReadPartGroupNameRangeForPartCore(ulong seriesID, PartID? parentPartID)
		{
			var result = await _partGroupNameAccess.ReadPartGroupNameRangeForParentPart(seriesID, parentPartID);
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("{seriesID}")]
		public async Task<ActionResult<IEnumerable<PartGroupName>>> ReadPartGroupNameRangeForSeries(ulong seriesID)
		{
			var result = await _partGroupNameAccess.ReadPartGroupNameRangeForSeries(seriesID);
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("all")]
		public async Task<ActionResult<IEnumerable<PartGroupName>>> ReadAllPartGroupNames()
		{
			var result = await _partGroupNameAccess.ReadAllPartGroupNames();
			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpDelete("{seriesID}/null/{language}/{script}")]
		public async Task<IActionResult> DeletePartGroupNameNull(ulong seriesID, CultureInfo language, ScriptType script) => await DeletePartGroupNameCore(seriesID, null, language, script);
		[HttpDelete("{seriesID}/{parentPartID}/{language}/{script}")]
		public async Task<IActionResult> DeletePartGroupName(ulong seriesID, PartID parentPartID, CultureInfo language, ScriptType script) => await DeletePartGroupNameCore(seriesID, parentPartID, language, script);
		private async Task<IActionResult> DeletePartGroupNameCore(ulong seriesID, PartID? parentPartID, CultureInfo language, ScriptType script)
		{
			await _partGroupNameAccess.DeletePartGroupName(seriesID, parentPartID, language, script);
			return NoContent();
		}

		[HttpDelete("{seriesID}/null")]
		public async Task<IActionResult> DeletePartGroupNameRangeForPartNull(ulong seriesID) => await DeletePartGroupNameRangeForPartCore(seriesID, null);
		[HttpDelete("{seriesID}/{parentPartID}")]
		public async Task<IActionResult> DeletePartGroupNameRangeForPart(ulong seriesID, PartID parentPartID) => await DeletePartGroupNameRangeForPartCore(seriesID, parentPartID);
		private async Task<IActionResult> DeletePartGroupNameRangeForPartCore(ulong seriesID, PartID? parentPartID)
		{
			await _partGroupNameAccess.DeletePartGroupNameRangeForPart(seriesID, parentPartID);
			return NoContent();
		}

		[HttpDelete("{seriesID}")]
		public async Task<IActionResult> DeletePartGroupNameRangeForSeries(ulong seriesID)
		{
			await _partGroupNameAccess.DeletePartGroupNameRangeForSeries(seriesID);
			return NoContent();
		}
	}
}
