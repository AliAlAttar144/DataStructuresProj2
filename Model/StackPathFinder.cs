
namespace Model
{
    public class StackPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Stack;
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
            var stack = new Stack<(int row, int col)>();

            var start = (row: pos[0], col: pos[1]);
            var end = (row: maze.End[0], col: maze.End[1]);

            stack.Push(start);
            visited[start.row, start.col] = true;

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                visitedPositions.Enqueue([current.row, current.col]);

                if (current == end)
                {
                    break;
                }

                // Stack is LIFO: iterate moves in reverse so pop order follows maze.moves bias.
                foreach (var move in maze.moves.Reverse())
                {
                    int nextRow = current.row + move[0];
                    int nextCol = current.col + move[1];

                    if (!maze.IsValidMove(nextRow, nextCol) || visited[nextRow, nextCol])
                    {
                        continue;
                    }

                    visited[nextRow, nextCol] = true;
                    stack.Push((nextRow, nextCol));
                }
            }
        }       
    }
}

            

