using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingScreen : MonoBehaviour
{
    [SerializeField] Image blackScreen;

    bool canFadeOut = true;

    public void FadeWithBlackScreen()
    {
        StopAllCoroutines();
        StartCoroutine(BlackScreenFade());
    }

    IEnumerator BlackScreenFade()
    {
        while(blackScreen.color.a + Time.deltaTime < 1)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a + Time.deltaTime);
            yield return null;
        }
        blackScreen.color = Color.black;
        while (!canFadeOut)
        {
            yield return null;
        }
        while (blackScreen.color.a - Time.deltaTime > 0)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a - Time.deltaTime);
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);
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
