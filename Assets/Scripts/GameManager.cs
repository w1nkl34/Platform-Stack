using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Material finishMeshMaterial;
    public int gameLength;
    public GameObject cameraLookAt;


    private void Start()
    {
        GenerateGame();
    }

    private void GenerateGame()
    {
        ResetGame();
        GenerateLevel();
        Constants.gameGenerated = true;
    }

    private void ResetGame()
    {
        Constants.allStackControllers = new List<StackController>();
        Constants.gameStarted = false;
        Constants.gameWon = false;
    }

    private void GenerateLevel()
    {
        Constants.allStackControllers = LevelGeneration.GenerateLevel(gameLength, finishMeshMaterial);
    }

    private void StartGame()
    {
        Constants.allStackControllers[Constants.currentStack].StartStack();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Constants.gameStarted)
            {
                ClickAfterStart();
            }
            if (Constants.gameGenerated && !Constants.gameStarted)
            {
                PlayerStartGame();
                ChangeCameraLookAtPosition();
            }
        }
    }

    private void ClickAfterStart()
    {
        Constants.currentStack++;
        if (Constants.currentStack == gameLength)
        {
            Constants.gameWon = true;
            Constants.gameStarted = false;
            Constants.gameGenerated = false;
            return;
        }

        StackController nextStack = Constants.allStackControllers[Constants.currentStack];
        StackController currentStack = Constants.allStackControllers[Constants.currentStack - 1];
        StackController lastStack = Constants.allStackControllers[Constants.currentStack - 2];

        currentStack.StopStack();
        GameObject cutStack = StackCut.CutStack(currentStack.transform, lastStack.transform);

        if (cutStack != null)
        {
            Constants.allStackControllers[Constants.currentStack - 1] = cutStack.GetComponent<StackController>();
            Constants.allStackControllers[Constants.currentStack].transform.localScale = cutStack.transform.localScale;
            ChangeCameraLookAtPosition();
            nextStack.StartStack();
        }
        else
        {
            currentStack.gameObject.SetActive(true);
            currentStack.StopStack();
            currentStack.gameObject.AddComponent<Rigidbody>().mass = 100f;
        }
    }

    private void PlayerStartGame()
    {
        StartGame();
        Constants.gameStarted = true;
    }

    private void ChangeCameraLookAtPosition()
    {
        LeanTween.move(cameraLookAt,
            new Vector3(Constants.allStackControllers[Constants.currentStack - 1].transform.position.x,
            cameraLookAt.transform.position.y, Constants.allStackControllers[Constants.currentStack - 1].transform.position.z + 1f), 0.5f);
    }

}
