using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTile : MonoBehaviour
{
    [SerializeField]
    protected new MeshRenderer renderer;
    [SerializeField]
    protected Material originalMaterial;
    [SerializeField]
    new Rigidbody rigidbody;
    [SerializeField]
    float chainExplodeDelay = 0.1f;
    [SerializeField]
    ParticleSystem explosionFx;
    [SerializeField]
    bool colorizeFx;
    [SerializeField]
    float raycastCheckInterval = 0.25f;

    [SerializeField]
    float waterDamp = 1f;

    public int Floor { get; set; }
    public int ColorIndex { get; protected set; }
    public System.Action<TowerTile> OnTileDestroyed;
    public bool Active { get; protected set; }

    List<TowerTile> connectedTiles = new List<TowerTile>();
    float nextCheckTime = 0;
    private bool drifting;
    private bool initialized;
    private bool freezed;

    protected virtual void Awake()
    {
        TileColorManager.Instance.OnColorListChanged += ResetColor;
    }

    protected virtual void OnDestroy()
    {
        if (CameraShakeManager.Instance)
            CameraShakeManager.Instance.Play(0);
        if (TileColorManager.Instance)
            TileColorManager.Instance.OnColorListChanged -= ResetColor;
    }

    private void FixedUpdate()
    {
        if (Active) {
            if (Time.time > nextCheckTime && rigidbody.velocity.y < -1) {
                nextCheckTime = Time.time + raycastCheckInterval;
                if (!Physics.Raycast(rigidbody.worldCenterOfMass, Vector3.down, rigidbody.worldCenterOfMass.y + 1, 1 << 9)) {
                    Active = false;
                    OnTileDestroyed?.Invoke(this);
                }
            }
        } else if (!freezed) {
            Vector3 actionPoint = rigidbody.worldCenterOfMass;
            float forceFactor = 1f - ((actionPoint.y) / 0.5f);
            if (actionPoint.y < -2.5f) {
                if (!drifting) {
                    drifting = true;
                    rigidbody.angularDrag = 0.1f;
                    rigidbody.drag = 0.1f;
                }
                var uplift = -Physics.gravity * (forceFactor - rigidbody.velocity.y * waterDamp);
                rigidbody.AddForceAtPosition(uplift, actionPoint);
            }
        }
    }

    private void Reset()
    {
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<MeshRenderer>();
        if (renderer)
            originalMaterial = renderer.sharedMaterial;
    }

    public void ResetColor()
    {
        if (Active || !initialized)
            SetColor(TileColorManager.Instance.GetColor(ColorIndex));
        else
            SetColor(TileColorManager.Instance.GetDisabledColor());
    }

    public void SetColorIndex(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < TileColorManager.Instance.ColorCount)
            ColorIndex = colorIndex;
        SetColor(TileColorManager.Instance.GetColor(colorIndex));
    }

    public virtual void SetColor(Color color)
    {
        renderer.sharedMaterial = TileColorManager.GetSharedMaterial(originalMaterial, color);
    }

    public void SetFreezed(bool value)
    {
        if (freezed != value) {
            freezed = value;
            rigidbody.isKinematic = value;
        }
    }

    public void SetEnabled(bool value)
    {
        initialized = true;
        Active = value;
        if (value)
            SetColor(TileColorManager.Instance.GetColor(ColorIndex));
        else
            SetColor(TileColorManager.Instance.GetDisabledColor());
    }

    private void OnTriggerEnter(Collider other)
    {
        TowerTile otherTile = other.transform.parent?.GetComponent<TowerTile>();
        if (otherTile && otherTile.ColorIndex == ColorIndex) {
            connectedTiles.Add(otherTile);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TowerTile otherTile = other.transform.parent?.GetComponent<TowerTile>();
        if (otherTile && otherTile.ColorIndex == ColorIndex) {
            connectedTiles.Remove(otherTile);
        }
    }

    public virtual void Explode(bool instant = false)
    {
        if (Active) {
            Active = false;
            StartCoroutine(ChainExplodeRoutine(instant));
        }
    }

    IEnumerator ChainExplodeRoutine(bool instant)
    {
        if (!instant)
            yield return new WaitForSeconds(chainExplodeDelay * Time.timeScale);
        for (int i = 0; i < connectedTiles.Count; i++) {
            connectedTiles[i]?.Explode(false);
        }
        OnTileDestroyed?.Invoke(this);
        ParticleSystem fx = FxPool.Instance.GetPooled(explosionFx, transform.position, Quaternion.identity);
        if (colorizeFx) {
            ParticleSystem.MainModule main = fx.main;
            main.startColor = TileColorManager.Instance.GetColor(ColorIndex);
        }
        Destroy(gameObject);
    }
}
