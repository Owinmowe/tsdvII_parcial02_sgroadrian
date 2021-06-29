using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Ship player = null;
    [SerializeField] float playerClosePosition = 7f;
    [SerializeField] float playerBackUpPosition = 7f;
    [SerializeField] float playerFollowYPosition = 10f;
    [SerializeField] float cameraZoomInDistance = 6f;
    [SerializeField] float cameraZoomInAltitude = 3f;
    [SerializeField] float cameraZoomOutDistance = 10f;

    Camera camComponent;
    bool closeUp = false;
    bool followingPlayer = true;
    float distanceFromPlayer = 0;

    private void Awake()
    {
        camComponent = GetComponent<Camera>();
        distanceFromPlayer = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), player.transform.position);
        player.OnOutOfMoonGravity += StopFollowingPlayer;
        if (player == null) followingPlayer = false;
    }

    private void Update()
    {
        if (!followingPlayer) return;
        if (!closeUp && player.transform.position.y < playerClosePosition) CloseUp();
        else if(closeUp && player.transform.position.y > playerBackUpPosition) BackUp();

        if (player.transform.position.y > playerFollowYPosition)
        {
            FollowPlayerXY();
        }
        else
        {
            FollowPlayerX();
        }
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

    void FollowPlayerXY()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y - distanceFromPlayer, transform.position.z);
    }

    void FollowPlayerX()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }

    void StopFollowingPlayer()
    {
        followingPlayer = false;
    }

}
