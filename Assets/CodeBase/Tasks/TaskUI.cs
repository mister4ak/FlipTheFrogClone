using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Tasks
{
    public class TaskUI: MonoBehaviour
    {
        [SerializeField] private CanvasGroup _taskButtonCanvas;
        [SerializeField] private Button _taskButton;
        [SerializeField] private Image _taskButtonImage;
        [SerializeField] private Image _progressBar;
        [SerializeField] private Sprite _taskCompleteSprite;
        [SerializeField] private TMP_Text _taskNameText;
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private TMP_Text _taskDescription;
        
        private Vector3 _rewardTextPosition;
        private Vector3 _taskDescriptionPosition;
        private Task _currentTask;
        private bool _isTaskCompleted;
        private int _taskProgress;
        private const float _rewardTextOffsetY = 45f;
        private const float _taskDescriptionOffsetY = 46f;

        public event Action RewardReceived;

        public void Initialize(Task currentTask, bool isTaskCompleted, int taskProgress)
        {
            _currentTask = currentTask;
            _isTaskCompleted = isTaskCompleted;
            _taskProgress = taskProgress;

            if (_isTaskCompleted)
            {
                _taskNameText.text = string.Empty;
                _taskButtonImage.sprite = _taskCompleteSprite;
                _progressBar.fillAmount = 1f;
            }
            else
            {
                _taskNameText.text = _currentTask.taskId.ToString();
                _progressBar.fillAmount = (float)_taskProgress / _currentTask.conditionNumber;
                _taskButtonImage.sprite = _currentTask.sprite;
            }

            _taskDescriptionPosition = _taskDescription.transform.position;
            _rewardTextPosition = _rewardText.transform.position;
            _taskButton.onClick.AddListener(OnTaskButtonClicked);
        }

        private void OnTaskButtonClicked()
        {
            if (_isTaskCompleted)
            {
                RewardAnimation();
                HideTaskButton();
                
                RewardReceived?.Invoke();
                _taskButton.onClick.RemoveListener(OnTaskButtonClicked);
            }
            else
                ShowTaskDescription();
        }

        private void RewardAnimation()
        {
            _rewardText.transform.position = _rewardTextPosition;
            _rewardText.text = $"+{_currentTask.reward}";
            _rewardText.transform.DOMoveY( _rewardText.transform.position.y + _rewardTextOffsetY, 0.5f);
            _rewardText.DOFade(1f, 0.7f);
            _rewardText.DOFade(0f, 0.5f).SetDelay(1f);
        }

        private void HideTaskButton()
        {
            _taskButtonCanvas.DOFade(0f, 0.5f).SetDelay(1f);
            _taskButtonCanvas.interactable = false;
        }

        private void ShowTaskDescription()
        {
            _taskDescription.transform.position = _taskDescriptionPosition;
            _taskDescription.text = _currentTask.description;
            _taskDescription.transform.DOMoveY(_taskDescription.transform.position.y + _taskDescriptionOffsetY, 0.5f);
            _taskDescription.DOFade(1f, 0.7f);
            _taskDescription.DOFade(0f, 0.5f).SetDelay(3f);
        }
    }
}
