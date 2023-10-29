using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct AdvV1Component : IComponent
{
    public Button openObject;
    public Button closeObject;
    [FormerlySerializedAs("thisGO")] public GameObject thisGo;
    public int takeAwayScore;
}