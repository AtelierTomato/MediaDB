using AtelierTomato.MediaDB.Model;
using AtelierTomato.MediaDB.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AtelierTomato.MediaDB.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PartGroupInfoController : ControllerBase
	{
		private readonly IPartGroupInfoAccess _partGroupInfoAccess;
		public PartGroupInfoController(IPartGroupInfoAccess partGroupInfoAccess)
		{
			_partGroupInfoAccess = partGroupInfoAccess;
		}

		[HttpPost]
		public async Task<IActionResult> WritePartGroupInfo([FromBody] PartGroupInfo partGroupInfo)
		{
			if (partGroupInfo is null)
				return BadRequest("PartGroupInfo cannot be null.");

			await _partGroupInfoAccess.WritePartGroupInfo(partGroupInfo);
			return NoContent();
		}

		[HttpPost("range")]
		public async Task<IActionResult> WritePartGroupInfoRange([FromBody] IEnumerable<PartGroupInfo> partGroupInfoRange)
		{
			if (partGroupInfoRange is null || !partGroupInfoRange.Any())
				return BadRequest("PartGroupInfo range cannot be null.");

			await _partGroupInfoAccess.WritePartGroupInfoRange(partGroupInfoRange);
			return NoContent();
		}

		[HttpGet("{seriesID}/null")]
		public async Task<ActionResult<PartGroupInfo>> ReadPartGroupInfoNull(ulong seriesID) => await ReadPartGroupInfoCore(seriesID, null);
		[HttpGet("{seriesID}/{parentPartID}")]
		public async Task<ActionResult<PartGroupInfo>> ReadPartGroupInfo(ulong seriesID, PartID parentPartID) => await ReadPartGroupInfoCore(seriesID, parentPartID);
		private async Task<ActionResult<PartGroupInfo>> ReadPartGroupInfoCore(ulong seriesID, PartID? parentPartID)
		{
			var result = await _partGroupInfoAccess.ReadPartGroupInfo(seriesID, parentPartID);

			if (result is null)
				return NotFound();

			return Ok(result);
		}

		[HttpGet("{seriesID}")]
		public async Task<ActionResult<IEnumerable<PartGroupInfo>>> ReadPartGroupInfoRangeForSeries(ulong seriesID)
		{
			var result = await _partGroupInfoAccess.ReadPartGroupInfoRangeForSeries(seriesID);

			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpGet("all")]
		public async Task<ActionResult<IEnumerable<PartGroupInfo>>> ReadAllPartGroupInfos()
		{
			var result = await _partGroupInfoAccess.ReadAllPartGroupInfos();

			if (result is null || !result.Any())
				return NotFound();

			return Ok(result);
		}

		[HttpDelete("{seriesID}/null")]
		public async Task<IActionResult> DeletePartGroupInfoNull(ulong seriesID) => await DeletePartGroupInfoCore(seriesID, null);
		[HttpDelete("{seriesID}/{parentPartID}")]
		public async Task<IActionResult> DeletePartGroupInfo(ulong seriesID, PartID parentPartID) => await DeletePartGroupInfoCore(seriesID, parentPartID);
		private async Task<IActionResult> DeletePartGroupInfoCore(ulong seriesID, PartID? parentPartID)
		{
			await _partGroupInfoAccess.DeletePartGroupInfo(seriesID, parentPartID);
			return NoContent();
		}

		[HttpDelete("{seriesID}")]
		public async Task<IActionResult> DeletePartGroupInfoRangeForSeries(ulong seriesID)
		{
			await _partGroupInfoAccess.DeletePartGroupInfoRangeForSeries(seriesID);
			return NoContent();
		}
	}
}
