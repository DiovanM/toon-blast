using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBlock : BlockBase
{

    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public Orientation orientation;
    public GameObject fireObject;

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
        fireObject.SetActive(false);
    }

    public void Play()
    {
        fired = true;
        image.enabled = false;
        fireObject.SetActive(true);
    }

}
