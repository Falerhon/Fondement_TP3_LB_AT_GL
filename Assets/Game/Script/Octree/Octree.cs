#define OCTREE_TrackStats
using System;
using System.Collections.Generic;
using UnityEngine;


public interface ISpacialData3D
{
    Vector3 GetLocation();
    Bounds GetBounds();
    float GetRadius();
    GameObject GetGameObject();
}

public class Octree : MonoBehaviour
{

    class Node
    {
        public Bounds NodeBounds;
        int DataCount; //Data inside this node and in its children
        public Node[] Children = new Node[0];
        Node Parent;
        public string Name;
        int Depth = -1;

        HashSet<ISpacialData3D> Data = new HashSet<ISpacialData3D>();

        public Node(Bounds InBounds, int InDepth = 0, string InName = "")
        {
            Name = InName;
            NodeBounds = InBounds;
            Depth = InDepth;
        }

        public void AddData(Octree Owner, ISpacialData3D Data3D)
        {
            //Check if this node as any child
            if (Children.Length == 0)
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

#if OCTREE_TrackStats
            Owner.NewNodesCreated(8, newDepth);
#endif

            Parent = this;
            Children = new Node[8]
            {
                //Lower layer
                new Node(new Bounds(NodeBounds.center + new Vector3(-offset.x,  -offset.y,  offset.z  ), childSize), newDepth, Name + " 1"),
                new Node(new Bounds(NodeBounds.center + new Vector3( offset.x,  -offset.y,  offset.z  ), childSize), newDepth, Name + " 2"),
                new Node(new Bounds(NodeBounds.center + new Vector3(-offset.x,  -offset.y, -offset.z  ), childSize), newDepth, Name + " 3"),
                new Node(new Bounds(NodeBounds.center + new Vector3( offset.x,  -offset.y, -offset.z  ), childSize), newDepth, Name + " 4"),
                                                                     
                //Upper Layer                                        
                new Node(new Bounds(NodeBounds.center + new Vector3(-offset.x,  offset.y,  offset.z  ), childSize), newDepth, Name + " 5"),
                new Node(new Bounds(NodeBounds.center + new Vector3( offset.x,  offset.y,  offset.z  ), childSize), newDepth, Name + " 6"),
                new Node(new Bounds(NodeBounds.center + new Vector3(-offset.x,  offset.y, -offset.z  ), childSize), newDepth, Name + " 7"),
                new Node(new Bounds(NodeBounds.center + new Vector3( offset.x,  offset.y, -offset.z  ), childSize), newDepth, Name + " 8")
            };

            foreach (var data in Data)
            {
                AddDataToChildren(Owner, data);
            }

            Data.Clear(); //All data was transfered to the children
        }

        public void MergeNodes(Octree Owner)
        {
            if (Children.Length != 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    AddDataToParent(Owner, Children[i]);
                }

                Children = new Node[0]; //Delete childs
            }

        }

