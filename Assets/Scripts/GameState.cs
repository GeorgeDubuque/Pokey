using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState { 
    public static int currLevelNum = 1;
    public static List<string> levelsCompleted = new List<string>();
    public static List<string> skinsUnlocked = new List<string>() { "SkinRed", "SkinBlue" };
    public static int numCoins = 400;
    public static string skinName = "SkinRed";
}
