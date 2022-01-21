using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Doozy.Engine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : Singleton<GameManager>
{

    private float cubeDropTimer = 1.5f;
    
    public GameObject[] PlaywallDropPoints;
    public GameObject[] PlaywallEndPoints;

    private GameObject cube; // spawned cube
    public GameObject BlueCube;
    public GameObject RedCube;
    public GameObject GoldCube;
    public GameObject NeutralCube;

    public GameObject NetworkBlueCube;
    public GameObject NetworkRedCube;
    public GameObject NetworkGoldCube;
    public GameObject NetworkNeutralCube;



    public GameObject buildWall1;
    public GameObject buildWall2;
    public GameObject ViewWall1;
    public GameObject ViewWall2;

    public UIView startView;
    public UIView waitingView;
    public UIView gameOverView;

    public bool dropCubes = false;
    public bool dropNetworkCubes = false;
    public bool host = false;
    public bool allPlayersConnected = false;

    public TextAsset[] easyLevels;
    public TextAsset[] mediumLevels;
    public TextAsset[] hardLevels;

    List<GameObject> Players = new List<GameObject>();

    int gameDiff;
    string strGameDiff;
    public int playerCount;
    int playerDesignation;
    int numOfPlayersInGame;


    GameObject[,] buildWallArr;


    // Start is called before the first frame update
    void Start()
    {
        strGameDiff = PlayerPrefs.GetString("gameDifficulty");
        playerCount = PlayerPrefs.GetInt("playerCount");
        Debug.Log("the player count is: " + playerCount);
        
        ConvertGameDiffToInt(strGameDiff); //gets game difficulty 
        MakeViewWall(); // detects # of players and spawns appropriate view walls
        if (playerCount == 2)
        {
            StartCoroutine( DetectNumOfPlayers()); // this will set the host and give access to the players gameobjects
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (dropCubes)
        {
            StartCoroutine(BuildNewCube());
        }
        if (dropNetworkCubes)
        {
            StartCoroutine(BuildNewNetworkCube());
        }
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

    private int[,] ReadRandomLevel(int difficulty)
    {
        int levelSize = difficulty * 5;
        int[,] level = new int[levelSize, levelSize];

        string levelText = GetRandomLevel(difficulty).text;
        Regex rgx = new Regex("[^a-zA-Z0-9 -]");
        levelText = rgx.Replace(levelText, "");

        for (int i = 0; i < levelSize; i++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                //Debug.Log("Attempting to convert to integer");
                level[i, j] = int.Parse(char.ToString(levelText[j + (levelSize * i)]));
                //Debug.Log("Successfully converted to integer");
            }
        }

        return level;
    }

    private TextAsset GetRandomLevel(int difficulty)
    {
        TextAsset randLevel;
        switch (difficulty)
        {
            case 1:
                return easyLevels[Mathf.FloorToInt(Random.Range(0, easyLevels.Length))];
            case 2:
                return mediumLevels[Mathf.FloorToInt(Random.Range(0, mediumLevels.Length))];
            default:
                return hardLevels[Mathf.FloorToInt(Random.Range(0, hardLevels.Length))];
        }
    }


    // this will be called at the start of the game to build a the view wall for the player
    public void BuildViewWall(int difficulty, GameObject[] spawnLocations)
    {


        //string diff = "easy";
        int viewWallSize = difficulty * 5;
        /*
        switch (diff)
        {
            case "easy":
                viewWallSize = 5;
                break;
            case "medium":
                viewWallSize = 10;
                break;
            case "hard":
                viewWallSize = 15;
                break;
        } */

        int[,] testLevel = {{1,2,1,2,1},
                            {2,1,2,1,2},
                            {1,2,1,2,1},
                            {2,1,2,1,2},
                            {1,2,1,2,1}};

        int[,] easy1 =     {{1,1,1,1,1},
                            {1,2,2,2,1},
                            {1,2,3,2,1},
                            {1,2,2,2,1},
                            {1,1,1,1,1}};

        int[,] level = ReadRandomLevel(difficulty);

        buildWall1.GetComponent<BuildWall>().SetViewWall(level);
        buildWall2.GetComponent<BuildWall>().SetViewWall(level);

        // Making the view wall depending on the difficulty
        for (int l = 0; l < spawnLocations.Length; l++)
        {
            for (int i = 0; i < viewWallSize; i++)
            {
                for (int j = 0; j < viewWallSize; j++)
                {
                    Vector3 spawnLocation = spawnLocations[l].transform.position;
                    spawnLocation += spawnLocations[l].transform.right * -i;
                    spawnLocation += spawnLocations[l].transform.up * j;
                    GameObject cube = BlueCube;
                    switch (level[i, j])
                    {
                        case 1:
                            cube = BlueCube;
                            break;
                        case 2:
                            cube = RedCube;
                            break;
                        case 3:
                            cube = GoldCube;
                            break;
                        case 4:
                            cube = NeutralCube;
                            break;
                    }
                    GameObject spawnedCube = Instantiate(cube, spawnLocation, spawnLocations[l].transform.rotation);
                    //Destroy(spawnedCube.GetComponent<Rigidbody>());
                    spawnedCube.GetComponent<Cube>().enabled = false;

                    if (l == 0)
                    {
                        spawnedCube.transform.parent = ViewWall1.transform;
                    }
                    else
                    {
                        spawnedCube.transform.parent = ViewWall2.transform;
                    }
                }
            }
        }
    }

    

    // this will be called durring the game in order to build a new cube on the Play Wall
    public IEnumerator BuildNewCube()
    {
        if (dropCubes)
        {
            dropCubes = false;
            int spawnPointChoice = Random.Range(0, PlaywallDropPoints.Length);
            int cubeChoice = Random.Range(0, 4); // 4 = the number of cube choices


            if (cubeChoice == 0)
            {
                cube = Instantiate(RedCube,PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity) ;
            }
            if (cubeChoice == 1)
            {
                cube = Instantiate(BlueCube, PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity);
            }
            if (cubeChoice == 2)
            {
                cube = Instantiate(GoldCube, PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity);
            }
            if (cubeChoice == 3)
            {
                cube = Instantiate(NeutralCube, PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity);
            }
            cube.gameObject.GetComponent<Cube>().playWallTargetPos = PlaywallEndPoints[spawnPointChoice].transform.position;
            cube.GetComponent<Cube>().SetZoneToPlay();

        }
        yield return new WaitForSeconds(cubeDropTimer);
        dropCubes = true;
    }

    public IEnumerator BuildNewNetworkCube()
    {
        if (host)
        {
            if (dropNetworkCubes)
            {
                dropNetworkCubes = false;
                int spawnPointChoice = Random.Range(0, PlaywallDropPoints.Length);
                int cubeChoice = Random.Range(0, 4); // 4 = the number of cube choices


                if (cubeChoice == 0)
                {
                    cube = PhotonNetwork.Instantiate("Network Red Cube", PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity);
                }
                if (cubeChoice == 1)
                {
                    cube = PhotonNetwork.Instantiate("Network Blue Cube", PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity);
                }
                if (cubeChoice == 2)
                {
                    cube = PhotonNetwork.Instantiate("Network Gold Cube", PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity);
                }
                if (cubeChoice == 3)
                {
                    cube = PhotonNetwork.Instantiate("Network Neutral Cube", PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity);
                }
                cube.gameObject.GetComponent<Cube>().playWallTargetPos = PlaywallEndPoints[spawnPointChoice].transform.position;
                cube.GetComponent<Cube>().SetZoneToPlay();

            }
            yield return new WaitForSeconds(cubeDropTimer);
            dropNetworkCubes = true;
        }

    }
    public void MakeViewWall()
    {
        if (PlayerPrefs.GetInt("playerCount") == 1)
        {
            BuildViewWall(gameDiff, new GameObject[] { ViewWall1 });
        }
        else
        {
            BuildViewWall(gameDiff, new GameObject[] { ViewWall1, ViewWall2 });
        }
    }


    //This will detect the number of players and start the game accordingly
    public void StartTheGame()
    {
        //Analytics.instance.WriteData("Game Start", "placeholder", TimerScript.instance.currentTime.ToString());
        TimerScript.instance.record = true;
        if(playerCount == 2 && host)
        {
            MultiplayerStart();
        }
        else
        {
            SinglePlayerStart();
        }
    }

    public void SinglePlayerStart()
    {
        Debug.Log("single player start");

        dropCubes = true;
    }

    //This will be the Game Start Function for Multiplayer Player Mode
    public void MultiplayerStart()
    {
        Debug.Log("multiplayer start");
        dropNetworkCubes = true;
    }
    public IEnumerator DetectNumOfPlayers()
    {
        yield return new WaitForSeconds(3);

        Debug.Log("deteting the num of players");
        numOfPlayersInGame = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Network Player"))
        {


            if (obj.name == "Network Player(Clone)")
            {

                Players.Add(obj);
                numOfPlayersInGame++;
                if (Players.Count == 2)
                {
                    allPlayersConnected = true;
                    host = true;
                    this.tag = "host";
                    AssignPlayerColors();
                    Debug.Log("the host has been set");
                }
                else if(Players.Count == 1)
                {
                    host = false;
                    this.tag = "cliant";
                    Debug.Log("a cliant has been set");
                }
            }
        }

    }

    public void Gameover()
    {
        //Analytics.instance.WriteData("Game Start", "placeholder", TimerScript.instance.currentTime.ToString());
        TimerScript.instance.record = false;
        gameOverView.Show();
        dropCubes = false;
        dropNetworkCubes = false;
    }

    //tags the player gameobjects so that the cubes can identify them
    public void AssignPlayerColors()
    {
        Players[0].tag = "P1";
        Players[1].tag = "P2";
    }


}
