using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private CameraController cameraController;
    public float speed = 1;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

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
        if (!Constants.gameGenerated)
            return 2;
        return Mathf.Max(Vector3.Distance(Constants.allStackControllers[Constants.currentStack - 1].transform.position, transform.position),1);
    }

    public void WonGameBegin(GameManager gameManager)
    {
        LeanTween.move(gameObject,
         new Vector3(0,
         transform.position.y, GameObject.FindGameObjectWithTag("finishStack").transform.position.z), 0.5f).setOnComplete(()  =>
         {
             ChangeToDance(true);
             cameraController.RotateCameraAroundCharacter(this,gameManager);
         });
    }

    public void ToNextLevel(GameManager gameManager)
    {
        ChangeToDance(false);
        gameManager.NextLevel();
    }

    public void ChangeToDance(bool dance)
    {
        anim.SetBool("dance", dance);
    }

    public void UseGravity(bool use)
    {
        rb.useGravity = use;
    }

}
