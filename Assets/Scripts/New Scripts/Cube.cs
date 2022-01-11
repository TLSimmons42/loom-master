using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Cube : XRGrabInteractable
{
    public GameManager gameManager;
    private Rigidbody rb;
    private BoxCollider collider;

    public Vector3 playWallTargetPos, buildWallTargetPos;

    public string currentZone;

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
        
            transform.position = Vector3.MoveTowards(transform.position, playWallTargetPos, Time.deltaTime* playZoneFallSpeed);
        
    }

    public void MoveCubeBuildWall()
    {
        Debug.Log("moving the block on the BuildWall");
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

        //  this will grab all colors in single player mode
        if(gameManager.playerCount == 1)
        {
            PlayerGrab();
        }
        else if (gameManager.playerCount > 1)  // this will grab the blocks assigned to you 
        {
            if (interactor.transform.parent.gameObject.tag == "P1" && (gameObject.tag == "red cube" || gameObject.tag == "invis cube"))
            {
                PlayerGrab();
            }
            if (interactor.transform.parent.gameObject.tag == "P2" && (gameObject.tag == "blue cube" || gameObject.tag == "invis cube"))
            {
                PlayerGrab();
            }
        }

    }
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("droped cube");
        collider.isTrigger = false;
        if(currentZone == BuildWallZone)
        {
            Debug.Log("In the build wall");
        }
        else
        {
            Debug.Log("destory this cube");
            Destroy(gameObject);
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DropZone")
        {
            Debug.Log("cube in the dropzone");
           // currentZone = BuildWallZone;
        }
        if (other.tag == "cube despawner")
        {
            Destroy(this.gameObject);
           // Debug.Log("cube destroyed");
        }
    }


}
