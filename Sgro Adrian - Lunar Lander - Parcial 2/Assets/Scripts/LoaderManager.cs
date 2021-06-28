﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonoBehaviourSingleton<LoaderManager>
{

    [SerializeField] float minTimeToLoadScene = 1f;
    [SerializeField] UI_LoadingScreen uI_LoadingScreen = null;

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(AsynchronousLoadWithFake(sceneName));
    }

    IEnumerator AsynchronousLoadWithFake(string scene)
    {
        float loadingProgress = 0;
        float timeLoading = 0;

        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        uI_LoadingScreen.FadeWithBlackScreen();
        uI_LoadingScreen.LockFade();
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            timeLoading += Time.deltaTime;
            loadingProgress = ao.progress + 0.1f;
            loadingProgress = loadingProgress * timeLoading / minTimeToLoadScene;

            // Se completo la carga
            if (loadingProgress >= 1)
            {
                ao.allowSceneActivation = true;
                uI_LoadingScreen.UnlockFade();
            }
            yield return null;
        }

    }

    public void FakeLoad(float time)
    {
        StartCoroutine(FakeLoadingWithBlackScreen(time));
    }

    IEnumerator FakeLoadingWithBlackScreen(float time)
    {
        uI_LoadingScreen.FadeWithBlackScreen();
        uI_LoadingScreen.LockFade();
        yield return new WaitForSeconds(time);
        uI_LoadingScreen.UnlockFade();
    }
}
