namespace DroneSimulator.Data.Config
{
    public interface IConfigProvider
    {
        DronePhysicsConfig GetPhysicsConfig();

        DronePidConfig GetPidConfig();

        DroneAcroConfig GetAcroConfig();
    }
}
