using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;   // 성능 측정용

public class InventoryBigTest : MonoBehaviour
{
    List<Item> items = new List<Item>();
    private System.Random rand = new System.Random();

    public string name;

    void Start()
    {
        for (int i = 0; i < 100000; i++)
        {
            string name = $"Item_{i:DS}";
            int qty = rand.Next(1, 100);
            items.Add(new Item(name, qty));
        }

        string target = name;
        Stopwatch sw = Stopwatch.StartNew();
        Item foundLinear = FindItemLinear(target);
        sw.Stop();
        UnityEngine.Debug.Log($"[선형 탐색] {target} 개수, {foundLinear?.quantity}, 시간 : {sw.ElapsedMilliseconds}ms");

        items.Sort((a, b) => a.itemName.CompareTo(b.itemName));

        sw.Restart();
        Item foundBinear = FindItemBinary(target);
        sw.Stop();
        UnityEngine.Debug.Log($"[이진 탐색] {target} 개수, {foundLinear?.quantity}, 시간 : {sw.ElapsedMilliseconds}ms");
    }

    public Item FindItemLinear(string targetName)
    {
        foreach (var item in items)
        {
            if (item.itemName == targetName)
                return item;
        }
        return null;
    }

    public Item FindItemBinary(string targetName)
    {
        int left = 0;
        int right = items.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int compare = items[mid].itemName.CompareTo(targetName);

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
