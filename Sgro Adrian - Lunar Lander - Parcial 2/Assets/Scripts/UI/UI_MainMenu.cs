using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] List<UI_Component> splashArts = null;
    [Space(10)]
    [SerializeField] List<UI_Component> mainMenuComponents = null;
    [SerializeField] List<UI_Component> creditsMenuComponents = null;

    int currentSplashArt = 0;

    private void Start()
    {
        foreach (var item in splashArts)
        {
            item.OnTransitionEnd += NextSplashArt;
        }
        if(splashArts != null)
        {
            splashArts[0].TransitionIn();
        }
    }

    void NextSplashArt()
    {
        currentSplashArt++;
        if (currentSplashArt < splashArts.Count)
        {
            splashArts[currentSplashArt].TransitionIn();
        }
        else
        {
            foreach (var item in mainMenuComponents)
            {
                item.TransitionIn();
            }
        }
    }

    public void StartMenu()
    {
        foreach (var item in creditsMenuComponents)
        {
            item.TransitionOut();
        }
        foreach (var item in mainMenuComponents)
        {
            item.TransitionIn();
        }
    }

    public void StartCredits()
    {
        foreach (var item in mainMenuComponents)
        {
            item.TransitionOut();
        }
        foreach (var item in creditsMenuComponents)
        {
            item.TransitionIn();
        }
    }

    public void StartGame()
    {
        foreach (var item in mainMenuComponents)
        {
            item.TransitionOut();
        }
        LoaderManager.Get().LoadSceneAsync("Gameplay");
    }

}
