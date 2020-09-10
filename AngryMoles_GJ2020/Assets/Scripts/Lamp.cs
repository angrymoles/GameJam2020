using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public float innerMaxAngle = 60.0f;
    public float outerMaxAngle = 120.0f;
    public float innerMaxRadius = 1.0f;
    public float outerMaxRadius = 5.0f;
    public float maxIntensity = 2.0f;
    public float maxBarrierScale = 10.0f;
    public float speed = 0.1f;
    public Light2D lamp;

    public float powerRate = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        powerRate = 1.0f;
    }

    void Update()
    {
        UpdateLight();
    }

    public void SetPower(float value)
    {
        powerRate = powerRate + speed * value;

        if (powerRate > 1.0f)
        {
            powerRate = 1.0f;
        }

        if (powerRate < 0.0f)
        {
            powerRate = 0.0f;
        }
    }

    public void UpdateLight()
    {
        lamp.pointLightInnerAngle = innerMaxAngle * powerRate;
        lamp.pointLightOuterAngle = outerMaxAngle * powerRate;
        lamp.pointLightInnerRadius = innerMaxRadius * powerRate;
        lamp.pointLightOuterRadius = outerMaxRadius * powerRate;
        lamp.intensity = maxIntensity * powerRate;
    }
}
