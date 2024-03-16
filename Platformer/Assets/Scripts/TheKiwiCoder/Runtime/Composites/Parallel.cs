using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TheKiwiCoder {
    [System.Serializable]
    public class Parallel : CompositeNode {
        List<ProcessState> childrenLeftToExecute = new List<ProcessState>();

        protected override void OnStart() {
            childrenLeftToExecute.Clear();
            children.ForEach(a => {
                childrenLeftToExecute.Add(ProcessState.Running);
            });
        }

        protected override void OnStop() {
        }

        protected override ProcessState OnUpdate() {
            bool stillRunning = false;
            for (int i = 0; i < childrenLeftToExecute.Count; ++i) {
                if (childrenLeftToExecute[i] == ProcessState.Running) {
                    var status = children[i].Update();
                    if (status == ProcessState.Failure) {
                        AbortRunningChildren();
                        return ProcessState.Failure;
                    }

                    if (status == ProcessState.Running) {
                        stillRunning = true;
                    }

                    childrenLeftToExecute[i] = status;
                }
            }

            return stillRunning ? ProcessState.Running : ProcessState.Success;
        }

        void AbortRunningChildren() {
            for (int i = 0; i < childrenLeftToExecute.Count; ++i) {
                if (childrenLeftToExecute[i] == ProcessState.Running) {
                    children[i].Abort();
                }
            }
        }
    }
}