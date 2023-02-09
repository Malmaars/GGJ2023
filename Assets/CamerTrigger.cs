using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerTrigger : MonoBehaviour
{
    public CameraMover mover;

    bool up;

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
            if (!up)
                mover.MoveToTheNext();

            if (up)
            {
                Debug.Log("Move down");
                mover.MoveToThePrevious();
            }
            up = !up;
        }
    }
}
