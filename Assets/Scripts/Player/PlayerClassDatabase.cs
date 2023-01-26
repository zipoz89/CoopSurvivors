using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Zipoz/Player Classes/Prefabs Database")]
public class PlayerClassDatabase : ScriptableObject
{
    [SerializeField] private ClassRecord[] classes;

    public GameObject GetClassPrefab(PlayerClassType classType)
    {
        foreach (var c in classes)
        {
            if (c.ClassType == classType)
            {
                return c.Prefab;
            }
        }

        Debug.LogError("Class " + classType + " not found in prefab database");
        return classes[0].Prefab;
    }
}

public enum PlayerClassType
{
    Marine,
    Medium,
}

[System.Serializable]
public struct ClassRecord
{
    public PlayerClassType ClassType;
    public GameObject Prefab;
}
