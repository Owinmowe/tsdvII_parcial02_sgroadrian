using UnityEngine;
using TMPro;

public class LandingSite : MonoBehaviour
{

    int points = 0;
    [SerializeField] GameObject LandingLine = null;
    [SerializeField] TextMeshPro textComponent = null;
    [SerializeField] float textBlinkSpeed = 1f;

    private void Awake()
    {
        GetComponent<Animator>().SetFloat("BlinkSpeed", textBlinkSpeed);
    }

    public void SetLandingType(int points, float colliderXSize, Color color)
    {
        this.points = points;
        gameObject.name = points.ToString() + "X";
        textComponent.text = points.ToString() + "X";
        textComponent.color = color;
        LandingLine.GetComponent<SpriteRenderer>().color = color;
        LandingLine.transform.localScale = new Vector2(colliderXSize, LandingLine.transform.localScale.y); 
    }
}
