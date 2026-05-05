using UnityEngine;

public class EyeRotator : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void Start()
    {

    }

    private float Angle(Vector3 v, Vector3 w)
    {
        return Mathf.Acos(Vector3.Dot(v, w) / (v.magnitude * w.magnitude));
    }

    private void Update()
    {
        var dir = target.position - transform.position;
        Debug.DrawRay(transform.position, dir);

        var dirX = dir;
        dirX.y = 0f;
        //Debug.DrawRay(transform.position, dirX, Color.red);

        var dirY = dir;
        dirY.x = 0f;
        //Debug.DrawRay(transform.position, dirY, Color.green);

        var lookDirX = transform.forward;
        lookDirX.y = 0f;
        //Debug.DrawRay(transform.position, lookDirX.normalized, Color.blue);

        var lookdirY = transform.forward;
        lookdirY.x = 0f;
        //Debug.DrawRay(transform.position, lookdirY.normalized, Color.white);

        var rotDirX = Vector3.Cross(lookDirX, dirX);
        var rotDirY = Vector3.Cross(lookdirY, dirY);

        var angleX = Angle(lookDirX, dirX);
        var AngleY = Angle(lookdirY, dirY);

        transform.Rotate(Vector3.up, angleX * Mathf.Sign(rotDirX.y) * Mathf.Rad2Deg * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, AngleY * Mathf.Sign(rotDirY.x) * Mathf.Rad2Deg * Time.deltaTime);
    }
}
