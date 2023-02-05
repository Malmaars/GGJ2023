using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MeleeEnemy : Enemy
{
    public Animator animator;
    Vector3 destination;
    Player playerReference;

    public float attackPause;
    float pauseTimer;

    public float deathTimer;

    public float hitStun;
    float stunTimer;

    NavMeshAgent agent;
    public AudioClip slash;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        alive = true;
        playerReference = FindObjectOfType<Player>();

        StartCoroutine(FollowPlayer());
    }

    private void OnEnable()
    {
        alive = true;
        StartCoroutine(FollowPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        pauseTimer -= Time.deltaTime;
        stunTimer -= Time.deltaTime;

        if(playerReference == null)
        {
            playerReference = FindObjectOfType<Player>();
            return;
        }

        //walk towards the player and attack when close

        if (Vector2.Distance(transform.position, destination) < 1f)
        {
            animator.SetBool("Walking", false);
        }

        if (pauseTimer <= 0 && stunTimer <= 0)
        {
            if (Vector2.Distance(transform.position, destination) >= 1f)
                animator.SetBool("Walking", true);

            //Vector2 directionToDestination = (destination - transform.position).normalized;
            //transform.position += new Vector3(directionToDestination.x, directionToDestination.y, 0) * Time.deltaTime * moveSpeed;
        }

        //if player is close, attack
        if(Vector2.Distance(playerReference.transform.position, transform.position) < 1f && pauseTimer <= 0 && stunTimer <= 0)
        {
            animator.SetTrigger("Attack");
            AttackPlayer();
            pauseTimer = attackPause;
        }

        CheckDeath();
    }

    public override void Die()
    {
        base.Die();
        this.gameObject.SetActive(false);
    }

    public override void CheckDeath()
    {
        if (health <= 0)
        {
            alive = false;
            animator.SetTrigger("Die");
            deathTimer -= Time.deltaTime;

            if (deathTimer <= 0)
            {
                animator.SetTrigger("Explode");
            }
        }
    }

    void newDestination()
    {
        if (Vector2.Distance(playerReference.transform.position, transform.position) < 6)
        {
            Vector2 newDes = playerReference.transform.position;
            destination = newDes;
        }

        else
        {
            Vector2 newDes = new Vector2(Random.Range(1f,4f), Random.Range(1f, 4f));
            destination = new Vector2(transform.position.x, transform.position.y) + newDes;
        }
        agent.SetDestination(destination);
    }

    void AttackPlayer()
    {
        playerReference.TakeDamage(damage);
        source.PlayOneShot(slash);
    }

    public override void ChangeHealth(int _change)
    {
        base.ChangeHealth(_change);

        animator.SetTrigger("Hit");

        stunTimer = hitStun;
        //trigger hitstun
    }

    IEnumerator FollowPlayer()
    {
        while (alive)
        {
            if (playerReference != null)
                newDestination();

            yield return new WaitForSeconds(1f);
        }
    }
}
