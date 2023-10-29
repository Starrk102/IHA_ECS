using System;
using System.Linq;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ScreenManagerSystem))]
public sealed class ScreenManagerSystem : UpdateSystem
{
    private Filter filter;
    private Stash<ScreenManagerComponent> screenManagerStash;

    private Event<InitEvent> initEvent;
    private Event<CreateScreenEvent> createScreenEvent;
    private Request<GameMenuRequest> gameMenuRequest;
    private Event<GameMenuEvent> gameMenuEvent;

    private GameObject currentScreen;

    private bool token = false;
    
    public override void OnAwake() 
    {
        this.filter = this.World.Filter.With<ScreenManagerComponent>().Build();
        this.screenManagerStash = this.World.GetStash<ScreenManagerComponent>();

        initEvent = World.GetEvent<InitEvent>();
        createScreenEvent = World.GetEvent<CreateScreenEvent>();
        
        gameMenuRequest = World.GetRequest<GameMenuRequest>();
        gameMenuEvent = World.GetEvent<GameMenuEvent>();
        
        foreach (var entity in this.filter)
        {
            ref var manager = ref screenManagerStash.Get(entity);
            var screenManagerComponent = manager;
            initEvent.Subscribe((value) =>
            {
                CreateScreen(screenManagerComponent, ScreensName.MainMenu, screenManagerComponent.canvas.transform);    
            });
            
            createScreenEvent.Subscribe(value =>
            {
                var findElement = value.data[0];
                CreateScreen(screenManagerComponent, 
                    findElement.screensName, 
                    screenManagerComponent.canvas.transform, 
                    () => { token = true; });
            });
        }
    }

    public override void OnUpdate(float deltaTime) 
    {
        if (token)
        {
            gameMenuRequest.Publish(new GameMenuRequest
            {
                token = true
            });

            token = false;
        }
        
        foreach (var tRequest in gameMenuRequest.Consume())
        {
            gameMenuEvent.NextFrame(new GameMenuEvent
            {
                token = tRequest.token
            });
        }
    }
    
    private void CreateScreen(ScreenManagerComponent manager, ScreensName screensName, Transform parent, Action action = default)
    {
        action?.Invoke();
        
        if (currentScreen != null)
        {
            Destroy(currentScreen);
        }
        
        var createScreen = manager.screens[(int)screensName];
        var go = Instantiate(createScreen, parent);
        currentScreen = go;
    }
}