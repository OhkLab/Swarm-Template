using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class GameManager : MonoBehaviour
{
    public List<RobotAgent> agents;
    public List<Transform> targets;
    private List<Vector3> firstLocation;
    
    void Start()
    {
        firstLocation = new List<Vector3>();
        for (int i = 0; i < agents.Count; i++) {
            firstLocation.Add(agents[i].transform.position);
        }

        Reset();
    }

    public void Reset()
    {
        for (int i = 0; i < agents.Count; i++) {
            agents[i].transform.position = firstLocation[i];
            agents[i].axleInfo.rightWheel.motorTorque = 0;
            agents[i].axleInfo.leftWheel.motorTorque = 0;
            agents[i].rBody.angularVelocity = Vector3.zero;
            agents[i].rBody.velocity = Vector3.zero;
        }

        for (int i = 0; i < targets.Count; i++){
            targets[i].localPosition = new Vector3(Random.value*8-4, 0.5f, Random.value*8-4);
        }
    }

    public void EndEpisode()
    {
        for (int i = 0; i < agents.Count; i++) {
            agents[i].EndEpisode();
        }

        Reset();
        // Debug.Log("Reset");
    }
}
