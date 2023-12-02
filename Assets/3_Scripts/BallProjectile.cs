using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallProjectile : MonoBehaviour
{
    new Rigidbody rigidbody;
    int ColorIndex = 0;
    new Renderer renderer;
    Material originalMaterial;

    TowerTile target;
    Collider _collider;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
        
        if (renderer)
            originalMaterial = renderer.material;

        TileColorManager.Instance.OnColorListChanged += ResetColor;
    }

    private void Update()
    {
        if(target) _collider.isTrigger = target.ColorIndex == ColorIndex;
    }

    private void OnDestroy()
    {
        if (TileColorManager.Instance)
            TileColorManager.Instance.OnColorListChanged -= ResetColor;
    }

    public void ResetColor()
    {
        SetColor(TileColorManager.Instance.GetColor(ColorIndex));
    }

    public void SetColor(Color color)
    {
        renderer.sharedMaterial = TileColorManager.GetSharedMaterial(originalMaterial, color);
    }

    public void SetColorIndex(int index)
    {
        ColorIndex = index;
        SetColor(TileColorManager.Instance.GetColor(index));
    }

    public void SetVelocity(Vector3 velocity)
    {
        transform.parent = null;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(velocity, ForceMode.VelocityChange);
    }

    public void SetTarget(TowerTile target)
    {
        this.target = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        TowerTile tile = other.GetComponentInParent<TowerTile>();
        if (tile == target) {
            Explode();
            tile?.Explode(true);
        }
    }

    public void Explode()
    {
        Destroy(this.gameObject);
    }
}
