using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkyFSM : MonoBehaviour
{
    GhostAI ai;
    GameObject gate;
    GameObject pacMan;
    GameObject pinky;
    Movement pacManMove;
    Movement pinkyMove;
    GameObject inkyTarget;
    Sprite dummy;
    LineRenderer line;
    GameObject scatterTarget;

    // Start is called before the first frame update
    void Start()
    {
        ai = gameObject.GetComponent<GhostAI>();
        gate = GameObject.Find("Gate(Clone)");
        pacMan = GameObject.Find("PacMan(Clone)") ? GameObject.Find("PacMan(Clone)") : GameObject.Find("PacMan 1(Clone)");
        pinky = GameObject.Find("Pinky(Clone)") ? GameObject.Find("Pinky(Clone)") : GameObject.Find("Pinky 1(Clone)");
        pacManMove = pacMan.GetComponent<Movement>();
        pinkyMove = pinky.GetComponent<Movement>();
        inkyTarget = new GameObject("inky Target");
        line = gameObject.GetComponent<LineRenderer>();
        scatterTarget = new GameObject("Inky Target");
        scatterTarget.transform.position = new Vector3(26, -1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        switch (ai._state)
        {
            case GhostAI.State.active:
                //Vector2 inkyTargetTemp = ai.move2vec(pinkyMove._dir);
                float dist = Vector3.Distance(transform.position, pinky.transform.position);
                if (dist <= 7)
                {
                    ai._state = GhostAI.State.scatter;
                    ai.target = scatterTarget;
                    break;
                }
                inkyTarget.transform.position = pinky.transform.position;
                ai.target = inkyTarget;
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
            case GhostAI.State.scatter:
                ai.target = scatterTarget;
                if (Vector3.Distance(transform.position, pinky.transform.position) >= 12f || 
                    Vector3.Distance(ai.target.transform.position, transform.position) <= 4.5f)
                {
                    ai._state = GhostAI.State.active;
                }

                if (ai.dead)
                {
                    ai._state = GhostAI.State.entering;
                }
                else if (ai.fleeing)
                {
                    ai._state = GhostAI.State.fleeing;
                }
                break;
        }
    }
}
