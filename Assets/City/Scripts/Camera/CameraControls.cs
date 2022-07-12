using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float mouseSpeed = 1;
    [SerializeField] private float moveSpeed = 1;

    // Rotation
    private float xRot, yRot;

    private void Start()
    {
        Vector3 currentRot = transform.rotation.eulerAngles;
        xRot = currentRot.x;
        yRot = currentRot.y;
    }

    private void Update()
    {
        float scalar = Mathf.Min(Time.deltaTime * 100, 2);

        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        if (Input.GetMouseButton(1))
        {
            xRot -= my * mouseSpeed * scalar;
            yRot += mx * mouseSpeed * scalar;
        }

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);

        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");
        float dz = (Input.GetKey(KeyCode.Q) ? -1 : 0) + (Input.GetKey(KeyCode.E) ? 1 : 0);
        transform.position += (transform.forward * dy + transform.right * dx + transform.up * dz) * moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? 3 : 1) * scalar;
    }
}
