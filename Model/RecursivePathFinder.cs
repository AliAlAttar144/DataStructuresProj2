
namespace Model
{
    public class RecursivePathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Recursive;
        public PathFinderType algType { get => _algType; set {} }

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            if (maze?.MazeArray == null || pos == null || pos.Length != 2 || visitedPositions == null)
            {
                return;
            }

            visitedPositions.Clear();

            int rows = maze.MazeArray.Length;
            int cols = maze.MazeArray[0].Length;
            var visited = new bool[rows, cols];

            Dfs(maze, pos[0], pos[1], visited, visitedPositions);
        }

        private static bool Dfs(Maze maze, int row, int col, bool[,] visited, Queue<int[]> visitedPositions)
        {
            if (!maze.IsValidMove(row, col))
            {
                return false;
            }

            if (visited[row, col])
            {
                return false;
            }

            visited[row, col] = true;
            visitedPositions.Enqueue([row, col]);

            if (row == maze.End[0] && col == maze.End[1])
            {
                return true;
            }

            foreach (var move in GetBiasedMoves(maze.moves))
            {
                int nextRow = row + move[0];
                int nextCol = col + move[1];
                if (Dfs(maze, nextRow, nextCol, visited, visitedPositions))
                {
                    return true;
                }
            }

            return false;
        }

        private static IEnumerable<int[]> GetBiasedMoves(int[][] moves)
        {
            return moves
                .Select(move => new
                {
                    Move = move,
                    Score = Random.Shared.NextDouble()
                        + (move[0] == 1 ? 0.35 : 0.0) 
                        + (move[1] == 1 ? 0.35 : 0.0) 
                })
                .OrderByDescending(x => x.Score)
                .Select(x => x.Move);
        }
    }
}
