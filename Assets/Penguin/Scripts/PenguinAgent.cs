using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PenguinAgent : Agent
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;
    private Rigidbody rBody;
    public Fish target;

    public override void Initialize()
    {
        this.rBody = this.GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = Vector3.zero;
        target.Init();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //actionBuffer로 부터 전달 받은 값을 가져온다 
        //이동 방향 
        Vector3 moveDir = this.transform.forward * actions.DiscreteActions[0];  //0, 1

        //회전 방향 
        Vector3 turnDir = Vector3.up * (actions.DiscreteActions[1] - 1);   //0, 1, 2 -> -1, 0, 1

        //이동
        Vector3 movement = moveDir * this.moveSpeed * Time.fixedDeltaTime;
        this.rBody.MovePosition(this.transform.position + movement);

        //회전 
        Vector3 rotation = turnDir * this.turnSpeed * Time.fixedDeltaTime;
        this.transform.Rotate(rotation);

        //보상을 감소 시킨다 
        AddReward(-1 / this.MaxStep);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        int turnAction = 1;

        if (Input.GetKey(KeyCode.W)) {
            forwardAction = 1;
        }

        if (Input.GetKey(KeyCode.A)) {
            turnAction = 0;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnAction = 2;
        }

        var discreteActionOut = actionsOut.DiscreteActions;
        discreteActionOut[0] = forwardAction;
        discreteActionOut[1] = turnAction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Fish"))
        {
            AddReward(1.0f);
            EndEpisode();
        }
    }
}
