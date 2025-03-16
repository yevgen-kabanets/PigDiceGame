using PigDiceGame.Core;
using PigDiceGame.Core.Interfaces;

namespace PigDiceGame.ConsoleClient
{
    class ConsoleDice : IDice
    {
        private readonly Dice _dice = new();

        public int Roll()
        {
            var result = _dice.Roll();
            Console.WriteLine($"Dice result: {result}");
            return result;
        }
    }
}
