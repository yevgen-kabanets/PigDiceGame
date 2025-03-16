using PigDiceGame.Core.Enums;
using PigDiceGame.Core.Interfaces;
using PigDiceGame.Core;

namespace PigDiceGame.Bots
{
    public class FourScoringTurns(Game game) : IPlayer
    {
        public Move GetNextMove()
        {
            var score = game.PlayersScores[this];
            var threshold = Math.Ceiling((100.0 - score)/(4 - score / 25));
            return game.TurnTotal >= threshold ? Move.Hold : Move.Roll;
        }
    }
}
