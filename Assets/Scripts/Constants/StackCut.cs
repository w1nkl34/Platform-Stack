using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class StackCut : MonoBehaviour
{
    static GameObject activeObject;
    static GameObject inActiveObject;

    public static GameObject CutStack(Transform currentCube,Transform lastCube)
    {
        currentCube.gameObject.SetActive(false);

        if (Mathf.Abs(currentCube.position.x - lastCube.position.x) >= currentCube.localScale.x)
            return null;

        GenerateTwoStacks(currentCube,lastCube);

        if (activeObject.transform.localScale.x == 0)
        {
            Destroy(activeObject);
            Destroy(inActiveObject);
            return null;
        }

        activeObject.GetComponent<MeshRenderer>().material.SetColor("_Color", currentCube.GetComponent<MeshRenderer>().material.color);
        activeObject.AddComponent<StackController>();
        activeObject.GetComponent<StackController>().StopStack();

        if(inActiveObject.transform.localScale.x != 0)
        {
            inActiveObject.GetComponent<MeshRenderer>().material.SetColor("_Color", currentCube.GetComponent<MeshRenderer>().material.color);
            inActiveObject.AddComponent<Rigidbody>().mass = 100f;
            inActiveObject.SetActive(true);
            Destroy(inActiveObject, 2);
        }
        else
        {
            Destroy(inActiveObject);
        }

        activeObject.SetActive(true);
        return activeObject;
    }

    public static void GenerateTwoStacks(Transform currentCube, Transform lastCube)
    {
        bool leftSide = currentCube.position.x <= lastCube.position.x;
        float objRightSidePosX = Mathf.Round((currentCube.position.x + (currentCube.localScale.x / 2)) * 2) / 2;
        float objLeftSidePosX = Mathf.Round((currentCube.position.x - (currentCube.localScale.x / 2)) * 2) / 2;
        float lastObjLeftSidePosX = Mathf.Round((lastCube.position.x - (lastCube.localScale.x / 2)) * 2) / 2;
        float lastObjRightSidePosX = Mathf.Round((lastCube.position.x + (lastCube.localScale.x / 2)) * 2) / 2;

        GameObject rightSideObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject leftSideObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        rightSideObj.transform.localScale =
            new Vector3(leftSide ? (objRightSidePosX - lastObjLeftSidePosX) : (objRightSidePosX - lastObjRightSidePosX), currentCube.localScale.y, currentCube.localScale.z);
        rightSideObj.transform.position =
            new Vector3(leftSide ? (lastObjLeftSidePosX + objRightSidePosX) / 2 : (objRightSidePosX + lastObjRightSidePosX) / 2, currentCube.position.y, currentCube.position.z);


        leftSideObj.transform.localScale =
            new Vector3(leftSide ? lastObjLeftSidePosX - objLeftSidePosX : lastObjRightSidePosX - objLeftSidePosX, currentCube.localScale.y, currentCube.localScale.z);
        leftSideObj.transform.position =
            new Vector3(leftSide ? (lastObjLeftSidePosX + objLeftSidePosX) / 2 : (lastObjRightSidePosX + objLeftSidePosX) / 2, currentCube.position.y, currentCube.position.z);

        activeObject = leftSide ? rightSideObj : leftSideObj;
        inActiveObject = leftSide ? leftSideObj : rightSideObj;
    }
}