using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaShoot : MonoBehaviour
{
    public AudioClip FireSound;
    public GameObject fireball;
    public static bool fire = true;
    float rate = 2f;
    float speed = 4.5f;

    // Update is called once per frame
    public void Update()
    {
        rate -= Time.deltaTime;
        if (rate <= 0)
        {
            rate = 2f;
            Vector3 CameraPos = CameraMovement.instance.gameObject.transform.position;
            if (!gameObject.transform.parent.gameObject.GetComponent<Aquamentus>().SkelResMove)
            {
                Shoot();
            }
        }
    }
    void Shoot()
    {
        AudioSource.PlayClipAtPoint(FireSound, Camera.main.transform.position);
        StartCoroutine(ShotsFired(0f));
        StartCoroutine(ShotsFired(2.5f));
        StartCoroutine(ShotsFired(-2.5f));
    }

    public IEnumerator ShotsFired(float offset)
    {
        Vector3 playerPos = ArrowKeyMovement.instance.rb.position;
        GameObject clone;
        clone = Instantiate(fireball, transform.position, transform.rotation);
        Rigidbody rb = clone.GetComponent<Rigidbody>();
        fire = true;
        rb.velocity = ((ArrowKeyMovement.instance.rb.position + new Vector3(0, offset, 0)) - rb.position).normalized * speed;
        fire = false;
        yield return new WaitForSeconds(3);
        Destroy(clone);
        yield return null;
    }

}
