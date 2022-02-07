using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Events;

public class ItemSelection : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public string name;
    }

    [Header("References")]

    [SerializeField]
    private TextMeshProUGUI textDisplay;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button previousButton;

    [Header("Settings")]

    [SerializeField]
    private List<Item> selectableItems = new List<Item>();

    public Item currentItem { get; private set; }

    public UnityAction<ItemSelection> OnCurrentItemChangedEvent;



    private void OnEnable() => Initialize();

    private void OnDisable()
    {
        if (nextButton != null)
            nextButton.onClick.RemoveListener(NextItem);

        if (previousButton != null)
            previousButton.onClick.RemoveListener(PreviousItem);
    }

    private void Start()
    {
        SetItem(selectableItems[0]);
    }

    private void Initialize()
    {
        if (selectableItems.Count < 1 || selectableItems[0] == null) return;      

        if (nextButton != null)
            nextButton.onClick.AddListener(NextItem);

        if (previousButton != null)
            previousButton.onClick.AddListener(PreviousItem);
    }

    public void NextItem()
    {
        if (currentItem == null) return;

        int currentItemIndex = selectableItems.IndexOf(currentItem);
        bool isCurrentItemLastItem = currentItemIndex == selectableItems.Count - 1;
        int indexOfNextItem = isCurrentItemLastItem ? 0 : currentItemIndex + 1;

        SetItem(selectableItems[indexOfNextItem]);

    }

    public void PreviousItem()
    {
        if (currentItem == null) return;

        int currentItemIndex = selectableItems.IndexOf(currentItem);
        bool isCurrentItemFirstItem = currentItemIndex == 0;
        int indexOfPreviousItem = isCurrentItemFirstItem ? selectableItems.Count - 1 : currentItemIndex - 1;

        SetItem(selectableItems[indexOfPreviousItem]);
    }

    public void SetItem(string itemName)
    {
        Item item = selectableItems.FirstOrDefault((x) => x.name == itemName);
        SetItem(item);
    }

    public void SetItem(Item item)
    {
        if (item == null) return;

        currentItem = item;
        UpdateUI();

        //Debug.Log("Current Item Changed Called: " + gameObject.name);

        OnCurrentItemChangedEvent?.Invoke(this);       
    }

    private void UpdateUI()
    {
        if (textDisplay != null)
            textDisplay.text = currentItem.name;
    }
}
