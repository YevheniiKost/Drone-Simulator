using System;
using System.Collections.Generic;

using DroneSimulator.Utilities.DI;
using DroneSimulator.Utilities.Log;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace DroneSimulator.Presentation.UI
{
    public class UIManager : MonoBehaviour
    {
        private const string LogTag = "UIManager";

        [SerializeField]
        private InputSystemUIInputModule _inputSystemUiModule;

        [SerializeField]
        private List<Window> _windows = new List<Window>();

        private DiContainer _diContainer;

        private void Awake()
        {
            if (_inputSystemUiModule != null && _inputSystemUiModule.cancel != null)
            {
                _inputSystemUiModule.cancel.action.performed += OnCancelPerformed;
            }
        }

        private void OnDestroy()
        {
            if (_inputSystemUiModule != null && _inputSystemUiModule.cancel != null)
            {
                _inputSystemUiModule.cancel.action.performed -= OnCancelPerformed;
            }
        }

        public void Init(DiContainer diContainer)
        {
            if (diContainer == null)
            {
                DroneSimulatorLogger.LogError(LogTag, "DiContainer is null");
                return;
            }

            _diContainer = diContainer;
        }

        public void OpenWindow(WindowType windowType)
        {
            try
            {
                Window window = _windows.Find(x => x.WindowType == windowType) ??
                                throw new Exception($"Window of type {windowType} not found");

                if (window.IsActive)
                {
                    DroneSimulatorLogger.LogWarning(LogTag, $"Window {windowType} already opened");
                    return;
                }

                window.Init(this, _diContainer);
                window.Show();
            }
            catch (Exception ex)
            {
                DroneSimulatorLogger.LogError(LogTag, $"Failed to open window of type {windowType}: {ex.Message}");
            }
        }

        public void HideWindow(WindowType windowType)
        {
            try
            {
                Window window = _windows.Find(x => x.WindowType == windowType) ??
                                throw new Exception($"Window of type {windowType} not found");

                if (!window.IsActive)
                {
                    DroneSimulatorLogger.LogWarning(LogTag, $"Window {windowType} already closed");
                    return;
                }

                window.Hide();
            }
            catch (Exception ex)
            {
                DroneSimulatorLogger.LogError(LogTag, $"Failed to close window of type {windowType}: {ex.Message}");
            }
        }

        private void OnCancelPerformed(InputAction.CallbackContext context)
        {
            Window activeWindow = _windows.Find(x => x.IsActive);
            activeWindow?.OnCancel();
        }
    }
}
