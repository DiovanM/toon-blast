using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeBlock : BlockBase
{

    public string normalBlockId;

    private bool fired;

    public override void Destroy()
    {
        if (!fired)
            onActivate?.Invoke();
        else
        {
            onDestroy?.Invoke();
            Destroy(gameObject);
        }
    }

    public override void OnNeighbourDestroyed()
    {

    }

    public override void Setup()
    {
        image.sprite = startingSprite;
    }

    public void Play()
    {
        fired = true;
    }

}
