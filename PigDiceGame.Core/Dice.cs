using PigDiceGame.Core.Interfaces;

namespace PigDiceGame.Core
{
    public class Dice : IDice
    {
        private readonly Random _random = new();

        public int Roll()
        {
            return _random.Next(1, 6);
        }
    }
}
