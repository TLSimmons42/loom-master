using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;
using Photon.Pun;

public class MasterBuildWall : Singleton<MasterBuildWall>
{
    [SerializeField]
    private Array2DString editorBuildArray;

    private string[,] levelImport;
    private string[,] targetWall;
    private string[,] masterBuildArray = new string[5,5];

    public GameObject dropZone;

    public GameObject redCube, blueCube, clearCube, goldCube, leftGoldCube, rightGoldCube;


    public void Start()
    {
        initBuildWallTest();
        displayEditorArray();
    }

    public void importLevel()
    {
        levelImport = Levels.instance.getRandomLevel(PlayerPrefs.GetString("gameDifficulty"));
    }

    public void initalizeDropZones()
    {

    }

    public void setTargetWall()
    {
        targetWall = new string[levelImport.GetLength(0) - 2, levelImport.GetLength(1) - 2];

        for (int i = 1; i < levelImport.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < levelImport.GetLength(1) - 1; j++)
            {
                targetWall[i - 1, j - 1] = levelImport[i, j];
            }
        }
    }

    public void initializeBuildWalls()
    {

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

    private void displayEditorArray()
    {
        for (int x = 0; x < masterBuildArray.GetLength(0); x++)
        {
            for (int y = 0; y < masterBuildArray.GetLength(1); y++)
            {
                Debug.Log("X:" + x + ", Y:" + y);
                Debug.Log(masterBuildArray[x, y]);
                editorBuildArray.SetCell(y, x, masterBuildArray[x, y]);
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
            case "C":
                return clearCube;
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
                return "C";
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

    }

    [PunRPC]

    public void removeCube(int x, int y, string cubeCode)
    {

    }
}