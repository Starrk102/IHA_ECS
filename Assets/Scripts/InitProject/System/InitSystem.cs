using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InitSystem))]
public sealed class InitSystem : UpdateSystem
{
    private Filter filter;
    private Stash<InitComponent> initStash;
    
    private Request<InitRequest> initRequest;
    private Event<InitEvent> initEvent;
    
    public override void OnAwake()
    {
        initRequest = World.GetRequest<InitRequest>();
        initEvent = World.GetEvent<InitEvent>();
        
        initRequest.Publish(new InitRequest
        {
            id = (int)ScreensName.MainMenu
        });
        
        this.filter = this.World.Filter.With<InitComponent>().Build();
        this.initStash = this.World.GetStash<InitComponent>();
        
        foreach (var entity in this.filter)
        {
            ref var init = ref initStash.Get(entity);
        }
    }

    public override void OnUpdate(float deltaTime) 
    {
        foreach (var tRequest in initRequest.Consume())
        {
            initEvent.NextFrame(new InitEvent
            {
                id = tRequest.id
            });
        }
    }
}