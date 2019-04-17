using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameModeSelector
{
    private static int playerCloneCount = 1;
    private static int maxWinCount = 2;

    public static int PlayerCloneCount
    {
        get
        {
            return playerCloneCount;
        }
        set
        {
            playerCloneCount = value;
        }
    }

    public static int MaxWinCount
    {
        get
        {
            return maxWinCount;
        }
        set
        {
            maxWinCount = value;
        }
    }
}
