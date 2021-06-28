using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LoadingScreen : MonoBehaviour
{
    [SerializeField] Image blackScreen = null;
    [SerializeField] TextMeshProUGUI textComponent = null;

    bool canFadeOut = true;

    public void FadeWithBlackScreen()
    {
        StopAllCoroutines();
        textComponent.text = "";
        StartCoroutine(BlackScreenFade());
    }

    public void FadeWithBlackScreen(string text)
    {
        StopAllCoroutines();
        textComponent.text = text;
        StartCoroutine(BlackScreenFade());
    }

    IEnumerator BlackScreenFade()
    {
        while(blackScreen.color.a + Time.deltaTime < 1)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a + Time.deltaTime);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, textComponent.color.a + Time.deltaTime);
            yield return null;
        }
        blackScreen.color = Color.black;
        textComponent.color = Color.white;
        while (!canFadeOut)
        {
            yield return null;
        }
        while (blackScreen.color.a - Time.deltaTime > 0)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a - Time.deltaTime);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, blackScreen.color.a - Time.deltaTime);
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0);
    }

    public void LockFade()
    {
        canFadeOut = false;
    }

    public void UnlockFade()
    {
        canFadeOut = true;
    }
}
