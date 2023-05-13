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
        rocket,
        bomb,
        globe
    }

    public Sprite rocketSprite;
    public Sprite bombSprite;
    public Sprite globeSprite;

    public int destroyCondition = 3;
    public int rocketCondition = 5;
    public int bombCondition = 7;
    public int globeCondition = 9;

    public override void Setup()
    {
        image.sprite = startingSprite;
    }

    public override void UpdateBlock()
    {
        throw new System.NotImplementedException();
    }

    public override void OnNeighbourDestroyed()
    {

    }

    public override void Destroy()
    {
        throw new System.NotImplementedException();
    }
}
