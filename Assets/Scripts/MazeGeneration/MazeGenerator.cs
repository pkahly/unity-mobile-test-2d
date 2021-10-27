using System;

public static class MazeGenerator {
    public static MazeCell[,] Generate(MazeSpec mazeSpec) {
        int width = mazeSpec.mazeXLength;
        int height = mazeSpec.mazeZLength;

        // Create Maze with all walls intact
        MazeCell[,] maze = new MazeCell[width, height];

        for (int i = 0; i < width; ++i) {
            for (int j = 0; j < height; ++j) {
                maze[i, j] = new MazeCell();
            }
        }

        // Pick generation algorithm
        Algorithm algorithm = Algorithm.GetAlgorithm(mazeSpec.algorithm);

        // Add a courtyard by marking the nodes visited and remove the interior walls
        algorithm.AddCourtyard(maze, width, height, mazeSpec.courtyardSize, mazeSpec.courtyardOpenings);
        
        // Generate maze
        maze = algorithm.Generate(maze, width, height);

        // Add exit(s)
        algorithm.AddExits(maze, mazeSpec.numExits, width, height);
        
        return maze;
    }
}