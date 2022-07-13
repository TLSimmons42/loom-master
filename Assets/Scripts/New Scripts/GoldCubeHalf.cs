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
    public string currentBuildWall;

    public bool canBeDroped =false;
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

    private float playZoneFallSpeed = 2f;

    public Quaternion buildWallTargetRotation;

    public PhotonView PV;
    public BoxCollider collider;

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
        if(currentZone == NoZone)
        {
            PlayerMovesHalf();
        }
        if(currentZone == BuildWallZone)
        {
            collider.isTrigger = false;
            MoveCubeBuildWall();
        }
    }
    IEnumerator CanDropCubeTimer()
    {
        yield return new WaitForSeconds(3);
        PV.RPC("ChangeStateBuildWall", RpcTarget.AllBuffered);
        canBeDroped = true;
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
        if(this.name == "Network Gold Left Half(Clone)")
        {
            if(rightRay.transform.parent.parent.gameObject.tag == "P1")
            {
                PV.RequestOwnership();
                if (currentZone != BuildWallZone)
                {
                    currentZone = NoZone;
                }
            }
        }else if (this.name == "Network Gold Right Half(Clone)")
        {
            if (rightRay.transform.parent.parent.gameObject.tag == "P2")
            {
                PV.RequestOwnership();
                if (currentZone != BuildWallZone)
                {
                    currentZone = NoZone;
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

    [PunRPC]
    public void changeState()
    {
        currentZone = NoZone;
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

        PhotonNetwork.Destroy(temp.gameObject);


    }

    [PunRPC]
    public void ChangeStateBuildWall()
    {
        Debug.Log("current zone is now buildwall");
        currentZone = "BuildWall";
    }
}
