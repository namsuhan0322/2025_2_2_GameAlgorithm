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
                Debug.Log(name + " 추가됨 (슬롯 "+ i + ")");
                return;
            }
        }

        Debug.Log("인벤토리가 가득 찾습니다!");
    }

    public void RemoveItem(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemName == name)
            {
                Debug.Log($"{name} + 삭제됨 (슬롯 {i})");
                items[i] = null;
                return;
            }
        }

        Debug.Log($"{name} 아이템이 없습니다.");
    }

    public void PrintInventory()
    {
        Debug.Log("=== 배열 인벤토리 상태 ===");
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
                Debug.Log($"{i} 번 슬롯: {items[i].itemName} x {items[i].quantity}");
            else
                Debug.Log($"{i} 번 슬롯: 비어있음");
        }
    }
}
