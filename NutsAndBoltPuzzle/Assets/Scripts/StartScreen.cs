using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class StartScreen : MonoBehaviour
{
    private int _sceneToLoad;
    private void Start()
    {
        if (PlayerPrefs.GetInt("level", 1) > SceneManager.sceneCountInBuildSettings - 1)
        {
            _sceneToLoad = Random.Range(10, SceneManager.sceneCountInBuildSettings - 1);
        }
        else
        {
            _sceneToLoad = PlayerPrefs.GetInt("level", 1);
        }
        LoadSceneAsync();
    }
    public void LoadSceneAsync()
    {
        StartCoroutine(LoadSceneAsyncCoroutine());
    }

    IEnumerator LoadSceneAsyncCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneToLoad);
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            yield return null;
        }
    }
}
