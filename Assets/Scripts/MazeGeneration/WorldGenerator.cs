using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class WorldGenerator
{
    private static System.Random rand = new System.Random();

    // Size of world (mazeSize * 2 + 1)
    private int xLength;
    private int zLength;

    private WorldSpace[,] world;
    private MazeSpec mazeSpec;
    private MazeCell[,] maze;
    private List<Position> exitList;

    public WorldSpace[,] GenerateWorld(MazeSpec mazeSpec)
    {
        this.mazeSpec = mazeSpec;

        // Set World Size
        this.xLength = ConvertToWorldCoord(mazeSpec.mazeXLength);
        this.zLength = ConvertToWorldCoord(mazeSpec.mazeZLength);

        // Create Empty World
        world = new WorldSpace[xLength, zLength];

        for (int x = 0; x < xLength; x++)
        {
            for (int z = 0; z < zLength; z++)
            {
                world[x, z] = new WorldSpace(WorldSpace.Type.floor);
            }
        }

        // Validate MazeSpec
        if (mazeSpec.mazeStartX > xLength)
        {
            throw new ArgumentException("Maze StartX is out of bounds: " + mazeSpec.mazeStartX);
        }
        if (mazeSpec.mazeStartZ > zLength)
        {
            throw new ArgumentException("Maze StartZ is out of bounds: " + mazeSpec.mazeStartZ);
        }
        if (mazeSpec.mazeStartX + mazeSpec.mazeXLength > xLength)
        {
            throw new ArgumentException("Maze X Length is out of bounds: " + mazeSpec.mazeXLength);
        }
        if (mazeSpec.mazeStartZ + mazeSpec.mazeZLength > zLength)
        {
            throw new ArgumentException("Maze Z Length is out of bounds: " + mazeSpec.mazeZLength);
        }

        // Generate Maze
        MazeGenData data = MazeGenerator.Generate(mazeSpec);
        maze = data.maze;
        exitList = data.exitList;

        // Add Maze to the World
        ApplyMazeToWorld(world, mazeSpec, maze);

        return world;
    }

    public WorldSpace[,] RebuildWorld()
    {
        ApplyMazeToWorld(world, mazeSpec, maze);
        return world;
    }

    public void TryAddPath(Position mazePos)
    {
        if (exitList.Count != 2)
        {
            throw new Exception("Wrong number of exits");
        }
        Position exit1 = exitList[0];
        Position exit2 = exitList[1];

        int mazeX = mazePos.x;
        int mazeY = mazePos.y;

        if (mazeX >= 0 && mazeX < mazeSpec.mazeXLength && mazeY >= 0 && mazeY < mazeSpec.mazeZLength)
        {
            // Try starting from first exit
            List<MazeCell> path1 = DFS.FindPath(maze,
                exit1,
                new Position
                {
                    x = mazeX,
                    y = mazeY,
                }, mazeSpec.mazeXLength, mazeSpec.mazeZLength);

            // Try starting from second exit
            List<MazeCell> path2 = DFS.FindPath(maze,
             exit2,
             new Position
             {
                 x = mazeX,
                 y = mazeY,
             }, mazeSpec.mazeXLength, mazeSpec.mazeZLength);

            // Decide which path to use
            List<MazeCell> path;
            if (path1 == null && path2 == null)
            {
                // If no path found, stop
                return;
            }
            else if (path1 == null)
            {
                path = path2;
            }
            else if (path2 == null)
            {
                path = path1;
            }
            else if (path1.Count >= path2.Count)
            {
                path = path1;
            }
            else
            {
                path = path2;
            }

            // Set 'onPath' to true for the nodes in the path
            foreach (MazeCell cell in path)
            {
                cell.onPath = true;
            }
        }
    }

    public bool IsMazeSolved()
    {
        if (exitList.Count != 2)
        {
            throw new Exception("Wrong number of exits");
        }
        Position exit1 = exitList[0];
        Position exit2 = exitList[1];

        List<MazeCell> path = DFS.FindPath(maze, exit1, exit2, mazeSpec.mazeXLength, mazeSpec.mazeZLength);

        if (path != null)
        {
            // If we found a path, reset all the nodes to have 'onPath' false
            for (int x = 0; x < mazeSpec.mazeXLength; x++)
            {
                for (int y = 0; y < mazeSpec.mazeZLength; y++)
                {
                    maze[x, y].onPath = false;
                }
            }

            // Set 'onPath' to true for the nodes in the path
            foreach (MazeCell cell in path)
            {
                cell.onPath = true;
            }
        }

        return path != null;
    }

    public Vector3 GetRandomPosition(MazeSpec mazeSpec)
    {
        int xPos = ConvertToWorldCoord(mazeSpec.mazeStartX + rand.Next(mazeSpec.mazeXLength));
        int zPos = ConvertToWorldCoord(mazeSpec.mazeStartZ + rand.Next(mazeSpec.mazeZLength));

        return new Vector3(xPos, 0, zPos);
    }

    private void ApplyMazeToWorld(WorldSpace[,] world, MazeSpec mazeSpec, MazeCell[,] maze)
    {
        int worldStartX = ConvertToWorldCoord(mazeSpec.mazeStartX) - 1;
        int worldStartZ = ConvertToWorldCoord(mazeSpec.mazeStartZ) - 1;
        int worldEndX = ConvertToWorldCoord(mazeSpec.mazeStartX + mazeSpec.mazeXLength);
        int worldEndZ = ConvertToWorldCoord(mazeSpec.mazeStartZ + mazeSpec.mazeZLength);

        // Fill the maze area with walls
        for (int x = worldStartX; x < worldEndX; x++)
        {
            for (int z = worldStartZ; z < worldEndZ; z++)
            {
                world[x, z] = new WorldSpace(WorldSpace.Type.wall);
            }
        }

        for (int mazeX = 0; mazeX < mazeSpec.mazeXLength; mazeX++)
        {
            for (int mazeZ = 0; mazeZ < mazeSpec.mazeZLength; mazeZ++)
            {
                var cell = maze[mazeX, mazeZ];
                int worldX = worldStartX + ConvertToWorldCoord(mazeX);
                int worldZ = worldStartZ + ConvertToWorldCoord(mazeZ);

                // Set cell's coordinates to floor type
                world[worldX, worldZ].type = FloorOrPath(cell);

                // Knock down walls
                if (!cell.isWallUp(Wall.UP))
                {
                    int wallX = worldX;
                    int wallZ = worldZ + 1;
                    world[wallX, wallZ].type = FloorOrPath(cell);
                }

                if (!cell.isWallUp(Wall.LEFT))
                {
                    int wallX = worldX - 1;
                    int wallZ = worldZ;
                    world[wallX, wallZ].type = FloorOrPath(cell);
                }

                if (!cell.isWallUp(Wall.RIGHT))
                {
                    int wallX = worldX + 1;
                    int wallZ = worldZ;
                    world[wallX, wallZ].type = FloorOrPath(cell);
                }

                if (!cell.isWallUp(Wall.DOWN))
                {
                    int wallX = worldX;
                    int wallZ = worldZ - 1;
                    world[wallX, wallZ].type = FloorOrPath(cell);
                }

                if (!cell.isWallUp(Wall.UP_LEFT_CORNER))
                {
                    int wallX = worldX - 1;
                    int wallZ = worldZ + 1;
                    world[wallX, wallZ].type = FloorOrPath(cell);
                }
            }
        }
    }

    private WorldSpace.Type FloorOrPath(MazeCell cell)
    {
        if (cell.onPath)
        {
            return WorldSpace.Type.path;
        }
        return WorldSpace.Type.floor;
    }

    public int GetXLength()
    {
        return xLength;
    }

    public int GetZLength()
    {
        return zLength;
    }

    public int ConvertToWorldCoord(float coord)
    {
        return Mathf.RoundToInt(coord * 2 + 1);
    }

    public int ConvertToMazeCoord(float coord)
    {
        return Mathf.RoundToInt((coord - 1) / 2.0f);
    }
}