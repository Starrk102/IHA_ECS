using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;


[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MainMenuSystem))]
public sealed class MainMenuSystem : UpdateSystem
{
    private Filter filter;
    private Stash<MainMenuComponents> mainMenuStash;
    
    private Request<CreateScreenRequest> createScreenRequest;
    private Event<CreateScreenEvent> createScreenEvent;
    private MainMenuComponents mainMenuComponents;
    
    public override void OnAwake()
    {
        this.filter = this.World.Filter.With<MainMenuComponents>().Build();
        this.mainMenuStash = this.World.GetStash<MainMenuComponents>();
        
        createScreenRequest = World.GetRequest<CreateScreenRequest>();
        createScreenEvent = World.GetEvent<CreateScreenEvent>();
    }

    public override void OnUpdate(float deltaTime) 
    {
        foreach (var entity in this.filter)
        {
            ref var menuComponents = ref mainMenuStash.Get(entity);
            mainMenuComponents = menuComponents;
                
            mainMenuComponents.play.onClick.AddListener(() =>
            {
                createScreenRequest.Publish(new CreateScreenRequest
                {
                    screensName = ScreensName.GameScene
                });
            });
            
            mainMenuComponents.exit.onClick.AddListener(Application.Quit);
        }
        
        foreach (var tRequest in createScreenRequest.Consume())
        {
            createScreenEvent.NextFrame(new CreateScreenEvent
            {
                screensName = tRequest.screensName
            });
        }
    }

}
