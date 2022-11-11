using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtility
{
    //Takes the total index, and a float which ranges from 0 to 1
    public static int IndexFromPercentage(int totalIndex, float percentage)
    {
        return Mathf.FloorToInt(Mathf.Min(totalIndex * percentage, totalIndex));
    }

    public static int CurrentSpriteIndex(int totalSpriteIndex, float elapsedTime, float totalTime)
    {
        return IndexFromPercentage(totalSpriteIndex, elapsedTime / totalTime);
    }
}
