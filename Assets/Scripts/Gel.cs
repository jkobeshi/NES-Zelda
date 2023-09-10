using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gel : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        direction = RandVector();
        SkelRb = GetComponent<Rigidbody>();
        EnemySprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        StartCoroutine(Randomizer());
        health = 0.5f;
        knockBack = 7f;
    }
}
