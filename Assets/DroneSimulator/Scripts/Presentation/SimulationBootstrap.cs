using System;

using DroneSimulator.Data.Config;
using DroneSimulator.Domain.Simulation;
using DroneSimulator.Presentation.Simulation;
using DroneSimulator.Presentation.UI;
using DroneSimulator.Utilities.DI;

using UnityEngine;

namespace DroneSimulator.Presentation
{
    [DefaultExecutionOrder(-1)]
    public sealed class SimulationBootstrap : MonoBehaviour
    {
        [SerializeField]
        private UIManager _uiManager;

        [SerializeField]
        private bool _openMainMenuOnStart;

        [SerializeField]
        private bool _openHudOnStart;

        [SerializeField]
        private QuadcopterView _quadrocopterView;

        private DiContainer _container;

        private void Awake()
        {
            try
            {
                _container = BuildContainer();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void Start()
        {
            try
            {
                if (_container == null)
                {
                    return;
                }

                IConfigProvider configProvider = _container.GetService<IConfigProvider>();
                ISimulationModel simulationModel = _container.GetService<ISimulationModel>();

                if (_quadrocopterView != null)
                {
                    _quadrocopterView.Configure(configProvider, simulationModel);
                }

                if (_uiManager != null)
                {
                    _uiManager.Init(_container);
                    if (_openMainMenuOnStart)
                    {
                        _uiManager.OpenWindow(WindowType.MainMenu);
                    }
                    else if (_openHudOnStart)
                    {
                        _uiManager.OpenWindow(WindowType.HUD);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private DiContainer BuildContainer()
        {
            DiServiceCollection services = new DiServiceCollection();

            services.RegisterSingleton<IConfigProvider, ConfigProvider>();
            services.RegisterSingleton<ISimulationModel, SimulationModel>();
            services.RegisterTransient<IHudPresenter, HudPresenter>();

            return services.GenerateContainer();
        }
    }
}
