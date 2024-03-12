using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System;

namespace TheKiwiCoder {

    [System.Serializable]
    public class SubTree : ActionNode {
        
        [Tooltip("Behaviour tree asset to run as a subtree")] public BehaviourTree treeAsset;
        [HideInInspector] public BehaviourTree treeInstance;

        public override void OnInit() {
            if (treeAsset) {
                treeInstance = treeAsset.Clone();
                treeInstance.Bind(context);
            }
        }

        protected override void OnStart() {
            if (treeInstance) {
                treeInstance.treeState = Node.NodeState.Running;
            }
        }

        protected override void OnStop() {
        }

        protected override NodeState OnUpdate() {
            if (treeInstance) {
                return treeInstance.Update();
            }
            return NodeState.Failure;
        }
    }
}
