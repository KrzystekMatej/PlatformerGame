using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    [System.Serializable]
    public abstract class Node {
        public enum NodeState {
            Running,
            Failure,
            Success
        }

        [HideInInspector] public NodeState state = NodeState.Running;
        [HideInInspector] public bool started = false;
        [HideInInspector] public string guid = System.Guid.NewGuid().ToString();
        [HideInInspector] public Vector2 position;
        [HideInInspector] public Context context;
        [HideInInspector] public Blackboard blackboard;
        [TextArea] public string description;
        [Tooltip("When enabled, the nodes OnDrawGizmos will be invoked")] public bool drawGizmos = false;
#if UNITY_EDITOR
        public bool log = false;
#endif

        public virtual void OnInit() {
            // Nothing to do here
        }

        public NodeState Update() {

            if (!started) {
                OnStart();
                started = true;

#if UNITY_EDITOR
                if (log) Log("Started");
#endif
            }

            state = OnUpdate();

            if (state != NodeState.Running) {
                OnStop();
                started = false;

#if UNITY_EDITOR
                if (log) Log("Ended");
#endif
            }

            return state;
        }

        public void Abort() {
            BehaviourTree.Traverse(this, (node) => {
                node.started = false;
                node.state = NodeState.Running;
                node.OnStop();
            });
        }

        public virtual void OnDrawGizmos() { }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract NodeState OnUpdate();

        protected virtual void Log(string message) {
            Debug.Log($"[{GetType()}]{message}");
        }
    }
}