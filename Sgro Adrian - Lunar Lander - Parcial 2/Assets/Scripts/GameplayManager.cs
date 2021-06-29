using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] Ship playerShip = null;
    [SerializeField] TerrainGenerator terrainGenerator = null;
    [SerializeField] float timeBetweenLevelCreation = 3f;
    [SerializeField] float timeBeforeScoreScreen = 3f;
    int currentScore = 0;
    int currentLevel = 1;
    bool nextLevelUnlocked = false;

    public static Action<int> UpdateScore;

    private void Awake()
    {
        playerShip.OnScoreGet += AddScore;
        playerShip.OnLanding += PlayerLanded;
        playerShip.OnOutOfMoonGravity += OverTheLimit;
        PlayerInput.OnPausePressed += Pause;
        terrainGenerator.OnSetNewLimit += SetNewLimit;
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
            StartCoroutine(SuccessfulLanding());
        }
        else
        {
            StartCoroutine(EndGameplay());
        }
    }

    public void UnlockNextLevel()
    {
        nextLevelUnlocked = true;
    }

    IEnumerator SuccessfulLanding()
    {
        currentLevel++;
        while (!nextLevelUnlocked) yield return null;
        nextLevelUnlocked = false;
        LoaderManager.Get().FakeLoad(timeBetweenLevelCreation, "Level " + currentLevel);
        yield return new WaitForSeconds(timeBetweenLevelCreation / 2);
        terrainGenerator.CreateNewTerrain();
        playerShip.ResetPositionToStart();
    }

    IEnumerator EndGameplay()
    {
        LoaderManager.Get().SetLastSessionScore(currentScore);
        yield return new WaitForSeconds(timeBeforeScoreScreen);
        LoaderManager.Get().LoadSceneAsync("Score Scene");
    }

    void SetNewLimit(int limit)
    {
        playerShip.SetLimitY(limit);
    }

    void OverTheLimit()
    {
        StartCoroutine(EndGameplay());
    }

}
