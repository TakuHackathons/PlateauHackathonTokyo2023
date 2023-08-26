using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    public float initialVelocity = 10f;  // 初速度
    public float angle = 45f;            // 射角

    private Vector3 initialPosition;
    private Vector3 initialVelocityVector;

    private void Start()
    {
        initialPosition = transform.position;
        CalculateInitialVelocityVector();
        Launch();
    }

    private void CalculateInitialVelocityVector()
    {
        float horizontalVelocity = initialVelocity * Mathf.Cos(angle * Mathf.Deg2Rad);
        float verticalVelocity = initialVelocity * Mathf.Sin(angle * Mathf.Deg2Rad);

        initialVelocityVector = new Vector3(horizontalVelocity, verticalVelocity, 0f);
    }

    public void Launch()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = initialVelocityVector;

        StartCoroutine(UpdatePosition(rb));
    }

    private IEnumerator UpdatePosition(Rigidbody rb)
    {
        while (true)
        {
            Vector3 displacement = rb.velocity * Time.deltaTime;
            transform.position += displacement;

            yield return null;
        }
    }
}
