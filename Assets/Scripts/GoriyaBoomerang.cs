using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoriyaBoomerang : MonoBehaviour
{
    Rigidbody rb;
    float BoomSpeed = 5f;
    public float BoomDmg = 0.5f;
    float distCanTravel = 5.5f;

    float lx, ly;
    Vector3 startLoc;
    bool comeBack = false;
    GameObject Parent;
    Goriya ParentGoriya;
    Vector3 CameraPos;

    private void Start()
    {
        Parent = transform.parent.gameObject;
        ParentGoriya = Parent.GetComponent<Goriya>();
        rb = GetComponent<Rigidbody>();
        lx = ParentGoriya.direction.x; ly = ParentGoriya.direction.y;
        Vector2 current_input = new Vector2(lx, ly); rb.velocity = current_input * BoomSpeed;
        ParentGoriya.thrown = true;
        startLoc = rb.position;
        CameraPos = CameraMovement.instance.gameObject.transform.position;
        transform.parent.GetComponent<Goriya>().SkelResMove = true;
    }

    private void Update()
    {
        if (comeBack)
        {
            rb.velocity = (Parent.transform.position - rb.position).normalized * BoomSpeed;
        }
        if (Vector3.Distance(startLoc, transform.position) >= distCanTravel)
        {
            comeBack = true;
        }
        if ((Mathf.Abs(CameraPos.x - transform.position.x) >= 7.5f)
            || ((transform.position.y - CameraPos.y) >= 3.5)
            || ((CameraPos.y - transform.position.y) >= 7))
        {
            comeBack = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            ParentGoriya.thrown = false;
            comeBack = false;
            transform.parent.GetComponent<Goriya>().SkelResMove = false;
            Destroy(gameObject);
        }
        if ((other.gameObject.name == "Player") && !Inventory.instance.god_mode)
        {
            comeBack = true;
            if (!ArrowKeyMovement.instance.invincible)
            {
                ArrowKeyMovement.instance.DamagePlayer((other.transform.position - rb.position).normalized, 6f, BoomDmg);
            }
        }
    }
}
