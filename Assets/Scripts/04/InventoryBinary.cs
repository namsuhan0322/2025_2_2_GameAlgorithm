using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBinary : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public string name;

    void Start()
    {
        items.Add(new Item("Posion", 5));
        items.Add(new Item("High-Posion", 2));
        items.Add(new Item("Elixir", 1));
        items.Add(new Item("Sword", 1));

        items.Sort((a, b) => a.itemName.CompareTo(b.itemName));

        Item found = FindItem(name);

        if (found != null)
            Debug.Log($"[���� Ž��] ã�� ������ : {found.itemName}, ���� : {found.quantity} ��");
        else
            Debug.Log("�������� ã�� �� �����ϴ�.");
    }

    public Item FindItem(string itemName)
    {
        int left = 0;
        int right = items.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int compare = items[mid].itemName.CompareTo(itemName);

            if (compare == 0)
                return items[mid];
            else if (compare < 0)
                left = mid + 1;
            else
                right = mid - 1;

        }

        return null;
    }
}
