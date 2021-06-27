using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Ingame : MonoBehaviour
{

    [SerializeField] Ship playerShip = null;
    [Header("Game HUD")]
    [SerializeField] int scaleMultiplier = 100;
    [SerializeField] UI_Component velocityXText = null;
    [SerializeField] UI_Component velocityYText = null;
    [SerializeField] UI_Component AltitudeText = null;
    [SerializeField] UI_Component fuelBar = null;
    [SerializeField] List<UI_Component> general_HUD = null;

    [Header("Pause HUD")]
    [SerializeField] List<UI_Component> pause_HUD = null;

    [Header("End game HUD")]
    [SerializeField] List<UI_Component> success_HUD = null;
    [SerializeField] List<UI_Component> fail_HUD = null;

    [Header("Background")]
    [SerializeField] BackgroundController bg = null;
    [SerializeField] Vector2 slowVelocity = Vector2.zero;

    TextMeshProUGUI velocityXTextComponent = null;
    TextMeshProUGUI velocityYTextComponent = null;
    TextMeshProUGUI AltitudeTextComponent = null;
    Image fuelImageComponent = null;

    private void Awake()
    {
        if (playerShip)
        {
            playerShip.OnVelocityChange += UpdateVelocity;
            playerShip.OnLanding += LandingEvent;
            playerShip.OnAltitudeChange += UpdateAltitude;
            playerShip.OnFuelConsumed += UpdateFuel;
        }        
    }

    void Start()
    {
        InitializeMenus();
    }

    private void InitializeMenus()
    {
        if (velocityXText)
        {
            velocityXText.TransitionIn();
            velocityXTextComponent = velocityXText.GetComponent<TextMeshProUGUI>();
        }
        if (velocityYText)
        {
            velocityYText.TransitionIn();
            velocityYTextComponent = velocityYText.GetComponent<TextMeshProUGUI>();
        }
        if (AltitudeText)
        {
            AltitudeText.TransitionIn();
            AltitudeTextComponent = AltitudeText.GetComponent<TextMeshProUGUI>();
        }
        if (fuelBar)
        {
            fuelBar.TransitionIn();
            fuelImageComponent = fuelBar.GetComponent<Image>();
        }
        foreach (var uI_Component in general_HUD)
        {
            uI_Component.TransitionIn();
        }
    }

    void UpdateVelocity(Vector2 velocity)
    {
        velocityXTextComponent.text = "Velocity X: " + Mathf.RoundToInt(velocity.x * scaleMultiplier).ToString();
        velocityYTextComponent.text = "Velocity y: " + Mathf.RoundToInt(velocity.y * scaleMultiplier).ToString();
        bg.SetBackgroundSpeed(velocity);
    }

    void UpdateAltitude(bool canCheck, float altitude)
    {
        if (canCheck)
        {
            if(altitude < 1) altitude = 0;
            AltitudeTextComponent.text = "Altitude: " + Mathf.RoundToInt(altitude * scaleMultiplier).ToString();
        }
        else AltitudeTextComponent.text = "Altitude: Undefined \nSensors can't reach terrain.";
    }

    void UpdateFuel(float currentFuel, float maxFuel)
    {
        fuelImageComponent.fillAmount = currentFuel / maxFuel;
    }

    void LandingEvent(bool success)
    {
        bg.SetBackgroundSpeed(slowVelocity);
        if (success)
        {
            foreach (var item in success_HUD)
            {
                item.TransitionIn();
            }
        }
        else
        {
            foreach (var item in fail_HUD)
            {
                item.TransitionIn();
            }
        }
    }

}
