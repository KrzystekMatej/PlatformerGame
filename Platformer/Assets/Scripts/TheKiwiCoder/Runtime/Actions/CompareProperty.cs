using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace TheKiwiCoder {

    [System.Serializable]
    public class CompareProperty : ActionNode
    {
        public BlackboardKeyValuePair pair;

        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override ProcessState OnUpdate() {
            BlackboardKey source = pair.value;
            BlackboardKey destination = pair.key;

            if (source != null && destination != null) {
                if (destination.Equals(source)) {
                    return ProcessState.Success;
                }
            }

            return ProcessState.Failure;
        }
    }
}
