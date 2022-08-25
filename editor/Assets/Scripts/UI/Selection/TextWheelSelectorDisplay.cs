using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Selection
{
    public class TextWheelSelectorDisplay : WheelSelectorDisplayBase<string>
    {
        [Header("Fade")]
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float fadeOutDuration;

        [SerializeField] private AnimationCurve fadeInCurve;
        [SerializeField] private AnimationCurve fadeOutCurve;
        
        [Space]
        
        [SerializeField] private TextMeshProUGUI text;

        private bool _isShown;

        private void Start()
        {
            text.alpha = 0f;
            _isShown = false;
        }

        public override void Display(string value)
        {
            if (_isShown)
            {
                var hide = DOTween.To(() => text.alpha, alpha => text.alpha = fadeOutCurve.Evaluate(alpha), 0f, fadeOutDuration);
                hide.onComplete += Show;
            }
            else
            {
                Show();
            }

            void Show()
            {
                text.text = value.ToString();
                var show = DOTween.To(() => text.alpha, alpha => text.alpha = fadeInCurve.Evaluate(alpha), 1f, fadeInDuration);
                _isShown = true;
            }
        }

        public override void SetColor(Color color)
        {
            text.color = color;
        }
    }
}