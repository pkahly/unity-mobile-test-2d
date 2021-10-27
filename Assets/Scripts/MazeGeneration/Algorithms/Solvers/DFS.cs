using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DFS
{
    public static List<MazeCell> FindPath(MazeCell[,] maze, Position start, Position end, int width, int height)
    {
        return FindPathRecursive(maze, start, start, end, width, height);
    }

    private static List<MazeCell> FindPathRecursive(MazeCell[,] maze, Position last, Position current, Position end, int width, int height)
    {
        // Base case
        if (current.x == end.x && current.y == end.y)
        {
            List<MazeCell> path = new List<MazeCell>();
            path.Add(maze[current.x, current.y]);
            return path;
        }

        // Require all the cells in the path to have 'onPath' set to true
        // (Except for the end node)
        if (!maze[current.x, current.y].onPath)
        {
            return null;
        }

        // Recursive case
        List<Position> children = GetOpenNeighbors(maze[current.x, current.y], current, width, height);
        foreach (Position child in children)
        {
            // Skip the previous node
            if (child.x == last.x && child.y == last.y)
            {
                continue;
            }

            // Recursive call
            List<MazeCell> path = FindPathRecursive(maze, current, child, end, width, height);
            if (path != null)
            {
                // Add current cell to the beginning of the list and return
                path.Insert(0, maze[current.x, current.y]);
                return path;
            }
        }

        // No path found
        return null;
    }

    private static List<Position> GetOpenNeighbors(MazeCell cell, Position p, int width, int height)
    {
        List<Position> list = new List<Position>();

        // Left
        if (p.x > 0)
        {
            if (!cell.isWallUp(Wall.LEFT))
            {
                list.Add(new Position
                {
                    x = p.x - 1,
                    y = p.y
                });
            }
        }

        // DOWN
        if (p.y > 0)
        {
            if (!cell.isWallUp(Wall.DOWN))
            {
                list.Add(new Position
                {
                    x = p.x,
                    y = p.y - 1
                });
            }
        }

        // UP
        if (p.y < height - 1)
        {
            if (!cell.isWallUp(Wall.UP))
            {
                list.Add(new Position
                {
                    x = p.x,
                    y = p.y + 1
                });
            }
        }

        // RIGHT
        if (p.x < width - 1)
        {
            if (!cell.isWallUp(Wall.RIGHT))
            {
                list.Add(new Position
                {
                    x = p.x + 1,
                    y = p.y
                });
            }
        }

        return list;
    }
}
