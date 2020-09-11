using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public float moveSpeed = 5f;
    public float dashSpeed = 4f;
    public float pointerDist = 5f;
    public Rigidbody2D rigidBody;
    private Camera mainCamera;

    Vector2 movement;
    Vector2 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        var gameObj = GameObject.FindGameObjectWithTag("MainCamera");
        if ( gameObj != null )
        {
            mainCamera = gameObj.GetComponent<Camera>();
        }

        if ( mainCamera == null )
        {
            Debug.LogError("ERROR - NO MAIN CAMERA WITH TAG 'MainCamera' IN SCENE");
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        Vector2 frameTargetPos = (mousePos - rigidBody.position);
        float dist = frameTargetPos.sqrMagnitude;
        if ( pointerDist * pointerDist > dist )
        {
            return;
        }
        frameTargetPos.Normalize();
        frameTargetPos = frameTargetPos * moveSpeed * Time.fixedDeltaTime * 5;
        rigidBody.MovePosition(rigidBody.position + frameTargetPos);
        Vector2 lookDir = mousePos - rigidBody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90.0f;
        rigidBody.rotation = angle;
    }
}
