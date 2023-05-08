using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private bool spawned;

    public Transform playerTransform;

    public CharacterController controller;
    public Transform cam;
    public Animator animator;

    Vector3 moveDirection;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;

    public Vector3 playerVelocity;
    public float jumpHeight = 5f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public bool grounded;
    private bool jumping;

    [SerializeField] StaffManager staff;
    public bool successfulHit;
    public String currentTarget;

    private bool hasKey1, hasKey2;

    private float currentHealth, maxHealth;
    private bool dead;

    [SerializeField] private AudioClip staffSwing;
    [SerializeField] private AudioClip staffHit;
    [SerializeField] private AudioClip ellenHurt;
    [SerializeField] private AudioClip ellenDeath;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = 100;
        jumping = false;
        ChangeHealth(0);
        dead = false;
        hasKey1 = false;
        hasKey2 = false;
        successfulHit = false;
        currentTarget = "";
        GameManager.instance.player = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawned && !dead)
        {
            GroundCheck();
            MovePlayer();

            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log("E button pressed, do attack");
                animator.SetTrigger("Attack");
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                //Test Key: Adds 5 health
                ChangeHealth(5);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                //Test Key: Removes 5 health
                ChangeHealth(-5);
            }
        }
    }

    private void GroundCheck()
    {
        //Check if the player is touching ground
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (grounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
            
            animator.SetBool("isGrounded", true);
            animator.SetBool("isFalling", false);
        }

        if (grounded == false && jumping == false)
        {
            animator.SetBool("isGrounded", false);
            animator.SetBool("isFalling", true);
        }
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection * speed * Time.deltaTime);
            animator.SetFloat("Speed", 1);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        if (grounded && Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("jumping");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void EndCheatPressed(Vector3 newPos)
    {
        print("Cheat used");
        hasKey1 = true;
        hasKey2 = true;
        playerTransform.position = newPos;
        Physics.SyncTransforms();
    }

    public void PlayHitSound()
    {
        if (!dead)
        {
            AudioSource.PlayClipAtPoint(ellenHurt, playerTransform.position, 1);
        }
        
    }

    public void ChangeHealth(int value)
    {
        currentHealth += value;
        
        if(currentHealth <= 0 && dead == false)
        {
            dead = true;
            AudioSource.PlayClipAtPoint(ellenDeath, playerTransform.position, 1);
            animator.SetBool("isDead", true);
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        GUIManager.instance.UpdateHealthBar((float)currentHealth / maxHealth);
    }

    public void CheckForHit()
    {
        if (staff.isHittingEnemy)
        {
            successfulHit = true;
            AudioSource.PlayClipAtPoint(staffHit, playerTransform.position, 1);
            staff.GetEnemyHit().TakeDamage(currentTarget);
        } else
        {
            successfulHit = false;
            AudioSource.PlayClipAtPoint(staffSwing, playerTransform.position, 1);
        }
    }

    public void SetCurrentTarget(string targetName)
    {
        currentTarget = targetName;
    }

    public void PickUpKey(string keyName)
    {
        if (keyName.Equals("Key1"))
        {
            //print("Player has picked up key 1");
            hasKey1 = true;
        } else if (keyName.Equals("Key2"))
        {
            //print("Player has picked up key 2");
            hasKey2 = true;
        }
    }

    public bool HasBothKeys()
    {
        return (hasKey1 && hasKey2);
    }

    public void Spawn()
    {
        spawned = true;
    }

    public void Death()
    {
        StartCoroutine(GameManager.instance.Lose());
    }
}