        void AddDataToParent(Octree Owner, Node child)
        {
            if (child != null)
            {
                if (Parent != null && child.Children.Length == 0) //Child is a leaf
                {
                    foreach (var data in child.Data)
                    {
                        Parent.Data.Add(data);
                    }

                    return;
                }
                else if (Children.Length != 0 && child != null) //Child is not null
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (Children[i] != null)
                            AddDataToParent(Owner, Children[i]);
                    }
                }
            }

        }

        void AddDataToChildren(Octree Owner, ISpacialData3D Data3D)
        {
            foreach (var Child in Children)
            {
                if (Child != null && Child.Overlaps(Data3D.GetBounds()))
                {
                    Child.AddData(Owner, Data3D);
                }
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
            if (Children.Length == 0)
            {
                if (Data == null || Data.Count == 0)
                    return;

                if (Depth == 0)//If Root with no child, optimized check
                {
                    foreach (var data in Data)
                    {
                        if (SearchBounds.Intersects(data.GetBounds()))
                            OutFoundData.Add(data);
                    }

                    return;
                }

                OutFoundData.UnionWith(Data);
                return;
            }

            foreach (var Child in Children)
            {
                if (Child != null)
                    if (Child.Overlaps(SearchBounds))
                        Child.FindDataInBox(SearchBounds, OutFoundData);
            }
        }

        public void FindDataInRange(Vector3 SearchLocation, float SearchRange, HashSet<ISpacialData3D> OutFoundData)
        {
            if (Depth != 0)
            {
                throw new System.InvalidOperationException("FindDataInRange cannot be run on anything other than the root node");
            }

            Bounds searchBounds = new Bounds(SearchLocation, SearchRange * Vector3.one * 2f);

            FindDataInBox(searchBounds, OutFoundData);
        }

        public int RecalculateDataInChildren(Octree Owner)
        {
            DataCount = 0;
            if (Children.Length == 0)
            {
                if (Data == null || Data.Count == 0)
                    return 0;

                List<ISpacialData3D> dataToRemove = new List<ISpacialData3D>();

                foreach (var data in Data)
                {
                    if (!NodeBounds.Intersects(data.GetBounds())) //Data moved, no longer in this node
                    {
                        dataToRemove.Add(data);
                    }
                }

                if (dataToRemove.Count != 0)
                {
                    foreach (var data in dataToRemove)
                    {
                        Data.Remove(data);
                        Owner.AddData(data);
                    }
                }

                CheckDataCount(Owner);
                return Data.Count;
            }
            else
            {
                //Double check that child weren't deleted while doing the other stuff
                for (int i = 0; i < 8; i++)
                {
                    if (Children.Length != 0)
                        DataCount += Children[i].RecalculateDataInChildren(Owner);
                }

                CheckDataCount(Owner);
                return DataCount;
            }
        }

        void CheckDataCount(Octree Owner)
        {
            //Should we split AND are we able to split?
            if ((Data.Count) > Owner.PreferedMaxDataPerNode && CanSplit(Owner) && Children.Length == 0)
            {
                SplitNode(Owner);
                foreach (var data in Data)
                    AddDataToChildren(Owner, data);
            }
            else if (DataCount <= Owner.PreferedMaxDataPerNode && Children.Length != 0)
                MergeNodes(Owner);
        }
    }



    [field: SerializeField] public int PreferedMaxDataPerNode { get; private set; } = 50;
    [field: SerializeField] public int MinimumNodeSize { get; private set; } = 5;


    Node RootNode;
    public bool isOctreeReady = false;
    public void PrepareTree(Bounds inBounds)
    {
        RootNode = new Node(inBounds, 0, "0");
#if OCTREE_TrackStats
        MaxDepth = 0;
        NbNodes = 1;
#endif
    }

    public void AddData(ISpacialData3D Data3D)
    {
        RootNode.AddData(this, Data3D);
    }

    public void AddData(HashSet<ISpacialData3D> Data3Ds)
    {
        foreach (ISpacialData3D data in Data3Ds)
        {
            AddData(data);
        }
    }
    public HashSet<ISpacialData3D> FindDataInRange(Vector3 SearchLocation, float SearchRange)
    {
        HashSet<ISpacialData3D> foundData = new();

        RootNode.FindDataInRange(SearchLocation, SearchRange, foundData);
        return foundData;
    }

    public void ShowStats()
    {
#if OCTREE_TrackStats
        Debug.Log($"Max Depth : {MaxDepth}");
        Debug.Log($"Nb Nodes : {NbNodes}");
#endif
    }

    List<Node> nodesToPrint = new List<Node>();
    private void OnDrawGizmos()
    {
        if (RootNode != null)
        {
            nodesToPrint.Clear();
            FindNodes(RootNode);
            foreach (var node in nodesToPrint)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(node.NodeBounds.center, node.NodeBounds.size);
            }
        }
    }

    private void Update()
    {
        if (isOctreeReady)
            UpdateOctree(RootNode);
    }

    void UpdateOctree(Node node)
    {
        //If leaf
        if (node.Children == null)
            return;
        else
            node.RecalculateDataInChildren(this);

    }

    void FindNodes(Node node)
    {
        if (node == null)
            return;

        // if node is leaf node, print its data
        if (node.Children == null)
        {
            nodesToPrint.Add(node);
            return;
        }
        else
        {
            nodesToPrint.Add(node);
            foreach (var child in node.Children)
                FindNodes(child);
        }
    }

#if OCTREE_TrackStats
    int MaxDepth = -1;
    int NbNodes = 0;

    public void NewNodesCreated(int NbAdded, int NodeDepth)
    {
        NbNodes += NbAdded;
        MaxDepth = Mathf.Max(NodeDepth, MaxDepth);
    }
#endif
}
