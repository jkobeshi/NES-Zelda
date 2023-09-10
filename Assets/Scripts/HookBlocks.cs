using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBlocks : MonoBehaviour
{
    Rigidbody rb;
    public bool hooked = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ArrowKeyMovement.instance.restrictMovement == false)
        {
            hooked = false;
        }
        if (!hooked)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hook")
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            hooked = true;
        }
    }
}
