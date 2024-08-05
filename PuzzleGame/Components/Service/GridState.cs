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
            }
        }

        public Difficulty DifficultyChosen { get; set; } = Difficulty.None;

        public int[,] RandomGrid(Difficulty difficulty)
        {
            int size = GetGridSize(difficulty);
            int[,] grid = InitializeGrid(size);
            int accessibleCells = CalculateAccessibleCells(size);

            Random random = new Random();
            int startX = random.Next(1, size - 1);
            int startY = random.Next(1, size - 1);
            grid[startX, startY] = 0;
            accessibleCells--;

            int currentX = startX;
            int currentY = startY;

            while (IsInsideGrid(currentX, currentY, size) && accessibleCells > 0)
            {
                int direction = random.Next(0, 4);
                if (TryMoveInDirection(direction, ref currentX, ref currentY, grid, ref accessibleCells))
                {
                    continue;
                }

                int additionalPaths = random.Next(0, 2);
                for (int i = 0; i < additionalPaths && accessibleCells > 0; i++)
                {
                    int randomDirection = random.Next(0, 4);
                    TryMoveInDirection(randomDirection, ref currentX, ref currentY, grid, ref accessibleCells);
                }
            }

            return grid;
        }

        private int GetGridSize(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => 6,
                Difficulty.Medium => 8,
                Difficulty.Hard => 10,
                _ => throw new InvalidOperationException("Difficulty not set"),
            };
        }

        private int[,] InitializeGrid(int size)
        {
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

        private int CalculateAccessibleCells(int size)
        {
            return (int)Math.Ceiling((size - 2) * (size - 2) * 0.4);
        }

        private bool IsInsideGrid(int x, int y, int size)
        {
            return x != 0 && x != size - 1 && y != 0 && y != size - 1;
        }

        private bool TryMoveInDirection(int direction, ref int x, ref int y, int[,] grid, ref int accessibleCells)
        {
            switch (direction)
            {
                case 0: // Up
                    if (x - 2 >= 0 && grid[x - 2, y] == 1)
                    {
                        grid[x - 1, y] = 0;
                        grid[x - 2, y] = 0;
                        x -= 2;
                        accessibleCells--;
                        return true;
                    }
                    break;
                case 1: // Right
                    if (y + 2 < grid.GetLength(1) && grid[x, y + 2] == 1)
                    {
                        grid[x, y + 1] = 0;
                        grid[x, y + 2] = 0;
                        y += 2;
                        accessibleCells--;
                        return true;
                    }
                    break;
                case 2: // Down
                    if (x + 2 < grid.GetLength(0) && grid[x + 2, y] == 1)
                    {
                        grid[x + 1, y] = 0;
                        grid[x + 2, y] = 0;
                        x += 2;
                        accessibleCells--;
                        return true;
                    }
                    break;
                case 3: // Left
                    if (y - 2 >= 0 && grid[x, y - 2] == 1)
                    {
                        grid[x, y - 1] = 0;
                        grid[x, y - 2] = 0;
                        y -= 2;
                        accessibleCells--;
                        return true;
                    }
                    break;
            }
            return false;
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