using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] Ship playerShip = null;
    [SerializeField] TerrainGenerator terrainGenerator = null;
    [SerializeField] float timeBetweenLevelCreation = 3f;
    int currentScore = 0;

    private void Awake()
    {
        playerShip.OnScoreGet += AddScore;
        playerShip.GetComponent<PlayerInput>().OnPausePressed += Pause;
        playerShip.OnLanding += PlayerLanded;
    }

    void AddScore(int score)
    {
        currentScore += score;
    }

    private void Pause()
    {
        playerShip.ToggleMovement();
    }

    void PlayerLanded(bool successful)
    {
        if (successful)
        {
            StartCoroutine(SuccessfulLanding());
        }
    }

    IEnumerator SuccessfulLanding()
    {
        LoaderManager.Get().FakeLoad(timeBetweenLevelCreation);
        yield return new WaitForSeconds(timeBetweenLevelCreation / 2);
        terrainGenerator.CreateNewTerrain();
        playerShip.ResetPositionToStart();
    }

}
