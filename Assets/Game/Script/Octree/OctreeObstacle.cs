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

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void UpdateCachedData()
    {
        CachedPosition = transform.position;
        CachedBounds = Collider.bounds;
        CachedRadius = Collider.bounds.extents.magnitude;
    }

    void Start()
    {
        MeshRenderer.material.color = Color.red;
    }

    public void AddHighlight(Color color)
    {
        print(this.gameObject.name +" is Changing color to " + color);
        MeshRenderer.material.color = color;
    }

    public void RemoveHighlight()
    {
        MeshRenderer.material.color = Color.red;
    }    
}
