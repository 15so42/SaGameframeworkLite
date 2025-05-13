using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDataRow : SoDataRow
{
    public Sprite icon;
    public string desc;
    public int maxStack;
    public string timeline;
}
[CreateAssetMenu(menuName = "SoDataTable/Item表")]
public class ItemDataTable : SoDataTable<ItemDataRow>
{
    
}
