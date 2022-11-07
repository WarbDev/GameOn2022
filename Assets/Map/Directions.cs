using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Directions
{
    public static (int, int) N = (0, 1);
    public static (int, int) NE = (1, 1);
    public static (int, int) E = (1, 0);
    public static (int, int) SE = (1, -1);
    public static (int, int) S = (0, -1);
    public static (int, int) SW = (-1, -1);
    public static (int, int) W = (-1, 0);
    public static (int, int) NW = (-1, 1);

    public static List<(int, int)> All = new List<(int, int)> { N, NE, E, SE, S, SW, W, NW };
}