using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISpacialData3D
{
    Vector3 GetLocation();
    Bounds GetBounds();
    float GetRadius();
}

public class Octree : MonoBehaviour
{

    class Node
    {
        Bounds NodeBounds;
        Node[] Children;
        int Depth = -1;

        HashSet<ISpacialData3D> Data;

        public Node(Bounds InBounds, int InDepth = 0)
        {
            NodeBounds = InBounds;
            Depth = InDepth;
        }

        public void AddData(Octree Owner, ISpacialData3D Data3D)
        {
            //Check if this node as any child
            if(Children == null)
            {
                //Is it the first time that data is added to this node?
                if (Data == null)
                    Data = new();

                //Should we split AND are we able to split?
                if ((Data.Count + 1) >= Owner.PreferedMaxDataPerNode && CanSplit(Owner))
                {
                    SplitNode(Owner);
                    AddDataToChildren(Owner, Data3D);
                }
                else
                    Data.Add(Data3D);

                return;
            }

            AddDataToChildren(Owner, Data3D);
        }

        void SplitNode(Octree Owner)
        {
            //Get the size of the new nodes
            Vector3 childSize = NodeBounds.extents;
            Vector3 offset = childSize / 2f;
            int newDepth = Depth + 1;

            Children = new Node[8]
            {
                //Lower layer
                new Node(new Bounds(NodeBounds.center + new Vector3(-offset.x,  -offset.y,  offset.z  ), childSize), newDepth),
                new Node(new Bounds(NodeBounds.center + new Vector3( offset.x,  -offset.y,  offset.z  ), childSize), newDepth),
                new Node(new Bounds(NodeBounds.center + new Vector3(-offset.x,  -offset.y, -offset.z  ), childSize), newDepth),
                new Node(new Bounds(NodeBounds.center + new Vector3( offset.x,  -offset.y, -offset.z  ), childSize), newDepth),
                                                                     
                //Upper Layer                                        
                new Node(new Bounds(NodeBounds.center + new Vector3(-offset.x,  offset.y,  offset.z  ), childSize), newDepth),
                new Node(new Bounds(NodeBounds.center + new Vector3( offset.x,  offset.y,  offset.z  ), childSize), newDepth),
                new Node(new Bounds(NodeBounds.center + new Vector3(-offset.x,  offset.y, -offset.z  ), childSize), newDepth),
                new Node(new Bounds(NodeBounds.center + new Vector3( offset.x,  offset.y, -offset.z  ), childSize), newDepth)
            };

            foreach(var data in Data)
            {
                AddDataToChildren(Owner, data);
            }

            Data = null; //All data was transfered to the children
        }

        void AddDataToChildren(Octree Owner, ISpacialData3D Data3D)
        {
            foreach(var Child in Children)
            {
                if (Child.Overlaps(Data3D.GetBounds()))
                    Child.AddData(Owner, Data3D);
            }
        }

        bool Overlaps(Bounds Other)
        {
            return NodeBounds.Intersects(Other);
        }

        bool CanSplit(Octree Owner)
        {
            return NodeBounds.size.x >= Owner.MinimumNodeSize &&
                   NodeBounds.size.y >= Owner.MinimumNodeSize &&
                   NodeBounds.size.z >= Owner.MinimumNodeSize;
        }

        public void FindDataInBox(Bounds SearchBounds, HashSet<ISpacialData3D> OutFoundData)
        {
            if(Children == null)
            {
                if (Data == null || Data.Count == 0)
                    return;

                if(Depth == 0)//If Root with no child, optimized check
                {
                    foreach(var data in Data)
                    {
                        if (SearchBounds.Intersects(data.GetBounds()))
                            OutFoundData.Add(data);
                    }

                    return;
                }

                OutFoundData.UnionWith(Data);
                return;
            }

            foreach(var Child in Children)
            {
                if (Child.Overlaps(SearchBounds))
                    Child.FindDataInBox(SearchBounds, OutFoundData);
            }

            //If we're on the root node, filter out data not within the search bounds
            if(Depth == 0)
            {
                OutFoundData.RemoveWhere(data =>
                {
                    return !SearchBounds.Intersects(data.GetBounds());
                });
            }
        }

        public void FindDataInRange(Vector3 SearchLocation, float SearchRange, HashSet<ISpacialData3D> OutFoundData)
        {
            if(Depth != 0)
            {
                throw new System.InvalidOperationException("FindDataInRange cannot be run on anything other than the root node");
            }

            Bounds searchBounds = new Bounds(SearchLocation, SearchRange * Vector3.one * 2f);

            FindDataInBox(searchBounds, OutFoundData);
        }
    }

    [field: SerializeField] public int PreferedMaxDataPerNode { get; private set; } = 50;
    [field: SerializeField] public int MinimumNodeSize { get; private set; } = 5;


    Node RootNode;

    public void PrepareTree(Bounds inBounds)
    {
        RootNode = new Node(inBounds);
    }

    public void AddData(ISpacialData3D Data3D)
    {
        RootNode.AddData(this, Data3D);
    }

    public void AddData(List<ISpacialData3D> Data3Ds)
    {
        foreach(ISpacialData3D data in Data3Ds)
        {
            AddData(data);
        }
    }

    public void ShowStats()
    {

    }

    public HashSet<ISpacialData3D> FindDataInRange(Vector3 SearchLocation, float SearchRange)
    {
        HashSet<ISpacialData3D> foundData = new();

        RootNode.FindDataInRange(SearchLocation, SearchRange, foundData);

        return foundData;
    }
}
