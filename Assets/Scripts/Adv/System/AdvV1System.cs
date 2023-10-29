using Coffee.UIEffects;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(AdvV1System))]
public sealed class AdvV1System : UpdateSystem 
{
    private Filter filter;
    private Stash<AdvV1Component> gameObjectStash;
    private Request<CloseAdvRequest> closeAdvRequest;
    private Event<CloseAdvEvent> closeAdvEvent;
    private Request<OpenAdvRequest> openAdvRequest;
    private Event<OpenAdvEvent> openAdvEvent;
    private Event<CreateAdvEvent> createAdvEvent;

    private bool token = false;
    
    private int takeAwayScore;
    
    public override void OnAwake() 
    {
        this.filter = this.World.Filter.With<AdvV1Component>().Build();
        this.gameObjectStash = this.World.GetStash<AdvV1Component>();
        
        closeAdvRequest = World.GetRequest<CloseAdvRequest>();
        closeAdvEvent = World.GetEvent<CloseAdvEvent>();
        openAdvRequest = World.GetRequest<OpenAdvRequest>();
        openAdvEvent = World.GetEvent<OpenAdvEvent>();
        createAdvEvent = World.GetEvent<CreateAdvEvent>();

        createAdvEvent.Subscribe(value =>
        {
            foreach (var entity in this.filter)
            {
                ref var component = ref gameObjectStash.Get(entity);

                //component.thisGo.GetComponent<UIDissolve>().effectPlayer.play;
                    
                var objectComponent = component;

                takeAwayScore = objectComponent.takeAwayScore;
                
                var w = component.openObject.GetComponent<RectTransform>().rect.width / 2;
                var h = component.openObject.GetComponent<RectTransform>().rect.height / 2;
                var deltaW = component.closeObject.GetComponent<RectTransform>().rect.width / 2;
                var deltaH = component.closeObject.GetComponent<RectTransform>().rect.height / 2;
            
                var randVector = new Vector2(Random.Range((-w) + deltaW, w - deltaW), Random.Range((-h) + deltaH, h - deltaH));
            
                component.closeObject.gameObject.transform.localPosition = randVector;
            
                component.closeObject.onClick.AddListener(() =>
                {
                    closeAdvRequest.Publish(new CloseAdvRequest());
                    closeAdvEvent.NextFrame(new CloseAdvEvent());

                    Destroy(objectComponent.thisGo);
                });
            
                component.openObject.onClick.AddListener(() =>
                {
                    openAdvRequest.Publish(new OpenAdvRequest
                    {
                        takeAwayScore = takeAwayScore
                    });

                    openAdvEvent.NextFrame(new OpenAdvEvent
                    {
                        takeAwayScore = takeAwayScore
                    });

                        
                    Destroy(objectComponent.thisGo);
                });
            }
        });
    }

    public override void OnUpdate(float deltaTime) 
    {

    }
}