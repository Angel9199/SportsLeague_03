using SportsLeague.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IMatchLineupService
    {
        Task<MatchLineup> AddLineupAsync(int matchId, int playerId, bool isStarter, string position);
        Task<IEnumerable<MatchLineup>> GetLineupAsync(int matchId);
        Task<IEnumerable<MatchLineup>> GetTeamLineupAsync(int matchId, int teamId);
        Task DeleteLineupAsync(int matchId, int lineupId);
    }
}
