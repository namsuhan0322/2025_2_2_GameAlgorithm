using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SearchItem : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    [Header("UI")]
    public TMP_InputField searchItem;

    public RectTransform pos; 
    public GameObject itemPrefab;

    public void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Instantiate(itemPrefab, pos);
        }
    }

    public void LinearSearch()
    {

    }

    public void BinarySearch()
    {

    }
}
