// BulletVisuals.cs
using UnityEngine;

// Esta estrutura associa um tipo de poder a um sprite de projétil.
[System.Serializable]
public struct BulletVisual
{
    public PowerType powerType;
    public Sprite bulletSprite;
}