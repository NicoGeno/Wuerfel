namespace Würfel
{
    internal class ScoreBoard
    {
        public int Id { get; set; }
        public Benutzer Benutzer { get; set; } = null!;
        public int Winstreak { get; set; }
        public double Bet { get; set; }
        public double Win { get; set; }
    }
}
