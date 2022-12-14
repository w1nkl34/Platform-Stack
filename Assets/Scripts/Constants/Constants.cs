using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants 
{
    public static bool gameStarted = false;
    public static bool gameGenerated = false;
    public static bool gameWon = false;
    public static int currentStack = 1;
    public static int lastStack = 0;
    public static int level = 1;
    public static List<StackController> allStackControllers = new List<StackController>();
}
