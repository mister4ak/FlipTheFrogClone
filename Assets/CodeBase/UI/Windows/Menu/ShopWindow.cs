using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.MainMenu;
using CodeBase.UI.Menu.Shop;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Menu
{
    public class ShopWindow : BaseWindow, ISavedProgressReader
    {
        private const string BuyedSkinText = "Choose";
        private const float NotEnoughCoinsTextOffsetY = 37f;

        [SerializeField] private GameObject _chosenSkinImage;
        [SerializeField] private TMP_Text _skinName;
        [SerializeField] private TMP_Text _skinCost;
        [SerializeField] private Button _buySkinButton;
        [SerializeField] private TMP_Text _notEnoughCoinsText;
        
        private Vector3 _notEnoughCoinsTextPosition;
        private ShopItem[] _items;
        private SkinData _skinData;
        private int _selectedItemID;

        public event Action BuySkinClicked;
        public event Action ChooseSkinClicked;

        public void Initialize(ShopItem[] shopItemNews, int selectedItemId)
        {
            _items = shopItemNews;
            _selectedItemID = selectedItemId;
            _notEnoughCoinsTextPosition = _notEnoughCoinsText.transform.position;
            
            ChangeCurrentItem(_selectedItemID);
            ChangeButtonState();
        }

        protected override void SubscribeUpdates()
        {
            base.SubscribeUpdates();
            _buySkinButton.onClick.AddListener(OnBuyButtonClicked);
        }

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
            _skinName.text = _items[itemID].Title;
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

        private void ShowSkinIsBuyed()
        {
            _chosenSkinImage.gameObject.SetActive(false);
            _buySkinButton.gameObject.SetActive(true);
            _skinCost.text = BuyedSkinText;
        }

        private void ShowSkinPrice()
        {
            _chosenSkinImage.gameObject.SetActive(false);
            _buySkinButton.gameObject.SetActive(true);
            _skinCost.text = _items[_selectedItemID].Cost.ToString();
        }

        public void ShowNotEnoughCoinsMessage()
        {
            _notEnoughCoinsText.transform.position = _notEnoughCoinsTextPosition;
            _notEnoughCoinsText.transform.DOMoveY(_notEnoughCoinsText.transform.position.y + NotEnoughCoinsTextOffsetY, 0.5f);
            _notEnoughCoinsText.DOFade(1f, 0.7f);
            _notEnoughCoinsText.DOFade(0f, 0.5f).SetDelay(3f);
        }

        public void LoadProgress(PlayerProgress progress) => 
            _skinData = progress.SkinData;
    }
}



