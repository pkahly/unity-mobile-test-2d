using System.Collections;
using System.Collections.Generic;
using System;

public enum Wall {
    LEFT = 0,
    RIGHT = 1,
    UP = 2,
    DOWN = 3,

    UP_LEFT_CORNER = 4,
}

public enum CellType {
    MAZE = 0,
    COURTYARD = 1,
}

public class MazeCell {
    private Dictionary<Wall, bool> walls = new Dictionary<Wall, bool>() {
        { Wall.LEFT, true },
        { Wall.RIGHT, true },
        { Wall.UP, true },
        { Wall.DOWN, true },

        { Wall.UP_LEFT_CORNER, true },
    };

    private bool visited = false;
    private CellType type = CellType.MAZE;

    public bool isWallUp(Wall wall) {
        return walls[wall];
    }

    public void removeWall(Wall wall) {
        walls[wall] = false;
    }

    public void removeWalls(Wall[] wallsToRemove) {
        foreach (Wall wall in wallsToRemove) {
            walls[wall] = false;
        }
    }

    public bool isVisited() {
        return visited;
    }

    public void markVisited() {
        visited = true;
    }

    public static Wall GetOppositeWall(Wall wall) {
        switch (wall)
        {
            case Wall.RIGHT: return Wall.LEFT;
            case Wall.LEFT: return Wall.RIGHT;
            case Wall.UP: return Wall.DOWN;
            case Wall.DOWN: return Wall.UP;
        }

        throw new ArgumentException("No such wall: " + wall);
    }

    public CellType getType() {
        return type;
    }

    public void setType(CellType type) {
        this.type = type;
    }
}