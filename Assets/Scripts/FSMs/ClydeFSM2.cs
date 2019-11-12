﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeFSM2 : MonoBehaviour
{
    GhostAI ai;
    GameObject gate;
    GameObject pacMan;
    GameObject scatterTarget;
    List<GameObject> powerPellets;

    // Start is called before the first frame update
    void Start()
    {
        ai = gameObject.GetComponent<GhostAI>();
        gate = GameObject.Find("Gate(Clone)");
        pacMan = GameObject.Find("PacMan(Clone)") ? GameObject.Find("PacMan(Clone)") : GameObject.Find("PacMan 1(Clone)");
        scatterTarget = new GameObject("Clyde Target");
        scatterTarget.transform.position = new Vector3(3, -29, 2);
        powerPellets = GameObject.Find("GameManager").GetComponent<MazeCreator>().powerPellets;
    }

    // Update is called once per frame
    void Update()
    {
        switch (ai._state)
        {
            case GhostAI.State.active:
                ai.target = pacMan;

                if (ai.dead)
                {
                    ai._state = GhostAI.State.entering;
                }
                else if (ai.fleeing)
                {
                    ai._state = GhostAI.State.fleeing;
                }
                else
                {
                    for (int i = 0; i < powerPellets.Count; i++)
                    {
                        if (Vector3.Distance(pacMan.transform.position, powerPellets[i].transform.position) < 5f)
                        {
                            ai._state = GhostAI.State.scatter;
                            break;
                        }
                    }
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
                if (Vector3.Distance(ai.target.transform.position, transform.position) <= 4.5f)
                {
                    ai._state = GhostAI.State.active;
                }

                if (ai.dead)
                {
                    ai._state = GhostAI.State.entering;
                    ai.fleeing = false;
                }
                else if (ai.fleeing)
                {
                    ai._state = GhostAI.State.fleeing;
                }
                break;
        }
    }
}
