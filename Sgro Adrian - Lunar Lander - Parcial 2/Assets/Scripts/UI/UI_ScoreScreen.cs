using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ScoreScreen : MonoBehaviour
{

    [SerializeField] List<UI_Component> ScoreScreen_HUD;
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText.text = "Final Score:\n" + LoaderManager.Get().GetLastSessionScore().ToString();
        foreach (var item in ScoreScreen_HUD)
        {
            item.TransitionIn();
        }
    }

    public void BackToMenu()
    {
        LoaderManager.Get().LoadSceneAsync("Main Menu");
    }

    public void PlayAgain()
    {
        LoaderManager.Get().LoadSceneAsync("Gameplay");
    }
}
