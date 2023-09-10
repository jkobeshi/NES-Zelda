using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wallmaster : Enemy
{
    public Collider coll;
    public bool aggro = false;
    public bool ret = false;
    Vector3 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        EnemySprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        SkelRb = GetComponent<Rigidbody>();
        health = 2f;
        EneMoveSpeed = 1.5f;
        StartPos = SkelRb.position;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!coh)
        {
            if (((CameraMovement.instance.gameObject.transform.position.x - 7.5f) != gameObject.transform.parent.position.x)
                || ((CameraMovement.instance.gameObject.transform.position.y - 7) != gameObject.transform.parent.position.y))
            {
                aggro = false;
                SkelResMove = true;
            }
            else
            {
                SkelResMove = false;
            }
        }
        if (!ArrowKeyMovement.instance.invincible && !SkelResMove && !ret && !coh)
        {
            Aggro();
        }
        if (!hooked)
        {
            if (SkelResMove)
            {
                ani.speed = 0.0f;
                if (!coh)
                {
                    SkelRb.velocity = Vector3.zero;
                }
            }
            else if (aggro)
            {
                ani.speed = 1f;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                SkelRb.velocity = (ArrowKeyMovement.instance.rb.position - SkelRb.position).normalized * EneMoveSpeed;
            }
            else if (ret)
            {
                ani.speed = 0.0f;
                if (Vector2.Distance(StartPos, SkelRb.position) <= 0.1f)
                {
                    ArrowKeyMovement.instance.kb = false;
                    SkelResMove = true; coh = false; ret = false;
                    transform.position = StartPos;
                    ArrowKeyMovement.instance.transform.position = StartPos;
                    StartCoroutine(resetScene());

                }
                else
                {
                    ArrowKeyMovement.instance.rb.velocity = (SkelRb.position - ArrowKeyMovement.instance.rb.position).normalized * EneMoveSpeed;
                    SkelRb.velocity = (StartPos - SkelRb.position).normalized * EneMoveSpeed;
                }
            }
            else
            {
                if (Vector2.Distance(StartPos, SkelRb.position) <= 0.1f)
                {
                    SkelRb.position = StartPos;
                }
                else
                {
                    SkelRb.velocity = (StartPos - SkelRb.position).normalized * EneMoveSpeed;
                }
            }
        }
    }
    void Aggro()
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, 4.0f);
        foreach (var collider in collidersInRange)
        {
            if ((collider.gameObject.name == "Player"))
            {
                coll = collider;
                aggro = true;
                EnemySprite.sortingOrder = 3;
            }
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.name == "Player") && !Inventory.instance.god_mode && (Vector3.Distance(other.transform.position, SkelRb.position) <= 0.1f) && !coh)
        {
            GameObject[] Walls = GameObject.FindGameObjectsWithTag("Wall");
            for(int i = 0; i < Walls.Length; ++i)
            {
                if(Walls[i] != null)
                {
                    if((Walls[i].transform.parent.position.x == Walls[i].transform.position.x)
                        || ((Walls[i].transform.parent.position.x + 1) == Walls[i].transform.position.x)
                        || (Walls[i].transform.parent.position.y == Walls[i].transform.position.y)
                        || ((Walls[i].transform.parent.position.y + 1)== Walls[i].transform.position.y)
                        || ((Walls[i].transform.parent.position.x + 15) == Walls[i].transform.position.x)
                        || ((Walls[i].transform.parent.position.x + 14) == Walls[i].transform.position.x)
                        || ((Walls[i].transform.parent.position.y + 10) == Walls[i].transform.position.y)
                        || ((Walls[i].transform.parent.position.y + 9) == Walls[i].transform.position.y))
                    Walls[i].GetComponent<SpriteRenderer>().sortingOrder = 4;
                }
            }
            GameObject[] WallMast = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i = 0; i < WallMast.Length; ++i)
            {
                if(WallMast[i] != null)
                {
                    if((WallMast[i].transform.parent == transform.parent) && (WallMast[i].GetComponent<Wallmaster>() != null))
                    {
                        WallMast[i].GetComponent<Wallmaster>().aggro = false;
                        WallMast[i].GetComponent<Wallmaster>().coh = true;
                    }
                }
            }

            ArrowKeyMovement.instance.restrictMovement = true;
            ArrowKeyMovement.instance.GetComponent<Collider>().isTrigger = true;
            ArrowKeyMovement.instance.kb = true;
            transform.position = other.transform.position;
            ret = true; aggro = false; coh = true;
        }
    }
    IEnumerator resetScene()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
