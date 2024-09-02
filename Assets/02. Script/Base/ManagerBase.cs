using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ManagerBase : MonoBehaviour
{
    protected static IEnumerable<FieldInfo> AppFieldInfo;

    static ManagerBase()
    {
        var flag = BindingFlags.Instance | BindingFlags.NonPublic;
        AppFieldInfo = typeof(App).GetFields(flag);
    }

    protected abstract void Awake();

    internal static void SetFieldValue(Type type, MonoBehaviour manager)
    {
        var fields = AppFieldInfo.Where(field => field.FieldType.IsAssignableFrom(type));
        if (fields == null || fields.Count() != 1)
        {
            Debug.LogError($"Unresolved manager found. Type: {type.Name}");
            return;
        }

        var targetField = fields.ElementAt(0);
        targetField.SetValue(App.instance, manager);
    }

    internal static void SetFieldValue(MonoBehaviour manager)
    {
        SetFieldValue(manager.GetType(), manager);
    }
}


/// <summary>
/// Base data manager class
/// By calling Awake function, manager will be registered to App(manager router).
/// Manager will not be unregistered on destroy. 
/// Instead it will be overriden on new manager (of same type) appears. 
/// </summary>
public class Data : ManagerBase
{
    protected override void Awake() => SetFieldValue(this);
}

/// <summary>
/// Base manager class.
/// If the manager class use network functionality, please use GameManager.
/// By calling Awake function, manager will be registered to App(manager router).
/// Manager will not be unregistered on destroy. 
/// Instead it will be overriden on new manager (of same type) appears.
/// </summary>
public class Manager : ManagerBase
{
    protected override void Awake() => SetFieldValue(this);
}

public class ViewManager : Fusion.Behaviour
{
    protected virtual void Awake() => ManagerBase.SetFieldValue(typeof(ViewManager), this);
}