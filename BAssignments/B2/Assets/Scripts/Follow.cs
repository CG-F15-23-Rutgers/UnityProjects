﻿using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class Follow : MonoBehaviour
{
    public Transform target;
    public GameObject participant;

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
    {
        return new DecoratorLoop(
                new Sequence(
                    this.ST_ApproachAndWait(this.target)));
    }
}
