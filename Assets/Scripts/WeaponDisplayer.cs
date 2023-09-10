using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponDisplayer : MonoBehaviour
{
    public Inventory inventory;

    Image Image_component;
    public Sprite Bow;
    public Sprite Grapling;
    public Sprite Boomerang;
    public Sprite Bomb;
    void Start()
    {
        Image_component = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        int Weapon = inventory.GetWep();
        Image_component.sprite = Bomb;
        if (Weapon == 0)
        {
            Image_component.sprite = Bow;
        }
        else if (Weapon == 1)
        {
            Image_component.sprite = Boomerang;
        }
        else if (Weapon == 3)
        {
            Image_component.sprite = Grapling;
        }
    }
}
