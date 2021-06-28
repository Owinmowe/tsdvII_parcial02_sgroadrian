using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] GameObject layerPrefab = null;
    [SerializeField] Background background = null;
    [SerializeField] Vector2 startingVelocity = Vector2.zero;
    const int backgroundRepeatAmount = 3;

    List<BackgroundLayer> layers = new List<BackgroundLayer>();

    private void Start()
    {
        StartBackground();
        ResetBackgroundSpeed();
    }

    public void StartBackground()
    {

        foreach (var layer in background.backgroundLayersList)
        {
            GameObject go = Instantiate(layerPrefab);
            go.name = layer.layerName;
            go.transform.parent = transform;

            BackgroundLayer BGlayer = go.GetComponent<BackgroundLayer>();
            layers.Add(BGlayer);
            BGlayer.SetBaseSpeedMultiplier(layer.speedMultiplier);

            var renderer = go.GetComponent<SpriteRenderer>();
            renderer.sprite = layer.sprite;
            renderer.sortingOrder = layer.drawOrder;

            go.transform.localScale = Vector3.one;

            var width = renderer.sprite.bounds.size.x;
            var height = renderer.sprite.bounds.size.y;

            float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            BGlayer.SetScreenSize(new Vector2(worldScreenWidth, worldScreenHeight));

            go.transform.localScale = new Vector2((worldScreenWidth / width) , worldScreenHeight / height);
            renderer.size = new Vector2(renderer.size.x * backgroundRepeatAmount, renderer.size.y * backgroundRepeatAmount);
        }
    }

    private void Update()
    {
        foreach (var layer in layers)
        {
            layer.UpdateLayer();
        }
    }

    public void SetBackgroundSpeed(Vector2 velocity)
    {
        foreach (var layer in layers)
        {
            layer.SetVelocity(velocity);
        }
    }

    public void ResetBackgroundSpeed()
    {
        foreach (var layer in layers)
        {
            layer.SetVelocity(startingVelocity);
        }
    }

}