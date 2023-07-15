using GXPEngine;
using System;
using System.Collections.Generic;
class DijkstraPathFinder : PathFinder
{
    public DijkstraPathFinder(NodeGraph pNodeGraph) : base(pNodeGraph)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        List<Node> todoList = new List<Node>() { pFrom };
        Node _currentNode = pFrom;
        List<Node> visited = new List<Node>();
        Dictionary<Node, Node> nodeParent = new Dictionary<Node, Node>();
        List<Node> _pathReversed = new List<Node>();
        List<Node> _path = new List<Node>();
        bool done = false;

        while (!done && todoList.Count > 0)
        {
            int minimumDistance = 999999;
            List<Node> _minimumDistanceNodes = new List<Node>();
            foreach (Node node in _currentNode.connections)
            {
                if (node.location.X - _currentNode.location.X < minimumDistance)
                    minimumDistance = node.location.X - _currentNode.location.X;
            }
            foreach (Node node in _currentNode.connections)
            {
                if (node.location.X - _currentNode.location.X == minimumDistance)
                    _minimumDistanceNodes.Add(node);
            }
            visited.Add(_currentNode);
            todoList.Remove(_currentNode);
            if (_currentNode.Equals(pTo))
                done = true;
            foreach (Node node in _currentNode.connections)
            {
                if (!todoList.Contains(node) && !visited.Contains(node))
                {
                    visited.Add(node);
                    todoList.Add(node);
                }
            }
        }
        return null;
    }
}