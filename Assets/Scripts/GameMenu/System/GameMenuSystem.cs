using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using TMPro;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GameMenuSystem))]
public sealed class GameMenuSystem : UpdateSystem
{
    private Filter filter;
    private Filter filter1;
    private Event<GameMenuEvent> gameMenuEvent;
    private Event<CloseAdvEvent> closeAdvEvent;
    private Event<OpenAdvEvent> openAdvEvent;
    private Event<CreateAdvEvent> createAdvEvent;
    private Request<CreateAdvRequest> createAdvRequest;
    
    private List<GameObject> gamesObject = new List<GameObject>();
    private GameObject parent;

    private bool token;
    private TMP_Text timeText;
    private TMP_Text scoreText;
    private float time;
    private float width;
    private float height;
    private float score;
    
    private Action startGame;
    
    public override void OnAwake() 
    {
        this.filter = this.World.Filter.With<GameMenuComponent>().Build();
        gameMenuEvent = World.GetEvent<GameMenuEvent>();
        closeAdvEvent = World.GetEvent<CloseAdvEvent>();
        openAdvEvent = World.GetEvent<OpenAdvEvent>();
        createAdvEvent = World.GetEvent<CreateAdvEvent>();
        createAdvRequest = World.GetRequest<CreateAdvRequest>();
        
        gameMenuEvent.Subscribe(value =>
        {
            SetTime(3.0f);
        });

        closeAdvEvent.Subscribe(value =>
        {
            StartGame();
            score++;
            scoreText.text = score.ToString("N");
        });

        openAdvEvent.Subscribe(value =>
        {
            var v = value.data[0].takeAwayScore;
            
            StartGame();
            score -= v;
            scoreText.text = score.ToString("N");
        });
        
        startGame += StartGame;
    }

    public override void OnUpdate(float deltaTime) 
    {
        foreach (var entity in this.filter)
        {
            ref var component = ref entity.GetComponent<GameMenuComponent>();
            timeText = component.textTime;
            scoreText = component.scoreText;
            gamesObject = component.gameObjects;
            parent = component.field;
            width = Screen.width / 2;
            height = Screen.height / 2;
        }

        Timer(deltaTime);
    }

    private void StartGame()
    {
        var rand = Random.Range(0, gamesObject.Count);
        var go = Instantiate(gamesObject[rand].gameObject, parent.transform);
        go.GetComponent<RectTransform>().SetAsFirstSibling();
        
        var deltaW = go.GetComponent<RectTransform>().rect.width / 2;
        var deltaH = go.GetComponent<RectTransform>().rect.height / 2;
        
        var randVector = new Vector2(Random.Range((-width) + deltaW, width - deltaW), Random.Range((-height) + deltaH, height - deltaH));
        
        go.transform.localPosition = randVector;
        
        createAdvRequest.Publish(new CreateAdvRequest());
        createAdvEvent.NextFrame(new CreateAdvEvent());
    }
    
    private void SetTime(float time)
    {
        score = 0;
        this.time = time;
        timeText.text = time.ToString("F1");
        token = true;
    }
    
    private void Timer(float deltaTime)
    {
        if (token)
        {
            time -= deltaTime;
            timeText.text = time.ToString("F1");
            
            if (time <= 0)
            {
                token = false;
                timeText.gameObject.SetActive(false);
                startGame.Invoke();
            }
        }
    }
}