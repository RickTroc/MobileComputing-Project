using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectables : MonoBehaviour
{
    public Text name;
    public Text description;
    public Image sprite;

    public Collectables(Text name, Text desctiption, Image sprite)
    {
        this.name = name;
        this.description = desctiption;
        this.sprite = sprite;
    }

    



}
