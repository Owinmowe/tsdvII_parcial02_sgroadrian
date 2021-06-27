using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] GameObject layerPrefab = null;
    [SerializeField] [Range(0, 1)] float backgroundSizeX = .5f;
    [SerializeField] Background background = null;
    [SerializeField] Vector2 startingVelocity = Vector2.zero;

    List<Animator> layersAnimators = new List<Animator>();

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

            Animator anim = go.GetComponent<Animator>();
            layersAnimators.Add(anim);

            var renderer = go.GetComponent<SpriteRenderer>();
            renderer.sprite = layer.sprite;
            renderer.sortingOrder = layer.drawOrder;

            go.transform.localScale = Vector3.one;

            var width = renderer.sprite.bounds.size.x;
            var height = renderer.sprite.bounds.size.y;

            float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            go.transform.localScale = new Vector2((worldScreenWidth / width) * backgroundSizeX, worldScreenHeight / height);
            renderer.size = new Vector2(renderer.size.x * 3, renderer.size.y * 3);
        }
    }

    public void SetBackgroundSpeed(Vector2 speed)
    {
        for (int i = 0; i < background.backgroundLayersList.Count; i++)
        {
            layersAnimators[i].SetFloat("SpeedMultiplierX", speed.x * background.backgroundLayersList[i].speedMultiplier);
            layersAnimators[i].SetFloat("SpeedMultiplierY", speed.y * background.backgroundLayersList[i].speedMultiplier);
        }
    }

    public void ResetBackgroundSpeed()
    {
        for (int i = 0; i < background.backgroundLayersList.Count; i++)
        {
            layersAnimators[i].SetFloat("SpeedMultiplierX", startingVelocity.x);
            layersAnimators[i].SetFloat("SpeedMultiplierY", startingVelocity.y);
        }
    }

}