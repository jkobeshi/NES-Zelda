using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public AudioClip Boomerang1;
    Rigidbody rb;
    float stunDuration = 1.5f;
    float boomkb = 0f;
    float BoomSpeed = 9f;
    float BoomDmg = 0.5f;
    float distCanTravel = 5f;
    float lx, ly;
    Vector3 startLoc;
    Vector3 PlayerStart;
    bool comeBack = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lx = ArrowKeyMovement.instance.lhorz_inp; ly = ArrowKeyMovement.instance.lvert_inp;
        Vector2 current_input = new Vector2(lx, ly); rb.velocity = current_input * BoomSpeed;
        Inventory.instance.threwC = true;
        startLoc = rb.position;
        PlayerStart = ArrowKeyMovement.instance.rb.position;
        StartCoroutine(BoomerangThrow());
    }

    private void Update()
    {
        if (comeBack)
        {
            rb.velocity = (ArrowKeyMovement.instance.rb.position - rb.position).normalized * BoomSpeed;
        }
        if(Vector3.Distance(startLoc, rb.position) >= distCanTravel)
        {
            comeBack = true;
        }
        Vector3 CameraPos = CameraMovement.instance.gameObject.transform.position;
        if ((Mathf.Abs(CameraPos.x - transform.position.x) >= 7.5f)
            || ((transform.position.y - CameraPos.y) >= 3.5)
            || ((CameraPos.y - transform.position.y) >= 7))
        {
            comeBack = true;
        }
    }

    IEnumerator BoomerangThrow()
    {
        while (true)
        {
            AudioSource.PlayClipAtPoint(Boomerang1, Camera.main.transform.position);
            yield return new WaitForSeconds(0.175f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().DmgEnemy(BoomDmg, stunDuration, boomkb, (transform.position - PlayerStart).normalized);
            comeBack = true;
        }
        if (other.gameObject.name == "Player")
        {
            comeBack = false;
            Inventory.instance.threwC = false;
            Destroy(gameObject);
        }
    }
}
