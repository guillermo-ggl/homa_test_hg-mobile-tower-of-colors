using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tile Color List")]
public class TileColorList : ScriptableObject
{
    [SerializeField]
    Color DisabledColor = Color.grey;
    [SerializeField]
    List<Color> ColorList = new List<Color>{
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.magenta
    };

    public List<Color> GetListCopy()
    {
        return new List<Color>(ColorList);
    }

    public int ColorCount => ColorList.Count;
    public Color GetColor(int index) => ColorList[index];
    public Color GetDisabledColor() => DisabledColor;
}
