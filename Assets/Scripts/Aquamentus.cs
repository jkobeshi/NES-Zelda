using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aquamentus : Enemy
{
    public AudioClip Yelling;
    AudioSource yel;
    Vector3 max;
    bool play = true;
    // Start is called before the first frame update
    void Start()
    {
        health = 6f;
        SkelRb = GetComponent<Rigidbody>();
        EnemySprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        yel = GetComponent<AudioSource>();
        yel.clip = Yelling;
        max = transform.position;
        direction = new Vector3(1f, 0, 0);
        EneMoveSpeed = 1.5f;
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
                if (play)
                {
                    StartCoroutine(MovementSound());
                }
                ani.speed = 1f;
                if (transform.position.x >= max.x + 1.5f)
                {
                    direction = new Vector3(-1, 0, 0);
                }
                else if (transform.position.x <= max.x)
                {
                    direction = new Vector3(1, 0, 0);
                }
                SkelRb.velocity = direction * EneMoveSpeed;
                if (AquaShoot.fire == true)
                {
                    ani.SetTrigger("Fire");
                }
                else
                {
                    ani.ResetTrigger("Fire");
                }
            }
        }
    }
    public override void DmgEnemy(float dmg, float stun, float wepKB, Vector3 dir)
    {
        if (health <= 0)
        {
            yel.Stop();
        }
        if ((dmg > 0.5f) || ((dmg == 0.5f) && (health == 0.5f)))
        {
            health -= dmg;
            StartCoroutine(DmgColor());
        }
        StartCoroutine(EneDmgCoroutine(stun, 0.0f, dir));
    }

    IEnumerator MovementSound()
    {
        play = false;
        yel.Play();
        yield return new WaitForSeconds(10f);
        play = true;
    }
}
