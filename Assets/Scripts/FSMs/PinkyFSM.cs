using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkyFSM : MonoBehaviour
{
    GhostAI ai;
    GameObject gate;
    GameObject pacMan;
    Movement pacManMove;
    GameObject pinkyTarget;
    Sprite dummy;
    LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        ai = gameObject.GetComponent<GhostAI>();
        gate = GameObject.Find("Gate(Clone)");
        pacMan = GameObject.Find("PacMan(Clone)") ? GameObject.Find("PacMan(Clone)") : GameObject.Find("PacMan 1(Clone)");
        pacManMove = pacMan.GetComponent<Movement>();
        pinkyTarget = new GameObject("Pinky Target");
        line = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (ai._state)
        {
            case GhostAI.State.active:
                Vector2 pinkyTargetTemp = ai.move2vec(pacManMove._dir);
                print(pinkyTarget.transform.position);
                pinkyTarget.transform.position = pacMan.transform.position + new Vector3(pinkyTargetTemp.x, pinkyTargetTemp.y, 0);
                ai.target = pinkyTarget;
                if (ai.dead)
                {
                    ai._state = GhostAI.State.entering;
                }
                else if (ai.fleeing)
                {
                    ai._state = GhostAI.State.fleeing;
                }
                break;

            case GhostAI.State.fleeing:
                if (ai.dead)
                {
                    ai._state = GhostAI.State.entering;
                    ai.fleeing = false;
                }
                else if (!ai.fleeing)
                {
                    ai._state = GhostAI.State.active;
                }
                break;

            case GhostAI.State.leaving:
                if (gate.transform.position.y + 0.5f < gameObject.transform.position.y)
                {
                    ai._state = GhostAI.State.active;
                }
                break;
        }
    }
}
