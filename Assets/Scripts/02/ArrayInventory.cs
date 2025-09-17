using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayInventory : MonoBehaviour
{
    public int inventorySize = 10;

    public Item[] items;
    
    void Start()
    {
        items = new Item[inventorySize];    
    }

    public void AddItem(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = new Item(name, 1);
                Debug.Log(name + " �߰��� (���� "+ i + ")");
                return;
            }
        }

        Debug.Log("�κ��丮�� ���� ã���ϴ�!");
    }

    public void RemoveItem(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemName == name)
            {
                Debug.Log($"{name} + ������ (���� {i})");
                items[i] = null;
                return;
            }
        }

        Debug.Log($"{name} �������� �����ϴ�.");
    }

    public void PrintInventory()
    {
        Debug.Log("=== �迭 �κ��丮 ���� ===");
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
                Debug.Log($"{i} �� ����: {items[i].itemName} x {items[i].quantity}");
            else
                Debug.Log($"{i} �� ����: �������");
        }
    }
}
