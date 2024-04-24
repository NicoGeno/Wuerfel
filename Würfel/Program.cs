using Microsoft.EntityFrameworkCore;
using Würfel.daten;

namespace Würfel
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            WürfelContext dbContext = new WürfelContext();
            Benutzer EingeloggterBenutzer = null;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Willkommen im Würfel Spiel von Nico");
                EingeloggterBenutzer = await Anmeldung(dbContext);
                if (EingeloggterBenutzer != null)
                    break;
            }
            int streak = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Willkommen im Würfel Spiel von Nico, Sie sind angemeldet mit dem benutzernamen {EingeloggterBenutzer.Benutzername}");
                Console.WriteLine("Menu");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("1. Spielen");
                Console.WriteLine("2. ScoreBoard");
                Console.WriteLine("3. Beenden");
                string Auswahl = Console.ReadLine();
                switch (Auswahl)
                {
                    case "1":
                        Console.Clear();
                        Random RandomZahl = new Random();
                        Console.WriteLine("Willkommen im würfel spiel von nico");
                        Console.WriteLine($"Aktuell Guthaben {EingeloggterBenutzer.Balance}");
                        Console.WriteLine("Wieviel wollen wie wetten?");
                        double Einsatz = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("auf welche zahl wollen sie wetten? (0-6)");
                        double EinsatzZahl = Convert.ToDouble(Console.ReadLine());
                        double Gewinn = 0;
                        int RandomNumber = RandomZahl.Next(6);

                        if (EinsatzZahl == RandomNumber)
                        {
                            Console.WriteLine($"Die zahl war {RandomNumber}");
                            Console.WriteLine("Sie haben gewonnen");
                            Gewinn += Einsatz * 10;
                            streak++;
                            Benutzer BenutzerToUpdate = await dbContext.Benutzer.FirstOrDefaultAsync(b => b.Id == EingeloggterBenutzer.Id); // Angenommen, die ID ist bekannt
                            if (BenutzerToUpdate != null)
                            {
                                BenutzerToUpdate.Balance += Gewinn;
                                await dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Die zahl war {RandomNumber}");
                            Console.WriteLine("Sie haben verloren");
                            Gewinn -= Einsatz;
                            streak = 0;
                            Benutzer BenutzerToUpdate = await dbContext.Benutzer.FirstOrDefaultAsync(b => b.Id == EingeloggterBenutzer.Id); // Angenommen, die ID ist bekannt
                            if (BenutzerToUpdate != null)
                            {
                                BenutzerToUpdate.Balance += Gewinn;
                                await dbContext.SaveChangesAsync();
                            }
                        }
                        dbContext.ScoreBoard.Add(new ScoreBoard { Benutzer = EingeloggterBenutzer, Win = Gewinn, Bet = Einsatz, Winstreak = streak });
                        await dbContext.SaveChangesAsync();
                        Console.WriteLine($"Gewinn: {Gewinn} ");
                        Console.WriteLine($"Guthaben: {EingeloggterBenutzer.Balance}");
                        Console.ReadKey();
                        break;
                    case "2":
                        ScoreBoardAnzeigen(EingeloggterBenutzer, dbContext);
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Falsche eingabe!");
                        Console.ReadKey();
                        break;
                }
            }
        }
        public static async Task<Benutzer> Anmeldung(WürfelContext dbContext)
        {
            Console.WriteLine("1. Anmelden");
            Console.WriteLine("2. Registrieren");
            Console.WriteLine("3. Beenden");
            string Anmeldewauswahl = Console.ReadLine();
            switch (Anmeldewauswahl)
            {
                case "1":
                    Console.Write("Geben Sie Ihren Benutzernamen an:");
                    string Username = Console.ReadLine();
                    Console.Write("Geben Sie Ihr Passwort an:");
                    string Password = Console.ReadLine();
                    Benutzer benutzerLogin = await dbContext.Benutzer.FirstOrDefaultAsync(b => b.Benutzername == Username && b.Passwort == Password);
                    if (benutzerLogin == null)
                    {
                        Console.WriteLine("Benutzername oder Passwort ist falsch.");
                        Console.ReadKey();
                        return null;
                    }
                    Console.Clear();
                    return benutzerLogin;
                case "2":
                    Console.WriteLine("Geben Sie Ihren Benutzernamen an:");
                    string benutzername = Console.ReadLine();
                    Console.WriteLine("Geben Sie Íhr Passwort an:");
                    string passwort = Console.ReadLine();
                    Console.WriteLine("Geben Sie ihre EMAIL an:");
                    string EMail = Console.ReadLine();
                    Benutzer benutzerNeu = new Benutzer { Benutzername = benutzername, Passwort = passwort, Email = EMail, Balance = 500 };
                    dbContext.Benutzer.Add(benutzerNeu);
                    dbContext.SaveChanges();
                    Console.Clear();
                    return benutzerNeu;
                case "3":
                    Environment.Exit(0);
                    return null;
                default:
                    Console.WriteLine("Falsche Eingabe!");
                    Console.ReadKey();
                    Console.Clear();
                    return null;
            }
        }
        public static async Task ScoreBoardAnzeigen(Benutzer AktuellerBenutzer, WürfelContext dbContext)
        {
            Console.Clear();
            var AlleScores = await dbContext.ScoreBoard
                                                  .Include(b => b.Benutzer) // Includiert die Kundendaten
                                                  .Where(l => l.Benutzer == AktuellerBenutzer)
                                                  .ToListAsync();

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Alle Scores:");
            foreach (var Scores in AlleScores)
            {
                Console.WriteLine();
                Console.Write($"ID: {Scores.Id} ".PadRight(10));
                Console.Write($"Winstreak: {Scores.Winstreak} ".PadRight(20));
                Console.Write($"Gewinn: {Scores.Win} ".PadRight(20));
                Console.Write($"Einsatz: {Scores.Bet} ".PadRight(20));
                Console.Write($"Benutzername: {Scores.Benutzer.Benutzername}");
            }
            Console.ReadKey();
        }
    }
}