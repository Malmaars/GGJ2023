using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletEnemy : Enemy
{

    ShootableType shotType;

    Vector3 destination;
    public Animator animator;


    Player playerReference;

    public float attackPause;
    float pauseTimer;

    public float hitStun;
    float stunTimer;


    public AudioClip ShotSound;
    public float deathTimer;


    public Vector2 min, max;

    public Transform barrel;

    Player playerRef;

    public Transform myGun;
    NavMeshAgent agent;

    public SpriteRenderer rend;
    // Start is called before the first frame update
    void Awake()
    {
        playerRef = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();

        alive = true;
        StartCoroutine(FollowTarget());

    }

    private void OnEnable()
    {
        alive = true;
        StartCoroutine(FollowTarget());
    }

    void newDestination()
    {
        Vector2 newDes = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        destination = newDes;
        agent.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeath();
        if (!alive)
            return;

        pauseTimer -= Time.deltaTime;
        stunTimer -= Time.deltaTime;

        min = Camera.main.transform.position + new Vector3(-13, -7.5f);
        max = Camera.main.transform.position + new Vector3(13, 7.5f);

        if (playerReference == null)
        {
            playerReference = FindObjectOfType<Player>();
            return;
        }
        //shoot at the player and walk to random locations on the map
        if (Vector2.Distance(transform.position, destination) < 1f)
        {
            animator.SetBool("Walking", false);
        }

        if (stunTimer <= 0)
        {
            if (Vector2.Distance(transform.position, destination) >= 1f)
                animator.SetBool("Walking", true);

            //Vector2 directionToDestination = (destination - transform.position).normalized;
            //transform.position += new Vector3(directionToDestination.x, directionToDestination.y, 0) * Time.deltaTime * moveSpeed;
        }

        if (pauseTimer < 0) {
            //animator.SetTrigger("Attack");
            AttackPlayer();
            pauseTimer = attackPause;
        }

        Vector2 directionToPlayer = (playerRef.transform.position - transform.position).normalized;
        myGun.up = directionToPlayer;
        myGun.transform.position = transform.position + new Vector3(directionToPlayer.x, directionToPlayer.y) * 0.6f;
    }

    public void AttackPlayer()
    {
        //shoot at the player
        if (rend.isVisible)
        {
            source.PlayOneShot(ShotSound);
        }

        Shoot();
    }

    public override void CheckDeath()
    {
        if(health <= 0)
        {
            alive = false;
            animator.SetTrigger("Die");
            deathTimer -= Time.deltaTime;

            if(deathTimer <= 0)
            {
                animator.SetTrigger("Explode");
            }
        }
    }

    public override void ChangeHealth(int _change)
    {
        base.ChangeHealth(_change);
        animator.SetTrigger("Hit");
    }

    public void Shoot()
    {
        switch (shotType)
        {
            case ShootableType.bullet:
                EnemyBullet toShoot = EnemyHandler.RequestBullet();
                toShoot.MakeBullet(barrel.position, (playerRef.transform.position - transform.position).normalized);

                Debug.Log(rend.isVisible);
                //evil bullet
                break;
        }

        
        
    }

    void AimBarrel()
    {
        //turn the barrel towards the player
    }

    public override void Die()
    {
        base.Die();
        this.gameObject.SetActive(false);
    }

    IEnumerator FollowTarget()
    {
        while (alive)
        {
            newDestination();

            yield return new WaitForSeconds(3f);
        }
    }
}
