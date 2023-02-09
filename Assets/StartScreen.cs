using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public SpriteRenderer spr;
    public AudioClip startSound;
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source.PlayOneShot(startSound);
        GunManager.UpdateBulletToShoot(new Shootable[0]);
    }

    // Update is called once per frame
    void Update()
    {
        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, spr.color.a - Time.deltaTime * 0.5f);

        if (spr.color.a <= 0)
        {
            Destroy(this);
        }
    }
}
