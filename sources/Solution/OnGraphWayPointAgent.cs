using GXPEngine;
using System;
using System.Collections.Generic;

class OnGraphWayPointAgent : NodeGraphAgent
{
    protected enum AgentGrade { Sufficient_OnGraphWayPoint, Good_RandomWayPoint };

    protected List<Node> _target;
    protected Node currentNode = null;
    private int currentTargetNumber = 0;
    protected AgentGrade _agentGrade = AgentGrade.Sufficient_OnGraphWayPoint;

    private NodeGraph _nodeGraph; //used for Good
    public OnGraphWayPointAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
    {
        Console.WriteLine($"\t!!!The state of the Agent is {_agentGrade}");
        _nodeGraph = pNodeGraph;

        SetOrigin(width / 2, height / 2);
        AgentSettingsSet(_agentGrade);

        _target = new List<Node>();

        //Initial position set to a random node
        if (pNodeGraph.nodes.Count > 0)
        {
            currentNode = pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)];
            jumpToNode(currentNode);
        }

        pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
    }

    void AgentSettingsSet(AgentGrade agentGrade)
    {
        if(agentGrade == AgentGrade.Sufficient_OnGraphWayPoint)
        {
            REGULAR_SPEED = 0.2f;
            FAST_TRAVEL_SPEED = 300 * REGULAR_SPEED;
        }
        else if(agentGrade == AgentGrade.Good_RandomWayPoint)
        {
            REGULAR_SPEED = 15f;
            FAST_TRAVEL_SPEED = 20 * REGULAR_SPEED;
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(Key.M) || Input.GetKeyDown(Key.R))
        {
            if (_nodeGraph.nodes.Count > 0)
            {
                currentNode = _nodeGraph.nodes[Utils.Random(0, _nodeGraph.nodes.Count)];
                jumpToNode(currentNode);
            }
        }
        SwitchAgentState();
        WalkToQueuedNodes();
    }
    void SufficientOnClickEvent(Node pNode)
    {
        if (_target.Count == 0)
        {
            if (currentNode.connections.Contains(pNode))
                _target.Add(pNode);
        }
        else if (_target[_target.Count - 1].connections.Contains(pNode))
            _target.Add(pNode);
    }

    void GoodOnClickEvent(Node pNode)
    {
        // Ensures Murc can only click on a node when he is not moving
        if (_target.Count == 0)
        {
            if (currentNode.connections.Contains(pNode)) //Ensures Murc goes to the node if it can do some directly from its current position
            {
                _target.Add(pNode);
            }
            else
            {
                WanderRandomly(pNode);
            }
        }
    }

    void WanderRandomly(Node pNode)
    {
        Node previousNode = null; 
        Node nodeToAdd;
        while (!currentNode.Equals(pNode))
        {
            nodeToAdd = currentNode.connections[Utils.Random(0,currentNode.connections.Count)];
            while(currentNode.connections.Count > 1 && nodeToAdd == previousNode) //Ensures that Murc only goes to the previous node if it's the ONLY option
            {
                nodeToAdd = currentNode.connections[Utils.Random(0, currentNode.connections.Count)];
            }
            _target.Add(nodeToAdd);
            previousNode = currentNode;
            currentNode = nodeToAdd;
        }
    }

    protected virtual void SwitchAgentState()
    {
        if (Input.GetKeyDown(Key.V))
        {
            _agentGrade += 1;

            if ((int)_agentGrade > 1)
                _agentGrade = 0;

            AgentSettingsSet(_agentGrade);

            Console.WriteLine($"The state of the Agent is {_agentGrade}");
        }
    }


    protected void WalkToQueuedNodes()
    {
        if (_target.Count == 0) //If no target is found, Morc won't move
            return;
        else //Move towards the target node, if we reached it, clear the target
        {
            if (currentTargetNumber < _target.Count)
            {
                if (moveTowardsNode(_target[currentTargetNumber]))
                {
                    currentNode = _target[currentTargetNumber];
                    ++currentTargetNumber;
                }
                if (x == _target[_target.Count - 1].location.X && y == _target[_target.Count - 1].location.Y) // If Murc is on the last node, reset the whole queue
                {
                    currentTargetNumber = 0;
                    _target.Clear();
                }
            }
        }
    }

    protected virtual void onNodeClickHandler(Node pNode)
    {
        switch (_agentGrade)
        {
            case AgentGrade.Sufficient_OnGraphWayPoint:
                SufficientOnClickEvent(pNode);
                break;
            case AgentGrade.Good_RandomWayPoint:
                GoodOnClickEvent(pNode);
                break;

        }
    }
}