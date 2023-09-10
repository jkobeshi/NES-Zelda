using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{ 
    int PlayerLayer = 1 << 3;
    int BarrierLayer = 6;
    float[] NearbyWalls;
    Vector3 StartPos;
    public int MoveDir = 0;
    public float fowardSpeed = 5.5f;
    float backSpeed = 3.5f;
    Rigidbody rb;
    float knockBack = 10f;
    float Damage = 0.5f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        NearbyWalls = FindWalls();
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveDir == 1)
        {
            Move(StartPos, backSpeed);
        }
        else if (MoveDir == 2)
        {
            Vector3 newDest = StartPos;
            newDest.x += NearbyWalls[0];
            Move(newDest, fowardSpeed);
        }
        else if (MoveDir == 3)
        {
            Vector3 newDest = StartPos;
            newDest.x -= NearbyWalls[1];
            Move(newDest, fowardSpeed);
        }
        else if (MoveDir == 4)
        {
            Vector3 newDest = StartPos;
            newDest.y += NearbyWalls[2];
            Move(newDest, fowardSpeed);
        }
        else if (MoveDir == 5)
        {
            Vector3 newDest = StartPos;
            newDest.y -= NearbyWalls[3];
            Move(newDest, fowardSpeed);
        }
        else if (MoveDir == 0)
        {
            if (Physics.Raycast(transform.position, new Vector3(1, 0, 0), NearbyWalls[0], PlayerLayer))
            {
                MoveDir = 2;
            }
            if (Physics.Raycast(transform.position, new Vector3(-1, 0, 0), NearbyWalls[1], PlayerLayer))
            {
                MoveDir = 3;
            }
            if (Physics.Raycast(transform.position, new Vector3(0, 1, 0), NearbyWalls[2], PlayerLayer))
            {
                MoveDir = 4;
            }
            if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), NearbyWalls[3], PlayerLayer))
            {
                MoveDir = 5;
            }
            rb.velocity = Vector3.zero;
        }
    }

    void Move(Vector3 dest, float speed)
    {
        rb.velocity = (dest - rb.position).normalized * speed;
        if(Vector3.Distance(dest, rb.position) < 0.05f)
        {
            transform.position = dest;
            if (MoveDir == 1)
            {
                MoveDir = 0;
            }
            else
            {
                MoveDir = 1;
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if ((collision.gameObject.layer == BarrierLayer) && (collision.tag != "pit"))
        {
            MoveDir = 1;
        }
        if(collision.gameObject.name == "Player" && !Inventory.instance.god_mode)
        {
            Vector2 direction = new Vector2(-ArrowKeyMovement.instance.lhorz_inp, -ArrowKeyMovement.instance.lvert_inp);
            if (MoveDir == 1)
            {
                direction = (StartPos - rb.position).normalized;
            }
            else if (MoveDir == 2)
            {
                direction = new Vector3(1, 0, 0);
            }
            else if (MoveDir == 3)
            {
                direction = new Vector3(-1, 0, 0);
            }
            else if (MoveDir == 4)
            {
                direction = new Vector3(0, 1, 0);
            }
            else if (MoveDir == 5)
            {
                direction = new Vector3(0, -1, 0);
            }
            if (!ArrowKeyMovement.instance.invincible)
            {
                ArrowKeyMovement.instance.DamagePlayer(direction, knockBack, Damage);
            }
        }
    }

    float[] FindWalls()
    {
        GameObject[] Walls = GameObject.FindGameObjectsWithTag("Wall");
        float[] dist = new float[4];
        for(int i = 0; i < dist.Length; ++i)
        {
            dist[i] = Mathf.Infinity;
        }
        for(int i = 0; i < Walls.Length; ++i)
        {
            Transform pos = Walls[i].GetComponent<Transform>();
            if (pos.position.x == transform.position.x)
            {
                if ((pos.position.y > transform.position.y) && ((pos.position.y - transform.position.y - 1f) < dist[2]))
                {
                    dist[2] = (pos.position.y - transform.position.y) - 1f;
                }
                else if ((pos.position.y < transform.position.y) && ((transform.position.y - pos.position.y - 1f) < dist[3]))
                {
                    dist[3] = (transform.position.y - pos.position.y) - 1f;
                }  
            }
            else if (pos.position.y == transform.position.y)
            {
                if ((pos.position.x > transform.position.x) && ((pos.position.x - transform.position.x - 1f) < dist[0]))
                {
                    dist[0] = (pos.position.x - transform.position.x) - 1f;
                }
                else if ((pos.position.x < transform.position.x) && ((transform.position.x - pos.position.x - 1f) < dist[1]))
                {
                    dist[1] = (transform.position.x - pos.position.x) - 1f;
                }
            }
        }
        return dist;
    }
}
