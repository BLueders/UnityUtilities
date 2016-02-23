using UnityEngine;
using System.Collections;

/// <summary>
/// Singleton options: DontDestroyOnLoad, CreateInstanceAutomatically
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Class)]
public class SingletonOptions : System.Attribute
{
    public bool DontDestroyOnLoad = false;
    public bool CreateInstanceAutomatically = true;

    public override string ToString()
    {
        return string.Format("[DontDestroyOnLoad:{0}, CreateInstanceAutomatically:{1}]", DontDestroyOnLoad, CreateInstanceAutomatically);
    }
}
