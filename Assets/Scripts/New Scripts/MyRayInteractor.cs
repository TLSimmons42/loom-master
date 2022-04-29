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
    public void CheckDelete(GameObject obj)
    {
        if (couldDelete)
        {
            PhotonNetwork.Destroy(obj);
            couldDelete = false;
        }
    }

    [PunRPC]

    public void DeleteObjsOverNetwork(GameObject obj1, GameObject obj2)
    {
        PhotonNetwork.Destroy(obj1);
        PhotonNetwork.Destroy(obj2);
    }

    protected override void OnSelectExited(XRBaseInteractable interactor)
    {
        Debug.Log("the ray is about to delete the cube");
        PhotonNetwork.Destroy(gameObject);
        Debug.Log("ray select exited");
        //if(interactor != null && interactor.gameObject.tag == "left gold cube" || interactor.gameObject.tag == "right gold cube"){
        //    GameObject leftCube = GameObject.FindGameObjectWithTag("left gold cube");
        //    GameObject rightCube = GameObject.FindGameObjectWithTag("right gold cube");
        //    Debug.Log("exited gold cube");
        //    if (GameManager.instance.tag == "host")
        //    {
        //        if (leftCube.GetComponent<GoldCubeHalf>().currentZone != "BuildWall")
        //        {
        //            Debug.Log("host destroy cube");
        //            PhotonNetwork.Destroy(leftCube);
        //        }
        //    }
        //    if (GameManager.instance.tag == "cliant")
        //    {
        //        if (rightCube.GetComponent<GoldCubeHalf>().currentZone != "BuildWall")
        //        {
        //            PhotonNetwork.Destroy(rightCube);
        //        }
        //    }


        //}
        //if (GameManager.instance.host)
        //{
        //    Debug.Log("the ray is about to delete the cube");
        //    PhotonNetwork.Destroy(gameObject);
        //}
        //PV.RPC("DecreaseGoldCubeNetworkVar", RpcTarget.AllBuffered);
    }

    protected override void OnHoverExited(XRBaseInteractable obj)
    {

    }
}
