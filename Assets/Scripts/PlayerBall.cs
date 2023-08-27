using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody playerBallRigid;
    private Camera mainCamera;
    private Vector3 playerCameraOffset;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerCameraOffset = playerCamera.transform.position - playerBallRigid.transform.position;
    }

    private void Update()
    {
        playerCamera.transform.position = playerBallRigid.transform.position + playerCameraOffset;
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
        Vector3 velocity = CalculateVelocity(playerBallRigid.transform.position, targetPosition, angle);

        // 射出
        playerBallRigid.AddForce(velocity * playerBallRigid.mass, ForceMode.Impulse);

        playerBallRigid.transform.rotation = Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z));
        if(playerCamera.gameObject.activeSelf)
        {
            playerCamera.transform.RotateAround(playerBallRigid.transform.position, Vector3.up, Vector3.Angle(new Vector3(velocity.x, 0, velocity.z), transform.forward));
            playerCameraOffset = playerCamera.transform.position - playerBallRigid.transform.position;
        }
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
}
