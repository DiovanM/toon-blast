using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBlock : BlockBase
{

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

    public override void OnClick()
    {
        throw new System.NotImplementedException();
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
