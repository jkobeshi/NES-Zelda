using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Accurate
    public GameObject HpDrop;
    public GameObject BombDrop;
    public GameObject RupeeDrop;
    public float enemyDamage = 0.5f;
    public AudioClip Dmgd9;
    public AudioClip Dead8;
    protected SpriteRenderer EnemySprite;
    public Animator ani;
    protected Rigidbody SkelRb;
    protected float health = 2;
    protected float knockBack = 9f;
    public float EneMoveSpeed = 2.5f;
    public Vector2 direction;
    public bool SkelResMove = false;
    public bool hooked = false;
    public Vector3 CameraPos;
    public Vector3 ParentPos;
    public Vector3 tempDir;
    public bool coh = false;
    public int layerMask = 1 << 6;
    private void Start()
    {
        EnemySprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        SkelRb = GetComponent<Rigidbody>();
        direction = RandVector();
        StartCoroutine(Randomizer());
        CameraPos = CameraMovement.instance.gameObject.transform.position;
    }

    public virtual void Update()
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

    public virtual IEnumerator Randomizer()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            direction = RandVector();
        }
    }

    public virtual Vector2 RandVector()
    {
        int dir = Random.Range(1, 5);
        if (dir == 1)
        {
            return new Vector2(1, 0);
        }
        else if (dir == 2)
        {
            return new Vector2(-1, 0);
        }
        else if (dir == 3)
        {
            return new Vector2(0, 1);
        }
        else
        {
            return new Vector2(0, -1);
        }

    }
    public virtual void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.name == "Player") && !Inventory.instance.god_mode)
        {
            if (!ArrowKeyMovement.instance.invincible)
            {
                ArrowKeyMovement.instance.DamagePlayer((other.transform.position - SkelRb.position).normalized, knockBack, enemyDamage);
            }
        }
        if ((other.tag == "pit") && (Vector2.Distance(other.transform.position, transform.position) <= 0.5f) && !hooked)
        {
            DmgEnemy(20f, 0f, 0f, Vector3.zero);
        }
    }
    public virtual void DmgEnemy(float dmg, float stun, float wepKB, Vector3 dir)
    {
        if ((dmg > 0.5f) || ((dmg == 0.5f) && (health == 0.5f)) )
        {
            health -= dmg;
            StartCoroutine(DmgColor());
        }
        StartCoroutine(EneDmgCoroutine(stun, wepKB, dir));
    }

    public IEnumerator DmgColor()
    {
        EnemySprite.color = Color.red;
        yield return new WaitForSeconds(0.07f);
        EnemySprite.color = Color.white;
        yield return new WaitForSeconds(0.07f);
        EnemySprite.color = Color.red;
        yield return new WaitForSeconds(0.07f);
        EnemySprite.color = Color.white;
        yield return new WaitForSeconds(0.07f);
        EnemySprite.color = Color.red;
        yield return new WaitForSeconds(0.07f);
        EnemySprite.color = Color.white;
    }
    public IEnumerator EneDmgCoroutine(float stunDur, float wepKb, Vector3 dir)
    {
        coh = true;
        SkelResMove = true;

        if (health <= 0)
        {
            Drop();
            AudioSource.PlayClipAtPoint(Dead8, Camera.main.transform.position);
            if (gameObject.name != "Aquamentus")
            {
                transform.DetachChildren();
            }
            Destroy(gameObject);
        }
        AudioSource.PlayClipAtPoint(Dmgd9, Camera.main.transform.position);

        tempDir = dir;
        SkelRb.velocity = dir * wepKb;

        yield return new WaitForSeconds(stunDur);
        SkelResMove = false;
        coh = false;
        yield return new WaitForSeconds(0.1f);
        ani.speed = 0.4f;
    }

    protected void Drop()
    {
        int prob = Random.Range(0, 100);
        if(prob < 30)
        {
            prob = Random.Range(0, 10);
            if(prob < 1)
            {
                Instantiate(BombDrop, transform.position, new Quaternion(0, 0, 0, 0));
            }
            else if (prob < 3)
            {
                Instantiate(HpDrop, transform.position, new Quaternion(0, 0, 0, 0));
            }
            else
            {
                Instantiate(RupeeDrop, transform.position, new Quaternion(0, 0, 0, 0));
            }
        }
    }
}
