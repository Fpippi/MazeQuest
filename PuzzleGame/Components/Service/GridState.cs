namespace MazeGame.Services
{
    public class GridState
    {
        private int[,] _grid;
        public int[,] Grid
        {
            get => _grid;
            set
            {
                _grid = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public delegate void StateChangedHandler();
        public event StateChangedHandler OnChange;

        public required Difficulty DifficultyChosen { get; set; } = Difficulty.None;
        public int[,] RandomGrid(Difficulty difficulty)
        {
            int size = 6;

            switch (difficulty)
            {
                case Difficulty.Easy:
                    size = 6;
                    break;
                case Difficulty.Medium:
                    size = 8;
                    break;
                case Difficulty.Hard:
                    size = 10;
                    break;
                case Difficulty.None:
                    size = 6;
                    break;
            }

            int[,] grid = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = 0;
                }
            }
            for (int i = 0; i < size; i++)
            {
                grid[0, i] = 1;
                grid[size - 1, i] = 1;
                grid[i, 0] = 1;
                grid[i, size - 1] = 1;
            }
            for (int i = 1; i < size - 1; i++)
            {
                for (int j = 1; j < size - 1; j++)
                {
                    grid[i, j] = 1;
                }
            }
            return grid;
        }
    }

    public enum Difficulty
    {
        None,
        Easy,
        Medium,
        Hard
    }
}