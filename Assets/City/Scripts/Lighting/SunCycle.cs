using UnityEngine;

public class SunCycle : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float rotationSpeed = 5f;

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime, 0, 0);
    }
}
