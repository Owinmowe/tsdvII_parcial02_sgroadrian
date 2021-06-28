using UnityEngine;
using TMPro;

public class LandingSite : MonoBehaviour
{

    int score = 0;
    [SerializeField] GameObject LandingLine = null;
    [SerializeField] TextMeshPro textComponent = null;
    [SerializeField] float textBlinkSpeed = 1f;

    private void Awake()
    {
        GetComponent<Animator>().SetFloat("BlinkSpeed", textBlinkSpeed);
    }

    public void SetLandingType(int baseScore, int multi, float colliderXSize, Color color)
    {
        score = multi * baseScore;
        gameObject.name = multi.ToString() + "X";
        textComponent.text = multi.ToString() + "X";
        textComponent.color = color;
        LandingLine.GetComponent<SpriteRenderer>().color = color;
        LandingLine.transform.localScale = new Vector2(colliderXSize, LandingLine.transform.localScale.y); 
    }

    public int GetScore()
    {
        return score;
    }
}
