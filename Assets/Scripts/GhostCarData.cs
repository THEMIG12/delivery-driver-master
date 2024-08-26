using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GhostCarData
{
    [SerializeField]
    List<GhostCarDataItem> ghostCarDataItems = new List<GhostCarDataItem>();

    public void AddDataItem(GhostCarDataItem ghostCarDataItem)
    {
        ghostCarDataItems.Add(ghostCarDataItem);
    }
    public List<GhostCarDataItem> GetDataItem()
    {
        return ghostCarDataItems;
    }
}
