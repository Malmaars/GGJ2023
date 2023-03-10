using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    public GameObject body;

    public int baseHealth;
    public int health;

    public float moveSpeed;
    public int damage;
    public bool active { get; set; }

    public bool alive;

    public AudioSource source;

    public void OnEnableObject()
    {
        alive = true;
        health = baseHealth;
    }
    public void OnDisableObject()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        health = baseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //make it walk and attack the player

    }

    public virtual void CheckDeath()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public void ExplosionSound()
    {
        source.PlayOneShot(Resources.Load("ExplosionSound") as AudioClip);
    }

    public virtual void ChangeHealth(int _change)
    {
        health -= _change;
    }

    public virtual void Die()
    {
        alive = false;
    }
}
