using System;
using System.Collections;
using System.Collections.Generic;

/*
- Push a starting cell onto the stack
- While the stack is not empty
- Pop a cell from the stack
- If there are unvisited neighbours
- Pick a random neighbour and visit it
- Push the neighbour onto the stack

- If there are no unvisited neighbours
- Nothing will be pushed and the next iteration will backtrack to the previous node.
*/
class RecursiveBacktracker : Algorithm {
    public override MazeCell[,] Generate(MazeCell[,] maze, int width, int height) {
        var positionStack = new Stack<Position>();

        // Start at 0,0
        var position = new Position { x = 0, y = 0 };
        maze[position.x, position.y].markVisited();
        positionStack.Push(position);

        while (positionStack.Count > 0) {
            // Move to the next position and get neighbours
            var current = positionStack.Pop();
            var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

            // Visit a random neighbour
            if (neighbours.Count > 0) {
                positionStack.Push(current);

                var randIndex = rand.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randIndex];

                VisitCell(maze, current, randomNeighbour);

                positionStack.Push(randomNeighbour.position);
            }
        }

        return maze;
    }
}