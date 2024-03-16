using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    [System.Serializable]
    public class Selector : CompositeNode {
        protected int current;

        protected override void OnStart() {
            current = 0;
        }

        protected override void OnStop() {
        }

        protected override ProcessState OnUpdate() {
            for (int i = current; i < children.Count; ++i) {
                current = i;
                var child = children[current];

                switch (child.Update()) {
                    case ProcessState.Running:
                        return ProcessState.Running;
                    case ProcessState.Success:
                        return ProcessState.Success;
                    case ProcessState.Failure:
                        continue;
                }
            }

            return ProcessState.Failure;
        }
    }
}