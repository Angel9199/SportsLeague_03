using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class MatchLineupRepository : IMatchLineupRepository
    {
        private readonly LeagueDbContext _context;

        public MatchLineupRepository(LeagueDbContext context)
        {
            _context = context;
        }

        public async Task<MatchLineup?> GetByIdAsync(int id) =>
            await _context.Set<MatchLineup>()
                          .Include(ml => ml.Player)
                          .ThenInclude(p => p.Team)
                          .FirstOrDefaultAsync(ml => ml.Id == id);

        public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId) =>
            await _context.Set<MatchLineup>()
                          .Include(ml => ml.Player)
                          .ThenInclude(p => p.Team)
                          .Where(ml => ml.MatchId == matchId)
                          .ToListAsync();

        public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId) =>
            await _context.Set<MatchLineup>()
                          .Include(ml => ml.Player)
                          .ThenInclude(p => p.Team)
                          .Where(ml => ml.MatchId == matchId && ml.Player.TeamId == teamId)
                          .ToListAsync();

        public async Task<bool> ExistsAsync(int matchId, int playerId) =>
            await _context.Set<MatchLineup>()
                          .AnyAsync(ml => ml.MatchId == matchId && ml.PlayerId == playerId);

        public async Task<int> CountStartersAsync(int matchId, int teamId) =>
            await _context.Set<MatchLineup>()
                          .CountAsync(ml => ml.MatchId == matchId && ml.Player.TeamId == teamId && ml.IsStarter);

        public async Task AddAsync(MatchLineup lineup)
        {
            await _context.Set<MatchLineup>().AddAsync(lineup);
        }

        public void Delete(MatchLineup lineup)
        {
            _context.Set<MatchLineup>().Remove(lineup);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
