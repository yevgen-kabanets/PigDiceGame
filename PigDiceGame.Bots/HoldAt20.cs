using PigDiceGame.Core;
using PigDiceGame.Core.Enums;
using PigDiceGame.Core.Interfaces;

namespace PigDiceGame.Bots
{
    public class HoldAt20(Game game) : IPlayer
    {
        public Move GetNextMove()
        {
            return game.TurnTotal >= 20 ? Move.Hold : Move.Roll;
        }
    }
}
