using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHandAvatar : MonoBehaviour
{
    public Vector3 velocity; 


    // Start is called before the first frame update
    void Start()
    {
        velocity = this.gameObject.GetComponent<Rigidbody>().velocity; // humanAvatar.velocity, humanAvatar.angularVelocity, humanAvatar.position, humanAvatar.rotation ...  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
