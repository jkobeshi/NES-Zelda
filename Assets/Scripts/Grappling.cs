using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    float lh_x;
    float lv_x;
    Vector3 dir;
    float shootSpeed = 10f; 
    Rigidbody hookRb;
    public AudioClip Hook1;
    // Start is called before the first frame update
    void Start()
    {
        hookRb = transform.GetChild(0).GetComponent<Rigidbody>();
        lh_x = ArrowKeyMovement.instance.lhorz_inp;
        lv_x = ArrowKeyMovement.instance.lvert_inp;
        dir = new Vector3(lh_x, lv_x, 0);
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        ArrowKeyMovement.instance.restrictMovement = true;
        ArrowKeyMovement.instance.kb = false;
        if (Input.GetKeyDown("z"))
        {
            RetGrap();
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.6f);
        hookRb.velocity = dir * shootSpeed;
        AudioSource.PlayClipAtPoint(Hook1, Camera.main.transform.position);
    }

    public void RetGrap()
    {
        if (hookRb.GetComponent<Hooking>().hooked == true)
        {
            if (hookRb.GetComponent<Hooking>().HookedRb.GetComponent<Enemy>() != null)
            {
                hookRb.GetComponent<Hooking>().HookedRb.GetComponent<Enemy>().hooked = false;
            }
        }
        ArrowKeyMovement.instance.lhorz_inp = lh_x;
        ArrowKeyMovement.instance.lvert_inp = lv_x;
        ArrowKeyMovement.instance.restrictMovement = false;
        Destroy(gameObject);
    }
}
