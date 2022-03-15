using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float moveSpeed;
    public Transform targetPoint;

    private Vector3 targetPosition;
    private float nextActionTime = -1f;

    private void FixedUpdate()
    {
        if (this.moveSpeed > 0) {
            this.Swim();
        }
    }

    private void Swim() {
        if (Time.fixedTime >= nextActionTime)
        {

            //랜덤 위치 구하기 
            this.targetPosition = this.ChooseRandomPosition(100f, 260f, 2f, 13f);

            //Debug.Log(this.targetPosition);

            this.targetPoint.position = this.targetPosition;

            //해당 방향을 바라보게 
            this.transform.rotation = Quaternion.LookRotation(this.targetPosition - this.transform.position, Vector3.up);

            //다음 시간 설정 
            //      거 
            //    ------
            //    속 | 시 

            this.nextActionTime = Time.fixedTime + Vector3.Distance(this.targetPosition, this.transform.position) / this.moveSpeed;
        }
        else
        {
            Vector3 movement = this.moveSpeed * this.transform.forward * Time.fixedDeltaTime;

            if (movement.magnitude <= Vector3.Distance(this.targetPosition, this.transform.position))
            {
                this.transform.position += movement;
            }
            else 
            {
                this.transform.position = movement;
                nextActionTime = Time.fixedTime;
            }
        }
    }

    public void Init() {
        this.targetPosition = this.ChooseRandomPosition(100f, 260f, 2f, 13f);
        this.nextActionTime = -1f;
    }

    private Vector3 ChooseRandomPosition(float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        var angle = Random.Range(minAngle, maxAngle);
        var radius = Random.Range(minRadius, maxRadius);

        return this.transform.parent.position + (Quaternion.Euler(0, angle, 0) * Vector3.forward * radius);
    }

    private void OnDrawGizmos()
    {
        GizmosExtensions.DrawWireArc(this.transform.parent.position, -Vector3.forward, 260, 13);
    }
}
