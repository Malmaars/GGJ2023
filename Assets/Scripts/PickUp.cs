using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    public float moveUpSpeed;
    public float moveDownSpeed;

    bool goingUp;

    public Transform upPos, downPos;

    Vector2 actualUp, actualDown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp)
        {
            transform.position = Vector2.Lerp(transform.position, upPos.position, moveUpSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, upPos.position) < 0.2f)
            {
                goingUp = false;
            }
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, downPos.position, moveDownSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, downPos.position) < 0.2f)
            {
                goingUp = true;
            }
        }
    }

    public void Die()
    {
        Destroy(transform.parent.gameObject);
    }
}
