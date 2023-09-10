using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowKeyMovement : MonoBehaviour
{
    public static ArrowKeyMovement instance;
    public Rigidbody rb;
    public GameObject openDoor;
    public GameObject openDoorL;
    public GameObject openDoorR;
    public GameObject OldManRoom;
    public GameObject MoveBlock1;
    public GameObject MoveBlock2;
    public GameObject Door;
    SpriteRenderer Link_Sprite;
    public float movement_speed = 4;
    public float lhorz_inp = 0f;
    public float lvert_inp = 1f;
    public bool restrictMovement = false;
    public bool kb = false;
    public bool invincible = false;
    bool OldManOn = false;
    bool CameraMove = true;
    int layerMask = 1 << 6;
    bool vertMove = true;
    bool opened = false;
    public AudioClip OpenDoor;
    public AudioClip MoveBlock;
    public Scene WallMasterScene;
    public bool pushed = false;
    Vector3 Block1;
    Vector3 Block2;
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

    void Start()
    {
        if (MoveBlock1 != null)
        {
            Block1 = MoveBlock1.transform.position;
            Block2 = MoveBlock2.transform.position;
        }
        Link_Sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.instance.gameOver)
        {
            restrictMovement = true;
            kb = false;
            invincible = true;
        }
        float lastX = lhorz_inp; float lastY = lvert_inp;
        UpdateInput();
        if (!restrictMovement)
        {
            Vector2 current_input = GetInput();
            Vector3 newPos1 = new Vector3(transform.position.x, Mathf.Round(transform.position.y * 2f) / 2f, 0.0f);
            Vector3 newPos2 = new Vector3(Mathf.Round(transform.position.x * 2f) / 2f, transform.position.y, 0.0f);
            if ((Input.GetAxisRaw("Horizontal") != 0) && (transform.position.y != newPos1.y))
            {
                rb.velocity = (newPos1 - rb.position).normalized * movement_speed;
                if(Vector3.Distance(rb.position, newPos1) <= 0.15f)
                {
                    transform.position = newPos1;
                }
            }
            else if ((Input.GetAxisRaw("Vertical") != 0) && (Input.GetAxisRaw("Horizontal") == 0) && (transform.position.x != newPos2.x))
            {
                rb.velocity = (newPos2 - rb.position).normalized * movement_speed;
                if (Vector3.Distance(rb.position, newPos2) <= 0.15f)
                {
                    transform.position = newPos2;
                }
            }
            else
            {
                rb.velocity = current_input * movement_speed;
            }
        }
        else if (!kb)
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
        }
    }

    Vector2 GetInput()
    {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(horizontal_input) > 0.0f)
        {
            vertical_input = 0.0f;
        }
        if (!vertMove)
        {
            vertical_input = 0.0f;
        }
        return new Vector2(horizontal_input, vertical_input);
    }

    void UpdateInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            lhorz_inp = Input.GetAxisRaw("Horizontal");
            lvert_inp = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(lhorz_inp) > 0.0f)
            {
                lvert_inp = 0.0f;
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "BowRoomTile")
        {
            vertMove = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "BowRoomTile")
        {
            vertMove = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject object_collided_with = other.gameObject;
        if (object_collided_with.tag == "OldManTrig" && !OldManOn)
        {
            GameObject OldMan = Instantiate(OldManRoom, new Vector3(0, 33, 0), new Quaternion(0, 0, 0, 0));
            OldManOn = true;
        }
        if (object_collided_with.tag == "ToBowRoom")
        {
            CameraMovement.instance.BowRoomMove(5);
        }
        if (object_collided_with.tag == "Checkpoint")
        {
            transform.position = object_collided_with.transform.position - new Vector3(1, 0, 0);
            Instantiate(Door, object_collided_with.transform.position, new Quaternion(0, 0, 0, 0));
            AudioSource.PlayClipAtPoint(OpenDoor, Camera.main.transform.position);
            Destroy(object_collided_with);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "pit") && (Vector2.Distance(other.transform.position, transform.position) <= 0.65f) && !Inventory.instance.god_mode && !GameControl.instance.gameOver)
        {
            GameControl.instance.LinkDead();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject object_collided_with = collision.gameObject;
        if(object_collided_with.tag == "BowRoom")
        {
            if (MoveBlock1 != null)
            {
                pushed = false;
                MoveBlock1.transform.position = Block1;
                MoveBlock2.transform.position = Block2;
            }
            CameraMovement.instance.BowRoomMove(6);
        }
        if (object_collided_with.tag == "cGate" && Inventory.instance.key_count > 0)
        {
            Inventory.instance.key_count--;
            AudioSource.PlayClipAtPoint(OpenDoor, Camera.main.transform.position);
            GameObject openGate = Instantiate(openDoor, object_collided_with.transform.position,
                object_collided_with.transform.rotation, object_collided_with.transform.parent);
            Destroy(object_collided_with);
        }
        if (object_collided_with.tag == "cGateL" && Inventory.instance.key_count > 0)
        {
            Inventory.instance.key_count--;
            AudioSource.PlayClipAtPoint(OpenDoor, Camera.main.transform.position);
            GameObject openGate = Instantiate(openDoorL, object_collided_with.transform.position,
                object_collided_with.transform.rotation, object_collided_with.transform.parent);
            Destroy(object_collided_with);
        }
        if (object_collided_with.tag == "cGateR" && Inventory.instance.key_count > 0)
        {
            Inventory.instance.key_count--;
            AudioSource.PlayClipAtPoint(OpenDoor, Camera.main.transform.position);
            GameObject openGate = Instantiate(openDoorR, object_collided_with.transform.position,
                object_collided_with.transform.rotation, object_collided_with.transform.parent);
            Destroy(object_collided_with);
        }
        if ((object_collided_with.tag == "PushWall") && !pushed && 
            Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z),
                new Vector3(lhorz_inp, lvert_inp, 0), 1f, layerMask)) {
            StartCoroutine(PushBlock(collision.gameObject.GetComponent<Transform>()));
        }
        if (CameraMove)
        {
            if ((object_collided_with.tag == "oGateL") && (Input.GetAxisRaw("Horizontal") == -1))
            {
                if (MoveBlock1 != null)
                {
                    pushed = false;
                    MoveBlock1.transform.position = Block1;
                    MoveBlock2.transform.position = Block2;
                }
                invincible = true;
                StartCoroutine(CameraMovement.instance.MoveCamera(0));
            }
            if ((object_collided_with.tag == "oGateR") && (Input.GetAxisRaw("Horizontal") == 1))
            {
                if (MoveBlock1 != null)
                {
                    pushed = false;
                    MoveBlock1.transform.position = Block1;
                    MoveBlock2.transform.position = Block2;
                }
                invincible = true;
                StartCoroutine(CameraMovement.instance.MoveCamera(1));
            }
            if ((object_collided_with.tag == "oGate") && (Input.GetAxisRaw("Vertical") == 1))
            {
                if (MoveBlock1 != null)
                {
                    pushed = false;
                    MoveBlock1.transform.position = Block1;
                    MoveBlock2.transform.position = Block2;
                }
                invincible = true;
                StartCoroutine(CameraMovement.instance.MoveCamera(2));
            }
            if ((object_collided_with.tag == "oGateD") && (Input.GetAxisRaw("Vertical") == -1))
            {
                if (MoveBlock1 != null)
                {
                    pushed = false;
                    MoveBlock1.transform.position = Block1;
                    MoveBlock2.transform.position = Block2;
                }
                invincible = true;
                StartCoroutine(CameraMovement.instance.MoveCamera(3));
            }
        }
    }

    IEnumerator PushBlock(Transform Block)
    {
        int count = 0;
        if (Block.gameObject.name == "OldManBlock")
        {
            GameObject[] WallMasters = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject i in WallMasters)
            {
                if (i.transform.parent.gameObject == Block.transform.parent.gameObject)
                {
                    ++count;
                }
            }
        }
        Vector3 dest_pos = new Vector3(Block.position.x + lhorz_inp, Block.position.y + lvert_inp, Block.position.z);
        if (!Physics.Raycast(Block.position, dest_pos - Block.position, 0.5f, layerMask) && (count == 0))
        {
            pushed = true;
            float init_time = Time.time;
            float progress = (Time.time - init_time) / 20f;
            while (progress < .1f)
            {
                progress = (Time.time - init_time) / 20f;
                Vector3 new_position = Vector3.Lerp(Block.position, dest_pos, progress);
                Block.position = new_position;
                yield return null;
            }
            Block.position = dest_pos;
            AudioSource.PlayClipAtPoint(MoveBlock, Camera.main.transform.position);
            if ((Block.gameObject.name == "OldManBlock") && !opened)
            {
                GameObject object_collided_with = GameObject.FindGameObjectWithTag("OldManGate");
                if (object_collided_with != null)
                {
                    GameObject openGate = Instantiate(openDoorL, object_collided_with.transform.position,
                    object_collided_with.transform.rotation, object_collided_with.transform.parent);
                    Destroy(object_collided_with);
                    opened = true;
                }
            }
        }
        else
        {
            yield return null;
        }
    }
    public void DamagePlayer(Vector2 direction, float knockBack, float Damage)
    {
        Inventory.instance.health -= Damage;
        if ((Inventory.instance.health <= 0) && !GameControl.instance.gameOver)
        {
            GameControl.instance.LinkDead();
        }
        StartCoroutine(DmgColor());
        StartCoroutine(ArrowKeyMovement.instance.DmgCoroutine(direction, knockBack));
    }

    IEnumerator DmgCoroutine(Vector2 direction, float knockBack)
    {
        invincible = true; 
        restrictMovement = true; kb = true;
        if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                direction = new Vector3(1, 0, 0);
            }
            else
            {
                direction = new Vector3(-1, 0, 0);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                direction = new Vector3(0, 1, 0);
            }
            else
            {
                direction = new Vector3(0, -1, 0);
            }
        }
        rb.velocity = direction * knockBack;
        yield return new WaitForSeconds(0.5f);
        restrictMovement = false; kb = false;
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
    IEnumerator DmgColor()
    {
        Link_Sprite.color = Color.red;
        yield return new WaitForSeconds(0.07f);
        Link_Sprite.color = Color.white;
        yield return new WaitForSeconds(0.07f);
        Link_Sprite.color = Color.red;
        yield return new WaitForSeconds(0.07f);
        Link_Sprite.color = Color.white;
        yield return new WaitForSeconds(0.07f);
        Link_Sprite.color = Color.red;
        yield return new WaitForSeconds(0.07f);
        Link_Sprite.color = Color.white;
    }

    public void movePlayer(int direction)
    {
        float displacement = 3;
        float disX = 0; float disY = 0;
        if (direction < 4)
        {
            if (direction == 0) disX = -displacement;
            else if (direction == 1) disX = displacement;
            else if (direction == 2) disY = displacement - 0.5f;
            else if (direction == 3) disY = -displacement;
            transform.position = transform.position + new Vector3(disX, disY, 0f);
        }
        else
        {
            if (direction == 5)
            {
                transform.position = new Vector3(3, 64, 0);
            }
            else if(direction == 6)
            {
                transform.position = new Vector3(23, 60, 0);
            }
        }
    }
}
