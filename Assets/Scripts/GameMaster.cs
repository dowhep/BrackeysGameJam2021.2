using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameMaster 
{

    public static int lastBuildIndex { get; private set; } = -1;
    public static List<CheckpointScr> checkpoints;
    public static Vector3 CPPos { get; private set; }
    public static bool[] CPCharAccesses { get; private set; }
    public static float CPGravity { get; private set; }
    public static int CPChar { get; private set; }
    public static Vector3 GetPlayerSpawn()
    {
        return SceneManager.GetActiveScene().buildIndex == lastBuildIndex ? CPPos : Vector3.zero;
    }
    public static bool[] GetAccesses()
    {
        return SceneManager.GetActiveScene().buildIndex == lastBuildIndex ? CPCharAccesses : null;
    }
    public static float GetGravity()
    {
        return SceneManager.GetActiveScene().buildIndex == lastBuildIndex ? CPGravity : -1f;
    }
    public static int GetChar()
    {
        return SceneManager.GetActiveScene().buildIndex == lastBuildIndex ? CPChar : -1;
    }
    public static void Checkpointed(CheckpointScr checkpoint, Vector3 pos, bool[] accesses, float curG, int curChar)
    {
        lastBuildIndex = SceneManager.GetActiveScene().buildIndex;
        CPPos = pos;
        CPCharAccesses = accesses;
        CPGravity = curG;
        CPChar = curChar;

        ResetAllCheckpoint(checkpoint);
    }

    private static void ResetAllCheckpoint(CheckpointScr activator)
    {
        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (checkpoints[i] != activator)
            {
                checkpoints[i].ResetCP();
            }
        }
    }

    public static void Reset()
    {
        lastBuildIndex = -1;
    }
}
