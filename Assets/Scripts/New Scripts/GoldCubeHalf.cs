using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class GoldCubeHalf : XRGrabInteractable
{

    public string currentZone;
    public string NoZone = "No Zone";
    public string BuildWallZone = "BuildWall";
    public string deleteZone = "delete zone";
    public string currentBuildWall;

    public bool canDrop =false;
    public GameObject replacedCube;
    public GameObject rightRay;
    public GameObject leftRay;
    public GameObject mirroredBuildWallCube;

    public int mirroredBuildWallCubeID;
    public Vector2Int index;

    public LineRenderer rightLineRenderer;
    public LineRenderer leftLineRenderer;

    private Vector3[] rightRayPoints = new Vector3[2];
    public Vector3[] leftRayPoints;
    public Vector3 buildWallTargetPos;

    private float playZoneFallSpeed = 3f;

    public Quaternion buildWallTargetRotation;

    public PhotonView PV;
    public BoxCollider collider;

    public bool updateBuildWallState = false;
    public bool destroyCube = false;

    MyRayInteractor myRay;
    void Start()
    {
        StartCoroutine(CanDropCubeTimer());
        collider = GetComponent<BoxCollider>();
        PV = GetComponent<PhotonView>();
        if(index.x != 0 || index.y != 0)
        {
            PV.RPC("assignNetworkIndex", RpcTarget.AllBuffered, index.x, index.y);
            Debug.Log("did a network index");
        }
        Debug.Log("the PV is assigned");
        //currentZone = NoZone;
        rightRay = GameObject.FindGameObjectWithTag("right ray");
        rightLineRenderer = rightRay.GetComponent<LineRenderer>();

        
        AssignCubeToPlayers();

    }

    // Update is called once per frame
    void Update()
    {
        if(currentZone == deleteZone)
        {
            if (GameManager.instance.host)
            {
                GameManager.instance.holdingGoldHalf = false;
                PhotonNetwork.Destroy(gameObject);
            }
        }
        if (updateBuildWallState)
        {
            PV.RPC("ChangeStateBuildWall", RpcTarget.AllBuffered);
            updateBuildWallState = false;
        }
        if (destroyCube)
        {
            PV.RPC("ChangeDestoryVariable", RpcTarget.AllBuffered);
            //Debug.Log("meow");

            if (GameManager.instance.host)
            {
                GameManager.instance.holdingGoldHalf = false;
                PhotonNetwork.Destroy(this.gameObject);
                //destroyCube = false;
            }
           
        }

        if(currentZone == NoZone)
        {
            if (PV.IsMine)
            {
                GameManager.instance.holdingGoldHalf = true;
                PlayerMovesHalf();
            }
        }
        if(currentZone == BuildWallZone)
        {
            
                collider.isTrigger = false;
            if (GameManager.instance.host)
            {
                MoveCubeBuildWall();
            }
        }
    }
    IEnumerator CanDropCubeTimer()
    {
        yield return new WaitForSeconds(1.5f);
        //PV.RPC("ChangeStateBuildWall", RpcTarget.AllBuffered);
        canDrop = true;
    }

    public void PlayerMovesHalf()
    {
        rightLineRenderer.GetPositions(rightRayPoints);
        gameObject.transform.position = rightRayPoints[rightRayPoints.Length - 1];
    }
    public void MoveCubeBuildWall()
    {
        //Debug.Log("moving the block on the BuildWall");
        transform.position = Vector3.MoveTowards(transform.position, buildWallTargetPos, Time.deltaTime * playZoneFallSpeed);
        transform.rotation = buildWallTargetRotation;
    }

    public void SetMirrorObj(GameObject obj)
    {
        obj = replacedCube;
    }

    public void AssignCubeToPlayers()
    {
        //PV.RPC("changeState", RpcTarget.AllBuffered);

        if (this.name == "Network Gold Left Half(Clone)")
        {
            if(rightRay.transform.parent.parent.gameObject.tag == "P1")
            {
                if (currentZone != BuildWallZone)
                {
                    PV.RequestOwnership();
                    PV.RPC("changeState", RpcTarget.AllBuffered);
                    //Analytics.instance.writeEvent("Player " + rightRay.transform.parent.parent.gameObject.tag + " grabs the " + this.name + " cube.", 3);
                }
            }
        }else if (this.name == "Network Gold Right Half(Clone)")
        {
            if (rightRay.transform.parent.parent.gameObject.tag == "P2")
            {
                if (currentZone != BuildWallZone)
                {
                    PV.RequestOwnership();
                    PV.RPC("changeState", RpcTarget.AllBuffered);
                    //Analytics.instance.writeEvent("Player 2 grabs the gold cube.", 3);
                }
            }
        }
    }
    public void PlayerGrab()
    {
        PV.RequestOwnership();
        currentZone = NoZone;
        Debug.Log("New zone: " + currentZone);
        PV.RPC("changeState", RpcTarget.AllBuffered);
        collider.isTrigger = true;
    }


    protected override void OnSelectEntered(XRBaseInteractor interactor)
    { 
        if(currentZone == BuildWallZone)
        {
            PlayerGrab();
            PV.RPC("removeCube", RpcTarget.AllBuffered, index.x, index.y);
        }
    }
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        //collider.isTrigger = false;
        if (currentZone == BuildWallZone)
        {
            Debug.Log("In the build wall");
        }
        else
        {
            GameManager.instance.holdingGoldHalf = false;
            PhotonView.Destroy(this.gameObject);
            Debug.Log("destory this cube");
            //MasterBuildWall.instance.removeCube(index, MasterBuildWall.instance.gameObjectToCubeCode(this.gameObject));
        }
    }

    public void AssignIndex(int x, int y)
    {
        Debug.Log("index x: " + x + "   index y: " + y);
        if (PV != null)
        {
            PV.RPC("assignNetworkIndex", RpcTarget.AllBuffered, x, y);
        }
        else
        {
            Debug.Log("the PV was null");
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "DropZone")
        {
            canDrop = false;
            // currentZone = BuildWallZone;
        }
    }


    [PunRPC]
    public void changeState()
    {
        currentZone = NoZone;
    }
    [PunRPC]
    public void ChangeStateToDelete()
    {
        currentZone = deleteZone;
        Analytics.instance.writeEvent("Gold cube destroyed", 3);
    }

    [PunRPC]
    public void assignNetworkIndex(int x, int y)
    {
        index.x = x;
        index.y = y;
        Debug.Log("network index is now: " + index);
    }

    [PunRPC]
    public void removeCube(int x, int y)
    {

        //MasterBuildWall.instance.GetComponent<PhotonView>().RPC("removeCubeFromMasterWall", RpcTarget.AllBuffered, x, y);
        MasterBuildWall.instance.masterBuildArray[x, y] = null;
        MasterBuildWall.instance.updateMasterArray = true;

        //Debug.Log("Delete this cube: "+ mirroredBuildWallCube.name);

        PhotonView temp = PhotonView.Find(mirroredBuildWallCubeID);
        Analytics.instance.writeEvent("Gold cube destroyed", 3);
        PhotonNetwork.Destroy(temp.gameObject);

    }

    [PunRPC]
    public void ChangeStateBuildWall()
    {
        Debug.Log("current zone is now buildwall");
        currentZone = "BuildWall";
    }

    [PunRPC]
    public void ChangeDestoryVariable()
    {
        destroyCube = true;
    }


}
