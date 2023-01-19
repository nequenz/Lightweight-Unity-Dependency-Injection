using System.Collections.Generic;

public class BakedDependenciesRoot
{
    private RootComponent _rootComponent;
    private List<object> _bakedComponents = new();

    public RootComponent InstallComponent => _rootComponent;

    public BakedDependenciesRoot(RootComponent installComponent)
    {
        _rootComponent = installComponent;
    }

    public T GetInstance<T>()
    {
        object component = _bakedComponents.Find((instance) => instance is T);

        return component is not null ? (T)component : default;
    }

    public void BakeComponent(object newComponent)
    {
        object sameComponent = _bakedComponents.Find((component) => component.GetType() == newComponent.GetType());

        if (sameComponent is not null)
            _bakedComponents.Remove(sameComponent);

        _bakedComponents.Add(newComponent);
    }
}
