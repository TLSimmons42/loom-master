using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabNetworkInteractable : XRGrabInteractable
{

    private BoxCollider collider;
    private PhotonView photonView;
    private Transform currentPos;
    Cube cube;

    private int playersHoldingCube = 0;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        collider = GetComponent<BoxCollider>();
        cube = GetComponent<Cube>();
    }

    void Update()
    {

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
                cube.GoldHold(gameObject.transform.position);
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
}
