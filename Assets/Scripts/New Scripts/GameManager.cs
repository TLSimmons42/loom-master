using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using TMPro;

public class GameManager : MonoBehaviour
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
    public bool networking = false;


    // Start is called before the first frame update
    void Start()
    {
        dropCubes = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (dropCubes)
        {
            StartCoroutine(BuildNewCube());
        }
    }


    
    // this will be called at the start of the game to build a the view wall for the player
    public void BuildViewWall(int difficulty)
    {

    }


    // this will be called durring the game in order to build a new cube on the Play Wall
    public IEnumerator BuildNewCube()
    {
        if (dropCubes && !networking)
        {
            dropCubes = false;
            int spawnPointChoice = Random.Range(0, PlaywallDropPoints.Length);
            int cubeChoice = Random.Range(0, 4); // 4 = the number of cube choices
            Debug.Log("spawn point " + spawnPointChoice);
            Debug.Log("Cube choice " + cubeChoice);

            if (cubeChoice == 0)
            {
                cube = Instantiate(RedCube,PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity) ;
            }
            if (cubeChoice == 1)
            {
                cube =Instantiate(BlueCube, PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity);
            }
            if (cubeChoice == 2)
            {
                cube =Instantiate(GoldCube, PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity);
            }
            if (cubeChoice == 3)
            {
                cube = Instantiate(NeutralCube, PlaywallDropPoints[spawnPointChoice].transform.position, Quaternion.identity);
            }
            cube.gameObject.GetComponent<Cube>().playWallTargetPos = PlaywallEndPoints[spawnPointChoice].transform.position;
        
        }
        yield return new WaitForSeconds(cubeDropTimer);
        dropCubes = true;
    }

    //This will be the Game Start Function for Single Player Mode
    public void SinglePlayerStart()
    {

    }

    // Cube Drop from play wall function for Single player mode
    public void SinglePlayerCubeDrop()
    {

    }

    //This will be the Game Start Function for Multiplayer Player Mode
    public void MultiplayerStart()
    {

    }

    // Cube Drop from play wall function for Single MultiplayerMode mode
    public void MultiplayerCubeDrop()
    {

    }


}
