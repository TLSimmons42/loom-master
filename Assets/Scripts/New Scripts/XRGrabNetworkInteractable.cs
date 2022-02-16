﻿using Photon.Pun;
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

        Debug.Log("the tag is: " + interactor.transform.parent.gameObject.tag);
        Debug.Log("the NAME is: " + interactor.transform.parent.parent.gameObject.name);

        if (gameObject.tag == "gold cube")
        {
            Debug.Log("gold cube was hit");
            playersHoldingCube++;
            if (playersHoldingCube == 2)
            {
                // spawn gold half
                NetworkManager.Destroy(gameObject);
            }
            else
            {
                Debug.Log("gold cube zone change");

                cube.GoldHold(gameObject.transform.position);
                //PlayerGrab();
            }

        }
        else if (interactor.transform.parent.parent.gameObject.tag == "P1")
        {
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
        //rb.detectCollisions = false;
        //gameObject.layer = 2;
        //isHeld = true;
        //Debug.Log("GRABBED cube");
    }
}
