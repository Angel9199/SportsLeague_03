using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

[ApiController]
[Route("api/match/{matchId}/lineup")]
public class MatchLineupController : ControllerBase
{
    private readonly IMatchLineupService _service;
    private readonly IMapper _mapper;

    public MatchLineupController(IMatchLineupService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddLineup(int matchId, [FromBody] MatchLineupRequestDto dto)
    {
        var lineup = await _service.AddLineupAsync(matchId, dto.PlayerId, dto.IsStarter, dto.Position);
        var response = _mapper.Map<MatchLineupResponseDto>(lineup);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetLineup(int matchId)
    {
        var lineups = await _service.GetLineupAsync(matchId);
        var response = _mapper.Map<IEnumerable<MatchLineupResponseDto>>(lineups);
        return Ok(response);
    }

    [HttpGet("team/{teamId}")]
    public async Task<IActionResult> GetTeamLineup(int matchId, int teamId)
    {
        var lineups = await _service.GetTeamLineupAsync(matchId, teamId);
        var response = _mapper.Map<IEnumerable<MatchLineupResponseDto>>(lineups);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLineup(int matchId, int id)
    {
        await _service.DeleteLineupAsync(matchId, id);
        return NoContent();
    }
}

