using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public float cubeDropTimer = .5f;
    public GameObject[] PlaywallDropPoints;

    public GameObject BlueCube;
    public GameObject RedCube;
    public GameObject GoldCube;
    public GameObject NeutralCube;

    public GameObject buildWall1;
    public GameObject buildWall2;
    public GameObject ViewWall1;
    public GameObject ViewWall2;

    public UIView startView;
    public UIView waitingView;
    public UIView gameOverView;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    // this will be called at the start of the game to build a the view wall for the player
    public void BuildViewWall(int difficulty)
    {

    }


    // this will be called durring the game in order to build a new cube on the Play Wall
    public void BuildNewCube()
    {
        
    }
}
