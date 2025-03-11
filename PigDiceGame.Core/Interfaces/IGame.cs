namespace PigDiceGame.Core.Interfaces
{
    public interface IGame
    {
        void Reset();
        void NewGame();
        void AddPlayer(IPlayer player);
        void MakeMove();
    }
}
