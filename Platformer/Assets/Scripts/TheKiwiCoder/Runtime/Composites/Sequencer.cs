using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    [System.Serializable]
    public class Sequencer : CompositeNode {
        protected int current;

        protected override void OnStart() {
            current = 0;
        }

        protected override void OnStop() {
        }

        protected override ProcessState OnUpdate() {
            for (int i = current; i < children.Count; i++) {
                current = i;
                var child = children[current];

                switch (child.Update()) {
                    case ProcessState.Running:
                        return ProcessState.Running;
                    case ProcessState.Failure:
                        return ProcessState.Failure;
                    case ProcessState.Success:
                        continue;
                }
            }

            return ProcessState.Success;
        }
    }
}