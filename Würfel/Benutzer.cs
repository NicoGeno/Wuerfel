namespace Würfel
{
    internal class Benutzer
    {
        public int Id { get; set; }
        public string Benutzername { get; set; } = null!;
        public string Passwort { get; set; } = null!;
        public string Email { get; set; } = null!;
        public double Balance { get; set; }
    }
}
