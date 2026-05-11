using UnityEngine;

public class EyeRotator : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private float Angle(Vector3 v, Vector3 w)
    {
        return Mathf.Acos(Vector3.Dot(v, w) / (v.magnitude * w.magnitude)) * Mathf.Rad2Deg;
    }

    private void Update()
    {
        var dir = target.position - transform.position;
        Debug.DrawRay(transform.position, dir);

        var dirX = dir;
        dirX.y = 0f;
        Debug.DrawRay(transform.position, dirX.normalized, Color.red);

        var dirY = dir;
        dirY.x = 0f;
        dirY.z = Mathf.Abs(dirY.z);
        Debug.DrawRay(transform.position, dirY.normalized, Color.green);

        var rotDirY = Vector3.Cross(Vector3.forward, dirX);
        var rotDirX = Vector3.Cross(Vector3.forward, dirY);

        var angleX = Angle(dir, dirX);
        var angleY = Angle(Vector3.forward, dirX);

        if (dirX.magnitude == 0)
        {
            transform.rotation = Quaternion.Euler(90f * Mathf.Sign(rotDirX.x), 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(angleX * Mathf.Sign(rotDirX.x), angleY * Mathf.Sign(rotDirY.y), 0);
        }

    }
}
