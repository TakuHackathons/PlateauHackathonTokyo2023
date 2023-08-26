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

    [SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody playerBallRigid;
    private Camera mainCamera;

    private Vector3 initialPosition;
    private Vector3 initialVelocityVector;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void followPlayerCamera()
    {
        mainCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }

    public void returnMainCamera()
    {
        mainCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
    }

    /// <summary>
    /// ボールを射出する
    /// </summary>
    /// <param name="targetPosition">標的の座標</param>
    /// <param name="射出角度">angle</param>
    public void ThrowBall(Vector3 targetPosition,float angle = 45)
    {
        // 射出速度を算出
        Vector3 velocity = CalculateVelocity(this.transform.position, targetPosition, angle);

        // 射出
        playerBallRigid.AddForce(velocity * playerBallRigid.mass, ForceMode.Impulse);
    }

    /// 標的に命中する射出速度の計算
    /// </summary>
    /// <param name="pointA">射出開始座標</param>
    /// <param name="pointB">標的の座標</param>
    /// <returns>射出速度</returns>
    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // 射出角をラジアンに変換
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
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
