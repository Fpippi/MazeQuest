namespace MazeGame.Services
{
    public class GridState
    {
        private int[,] _grid;
        public int[,] Grid
        {
            get => _grid;
            set => _grid = value;
        }

        public Difficulty DifficultyChosen { get; set; } = Difficulty.None;

        public int[,] GenerateRandomGrid(Difficulty difficulty)
        {
            int size = GetGridSize(difficulty);
            int[,] grid = InitializeGrid(size);
            int totalCells = (size - 2) * (size - 2);
            int accessibleCellsTarget = (int)Math.Ceiling(totalCells * 0.3);

            Random random = new Random();
            Stack<(int x, int y)> stack = new Stack<(int x, int y)>();

            int startX = random.Next(1, size - 1);
            int startY = random.Next(1, size - 1);
            grid[startX, startY] = 0;
            int accessibleCells = 1;

            stack.Push((startX, startY));

            bool reachedBorder = false;

            while (stack.Count > 0 && accessibleCells < accessibleCellsTarget)
            {
                var (currentX, currentY) = stack.Peek();
                var neighbors = GetAccessibleNeighbors(currentX, currentY, size, grid);

                if (neighbors.Count > 0)
                {
                    var (nextX, nextY) = neighbors[random.Next(neighbors.Count)];
                    grid[nextX, nextY] = 0;
                    grid[(currentX + nextX) / 2, (currentY + nextY) / 2] = 0;
                    stack.Push((nextX, nextY));
                    accessibleCells++;

                    if (nextX == 1 || nextX == size - 2 || nextY == 1 || nextY == size - 2)
                    {
                        reachedBorder = true;
                    }
                }
                else
                {
                    stack.Pop();
                }
            }

            if (!reachedBorder)
            {
                var (finalX, finalY) = stack.Peek();
                ForcePathToBorder(finalX, finalY, size, grid, ref accessibleCells);
            }

            while (accessibleCells < accessibleCellsTarget)
            {
                int x = random.Next(1, size - 1);
                int y = random.Next(1, size - 1);

                if (grid[x, y] == 1)
                {
                    grid[x, y] = 0;
                    accessibleCells++;
                }
            }

            EnsureAccessibleBorderCell(size, grid);

            return grid;
        }

        private int GetGridSize(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => 10,
                Difficulty.Medium => 15,
                Difficulty.Hard => 20,
                _ => throw new InvalidOperationException("Invalid difficulty"),
            };
        }

        private int[,] InitializeGrid(int size)
        {
            int[,] grid = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = 1;
                }
            }
            for (int i = 0; i < size; i++)
            {
                grid[0, i] = 1;
                grid[size - 1, i] = 1;
                grid[i, 0] = 1;
                grid[i, size - 1] = 1;
            }
            return grid;
        }

        private List<(int x, int y)> GetAccessibleNeighbors(int x, int y, int size, int[,] grid)
        {
            var neighbors = new List<(int, int)>();
            if (x - 2 > 0 && grid[x - 2, y] == 1) neighbors.Add((x - 2, y));
            if (x + 2 < size - 1 && grid[x + 2, y] == 1) neighbors.Add((x + 2, y));
            if (y - 2 > 0 && grid[x, y - 2] == 1) neighbors.Add((x, y - 2));
            if (y + 2 < size - 1 && grid[x, y + 2] == 1) neighbors.Add((x, y + 2));
            return neighbors;
        }

        private void ForcePathToBorder(int x, int y, int size, int[,] grid, ref int accessibleCells)
        {
            Random random = new Random();

            while (!(x == 1 || x == size - 2 || y == 1 || y == size - 2))
            {
                var directions = new List<(int, int)>
                    {
                        (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)
                    };

                var validDirections = directions.Where(d => IsInsideGrid(d.Item1, d.Item2, size) && grid[d.Item1, d.Item2] == 1).ToList();

                if (validDirections.Count == 0) break;

                var (nextX, nextY) = validDirections[random.Next(validDirections.Count)];

                grid[nextX, nextY] = 0;
                grid[(x + nextX) / 2, (y + nextY) / 2] = 0;
                x = nextX;
                y = nextY;
                accessibleCells++;
            }
        }

        private bool IsInsideGrid(int x, int y, int size)
        {
            return x > 0 && x < size - 1 && y > 0 && y < size - 1;
        }

        private void EnsureAccessibleBorderCell(int size, int[,] grid)
        {
            Random random = new Random();
            List<(int x, int y)> borderCells = new List<(int x, int y)>();

            for (int i = 1; i < size - 1; i++)
            {
                borderCells.Add((0, i));
                borderCells.Add((size - 1, i));
                borderCells.Add((i, 0));
                borderCells.Add((i, size - 1));
            }

            while (borderCells.Count > 0)
            {
                var (x, y) = borderCells[random.Next(borderCells.Count)];
                if (grid[x, y] == 1)
                {
                    grid[x, y] = 0;
                    CreatePathToBorder(x, y, size, grid);
                    break;
                }
                borderCells.Remove((x, y));
            }
        }

        private void CreatePathToBorder(int x, int y, int size, int[,] grid)
        {
            Random random = new Random();
            List<(int dx, int dy)> directions = new List<(int, int)>
                {
                    (1, 0), (-1, 0), (0, 1), (0, -1)
                };

            while (true)
            {
                var (dx, dy) = directions[random.Next(directions.Count)];
                int newX = x + dx;
                int newY = y + dy;

                if (IsInsideGrid(newX, newY, size) && grid[newX, newY] == 1)
                {
                    grid[newX, newY] = 0;
                    grid[(x + newX) / 2, (y + newY) / 2] = 0;
                    x = newX;
                    y = newY;
                }
                else if (!IsInsideGrid(newX, newY, size))
                {
                    break;
                }
            }
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
