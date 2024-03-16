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

        protected override ProcessState OnUpdate() {
            if (child == null) {
                return ProcessState.Failure;
            }

            switch (child.Update()) {
                case ProcessState.Running:
                    break;
                case ProcessState.Failure:
                    if (restartOnFailure) {
                        iterationCount++;
                        if (iterationCount >= maxRepeats && maxRepeats > 0) {
                            return ProcessState.Failure;
                        } else {
                            return ProcessState.Running;
                        }
                    } else {
                        return ProcessState.Failure;
                    }
                case ProcessState.Success:
                    if (restartOnSuccess) {
                        iterationCount++;
                        if (iterationCount >= maxRepeats && maxRepeats > 0) {
                            return ProcessState.Success;
                        } else {
                            return ProcessState.Running;
                        }
                    } else {
                        return ProcessState.Success;
                    }
            }
            return ProcessState.Running;
        }
    }


}
