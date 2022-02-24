using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyetrackerCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "blue cube")
        {
            Analytics.instance.WriteData("looking at blue cube", "", "", gameObject.transform.position.x.ToString(), gameObject.transform.position.y.ToString(), gameObject.transform.position.z.ToString());
            Debug.Log("looking at: " + other.ToString());
        }
        if (other.gameObject.tag == "red cube")
        {
            Analytics.instance.WriteData("looking at red cube", "", "", gameObject.transform.position.x.ToString(), gameObject.transform.position.y.ToString(), gameObject.transform.position.z.ToString());
            Debug.Log("looking at: " + other.ToString());
        }
        if (other.gameObject.tag == "invis cube")
        {
            Analytics.instance.WriteData("looking at invis cube", "", "", gameObject.transform.position.x.ToString(), gameObject.transform.position.y.ToString(), gameObject.transform.position.z.ToString());
            Debug.Log("looking at: " + other.ToString());
        }
        if (other.gameObject.tag == "gold cube")
        {
            Analytics.instance.WriteData("looking at gold cube", "", "", gameObject.transform.position.x.ToString(), gameObject.transform.position.y.ToString(), gameObject.transform.position.z.ToString());
            Debug.Log("looking at: " + other.ToString());
        }
    }
}
