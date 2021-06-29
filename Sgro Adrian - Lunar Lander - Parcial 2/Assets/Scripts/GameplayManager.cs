using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] Ship playerShip = null;
    [SerializeField] TerrainGenerator terrainGenerator = null;
    [SerializeField] float timeBeforeReload = 3f;
    [SerializeField] float timeBetweenLevelCreation = 3f;
    int currentScore = 0;
    int currentLevel = 1;

    public static Action<int> UpdateScore;

    private void Awake()
    {
        playerShip.OnScoreGet += AddScore;
        playerShip.OnLanding += PlayerLanded;
        PlayerInput.OnPausePressed += Pause;
    }

    void AddScore(int score)
    {
        currentScore += score;
        UpdateScore?.Invoke(currentScore);
    }

    private void Pause()
    {
        playerShip.ToggleMovement();
    }

    void PlayerLanded(bool successful)
    {
        if (successful)
        {
            currentLevel++;
            StartCoroutine(SuccessfulLanding());
        }
    }

    IEnumerator SuccessfulLanding()
    {
        yield return new WaitForSeconds(timeBeforeReload);
        LoaderManager.Get().FakeLoad(timeBetweenLevelCreation, "Level " + currentLevel);
        yield return new WaitForSeconds(timeBetweenLevelCreation / 2);
        terrainGenerator.CreateNewTerrain();
        playerShip.ResetPositionToStart();
    }

}
