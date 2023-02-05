using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environmental : MonoBehaviour
{
    public Transform player;

    public SpriteRenderer rend;
    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position.y < transform.position.y + offset)
        {
            rend.sortingOrder = -1;
        }

        else
        {
            rend.sortingOrder = 1;
        }
    }
}
