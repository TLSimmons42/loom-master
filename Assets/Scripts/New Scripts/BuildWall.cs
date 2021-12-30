using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWall : MonoBehaviour
{
    GameObject[,] buildWallArr;
    public GameObject otherBuildWall;
    int gameDiff, levelSize;
    public GameObject cubeSlot, dropZone;


    private void Awake()
    {
        PlayerPrefs.SetString("gameDifficulty", "easy");
        PlayerPrefs.SetInt("playerCount", 2);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetString("gameDifficulty"));
        ConvertGameDiffToInt(PlayerPrefs.GetString("gameDifficulty"));
        InitiateBuildWall(gameDiff);
        DisplayBuildWall(gameDiff);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ConvertGameDiffToInt(string strDiff)
    {
        switch (strDiff)
        {
            case "easy":
                gameDiff = 1;
                break;
            case "medium":
                gameDiff = 2;
                break;
            case "hard":
                gameDiff = 3;
                break;
        }
    }

    void InitiateBuildWall(int difficulty)
    {
        levelSize = difficulty * 5;
        Debug.Log("levelSize: " + levelSize);
        buildWallArr = new GameObject[levelSize, levelSize];
        //Debug.Log(buildWallArr.Length);
    }

    void DisplayBuildWall(int difficulty)
    {


        int viewWallSize = difficulty * 5;
        //viewWallSize = 15;

        for (int i = 0; i < viewWallSize; i++)
        {
            Vector3 spawnLocation = transform.position;
            spawnLocation += transform.right * -i;
            spawnLocation += (viewWallSize) * transform.up;

            //Debug.Log("Attempting to spawn placeholder");

            GameObject spawnedCube = Instantiate(dropZone, spawnLocation, transform.rotation);
            spawnedCube.GetComponent<DropzoneScript>().SetColumn(i);
            spawnedCube.transform.parent = transform;

            for (int j = 0; j < viewWallSize; j++)
            {
                spawnLocation = transform.position;
                spawnLocation += transform.right * -i;
                spawnLocation += transform.up * j;

               // Debug.Log("Attempting to spawn placeholder");

                spawnedCube = Instantiate(cubeSlot, spawnLocation, transform.rotation);
                spawnedCube.transform.parent = transform;
                //spawnedCube.GetComponent<Cube>().enabled = false;
            }
        }
    }

    int GetNextFreeRow(int col)
    {
        Debug.Log(buildWallArr.Length);
        for (int i = 0; i < buildWallArr.Length; i++)
        {
            Debug.Log(i);
            Debug.Log(col + ", " + i + ": " + buildWallArr[col, i]);

            if (buildWallArr[col, i] == null)
            {
                Debug.Log("The next free row is: " + i);
                return i;
            }
        }
        Debug.Log("There is no free row");
        return -1;
    }

    public void DropBox(GameObject box, int col)
    {
        int nextFreeRow = GetNextFreeRow(col);

        if (nextFreeRow == -1)
        {
            Destroy(box);
        }

        else
        {
            Debug.Log("calculating the build wall pos for the cube");
            box.transform.position = transform.position+ (transform.right * -col) + (transform.up * (levelSize + 1));

            Vector3 newLocation = transform.position;
            newLocation += transform.right * -col;
            newLocation += transform.up * nextFreeRow;

            buildWallArr[col, nextFreeRow] = box;

            //box.transform.position = newLocation;
            box.GetComponent<Cube>().currentZone = "BuildWall";
            box.GetComponent<Cube>().SetZoneToBuild();
            box.GetComponent<Cube>().buildWallTargetPos = newLocation;
            box.GetComponent<BoxCollider>().enabled = false;

            MirrorBuildWalls(col, nextFreeRow, box);
        }
    }

    public void MirrorBuildWalls(int col, int row, GameObject clonedBox)
    {
        Debug.Log("# of players = " + PlayerPrefs.GetInt("playerCount"));
        if (PlayerPrefs.GetInt("playerCount") == 2)
        {
            Debug.Log("Attempting to clone boxes...");

            GameObject newBox = Instantiate(clonedBox);
            newBox.GetComponent<Cube>().currentZone = newBox.GetComponent<Cube>().BuildWallZone;
            newBox.GetComponent<Cube>().SetZoneToBuild();


            Vector3 newLocation = otherBuildWall.transform.position;
            newLocation += otherBuildWall.transform.right * -col;
            newLocation += otherBuildWall.transform.up * row;

            newBox.GetComponent<Cube>().buildWallTargetPos = newLocation;
            newBox.GetComponent<BoxCollider>().enabled = false;
            newBox.transform.position = otherBuildWall.transform.position + (otherBuildWall.transform.right * -col) + (otherBuildWall.transform.up * (levelSize + 1));

            

            //buildWallArr[col, row] = box;

            otherBuildWall.GetComponent<BuildWall>().buildWallArr = buildWallArr;   

            //box.transform.position = newLocation;
            
        }
    }

    public int[,] CubeToInt()
    {
        int[,] intArr = new int[levelSize, levelSize];

        for (int i = 0; i < levelSize; i++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                string cubeName = buildWallArr[i, j].tag;

                switch (cubeName)
                {
                    case "blue cube":
                        intArr[i,j] = 1;
                        break;
                    case "red cube":
                        intArr[i, j] = 2;
                        break;
                    case "gold cube":
                        intArr[i, j] = 3;
                        break;
                    case "invis cube":
                        intArr[i, j] = 4;
                        break;
                }
            }
        }

        return intArr;
    }
}
