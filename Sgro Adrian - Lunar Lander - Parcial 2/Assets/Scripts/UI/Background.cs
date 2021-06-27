using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class backgroundLayer
{
    public string layerName;
    public float speedMultiplier;
    public Sprite sprite;
    public int drawOrder;
}

[CreateAssetMenu(fileName = "Background", menuName = "Background/Background")]
public class Background : ScriptableObject
{
    public List<backgroundLayer> backgroundLayersList;
}
