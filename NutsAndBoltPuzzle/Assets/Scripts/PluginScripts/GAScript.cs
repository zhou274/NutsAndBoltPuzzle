using System;
using UnityEngine;
using GameAnalyticsSDK;
using System.Collections.Generic;


public class GAScript : MonoBehaviour
{
    public static GAScript Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Start()
    {
        GameAnalytics.Initialize();
    }

    public void LevelStart(string levelName)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelName);
    }

    public void LevelEnd(bool isWin, string levelName)
    {
        if (isWin) LevelCompleted(levelName);
        else LevelFail(levelName);
    }

    private void LevelFail(string levelName)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelName);
    }

    private void LevelCompleted(string levelName)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelName);
    }
}