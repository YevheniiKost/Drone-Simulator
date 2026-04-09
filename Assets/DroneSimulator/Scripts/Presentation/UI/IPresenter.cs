namespace DroneSimulator.Presentation.UI
{
    public interface IPresenter<TView>
    {
        void AttachView(TView view);
        void DetachView();
    }
}
