using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualPickUp : MonoBehaviour
{
    public GameObject tileToAcquire;

    public Animator TileToExplode;

    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            AddTileToCollection();

            //trigger animation
            TileToExplode.SetTrigger("Die");

            source.PlayOneShot(Resources.Load("PickupSound") as AudioClip);
        }
    }

    void AddTileToCollection()
    {
        GunManager.AddTileToCollection(tileToAcquire);
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
