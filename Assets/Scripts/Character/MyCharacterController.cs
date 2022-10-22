using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    public bool startCharacter;
    public float speed = 1;

    private void Update()
    {
        if (!Constants.gameStarted)
            return;
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        transform.position = new Vector3(
            transform.position.x, transform.position.y, transform.position.z + Time.deltaTime *
            speed * CalculateDistanceToCurrentStack());

        transform.position = Vector3.Lerp(transform.position,
         new Vector3(Constants.allStackControllers[Constants.currentStack - 1].transform.position.x, transform.position.y, transform.position.z)
         , speed * Time.deltaTime);
    }

    private float CalculateDistanceToCurrentStack()
    {
        return Mathf.Max(Vector3.Distance(Constants.allStackControllers[Constants.currentStack - 1].transform.position, transform.position),1);
    }
}
