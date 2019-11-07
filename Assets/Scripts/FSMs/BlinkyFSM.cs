using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkyFSM : MonoBehaviour
{
    GhostAI ai;
    GameObject gate;
    GameObject pacMan;

    // Start is called before the first frame update
    void Start()
    {
        ai = gameObject.GetComponent<GhostAI>();
        gate = GameObject.Find("Gate(Clone)");
        pacMan = GameObject.Find("PacMan(Clone)") ? GameObject.Find("PacMan(Clone)") : GameObject.Find("PacMan 1(Clone)");
    }

    // Code FSM transition for Blinky here
    void Update()
    {
        switch(ai._state)
        {
            case GhostAI.State.active:
                ai.target = pacMan;
                if (ai.dead) {
                    ai._state = GhostAI.State.entering;
                } else if (ai.fleeing) {
                    ai._state = GhostAI.State.fleeing;
                }
                break;

            case GhostAI.State.fleeing:
                if (ai.dead) {
                    ai._state = GhostAI.State.entering;
                    ai.fleeing = false;
                } else if (!ai.fleeing) {
                    ai._state = GhostAI.State.active;
                }
                break;

            case GhostAI.State.leaving:
                if (gate.transform.position.y + 0.5f < gameObject.transform.position.y) {
                    ai._state = GhostAI.State.active;
                }
                break;
        }
    }
}
