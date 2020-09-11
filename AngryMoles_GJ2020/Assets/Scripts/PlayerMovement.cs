using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public float moveSpeed = 5f;
   
    public float pointerDist = 5f;

    [SerializeField]
    public Rigidbody2D rigidBodyLamp;
    [SerializeField]
    public Rigidbody2D rigidBodyPlayer;
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
        Vector2 frameTargetPos = (mousePos - rigidBodyPlayer.position);
        float dist = frameTargetPos.sqrMagnitude;
        if ( pointerDist * pointerDist > dist )
        {
            return;
        }
        frameTargetPos.Normalize();
        frameTargetPos = frameTargetPos * moveSpeed * Time.fixedDeltaTime * 5;
        rigidBodyPlayer.MovePosition(rigidBodyPlayer.position + frameTargetPos);
        rigidBodyLamp.MovePosition(rigidBodyPlayer.position + frameTargetPos);
        Vector2 lookDir = mousePos - rigidBodyLamp.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90.0f;
        rigidBodyLamp.rotation = angle;
    }
}
