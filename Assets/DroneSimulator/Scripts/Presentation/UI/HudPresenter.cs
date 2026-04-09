using System;
using System.Text;

using DroneSimulator.Domain.Flight;
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

            _view.SetPidDebugText(FormatPidDebug(data.PidDebug));
        }

        private static string FormatPidDebug(FlightPidDebugTelemetry t)
        {
            if (t.Mode == FlightPidDebugMode.None)
            {
                return string.Empty;
            }

            var sb = new StringBuilder(640);
            sb.AppendLine(t.Mode == FlightPidDebugMode.AcroRate ? "PID — Acro (rate, deg/s)" : "PID — Stabilised (angle deg, rate deg/s)");

            sb.Append("Ang P:").Append(t.PitchAngleDeg.ToString("F1")).Append(" R:").Append(t.RollAngleDeg.ToString("F1"));
            sb.Append(" | w P:").Append(t.PitchRateDegS.ToString("F1")).Append(" R:").Append(t.RollRateDegS.ToString("F1")).Append(" Y:");
            sb.Append(t.YawRateDegS.ToString("F1")).AppendLine(" deg/s");

            sb.Append("Err P:").Append(t.PitchError.ToString("F2")).Append(" R:").Append(t.RollError.ToString("F2")).Append(" Y:");
            sb.Append(t.YawError.ToString("F2")).AppendLine();

            sb.Append("Pitch P/I/D: ").Append(t.PitchPid.P.ToString("F3")).Append(" / ").Append(t.PitchPid.I.ToString("F3"));
            sb.Append(" / ").Append(t.PitchPid.D.ToString("F3")).Append(" | out ").AppendLine(t.PitchMixerOut.ToString("F3"));

            sb.Append("Roll  P/I/D: ").Append(t.RollPid.P.ToString("F3")).Append(" / ").Append(t.RollPid.I.ToString("F3"));
            sb.Append(" / ").Append(t.RollPid.D.ToString("F3")).Append(" | out ").AppendLine(t.RollMixerOut.ToString("F3"));

            sb.Append("Yaw   P/I/D: ").Append(t.YawPid.P.ToString("F3")).Append(" / ").Append(t.YawPid.I.ToString("F3"));
            sb.Append(" / ").Append(t.YawPid.D.ToString("F3")).Append(" | tau ").Append(t.YawTorqueOut.ToString("F3"));
            sb.Append(" | FF ").Append(t.YawFeedForwardTorque.ToString("F3"));

            return sb.ToString();
        }
    }
}
