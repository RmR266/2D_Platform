using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;

    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;
    Vector2 moveInput;

    TouchingDIrections touchingDirections;
    Damageable damageable;

    

    public float CurrentMoveSpeed
    {
        get
        {
            if(canMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
            {
                
                    if (touchingDirections.IsGrounded)
                
                {
                if (IsRunning)
                {
                    return runSpeed;
                }
                else
                {
                    return walkSpeed;
                }
            }
            else
            {
                // 0 is the value for idle i.e no movement 
                return airWalkSpeed;
            }
            }
            else
            {
                return 0;
            }
            
        } else 
        {
            // Movement Locked 
            return 0;
        }
    }

     }
            

    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isRunning = false;

    public bool _isFacingRight = true;

    public bool IsFacingRight { get { return _isFacingRight; }  set{
        if(_isFacingRight != value)
        {
            // Flip the local scale to make the player face the opp direction 
            transform.localScale *= new Vector2(-1, 1);
        }
    } }

    public bool canMove { get
    {
        return animator.GetBool(AnimationStrings.canMove);

    }}

    public bool IsAlive {
        get{
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }



    public bool IsMoving { get {
        return _isMoving; 
    }
      set
     {
        _isMoving = value;
        animator.SetBool(AnimationStrings.isMoving , value);
    } }

    public bool IsRunning {
         get 
         {
        return _isRunning;
    }
    set{
        _isRunning = value;
        animator.SetBool(AnimationStrings.isRunning , value);
    }
    }

    public bool LockVelocity { get {
        return animator.GetBool(AnimationStrings.lockVelocity);

    }
    set
    {
        animator.SetBool(AnimationStrings.lockVelocity, value);
    } }

    Rigidbody2D rb;

    Animator animator;

    private void  Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDIrections>();
        damageable = GetComponent<Damageable>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        {
     if(moveInput.x < 0)
     {
         transform.localScale = new Vector3(-1, 1, 1);
     }else if(moveInput.x > 0)
     {
         transform.localScale = Vector3.one;
     }
 }
        
    }
    
    private void FixedUpdate()
    {
        if(!damageable.LockVelocity)
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }
    public void OnMove(InputAction.CallbackContext context)
    { 
        moveInput = context.ReadValue<Vector2>();

        if(IsAlive)
        {
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);

        } else
        {
            IsMoving = false;
        }
        
        
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            //face the right
            IsFacingRight = true;
        } else if (moveInput.x < 0 && IsFacingRight)
        {
            //face the left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            IsRunning = true;   
         } else if(context.canceled)
         {
            IsRunning = false;
         }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && touchingDirections.IsGrounded && canMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }


        }

    public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                animator.SetTrigger(AnimationStrings.attackTrigger);
            }
        }

         public void OnRangedAttack(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
            }
        }


        public void OnHit(int damaage, Vector2 knockback)
        {
            
            rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        }



    }

    

    

    
