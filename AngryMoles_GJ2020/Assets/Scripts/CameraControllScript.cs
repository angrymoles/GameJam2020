using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllScript : MonoBehaviour
{

    public List<Transform> targets;
    public float smoothtime;

    public Vector3 offset;
    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothtime);


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
