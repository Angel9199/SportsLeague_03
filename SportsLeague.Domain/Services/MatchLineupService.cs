using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

public class MatchLineupService : IMatchLineupService
{
    private readonly IMatchRepository _matchRepo;
    private readonly IPlayerRepository _playerRepo;
    private readonly IMatchLineupRepository _lineupRepo;

    public MatchLineupService(IMatchRepository matchRepo, IPlayerRepository playerRepo, IMatchLineupRepository lineupRepo)
    {
        _matchRepo = matchRepo;
        _playerRepo = playerRepo;
        _lineupRepo = lineupRepo;
    }

    public async Task<MatchLineup> AddLineupAsync(int matchId, int playerId, bool isStarter, string position)
    {
        var match = await _matchRepo.GetByIdAsync(matchId)
            ?? throw new InvalidOperationException($"No se encontró el partido con ID {matchId}");

        var player = await _playerRepo.GetByIdAsync(playerId)
            ?? throw new InvalidOperationException($"No se encontró el jugador con ID {playerId}");

        if (player.TeamId != match.HomeTeamId && player.TeamId != match.AwayTeamId)
            throw new InvalidOperationException("El jugador no pertenece a ninguno de los equipos del partido");

        if (await _lineupRepo.ExistsAsync(matchId, playerId))
            throw new InvalidOperationException("El jugador ya está registrado en la alineación de este partido");

        if (isStarter)
        {
            int startersCount = await _lineupRepo.CountStartersAsync(matchId, player.TeamId);
            if (startersCount >= 11)
                throw new InvalidOperationException("El equipo ya tiene 11 titulares registrados en este partido");
        }

        if (match.Status != MatchStatus.Scheduled)
            throw new InvalidOperationException("Solo se pueden registrar alineaciones en partidos Scheduled");

        var lineup = new MatchLineup
        {
            MatchId = matchId,
            PlayerId = playerId,
            IsStarter = isStarter,
            Position = position
        };

        await _lineupRepo.AddAsync(lineup);
        await _lineupRepo.SaveChangesAsync();

        return lineup;
    }

    public async Task<IEnumerable<MatchLineup>> GetLineupAsync(int matchId) =>
        await _lineupRepo.GetByMatchAsync(matchId);

    public async Task<IEnumerable<MatchLineup>> GetTeamLineupAsync(int matchId, int teamId) =>
        await _lineupRepo.GetByMatchAndTeamAsync(matchId, teamId);

    public async Task DeleteLineupAsync(int matchId, int lineupId)
    {
        var lineup = await _lineupRepo.GetByIdAsync(lineupId)
            ?? throw new InvalidOperationException($"No se encontró la alineación con ID {lineupId}");

        if (lineup.MatchId != matchId)
            throw new InvalidOperationException("La alineación no corresponde al partido indicado");

        _lineupRepo.Delete(lineup);
        await _lineupRepo.SaveChangesAsync();
    }
}
