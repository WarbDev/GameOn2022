using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate List<Location> ShapeWithRadius(Location start, int radius);

public delegate List<Location> ShapeWithRadiusAndDirection(Location start, int radius, (int, int) direction);