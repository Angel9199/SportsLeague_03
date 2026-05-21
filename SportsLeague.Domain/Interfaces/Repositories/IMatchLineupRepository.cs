using SportsLeague.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IMatchLineupRepository
    {
        Task<MatchLineup?> GetByIdAsync(int id);
        Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);
        Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId);
        Task<bool> ExistsAsync(int matchId, int playerId);
        Task<int> CountStartersAsync(int matchId, int teamId);
        Task AddAsync(MatchLineup lineup);
        void Delete(MatchLineup lineup);
        Task SaveChangesAsync();
    }
}
