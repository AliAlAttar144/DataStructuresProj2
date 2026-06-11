
namespace Model
{
    public class AStarPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Astar;
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
            var start = (row: pos[0], col: pos[1]);
            var end = (row: maze.End[0], col: maze.End[1]);

            var gScore = new int[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    gScore[r, c] = int.MaxValue;
                }
            }

            var parent = new Dictionary<(int row, int col), (int row, int col)>();
            var closed = new bool[rows, cols];
            var open = new PriorityQueue<(int row, int col), int>();

            gScore[start.row, start.col] = 0;
            open.Enqueue(start, Heuristic(start, end));

            while (open.Count > 0)
            {
                var current = open.Dequeue();
                if (closed[current.row, current.col])
                {
                    continue;
                }

                if (current == end)
                {
                    break;
                }

                closed[current.row, current.col] = true;

                int moveIndex = 0;
                foreach (var move in maze.moves)
                {
                    int nextRow = current.row + move[0];
                    int nextCol = current.col + move[1];

                    if (!maze.IsValidMove(nextRow, nextCol) || closed[nextRow, nextCol])
                    {
                        continue;
                    }

                    int tentativeG = gScore[current.row, current.col] + 1;
                    if (tentativeG < gScore[nextRow, nextCol])
                    {
                        gScore[nextRow, nextCol] = tentativeG;
                        parent[(nextRow, nextCol)] = current;
                        int fScore = tentativeG + Heuristic((nextRow, nextCol), end);
                        int priority = fScore * 10 + moveIndex;
                        open.Enqueue((nextRow, nextCol), priority);
                    }

                    moveIndex++;
                }
            }

            if (gScore[end.row, end.col] == int.MaxValue)
            {
                return;
            }

            var path = new Stack<int[]>();
            var step = end;
            path.Push([step.row, step.col]);

            while (step != start)
            {
                step = parent[step];
                path.Push([step.row, step.col]);
            }

            while (path.Count > 0)
            {
                visitedPositions.Enqueue(path.Pop());
            }
        }

        private static int Heuristic((int row, int col) a, (int row, int col) b)
        {
            return Math.Abs(a.row - b.row) + Math.Abs(a.col - b.col);
        }

    }
}

            

