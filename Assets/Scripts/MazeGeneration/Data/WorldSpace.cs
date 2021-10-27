using System.Collections;
using System.Collections.Generic;

class WorldSpace
{
    public enum Type
    {
        wall = 0,
        floor = 1,
        path = 2,
    }

    public Type type;

    public WorldSpace(Type type)
    {
        this.type = type;
    }
}