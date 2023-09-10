using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingtile : MonoBehaviour
{
    public GameObject obj;
    Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            StartCoroutine(Collapse());
        }
    }

    IEnumerator Collapse()
    {
        GameObject pit;
        ani.SetTrigger("Falling");
        yield return new WaitForSeconds(2f);
        pit = Instantiate(obj, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
