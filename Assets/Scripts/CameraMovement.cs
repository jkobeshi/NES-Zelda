using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
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

    public void BowRoomMove(int direction)
    {
            StartCoroutine(BowMove(direction));
    }
    IEnumerator BowMove(int direction)
    {
        ArrowKeyMovement.instance.restrictMovement = true;
        yield return new WaitForSeconds(1.5f);
        if (direction == 5)
        {
            transform.position = new Vector3(23.5f-16f, 62f, -1);
        }
        if (direction == 6)
        {
            transform.position = new Vector3(23.5f, 62f, -1);
        }
        yield return new WaitForSeconds(1.5f);
        ArrowKeyMovement.instance.movePlayer(direction);
        ArrowKeyMovement.instance.restrictMovement = false;
    }

    public IEnumerator MoveCamera(int direction)
    {
        ArrowKeyMovement.instance.restrictMovement = true;
        Vector3 dest_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (direction == 0)
        {
            dest_pos.x = transform.position.x -16;
        }
        else if (direction == 1)
        {
            dest_pos.x = transform.position.x + 16;
        }
        else if (direction == 2)
        {
            dest_pos.y = transform.position.y + 11;
        }
        else if (direction == 3)
        {
            dest_pos.y = transform.position.y - 11;
        }
        float init_time = Time.time;
        float progress = (Time.time - init_time) / 20f;
        while(progress < .1f)
        {
            progress = (Time.time - init_time) / 20f;
            Vector3 new_position = Vector3.Lerp(transform.position, dest_pos, progress);
            transform.position = new_position;
            yield return null;
        }
        transform.position = dest_pos;
        ArrowKeyMovement.instance.movePlayer(direction);
        ArrowKeyMovement.instance.restrictMovement = false;
        ArrowKeyMovement.instance.invincible = false;
    }
}