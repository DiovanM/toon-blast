using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NormalBlock : BlockBase
{

    public enum ReadyState
    {
        normal,
        destroyable,
        rocket,
        bomb,
        globe
    }

    public Sprite rocketSprite;
    public Sprite bombSprite;
    public Sprite globeSprite;

    [HideInInspector] public ReadyState readyState;
    [HideInInspector] public List<TileBase> adjacentTiles = new();

    public override void Setup()
    {
        image.sprite = startingSprite;
        readyState = ReadyState.normal;
    }

    public override void OnNeighbourDestroyed()
    {

    }

    public override void Destroy()
    {
        onDestroy?.Invoke();
        Destroy(gameObject);
    }

    public override void OnClick()
    {
        throw new NotImplementedException();
    }

    public void SetState(ReadyState state)
    {
        switch(state)
        {
            case ReadyState.rocket:
                image.sprite = rocketSprite;
                break;
            case ReadyState.bomb:
                image.sprite = bombSprite;
                break;
            case ReadyState.globe:
                image.sprite = globeSprite;
                break;
            case ReadyState.destroyable:
            case ReadyState.normal:
                image.sprite = startingSprite;
                break;
        }

        readyState = state;
    }


}
