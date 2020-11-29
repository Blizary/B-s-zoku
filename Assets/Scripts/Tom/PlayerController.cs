using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 walkInput;
    [SerializeField] float movementSpeed = 5f;
    public bool frozen = false;

    // Cached component references
    Rigidbody2D myRigidbody;
    SpriteRenderer mySpriteRenderer;
    public GameObject myAttackRange;
    BoxCollider2D myAttackRangeCollider;
    PlayerAttackController myPAC;
    Animator myAnimator;

    /// COMBAT SYSTEM CODE
    /// This system is a first test for the player combat
    /// Once the first attack is done, the player will have a certain amount of time
    /// to perform attack2, if he does so succesfully he will again have a certain amount
    /// of time to perform the combo. 
    /// For now the system just uses booleans and coroutines but this might change in the future.
    // Combat variables
    [SerializeField] bool attack1WasPerformed = false;
    [SerializeField] bool attack2WasPerformed = false;
    [SerializeField] float timeBetweenAttack1And2 = 0.5f;
    [SerializeField] float timeBetweenAttack2AndCombo = 3f;
    [SerializeField] bool canDash = true;


    private void Start()
    {
        //myCC = GetComponent<CharacterController>();
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        myAttackRangeCollider = myAttackRange.GetComponent<BoxCollider2D>();
        myPAC = myAttackRange.GetComponent<PlayerAttackController>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Code for run animation
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed || playerHasVerticalSpeed)
        {
            myAnimator.SetBool("IsRunning", true);
        }
        else myAnimator.SetBool("IsRunning", false);

        if (frozen) return;

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
                myAttackRangeCollider.offset = new Vector2(-0.75f, 0);
            }
            else
            {
                mySpriteRenderer.flipX = false;
                myAttackRangeCollider.offset = new Vector2(0.75f, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!frozen) ApplyMovement();
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

    public void OnAttack1(InputAction.CallbackContext context)
    {
        if (context.performed && !attack1WasPerformed && !frozen)
        {
            myPAC.ExecuteAttack1();
            attack1WasPerformed = true;
            StartCoroutine(Attack1PerformedReset());
        }
    }

    public void OnAttack2(InputAction.CallbackContext context)
    {
        if (context.performed && attack1WasPerformed && !attack2WasPerformed && !frozen)
        {
            myPAC.ExecuteAttack2();
            attack2WasPerformed = true;
            StartCoroutine(Attack2PerformedReset());
        }
    }

    public void OnCombo(InputAction.CallbackContext context)
    {
        if (context.performed && attack2WasPerformed && !frozen)
        {
            myPAC.ExecuteCombo();
            attack2WasPerformed = false; // else combo can be spammed. (blanket fix)
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && !frozen)
        {
            Debug.Log("Dash");
            myAnimator.SetTrigger("Dash");

            // Dash Code here

            StartCoroutine(ResetDash());
        }
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<BoxCollider2D>().isTrigger = false;
       
        canDash = true;
    }

    IEnumerator Attack1PerformedReset()
    {
        yield return new WaitForSeconds(timeBetweenAttack1And2);
        attack1WasPerformed = false;
    }
    IEnumerator Attack2PerformedReset()
    {
        yield return new WaitForSeconds(timeBetweenAttack2AndCombo);
        attack2WasPerformed = false;
    }

    public void SetFreezeState(bool state)
    {
        frozen = state;

        if (frozen)
        {
            myRigidbody.velocity = new Vector2(0,0);
        }
    }
}
