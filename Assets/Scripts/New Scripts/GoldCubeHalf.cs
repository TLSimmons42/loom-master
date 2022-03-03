﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class GoldCubeHalf : XRSimpleInteractable
{

    public string currentZone;
    public string NoZone = "No Zone";
    public string BuildWallZone = "BuildWall";

    public GameObject replacedCube;
    public GameObject rightRay;
    public GameObject leftRay;

    public LineRenderer rightLineRenderer;
    public LineRenderer leftLineRenderer;

    private Vector3[] rightRayPoints = new Vector3[2];
    public Vector3[] leftRayPoints;

    PhotonView PV;
    private BoxCollider collider;


    void Start()
    {
        collider = GetComponent<BoxCollider>();
        PV = GetComponent<PhotonView>();
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
    }

    public void PlayerMovesHalf()
    {
        rightLineRenderer.GetPositions(rightRayPoints);
        gameObject.transform.position = rightRayPoints[rightRayPoints.Length - 1];
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
                currentZone = NoZone;
            }
        }else if (this.name == "Network Gold Right Half(Clone)")
        {
            if (rightRay.transform.parent.parent.gameObject.tag == "P2")
            {
                PV.RequestOwnership();
                currentZone = NoZone;
            }
        }
    }
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        collider.isTrigger = false;
        if (currentZone == BuildWallZone)
        {
            Debug.Log("In the build wall");
        }
        else
        {
            Debug.Log("destory this cube");
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

}
