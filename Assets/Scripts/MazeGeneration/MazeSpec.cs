using System;
using UnityEngine;

[System.Serializable]
public class MazeSpec {
    public string algorithm = "";

    public int mazeStartX = 0;
    public int mazeStartZ = 0;
    
    public int mazeXLength = 50;
    public int mazeZLength = 50;

    public int courtyardSize = 0;
    public int courtyardOpenings = 4;

    public int numExits = 0;

    public MazeSpec(string algorithm, int mazeStartX, int mazeStartZ, int mazeXLength, int mazeZLength, int courtyardSize=0, int courtyardOpenings=4, int numExits=0) {
        this.algorithm = algorithm;
        this.mazeStartX = mazeStartX;
        this.mazeStartZ = mazeStartZ;
        this.mazeXLength = mazeXLength;
        this.mazeZLength = mazeZLength;
        this.courtyardSize = courtyardSize;
        this.courtyardOpenings = courtyardOpenings;
        this.numExits = numExits;
    }
}