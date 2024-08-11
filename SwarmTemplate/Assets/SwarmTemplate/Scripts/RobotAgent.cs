using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class RobotAgent : Agent
{
    public GameManager gameManager;
    public Rigidbody rBody;
    public List<GameObject> ledFront;
    public List<GameObject> ledBack;

    public AxleInfo axleInfo;

    private float preDistanceToTarget = 100000000f;

    public float maxMotorTorque;

    public override void Initialize()
    {
        this.rBody = GetComponent<Rigidbody>();
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {   

        float rightMotor = maxMotorTorque * actionBuffers.ContinuousActions[0];
        float leftMotor = maxMotorTorque * actionBuffers.ContinuousActions[1];
        float ledFrontInfo = actionBuffers.ContinuousActions[2];
        float ledBackInfo = actionBuffers.ContinuousActions[3];

        float rightTorqueNow = axleInfo.rightWheel.motorTorque;
        if (rightTorqueNow * rightMotor < 0 || rightMotor == 0) {
			axleInfo.rightWheel.motorTorque = 0;
			axleInfo.rightWheel.brakeTorque = Mathf.Abs (rightTorqueNow-rightMotor);
		} else {
			if (Mathf.Abs (axleInfo.rightWheel.rpm) < 109.0f) {
				axleInfo.rightWheel.motorTorque = Mathf.Lerp(rightTorqueNow, rightMotor, Time.deltaTime);
			} else {
				axleInfo.rightWheel.motorTorque = 0;
			}
			axleInfo.rightWheel.brakeTorque = 0;
		}
        
        float leftTorqueNow = axleInfo.leftWheel.motorTorque;
		if (leftTorqueNow * leftMotor < 0 || leftMotor == 0) {
			axleInfo.leftWheel.motorTorque = 0;
			axleInfo.leftWheel.brakeTorque = Mathf.Abs (leftTorqueNow-leftMotor);
		} else {
			if (Mathf.Abs (axleInfo.leftWheel.rpm) < 109.0f) {
				axleInfo.leftWheel.motorTorque = Mathf.Lerp (leftTorqueNow, leftMotor, Time.deltaTime);
			} else {
				axleInfo.leftWheel.motorTorque = 0;
			}
			axleInfo.leftWheel.brakeTorque = 0;
		}
        
        if (ledFrontInfo >= 0) {
            ledFront[0].GetComponent<Renderer>().material.color = Color.blue;
            ledFront[1].GetComponent<Renderer>().material.color = Color.blue;
            ledFront[0].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            ledFront[0].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.0f, 0.0f, 5.0f));
            ledFront[1].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            ledFront[1].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.0f, 0.0f, 5.0f));
        }
        else {
            ledFront[0].GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0);
            ledFront[1].GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0);
            ledFront[0].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            ledFront[0].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f));
            ledFront[1].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            ledFront[1].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f));
        }

        if (ledBackInfo >= 0) {
            ledBack[0].GetComponent<Renderer>().material.color = Color.red;
            ledBack[1].GetComponent<Renderer>().material.color = Color.red;
            ledBack[0].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            ledBack[0].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(5.0f, 0.0f, 0.0f));
            ledBack[1].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            ledBack[1].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(5.0f, 0.0f, 0.0f));
        }
        else {
            ledBack[0].GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0);
            ledBack[1].GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0);
            ledBack[0].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            ledBack[0].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f));
            ledBack[1].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            ledBack[1].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f));
        }
        
        float distanceToTarget = 100000000f;
        for (int i = 0; i < gameManager.targets.Count; i++){
            float tempDistanceToTarget = Vector3.Distance(
                this.transform.position, gameManager.targets[i].position);
            distanceToTarget = Mathf.Min(distanceToTarget, tempDistanceToTarget);
        }

        // AddReward((preDistanceToTarget - distanceToTarget) * 5.0f);

        // if (distanceToTarget < preDistanceToTarget){
        //     AddReward(1.0f);
        // }

        preDistanceToTarget = distanceToTarget;

        if (distanceToTarget < 1.42f)
        {
            // Debug.Log("Enter End");
            AddReward(1.0f);
            gameManager.EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionBuffers)
    {
        var actionsOut = actionBuffers.ContinuousActions;
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
    }

}


[System.Serializable]
public class AxleInfo {
	public WheelCollider rightWheel;
	public WheelCollider leftWheel;

}