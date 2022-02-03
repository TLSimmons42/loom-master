using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;


public class Cube : XRGrabInteractable
{
    //public GameManager gameManager;
    private Rigidbody rb;
    private BoxCollider collider;

    public Vector3 playWallTargetPos, buildWallTargetPos;

    public string currentZone;
    public int playersHoldingCube = 0;

    public string playWallZone = "PlayWall";
    public string BuildWallZone = "BuildWall";
    public bool isHeld = false;
    public string NoZone = "No Zone";

    private float playZoneFallSpeed = 2f;

    public GameObject rightRay;
    public GameObject leftRay;

    public LineRenderer rightLineRenderer;
    public LineRenderer leftLineRenderer;

    private Vector3[] rightRayPoints = new Vector3[2];
    public Vector3[] leftRayPoints;



    void Start()
    {

        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        rightRay = GameObject.FindGameObjectWithTag("right ray");
        rightLineRenderer = rightRay.GetComponent<LineRenderer>();

    }
    void Update()
    {
        if (currentZone == playWallZone)
        {
            MoveCubePlayWall();
        }

        if (currentZone == BuildWallZone)
        {
            MoveCubeBuildWall();
        }
        if(currentZone == NoZone)
        {

            rightLineRenderer.GetPositions(rightRayPoints);
            gameObject.transform.position = rightRayPoints[rightRayPoints.Length - 1];
        }
        
    }
    public void SetZoneToPlay()
    {
        currentZone = playWallZone;
    }
    public void SetZoneToBuild()
    {
        Debug.Log("Changing to build zone...");
        currentZone = BuildWallZone;
    }

    public void MoveCubePlayWall() 
    {
        //if(currentZone == playWallZone && transform.position == gameManager.PlaywallDropPoints[0].transform.position)
        
        transform.position = Vector3.MoveTowards(transform.position, playWallTargetPos, Time.deltaTime * playZoneFallSpeed);
    }

    public void MoveCubeBuildWall()
    {
        //Debug.Log("moving the block on the BuildWall");
        transform.position = Vector3.MoveTowards(transform.position, buildWallTargetPos, Time.deltaTime * playZoneFallSpeed);
    }

    public void PlayerGrab()
    {
        currentZone = NoZone;
        collider.isTrigger = true;
        //rb.detectCollisions = false;
        //gameObject.layer = 2;
        //isHeld = true;
        //Debug.Log("GRABBED cube");
    }


    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        Debug.Log("gabbing someing");

        //  this will grab all colors in single player mode
        if (GameManager.instance.playerCount == 1)
        {
            Debug.Log("single player grab");
            PlayerGrab();
        }
        else if (GameManager.instance.playerCount > 1)   
        {
            Debug.Log("did nothing in the cube script");
            

            //if(gameObject.tag == "gold cube")
            //{
            //    Debug.Log("gold cube was hit");
            //    playersHoldingCube++;
            //    if(playersHoldingCube== 2)
            //    {
            //        // spawn gold half
            //        NetworkManager.Destroy(gameObject);
            //    }
            //    else
            //    {
            //        Debug.Log("gold cube zone change");
            //        currentZone = NoZone;
            //    }

            //}else if (interactor.transform.parent.parent.gameObject.tag == "P1")
            //    {
            //        Debug.Log("teir 1 pass");
            //        if (gameObject.tag == "red cube" || gameObject.tag == "invis cube") {
            //            Debug.Log(gameObject.tag + " was grabbed");
            //            PlayerGrab();
            //        }
            //    }
            //    if (interactor.transform.parent.parent.gameObject.tag == "P2")
            //    {
            //        if (gameObject.tag == "blue cube" || gameObject.tag == "invis cube")
            //        {
            //        Debug.Log(gameObject.tag + " was grabbed");
            //        PlayerGrab();
            //        }
            //    }
        }

    }
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {

            
        Debug.Log("droped cube");
        playersHoldingCube--;
        collider.isTrigger = false;


        if (currentZone == BuildWallZone)
        {
            Debug.Log("In the build wall");
        }
        else
        {
            if (GameManager.instance.playerCount == 2)
            {
                Debug.Log("destory this cube");
                PhotonNetwork.Destroy(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        //    }
        //}
    }




    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.tag == "DropZone")
        {
            Debug.Log("cube in the dropzone");
           // currentZone = BuildWallZone;
        }
        if (other.tag == "cube despawner")
        {
            if (GameManager.instance.playerCount == 2)
            {
                if (GameManager.instance.host)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
            else
            {
                Destroy(this.gameObject);

            }

           // Debug.Log("cube destroyed");
        }
    }


}
