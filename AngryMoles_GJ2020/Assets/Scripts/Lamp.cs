using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine;
using System;

[System.Serializable]
public class LightSettings
{
    public float innerMaxAngle = 60.0f;
    public float outerMaxAngle = 120.0f;
    public float innerMaxRadius = 1.0f;
    public float outerMaxRadius = 5.0f;
    public float maxIntensity = 2.0f;
    public float maxBarrierScale = 10.0f;
    public float transitionDuration = 1.0f;

    public LightSettings Clone() { return (LightSettings)this.MemberwiseClone(); }
}

public class Lamp : MonoBehaviour
{


    public LightSettings defaultLightSettings;
    public LightSettings shieldLightSettings;
    public LightSettings shadowLightSettings;
    public UnityEngine.Experimental.Rendering.Universal.Light2D lamp;
    public float powerRate = 1.0f;

    private float maxCapacity = 1f;
    private float currentCapacity;
    private float transitionTiming = 0f;

    private LightSettings currentLightSettings;
    private LightSettings targetLightSettings;


    // Start is called before the first frame update
    void Start()
    {
        currentCapacity = maxCapacity;
        currentLightSettings = defaultLightSettings.Clone();
        targetLightSettings = defaultLightSettings.Clone();
    }

    public void ActivateShadow()
    {
        targetLightSettings = shadowLightSettings;
        transitionTiming = 0f;
    }
    
    public void DeactivateShadow()
    {
        targetLightSettings = defaultLightSettings;
        transitionTiming = 0f;
    }

    public void ActivateShield()
    {
        targetLightSettings = shieldLightSettings;
        transitionTiming = 0f;
    }

    public void DeactivateShield()
    {
        DeactivateShadow();
    }

    void Update()
    {
        if (transitionTiming < targetLightSettings.transitionDuration)
        {
            transitionTiming += Time.deltaTime;
            if ( transitionTiming > targetLightSettings.transitionDuration)
            {
                transitionTiming = targetLightSettings.transitionDuration; ;
            }
        }
        UpdateLight();
    }

    public void UpdateLight()
    {
        float percentComplete = transitionTiming / targetLightSettings.transitionDuration;
        currentLightSettings.innerMaxAngle = Mathf.Lerp(currentLightSettings.innerMaxAngle, targetLightSettings.innerMaxAngle, percentComplete);
        currentLightSettings.outerMaxAngle = Mathf.Lerp(currentLightSettings.outerMaxAngle, targetLightSettings.outerMaxAngle, percentComplete);
        currentLightSettings.innerMaxRadius = Mathf.Lerp(currentLightSettings.innerMaxRadius, targetLightSettings.innerMaxRadius, percentComplete);
        currentLightSettings.outerMaxRadius = Mathf.Lerp(currentLightSettings.outerMaxRadius, targetLightSettings.outerMaxRadius, percentComplete);
        currentLightSettings.maxIntensity = Mathf.Lerp(currentLightSettings.maxIntensity, targetLightSettings.maxIntensity, percentComplete);

        lamp.pointLightInnerAngle = currentLightSettings.innerMaxAngle;
        lamp.pointLightOuterAngle = currentLightSettings.outerMaxAngle;
        lamp.pointLightInnerRadius = currentLightSettings.innerMaxRadius;
        lamp.pointLightOuterRadius = currentLightSettings.outerMaxRadius;
        lamp.intensity = currentLightSettings.maxIntensity;
    }
}
