using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooking : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hooked = false;
    Rigidbody rb;
    public Rigidbody HookedRb;
    Vector3 StartPos;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartPos = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (hooked)
        {
            HookedRb.velocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                HookedRb.velocity = (StartPos - HookedRb.position).normalized * 4f;
                rb.velocity = (HookedRb.position - rb.position).normalized * 4f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hooked)
        {
            if (other.tag == "Wall")
            {
                transform.parent.GetComponent<Grappling>().RetGrap();
            }
            if ((Vector2.Distance(other.transform.position, transform.position) <= 0.5f) && (other.GetComponent<Rigidbody>() != null) && (other.tag != "Wep"))
            {
                HookedRb = other.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                HookedRb.velocity = Vector3.zero;
                if(other.tag == "Enemy")
                {
                    other.GetComponent<Enemy>().hooked = true;
                    other.GetComponent<Enemy>().ani.speed = 0.0f;
                }
                hooked = true;
            }
        }
    }
}
