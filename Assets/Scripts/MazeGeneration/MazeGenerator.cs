using System;
using System.Collections.Generic;

public struct MazeGenData
{
    public MazeCell[,] maze;
    public List<Position> exitList;
}

public static class MazeGenerator
{
    public static MazeGenData Generate(MazeSpec mazeSpec)
    {
        int width = mazeSpec.mazeXLength;
        int height = mazeSpec.mazeZLength;

        // Create Maze with all walls intact
        MazeGenData data = new MazeGenData();
        data.maze = new MazeCell[width, height];

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                data.maze[i, j] = new MazeCell();
            }
        }

        // Pick generation algorithm
        Algorithm algorithm = Algorithm.GetAlgorithm(mazeSpec.algorithm);

        // Add a courtyard by marking the nodes visited and remove the interior walls
        algorithm.AddCourtyard(data.maze, width, height, mazeSpec.courtyardSize, mazeSpec.courtyardOpenings);

        // Generate maze
        data.maze = algorithm.Generate(data.maze, width, height);

        // Add exit(s)
        data.exitList = algorithm.AddExits(data.maze, mazeSpec.numExits, width, height);

        // TODO Save exit list

        return data;
    }
}