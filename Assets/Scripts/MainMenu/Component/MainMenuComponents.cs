using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.UI;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct MainMenuComponents : IComponent
{
    public Button play;
    public Button exit;
}

