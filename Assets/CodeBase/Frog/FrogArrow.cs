using CodeBase.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Frog
{
    public class FrogArrow : MonoBehaviour
    {
        [SerializeField] private LineRenderer _arrowLine;
        [SerializeField] private SpriteRenderer _arrowhead;

        private float _arrowLength = 2f;
        private float _maxDragDistance;
        private Vector2 _arrowStartPosition;
        private StaticDataService _staticData;

        [Inject]
        private void Construct(StaticDataService staticData) => 
            _staticData = staticData;

        private void Start()
        {
            _maxDragDistance = _staticData.PlayerData().maxDragDistance;
            _arrowLine.positionCount = 2;
            SetZeroPosition();
            SetActiveState(false);
        }

        public void SetStartPosition(Vector2 position) => 
            _arrowStartPosition = position;

        private void SetZeroPosition() => 
            _arrowLine.SetPosition(0, Vector2.zero);

        public void SetHoldArrowPosition(Vector2 holdPosition)
        {
            SetActiveState(true);

            float dragDistance = Vector2.Distance(_arrowStartPosition, holdPosition);
            if (dragDistance > _maxDragDistance)
                SetArrowlineMaxlength(holdPosition);
            else
                SetArrowlineCroppedLength(holdPosition, dragDistance);

            RotateArrowhead(holdPosition);
        }

        private void SetArrowlineMaxlength(Vector2 holdPosition)
        {
            _arrowLine.SetPosition(1, (_arrowStartPosition - holdPosition).normalized * _arrowLength);
            _arrowhead.transform.localPosition = (_arrowStartPosition - holdPosition).normalized * _arrowLength;
        }

        private void SetArrowlineCroppedLength(Vector2 holdPosition, float dragDistance)
        {
            _arrowLine.SetPosition(1, 
                (_arrowStartPosition - holdPosition).normalized * _arrowLength / _maxDragDistance * dragDistance);
            _arrowhead.transform.localPosition = 
                (_arrowStartPosition - holdPosition).normalized * _arrowLength / _maxDragDistance * dragDistance;

            ChangeArrowheadAlpha(dragDistance);
        }

        private void ChangeArrowheadAlpha(float dragDistance)
        {
            Color color = _arrowhead.color;
            color = new Color(color.r, color.g, color.b, dragDistance / _maxDragDistance);
            _arrowhead.color = color;
        }

        private void RotateArrowhead(Vector2 holdPosition) =>
            _arrowhead.transform.eulerAngles = Vector3.forward *
                                               (Mathf.Atan2(_arrowStartPosition.y - holdPosition.y,
                                                   _arrowStartPosition.x - holdPosition.x) * Mathf.Rad2Deg);

        public void SetActiveState(bool state)
        {
            if (_arrowLine.gameObject.activeSelf != state)
                _arrowLine.gameObject.SetActive(state);
        }
    }
}
