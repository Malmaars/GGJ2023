using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGridPlayer : MonoBehaviour
{
    Tileable currentlyHeld;
    public GunGridManager gridManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //check if we click on a moveable tile
            Collider2D[] collisions = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.01f);

            foreach(Collider2D col in collisions)
            {
                if(col.gameObject.GetComponent<Tileable>() != null)
                {
                    //it's a tileable. Grab it
                    Tileable grabbed = col.gameObject.GetComponent<Tileable>();
                    grabbed.OnGrab();
                    currentlyHeld = grabbed;
                    break;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(currentlyHeld != null)
            {
                currentlyHeld.OnRelease();
            }

            currentlyHeld = null;
        }

        if(currentlyHeld != null)
        {
            //move the currentlyheld
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,9);
            currentlyHeld.body.transform.position = mousePos;
        }
    }
}
