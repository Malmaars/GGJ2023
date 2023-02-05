using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Vector2 direction;
    public float speed;

    public int damage;
    public bool hit;
    bool exploding;
    public float explosionRadius;

    Animator anim;

    public AudioSource source;

    // Update is called once per frame
    void Update()
    {
        transform.up = (new Vector2(transform.position.x, transform.position.y) + direction.normalized * speed * Time.deltaTime) - new Vector2(transform.position.x, transform.position.y);
        
        if(!exploding)
        transform.position = new Vector2(transform.position.x, transform.position.y) + direction.normalized * speed * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet" || collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "Player")
        {
            return;
        }

        Collider2D[] explosionHit = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in explosionHit)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<Enemy>().ChangeHealth(damage);
            }

            if (col.gameObject.tag == "Player")
            {
                col.gameObject.GetComponent<Player>().TakeDamage(1);
            }
        }

        Debug.Log("HIT");
        exploding = true;
        GetComponent<Animator>().SetTrigger("Die");
    }

    public void GetHit()
    {
        hit = true;
    }

    public void SetDirection(Vector2 _newDir)
    {
        direction = _newDir;
    }

    public void ExplosionSound()
    {
        //source.PlayOneShot(Resources.Load("ExplosionSound") as AudioClip);
    }

    public void SetStats(int _damage, float _speed)
    {
        exploding = false;
        damage = _damage;
        speed = _speed;
    }
}
