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
        Destroy();
    }

    public override void Destroy()
    {
        Destroy(gameObject);
        onDestroy?.Invoke();
    }

    public override void OnClick()
    {

    }
}
