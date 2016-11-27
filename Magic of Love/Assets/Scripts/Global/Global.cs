using UnityEngine;
using System.Collections;

public class Global
{
    public static int[] story = { 2, 2, 1, 2, 1, 2, 2, 2, 1, 2, 2, 2, 2, 2, 0 };

    public static int currentStory = 0;   
    public static int currentStage = 0;
    public static int currentChat = 0;

    public static bool skip = false;

    public static bool isDetect = false;
    public static int result = -1;

    public static bool enableInput = true;

    public static Vector3 handPosition;

    public static float Distance(Vector2 v1, Vector2 v2)
    {
        return Mathf.Sqrt(Mathf.Pow(v1.x - v2.x, 2f) + Mathf.Pow(v1.y - v2.y, 2f));
    }

    public static void Init()
    {
        currentStory = 0;
        currentStage = 0;
        currentChat = 0;
        isDetect = false;
    }

    public static Vector3 GetPosition()
    {
        return handPosition;
    }
}

