using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombDisplayer : MonoBehaviour
{
    public Inventory inventory;

    Text text_component;
    void Start()
    {
        text_component = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory != null && text_component != null)
        {
            text_component.text = "X" + inventory.GetBomb().ToString();
        }
    }
}
