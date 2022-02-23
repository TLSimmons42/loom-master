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
    public bool isHeld = false;
    public string NoZone = "No Zone";
    public string holdGold = "Hold Gold";

    public int playersHoldingCube = 0;
    private float playZoneFallSpeed = 2f;

    public Vector3 playWallTargetPos;

    public PhotonView PV;


    void Start()
    {
        //playersHoldingCube = 1;
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentZone == playWallZone)
        {
            MoveCubePlayWall();
        }
    }

    public void MoveCubePlayWall()
    {
        //if(currentZone == playWallZone && transform.position == gameManager.PlaywallDropPoints[0].transform.position)

        transform.position = Vector3.MoveTowards(transform.position, playWallTargetPos, Time.deltaTime * playZoneFallSpeed);
        //transform.rotation = buildWallTargetRotation;
    }

    [PunRPC]
    public void IncreaseGoldCubeNetworkVar()
    {
        playersHoldingCube++;
    }

    [PunRPC]
    public void DecreaseGoldCubeNetworkVar()
    {
        playersHoldingCube--;
    }


    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        PV.RPC("IncreaseGoldCubeNetworkVar", RpcTarget.AllBuffered);
        if(playersHoldingCube == 2)
        {
            if (interactor.transform.parent.parent.gameObject.tag == "P1")
            {
                PhotonNetwork.Instantiate("Network Gold Left Half", transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                PhotonNetwork.Instantiate("Network Gold Right Half", transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            currentZone = holdGold;
            PV.RequestOwnership();
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        PhotonNetwork.Destroy(gameObject);
        //PV.RPC("DecreaseGoldCubeNetworkVar", RpcTarget.AllBuffered);
    }




}
