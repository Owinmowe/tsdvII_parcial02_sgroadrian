using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] List<UI_Component> splashArts = null;
    [Space(10)]
    [SerializeField] List<UI_Component> mainMenuComponents = null;
    [SerializeField] TextMeshProUGUI versionText = null;
    [Space(10)]
    [SerializeField] List<UI_Component> creditsMenuComponents = null;



    int currentSplashArt = 0;

    private void Start()
    {
        versionText.text = "V" + Application.version; 
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

    public void ExitGame()
    {
        Application.Quit();
    }

}
