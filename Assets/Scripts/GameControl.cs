using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    public GameObject gameOverText;
    public AudioClip Death22;
    bool res = false;
    public bool gameOver = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (gameOver && res && Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void LinkDead()
    {

        ArrowKeyMovement.instance.invincible = true;
        ArrowKeyMovement.instance.restrictMovement = true;
        gameOver = true;
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        ArrowKeyMovement.instance.gameObject.GetComponent<Animator>().speed = 1.0f;
        ArrowKeyMovement.instance.gameObject.GetComponent<Animator>().SetBool("Dead", true);
        AudioSource.PlayClipAtPoint(Death22, Camera.main.transform.position);
        yield return new WaitForSeconds(3f);
        gameOverText.SetActive(true);
        res = true;
    }
}
