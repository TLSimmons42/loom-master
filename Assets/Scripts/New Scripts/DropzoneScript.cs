using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropzoneScript : MonoBehaviour
{
    public string direction;
    public Vector2Int index;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag + " THIS IS THE TAG");
        if (GameManager.instance.playerCount == 1)
        {
            if ((other.tag == "B" || other.tag == "R" || other.tag == "G" || other.tag == "I") && (other.gameObject.GetComponent<Cube>().currentZone != "BuildWall")&& other.gameObject.GetComponent<Cube>().canDrop)
            {
                Debug.Log("drop zone script");
                Analytics.instance.WriteData(other.gameObject.name + "was placed in dropzone", "", "", transform.position.x.ToString(), transform.position.y.ToString(), transform.position.z.ToString());
                MasterBuildWall.instance.dropZoneHit(index, direction, other.gameObject);
            }
        }
        else
        if ((other.tag == "gold cube" || other.tag == "blue cube" || other.tag == "blue cube" || other.tag == "red cube" || other.tag == "invis cube") && (other.gameObject.GetComponent<XRGrabNetworkInteractable>().currentZone != "BuildWall") && other.gameObject.GetComponent<XRGrabNetworkInteractable>().canDrop)
        {
            
            Debug.Log("drop zone script");
            MasterBuildWall.instance.dropZoneHit(index, direction, other.gameObject);

        }
        else if ( other.tag == "right gold cube" || other.tag == "left gold cube" && (other.gameObject.GetComponent<GoldCubeHalf>().currentZone != "BuildWall")&& other.gameObject.GetComponent<GoldCubeHalf>().canBeDroped)
        {
            GameManager.instance.holdingGoldHalf = false;
            Debug.Log("bout to drop a cube");
            MasterBuildWall.instance.dropZoneHit(index, direction, other.gameObject);
            
        }
        else if (other.tag == "gold cube")
        {
            if (GameManager.instance.host)
            {
                MasterBuildWall.instance.dropZoneHit(index, direction, other.gameObject);
            }
        }
        else
        {
            Debug.Log("nothing happens in this collision");
        }
    }
}
