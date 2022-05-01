using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.MainMenu.PlayerShop
{
    public class ScrollUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private const float MinItemScale = 1f;
        private const float MaxItemScale = 2f;
        private const float Epsilon = 1f;
        
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _contentRectTransform;
        [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;
        [SerializeField] private float _snapSpeed;
        [SerializeField] private float _maxScrollVelocity;
        [SerializeField] private float _itemScaleOffset;
        [SerializeField] private float _itemScaleSpeed;

        private ShopItem[] _items;
        private Vector2[] _itemsPosition;
        private Vector2[] _itemsScale;
        private Vector2 _connectVector;

        private float _shopItemOffsetX;
        private float _itemWidth;
        private float _itemPositionOffset;

        private int _itemsLength;
        private int _selectedItemID;

        private bool _isScrolling;
        
        public event Action<int> ItemChanged;

        public void Initialize(ShopItem[] items)
        {
            _items = items;

            _itemWidth = _items[0].GetComponent<RectTransform>().sizeDelta.x;
            _itemsLength = _items.Length;
            _itemsPosition = new Vector2[_itemsLength];
            _itemsScale = new Vector2[_itemsLength];
            _shopItemOffsetX = _horizontalLayoutGroup.spacing;

            _items[0].transform.localScale = new Vector2(MaxItemScale, MaxItemScale);

            _itemPositionOffset = _itemWidth + _shopItemOffsetX;
            for (int i = 0; i < _itemsLength; i++)
            {
                items[i].transform.localPosition = new Vector2(_itemPositionOffset * i, 0);
                _itemsPosition[i] = -items[i].transform.localPosition;
            }
        }

        public void ScrollContent()
        {
            if (IsConnectedToItem())
                return;

            if (IsScrollAtEdges())
                TryChangeInertiaState(false);

            FindNearestItemID();
            ScaleNearestItems();

            if (IsScrollingStoping()) 
                StopScrollAtNearestItem();
        }

        private bool IsConnectedToItem() => 
            _contentRectTransform.anchoredPosition == _itemsPosition[_selectedItemID];

        private bool IsScrollAtEdges()
        {
            float contentRectPositionX = _contentRectTransform.anchoredPosition.x;
            return contentRectPositionX >= _itemsPosition[0].x && _isScrolling == false 
                   || contentRectPositionX <= _itemsPosition[_itemsLength - 1].x && _isScrolling == false;
        }

        private void TryChangeInertiaState(bool state)
        {
            if (_scrollRect.inertia == state)
                return;
            _scrollRect.inertia = state;
        }

        private void FindNearestItemID()
        {
            int previousItemID = _selectedItemID;

            _selectedItemID = Mathf.RoundToInt(_contentRectTransform.anchoredPosition.x / (_itemWidth + _shopItemOffsetX));
            if (_selectedItemID > 0)
                _selectedItemID = 0;
            else if (_selectedItemID <= -_itemsLength)
                _selectedItemID = _itemsLength - 1;
            else _selectedItemID = Mathf.Abs(_selectedItemID);
        
            if (previousItemID != _selectedItemID)
                ItemChanged?.Invoke(_selectedItemID);
        }



        private void ScaleNearestItems()
        {
            for (int itemID = 0; itemID < _itemsLength; itemID++)
            {
                if (Mathf.Abs(_contentRectTransform.anchoredPosition.x - _itemsPosition[itemID].x) < _itemPositionOffset * 2)
                {
                    float distance = Mathf.Abs(_contentRectTransform.anchoredPosition.x - _itemsPosition[itemID].x);
                    float scale = Mathf.Clamp(1 / (distance / _shopItemOffsetX) * _itemScaleOffset, MinItemScale, MaxItemScale);
                    _itemsScale[itemID].x = Mathf.SmoothStep(_items[itemID].transform.localScale.x, scale, _itemScaleSpeed * Time.fixedDeltaTime);
                    _itemsScale[itemID].y = Mathf.SmoothStep(_items[itemID].transform.localScale.y, scale, _itemScaleSpeed * Time.fixedDeltaTime);
                    _items[itemID].transform.localScale = _itemsScale[itemID];
                }
            }
        }

        private bool IsScrollingStoping()
        {
            float scrollVelocity = Mathf.Abs(_scrollRect.velocity.x);
            return _isScrolling == false && scrollVelocity < _maxScrollVelocity;
        }

        private void StopScrollAtNearestItem()
        {
            TryChangeInertiaState(false);
        
            _connectVector.x = Mathf.SmoothStep(_contentRectTransform.anchoredPosition.x, _itemsPosition[_selectedItemID].x, _snapSpeed * UnityEngine.Time.fixedDeltaTime);
            if (Mathf.Abs(_contentRectTransform.anchoredPosition.x - _connectVector.x) < Epsilon)
                _connectVector.x = _itemsPosition[_selectedItemID].x;
            _contentRectTransform.anchoredPosition = _connectVector;
        }

        public void OnBeginDrag(PointerEventData eventData) => 
            _isScrolling = true;

        public void OnEndDrag(PointerEventData eventData) => 
            _isScrolling = false;
    }
}
