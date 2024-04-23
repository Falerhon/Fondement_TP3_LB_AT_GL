using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeCharacter : MonoBehaviour, ISpacialData3D
{
    [SerializeField] private Octree Octree;
    [SerializeField] private float SearchRange;
    [SerializeField] private Collider goCollider;
    [SerializeField] private GameObject particle;
    [SerializeField] Collider Collider;

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
            data.GetGameObject().GetComponent<OctreeObstacle>().AddHighlight(Color.green);
            highlightedData.Add(data);

            //Check for collision
            if (data.GetBounds().Intersects(goCollider.bounds))
                IsColliding(data.GetGameObject());
        }
    }

    private void IsColliding(GameObject collidingObject)
    {
        GameObject particles = Instantiate(particle, collidingObject.transform.position, Quaternion.identity) as GameObject;
        Destroy(particles, 5f);
    }

    Vector3? CachedPosition;
    Bounds? CachedBounds;
    float? CachedRadius;

    bool IsCachedDataInvalid
    {
        get
        {
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

    public void UpdateCachedData()
    {
        CachedPosition = transform.position;
        CachedBounds = Collider.bounds;
        CachedRadius = Collider.bounds.extents.magnitude;
    }
}
