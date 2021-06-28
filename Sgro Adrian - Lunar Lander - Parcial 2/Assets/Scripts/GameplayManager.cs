using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] Ship playerShip = null;
    int currentScore = 0;

    private void Awake()
    {
        playerShip.OnScoreGet += AddScore;
        playerShip.GetComponent<PlayerInput>().OnPausePressed += Pause;
    }

    void AddScore(int score)
    {
        currentScore += score;
    }

    private void Pause()
    {
        playerShip.ToggleMovement();
    }



}
