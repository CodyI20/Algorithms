using System;
using System.Collections.Generic;

class BreadthFirstPathFinder : PathFinder
{
    public BreadthFirstPathFinder(NodeGraph pNodeGraph) : base(pNodeGraph)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        /**/
        HashSet<Node> visited = new HashSet<Node>(); //Using a HashSet for efficient storing of visited nodes
        Dictionary<Node, Node> nodeParent = new Dictionary<Node, Node>(); //Using a Dictionary for efficient storing of parent elements(In order to create the path later on)
        LinkedList<Node> partialPath = new LinkedList<Node>();
        Queue<Node> queuedNodes = new Queue<Node>();
        Node _currentNode;

        _currentNode = pFrom;
        queuedNodes.Enqueue(pFrom);
        visited.Add(pFrom);

        while (queuedNodes.Count > 0)
        {
            _currentNode = queuedNodes.Dequeue();

            if (_currentNode.Equals(pTo))
                break;

            foreach (Node node in _currentNode.connections)
            {
                if (!visited.Contains(node))
                {
                    visited.Add(node);
                    queuedNodes.Enqueue(node);
                    nodeParent[node] = _currentNode;
                }
            }
        }

        if (_currentNode.Equals(pTo))
        {
            partialPath.AddFirst(pTo);
            while (_currentNode != pFrom)
            {
                _currentNode = nodeParent[_currentNode];
                partialPath.AddFirst(_currentNode);
            }
        }

        if (partialPath.Count == 0)
        {
            Console.WriteLine("Path not found!");
            return null;
        }

        List<Node> shortestPath = new List<Node>(partialPath);
        return shortestPath;

        /**
        List<Node> todoList = new List<Node>() { pFrom };
        Node _currentNode;
        List<Node> visited = new List<Node>();
        Dictionary<Node, Node> nodeParent = new Dictionary<Node, Node>();
        List<Node> _pathReversed = new List<Node>();
        List<Node> _path = new List<Node>();
        bool done = false;

        while (!done && todoList.Count > 0)
        {
            _currentNode = todoList[0];
            visited.Add(_currentNode);
            todoList.RemoveAt(0);

            if (_currentNode.Equals(pTo)) //If the node is found, then set done to true
                done = true;

            foreach (Node node in _currentNode.connections)
            {
                if (!todoList.Contains(node) && !visited.Contains(node))
                {
                    visited.Add(node);
                    todoList.Add(node);
                    nodeParent[node] = _currentNode;
                }
            }
        }
        if (done)
        {
            _pathReversed.Add(pTo);
            _currentNode = pTo;
            while (_currentNode != pFrom)
            {
                _pathReversed.Add(nodeParent[_currentNode]);
                _currentNode = nodeParent[_currentNode];
            }
            for(int i=_pathReversed.Count - 1; i >= 0; --i)
            {
                _path.Add(_pathReversed[i]);
            }
        }
        if (_path == null)
        {
            Console.WriteLine("Path not found!");
            return null;
        }
        return _path;
        /**/
    }
}