using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BlockType
{
    normal = 0,
    powerUp = 1,
    obstacle = 2,
}

[Serializable]
public abstract class BlockBase : MonoBehaviour
{

    public Action onClick;
    public Action onDestroy;

    [HideInInspector] public bool updated;
    public bool stationary;

    public string blockId;
    public Image image;
    public BlockType type;
    public Sprite startingSprite;

    public abstract void Setup();
    public abstract void OnClick();
    public abstract void OnNeighbourDestroyed();
    public abstract void Destroy();

}
