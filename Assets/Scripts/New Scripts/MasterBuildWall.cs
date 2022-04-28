using System.Collections;
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
    private GameObject[,] hostSpotHolderArray = new GameObject[5,5];
    private GameObject[,] clientSpotHolderArray = new GameObject[5,5];

    public GameObject hostBuildWallLocation, clientBuildWallLocation;

    public GameObject dropZone;
    public GameObject spotPlaceHolder;

    public GameObject redCube, blueCube, invsCube, goldCube, leftGoldCube, rightGoldCube;
    List<GameObject> hostCubes = new List<GameObject>();
    List<GameObject> clientCubes = new List<GameObject>();

    PhotonView PV;


    public void Start()
    {
        PV = GetComponent<PhotonView>();
        importLevel();
        //initBuildWallTest();
        displayEditorMasterArray();
    }

    public void importLevel()
    {
        if (GameManager.instance.host)
        {
            Debug.Log("Difficulty: " + PlayerPrefs.GetString("gameDifficulty"));

            //setTargetWall();
            //initializeBuildWalls();
            PV.RPC("setLevelImport", RpcTarget.AllBuffered, Levels.instance.getRandomLevel(PlayerPrefs.GetString("gameDifficulty")));
            PV.RPC("setTargetWall", RpcTarget.AllBuffered);
            PV.RPC("initializeBuildWalls", RpcTarget.AllBuffered);
            PV.RPC("buildViewWall", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void buildViewWall()
    {
        GameManager.instance.targetWall = targetWall;
        GameManager.instance.BuildViewWall();
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

    [PunRPC]
    public void setLevelImport(string[,] level)
    {
        levelImport = level;
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
                        hostDropZone.GetComponent<DropzoneScript>().hostDropZone = true;

                        hostDropZone.transform.position = hostBuildWallLocation.transform.position;
                        hostDropZone.transform.position += hostBuildWallLocation.transform.right * -j;
                        hostDropZone.transform.position += hostBuildWallLocation.transform.up * -i;

                        hostDropZone.transform.parent = hostBuildWallLocation.transform;

                        GameObject clientDropZone = Instantiate(dropZone, clientBuildWallLocation.transform);
                        clientDropZone.GetComponent<DropzoneScript>().direction = indicesToDirection(j, i, levelImport);
                        clientDropZone.GetComponent<DropzoneScript>().index = new Vector2Int(j, i);
                        clientDropZone.GetComponent<DropzoneScript>().hostDropZone = false;

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

    [PunRPC]
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
        for (int i = 0; i < masterBuildArray.GetLength(0); i++)
        {
            for (int j = 0; j < masterBuildArray.GetLength(1); j++)
            {
                masterBuildArray[i, j] = null;
            }
        }
        hostBuildWallLocation.transform.position += hostBuildWallLocation.transform.up * levelImport.GetLength(0);
        clientBuildWallLocation.transform.position += clientBuildWallLocation.transform.up * levelImport.GetLength(0);
        ConstructBuildWall(hostBuildWallLocation.transform, "host");
        ConstructBuildWall(clientBuildWallLocation.transform, "client");
    }

    void ConstructBuildWall(Transform wallLocation, string wall)
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
                if(wall == "host")
                {
                    placeholder = hostSpotHolderArray[i, j];
                }
                else if(wall == "client")
                {
                    placeholder = clientSpotHolderArray[i, j];
                }
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

    public string cubeCodeToNetworked(string cubeCode)
    {
        switch (cubeCode)
        {
            case "R":
                return "Network Red Cube";
            case "B":
                return "Network Blue Cube";
            case "I":
                return "Network Neutral Cube";
            case "G":
                return "Network Gold Cube";
            case "lG":
                return "Network Gold Left Cube";
            case "rG":
                return "Network Gold Right Cube";
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

    public string cubeCodeToTag(string cubeCode)
    {
        switch (cubeCode)
        {
            case "R":
                return "red cube";
            case "B":
                return "blue cube";
            case "I":
                return "invis cube";
            case "G":
                return "gold cube";
            case "lG":
                return "left gold cube";
            case "rG":
                return "right gold cube";
            default:
                Debug.LogError("Invalid code: " + cubeCode);
                return null;
        }
    }


    public void dropZoneHit(Vector2Int dropIndex, string direction, GameObject cube)
    {
        Vector2Int startPos, targetPos;

        Debug.Log("Direction: " + direction);
        switch (direction)
        
        {
            case "right":
                for (int i = masterBuildArray.GetLength(0) - 1; i >= 0; i--)
                {
                    if (masterBuildArray[i, dropIndex.y] == null || masterBuildArray[dropIndex.x, i].Equals(""))
                    {
                        startPos = dropIndex;
                        targetPos = new Vector2Int(i, dropIndex.y);
                        Debug.Log("Start: " + startPos.ToString());
                        Debug.Log("Target: " + targetPos.ToString());
                        PV.RPC("addCube", RpcTarget.AllBuffered, startPos, targetPos, cube.tag);
                        PhotonView.Destroy(cube);
                    }
                    else
                    {
                        Debug.Log(new Vector2Int(i, dropIndex.y) + " is not null; actual: " + masterBuildArray[i, dropIndex.y]);
                    }
                }
                break;
            case "left":
                for (int i = 0; i < masterBuildArray.GetLength(0); i++)
                {
                    if (masterBuildArray[i, dropIndex.y] == null || masterBuildArray[dropIndex.x, i].Equals(""))
                    {
                        startPos = dropIndex;
                        targetPos = new Vector2Int(i, dropIndex.y);
                        PV.RPC("addCube", RpcTarget.AllBuffered, startPos, targetPos, cube.tag);
                        PhotonView.Destroy(cube);
                    }
                    else
                    {
                        Debug.Log(new Vector2Int(i, dropIndex.y) + " is not null; actual: " + masterBuildArray[i, dropIndex.y]);
                    }
                }

                break;
            case "up":
                for (int i = 0; i < masterBuildArray.GetLength(0); i++)
                {
                    if (masterBuildArray[dropIndex.x, i] == null || masterBuildArray[dropIndex.x, i].Equals(""))
                    {
                        startPos = dropIndex;
                        targetPos = new Vector2Int(dropIndex.x, i);
                        Debug.Log("Start: " + startPos.ToString());
                        Debug.Log("Target: " + targetPos.ToString());
                        PV.RPC("addCube", RpcTarget.AllBuffered, startPos, targetPos, cube.tag);
                        PhotonView.Destroy(cube);
                    }
                    else
                    {
                        Debug.Log(new Vector2Int(dropIndex.x, i) + " is not null; actual: " + masterBuildArray[dropIndex.x, i]);
                    }
                }
                break;
            case "down":
                for (int i = masterBuildArray.GetLength(0) - 1; i >= 0; i--)
                {
                    if (masterBuildArray[dropIndex.x, i] == null || masterBuildArray[dropIndex.x, i].Equals(""))
                    {
                        startPos = dropIndex;
                        targetPos = new Vector2Int(dropIndex.x, i);
                        Debug.Log("Start: " + startPos.ToString());
                        Debug.Log("Target: " + targetPos.ToString());
                        PV.RPC("addCube", RpcTarget.AllBuffered, startPos, targetPos, cube.tag);
                        PhotonView.Destroy(cube);
                    }
                    else
                    {
                        Debug.Log(new Vector2Int(dropIndex.x, i) + " is not null; actual: " + masterBuildArray[dropIndex.x, i]);
                    }
                }
                break;
            default:
                Debug.LogError("Trying to drop at zone without valid direction)");
                break;
        }

    }

    [PunRPC]
    public void addCube(Vector2Int start, Vector2Int target, string cubeCode)
    {
        Debug.Log(start + "  " + target + "  " + cubeCode);
        masterBuildArray[target.x, target.y] = cubeCode;
        if (GameManager.instance.host)
        {
            Vector3 hostSpawnLocation = hostBuildWallLocation.transform.position;
            hostSpawnLocation += hostBuildWallLocation.transform.right * -(start.x + 1);
            hostSpawnLocation += hostBuildWallLocation.transform.up * -(start.y + 1);

            Vector3 clientSpawnLocation = clientBuildWallLocation.transform.position;
            clientSpawnLocation += clientBuildWallLocation.transform.right * -(start.x + 1);
            clientSpawnLocation += clientBuildWallLocation.transform.up * -(start.y + 1);

            string networkedCode = cubeCodeToNetworked(cubeCode);

            GameObject hostCube = PhotonNetwork.Instantiate(networkedCode, hostSpawnLocation, hostBuildWallLocation.transform.rotation);
            GameObject clientCube = PhotonNetwork.Instantiate(networkedCode, clientSpawnLocation, clientBuildWallLocation.transform.rotation);

            if (cubeCode == "lG" || cubeCode == "rG")
            {
                hostCube.GetComponent<GoldCubeHalf>().canBeDroped = false;
                hostCube.GetComponent<GoldCubeHalf>().currentZone = hostCube.GetComponent<GoldCubeWhole>().BuildWallZone;
                hostCube.GetComponent<GoldCubeHalf>().index = target;
                hostCube.GetComponent<GoldCubeHalf>().buildWallTargetPos = new Vector3(-(target.x + 1), -(target.y + 1), 0) + hostBuildWallLocation.transform.position;

                clientCube.GetComponent<GoldCubeHalf>().canBeDroped = false;
                clientCube.GetComponent<GoldCubeHalf>().currentZone = clientCube.GetComponent<GoldCubeWhole>().BuildWallZone;
                clientCube.GetComponent<GoldCubeHalf>().index = target;
                clientCube.GetComponent<GoldCubeHalf>().buildWallTargetPos = new Vector3(-(target.x + 1), -(target.y + 1), 0) + clientBuildWallLocation.transform.position;
            }
            else if (cubeCode == "G")
            {
                hostCube.GetComponent<GoldCubeWhole>().canBeDroped = false;
                hostCube.GetComponent<GoldCubeWhole>().currentZone = hostCube.GetComponent<GoldCubeWhole>().BuildWallZone;
                hostCube.GetComponent<GoldCubeWhole>().index = target;
                hostCube.GetComponent<GoldCubeWhole>().buildWallTargetPos = new Vector3(-(target.x + 1), -(target.y + 1), 0) + hostBuildWallLocation.transform.position;

                clientCube.GetComponent<GoldCubeWhole>().canBeDroped = false;
                clientCube.GetComponent<GoldCubeWhole>().currentZone = clientCube.GetComponent<GoldCubeWhole>().BuildWallZone;
                clientCube.GetComponent<GoldCubeWhole>().index = target;
                clientCube.GetComponent<GoldCubeWhole>().buildWallTargetPos = new Vector3(-(target.x + 1), -(target.y + 1), 0) + clientBuildWallLocation.transform.position;
            }
            else
            {
                hostCube.GetComponent<XRGrabNetworkInteractable>().currentZone = hostCube.GetComponent<XRGrabNetworkInteractable>().BuildWallZone;
                hostCube.GetComponent<XRGrabNetworkInteractable>().index = target;
                hostCube.GetComponent<XRGrabNetworkInteractable>().buildWallTargetPos = new Vector3(-(target.x + 1), -(target.y + 1), 0) + hostBuildWallLocation.transform.position;

                clientCube.GetComponent<XRGrabNetworkInteractable>().currentZone = hostCube.GetComponent<XRGrabNetworkInteractable>().BuildWallZone;
                clientCube.GetComponent<XRGrabNetworkInteractable>().index = target;
                clientCube.GetComponent<XRGrabNetworkInteractable>().buildWallTargetPos = new Vector3(-(target.x + 1), -(target.y + 1), 0) + clientBuildWallLocation.transform.position;
            }


            
        }
    }

    


    [PunRPC]
    public void removeCube(Vector2Int index, string cubeCode)
    {
            masterBuildArray[index.x, index.y] = null;

            if (GameManager.instance.host)
            {
                GameObject[] cubes = GameObject.FindGameObjectsWithTag(cubeCodeToTag(cubeCode));
                foreach (GameObject c in cubes)
                {
                    if (c.GetComponent<XRGrabNetworkInteractable>().index.Equals(index) && c.GetComponent<XRGrabNetworkInteractable>().currentZone.Equals(c.GetComponent<XRGrabNetworkInteractable>().BuildWallZone))
                        PhotonNetwork.Destroy(c);
                }
            }
         
    }
}