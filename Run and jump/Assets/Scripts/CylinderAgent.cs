using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class CylinderAgent : Agent
{
    Rigidbody rBody;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform target;
    public override void OnEpisodeBegin()
    {
        //Reseting Agent
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(-9, 0.5f, 0);

        //Move target to a new spot
        target.localPosition = new Vector3(Random.value * 20 - 10, Random.value * 5, Random.value * 10 - 5);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent Positions & Agent Velocity 
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(rBody.velocity);
    }

    public float speed = 20;
    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 ControlSignal = Vector3.zero;
        ControlSignal.y = vectorAction[2];

        if (vectorAction[0] == 2)
        {
            ControlSignal.x = 1;
        }
        else
        {
            ControlSignal.x = -vectorAction[0];
        }

        if (vectorAction[1]==2)
        {
            ControlSignal.z = 1;
        }
        else
        {
            ControlSignal.z = -vectorAction[1];
        }

       if (this.transform.localPosition.x < 9)
        {
            rBody.AddForce(ControlSignal * speed);
        }

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);

        if(distanceToTarget< 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        if(this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
        actionsOut[2] = System.Convert.ToInt32(Input.GetKey(KeyCode.Space));
    }
}
