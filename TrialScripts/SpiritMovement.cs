using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMovement : MonoBehaviour
{
    public Transform centerPoint;
    public float radius = 5f;
    public float speed = 2f;

    float randomNum;

    public enum SpiritState { STAY, CIRCLE };
    public SpiritState currentState = SpiritState.CIRCLE;

    public void Awake()
    {
        randomNum = Random.Range(0, 10);
    }

    private void Update()
    {
        switch (currentState)
        {
            case SpiritState.STAY:
                stayPut();
                break;

            case SpiritState.CIRCLE:
                circle();
                break;
        }
    }

    private void circle()
    {
        // Calculate the desired position in the circle
        Vector3 desiredPosition = centerPoint.position + centerPoint.rotation * Quaternion.Euler(0f, 0f, (TimeKeeper.getOverworld().getTime() + randomNum) * speed) * new Vector3(radius, 0, 0f);

        // Rotate the object to face its direction of movement
        transform.LookAt(desiredPosition);

        // Move the object towards the desired position
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, speed * TimeKeeper.deltaPlayTime());
    }

    private void stayPut()
    {
        transform.position = Vector3.MoveTowards(transform.position, this.transform.parent.transform.position, speed * TimeKeeper.deltaPlayTime());
    }
}
