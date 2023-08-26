using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonBehaviour<GameController>
{
    [SerializeField] private PlayerBall playerBall;
    [SerializeField, Range(0F, 90F), Tooltip("射出する角度")] private float throwAngle;
    [SerializeField] private GameObject targetMarkerObj;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        playerBall.returnMainCamera();
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitpoint = hit.point;
            targetMarkerObj.SetActive(true);
            targetMarkerObj.transform.position = new Vector3(hitpoint.x, hitpoint.y + 0.1f, hitpoint.z);
            if (Input.GetMouseButtonDown(0))
            {
                playerBall.followPlayerCamera();
                playerBall.ThrowBall(hitpoint, throwAngle);
            }
        }
        else
        {
            targetMarkerObj.gameObject.SetActive(false);
        }
    }
}
