using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public Animator anim;

    public Vector2 direction;
    public float speed;
    public float slowDown;

    public float rotSpeed;

    public float explosionRadius;
    public float timeTillExplosion;
    float explosionTimer;

    public int damage;
    public bool hit;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + Time.deltaTime * rotSpeed));

        transform.position = new Vector2(transform.position.x, transform.position.y) + direction.normalized * speed * Time.deltaTime;

        if (speed >= 0)
        {
            speed -= slowDown * Time.deltaTime;
        }

        explosionTimer -= Time.deltaTime;

        if(explosionTimer > 0 && explosionTimer < 1f)
        {
            anim.SetBool("exploding", true);
        }

        if(explosionTimer <= 0)
        {
            anim.SetTrigger("Boom");

            //damage enemies in a radius
            Collider2D[] explosionHit = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

            foreach (Collider2D col in explosionHit)
            {
                if (col.gameObject.tag == "Enemy")
                {
                    col.gameObject.GetComponent<Enemy>().ChangeHealth(damage);
                }

                if(col.gameObject.tag == "Player")
                {
                    col.gameObject.GetComponent<Player>().TakeDamage(1);
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "PlayerBullet")
        //{
        //    return;
        //}

        ////explode
        //hit = true;
        ////spawn animation/trigger it

        ////damage enemies in a radius
        //Collider2D[] explosionHit = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        //foreach(Collider2D col in explosionHit)
        //{
        //    if(col.gameObject.tag == "Enemy")
        //    {
        //        col.gameObject.GetComponent<Enemy>().ChangeHealth(damage);
        //    }
        //}

    }

    public void SetDirection(Vector2 _newDir)
    {
        direction = _newDir;
        explosionTimer = timeTillExplosion;
    }

    public void SetStats(int _damage, float _speed)
    {
        damage = _damage;
        speed = _speed;
    }

    public void GotHit()
    {
        hit = true;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
