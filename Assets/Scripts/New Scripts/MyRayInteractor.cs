using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class MyRayInteractor : XRRayInteractor
{
    public bool couldDelete = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnSelectExited(XRBaseInteractable interactor)
    {
        if(interactor.gameObject.tag == "gold cube"){
            GameObject leftCube = GameObject.FindGameObjectWithTag("left gold cube");
            GameObject rightCube = GameObject.FindGameObjectWithTag("right gold cube");

            if(GameManager.instance.tag == "host")
            {
                if (leftCube.GetComponent<GoldCubeHalf>().currentZone != "BuildWall")
                {
                    PhotonNetwork.Destroy(leftCube);
                }
            }
            if (GameManager.instance.tag == "cliant")
            {
                if (rightCube.GetComponent<GoldCubeHalf>().currentZone != "BuildWall")
                {
                    PhotonNetwork.Destroy(rightCube);
                }
            }


        }
        PhotonNetwork.Destroy(gameObject);
        //PV.RPC("DecreaseGoldCubeNetworkVar", RpcTarget.AllBuffered);
    }

    public void CheckDelete(GameObject obj)
    {
        if (couldDelete)
        {
            PhotonNetwork.Destroy(obj);
            couldDelete = false;
        }
    }
    protected override void OnHoverExited(XRBaseInteractable obj)
    {

    }
}
