using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Rigidbody rb;
    float SwordSpeed = 8f;
    float SwordDmg = 1f;
    float stunDur = 0.2f;
    float swordKB = 16f;
    bool ThrownSword = false;
    bool Used = false;
    Animator anim;
    public AudioClip Attack6;
    public AudioClip Throw13;
    Vector3 PlayerStart;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        StartCoroutine(SwordCoroutine());
        anim.speed = 0.0f;
        PlayerStart = ArrowKeyMovement.instance.rb.position;
    }

    private void Update()
    {
        if (Inventory.instance.threw) {
            Vector3 CameraPos = CameraMovement.instance.gameObject.transform.position;
            if ((Mathf.Abs(CameraPos.x - transform.position.x) >= 7.5f)
                || ((transform.position.y - CameraPos.y) >= 3.5)
                || ((CameraPos.y - transform.position.y) >= 7))
            {
                Inventory.instance.threw = false;
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Used = true;
            other.GetComponent<Enemy>().DmgEnemy(SwordDmg, stunDur, swordKB, (transform.position - PlayerStart).normalized);
            if (ThrownSword)
            {
                Inventory.instance.threw = false;
                Destroy(gameObject);
            }
        }
    }
    IEnumerator SwordCoroutine()
    {
        AudioSource.PlayClipAtPoint(Attack6, Camera.main.transform.position);
        float lx = ArrowKeyMovement.instance.lhorz_inp; float ly = ArrowKeyMovement.instance.lvert_inp;
        ArrowKeyMovement.instance.restrictMovement = true;
        yield return new WaitForSeconds(0.4f);
        ArrowKeyMovement.instance.restrictMovement = false;
        if ((Inventory.instance.health == 3.0f) && !Inventory.instance.threw && !Used)
        {
            ThrownSword = true;
            AudioSource.PlayClipAtPoint(Throw13, Camera.main.transform.position);
            anim.speed = 1.5f;
            Inventory.instance.threw = true;
            Vector2 current_input = new Vector2(lx, ly);
            rb.velocity = current_input * SwordSpeed;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
