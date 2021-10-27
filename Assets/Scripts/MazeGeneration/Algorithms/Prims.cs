using System;
using System.Collections;
using System.Collections.Generic;

public struct UnvisitedCell
{
    public Position visitedPosition;
    public Neighbour neighbour;
}

/*
- Pick a starting cell
- Add the unvisited neighbors to a list
- While there are unvisited cells
- Pick a random unvisited neighbor from the list
- Visit the cell and add it's unvisited neighbors to the list
*/
class Prims : Algorithm {
    public override MazeCell[,] Generate(MazeCell[,] maze, int width, int height) {
        var unvisitedCells = new List<UnvisitedCell>();

        // Start at 0,0
        var currentPosition = new Position { x = 0, y = 0 };
        maze[0, 0].markVisited();
        AddNeighbours(unvisitedCells, currentPosition, maze, width, height);
        
        while (unvisitedCells.Count > 0) {
            // Pick a random cell
            var randIndex = rand.Next(0, unvisitedCells.Count);
            UnvisitedCell unvisitedCell = unvisitedCells[randIndex];
            unvisitedCells.RemoveAt(randIndex);
            Position newCellPosition = unvisitedCell.neighbour.position;

            // Verify that it hasn't been visited from a different neighboring cell
            if (maze[newCellPosition.x, newCellPosition.y].isVisited()) {
                continue;
            }

            // Visit the new cell
            VisitCell(maze, unvisitedCell.visitedPosition, unvisitedCell.neighbour);

            // Add the new cell's neighbours to the list
            AddNeighbours(unvisitedCells, newCellPosition, maze, width, height);
        }

        return maze;
    }

    // Add the unvisited neighbours of the current cell to the list
    // Includes the current cell's position so they can be connected later
    private void AddNeighbours(List<UnvisitedCell> unvisitedCells, Position currentPosition, MazeCell[,] maze, int width, int height) {
        var neighbours = GetUnvisitedNeighbours(currentPosition, maze, width, height);

        foreach (Neighbour neighbour in neighbours) {
            UnvisitedCell unvisitedCell;
            unvisitedCell.visitedPosition = currentPosition;
            unvisitedCell.neighbour = neighbour;

            unvisitedCells.Add(unvisitedCell);
        }
    }
}