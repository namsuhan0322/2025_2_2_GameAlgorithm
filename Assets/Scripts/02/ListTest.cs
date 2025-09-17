using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTest : MonoBehaviour
{
    void Start()
    {
        List<string> inventory = new List<string>();
        inventory.Add("Posion");
        inventory.Add("Sword");

        Debug.Log(inventory[0]);
        Debug.Log(inventory[1]);
        Debug.Log(inventory[2]);
    }
}
