using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    [System.Serializable]
    public class Succeed : DecoratorNode {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override ProcessState OnUpdate() {
            if (child == null) {
                return ProcessState.Failure;
            }

            var state = child.Update();
            if (state == ProcessState.Failure) {
                return ProcessState.Success;
            }
            return state;
        }
    }
}