using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcut_Controller : MonoBehaviour
{
    public bool Unlocked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Unlock(bool status)
    {
        Unlocked = status;
        if (status == true)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
