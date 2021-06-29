using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Ingame : MonoBehaviour
{

    [SerializeField] Ship playerShip = null;
    [Header("General Game HUD")]
    [SerializeField] int scaleMultiplier = 100;
    [SerializeField] List<UI_Component> general_HUD = null;

    [Header("Right Panel")]
    [SerializeField] TextMeshProUGUI velocityXTextComponent = null;
    [SerializeField] TextMeshProUGUI velocityYTextComponent = null;
    [SerializeField] Image velocityXArrow = null;
    [SerializeField] Image velocityYArrow = null;
    [SerializeField] TextMeshProUGUI AltitudeTextComponent = null;

    [Header("Left Panel")]
    [SerializeField] Image fuelImageComponent = null;
    [SerializeField] TextMeshProUGUI fuelTextComponent = null;
    [SerializeField] TextMeshProUGUI scoreTextComponent = null;
    [SerializeField] TextMeshProUGUI timeTextComponent = null;
    float currentTime = 0;
    bool timeStoped = false;

    [Header("Pause HUD")]
    [SerializeField] List<UI_Component> pause_HUD = null;
    [SerializeField] UI_Component pauseButton = null;
    bool onPauseMenu = false;

    [Header("Help HUD")]
    [SerializeField] List<UI_Component> help_HUD = null;
    bool onHelpMenu = false;

    [Header("End game HUD")]
    [SerializeField] List<UI_Component> success_HUD = null;
    [SerializeField] List<UI_Component> crash_HUD = null;
    [SerializeField] List<UI_Component> overLimit_HUD = null;

    [Header("Background")]
    [SerializeField] BackgroundController bg = null;
    [SerializeField] Vector2 bGVelocityMultiplier = new Vector2(.1f, .1f);
    [SerializeField] Vector2 minimunVelocity = Vector2.zero;

    private void Awake()
    {
        if (playerShip)
        {
            playerShip.OnLanding += LandingEvent;
            playerShip.OnOutOfMoonGravity += OutOfMoonGravity;
            playerShip.OnVelocityChange += UpdateVelocity;
            playerShip.OnAltitudeChange += UpdateAltitude;
            playerShip.OnFuelConsumed += UpdateFuel;
            playerShip.OnShipReset += RestartMenus;
            GameplayManager.UpdateScore += UpdateScore;
            PlayerInput.OnPausePressed += Pause;
        }
    }

    private void Start()
    {
        foreach (var uI_Component in general_HUD)
        {
            uI_Component.TransitionIn();
        }
    }

    private void Update()
    {
        if(!timeStoped) UpdateTime();
    }

    private void RestartMenus()
    {
        timeStoped = false;
        foreach (var uI_Component in success_HUD)
        {
            uI_Component.TransitionOut();
        }
        foreach (var uI_Component in crash_HUD)
        {
            uI_Component.TransitionOut();
        }
        foreach (var uI_Component in general_HUD)
        {
            uI_Component.TransitionIn();
        }
    }

    void UpdateTime()
    {
        currentTime += Time.deltaTime;
        int time = Mathf.RoundToInt(currentTime);
        timeTextComponent.text = "Time: " + time.ToString();
        LoaderManager.Get().SetLastSessionTime(time);
    }

    void UpdateVelocity(Vector2 velocity)
    {
        if (velocity.x < 0) velocityXArrow.transform.eulerAngles = new Vector3(0, 0, 90);
        else velocityXArrow.transform.eulerAngles = new Vector3(0, 0, -90);
        if (velocity.y < 0) velocityYArrow.transform.eulerAngles = new Vector3(0, 0, -180);
        else velocityYArrow.transform.eulerAngles = new Vector3(0, 0, 0);

        float absVelX = Mathf.Abs(velocity.x);
        float absVelY = Mathf.Abs(velocity.y);

        velocityXTextComponent.text = "Velocity X: " + Mathf.RoundToInt(absVelX * scaleMultiplier).ToString();
        velocityYTextComponent.text = "Velocity y: " + Mathf.RoundToInt(absVelY * scaleMultiplier).ToString();

        Vector2 tempVel = new Vector2(-velocity.x * bGVelocityMultiplier.x, -velocity.y * bGVelocityMultiplier.y);
        bg.SetBackgroundSpeed(tempVel);
    }

    void UpdateAltitude(bool canCheck, float altitude)
    {
        if (canCheck) AltitudeTextComponent.text = "Altitude: " + Mathf.RoundToInt(altitude * scaleMultiplier).ToString();
        else AltitudeTextComponent.text = "Altitude: Undefined";
    }

    void UpdateFuel(float currentFuel, float maxFuel)
    {
        fuelImageComponent.fillAmount = currentFuel / maxFuel;
        fuelTextComponent.text = "Fuel: " + Mathf.RoundToInt(currentFuel) + "/" + Mathf.RoundToInt(maxFuel);
    }

    void UpdateScore(int score)
    {
        scoreTextComponent.text = "Score: " + score.ToString();
    }

    void LandingEvent(bool success)
    {
        timeStoped = true;
        bg.SetBackgroundSpeed(minimunVelocity);
        pauseButton.TransitionOut();
        if (success)
        {
            foreach (var item in success_HUD)
            {
                item.TransitionIn();
            }
        }
        else
        {
            foreach (var item in crash_HUD)
            {
                item.TransitionIn();
            }
        }
    }

    void OutOfMoonGravity()
    {
        timeStoped = true;
        pauseButton.TransitionOut();
        bg.SetBackgroundSpeed(minimunVelocity);
        foreach (var item in overLimit_HUD)
        {
            item.TransitionIn();
        }
    }

    void Pause()
    {
        onPauseMenu = !onPauseMenu;
        timeStoped = !timeStoped;
        if (onHelpMenu)
        {
            onHelpMenu = false;
            foreach (var item in help_HUD)
            {
                item.TransitionOut();
            }
        }
        if (onPauseMenu)
        {
            bg.SetBackgroundSpeed(Vector2.zero);
            pauseButton.TransitionOut();
            foreach (var item in pause_HUD)
            {
                item.TransitionIn();
            }
        }
        else
        {
            pauseButton.TransitionIn();
            foreach (var item in pause_HUD)
            {
                item.TransitionOut();
            }
        }
    }

    public void OpenHelpPanel()
    {
        onHelpMenu = true;
        foreach (var item in pause_HUD)
        {
            item.TransitionOut();
        }
        foreach (var item in help_HUD)
        {
            item.TransitionIn();
        }
    }

    public void CloseHelpPanel()
    {
        onHelpMenu = false;
        foreach (var item in pause_HUD)
        {
            item.TransitionIn();
        }
        foreach (var item in help_HUD)
        {
            item.TransitionOut();
        }
    }

    public void GoBackToMenu()
    {
        LoaderManager.Get().LoadSceneAsync("Main Menu");
    }

}
