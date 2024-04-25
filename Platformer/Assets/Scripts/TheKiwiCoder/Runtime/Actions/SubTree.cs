using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System;

namespace TheKiwiCoder
{

    [System.Serializable]
    public class SubTree : ActionNode
    {

        [Tooltip("Behaviour tree asset to run as a subtree")] public BehaviourTree treeAsset;
        [HideInInspector] public BehaviourTree treeInstance;

        public override void OnInit()
        {
            if (treeAsset)
            {
                treeInstance = treeAsset.Clone();
                treeInstance.blackboard.OnInit();
                context.Steering.WritePipelinesToBlackboard(treeInstance.blackboard);
                treeInstance.Bind(context);
                drawGizmos = true;
            }
        }

        protected override void OnStart()
        {
            if (treeInstance)
            {
                treeInstance.treeState = ProcessState.Running;
            }
        }

        protected override void OnStop()
        {
        }

        protected override ProcessState OnUpdate()
        {
            if (treeInstance)
            {
                return treeInstance.Update();
            }
            return ProcessState.Failure;
        }

        public override void OnDrawGizmos(AgentManager agent)
        {
            if (Application.isPlaying && treeInstance)
            {
                treeInstance.OnDrawTreeGizmos(agent);
            }
            else if (!Application.isPlaying && treeAsset)
            {
                treeAsset.OnDrawTreeGizmos(agent);
            }
        }
    }
}