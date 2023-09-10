using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keese : Enemy
{
    public Rigidbody keese;
    float parX;
    float parY;
    public float aniSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        SkelRb = GetComponent<Rigidbody>();
        keese = GetComponent<Rigidbody>();
        direction = Random.insideUnitCircle;
        EnemySprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        parX = transform.parent.transform.position.x;
        parY = transform.parent.transform.position.y;
        StartCoroutine(RandMove());
        EneMoveSpeed = 3f;
        knockBack = 7f;
        health = 0.5f;
        StartCoroutine(Rest());
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!coh)
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
            if (SkelResMove)
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
                ani.speed = aniSpeed;
                if (transform.position.x < (parX + 1f))
                {
                    direction = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                }
                if (transform.position.x > (parX + 14f))
                {
                    direction = new Vector2(Random.Range(-1.0f, 0.0f), Random.Range(-1.0f, 1.0f));
                }
                if (transform.position.y < (parY + 1f))
                {
                    direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 1.0f));
                }
                if (transform.position.y > (parY + 9f))
                {
                    direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 0.0f));
                }
                keese.velocity = direction * EneMoveSpeed;
            }
        }
    }

    IEnumerator Rest()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(16f, 25f));
            while ((aniSpeed > 0) && (EneMoveSpeed > 0))
            {
                aniSpeed -= 0.1f;
                EneMoveSpeed -= 0.3f;
                if(EneMoveSpeed < 0 || aniSpeed < 0)
                {
                    aniSpeed = 0;
                    EneMoveSpeed = 0;
                }
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(2f);
            while ((aniSpeed < 1) && (EneMoveSpeed < 3))
            {
                aniSpeed += 0.1f;
                EneMoveSpeed += 0.3f;
                if (EneMoveSpeed > 3 || aniSpeed > 1)
                {
                    aniSpeed = 1;
                    EneMoveSpeed = 3;
                }
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    IEnumerator RandMove()
    {
        yield return new WaitForSeconds(4f);
        direction = Random.insideUnitCircle;
    }
}
