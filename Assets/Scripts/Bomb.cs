using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject expld;
    float explosionRadius = 1.5f;
    GameObject[] EnemiesInRange;
    float ExplosionTimer = 1f;
    float Damage = 3f;
    float stun = 0.0f;
    float BombKb = 0.0f;
    public AudioClip Place3;
    public AudioClip Explode10;
    private void Start()
    {
        StartCoroutine(Explode());
        AudioSource.PlayClipAtPoint(Place3, Camera.main.transform.position);
    }
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(ExplosionTimer);
        AudioSource.PlayClipAtPoint(Explode10, Camera.main.transform.position);
        Instantiate(expld, transform.position, new Quaternion(0,0,0,0));
        Instantiate(expld, transform.position + new Vector3(0.9f, 0, 0), new Quaternion(0, 0, 0, 0));
        Instantiate(expld, transform.position + new Vector3(-0.9f, 0, 0), new Quaternion(0, 0, 0, 0));
        Instantiate(expld, transform.position + new Vector3(0.4f, 0.9f, 0), new Quaternion(0, 0, 0, 0));
        Instantiate(expld, transform.position + new Vector3(-0.4f, 0.9f, 0), new Quaternion(0, 0, 0, 0));
        Instantiate(expld, transform.position + new Vector3(0.4f, -0.9f, 0), new Quaternion(0, 0, 0, 0));
        Instantiate(expld, transform.position + new Vector3(-0.4f, -0.9f, 0), new Quaternion(0, 0, 0, 0));
        EnemiesInRange = FindEnemies(explosionRadius);
        foreach (GameObject i in EnemiesInRange)
        {
            if (i != null)
            {
                i.GetComponent<Enemy>().DmgEnemy(Damage, stun, BombKb, (i.transform.position - transform.position).normalized);
            }
        }
        Destroy(gameObject);
    }

    GameObject[] FindEnemies(float radius)
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] inRange = new GameObject[enemies.Length];
        int j = 0;
        for(int i = 0; i < enemies.Length; ++i)
        {
            if(Mathf.Sqrt((enemies[i].transform.position.x - transform.position.x) * (enemies[i].transform.position.x - transform.position.x)
                + (enemies[i].transform.position.y - transform.position.y) * (enemies[i].transform.position.y - transform.position.y)) <= radius)
            {
                inRange[j] = enemies[i];
                ++j;
            }
        }
        return inRange;
    }
}
