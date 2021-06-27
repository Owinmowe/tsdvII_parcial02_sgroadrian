using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Ship player = null;
    [SerializeField] float playerClosePosition = 7f;
    [SerializeField] float playerBackUpPosition = 7f;
    [SerializeField] float cameraZoomInDistance = 6f;
    [SerializeField] float cameraZoomInAltitude = 3f;
    [SerializeField] float cameraZoomOutDistance = 10f;

    Vector3 playerInitialPosition = Vector3.zero;
    Camera camComponent;
    bool closeUp = false;


    private void Awake()
    {
        camComponent = GetComponent<Camera>();
    }

    private void Start()
    {
        playerInitialPosition = player.transform.position;
    }

    private void Update()
    {
        if (!closeUp && player.transform.position.y < playerClosePosition) CloseUp();
        else if(closeUp && player.transform.position.y > playerBackUpPosition) BackUp();
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }

    void CloseUp()
    {
        closeUp = true;
        camComponent.orthographicSize = cameraZoomInDistance;
        transform.position = new Vector3(player.transform.position.x, cameraZoomInAltitude, -1f);
    }

    void BackUp()
    {
        closeUp = false;
        camComponent.orthographicSize = cameraZoomOutDistance;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1f);
    }

}
