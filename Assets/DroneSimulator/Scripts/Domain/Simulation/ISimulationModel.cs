namespace DroneSimulator.Domain.Simulation
{
    public interface ISimulationModel
    {
        DroneRealtimeData DroneRealtimeData { get; }

        void UpdateDroneRealtimeData(DroneRealtimeData data);
    }
}
