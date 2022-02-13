using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace CybernautX
{
    public class ItemSelection : MonoBehaviour
    {
        public enum Type { Amount , PlayerItems }

        public Type type;

        [ShowIf("type", Type.PlayerItems)]
        //public Player player;
        //
        [ShowIf("type", Type.PlayerItems)]
        //public int index;

        [ShowIf("type", Type.PlayerItems)]
        [SerializeField]
        private List<Item> selectableItems = new List<Item>();

        [ShowIf("type", Type.Amount)]
        [SerializeField]
        private int maxAmount;
        
        [ShowIf("type", Type.Amount)]
        [SerializeField]
        private int minAmount;

        [SerializeField]
        private TextMeshProUGUI textDisplay;

        [SerializeField]
        private Button nextButton;

        [SerializeField]
        private Button previousButton;

        //private int minItemAmount;
        private int maxItemAmount;
        private int currentIndex;

        public int currentAmount { get => currentIndex + minAmount; }
        public Item currentItem { get; private set; }

        public UnityAction<ItemSelection> OnCurrentItemChangedEvent;

        [ShowIf("type", Type.PlayerItems)]
        public CustomUnityEvents.ItemEvent OnCurrentItemChangedUnityEvent = new CustomUnityEvents.ItemEvent();

        [ShowIf("type", Type.Amount)]
        public CustomUnityEvents.IntEvent OnCurrentAmountChangedEvent = new CustomUnityEvents.IntEvent();        


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
            UpdateItem(0);
        }

        private void Initialize()
        {
            if (nextButton != null)
                nextButton.onClick.AddListener(NextItem);

            if (previousButton != null)
                previousButton.onClick.AddListener(PreviousItem);

            switch (type)
            {
                case Type.Amount:
                    maxItemAmount = maxAmount;
                    break;
                case Type.PlayerItems:
                    maxItemAmount = selectableItems.Count;
                    break;
                default:
                    break;
            }
        }

        public void NextItem()
        {
            bool isCurrentItemLastItem = currentIndex == maxItemAmount - 1;
            int indexOfNextItem = isCurrentItemLastItem ? 0 : currentIndex + 1;

            UpdateItem(indexOfNextItem);
        }

        public void PreviousItem()
        {
            bool isCurrentItemFirstItem = currentIndex == 0;
            int indexOfPreviousItem = isCurrentItemFirstItem ? maxItemAmount - 1 : currentIndex - 1;

            UpdateItem(indexOfPreviousItem);
        }

        private void UpdateUI()
        {
            string text = "";

            switch (type)
            {
                case Type.Amount:
                    text = currentAmount.ToString();
                    break;
                case Type.PlayerItems:
                    text = currentItem.Name;
                    break;
                default:
                    break;
            }

            if (textDisplay != null)
                textDisplay.text = text;
        }

        private void UpdateItem(int index)
        {
            currentIndex = index;

            switch (type)
            {
                case Type.Amount:
                    OnCurrentAmountChangedEvent?.Invoke(currentAmount);
                    break;
                case Type.PlayerItems:
                    currentItem = selectableItems[index];
                    OnCurrentItemChangedUnityEvent?.Invoke(currentItem);
                    break;
                default:
                    break;
            }

            UpdateUI();
        }
    }
}


