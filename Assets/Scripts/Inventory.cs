using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public AudioClip ChangeScene;
    public GameObject Sword;
    public GameObject Bow;
    public GameObject Boomerang;
    public GameObject Bomb;
    public GameObject Grappling;
    public bool threw = false;
    public bool threwB = false;
    public bool threwC = false;
    int rupee_count = 0;
    int bomb_count = 0;
    public bool god_mode = false;
    public int key_count = 0;
    public float health = 3.0f;
    int lastRup_count = 0;
    int lastBomb_count = 0;
    int lastKey_count = 0;
    float lastHealth_count = 3.0f;
    public bool BowPickup = false;
    public bool BoomerangPickup = false;
    bool LastBow = false;
    bool Last_Boom = false;
    public bool GrapplingPickup = false;
    public GameObject OnHandA;
    public GameObject OnHandB;
    float waitB4shot = 0.8f;
    int Hand = 2;
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
    private void Start()
    {
        OnHandA = Sword;
        OnHandB = Bomb;
    }

    void Update()
    {
        if (Input.GetKeyDown("x") && !ArrowKeyMovement.instance.restrictMovement)
        {
            GameObject CloneA;
            CloneA = Instantiate(OnHandA, transform.position + new Vector3(ArrowKeyMovement.instance.lhorz_inp,
                ArrowKeyMovement.instance.lvert_inp, 0), Rotat());
        }
        if (Input.GetKeyDown("z") && !ArrowKeyMovement.instance.restrictMovement)
        {
            if (((Hand == 0) && !threwB && (rupee_count > 0)) || ((Hand == 1) && !threwC) || ((Hand == 2) && (bomb_count > 0)) || (Hand == 3)) {
                GameObject CloneB;
                Quaternion Rot = Rotat();
                if(Hand == 2)
                {
                    Rot = new Quaternion(0, 0, 0, 0);
                }
                CloneB = Instantiate(OnHandB, transform.position + new Vector3(ArrowKeyMovement.instance.lhorz_inp,
                    ArrowKeyMovement.instance.lvert_inp, 0), Rot);
                if (Hand != 3)
                {
                    StartCoroutine(Throw(0.3f));
                }
                if (Hand == 0)
                {
                    StartCoroutine(shootWait());
                    rupee_count--;
                }
                else if(Hand == 2)
                {
                    bomb_count--;
                }
            }
        }
        if (Input.GetKeyDown("space"))
        {
            SwitchWepB();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!god_mode)
            {
                lastRup_count = rupee_count;
                lastBomb_count = bomb_count;
                lastKey_count = key_count;
                lastHealth_count = health;
                LastBow = BowPickup;
                Last_Boom = BoomerangPickup;
                god_mode = true;
                rupee_count = 99;
                bomb_count = 99;
                key_count = 99;
                health = 3;
                BowPickup = true;
                BoomerangPickup = true;
            }
            else
            {
                god_mode = false;
                rupee_count = lastRup_count;
                bomb_count = lastBomb_count;
                key_count = lastKey_count;
                health = lastHealth_count;
                BowPickup = LastBow;
                BoomerangPickup = Last_Boom;
                Hand = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(SceneChange());
        }
    }

    IEnumerator SceneChange()
    {
        ArrowKeyMovement.instance.restrictMovement = true;
        ArrowKeyMovement.instance.kb = false;
        yield return new WaitForSeconds(0.2f);
        Camera.main.GetComponent<AudioSource>().Pause();
        AudioSource.PlayClipAtPoint(ChangeScene, Camera.main.transform.position);
        yield return new WaitForSeconds(3f);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene("CustomMap");
        }
        else
        {
            SceneManager.LoadScene("Zelda");
        }
    }

    public IEnumerator shootWait()
    {
        threwB = true;
        yield return new WaitForSeconds(waitB4shot);
        threwB = false;
    }
    void SwitchWepB()
    {
        if (Hand == 0)
        {
            if (BoomerangPickup)
            {
                OnHandB = Boomerang;
                Hand = 1;
            }
            else
            {
                OnHandB = Bomb;
                Hand = 2;
            }
        }
        else if (Hand == 1)
        {
            OnHandB = Bomb;
            ++Hand;
        }
        else if (Hand == 2)
        {
            if (GrapplingPickup)
            {
                OnHandB = Grappling;
                Hand = 3;
            }
            else
            {
                if (BowPickup)
                {
                    OnHandB = Bow;
                    Hand = 0;
                }
                else
                {
                    if (BoomerangPickup)
                    {
                        OnHandB = Boomerang;
                        Hand = 1;
                    }
                    else
                    {
                        OnHandB = Bomb;
                        Hand = 2;
                    }
                }
            }
        }
        else
        {
            if (BowPickup)
            {
                OnHandB = Bow;
                Hand = 0;
            }
            else
            {
                if (BoomerangPickup)
                {
                    OnHandB = Boomerang;
                    Hand = 1;
                }
                else
                {
                    OnHandB = Bomb;
                    Hand = 2;
                }
            }
        }
    }

    Quaternion Rotat()
    {
        float x = 0, y = 0, z = 0, w = 0;
            if (ArrowKeyMovement.instance.lhorz_inp > 0.0f)
            { x = -0.7071f; y = 0.7071f; }
            else if (ArrowKeyMovement.instance.lhorz_inp < 0.0f)
            { x = 0.7071f; y = 0.7071f; }
            else if (ArrowKeyMovement.instance.lvert_inp > 0.0f)
                x = 1;
        return new Quaternion(x, y, z, w);
    }

    public void AddRupees(int num_rupees)
    {
        rupee_count += num_rupees;
    }

    public int GetRupees()
    {
        return rupee_count;
    }
    public void AddKey(int num_keys)
    {
        key_count += num_keys;
    }

    public int GetKey()
    {
        return key_count;
    }
    public void AddHealth(float num_health)
    {
        if (health + num_health >= 3.0f)
        {
            health = 3.0f;
        }
        else
        {
            health += num_health;
        }
    }
    public float GetHealth()
    {
        return health;
    }

    public void AddBomb(int num_bomb)
    {
        bomb_count += num_bomb;
    }
    public int GetBomb()
    {
        return bomb_count;
    }

    public int GetWep()
    {
        return Hand;
    }
    IEnumerator Throw(float tim)
    {
        ArrowKeyMovement.instance.restrictMovement = true;
        yield return new WaitForSeconds(tim);
        ArrowKeyMovement.instance.restrictMovement = false;
    }
}
