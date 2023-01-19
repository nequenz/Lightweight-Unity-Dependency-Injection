
[System.Serializable]
public abstract class InjectedComponent : IInjectableComponent
{
    private RootComponent _root;

    protected RootComponent Root => _root;

    public abstract bool IsEnabled { get; }


    public abstract void Awake();

    public abstract void Away();

    public void SetRootComponent(RootComponent installComponent) => _root = installComponent;
}