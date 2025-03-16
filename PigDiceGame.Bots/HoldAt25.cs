using PigDiceGame.Core;
using PigDiceGame.Core.Enums;
using PigDiceGame.Core.Interfaces;

namespace PigDiceGame.Bots
{
    public class HoldAt25(Game game) : IPlayer
    {
        public Move GetNextMove()
        {
            return game.TurnTotal >= 25 ? Move.Hold : Move.Roll;
        }
    }
}
