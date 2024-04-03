using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TheKiwiCoder {

    // The context is a shared object every node has access to.
    // Commonly used components and subsytems should be stored here
    // It will be somewhat specfic to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context
    {
        // Add other game specific systems here
        public AIInputController InputController { get; private set; }
        public AgentManager Agent { get; private set; }
        public MovementSteering Steering { get; private set; }

        public static Context CreateFromGameObject(GameObject gameObject)
        {
            // Fetch all commonly used components
            Context context = new Context();
            // Add whatever else you need here...
            AIManager manager = gameObject.GetComponentInParent<AIManager>();
            context.InputController = (AIInputController)manager.Agent.InputController;
            context.Agent = manager.Agent;
            context.Steering = manager.Steering;

            return context;
        }
    }
}