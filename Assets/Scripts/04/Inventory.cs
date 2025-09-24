using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    void Start()
    {
        items.Add(new Item("Sword"));
        items.Add(new Item("Shield"));
        items.Add(new Item("Posion"));

        Item found = FindItem("Posion");

        if (found != null)
            Debug.Log($"ã�� ������ : {found.itemName}");
        else
            Debug.Log("�������� ã�� �� �����ϴ�.");
    }

    public Item FindItem(string itemName)
    {
        foreach (var item in items)
        {
            if (item.itemName == itemName)
                return item;
        }

        return null;
    }
}
