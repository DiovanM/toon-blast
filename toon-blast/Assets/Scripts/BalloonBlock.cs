using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BalloonBlock : BlockBase
{

    public override void Setup()
    {
        image.sprite = startingSprite;
    }
    public override void OnNeighbourDestroyed()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateBlock()
    {
        throw new System.NotImplementedException();
    }

    public override void Destroy()
    {
        Destroy(gameObject);
    }
}
