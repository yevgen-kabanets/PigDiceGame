using PigDiceGame.Core.Enums;
using PigDiceGame.Core.Interfaces;

namespace PigDiceGame.Core
{
    public class Game(IDice dice) : IGame
    {
        private OrderedDictionary<IPlayer, int> _playersScores = new();

        public IReadOnlyDictionary<IPlayer, int> PlayersScores { get => _playersScores; }
        public IPlayer? CurrentPlayer { get; private set; }
        public int TurnTotal { get; private set; }
        public GameState State { get; private set; }

        public void Reset()
        {
            State = GameState.NotStarted;
            _playersScores = new();
            TurnTotal = 0;
            CurrentPlayer = null;
        }

        public void NewGame()
        {
            if (PlayersScores.Count < 2)
            {
                throw new Exception("Can't start a game with less than 2 players.");
            }

            State = GameState.Started;
            foreach (var player in _playersScores.Keys)
            {
                _playersScores[player] = 0;
            }
            TurnTotal = 0;
            CurrentPlayer = _playersScores.GetAt(0).Key;
        }

        public void AddPlayer(IPlayer player)
        {
            if (State == GameState.Started || State == GameState.Ended)
            {
                throw new Exception("Can't add a player to existing game.");
            }

            _playersScores.Add(player, 0);
        }

        public void MakeMove()
        {
            if (State != GameState.Started)
            {
                throw new Exception("Please start a game to make a move.");
            }

            var move = CurrentPlayer!.GetNextMove();

            if (move == Move.Roll)
            {
                Roll();
            }
            else
            {
                Hold();
            }
        }

        private void Roll()
        {
            var diceResult = dice.Roll();

            if (diceResult == 1)
            {
                NextPlayersTurn();
            }
            else
            {
                TurnTotal += diceResult;
            }

            if (_playersScores[CurrentPlayer!] + TurnTotal >= 100)
            {
                _playersScores[CurrentPlayer!] += TurnTotal;
                State = GameState.Ended;
            }
        }

        private void Hold()
        {
            _playersScores[CurrentPlayer!] += TurnTotal;
            NextPlayersTurn();
        }

        private void NextPlayersTurn()
        {
            var nextPlayersIndex = (_playersScores.IndexOf(CurrentPlayer!) + 1) % _playersScores.Count;

            CurrentPlayer = _playersScores.GetAt(nextPlayersIndex).Key;
            TurnTotal = 0;
        }
    }
}
