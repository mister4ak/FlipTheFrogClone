using UnityEngine;

namespace CodeBase.MainMenu
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private string _title;
        [SerializeField] private int _cost;

        public string Title => _title;
        public int Cost => _cost;
    }
}