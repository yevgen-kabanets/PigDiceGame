using NSubstitute;
using PigDiceGame.Core.Enums;
using PigDiceGame.Core.Interfaces;

namespace PigDiceGame.Core.Tests
{
    public class GameTests
    {
        private Game _sut;
        private IDice _dice;
        private IPlayer _player1;
        private IPlayer _player2;

        [SetUp]
        public void Setup()
        {
            _dice = Substitute.For<IDice>();
            _sut = new Game(_dice);
            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            _sut.AddPlayer(_player1);
            _sut.AddPlayer(_player2);
            _sut.NewGame();
        }

        [Test]
        public void Reset_ResetsTheGame()
        {
            // Act
            _sut.Reset();

            // Assert
            Assert.That(_sut.State, Is.EqualTo(GameState.NotStarted));
            Assert.That(_sut.PlayersScores.Count, Is.EqualTo(0));
            Assert.That(_sut.CurrentPlayer, Is.Null);
            Assert.That(_sut.TurnTotal, Is.EqualTo(0));
        }

        [Test]
        public void NewGame_StartsNewGameWithSamePlayers()
        {
            // Act
            _sut.NewGame();

            // Assert
            Assert.That(_sut.State, Is.EqualTo(GameState.Started));
            Assert.That(_sut.PlayersScores.Count, Is.EqualTo(2));
            Assert.That(_sut.PlayersScores[_player1], Is.EqualTo(0));
            Assert.That(_sut.PlayersScores[_player2], Is.EqualTo(0));
            Assert.That(_sut.CurrentPlayer, Is.EqualTo(_player1));
            Assert.That(_sut.TurnTotal, Is.EqualTo(0));
        }

        [Test]
        public void NewGame_LessThanTwoPlayers_ThrowsException()
        {
            // Act
            _sut.Reset();
            _sut.AddPlayer(_player1);

            // Assert
            var ex = Assert.Throws<Exception>(_sut.NewGame);
            Assert.That(ex.Message, Is.EqualTo("Can't start a game with less than 2 players."));
        }

        [Test]
        public void AddPlayer_AddsPlayerToGame()
        {
            // Arrange
            _sut.Reset();
            var _player3 = Substitute.For<IPlayer>();

            // Act
            _sut.AddPlayer(_player3);

            // Assert
            Assert.That(_sut.PlayersScores.Count, Is.EqualTo(1));
            Assert.That(_sut.PlayersScores[_player3], Is.EqualTo(0));
            Assert.That(_sut.TurnTotal, Is.EqualTo(0));
        }

        [Test]
        public void AddPlayer_GivenExistingGame_ThrowsException()
        {
            // Arrange
            var _player3 = Substitute.For<IPlayer>();

            // Act
            // Assert
            var ex = Assert.Throws<Exception>(() => _sut.AddPlayer(_player3));
            Assert.That(ex.Message, Is.EqualTo("Can't add a player to existing game."));
        }

        [Test]
        public void MakeMove_PlayerRollsOne_ScoreNotUpdatedAndNextPlayersTurn()
        {
            // Arrange
            _player1.GetNextMove().Returns(Move.Roll);
            _dice.Roll().Returns(1);

            // Act
            _sut.MakeMove();

            // Assert
            Assert.That(_sut.PlayersScores[_player1], Is.EqualTo(0));
            Assert.That(_sut.CurrentPlayer, Is.EqualTo(_player2));
            Assert.That(_sut.TurnTotal, Is.EqualTo(0));
        }

        [Test]
        public void MakeMove_PlayerRollsNotOne_TurnTotalUpdatedAndTurnContinues()
        {
            // Arrange
            _player1.GetNextMove().Returns(Move.Roll);
            _dice.Roll().Returns(5);

            // Act
            _sut.MakeMove();

            // Assert
            Assert.That(_sut.PlayersScores[_player1], Is.EqualTo(0));
            Assert.That(_sut.CurrentPlayer, Is.EqualTo(_player1));
            Assert.That(_sut.TurnTotal, Is.EqualTo(5));
        }

        [Test]
        public void MakeMove_PlayerHolds_ScoreUpdatedAndNextPlayersTurn()
        {
            // Arrange
            _player1.GetNextMove().Returns(Move.Roll);
            _dice.Roll().Returns(5);

            // Act
            _sut.MakeMove();
            _player1.GetNextMove().Returns(Move.Hold);
            _sut.MakeMove();

            // Assert
            Assert.That(_sut.PlayersScores[_player1], Is.EqualTo(5));
            Assert.That(_sut.CurrentPlayer, Is.EqualTo(_player2));
            Assert.That(_sut.TurnTotal, Is.EqualTo(0));
        }

        [Test]
        public void MakeMove_TotalScore100_GameEnded()
        {
            // Arrange
            _player1.GetNextMove().Returns(Move.Roll);
            _dice.Roll().Returns(5);

            // Act
            for (int i = 0; i < 20; i++)
            {
                _sut.MakeMove();
            }

            // Assert
            Assert.That(_sut.State, Is.EqualTo(GameState.Ended));
            Assert.That(_sut.PlayersScores[_player1], Is.EqualTo(100));
            Assert.That(_sut.CurrentPlayer, Is.EqualTo(_player1));
        }

        [Test]
        public void MakeMove_AfterLastPlayer_TurnGoesToFirstPlayer()
        {
            // Act
            _player1.GetNextMove().Returns(Move.Hold);
            _sut.MakeMove();
            _player2.GetNextMove().Returns(Move.Hold);
            _sut.MakeMove();

            // Assert
            Assert.That(_sut.CurrentPlayer, Is.EqualTo(_player1));
        }
    }
}
