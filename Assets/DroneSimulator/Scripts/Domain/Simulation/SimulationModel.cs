namespace DroneSimulator.Domain.Simulation
{
    public sealed class SimulationModel : ISimulationModel
    {
        public DroneRealtimeData DroneRealtimeData { get; private set; }

        public void UpdateDroneRealtimeData(DroneRealtimeData data)
        {
            DroneRealtimeData = data;
        }
    }
}
