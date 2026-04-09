namespace DroneSimulator.Presentation.UI
{
    public interface IHudPresenter : IPresenter<IHudView>
    {
        void Tick();
    }
}
