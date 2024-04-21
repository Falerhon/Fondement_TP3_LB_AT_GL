using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeObstacle : MonoBehaviour, ISpacialData3D
{
    [SerializeField] Collider Collider;
    [SerializeField] MeshRenderer MeshRenderer;

    Vector3? CachedPosition;
    Bounds? CachedBounds;
    float? CachedRadius;

    bool IsCachedDataInvalid
    {
        get{
            if (CachedPosition == null || CachedBounds == null || CachedRadius == null)
                return true;
            //Check if data is still the same
            return !Mathf.Approximately((transform.position - CachedPosition.Value).sqrMagnitude, 0f);
        }
    }

    Bounds ISpacialData3D.GetBounds()
    {
        if (IsCachedDataInvalid)
            UpdateCachedData();
        return CachedBounds.Value;
    }

    Vector3 ISpacialData3D.GetLocation()
    {
        if (IsCachedDataInvalid)
            UpdateCachedData();
        return CachedPosition.Value;
    }

    float ISpacialData3D.GetRadius()
    {
        if (IsCachedDataInvalid)
            UpdateCachedData();
        return CachedRadius.Value;
    }

    private void UpdateCachedData()
    {
        CachedPosition = transform.position;
        CachedBounds = Collider.bounds;
        CachedRadius = Collider.bounds.extents.magnitude;        
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
