using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    [System.Serializable]
    public class Repeat : DecoratorNode {

        [Tooltip("Restarts the subtree on success")] public bool restartOnSuccess = true;
        [Tooltip("Restarts the subtree on failure")] public bool restartOnFailure = false;
        [Tooltip("Maximum number of times the subtree will be repeated. Set to 0 to loop forever")] public int maxRepeats = 0;

        int iterationCount = 0;

        protected override void OnStart() {
            iterationCount = 0;
        }

        protected override void OnStop() {

        }

        protected override NodeState OnUpdate() {
            if (child == null) {
                return NodeState.Failure;
            }

            switch (child.Update()) {
                case NodeState.Running:
                    break;
                case NodeState.Failure:
                    if (restartOnFailure) {
                        iterationCount++;
                        if (iterationCount >= maxRepeats && maxRepeats > 0) {
                            return NodeState.Failure;
                        } else {
                            return NodeState.Running;
                        }
                    } else {
                        return NodeState.Failure;
                    }
                case NodeState.Success:
                    if (restartOnSuccess) {
                        iterationCount++;
                        if (iterationCount >= maxRepeats && maxRepeats > 0) {
                            return NodeState.Success;
                        } else {
                            return NodeState.Running;
                        }
                    } else {
                        return NodeState.Success;
                    }
            }
            return NodeState.Running;
        }
    }


}
