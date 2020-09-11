using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine;
using System;


[System.Serializable]
public class LightPair
{
    public LightSettings cone;
    public LightSettings point;
    public float transitionDuration = 1.0f;
    public LightPair Clone() { return (LightPair)this.MemberwiseClone(); }
}
[System.Serializable]
public class LightSettings
{
    public float innerMaxAngle = 60.0f;
    public float outerMaxAngle = 120.0f;
    public float innerMaxRadius = 1.0f;
    public float outerMaxRadius = 5.0f;
    public float maxIntensity = 2.0f;
    public float maxBarrierScale = 10.0f;

    public LightSettings Clone() { return (LightSettings)this.MemberwiseClone(); }
}

public class Lamp : MonoBehaviour
{
    public LightPair defaultLightSettings;
    public LightPair shieldLightSettings;
    public LightPair shieldEmptyLightSettings;
    public LightPair shadowLightSettings;
    public UnityEngine.Experimental.Rendering.Universal.Light2D point;
    public UnityEngine.Experimental.Rendering.Universal.Light2D cone;

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

    private LightPair currentLightSettings;
    private LightPair targetLightSettings;

   
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

    private void UpdateShield()
    {
        if (shadowActive)
        {
            elapsedShadowTime += Time.deltaTime;
            if (elapsedShadowTime > maxShadowDuration)
            {
                DeactivateShadow();
            }
        }

        if (shieldActive)
        {
            elapsedShieldUpTime += Time.deltaTime;
            if (transitionTiming == targetLightSettings.transitionDuration)
            {
                Debug.Log("Shield degradation beginning");
                BeginShieldDegradation();
            }

            currentCapacity -= Time.deltaTime / localMaxShieldDuration;
            shieldCollider.size.Set(Mathf.Lerp(shieldMaxWidth, shieldMinWidth, currentCapacity), shieldCollider.size.y);
            if (currentCapacity <= 0)
            {
                // shield burned out
                DeactivateShield();
            }
        }
        else if (!shieldActive && !shadowActive)
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

            if (currentCapacity > 1)
            {
                currentCapacity = 1;
                elapsedShieldUpTime = 0;
            }
        }
    }

    public void UpdateLight()
    {
        float percentComplete = transitionTiming / targetLightSettings.transitionDuration;

        currentLightSettings.cone.innerMaxAngle = Mathf.Lerp(currentLightSettings.cone.innerMaxAngle, targetLightSettings.cone.innerMaxAngle, percentComplete);
        currentLightSettings.cone.outerMaxAngle = Mathf.Lerp(currentLightSettings.cone.outerMaxAngle, targetLightSettings.cone.outerMaxAngle, percentComplete);
        currentLightSettings.cone.innerMaxRadius = Mathf.Lerp(currentLightSettings.cone.innerMaxRadius, targetLightSettings.cone.innerMaxRadius, percentComplete);
        currentLightSettings.cone.outerMaxRadius = Mathf.Lerp(currentLightSettings.cone.outerMaxRadius, targetLightSettings.cone.outerMaxRadius, percentComplete);
        currentLightSettings.cone.maxIntensity = Mathf.Lerp(currentLightSettings.cone.maxIntensity, targetLightSettings.cone.maxIntensity, percentComplete);

        cone.pointLightInnerAngle = currentLightSettings.cone.innerMaxAngle;
        cone.pointLightOuterAngle = currentLightSettings.cone.outerMaxAngle;
        cone.pointLightInnerRadius = currentLightSettings.cone.innerMaxRadius;
        cone.pointLightOuterRadius = currentLightSettings.cone.outerMaxRadius;
        cone.intensity = currentLightSettings.cone.maxIntensity;

        currentLightSettings.point.innerMaxAngle = Mathf.Lerp(currentLightSettings.point.innerMaxAngle, targetLightSettings.point.innerMaxAngle, percentComplete);
        currentLightSettings.point.outerMaxAngle = Mathf.Lerp(currentLightSettings.point.outerMaxAngle, targetLightSettings.point.outerMaxAngle, percentComplete);
        currentLightSettings.point.innerMaxRadius = Mathf.Lerp(currentLightSettings.point.innerMaxRadius, targetLightSettings.point.innerMaxRadius, percentComplete);
        currentLightSettings.point.outerMaxRadius = Mathf.Lerp(currentLightSettings.point.outerMaxRadius, targetLightSettings.point.outerMaxRadius, percentComplete);
        currentLightSettings.point.maxIntensity = Mathf.Lerp(currentLightSettings.point.maxIntensity, targetLightSettings.point.maxIntensity, percentComplete);

        point.pointLightInnerAngle = currentLightSettings.point.innerMaxAngle;
        point.pointLightOuterAngle = currentLightSettings.point.outerMaxAngle;
        point.pointLightInnerRadius = currentLightSettings.point.innerMaxRadius;
        point.pointLightOuterRadius = currentLightSettings.point.outerMaxRadius;
        point.intensity = currentLightSettings.point.maxIntensity;
    }
}
