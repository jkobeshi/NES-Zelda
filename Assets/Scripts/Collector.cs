using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public AudioClip rupee_collection_sound_clip;
    public AudioClip hp_collection_sound_clip;
    public AudioClip key_collection_sound_clip;
    public AudioClip BowSound16;
    public AudioClip BowSound20;
    Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            inventory = Inventory.instance.gameObject.GetComponent<Inventory>();
        }
        if (inventory == null)
        {
            Debug.LogWarning("Warning: Gameobject with a collector has no inventory to store things in");
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        GameObject object_collided_with = coll.gameObject;
        if (object_collided_with.tag == "rupee")
        {
            if (inventory != null)
            {
                inventory.AddRupees(1);
            }
            Destroy(object_collided_with);
            AudioSource.PlayClipAtPoint(rupee_collection_sound_clip, Camera.main.transform.position);
        }
        else if (object_collided_with.tag == "hp")
        {
            if (inventory != null)
            {
                inventory.AddHealth(1.0f);
            }
            Destroy(object_collided_with);
            AudioSource.PlayClipAtPoint(hp_collection_sound_clip, Camera.main.transform.position);
        }
        else if (object_collided_with.tag == "key")
        {
            if (inventory != null)
            {
                inventory.AddKey(1);
            }
            Destroy(object_collided_with);

            AudioSource.PlayClipAtPoint(key_collection_sound_clip, Camera.main.transform.position);
        }
        else if (object_collided_with.tag == "bomb")
        {
            if (inventory != null)
            {
                inventory.AddBomb(1);
            }
            Destroy(object_collided_with);
            AudioSource.PlayClipAtPoint(BowSound16, Camera.main.transform.position);
        }
        else if (object_collided_with.tag == "Bow")
        {
            if (inventory != null)
            {
                Inventory.instance.BowPickup = true;
            }
            Destroy(object_collided_with);
            AudioSource.PlayClipAtPoint(BowSound16, Camera.main.transform.position);
            AudioSource.PlayClipAtPoint(BowSound20, Camera.main.transform.position);
        }
        else if (object_collided_with.tag == "BoomerangP")
        {
            if (inventory != null)
            {
                Inventory.instance.BoomerangPickup = true;
            }
            Destroy(object_collided_with);
            AudioSource.PlayClipAtPoint(BowSound16, Camera.main.transform.position);
        }
        else if (object_collided_with.tag == "GrapplingPickup")
        {
            if (inventory != null)
            {
                Inventory.instance.GrapplingPickup = true;
            }
            Destroy(object_collided_with);
            AudioSource.PlayClipAtPoint(BowSound20, Camera.main.transform.position);
        }
    }
}
