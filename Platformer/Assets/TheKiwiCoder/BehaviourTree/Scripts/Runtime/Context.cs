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
        public AIInputController InputController { get; private set; }
        public Agent Agent { get; private set; }
        public Vision Vision { get; private set; }
        public Steering Steering { get; private set; }
        public ISteeringBehaviour RootBehaviour { get; private set; }

        public static Context CreateFromGameObject(GameObject gameObject) {
            // Fetch all commonly used components
            Context context = new Context();
            // Add whatever else you need here...
            AIManager manager = gameObject.GetComponentInParent<AIManager>();
            context.InputController = manager.AIInputController;
            context.Agent = manager.Agent;
            context.Vision = manager.Vision;
            context.Steering = manager.Steering;
            context.RootBehaviour = manager.RootBehaviour;

            return context;
        }
    }
}