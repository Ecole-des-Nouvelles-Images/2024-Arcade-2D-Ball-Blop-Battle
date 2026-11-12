using DG.Tweening;
using Player.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelectionPlayer
{
    /// <summary>
    /// Affichage d'un côté de l'écran de sélection : grand aperçu du Blop, nom, description,
    /// étiquette joueur (1P / 2P / CPU), curseur sur la grille et invitation à rejoindre.
    /// </summary>
    public class SelectionSlotView : MonoBehaviour
    {
        [Header("===== PREVIEW =====")]
        [SerializeField] private Image _preview;
        [SerializeField] private TextMeshProUGUI _blopName;
        [SerializeField] private TextMeshProUGUI _specialSpikeDescription;
        [SerializeField] private TextMeshProUGUI _playerTag;
        [SerializeField] private GameObject _readyBadge;
        [SerializeField] private GameObject _joinPrompt;

        [Header("===== CURSOR =====")]
        [SerializeField] private RectTransform _cursor;
        [SerializeField] private CanvasGroup _cursorCanvasGroup;
        [SerializeField] private TextMeshProUGUI _cursorLabel;

        [Header("===== SETTINGS =====")]
        [SerializeField] private string _playerTagFormat = "J{0}";
        [SerializeField] private string _aiTag = "CPU";
        [SerializeField, Range(0f, 1f)] private float _idleCursorAlpha = 0.45f;
        [SerializeField, Range(0f, 1f)] private float _waitingPreviewAlpha = 0.25f;

        private RectTransform _cursorTarget;

        public void Refresh(SelectionSlot slot, BlopPortrait hoveredPortrait, bool isControlled)
        {
            Blop blop = slot.IsConfirmed ? slot.ConfirmedBlop : hoveredPortrait.Blop;
            string playerTag = slot.IsAi ? _aiTag : string.Format(_playerTagFormat, slot.PlayerId);

            _preview.sprite = blop.Sprite;
            _preview.color = new Color(1f, 1f, 1f, slot.IsWaitingForPlayer ? _waitingPreviewAlpha : 1f);
            _blopName.text = blop.PlayerName;
            _specialSpikeDescription.text = blop.SpecialSpikeDescription;
            _playerTag.text = playerTag;
            _cursorLabel.text = playerTag;

            _readyBadge.SetActive(slot.IsConfirmed);
            _joinPrompt.SetActive(!slot.HasDevice);

            _cursor.gameObject.SetActive(!slot.IsWaitingForPlayer);
            _cursorCanvasGroup.alpha = isControlled || slot.IsConfirmed ? 1f : _idleCursorAlpha;
            _cursorTarget = hoveredPortrait.RectTransform;
            FollowCursorTarget();
        }

        public void PlayConfirmFeedback()
        {
            _preview.transform.DOKill(true);
            _preview.transform.DOPunchScale(Vector3.one * 0.12f, 0.3f, 6);
            _cursor.DOKill(true);
            _cursor.DOPunchScale(Vector3.one * 0.15f, 0.25f, 6);
        }

        private void LateUpdate()
        {
            // La grille peut se réorganiser (layout), le curseur suit donc sa case à chaque frame
            FollowCursorTarget();
        }

        private void FollowCursorTarget()
        {
            if (!_cursorTarget) return;

            _cursor.position = _cursorTarget.TransformPoint(_cursorTarget.rect.center);
            _cursor.sizeDelta = _cursorTarget.rect.size;
        }

        private void OnDestroy()
        {
            _preview.transform.DOKill();
            _cursor.DOKill();
        }
    }
}
