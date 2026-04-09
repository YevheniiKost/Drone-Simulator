using DroneSimulator.Data.Input;
using DroneSimulator.Data.Sensors;

namespace DroneSimulator.Domain.Flight
{
    public interface IFlightController
    {
        DroneMotorOutput ComputeMotorOutput(DroneSensorData sensorData, DroneInputState inputState);
    }
}
