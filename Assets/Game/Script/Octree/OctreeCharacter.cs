using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeCharacter : MonoBehaviour
{
    [SerializeField] private Octree Octree;
    [SerializeField] private float SearchRange;

    HashSet<ISpacialData3D> highlightedData = new HashSet<ISpacialData3D>();

    // Update is called once per frame
    void Update()
    {
        //Highlight surrounding obstacles
        HashSet<ISpacialData3D> newDatas = Octree.FindDataInRange(transform.position, SearchRange);
        highlightedData.ExceptWith(newDatas);

        foreach (var data in highlightedData)
            data.GetGameObject().GetComponent<OctreeObstacle>().RemoveHighlight();
        highlightedData.Clear();

        foreach (var data in newDatas)
        {
            data.GetGameObject().GetComponent<OctreeObstacle>().AddHighlight();
            highlightedData.Add(data);
        }
    }
}
