using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Material finishMeshMaterial;
    public int gameLength;
    public int gameIncreaseByLevel = 2;
    private MyCharacterController myCharacterController;
    private CameraController cameraController;


    private void Awake()
    {
        myCharacterController = FindObjectOfType<MyCharacterController>();
        cameraController = myCharacterController.GetComponent<CameraController>();
    }

    private void Start()
    {
        GenerateGame();
    }

    public void NextLevel()
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
        Constants.currentStack = 1;
        Destroy(GameObject.FindGameObjectWithTag("finishStack"));
        Destroy(GameObject.FindGameObjectWithTag("startStack"));
        myCharacterController.UseGravity(false);
        cameraController.CameraChange(false);
        Constants.gameStarted = false;
        Constants.gameWon = false;
    }

    private void GenerateLevel()
    {
        Constants.allStackControllers = LevelGeneration.GenerateLevel(gameLength + gameIncreaseByLevel * Constants.level, finishMeshMaterial);
    }

    private void WonGameBegin()
    {
        Constants.lastStack = 0;
        for (int i = 1; i<= Constants.level; i++)
        {
            Constants.lastStack += gameLength + (i * gameIncreaseByLevel) + 1; 
        }
        Constants.level++;
        Constants.gameWon = true;
        Constants.gameStarted = false;
        Constants.gameGenerated = false;
        cameraController.CameraChange(true);
        myCharacterController.WonGameBegin(this);
    }

    private void LostGame(bool fromUpdate)
    {
        int beforeStack = fromUpdate ? 0 : 1;
        Constants.allStackControllers[Constants.currentStack - beforeStack].gameObject.SetActive(true);
        Constants.allStackControllers[Constants.currentStack - beforeStack].StopStack();
        Constants.allStackControllers[Constants.currentStack - beforeStack].gameObject.AddComponent<Rigidbody>().mass = 100f;
        Constants.gameGenerated = false;
        myCharacterController.UseGravity(true);
        StartCoroutine(LevelRepeat());
    }

    private IEnumerator LevelRepeat()
    {
        yield return new WaitForSeconds(2f);
        RefreshGame();
        GenerateGame();
        myCharacterController.ResetPosition();
    }

    private void RefreshGame()
    {
        foreach(var item in Constants.allStackControllers)
        {
            Destroy(item.gameObject);
        }
    }

    private void CheckCharacterDrop()
    {
        if(myCharacterController.transform.position.z >= Constants.allStackControllers[Constants.currentStack].transform.position.z)
        {
            LostGame(true);
        }
    }

    private void Update()
    {
        if (Constants.gameWon)
            return;

        if (Constants.gameGenerated && Constants.gameStarted)
        CheckCharacterDrop();
        if (Input.GetMouseButtonDown(0))
        {
            if (Constants.gameStarted && Constants.gameGenerated)
            {
                ClickAfterStart();
            }
            if (Constants.gameGenerated && !Constants.gameStarted)
            {
                PlayerStartGame();
                cameraController.ChangeCameraLookAtPosition();
            }
        }
    }

    private void ClickAfterStart()
    {
        NextStack();
    }

    private void NextStack()
    {
        Constants.currentStack++;
        StackController currentStack = Constants.allStackControllers[Constants.currentStack - 1];
        StackController lastStack = Constants.allStackControllers[Constants.currentStack - 2];
        currentStack.StopStack();
        GameObject cutStack = StackCut.CutStack(currentStack.transform, lastStack.transform);

        if (cutStack != null)
        {
            Constants.allStackControllers[Constants.currentStack - 1] = cutStack.GetComponent<StackController>();
            cameraController.ChangeCameraLookAtPosition();
            if (Constants.currentStack == gameLength + gameIncreaseByLevel * Constants.level)
            {
                WonGameBegin();
                return;
            }
            else
            {
                Constants.allStackControllers[Constants.currentStack].transform.localScale = cutStack.transform.localScale;
                StackController nextStack = Constants.allStackControllers[Constants.currentStack];
                nextStack.StartStack();
            }
        }
        else
        {
            LostGame(false);
        }
    }

    private void PlayerStartGame()
    {
        StartGame();
        Constants.gameStarted = true;
    }


    private void StartGame()
    {
        Constants.allStackControllers[Constants.currentStack].StartStack();
    }

}
