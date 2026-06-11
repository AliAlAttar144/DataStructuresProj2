namespace Model
{
    public class DijkstraPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Dijkstra;
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

            var distances = new int[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    distances[r, c] = int.MaxValue;
                }
            }

            var parent = new Dictionary<(int row, int col), (int row, int col)>();
            var processed = new bool[rows, cols];
            var pq = new PriorityQueue<(int row, int col), int>();

            distances[start.row, start.col] = 0;
            pq.Enqueue(start, 0);

            while (pq.Count > 0)
            {
                var current = pq.Dequeue();
                if (processed[current.row, current.col])
                {
                    continue;
                }

                processed[current.row, current.col] = true;
                if (current == end)
                {
                    break;
                }

                int moveIndex = 0;
                foreach (var move in maze.moves)
                {
                    int nextRow = current.row + move[0];
                    int nextCol = current.col + move[1];

                    if (!maze.IsValidMove(nextRow, nextCol) || processed[nextRow, nextCol])
                    {
                        continue;
                    }

                    int candidate = distances[current.row, current.col] + 1;
                    if (candidate < distances[nextRow, nextCol])
                    {
                        distances[nextRow, nextCol] = candidate;
                        parent[(nextRow, nextCol)] = current;
                        int priority = candidate * 10 + moveIndex;
                        pq.Enqueue((nextRow, nextCol), priority);
                    }

                    moveIndex++;
                }
            }

            if (distances[end.row, end.col] == int.MaxValue)
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
   }
}
