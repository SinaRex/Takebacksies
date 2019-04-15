using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameModeSelector
{
    private static int playerCloneCount = 2;

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
}
