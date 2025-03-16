using PigDiceGame.Bots;
using PigDiceGame.ConsoleClient;
using PigDiceGame.Core;
using PigDiceGame.Core.Enums;
using PigDiceGame.Core.Interfaces;
using System.Numerics;

var game = new Game(new ConsoleDice());
Dictionary<IPlayer, string> players;
ResetGame();

void ResetGame()
{
    game.Reset();
    players = [];
    while (true)
    {
        Console.WriteLine("1 - Add player. 2 - Add bot. 3 - Start game. 4 - Reset.");
        var choice = Console.ReadKey();
        Console.WriteLine();
        try
        {
            switch (choice.KeyChar)
            {
                case '1':
                    AddPlayer();
                    break;
                case '2':
                    AddBot();
                    break;
                case '3':
                    StartGame();
                    return;
                case '4':
                    ResetGame();
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

void StartGame()
{
    game.NewGame();
    Console.WriteLine("Game started.");
    while (game.State == GameState.Started)
    {
        Console.WriteLine($"\n{players[game.CurrentPlayer!]} move. Score: {game.PlayersScores[game.CurrentPlayer!]}. Turn total: {game.TurnTotal}");
        game.MakeMove();
    }
    Console.WriteLine($"\n{players[game.CurrentPlayer!]} won!!!");
    Console.WriteLine("Scores:");
    foreach (var player in players.Keys)
    {
        Console.WriteLine($"{players[player]}: {game.PlayersScores[player]}");
    }

    while (true)
    {
        Console.WriteLine("\n1 - Start new game. 2 - Reset game. 3 - Exit.");
        var choice = Console.ReadKey();
        Console.WriteLine();
        try
        {
            switch (choice.KeyChar)
            {
                case '1':
                    StartGame();
                    return;
                case '2':
                    ResetGame();
                    return;
                case '3':
                    return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

void AddPlayer()
{
    string? name = null;
    while (name == null)
    {
        Console.WriteLine("Enter player name:");
        name = Console.ReadLine();
    }
    var player = new ConsolePlayer();
    players[player] = name;
    game.AddPlayer(player);
}

void AddBot()
{
    while (true)
    {
        Console.WriteLine($"1 - {nameof(HoldAt20)}. 2 - {nameof(HoldAt25)}. 3 - {nameof(FourScoringTurns)}. 4 - {nameof(EndRaceOrKeepPace)}.");
        var choice = Console.ReadKey();
        Console.WriteLine();
        IPlayer? player = choice.KeyChar switch
        {
            '1' => new HoldAt20(game),
            '2' => new HoldAt25(game),
            '3' => new FourScoringTurns(game),
            '4' => new EndRaceOrKeepPace(game),
            _ => null
        };

        if (player != null)
        {
            players[player] = player.GetType().Name;
            game.AddPlayer(player);
            return;
        }
    }
}