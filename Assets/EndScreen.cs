using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public SpriteRenderer spr;
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delay -= Time.deltaTime;

        if (delay <= 0)
        {
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, spr.color.a + Time.deltaTime * 0.5f);

            if(spr.color.a >= 1)
            {
                Application.Quit();
            }
        }
    }
}
