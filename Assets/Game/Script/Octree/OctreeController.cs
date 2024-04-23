using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeController : MonoBehaviour
{
    [SerializeField] private Octree Octree;

    public void OnBoundsCalculated(Bounds bounds)
    {
        Octree.PrepareTree(bounds);
    }

    public void OnItemSpawned(GameObject gameObject)
    {
        OctreeObstacle obstacle = gameObject.GetComponent<OctreeObstacle>();
        if (obstacle != null)
            Octree.AddData(obstacle);
        Octree.ShowStats();
    }

    public void OnAllItemSpawned()
    {
        Octree.isOctreeReady = true;
    }
}
