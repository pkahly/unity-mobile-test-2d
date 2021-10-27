using System;
using System.Collections;
using System.Collections.Generic;

public struct Position
{
    public int x;
    public int y;
}

public struct Neighbour
{
    public Position position;
    public Wall sharedWall;
}

abstract class Algorithm {
    protected static System.Random rand = new System.Random();

    public static Algorithm GetAlgorithm(string name) {
        if (name == "RecursiveBacktracker" || name == "DFS") {
            return new RecursiveBacktracker();
        } else if (name == "Prims") {
            return new Prims();
        }

        throw new System.Exception("No such algorithm: " + name);
    }

    // Apply a maze generation algorithm
    // Will be implememented by subclasses
    public abstract MazeCell[,] Generate(MazeCell[,] maze, int width, int height);

    // Add exits to the edges of the maze
    public void AddExits(MazeCell[,] maze, int numExits, int width, int height) {
        for (int i = 0; i < numExits; i++) {
            int randX = rand.Next(0, width);
            int randY = rand.Next(0, height);

            Wall wall = (Wall)rand.Next(0, 4); // Walls are 0-3
            if (wall == Wall.UP) {
                maze[randX, height - 1].removeWall(wall);
            } else if (wall == Wall.DOWN) {
                maze[randX, 0].removeWall(wall);
            } else if (wall == Wall.RIGHT) {
                maze[width - 1, randY].removeWall(wall);
            } else { // wall == Wall.LEFT
                maze[0, randY].removeWall(wall);
            }
        }
    }

    // Add an open area to the maze
    public void AddCourtyard(MazeCell[,] maze, int width, int height, int courtyardSize, int numOpenings) {
        if (courtyardSize > 0) {
            // Place the courtyard at the maze's center
            int xStart = (width / 2) - (courtyardSize / 2);
            int xEnd = xStart + courtyardSize;
            int yStart = (height / 2) - (courtyardSize / 2);
            int yEnd = yStart + courtyardSize;

            for (int i = xStart; i <= xEnd; i++) {
                for (int j = yStart; j <= yEnd; j++) {
                    // Mark the Cell visited so the maze generation will ignore it
                    maze[i,j].markVisited();
                    maze[i,j].setType(CellType.COURTYARD);

                    // Remove all of the interior walls, but leave the outside ones intact
                    if (i != xStart) {
                        maze[i,j].removeWall(Wall.LEFT);
                    } 
                    
                    if (i != xEnd) {
                        maze[i,j].removeWall(Wall.RIGHT);
                    }
                        
                    if (j != yStart) {
                        maze[i,j].removeWall(Wall.DOWN);
                    } 
                    
                    if (j != yEnd ) {
                        maze[i,j].removeWall(Wall.UP);
                    }

                    // Remove corner walls
                    if (i != xStart && j != yEnd) {
                        maze[i,j].removeWall(Wall.UP_LEFT_CORNER);
                    }
                }
            }

            // Knock down a random exterior wall on each side
            // Stop when we reach the desired number of openings
            if (numOpenings < 1) {return;}
            maze[xStart, rand.Next(yStart, yEnd)].removeWall(Wall.LEFT);

            if (numOpenings < 2) {return;}
            maze[xEnd, rand.Next(yStart, yEnd)].removeWall(Wall.RIGHT);

            if (numOpenings < 3) {return;}
            maze[rand.Next(xStart, xEnd), yStart].removeWall(Wall.DOWN);

            if (numOpenings < 4) {return;}
            maze[rand.Next(xStart, xEnd), yEnd].removeWall(Wall.UP);
        }
    }

    protected List<Neighbour> GetUnvisitedNeighbours(Position p, MazeCell[,] maze, int width, int height) {
        var list = new List<Neighbour>();

        // Left
        if (p.x > 0) {
            if (!maze[p.x - 1, p.y].isVisited()) {
                list.Add(new Neighbour
                {
                    position = new Position
                    {
                        x = p.x - 1,
                        y = p.y
                    },
                    sharedWall = Wall.LEFT
                });
            }
        }

        // DOWN
        if (p.y > 0) {
            if (!maze[p.x, p.y - 1].isVisited()) {
                list.Add(new Neighbour
                {
                    position = new Position
                    {
                        x = p.x,
                        y = p.y - 1
                    },
                    sharedWall = Wall.DOWN
                });
            }
        }

        // UP
        if (p.y < height - 1) {
            if (!maze[p.x, p.y + 1].isVisited()) {
                list.Add(new Neighbour
                {
                    position = new Position
                    {
                        x = p.x,
                        y = p.y + 1
                    },
                    sharedWall = Wall.UP
                });
            }
        }

        // RIGHT
        if (p.x < width - 1) {
            if (!maze[p.x + 1, p.y].isVisited()) {
                list.Add(new Neighbour
                {
                    position = new Position
                    {
                        x = p.x + 1,
                        y = p.y
                    },
                    sharedWall = Wall.RIGHT
                });
            }
        }

        return list;
    }

    // Visit a new cell
    protected void VisitCell(MazeCell[,] maze, Position oldPosition, Neighbour neighbour) {
        var newPosition = neighbour.position;

        maze[oldPosition.x, oldPosition.y].removeWall(neighbour.sharedWall);
        maze[newPosition.x, newPosition.y].removeWall(MazeCell.GetOppositeWall(neighbour.sharedWall));
        maze[newPosition.x, newPosition.y].markVisited();
    }
}