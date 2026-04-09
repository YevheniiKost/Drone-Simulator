using System;

using DroneSimulator.Domain.Simulation;

namespace DroneSimulator.Presentation.UI
{
    public sealed class HudPresenter : IHudPresenter
    {
        private readonly ISimulationModel _simulationModel;
        private IHudView _view;

        public HudPresenter(ISimulationModel simulationModel)
        {
            _simulationModel = simulationModel ?? throw new ArgumentNullException(nameof(simulationModel));
        }

        public void AttachView(IHudView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void DetachView()
        {
            _view = null;
        }

        public void Tick()
        {
            if (_view == null)
            {
                return;
            }

            DroneRealtimeData data = _simulationModel.DroneRealtimeData;

            _view.SetHeightText($"Height: {data.Height:F2} m");

            float kmPerH = data.Speed * 3.6f;
            _view.SetSpeedText($"Speed: {data.Speed:F2} m/s ({kmPerH:F2} km/h)");

            _view.SetInputState(data.Input);
        }
    }
}
