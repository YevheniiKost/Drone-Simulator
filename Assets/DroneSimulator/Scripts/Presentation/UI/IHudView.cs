using DroneSimulator.Data.Input;

namespace DroneSimulator.Presentation.UI
{
    public interface IHudView
    {
        void SetHeightText(string text);

        void SetSpeedText(string text);

        void SetInputState(DroneInputState state);
    }
}
