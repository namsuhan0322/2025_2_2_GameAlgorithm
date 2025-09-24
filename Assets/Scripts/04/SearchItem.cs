using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchItem : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    [Header("UI")]
    public TMP_InputField searchItem;
    public RectTransform pos;
    public GameObject itemPrefab;

    private bool isSorted = false;

    public void Start()
    {
        for (int i = 0; i < 99; i++)
        {
            string currentItemName = "Item_" + i.ToString("D2");

            Item newItemData = new Item(currentItemName);

            GameObject newItemObject = Instantiate(itemPrefab, pos);
            newItemObject.GetComponentInChildren<TextMeshProUGUI>().text = currentItemName;
            newItemData.itemObject = newItemObject;

            items.Add(newItemData);
        }
    }

    private void ShowAllItems()
    {
        foreach (Item item in items)
        {
            if (item.itemObject != null)
            {
                item.itemObject.SetActive(true);
            }
        }
    }

    public void LinearSearch()
    {
        string targetName = searchItem.text;

        if (string.IsNullOrEmpty(targetName))
        {
            ShowAllItems();
            return;
        }

        foreach (Item item in items)
        {
            bool isMatch = item.itemName == targetName;
            item.itemObject.SetActive(isMatch);
        }
    }

    public void BinarySearch()
    {
        string targetName = searchItem.text;

        if (string.IsNullOrEmpty(targetName))
        {
            ShowAllItems();
            return;
        }

        if (!isSorted)
        {
            items.Sort((a, b) => a.itemName.CompareTo(b.itemName));
            isSorted = true;
            Debug.Log("리스트를 이름순으로 정렬했습니다.");
        }

        foreach (Item item in items)
        {
            item.itemObject.SetActive(false);
        }

        int left = 0;
        int right = items.Count - 1;
        while (left <= right)
        {
            int mid = (left + right) / 2;
            int comparison = items[mid].itemName.CompareTo(targetName);

            if (comparison == 0)
            {
                items[mid].itemObject.SetActive(true);
                return; 
            }
            else if (comparison < 0) 
            {
                left = mid + 1; 
            }
            else 
            {
                right = mid - 1;
            }
        }
    }
}