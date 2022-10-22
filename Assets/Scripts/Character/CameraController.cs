using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineFreeLook cmCharacterFollow;
    CinemachineFreeLook cmGameFollow;
    private GameObject cameraLookAt;


    private void Awake()
    {
        cameraLookAt = GameObject.FindGameObjectWithTag("cameraLookAt");
        cmCharacterFollow = GameObject.FindGameObjectWithTag("cmCharacter").GetComponent<CinemachineFreeLook>();
        cmGameFollow = GameObject.FindGameObjectWithTag("cmGameplay").GetComponent<CinemachineFreeLook>();
    }


    public void RotateCameraAroundCharacter(MyCharacterController myCharacterController,GameManager gameManager)
    {
        LeanTween.value(-180f, 180f, 3f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float val) =>
        {
            cmCharacterFollow.m_Heading.m_Bias = val;
        }).setOnComplete(() =>
        {
            CameraChange(false);
            myCharacterController.ToNextLevel(gameManager);
            ChangeCameraLookAtPositionToEnd();
        });
    }

    public void CameraChange(bool end)
    {
        cmCharacterFollow.gameObject.SetActive(end);
        cmGameFollow.gameObject.SetActive(!end);
    }



    public void ChangeCameraLookAtPositionToEnd()
    {
        cameraLookAt.transform.position = new Vector3(0,
         0, GameObject.FindGameObjectWithTag("finishStack").transform.position.z);
    }


    public void ChangeCameraLookAtPosition()
    {
        LeanTween.move(cameraLookAt,
            new Vector3(Constants.allStackControllers[Constants.currentStack - 1].transform.position.x,
            cameraLookAt.transform.position.y, Constants.allStackControllers[Constants.currentStack - 1].transform.position.z + 1f), 0.5f);
    }

}
