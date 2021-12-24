using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Cube : XRGrabInteractable
{
    public GameManager gameManager;
    private Rigidbody rb;

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



    // Update is called once per frame

    private void Awake()
    {
        SetZoneToPlay();
    }

    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
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

        if (isHeld)
        {
            rightLineRenderer.GetPositions(rightRayPoints);
            gameObject.transform.position = rightRayPoints[rightRayPoints.Length - 1];
            rb.detectCollisions = false;
            //rightRay.ResetRayObjectPos(gameObject);
        }
    }
    public void SetZoneToPlay()
    {
        Debug.Log("Changing to play zone...");
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
        if (currentZone == playWallZone)
        {
            transform.position = Vector3.MoveTowards(transform.position, playWallTargetPos, Time.deltaTime* playZoneFallSpeed);
        }
    }

    public void MoveCubeBuildWall()
    {
        transform.position = Vector3.MoveTowards(transform.position, buildWallTargetPos, Time.deltaTime * playZoneFallSpeed);
    }

    public void PlayerGrab()
    {
        if(currentZone == playWallZone)
        {
            currentZone = NoZone;
            isHeld = true;

        }
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        PlayerGrab();

    }
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("droped cube");
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cube despawner")
        {
            Destroy(this.gameObject);
            Debug.Log("cube destroyed");
        }
        else
        {
            Debug.Log("cube not destroyed");
        }
    }


}
