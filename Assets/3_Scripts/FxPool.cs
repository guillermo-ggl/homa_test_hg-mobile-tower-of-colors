using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxPool
{
    static FxPool m_instance;
    public static FxPool Instance
    {
        get {
            if (m_instance == null)
                m_instance = new FxPool();
            return m_instance;
        }
    }

    Dictionary<ParticleSystem, List<ParticleSystem>> poolDict;
    FxPool()
    {
        poolDict = new Dictionary<ParticleSystem, List<ParticleSystem>>();
    }

    public void EnsureQuantity(ParticleSystem prototype, int count)
    {
        if (!poolDict.ContainsKey(prototype)) {
            poolDict.Add(prototype, new List<ParticleSystem>());
        }
        int prevCount = poolDict[prototype].Count;
        for (int i = 0; i < count - prevCount; i++) {
            ParticleSystem newObj = Object.Instantiate(prototype);
            newObj.gameObject.SetActive(false);
            Object.DontDestroyOnLoad(newObj);
            poolDict[prototype].Add(newObj);
        }
    }

    public ParticleSystem GetPooled(ParticleSystem prototype, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!poolDict.ContainsKey(prototype)) {
            poolDict.Add(prototype, new List<ParticleSystem>());
        }
        ParticleSystem unused = poolDict[prototype].Find(x => !x.gameObject.activeSelf);
        if (!unused) {
            unused = Object.Instantiate(prototype, position, rotation, parent);
            poolDict[prototype].Add(unused);
            Object.DontDestroyOnLoad(unused);
        } else {
            unused.transform.position = position;
            unused.transform.rotation = rotation;
            if (unused.transform.parent != parent)
                unused.transform.parent = parent;
            unused.gameObject.SetActive(true);
        }
        return unused;
    }
}
