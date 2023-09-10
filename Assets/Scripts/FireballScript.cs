using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.name == "Player") && !Inventory.instance.god_mode)
        {
            if (!ArrowKeyMovement.instance.invincible)
            {
                ArrowKeyMovement.instance.DamagePlayer((other.transform.position - transform.position).normalized, 6f, 0.5f);
            }
        }
    }
}
