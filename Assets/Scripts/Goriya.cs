using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goriya : Enemy
{
    //public Goriya instance;
    public GameObject GorBoom;
    public bool thrown = false;

    void Start()
    {
        health = 3f;
        knockBack = 9f;
        direction = RandVector();
        SkelRb = GetComponent<Rigidbody>();
        EnemySprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        StartCoroutine(Randomizer());
        StartCoroutine(ThrowBoom());
    }
    public override void Update()
    {
        if (!coh && !thrown)
        {
            if (((CameraMovement.instance.gameObject.transform.position.x - 7.5f) != gameObject.transform.parent.position.x)
                || ((CameraMovement.instance.gameObject.transform.position.y - 7) != gameObject.transform.parent.position.y))
            {
                SkelResMove = true;
            }
            else
            {
                SkelResMove = false;
            }
        }
        if (!hooked)
        {
            if (SkelResMove || thrown)
            {
                ani.speed = 0.0f;
                if (!coh)
                {
                    SkelRb.velocity = Vector3.zero;
                }
                else
                {
                    if (Physics.Raycast(transform.position, tempDir, 0.7f, layerMask))
                    {
                        SkelRb.velocity = Vector3.zero;
                    }
                }
            }
            else
            {
                ani.SetFloat("horz", direction.x);
                ani.SetFloat("vert", direction.y);
                ani.speed = 1f;
                Vector2 directionOpp1 = new Vector2(direction.y, direction.x);
                Vector2 directionOpp2 = new Vector2(-direction.y, -direction.x);
                if (!Physics.Raycast(transform.position, direction, 0.5f, layerMask)
                    && !Physics.Raycast(transform.position, directionOpp1, 0.49f, layerMask)
                    && !Physics.Raycast(transform.position, directionOpp2, 0.49f, layerMask))
                {
                    Vector3 newPos1 = new Vector3(transform.position.x, Mathf.Round(transform.position.y * 2f) / 2f, 0.0f);
                    Vector3 newPos2 = new Vector3(Mathf.Round(transform.position.x * 2f) / 2f, transform.position.y, 0.0f);
                    if ((direction.x != 0) && (transform.position.y != newPos1.y))
                    {
                        SkelRb.velocity = (newPos1 - SkelRb.position).normalized * EneMoveSpeed;
                        if (Vector3.Distance(SkelRb.position, newPos1) <= 0.1f)
                        {
                            transform.position = newPos1;
                        }
                    }
                    else if ((direction.y != 0) && (transform.position.x != newPos2.x))
                    {
                        SkelRb.velocity = (newPos2 - SkelRb.position).normalized * EneMoveSpeed;
                        if (Vector3.Distance(SkelRb.position, newPos2) <= 0.1f)
                        {
                            transform.position = newPos2;
                        }
                    }
                    else
                    {
                        SkelRb.velocity = direction * EneMoveSpeed;
                    }
                }
                else
                {
                    direction = RandVector();
                }
            }
        }
    }

    IEnumerator ThrowBoom()
    {
        while (true)
        {
            if (!thrown && !SkelResMove && !hooked)
            {
                GameObject Boomer = Instantiate(GorBoom, transform.position + new Vector3(direction.x, direction.y, 0), new Quaternion(0, 0, 0, 0), transform);
            }
            yield return new WaitForSeconds(Random.Range(4f, 10f));
        }
    }
}
