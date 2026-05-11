using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float height = 1f;
    [Range(0, 2 * Mathf.PI), SerializeField]
    private float periodOffset;

    [SerializeField]
    private bool autoOffset;

    private Vector3 startPoint;
    private void Start()
    {
        startPoint = transform.position;
        if (autoOffset)
        {
            periodOffset = Random.Range(0, 2 * Mathf.PI);
        }
    }

    void Update()
    {
        var offset = Vector3.up * height * Mathf.Sin(speed * Time.time + periodOffset);

        transform.position = startPoint + offset;
    }
}
