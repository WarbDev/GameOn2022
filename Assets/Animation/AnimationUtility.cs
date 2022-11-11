using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtility
{
    //Takes the total Count, and a float which ranges from 0 to 1
    //If Float is above 1 returns totalIndex, to avoid OutOfBounds Exceptions
    public static int IndexFromPercentage(int totalCount, float percentage)
    {
        return Mathf.FloorToInt(Mathf.Min(totalCount * percentage, totalCount - 1));
    }

    public static int CurrentSpriteIndex(int totalSpriteCount, float elapsedTime, float totalTime)
    {
        return IndexFromPercentage(totalSpriteCount, elapsedTime / totalTime);
    }
}
