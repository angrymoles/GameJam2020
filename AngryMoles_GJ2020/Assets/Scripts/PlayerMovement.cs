using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rigidBody;
    public Camera mainCamera;

    Vector2 movement;
    Vector2 mousePos;
    // Start is called before the first frame update
    void Start()
    {
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
        rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - rigidBody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90.0f;
        rigidBody.rotation = angle;
    }
}
