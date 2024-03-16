using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    [System.Serializable]
    public class Inverter : DecoratorNode {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override ProcessState OnUpdate() {
            if (child == null) {
                return ProcessState.Failure;
            }

            switch (child.Update()) {
                case ProcessState.Running:
                    return ProcessState.Running;
                case ProcessState.Failure:
                    return ProcessState.Success;
                case ProcessState.Success:
                    return ProcessState.Failure;
            }
            return ProcessState.Failure;
        }
    }
}