using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperManager : MonoBehaviour
{
    [SerializeField] private Transform trans;

    [SerializeField] private Animator animator;
    
    [SerializeField] private float moveSpeed;
    private float rotationSpeed;

    private Transform target;
    private float radiusOfSatisfaction;
    private float radiusOfDetection;
    private float attackHitRange;

    private int enemyHealth;
    private bool dead;

    private AudioSource bite;
    [SerializeField] private AudioClip death;

    // Start is called before the first frame update
    void Start()
    {
        SetTarget(GameManager.instance.player.playerTransform);
        rotationSpeed = 5f;
        radiusOfSatisfaction = 2f;
        radiusOfDetection = 20f;
        attackHitRange = 1.5f;
        enemyHealth = 10;
        bite = GetComponent<AudioSource>();
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            PerformActions();
            FaceTarget();
        }
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    private void PerformActions()
    {
        //No target or target out of aggro range = stand still, play idle animation
        if(target == null || Vector3.Distance(trans.position, target.position) > radiusOfDetection)
        {
            if (bite.isPlaying)
            {
                bite.Stop();
            }
            animator.SetFloat("Speed", 0f);
            animator.ResetTrigger("Attack");
        }
        //Close to target = attack
        else if(Vector3.Distance(trans.position, target.position) < radiusOfSatisfaction)
        {
            animator.SetFloat("Speed", 0f);
            if (!bite.isPlaying)
            {
                bite.Play();
            }
            animator.SetTrigger("Attack");
        }
        //Target in aggro range but too far to hit = move towards target
        else
        {
            if (bite.isPlaying)
            {
                bite.Stop();
            }
            animator.SetFloat("Speed", 1f);
            animator.ResetTrigger("Attack");
        }
    }

    private void FaceTarget()
    {
        if(target == null) {
            trans.rotation = Quaternion.Lerp(trans.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        } else {
            Vector3 towardsTarget = target.position - trans.position;
            towardsTarget.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(towardsTarget);
            trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void CheckForHit()
    {
        //Check if the attack is in range to hit the player
        if (Vector3.Distance(trans.position, target.position) <= attackHitRange)
        {
            GameManager.instance.player.PlayHitSound();
            GameManager.instance.player.ChangeHealth(-5);
        }
        
    }

    public void TakeDamage(string name)
    {
        //Deal damage to the hit enemy
        GameManager.instance.player.successfulHit = false;
        enemyHealth -= 4;

        if (enemyHealth <= 0)
        {
            if (bite.isPlaying)
            {
                bite.Stop();
            }
            //Play death animation and destroy enemy object
            dead = true;
            AudioSource.PlayClipAtPoint(death, trans.position, 1);
            animator.SetBool("isDead", true);
        }
    }

    public int GetEnemyHealth()
    {
        return enemyHealth;
    }

    public void Death()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * 8 * Time.deltaTime);
        
        StartCoroutine(despawn());
    }

    IEnumerator despawn()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
