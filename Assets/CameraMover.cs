using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform[] Locations;
    int locationIndex;


    Vector3 destination;
    public float moveSpeed;

    private void Awake()
    {
        destination = new Vector3(0, 0, 10);
    }
    public void MoveToTheNext()
    {
        destination = Locations[locationIndex].position;
        locationIndex++;
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime * moveSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
