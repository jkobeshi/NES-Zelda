using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKey : MonoBehaviour
{
    public AudioClip SpawnSound19;
    public GameObject Key;
    bool stop = false;
    public float x;
    public float y;
    void Update()
    {
        if (!stop)
        {
            int count = 0;
            GameObject[] WallMasters = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject i in WallMasters)
            {
                if (i.transform.parent.gameObject == gameObject)
                {
                    ++count;
                }
            }
            if (count == 0)
            {
                if (SpawnSound19 != null)
                {
                    AudioSource.PlayClipAtPoint(SpawnSound19, Camera.main.transform.position);
                }
                GameObject CloneA = Instantiate(Key, transform.position + new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0));
                CloneA.transform.parent = gameObject.transform;
                if (CloneA.tag == "BatDoor")
                {
                    Debug.Log("Hi");
                    GameObject temp = GameObject.FindGameObjectWithTag("tempDoor");
                    Destroy(temp);
                }
                stop = true;
            }
        }
    }
}
