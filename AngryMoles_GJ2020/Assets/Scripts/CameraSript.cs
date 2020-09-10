using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSript : MonoBehaviour
{
    public List<Transform> targets;
    private Vector3 velocity;
    public float smoothtime=.5f;
    public Vector3 offset;
    

    public void LateUpdate()
    {
        Vector3 centerPoints = GetCenterPoint();
        Vector3 newPosition = centerPoints + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition,ref velocity,smoothtime);
    }

    Vector3 GetCenterPoint()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
}
