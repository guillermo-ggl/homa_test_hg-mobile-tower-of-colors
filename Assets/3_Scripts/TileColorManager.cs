using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileColorManager : Singleton<TileColorManager>
{
    [SerializeField]
    List<TileColorList> tileColorLists;

    List<int> shuffledIndexes;
    List<Color> currentColorList;
    Color currentDisabledColor;

    public int ColorCount { get; private set; }
    public Color GetColor(int index) => currentColorList[shuffledIndexes[index]];
    public Color GetDisabledColor() => currentDisabledColor;

    public System.Action OnColorListChanged;

    public void SetColorList(int index)
    {
        if (index < tileColorLists.Count) {
            if (currentColorList == null || currentColorList.Count != tileColorLists[index].ColorCount) {
                shuffledIndexes = Enumerable.Range(0, tileColorLists[index].ColorCount - 1).ToList();
            }
            currentColorList = tileColorLists[index].GetListCopy();
            currentDisabledColor = tileColorLists[index].GetDisabledColor();
            if (ColorCount > 0)
                SetMaxColors(ColorCount, false);
            else
                OnColorListChanged?.Invoke();
        }
    }

    public void SetMaxColors(int maxColors, bool randomize)
    {
        if (maxColors <= currentColorList.Count) {
            ColorCount = maxColors;

            if (randomize) {
                int n = shuffledIndexes.Count;
                while (n > 1) {
                    n--;
                    int k = Random.Range(0, n + 1);
                    int value = shuffledIndexes[k];
                    shuffledIndexes[k] = shuffledIndexes[n];
                    shuffledIndexes[n] = value;
                }
            }
            OnColorListChanged?.Invoke();
        }
    }

    public static Dictionary<int, Material> SharedMaterials = new Dictionary<int, Material>();
    public static Material GetSharedMaterial(Material baseMaterial, Color color)
    {
        if (!baseMaterial)
            return null;
        int hash = 17;
        unchecked {
            hash = hash * 31 + baseMaterial.GetHashCode();
            hash = hash * 31 + color.GetHashCode();
        }
        if (!SharedMaterials.ContainsKey(hash)) {
            Material newInstance = new Material(baseMaterial);
            newInstance.color = color;
            SharedMaterials.Add(hash, newInstance);
        }
        return SharedMaterials[hash];
    }
}
