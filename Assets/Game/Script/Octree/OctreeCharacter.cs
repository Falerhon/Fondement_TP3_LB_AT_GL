using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeCharacter : MonoBehaviour
{
    [SerializeField] private Octree Octree;
    [SerializeField] private float SearchRange;
    [SerializeField] private Collider goCollider;
    [SerializeField] private GameObject particle;

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
}
