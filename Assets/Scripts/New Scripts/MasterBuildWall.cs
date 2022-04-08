﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;
using Photon.Pun;

public class MasterBuildWall : Singleton<MasterBuildWall>
{
    [SerializeField]
    private Array2DString editorBuildMasterArray, editorTargetWall;

    private string[,] levelImport;
    private string[,] targetWall;
    private string[,] masterBuildArray = new string[5,5];

    public GameObject hostBuildWallLocation, clientBuildWallLocation;

    public GameObject dropZone;
    public GameObject spotPlaceHolder;

    public GameObject redCube, blueCube, invsCube, goldCube, leftGoldCube, rightGoldCube;
    List<GameObject> hostCubes = new List<GameObject>();
    List<GameObject> clientCubes = new List<GameObject>();


    public void Start()
    {
        importLevel();
        initBuildWallTest();
        displayEditorMasterArray();
    }

    public void importLevel()
    {
        Debug.Log("Difficulty: " + PlayerPrefs.GetString("gameDifficulty"));
        levelImport = Levels.instance.getRandomLevel(PlayerPrefs.GetString("gameDifficulty"));
        setTargetWall();
        initializeBuildWalls();
    }

    public void DebugLog2DArray(string[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Debug.Log(array[i, j]);
            }
        }
    }

    // Instaniates the drop zones
    public void initalizeDropZones()
    {
        for (int i = 0; i < levelImport.GetLength(0); i++)
        {
            for (int j = 0; j < levelImport.GetLength(1); j++)
            {
                Debug.Log("Current indices: (" + i + ", " + j + ")");
                if (i == 0 || i == levelImport.GetLength(0) - 1 || j == 0 || j == levelImport.GetLength(1) - 1)
                { 
                    if (!levelImport[i, j].Equals(""))
                    {
                        Debug.Log("Spawning cube at: (" + i + ", " + j + ")");
                        GameObject hostDropZone = Instantiate(dropZone, hostBuildWallLocation.transform);
                        hostDropZone.GetComponent<DropzoneScript>().direction = indicesToDirection(j, i, levelImport);
                        hostDropZone.GetComponent<DropzoneScript>().index = new Vector2Int(j, i);

                        hostDropZone.transform.position = hostBuildWallLocation.transform.position;
                        hostDropZone.transform.position += hostBuildWallLocation.transform.right * -j;
                        hostDropZone.transform.position += hostBuildWallLocation.transform.up * -i;

                        hostDropZone.transform.parent = hostBuildWallLocation.transform;

                        GameObject clientDropZone = Instantiate(dropZone, clientBuildWallLocation.transform);
                        clientDropZone.GetComponent<DropzoneScript>().direction = indicesToDirection(j, i, levelImport);
                        clientDropZone.GetComponent<DropzoneScript>().index = new Vector2Int(j, i);

                        clientDropZone.transform.position = clientBuildWallLocation.transform.position;
                        clientDropZone.transform.position += clientBuildWallLocation.transform.right * -j;
                        clientDropZone.transform.position += clientBuildWallLocation.transform.up * -i;

                        clientDropZone.transform.parent = clientBuildWallLocation.transform;
                    }
            }
                }
            }
        }

    // Converts given indices and returns the direction is should face
    public string indicesToDirection(int x, int y, string[,] array)
    {
        if (x == 0) {
            return "right";
        } else if (x == array.GetLength(0) - 1) {
            return "left";
        } else if (y == 0) {
            return "down";
        } else if (y == array.GetLength(1) - 1) {
            return "up";
        } else {
            return "invalid";
        }
    }

    // Grabs the inside of the imported wall (ie no dropzone notation) and sets it as the target
    public void setTargetWall()
    {
        targetWall = new string[levelImport.GetLength(0) - 2, levelImport.GetLength(1) - 2];

        for (int i = 1; i < levelImport.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < levelImport.GetLength(1) - 1; j++)
            {
                Debug.Log("Setting target (" + (i - 1) + ", " + (j - 1) + ") to " + levelImport[i, j]);
                targetWall[i - 1, j - 1] = levelImport[i, j];
            }
        }

        displayEditorTargetWall();
    }

    [PunRPC]
    public void initializeBuildWalls()
    {
        hostBuildWallLocation.transform.position += hostBuildWallLocation.transform.up * levelImport.GetLength(0);
        clientBuildWallLocation.transform.position += clientBuildWallLocation.transform.up * levelImport.GetLength(0);
        ConstructBuildWall(hostBuildWallLocation.transform);
        ConstructBuildWall(clientBuildWallLocation.transform);
    }

    void ConstructBuildWall(Transform wallLocation)
    {
        
        initalizeDropZones();
        for (int i = 0; i < targetWall.GetLength(0); i++)
        {
            for (int j = 0; j < targetWall.GetLength(0); j++)
            {
                Vector3 spawnLocation = wallLocation.position;
                spawnLocation += wallLocation.right * -(i + 1);
                spawnLocation += wallLocation.up * -(j + 1);

                GameObject placeholder = Instantiate(spotPlaceHolder, spawnLocation, wallLocation.rotation);
                placeholder.transform.parent = wallLocation.transform;
            }
        }
    }

    private void initBuildWallTest()
    {
        masterBuildArray = new string[5, 5]
        {
            {"a", "b", "c", "d", "e" },
            {"f", "g", "h", "i", "j" },
            {"k", "l", "m", "n", "o" },
            {"p", "q", "r", "s", "t" },
            {"u", "v", "w", "x", "y" }
        };
    }

    private void displayEditorMasterArray()
    {
        for (int x = 0; x < masterBuildArray.GetLength(0); x++)
        {
            for (int y = 0; y < masterBuildArray.GetLength(1); y++)
            {
                Debug.Log("X:" + x + ", Y:" + y);
                Debug.Log(masterBuildArray[x, y]);
                editorBuildMasterArray.SetCell(y, x, masterBuildArray[x, y]);
            }
        }
    }
    private void displayEditorTargetWall()
    {
        for (int x = 0; x < targetWall.GetLength(0); x++)
        {
            for (int y = 0; y < targetWall.GetLength(1); y++)
            {
                Debug.Log("X:" + x + ", Y:" + y);
                Debug.Log(targetWall[x, y]);
                editorTargetWall.SetCell(y, x, targetWall[x, y]);
            }
        }
    }

    public GameObject cubeCodeToGameObject(string cubeCode)
    {
        switch (cubeCode)
        {
            case "R":
                return redCube;
            case "B":
                return blueCube;
            case "I":
                return invsCube;
            case "G":
                return goldCube;
            case "lG":
                return leftGoldCube;
            case "rG":
                return rightGoldCube;
            default:
                Debug.LogError("Invalid cubeCode: " + cubeCode);
                return null;
        }
    }

    public string gameObjectToCubeCode(GameObject cube)
    {
        switch (cube.tag)
        {
            case "red cube":
                return "R";
            case "blue cube":
                return "B";
            case "invis cube":
                return "I";
            case "gold cube":
                return "G";
            case "left gold cube":
                return "lG";
            case "right gold cube":
                return "rG";
            default:
                Debug.LogError("Invalid cube: " + cube.tag);
                return null;
        }
    }

    [PunRPC]

    public void addCube(int x, int y, string cubeCode)
    {
        hostCubes.Add(cubeCodeToGameObject(cubeCode));
        clientCubes.Add(cubeCodeToGameObject(cubeCode));
    }

    [PunRPC]

    public void removeCube(int x, int y, string cubeCode)
    {
        foreach (GameObject cube in hostCubes)
        {
            if (cube.GetComponent<Cube>().index == new Vector2Int(x, y))
            {
                hostCubes.Remove(cube);
            }
        }

        foreach (GameObject cube in clientCubes)
        {
            if (cube.GetComponent<Cube>().index == new Vector2Int(x, y))
            {
                clientCubes.Remove(cube);
            }
        }
    }
}