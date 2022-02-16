using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabNetworkInteractable : XRGrabInteractable
{


    public Vector3 playWallTargetPos, buildWallTargetPos;
    public Quaternion buildWallTargetRotation;

    public string currentZone;
    public int playersHoldingCube = 0;
    public Vector3 goldCubeHoldPos;

    public string playWallZone = "PlayWall";
    public string BuildWallZone = "BuildWall";
    public bool isHeld = false;
    public string NoZone = "No Zone";
    public string holdGold = "Hold Gold";

    private BoxCollider collider;
    private PhotonView photonView;
    private Transform currentPos;
    Cube cube;
    private Rigidbody rb;

    private float playZoneFallSpeed = 2f;

    public GameObject rightRay;
    public GameObject leftRay;

    public LineRenderer rightLineRenderer;
    public LineRenderer leftLineRenderer;

    private Vector3[] rightRayPoints = new Vector3[2];
    public Vector3[] leftRayPoints;
 
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        collider = GetComponent<BoxCollider>();
        cube = GetComponent<Cube>();
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
        if (currentZone == NoZone)
        {
            rightLineRenderer.GetPositions(rightRayPoints);
            gameObject.transform.position = rightRayPoints[rightRayPoints.Length - 1];
        }
        if (currentZone == holdGold)
        {
            transform.position = goldCubeHoldPos;
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
        transform.rotation = buildWallTargetRotation;
    }
    public void MoveCubeBuildWall()
    {
        //Debug.Log("moving the block on the BuildWall");
        transform.position = Vector3.MoveTowards(transform.position, buildWallTargetPos, Time.deltaTime * playZoneFallSpeed);
        transform.rotation = buildWallTargetRotation;
    }

    public void GoldHold(Vector3 CubePos)
    {
        currentZone = holdGold;
        goldCubeHoldPos = CubePos;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);



        if (gameObject.tag == "gold cube")
        {
            Debug.Log("gold cube was hit");
            playersHoldingCube++;
            if (playersHoldingCube == 2)
            {
                if (interactor.transform.parent.parent.gameObject.tag == "P1")
                {

                    PhotonNetwork.Instantiate("Network Gold Left Half", transform.position, Quaternion.identity);
                    NetworkManager.Destroy(gameObject);
                }
                else
                {
                    PhotonNetwork.Instantiate("Network Gold Right Half", transform.position, Quaternion.identity);
                    NetworkManager.Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("gold cube zone change");
                GoldHold(gameObject.transform.position);
            }

        }
        else if (interactor.transform.parent.parent.gameObject.tag == "P1")
        {
            if(gameObject.tag == "left gold cube" || gameObject.tag == "left gold cube")
            {
                PlayerGrabGoldHalf();
            }

            Debug.Log("teir 1 pass");
            if (gameObject.tag == "red cube" || gameObject.tag == "invis cube")
            {
                Debug.Log(gameObject.tag + " was grabbed");
                PlayerGrab();
            }
        }
        if (interactor.transform.parent.parent.gameObject.tag == "P2")
        {
            if (gameObject.tag == "blue cube" || gameObject.tag == "invis cube")
            {
                Debug.Log(gameObject.tag + " was grabbed");
                PlayerGrab();
            }
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        if (cube.currentZone == cube.BuildWallZone)
        {
            Debug.Log("In the build wall");
        }else{
            Debug.Log("destory this cube");
            PhotonNetwork.Destroy(this.gameObject);
        }

        
    }

    protected override void OnHoverEntered(XRBaseInteractor interactor)
    {

    }

    public void GoldBlockHold(Transform obj)
    {
        //gameObject.transform.position = 
    }

    public void PlayerGrab()
    {
        photonView.RequestOwnership();
        cube.currentZone = cube.NoZone;
        collider.isTrigger = true;
    }
    public void PlayerGrabGoldHalf()
    {
        cube.currentZone = cube.NoZone;
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "DropZone")
        {

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
