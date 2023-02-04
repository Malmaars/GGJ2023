using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public Animator animator;
    Vector3 destination;
    GameObject playerReference;

    public float attackPause;
    float pauseTimer;

    public float hitStun;
    float stunTimer;
    // Start is called before the first frame update
    void Awake()
    {
        playerReference = FindObjectOfType<Player>().gameObject;

        StartCoroutine(FollowPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        pauseTimer -= Time.deltaTime;
        stunTimer -= Time.deltaTime;

        if(playerReference == null)
        {
            playerReference = FindObjectOfType<Player>().gameObject;
            return;
        }

        //walk towards the player and attack when close

        if (Vector2.Distance(transform.position, destination) < 1f)
        {
            animator.SetBool("Walking", false);
        }

        else if (pauseTimer <= 0 && stunTimer <= 0)
        {
            animator.SetBool("Walking", true);
            Vector2 directionToDestination = (destination - transform.position).normalized;
            transform.position += new Vector3(directionToDestination.x, directionToDestination.y, 0) * Time.deltaTime * moveSpeed;
        }

        //if player is close, attack
        if(Vector2.Distance(playerReference.transform.position, transform.position) < 1f && pauseTimer <= 0 && stunTimer <= 0)
        {
            animator.SetTrigger("Attack");
            pauseTimer = attackPause;
        }

        CheckDeath();
    }

    void newDestination()
    {
        Vector2 newDes = playerReference.transform.position;
        destination = newDes;
    }

    void AttackPlayer()
    {

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
