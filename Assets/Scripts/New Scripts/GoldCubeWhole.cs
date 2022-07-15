using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class GoldCubeWhole : XRSimpleInteractable
{
    public string currentZone;
    public string playWallZone = "PlayWall";
    public string BuildWallZone = "BuildWall";
    public string NoZone = "No Zone";
    public string holdGold = "Hold Gold";
    public string currentBuildWall;

    public bool isHeld = false;
    public bool isHeldByBoth = false;
    public bool canBeDroped = false;

    private BoxCollider collider;

    public Vector2Int index;

    public int playersHoldingCube = 0;
    private float playZoneFallSpeed = 2f;

    public Vector3 playWallTargetPos, buildWallTargetPos;
    public Quaternion buildWallTargetRotation;

    public PhotonView PV;

    public GameObject rightRay;
    public GameObject leftRay;
    public GameObject mirroredBuildWallCube;


    public LineRenderer rightLineRenderer;
    public LineRenderer leftLineRenderer;
    private Vector3[] rightRayPoints = new Vector3[2];




    void Start()
    {
        StartCoroutine(CanDropCubeTimer());
        PV = GetComponent<PhotonView>();
        collider = GetComponent<BoxCollider>();
        rightRay = GameObject.FindGameObjectWithTag("right ray");
        rightLineRenderer = rightRay.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentZone == playWallZone)
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

    public void PlayerGrab()
    {
        PV.RequestOwnership();
        currentZone = NoZone;
        Debug.Log("New zone: " + currentZone);
        PV.RPC("ChangeStateNoZone", RpcTarget.AllBuffered);
        collider.isTrigger = true;
    }

    public void MoveCubePlayWall()
    {
        //if(currentZone == playWallZone && transform.position == gameManager.PlaywallDropPoints[0].transform.position)

        transform.position = Vector3.MoveTowards(transform.position, playWallTargetPos, Time.deltaTime * playZoneFallSpeed);
        //transform.rotation = buildWallTargetRotation;
    }
    public void MoveCubeBuildWall()
    {
        //Debug.Log("moving the block on the BuildWall");
        transform.position = Vector3.MoveTowards(transform.position, buildWallTargetPos, Time.deltaTime * playZoneFallSpeed);
        transform.rotation = buildWallTargetRotation;
    }
    IEnumerator CanDropCubeTimer()
    {
        yield return new WaitForSeconds(4);
        canBeDroped = true;
    }

    [PunRPC]
    public void IncreaseGoldCubeNetworkVar()
    {
        playersHoldingCube++;
    }

    [PunRPC]
    public void SpawnCubeHalves(XRBaseInteractor interactor)
    {
        if (interactor.transform.parent.parent.gameObject.tag == "P1")
        {
            PhotonNetwork.Instantiate("Network Gold Left Half", transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }
        else if(interactor.transform.parent.parent.gameObject.tag == "P2")
        {
            PhotonNetwork.Instantiate("Network Gold Right Half", transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }
    }
    [PunRPC]
    public void ChangeStateToHold()
    {
        currentZone = holdGold;
    }
    [PunRPC]
    public void ChangeStateNoZone()
    {
        currentZone = NoZone;
    }


    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        isHeld = true;

        if (currentZone == BuildWallZone)
        {
            collider.isTrigger = true;
            PlayerGrab();
        }
        else
        {
            PV.RPC("IncreaseGoldCubeNetworkVar", RpcTarget.AllBuffered);
            if (playersHoldingCube == 2)
            {
                if (interactor.transform.parent.parent.gameObject.tag == "P1")
                {
                    PV.RequestOwnership();
                    Debug.Log("spawning left half");
                    //gameObject.transform.parent.GetComponent<GoldParent>().leftHalf.SetActive(true);
                    //gameObject.transform.parent.GetComponent<GoldParent>().rightHalf.SetActive(true);
                    //gameObject.SetActive(false);
                    GameObject cube = PhotonNetwork.Instantiate("Network Gold Left Half", transform.position, Quaternion.identity);
                    GameObject cube1 = PhotonNetwork.Instantiate("Network Gold Right Half", transform.position, Quaternion.identity);
                    PhotonNetwork.Destroy(this.gameObject);
                    //PV.RPC("DestoryThisObjectOnNetwork", RpcTarget.AllBuffered);
                }
                else if (interactor.transform.parent.parent.gameObject.tag == "P2")
                {
                    PV.RequestOwnership();

                    Debug.Log("spawning right half");
                    //gameObject.transform.parent.GetComponent<GoldParent>().leftHalf.SetActive(true);
                    //gameObject.transform.parent.GetComponent<GoldParent>().rightHalf.SetActive(true);
                    //gameObject.SetActive(false);

                    GameObject cube = PhotonNetwork.Instantiate("Network Gold Left Half", transform.position, Quaternion.identity);
                    GameObject cube1 = PhotonNetwork.Instantiate("Network Gold Right Half", transform.position, Quaternion.identity);


                    PV.TransferOwnership(PhotonNetwork.LocalPlayer);
                    
                    PhotonNetwork.Destroy(this.gameObject);
                    //PV.RPC("DestoryThisObjectOnNetwork", RpcTarget.AllBuffered);
                }
            }
            else
            {
                PV.RPC("changeState", RpcTarget.AllBuffered);
                currentZone = holdGold;
            }
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        //MasterBuildWall.instance.removeCube(index, MasterBuildWall.instance.gameObjectToCubeCode(this.gameObject));
        //PV.RPC("DecreaseGoldCubeNetworkVar", RpcTarget.AllBuffered);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "cube despawner")
        {
            if (GameManager.instance.playerCount == 2)
            {
                if (PV.IsMine)
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
    [PunRPC]
    public void DestoryThisObjectOnNetwork()
    {
        
        PhotonNetwork.Destroy(gameObject);
        
    }

    [PunRPC]
    public void SetMirrorCubeCode(int objID, int mirrorObjID)
    {

        PhotonView temp = PhotonView.Find(objID);
        temp.gameObject.GetComponent<GoldCubeHalf>().mirroredBuildWallCubeID = mirrorObjID;
       
    }




}
