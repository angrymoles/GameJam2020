﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine;
using UnityEngine.UI;
using System;


[System.Serializable]
public class LightPair
{
    public LightSettings cone;
    public LightSettings point;
    public float transitionDuration = 1.0f;
    public LightPair Clone()
    {
        var clone = new LightPair();
        clone.cone = cone.Clone();
        clone.point = point.Clone();
        clone.transitionDuration = transitionDuration;
        return clone;
    }
}
[System.Serializable]
public class LightSettings
{
    public float innerMaxAngle = 60.0f;
    public float outerMaxAngle = 120.0f;
    public float innerMaxRadius = 1.0f;
    public float outerMaxRadius = 5.0f;
    [Range(0,3f)]
    public float maxIntensity = 2.0f;
    public Color color;

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
    public Slider shadowSlider;
    public Slider shieldSlider;

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
    private LightPair oldLightSettings;
    private LightPair targetLightSettings;

   
    public PolygonCollider2D shieldCollider;
    private SpriteRenderer sprite;
    private Transform shieldTransform;
    private float shieldMaxWidth;
    private float shieldMinWidth;
    public float shieldMinWidthPercent = 0.5f;

    private float shieldMaxDist;
    private float shieldMinDist;
    public float shieldMinDistPercent = 0.5f;
    public GameObject shieldTriggerFX;
    public GameObject shieldEmptyFX;
    public GameObject onCollideProjectileFX;

