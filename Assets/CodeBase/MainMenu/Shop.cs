using System;
using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI.Menu.Shop;
using CodeBase.UI.Windows.Menu;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.MainMenu
{
    public class Shop: ITickable, ISavedProgress
    {
        private readonly ScrollUI _scrollUI;
        private readonly ShopWindow _shopWindow;
        private readonly RewardedAdItem _adItem;

        private ShopItem[] _shopItems;
        private PlayerData _playerData;
        private SkinData _skinData;
        private int _selectedItemId;

        public event Action ShopWindowClosed;
        
        public Shop(
            ScrollUI scrollUI,
            ShopWindow shopWindow,
            RewardedAdItem adItem
        )
        {
            _shopWindow = shopWindow;
            _scrollUI = scrollUI;
            _adItem = adItem;
        }
        
        public void Initialize()
        {
            _shopItems = _scrollUI.GetComponentsInChildren<ShopItem>();
            _selectedItemId = 0;

            _shopWindow.Initialize(_shopItems, _selectedItemId);
            _scrollUI.Initialize(_shopItems);
            _adItem.Initialize();

            _adItem.Subscribe();
            _scrollUI.ItemChanged += ChangeCurrentItemId;
            _shopWindow.ChooseSkinClicked += ChangeCurrentSkin;
            _shopWindow.BuySkinClicked += TryBuyItem;
            _shopWindow.CloseButtonClicked += CloseWindow;
        }

        public void Tick() => 
            _scrollUI.ScrollContent();

        private void ChangeCurrentItemId(int itemId)
        {
            _selectedItemId = itemId;
            _shopWindow.ChangeCurrentItem(_selectedItemId);
        }

        private void ChangeCurrentSkin()
        {
            _skinData.currentSkinID = _selectedItemId;
            _skinData.skinSpritePath = AssetPath.FrogSprites + _shopItems[_selectedItemId].GetComponent<Image>().sprite.name;

            _shopWindow.ChangeButtonState();
        }

        private void TryBuyItem()
        {
            if (_playerData.coins >= _shopItems[_selectedItemId].Cost)
            {
                _skinData.purchasedSkins.Add(_selectedItemId);
                _playerData.ReduceCoins(_shopItems[_selectedItemId].Cost);

                _shopWindow.ChangeButtonState();

            }
            else
                _shopWindow.ShowNotEnoughCoinsMessage();
        }

        public void OpenWindow() => 
            _shopWindow.Open();

        private void CloseWindow() => 
            ShopWindowClosed?.Invoke();

        public void LoadProgress(PlayerProgress progress)
        {
            _playerData = progress.PlayerData;
            _skinData = progress.SkinData;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.SkinData.purchasedSkins = _skinData.purchasedSkins;
            progress.SkinData.skinSpritePath = _skinData.skinSpritePath;
            progress.SkinData.currentSkinID = _skinData.currentSkinID;
        }

        public void Disable()
        {
            _adItem.Cleanup();
            _scrollUI.ItemChanged -= ChangeCurrentItemId;
            _shopWindow.CloseButtonClicked -= CloseWindow;
            _shopWindow.BuySkinClicked -= TryBuyItem;
            _shopWindow.ChooseSkinClicked -= ChangeCurrentSkin;
        }
    }
}