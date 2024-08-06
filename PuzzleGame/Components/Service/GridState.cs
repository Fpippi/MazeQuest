namespace MazeGame.Services
{
    // This class represents the state of the grid in the maze game.
    // It provides methods to generate a random grid based on the specified difficulty level.
    // The grid is represented as a 2D array of integers, where 0 represents an accessible cell and 1 represents a wall.
    // The GenerateRandomGrid method uses a randomized depth-first search algorithm to generate the maze.
    // The algorithm starts from a random cell and repeatedly chooses a random accessible neighbor until a certain number of accessible cells is reached.
    // If the generated maze does not have a path to the border, the algorithm will force a path to ensure accessibility.
    // The size of the grid is determined by the difficulty level, with easy, medium, and hard difficulties corresponding to grid sizes of 10x10, 15x15, and 20x20 respectively.
    // The GridState class also includes helper methods to initialize the grid, get accessible neighbors, force a path to the border, and create a path to the border.
    // The DifficultyChosen property stores the currently chosen difficulty level.
    public class GridState
    {
        private int[,] _grid;
        public int[,] Grid
        {
            get => _grid;
            set => _grid = value;
        }

        public Difficulty DifficultyChosen { get; set; } = Difficulty.None;

        // Generates a random grid based on the specified difficulty level.
        public int[,] GenerateRandomGrid(Difficulty difficulty)
        {
            try
            {
                // Get the size of the grid based on the difficulty level.
                int size = GetGridSize(difficulty);

                // Initialize the grid with walls.
                int[,] grid = InitializeGrid(size);

                // Calculate the total number of cells in the grid (excluding the border).
                int totalCells = (size - 2) * (size - 2);

                // Calculate the target number of accessible cells based on a percentage of the total cells.
                int accessibleCellsTarget = (int)Math.Ceiling(totalCells * 0.3);

                // Create a random number generator.
                Random random = new Random();

                // Create a stack to store the visited cells during the generation process.
                Stack<(int x, int y)> stack = new Stack<(int x, int y)>();

                // Choose a random starting cell.
                int startX = random.Next(1, size - 1);
                int startY = random.Next(1, size - 1);
                grid[startX, startY] = 0;

                // Keep track of the number of accessible cells generated.
                int accessibleCells = 1;

                // Push the starting cell onto the stack.
                stack.Push((startX, startY));

                // Flag to indicate if the generation process has reached the border.
                bool reachedBorder = false;

                // Perform a randomized depth-first search to generate the maze.
                while (stack.Count > 0 && accessibleCells < accessibleCellsTarget)
                {
                    // Get the current cell from the top of the stack.
                    var (currentX, currentY) = stack.Peek();

                    // Get the accessible neighbors of the current cell.
                    var neighbors = GetAccessibleNeighbors(currentX, currentY, size, grid);

                    if (neighbors.Count > 0)
                    {
                        // Choose a random accessible neighbor.
                        var (nextX, nextY) = neighbors[random.Next(neighbors.Count)];

                        // Remove the wall between the current cell and the chosen neighbor.
                        grid[nextX, nextY] = 0;
                        grid[(currentX + nextX) / 2, (currentY + nextY) / 2] = 0;

                        // Push the chosen neighbor onto the stack.
                        stack.Push((nextX, nextY));

                        // Increment the number of accessible cells.
                        accessibleCells++;

                        // Check if the chosen neighbor is on the border.
                        if (nextX == 1 || nextX == size - 2 || nextY == 1 || nextY == size - 2)
                        {
                            reachedBorder = true;
                        }
                    }
                    else
                    {
                        // If there are no accessible neighbors, backtrack by popping the current cell from the stack.
                        stack.Pop();
                    }
                }

                // If the generation process did not reach the border, force a path to ensure accessibility.
                if (!reachedBorder)
                {
                    var (finalX, finalY) = stack.Peek();
                    ForcePathToBorder(finalX, finalY, size, grid, ref accessibleCells);
                }

                // Fill the remaining cells with random accessible cells until the target number is reached.
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

                // Ensure that at least one border cell is accessible.
                EnsureAccessibleBorderCell(size, grid);

                // Return the generated grid.
                return grid;
            }
            catch (Exception ex)
            {
                // Handle the exception and retry generating the grid.
                return GenerateRandomGrid(difficulty);
            }
        }

        // Helper method to get the size of the grid based on the difficulty level.
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

        // Helper method to initialize the grid with walls.
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

        // Helper method to get the accessible neighbors of a cell.
        private List<(int x, int y)> GetAccessibleNeighbors(int x, int y, int size, int[,] grid)
        {
            var neighbors = new List<(int, int)>();

            if (x - 2 > 0 && grid[x - 2, y] == 1)
                neighbors.Add((x - 2, y));

            if (x + 2 < size - 1 && grid[x + 2, y] == 1)
                neighbors.Add((x + 2, y));

            if (y - 2 > 0 && grid[x, y - 2] == 1)
                neighbors.Add((x, y - 2));

            if (y + 2 < size - 1 && grid[x, y + 2] == 1)
                neighbors.Add((x, y + 2));

            return neighbors;
        }

        // Helper method to force a path to the border from a given cell.
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

                if (validDirections.Count == 0)
                    break;

                var (nextX, nextY) = validDirections[random.Next(validDirections.Count)];

                grid[nextX, nextY] = 0;
                grid[(x + nextX) / 2, (y + nextY) / 2] = 0;
                x = nextX;
                y = nextY;
                accessibleCells++;
            }
        }

        // Helper method to check if a cell is inside the grid.
        private bool IsInsideGrid(int x, int y, int size)
        {
            return x > 0 && x < size - 1 && y > 0 && y < size - 1;
        }

        // Helper method to ensure that at least one border cell is accessible.
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

            // Ensure that at least one adjacent cell is accessible.
            var (adjacentX, adjacentY) = GetAccessibleAdjacentCell(size, grid);
            grid[adjacentX, adjacentY] = 0;
            CreatePathToBorder(adjacentX, adjacentY, size, grid);
        }

        // Helper method to create a path to the border from a given cell.
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



        // Helper method to get a random accessible adjacent cell.
        private (int x, int y) GetAccessibleAdjacentCell(int size, int[,] grid)
        {
            Random random = new Random();
            List<(int x, int y)> adjacentCells = new List<(int x, int y)>();

            for (int i = 1; i < size - 1; i++)
            {
                if (grid[1, i] == 0)
                {
                    adjacentCells.Add((0, i));
                }
                if (grid[size - 2, i] == 0)
                {
                    adjacentCells.Add((size - 1, i));
                }
                if (grid[i, 1] == 0)
                {
                    adjacentCells.Add((i, 0));
                }
                if (grid[i, size - 2] == 0)
                {
                    adjacentCells.Add((i, size - 1));
                }
            }

            return adjacentCells[random.Next(adjacentCells.Count)];
        }
    }

    // Enumeration representing the difficulty levels.
    public enum Difficulty
    {
        None,
        Easy,
        Medium,
        Hard
    }

}
