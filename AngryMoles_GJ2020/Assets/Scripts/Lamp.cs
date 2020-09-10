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
    public LightSettings shieldEmptyLightSettings;
    public LightSettings shadowLightSettings;
    public UnityEngine.Experimental.Rendering.Universal.Light2D lamp;

    public float maxShadowDuration = 2.0f;
    public float maxShieldDuration = 5f;
    public float shieldRechargeTime = 5f;
    public float shieldRechargeInitialDelay = 1f;

    private float maxCapacity = 1f;
    private float currentCapacity;
    private float localMaxShieldDuration = 5f;
    private float transitionTiming = 0f;
    private float elapsedShieldUpTime = 0f;
    private float elapsedShadowTime = 0f;
    private float elapsedShieldDownTime = 0f;

    private bool shieldActive = false;
    private bool shadowActive = false;
    //private bool shieldBurnedOut = false;

    private LightSettings currentLightSettings;
    private LightSettings targetLightSettings;

    private BoxCollider2D shieldCollider;
    private SpriteRenderer sprite;
    private float shieldMaxWidth;
    private float shieldMinWidth;

    // Start is called before the first frame update
    void Start()
    {
        currentCapacity = maxCapacity;
        currentLightSettings = defaultLightSettings.Clone();
        targetLightSettings = defaultLightSettings.Clone();
        shieldCollider = GetComponent<BoxCollider2D>();
        shieldCollider.enabled = false;
        shieldMaxWidth = shieldCollider.size.x;
        shieldMinWidth = shieldMaxWidth / 2;

        // debug only
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    // in case we want to do something if the shield is hit by bullets
    void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private bool CanActivate()
    {
        return currentCapacity > 0 && !shieldActive && !shadowActive;
    }

    public void ActivateShadow()
    {
        if ( CanActivate() )
        {
            targetLightSettings = shadowLightSettings.Clone();
            transitionTiming = 0f;
            EmptyShield();
            shadowActive = true;
            elapsedShadowTime = 0f;
        }
    }
    
    public void DeactivateShadow()
    {
        currentCapacity = 0;
        elapsedShadowTime = 0;
        ReturnToDefault();
    }

    public void ActivateShield()
    {
        if ( CanActivate())
        {
            targetLightSettings = shieldLightSettings.Clone();
            transitionTiming = 0f;
            shieldActive = true;
            shieldCollider.enabled = true;
            //sprite.enabled = true;
            localMaxShieldDuration = maxShieldDuration * currentCapacity / maxCapacity;
        }
    }

    private void BeginShieldDegradation()
    {
        targetLightSettings = shieldEmptyLightSettings.Clone();
        targetLightSettings.transitionDuration = maxShieldDuration;
        transitionTiming = 0f;
    }

    // after burning the shield out?
    private void ReturnToDefault()
    {
        targetLightSettings = defaultLightSettings.Clone();
        transitionTiming = 0f;
        shadowActive = false;
        shieldActive = false;
    }

    private void EmptyShield()
    {
        currentCapacity = 0f;
        elapsedShieldUpTime = 0f;
    }

    public void DeactivateShield()
    {
        DeactivateShadow();
        shieldActive = false;
        shieldCollider.enabled = false;
        sprite.enabled = false;
    }

    private void UpdateShield()
    {
        if ( shadowActive )
        {
            elapsedShadowTime += Time.deltaTime;
            if ( elapsedShadowTime > maxShadowDuration)
            {
                DeactivateShadow();
            }
        }

        if ( shieldActive)
        {
            elapsedShieldUpTime += Time.deltaTime;
            if(transitionTiming == targetLightSettings.transitionDuration )
            {
                Debug.Log("Shield degradation beginning");
                BeginShieldDegradation();
            }

            currentCapacity -= Time.deltaTime / localMaxShieldDuration;
            shieldCollider.size.Set(Mathf.Lerp(shieldMaxWidth, shieldMinWidth, currentCapacity), shieldCollider.size.y);
            if ( currentCapacity <= 0 )
            {
                // shield burned out
                DeactivateShield();
            }
        }
        else if ( !shieldActive && !shadowActive )
        {
            elapsedShieldDownTime += Time.deltaTime;
            // regen shield
            if (elapsedShieldDownTime < shieldRechargeInitialDelay)
            {
                return;
            }
            else
            {
                currentCapacity = elapsedShieldDownTime / (shieldRechargeTime + shieldRechargeInitialDelay);
            }

            if ( currentCapacity > 1 )
            {
                currentCapacity = 1;
                elapsedShieldUpTime = 0;
            }
        }
    }

    void Update()
    {
        if (transitionTiming <= targetLightSettings.transitionDuration)
        {

            transitionTiming += Time.deltaTime;
            if (transitionTiming > targetLightSettings.transitionDuration)
            {
                transitionTiming = targetLightSettings.transitionDuration; ;
            }
        }
        UpdateShield();
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
