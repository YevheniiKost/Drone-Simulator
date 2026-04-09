namespace DroneSimulator.Data.Input
{
    public interface IDroneInputProvider
    {
        DroneInputState ReadInput();
    }
}
