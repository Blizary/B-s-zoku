using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 walkInput;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpSpeed = 7.5f;

    // Cached component references
    Rigidbody2D myRigidbody;
    SpriteRenderer mySpriteRenderer;

    private void Start()
    {
        //myCC = GetComponent<CharacterController>();
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FlipSprite();
    }

    // Function to swap sprite direction based on look direction
    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            if (Mathf.Sign(myRigidbody.velocity.x) < Mathf.Epsilon)
            {
                mySpriteRenderer.flipX = true;
            }
            else mySpriteRenderer.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        walkInput = context.ReadValue<Vector2>(); ; // value is between -1 to +1
    }

    private void ApplyMovement()
    {
        Vector2 playerVelocity = new Vector2(walkInput.x * movementSpeed, walkInput.y * movementSpeed);
        myRigidbody.velocity = playerVelocity;
    }
}
