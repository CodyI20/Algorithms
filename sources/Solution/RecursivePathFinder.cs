using System.Collections.Generic;

class RecursivePathFinder : PathFinder
{
    public RecursivePathFinder(NodeGraph pNodeGraph) : base(pNodeGraph)
    {
    }
    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        List<Node> shortestPath = new List<Node>();
        List<Node> currentPath = new List<Node>();

        GenerateShortestPathRecursive(pFrom, pTo, currentPath, shortestPath);

        return shortestPath;
    }

    private void GenerateShortestPathRecursive(Node currentNode, Node targetNode, List<Node> currentPath, List<Node> shortestPath)
    {
        currentPath.Add(currentNode);

        if (currentNode.Equals(targetNode))
        {
            // Found a path to the target node
            if (shortestPath.Count == 0 || currentPath.Count < shortestPath.Count)
            {
                shortestPath.Clear();
                shortestPath.AddRange(currentPath);
            }
        }
        else
        {
            foreach (Node neighborNode in currentNode.connections)
            {
                if (!currentPath.Contains(neighborNode))
                {
                    if (shortestPath.Count == 0 || currentPath.Count < shortestPath.Count)
                        GenerateShortestPathRecursive(neighborNode, targetNode, currentPath, shortestPath);
                }
            }
        }

        currentPath.Remove(currentNode);
    }
}