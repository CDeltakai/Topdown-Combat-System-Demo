using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerController : MonoBehaviour
{

    void Awake()
    {
        Application.targetFrameRate = 144;
    }

    [RuntimeInitializeOnLoadMethod]
    static void InitProfiler()
    {
        UnityEngine.Profiling.Profiler.maxUsedMemory = 30000000;
    }


}
