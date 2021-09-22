using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private Text itemsTextbox;

    private void Start()
    {
        ItemManager.GetItems(UpdateDisplay);
    }

    private void UpdateDisplay(Item[] items)
    {
        string output = "";
        foreach (Item item in items)
        {
            output += $"{item.name} (id = {item.id})\n";
        }
        itemsTextbox.text = output;
    }
}
