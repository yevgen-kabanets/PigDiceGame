using PigDiceGame.Core.Enums;
using PigDiceGame.Core.Interfaces;

namespace PigDiceGame.ConsoleClient
{
    class ConsolePlayer() : IPlayer
    {
       public Move GetNextMove()
        {
            while (true)
            {
                Console.WriteLine("Choose next move: 1 - Hold. 2 - Roll");
                var move = Console.ReadKey();
                Console.WriteLine();
                switch (move.KeyChar)
                {
                    case '1': return Move.Hold;
                    case '2': return Move.Roll;
                }
            }
        }
    }
}