    // Start is called before the first frame update
    void Start()
    {
        currentCapacity = maxCapacity;
        oldLightSettings = defaultLightSettings.Clone();
        currentLightSettings = defaultLightSettings.Clone();
        targetLightSettings = defaultLightSettings.Clone();        
        shieldCollider.enabled = false;
        
        Physics2D.IgnoreLayerCollision(10, 12);
        Physics2D.IgnoreLayerCollision(11, 12);

        // debug only
        sprite = shieldCollider.GetComponent<SpriteRenderer>();
        sprite.enabled = false;

        shieldTransform = shieldCollider.gameObject.transform;

        shieldMaxWidth = shieldTransform.localScale.x;
        shieldMinWidth = shieldMaxWidth * shieldMinWidthPercent;

        shieldMaxDist = shieldTransform.localPosition.y;
        shieldMinDist = shieldMaxDist * shieldMinDistPercent;

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
            oldLightSettings = currentLightSettings.Clone();
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
            var sourcelightSettings = shieldLightSettings.Clone();
            currentLightSettings.cone.innerMaxAngle = Mathf.Lerp(sourcelightSettings.cone.innerMaxAngle, shieldEmptyLightSettings.cone.innerMaxAngle, 1 - currentCapacity);
            currentLightSettings.cone.outerMaxAngle = Mathf.Lerp(sourcelightSettings.cone.outerMaxAngle, shieldEmptyLightSettings.cone.outerMaxAngle, 1 - currentCapacity);
            currentLightSettings.cone.innerMaxRadius = Mathf.Lerp(sourcelightSettings.cone.innerMaxRadius, shieldEmptyLightSettings.cone.innerMaxRadius, 1 - currentCapacity);
            currentLightSettings.cone.outerMaxRadius = Mathf.Lerp(sourcelightSettings.cone.outerMaxRadius, shieldEmptyLightSettings.cone.outerMaxRadius, 1 - currentCapacity);
            currentLightSettings.cone.maxIntensity = Mathf.Lerp(sourcelightSettings.cone.maxIntensity, shieldEmptyLightSettings.cone.maxIntensity, 1 - currentCapacity);
            currentLightSettings.cone.color = Color.Lerp(sourcelightSettings.cone.color, shieldEmptyLightSettings.cone.color, 1 - currentCapacity);

            currentLightSettings.point.innerMaxAngle = Mathf.Lerp(sourcelightSettings.point.innerMaxAngle, shieldEmptyLightSettings.point.innerMaxAngle, 1 - currentCapacity);
            currentLightSettings.point.outerMaxAngle = Mathf.Lerp(sourcelightSettings.point.outerMaxAngle, shieldEmptyLightSettings.point.outerMaxAngle, 1 - currentCapacity);
            currentLightSettings.point.innerMaxRadius = Mathf.Lerp(sourcelightSettings.point.innerMaxRadius, shieldEmptyLightSettings.point.innerMaxRadius, 1 - currentCapacity);
            currentLightSettings.point.outerMaxRadius = Mathf.Lerp(sourcelightSettings.point.outerMaxRadius, shieldEmptyLightSettings.point.outerMaxRadius, 1 - currentCapacity);
            currentLightSettings.point.maxIntensity = Mathf.Lerp(sourcelightSettings.point.maxIntensity, shieldEmptyLightSettings.point.maxIntensity, 1 - currentCapacity);
            currentLightSettings.point.color = Color.Lerp(sourcelightSettings.point.color, shieldEmptyLightSettings.point.color, 1 - currentCapacity);


            transitionTiming = 0f;
            shieldActive = true;
            shieldCollider.enabled = true;
            sprite.enabled = true;
            localMaxShieldDuration = maxShieldDuration * currentCapacity / maxCapacity;
            oldLightSettings = currentLightSettings.Clone();
            targetLightSettings = shieldEmptyLightSettings.Clone();
            targetLightSettings.transitionDuration = localMaxShieldDuration;
            elapsedShieldDownTime = 0f;
            
            if ( shieldTriggerFX != null)
            {
                GameObject effect = Instantiate(shieldTriggerFX, transform.position, Quaternion.identity);
                Destroy(effect, 2f);
            }
        }
    }

    private void BeginShieldDegradation()
    {
        oldLightSettings = currentLightSettings.Clone();
        targetLightSettings = shieldEmptyLightSettings.Clone();
        targetLightSettings.transitionDuration = maxShieldDuration;
        transitionTiming = 0f;
    }

    // after burning the shield out?
    private void ReturnToDefault()
    {
        oldLightSettings = currentLightSettings.Clone();
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
        //DeactivateShadow();
        shieldActive = false;
        shieldCollider.enabled = false;
        sprite.enabled = false;
        oldLightSettings = currentLightSettings.Clone();
        targetLightSettings = defaultLightSettings.Clone();
        targetLightSettings.transitionDuration = (1 - currentCapacity) / maxCapacity * (shieldRechargeTime + shieldRechargeInitialDelay);
        transitionTiming = 0f;
        if ( shieldEmptyFX != null )
        {
            GameObject effect = Instantiate(shieldEmptyFX, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    private void UpdateMeters()
    {
        shadowSlider.SetValueWithoutNotify((maxShadowDuration - elapsedShadowTime) / maxShadowDuration);
        shieldSlider.SetValueWithoutNotify(currentCapacity / maxCapacity);
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
        UpdateMeters();
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
                //Debug.Log("Shield degradation beginning");
                //BeginShieldDegradation();
            }

            currentCapacity -= Time.deltaTime / localMaxShieldDuration;
            Vector3 scale = shieldTransform.localScale;
            scale.x = Mathf.Lerp(shieldMinWidth, shieldMaxWidth, currentCapacity);
            shieldTransform.localScale = scale;
            shieldTransform.localPosition = new Vector3(0, Mathf.Lerp(shieldMinDist, shieldMaxDist, currentCapacity), 0);
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
                currentCapacity += Time.deltaTime / (shieldRechargeTime + shieldRechargeInitialDelay);
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
        float percentComplete = (1 - currentCapacity) / maxCapacity;  //transitionTiming / targetLightSettings.transitionDuration;
        if ( !shieldActive )
        {
            percentComplete = currentCapacity / maxCapacity;
        }

        currentLightSettings.cone.innerMaxAngle = Mathf.Lerp(oldLightSettings.cone.innerMaxAngle, targetLightSettings.cone.innerMaxAngle, percentComplete);
        currentLightSettings.cone.outerMaxAngle = Mathf.Lerp(oldLightSettings.cone.outerMaxAngle, targetLightSettings.cone.outerMaxAngle, percentComplete);
        currentLightSettings.cone.innerMaxRadius = Mathf.Lerp(oldLightSettings.cone.innerMaxRadius, targetLightSettings.cone.innerMaxRadius, percentComplete);
        currentLightSettings.cone.outerMaxRadius = Mathf.Lerp(oldLightSettings.cone.outerMaxRadius, targetLightSettings.cone.outerMaxRadius, percentComplete);
        currentLightSettings.cone.maxIntensity = Mathf.Lerp(oldLightSettings.cone.maxIntensity, targetLightSettings.cone.maxIntensity, percentComplete);
        currentLightSettings.cone.color = Color.Lerp(oldLightSettings.cone.color, targetLightSettings.cone.color, percentComplete);

        cone.pointLightInnerAngle = currentLightSettings.cone.innerMaxAngle;
        cone.pointLightOuterAngle = currentLightSettings.cone.outerMaxAngle;
        cone.pointLightInnerRadius = currentLightSettings.cone.innerMaxRadius;
        cone.pointLightOuterRadius = currentLightSettings.cone.outerMaxRadius;
        cone.intensity = currentLightSettings.cone.maxIntensity;
        cone.color = currentLightSettings.cone.color;

        currentLightSettings.point.innerMaxAngle = Mathf.Lerp(oldLightSettings.point.innerMaxAngle, targetLightSettings.point.innerMaxAngle, percentComplete);
        currentLightSettings.point.outerMaxAngle = Mathf.Lerp(oldLightSettings.point.outerMaxAngle, targetLightSettings.point.outerMaxAngle, percentComplete);
        currentLightSettings.point.innerMaxRadius = Mathf.Lerp(oldLightSettings.point.innerMaxRadius, targetLightSettings.point.innerMaxRadius, percentComplete);
        currentLightSettings.point.outerMaxRadius = Mathf.Lerp(oldLightSettings.point.outerMaxRadius, targetLightSettings.point.outerMaxRadius, percentComplete);
        currentLightSettings.point.maxIntensity = Mathf.Lerp(oldLightSettings.point.maxIntensity, targetLightSettings.point.maxIntensity, percentComplete);
        currentLightSettings.point.color = Color.Lerp(oldLightSettings.point.color, targetLightSettings.point.color, percentComplete);

        point.pointLightInnerAngle = currentLightSettings.point.innerMaxAngle;
        point.pointLightOuterAngle = currentLightSettings.point.outerMaxAngle;
        point.pointLightInnerRadius = currentLightSettings.point.innerMaxRadius;
        point.pointLightOuterRadius = currentLightSettings.point.outerMaxRadius;
        point.intensity = currentLightSettings.point.maxIntensity;
        point.color = currentLightSettings.point.color;
    }
}
