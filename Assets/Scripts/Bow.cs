using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public AudioClip BowAttk1;
    Rigidbody rb;
    float BowSpeed = 15f;
    float BowDmg = 2f;
    float stunDur = 0.2f;
    float bowKB = 16f;
    Vector3 PlayerStart;
    private void Start()
    {  
        rb = GetComponent<Rigidbody>();
        float lx = ArrowKeyMovement.instance.lhorz_inp; float ly = ArrowKeyMovement.instance.lvert_inp;
        Vector2 current_input = new Vector2(lx, ly); rb.velocity = current_input * BowSpeed;
        AudioSource.PlayClipAtPoint(BowAttk1, Camera.main.transform.position);
        PlayerStart = ArrowKeyMovement.instance.rb.position;
    }
    private void Update()
    {
        Vector3 CameraPos = CameraMovement.instance.gameObject.transform.position;
        if ((Mathf.Abs(CameraPos.x - transform.position.x) >= 7.5f)
            || ((transform.position.y - CameraPos.y) >= 3.5)
            || ((CameraPos.y - transform.position.y) >= 7))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().DmgEnemy(BowDmg, stunDur, bowKB, (transform.position - PlayerStart).normalized);
            Destroy(gameObject);
        }
    }
}
