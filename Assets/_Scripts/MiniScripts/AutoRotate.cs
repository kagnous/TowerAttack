using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotateSpeed = new Vector3(0, 1, 0);

    void FixedUpdate()
    {
        transform.Rotate(rotateSpeed);
    }
}