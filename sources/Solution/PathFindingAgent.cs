
class PathFindingAgent : OnGraphWayPointAgent
{
    private PathFinder _pathFinder;
    public PathFindingAgent(NodeGraph pNodeGraph, PathFinder pPathFinder) : base(pNodeGraph)
    {
        _pathFinder = pPathFinder;
        pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
    }

    protected override void SwitchAgentState()
    {
    }

    protected override void onNodeClickHandler(Node pNode)
    {
        if (_target.Count == 0)
            _target = _pathFinder.Generate(currentNode, pNode);
    }
}
