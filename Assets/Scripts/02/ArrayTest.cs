using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTest : MonoBehaviour
{
    void Start()
    {
        string[] inventory = new string[5];
        inventory[0] = "Posion";
        inventory[1] = "Sword";

        Debug.Log(inventory[0]);
        Debug.Log(inventory[1]);
        Debug.Log(inventory[2]);
    }
}
