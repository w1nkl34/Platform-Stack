using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    private bool toLeft;
    private float maxPosX = 3;
    public bool stop = false;
    private float speed = 6;

    private void Update()
    {
        if (stop || !Constants.gameStarted)
            return;
        ChangeStackPos();
        MoveStack();
    }

    public void StartStack()
    {
        gameObject.SetActive(true);
    }

    public void StopStack()
    {
        stop = true;
    }

    private void ChangeStackPos()
    {
        if(transform.position.x < -maxPosX)
        {
            toLeft = true;
        }
        else if(transform.position.x > maxPosX)
        {
            toLeft = false;
        }
    }

    private void MoveStack()
    {
        if (toLeft)
            transform.position = new Vector3(transform.position.x + Time.deltaTime * speed, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(transform.position.x - Time.deltaTime * speed, transform.position.y, transform.position.z);
    }
}
