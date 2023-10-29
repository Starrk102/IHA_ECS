using System.Collections.Generic;
using Scellecs.Morpeh;
using TMPro;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct GameMenuComponent : IComponent
{
    public TMP_Text textTime;
    public TMP_Text scoreText;
    
    public GameObject field;
    public List<GameObject> gameObjects;
}