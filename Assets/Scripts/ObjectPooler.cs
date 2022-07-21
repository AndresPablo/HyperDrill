using UnityEngine;

public static class ObjectPooler
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("After Scene is loaded and game is running");
    }
}
