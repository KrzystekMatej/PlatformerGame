using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    [System.Serializable]
    public class Failure : DecoratorNode {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override NodeState OnUpdate() {
            if (child == null) {
                return NodeState.Failure;
            }

            var state = child.Update();
            if (state == NodeState.Success) {
                return NodeState.Failure;
            }
            return state;
        }
    }
}