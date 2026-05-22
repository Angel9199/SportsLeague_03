namespace SportsLeague.Domain.Entities
{
    public class MatchLineup
    {
        public int Id { get; set; }

        // Foreign Keys
        public int MatchId { get; set; }
        public int PlayerId { get; set; }

        // Properties
        public bool IsStarter { get; set; }
        public string Position { get; set; }

        // Navegación
        public Match Match { get; set; }
        public Player Player { get; set; }
    }
}
