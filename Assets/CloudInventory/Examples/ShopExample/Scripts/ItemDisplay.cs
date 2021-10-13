using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CloudInventory.Examples.ShopExample
{
    public class ItemDisplay : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text goldText;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color inactiveColor;
        [SerializeField] private Color selectedColor;

        public Item Item { get; private set; }

        public UnityEvent OnSelected { get; private set; }

        private void Awake()
        {
            OnSelected = new UnityEvent();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnSelected.Invoke();
        }

        public void UpdateDisplay(Item item)
        {
            Item = item;
            nameText.text = item.Name;
            goldText.text = item.Price.ToString();
        }

        public void SetSelected(bool selected)
        {
            backgroundImage.color = selected ? selectedColor : inactiveColor;
        }
    }
}
