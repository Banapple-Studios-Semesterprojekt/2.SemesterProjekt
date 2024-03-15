using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Transform FindFirstParent(this Transform parent)
    {
        Transform t = parent;

        while (t.parent != null)
        {
            t = t.parent;
        }

        return t;
    }

    public static List<T> GetChildrenRecursive<T>(this Transform _this, bool returnParent = true) where T : Component
    {
        List<T> returns = new List<T>();
        if (returnParent == true)
        {
            T component = _this.GetComponent<T>();
            if(component != null) { returns.Add(component); }
        }

        foreach (Transform t in _this)
        {
            returns.AddRange(t.GetChildrenRecursive<T>(true));
        }

        return returns;
    }
}