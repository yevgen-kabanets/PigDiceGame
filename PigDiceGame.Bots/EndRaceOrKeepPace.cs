using PigDiceGame.Core.Enums;
using PigDiceGame.Core.Interfaces;
using PigDiceGame.Core;

namespace PigDiceGame.Bots
{
    public class EndRaceOrKeepPace(Game game) : IPlayer
    {
        public Move GetNextMove()
        {
            if (game.PlayersScores.Values.Any(score => score >= 71))
            {
                return Move.Roll;
            }
            var maxOpponentScore = game.PlayersScores.Where(kvp => kvp.Key != this).Select(kvp => kvp.Value).Max();
            var threshold = 21 + Math.Ceiling((maxOpponentScore - game.PlayersScores[this]) / 8.0);
            return game.TurnTotal >= threshold ? Move.Hold : Move.Roll;
        }
    }
}
