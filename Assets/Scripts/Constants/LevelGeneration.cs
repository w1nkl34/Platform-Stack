using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    static float meshYAxisPos = -0.15f;
    static List<StackController> stackControllers;

    public static List<StackController> GenerateLevel(int gameLength,Material finishMeshMaterial)
    {
        stackControllers = new List<StackController>();
        GenerateFinishAndStartMesh(finishMeshMaterial,gameLength);
        GenerateMiddleMeshes(gameLength);
        return stackControllers;
    }

    public static void GenerateFinishAndStartMesh(Material finishMeshMaterial,int gameLength)
    {
        GameObject startMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
        startMesh.GetComponent<MeshRenderer>().material = finishMeshMaterial;
        startMesh.transform.position = new Vector3(0, meshYAxisPos, Constants.lastStack * 3);
        startMesh.transform.localScale = new Vector3(10, 0.25f, 3f);
        startMesh.name = "Start Stack";
        startMesh.tag = "startStack";
        GameObject finishMesh = Instantiate(startMesh, new Vector3(0, meshYAxisPos, 3 + gameLength * 3 + Constants.lastStack * 3), Quaternion.identity);
        finishMesh.name = "Finish Stack";
        finishMesh.tag = "finishStack";
    }

    public static void GenerateMiddleMeshes(int gameLength)
    {
        GameObject middleStacksParent = new GameObject();
        middleStacksParent.name = "Stacks";
        for (int i = 0; i < gameLength; i++)
        {
            float randomColorValueX = Random.Range(0f, 1f);
            float randomColorValueY = Random.Range(0f, 1f);
            float randomColorValueZ = Random.Range(0f, 1f);
            float spawnPosx = 0;
            if(i != 0)
            {
                spawnPosx = Random.Range(-4f, 4f);
            }
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(spawnPosx , meshYAxisPos, 3f+ (i * 3) +  Constants.lastStack * 3);
            cube.transform.localScale = new Vector3(4, 0.25f, 3f);
            cube.transform.parent = middleStacksParent.transform;
            cube.GetComponent<MeshRenderer>().material.
                SetColor("_Color", new Color(randomColorValueX, randomColorValueY, randomColorValueZ, 1));
            StackController stackController = cube.AddComponent<StackController>();
            stackControllers.Add(stackController);
            cube.SetActive(false);
            if (i == 0)
            {
                cube.SetActive(true);
                cube.GetComponent<StackController>().StopStack();
            }
        }
    }
}
