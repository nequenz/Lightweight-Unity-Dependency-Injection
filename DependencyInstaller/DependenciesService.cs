using System;
using System.Collections.Generic;
using UnityEngine;


public class DependenciesService : MonoBehaviour
{
    private DependencyInstallContainer _dependenciesContainer;
    private List<BakedDependenciesRoot> _instances = new();

    private void InitDependencies()
    {

    }


    [ContextMenu("Resolve All Dependencies")]
    private void ResolveAllDependencies()
    {
        RootComponent[] allRootComponents = FindObjectsOfType<RootComponent>();

        foreach(RootComponent root in allRootComponents)
        {
            root.ResolveDependencies();
            Debug.Log("Dependencies of component root [" + root + "] have been rebult.");
        }
    }

    public I ResolveComponent<I>(Type currentType, RootComponent installComponent) where I : IInjectableComponent
    {
        IDependencyInstaller installer;
        BakedDependenciesRoot bakedInstance = _instances.Find((instance) => instance.InstallComponent == installComponent);
        I needComponent = default;

        if (bakedInstance is not null)
        {
            needComponent = bakedInstance.GetInstance<I>();
        }
        else
        {
            bakedInstance = new(installComponent);
            _instances.Add(bakedInstance);
        }

        if (_dependenciesContainer is null)
        {
            _dependenciesContainer = new(typeof(DependencyInstaller));

            InitDependencies();
        }

        if (needComponent is null)
        {
            installer = _dependenciesContainer.FindInstaller(currentType);

            if (installer is null)
                return default;

            needComponent = installer.Resolve<I>();

            if (needComponent is not null)
                bakedInstance.BakeComponent(needComponent);
        }

        if (needComponent is not null)
            needComponent.SetRootComponent(installComponent);

        return needComponent;
    }

    public I ResolveComponent<T, I>(RootComponent installComponent) where I : IInjectableComponent
    {
        return ResolveComponent<I>(typeof(T), installComponent);
    }

}