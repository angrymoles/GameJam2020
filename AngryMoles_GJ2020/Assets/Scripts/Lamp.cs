using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum LampFXType
{
    LampFX_Activate,
    LampFX_ShieldEmpty,
    LampFX_ShieldHit,
}

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

    [Space(10)]
    public float maxShadowDuration = 2.0f;
    public float shadowRechargeTime = 5f;
    public float shadowRechargeDelay = 1f;
    [Space(10)]
    public float maxShieldDuration = 5f;
    public float shieldRechargeTime = 5f;
    public float shieldRechargeInitialDelay = 1f;

    //shadow vars
    private float shadowCapacity = 1f;
    private float elapsedShadowDownTime = 0f;

    // light vars
    private float transitionTiming = 0f;

    // shield vars
    private float maxCapacity = 1f;
    private float shieldCapacity;
    private float localMaxShieldDuration = 5f;
    private float elapsedShieldUpTime = 0f;
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
    private Player player;
    public float shieldMinDistPercent = 0.5f;
    public GameObject shieldTriggerFX;
    public GameObject shieldEmptyFX;
    public GameObject onCollideProjectileFX;
    [Space(10)]
    public GameObject ShadowForm;

    // Start is called before the first frame update
    void Start()
    {
        shieldCapacity = maxCapacity;
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

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        ShadowForm.SetActive(false);
        shadowCapacity = 1f;
    }

    // in case we want to do something if the shield is hit by bullets
    public void PlayFX( Vector3 position, LampFXType lampFXType )
    {
        GameObject prefabToPlay;
        switch( lampFXType)
        {
            case LampFXType.LampFX_Activate:
                prefabToPlay = shieldTriggerFX;
                break;
            case LampFXType.LampFX_ShieldEmpty:
                prefabToPlay = shieldEmptyFX;
                break;
            default:
                prefabToPlay = onCollideProjectileFX;
                break;
        }

        GameObject effect = Instantiate(prefabToPlay, position, Quaternion.identity);
        Destroy(effect, 2f);
    }

    private bool CanActivate()
    {
        return shieldCapacity > 0 && !shieldActive && !shadowActive && shadowCapacity > 0;
    }

    public void ActivateShadow()
    {
        if ( CanActivate() )
        {
            // enable the dark form
            ShadowForm.SetActive(true);
            // disable the collider on the player
            var spriteRenderer = player.GetComponent<SpriteRenderer>();
            if ( spriteRenderer != null )
            {
                spriteRenderer.enabled = false;
            }
            var playerCollider = player.GetComponent<PolygonCollider2D>();
            if ( playerCollider != null )
            {
              //  playerCollider.enabled = false;
            }            
            oldLightSettings = currentLightSettings.Clone();
            targetLightSettings = shadowLightSettings.Clone();
            transitionTiming = 0f;
            if (shieldActive)
            {
                DeactivateShield();
            }
            shadowActive = true;
            player.gameObject.layer = 14;
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().darkMovespeed;
        }
    }
    
    public void DeactivateShadow()
    {
        if ( shadowActive)
        {
            ShadowForm.SetActive(false);
            var playerCollider = player.GetComponent<PolygonCollider2D>();
            if (playerCollider != null)
            {
                playerCollider.enabled = true;
            }
            var spriteRenderer = player.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }

            oldLightSettings = shieldEmptyLightSettings.Clone();
            targetLightSettings = defaultLightSettings.Clone();
            ReturnToDefault();
            elapsedShadowDownTime = 0f;
            player.gameObject.layer = 9;
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().lightMovespeed;
        }
    }

    public void ActivateShield()
    {
        if ( CanActivate())
        {
            var sourcelightSettings = shieldLightSettings.Clone();
            currentLightSettings.cone.innerMaxAngle = Mathf.Lerp(sourcelightSettings.cone.innerMaxAngle, shieldEmptyLightSettings.cone.innerMaxAngle, 1 - shieldCapacity);
            currentLightSettings.cone.outerMaxAngle = Mathf.Lerp(sourcelightSettings.cone.outerMaxAngle, shieldEmptyLightSettings.cone.outerMaxAngle, 1 - shieldCapacity);
            currentLightSettings.cone.innerMaxRadius = Mathf.Lerp(sourcelightSettings.cone.innerMaxRadius, shieldEmptyLightSettings.cone.innerMaxRadius, 1 - shieldCapacity);
            currentLightSettings.cone.outerMaxRadius = Mathf.Lerp(sourcelightSettings.cone.outerMaxRadius, shieldEmptyLightSettings.cone.outerMaxRadius, 1 - shieldCapacity);
            currentLightSettings.cone.maxIntensity = Mathf.Lerp(sourcelightSettings.cone.maxIntensity, shieldEmptyLightSettings.cone.maxIntensity, 1 - shieldCapacity);
            currentLightSettings.cone.color = Color.Lerp(sourcelightSettings.cone.color, shieldEmptyLightSettings.cone.color, 1 - shieldCapacity);

            currentLightSettings.point.innerMaxAngle = Mathf.Lerp(sourcelightSettings.point.innerMaxAngle, shieldEmptyLightSettings.point.innerMaxAngle, 1 - shieldCapacity);
            currentLightSettings.point.outerMaxAngle = Mathf.Lerp(sourcelightSettings.point.outerMaxAngle, shieldEmptyLightSettings.point.outerMaxAngle, 1 - shieldCapacity);
            currentLightSettings.point.innerMaxRadius = Mathf.Lerp(sourcelightSettings.point.innerMaxRadius, shieldEmptyLightSettings.point.innerMaxRadius, 1 - shieldCapacity);
            currentLightSettings.point.outerMaxRadius = Mathf.Lerp(sourcelightSettings.point.outerMaxRadius, shieldEmptyLightSettings.point.outerMaxRadius, 1 - shieldCapacity);
            currentLightSettings.point.maxIntensity = Mathf.Lerp(sourcelightSettings.point.maxIntensity, shieldEmptyLightSettings.point.maxIntensity, 1 - shieldCapacity);
            currentLightSettings.point.color = Color.Lerp(sourcelightSettings.point.color, shieldEmptyLightSettings.point.color, 1 - shieldCapacity);


            transitionTiming = 0f;
            shieldActive = true;
            shieldCollider.enabled = true;
            sprite.enabled = true;
            localMaxShieldDuration = maxShieldDuration * shieldCapacity / maxCapacity;
            oldLightSettings = currentLightSettings.Clone();
            targetLightSettings = shieldEmptyLightSettings.Clone();
            targetLightSettings.transitionDuration = localMaxShieldDuration;
            elapsedShieldDownTime = 0f;
            
            if ( shieldTriggerFX != null)
            {
                PlayFX(transform.position, LampFXType.LampFX_Activate);
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
        shieldCapacity = 0f;
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
        targetLightSettings.transitionDuration = (1 - shieldCapacity) / maxCapacity * (shieldRechargeTime + shieldRechargeInitialDelay);
        transitionTiming = 0f;
        if ( shieldEmptyFX != null )
        {
            PlayFX(transform.position, LampFXType.LampFX_ShieldEmpty);
        }
    }

    private void UpdateMeters()
    {
        shadowSlider.SetValueWithoutNotify(shadowCapacity / maxCapacity);
        shieldSlider.SetValueWithoutNotify(shieldCapacity / maxCapacity);
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
            shadowCapacity -= (Time.deltaTime / maxShadowDuration);
            if (shadowCapacity <= 0)
            {
                DeactivateShadow();
            }
        }
        else
        {
            if (elapsedShadowDownTime >= shadowRechargeDelay)
            {
                shadowCapacity += (Time.deltaTime / (shadowRechargeTime + shadowRechargeDelay));
            }
            elapsedShadowDownTime += Time.deltaTime;
        }

        if (shieldActive)
        {
            elapsedShieldUpTime += Time.deltaTime;
            shieldCapacity -= Time.deltaTime / localMaxShieldDuration;
            Vector3 scale = shieldTransform.localScale;
            scale.x = Mathf.Lerp(shieldMinWidth, shieldMaxWidth, shieldCapacity);
            shieldTransform.localScale = scale;
            shieldTransform.localPosition = new Vector3(0, Mathf.Lerp(shieldMinDist, shieldMaxDist, shieldCapacity), 0);
            if (shieldCapacity <= 0)
            {
                // shield burned out
                DeactivateShield();
            }
        }
        else
        {
            // regen shield
            if (elapsedShieldDownTime > shieldRechargeInitialDelay)
            {
                shieldCapacity += Time.deltaTime / (shieldRechargeTime + shieldRechargeInitialDelay);
            }
            elapsedShieldDownTime += Time.deltaTime;

            if (shieldCapacity > 1)
            {
                shieldCapacity = 1;
                elapsedShieldUpTime = 0;
            }

            if (shadowCapacity > 1f)
            {
                shadowCapacity = 1f;
            }
        }
    }

    public void UpdateLight()
    {
        float percentComplete = (1 - shieldCapacity) / maxCapacity;  //transitionTiming / targetLightSettings.transitionDuration;
        if ( !shieldActive )
        {
            percentComplete = shieldCapacity / maxCapacity;
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
