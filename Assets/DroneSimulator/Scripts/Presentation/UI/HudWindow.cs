using DroneSimulator.Data.Input;

using TMPro;

using UnityEngine;

namespace DroneSimulator.Presentation.UI
{
    public sealed class HudWindow : Window<IHudPresenter>, IHudView
    {
        [Header("Information")]
        [SerializeField]
        private TextMeshProUGUI _heightText;
        [SerializeField]
        private TextMeshProUGUI _speedText;

        [SerializeField]
        private InputValuesPanel _inputValuesPanel;

        [SerializeField]
        private TextMeshProUGUI _pidDebugText;

        public override WindowType WindowType => WindowType.HUD;

        public void SetHeightText(string text)
        {
            _heightText.text = text;
        }

        public void SetSpeedText(string text)
        {
            _speedText.text = text;
        }

        public void SetInputState(DroneInputState state)
        {
            _inputValuesPanel.UpdateInputValues(state);
        }

        public void SetPidDebugText(string text)
        {
            if (_pidDebugText == null)
            {
                return;
            }

            _pidDebugText.text = text;
        }

        protected override void OnShow()
        {
            Presenter.AttachView(this);
        }

        protected override void OnHide()
        {
            Presenter.DetachView();
        }

        private void OnDestroy()
        {
            if (Presenter != null)
            {
                Presenter.DetachView();
            }
        }

        private void Update()
        {
            Presenter.Tick();
        }
    }
}
