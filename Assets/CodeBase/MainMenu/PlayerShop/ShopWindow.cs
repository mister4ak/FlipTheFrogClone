using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=tObYBJIf1EM

namespace CodeBase.MainMenu.PlayerShop
{
    public class ShopWindow : MonoBehaviour, ISavedProgressReader
    {
        private const string _buyedSkinText = "Choose";

        [SerializeField] private Button _shopCloseButton;
        [SerializeField] private GameObject _chosenSkinImage;
        [SerializeField] private TMP_Text _skinName;
        [SerializeField] private TMP_Text _skinCost;
        [SerializeField] private Button _buySkinButton;
        [SerializeField] private TMP_Text _notEnoughCoinsText;
        private Vector3 _notEnoughCoinsTextPosition;
        private float _notEnoughCoinsTextOffsetY = 37f;
        
        private ShopItem[] _items;
        private SkinData _skinData;
        private int _selectedItemID;

        public event Action Closed;
        public event Action BuySkinClicked;
        public event Action ChooseSkinClicked;
        

        public void Initialize(ShopItem[] shopItemNews, int selectedItemId)
        {
            _items = shopItemNews;
            _selectedItemID = selectedItemId;
            _notEnoughCoinsTextPosition = _notEnoughCoinsText.transform.position;
            
            ChangeCurrentItem(_selectedItemID);
            ChangeButtonState();
            
            _buySkinButton.onClick.AddListener(OnBuyButtonClicked);
        }


        public void Open()
        {
            gameObject.SetActive(true);
            _shopCloseButton.onClick.AddListener(() => Closed?.Invoke());
        }

        public void Close() => 
            gameObject.SetActive(false);

        private void OnBuyButtonClicked()
        {
            if (IsItemPurchased())
                ChooseSkinClicked?.Invoke();
            else
                BuySkinClicked?.Invoke();
        }
        
        public void ChangeCurrentItem(int itemID)
        {
            _selectedItemID = itemID;
            _skinName.text = _items[itemID].title;
            ChangeButtonState();
        }

        public void ChangeButtonState()
        {
            if (IsItemPurchased())
                if (IsItemChosen())
                    ShowSkinIsChosen();
                else
                    ShowSkinIsBuyed();
            else
                ShowSkinPrice();
        }

        private bool IsItemPurchased() => 
            _skinData.purchasedSkins.Contains(_selectedItemID);

        private bool IsItemChosen() => 
            _skinData.currentSkinID == _selectedItemID;

        private void ShowSkinIsChosen()
        {
            _buySkinButton.gameObject.SetActive(false);
            _chosenSkinImage.gameObject.SetActive(true);
        }

        private void ShowSkinIsBuyed() => 
            _skinCost.text = _buyedSkinText;

        private void ShowSkinPrice()
        {
            _chosenSkinImage.gameObject.SetActive(false);
            _buySkinButton.gameObject.SetActive(true);
            _skinCost.text = _items[_selectedItemID].cost.ToString();
        }

        public void ShowNotEnoughCoinsMessage()
        {
            _notEnoughCoinsText.transform.position = _notEnoughCoinsTextPosition;
            _notEnoughCoinsText.transform.DOMoveY(_notEnoughCoinsText.transform.position.y + _notEnoughCoinsTextOffsetY, 0.5f);
            _notEnoughCoinsText.DOFade(1f, 0.7f);
            _notEnoughCoinsText.DOFade(0f, 0.5f).SetDelay(3f);
        }

        public void LoadProgress(PlayerProgress progress) => 
            _skinData = progress.SkinData;
    }
}



