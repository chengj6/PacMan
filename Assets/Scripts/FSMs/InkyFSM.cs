using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkyFSM : MonoBehaviour
{
    GhostAI ai;
    GameObject gate;
    GameObject pacMan;
    Movement pacManMove;
    GameObject blinky;
    Movement blinkyMove;
    GameObject inkyTarget;

    // Start is called before the first frame update
    void Start()
    {
        ai = gameObject.GetComponent<GhostAI>();
        gate = GameObject.Find("Gate(Clone)");
        pacMan = GameObject.Find("PacMan(Clone)") ? GameObject.Find("PacMan(Clone)") : GameObject.Find("PacMan 1(Clone)");
        pacManMove = pacMan.GetComponent<Movement>();
        blinky = GameObject.Find("Blinky(Clone)");
        blinkyMove = blinky.GetComponent<Movement>();
        inkyTarget = new GameObject("Inky Target");
    }

    // Update is called once per frame
    void Update()
    {
        switch (ai._state)
        {
            case GhostAI.State.active:

                Vector3 inkyTargetTemp = 4 * ai.move2vec(pacManMove._dir);
                inkyTarget.transform.position = pacMan.transform.position + new Vector3(inkyTargetTemp.x, inkyTargetTemp.y, 0);
                Vector3 normalVector = (inkyTarget.transform.position - blinky.transform.position).normalized;
                inkyTarget.transform.position += (Vector3.Distance(inkyTarget.transform.position, blinky.transform.position) / 4) * normalVector;

                float x = inkyTarget.transform.position.x;
                float y = inkyTarget.transform.position.y;
                if (inkyTarget.transform.position.x < 1) {
                    x = 1;
                } else if (inkyTarget.transform.position.x > 26) {
                    x = 26;
                }

                if (inkyTarget.transform.position.y < -29) {
                    y = -29;
                } else if (inkyTarget.transform.position.y > -1) {
                    y = -1;
                }

                inkyTarget.transform.position = new Vector3(x, y, inkyTarget.transform.position.z);
                ai.target = inkyTarget;
                
                if (ai.dead) {
                    ai._state = GhostAI.State.entering;
                    ai.fleeing = false;
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
