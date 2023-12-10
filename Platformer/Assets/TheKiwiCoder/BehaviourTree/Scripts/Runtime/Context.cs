using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TheKiwiCoder {

    // The context is a shared object every node has access to.
    // Commonly used components and subsystems should be stored here
    // It will be somewhat specific to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context {
        // Add other game specific systems here
        public Agent Agent;
        public AIInputController InputController;
        public CastDetector RayCastDetector;
        public AIManager AIManager;

        public static Context CreateFromGameObject(GameObject gameObject) {
            // Fetch all commonly used components
            Context context = new Context();
            // Add whatever else you need here...
            context.Agent = gameObject.GetComponentInChildren<Agent>();
            context.InputController = gameObject.GetComponentInChildren<AIInputController>();
            context.AIManager = gameObject.GetComponentInChildren<AIManager>();

            return context;
        }
    }
}