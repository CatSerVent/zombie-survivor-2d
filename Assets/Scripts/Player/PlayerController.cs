using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    Vector2 input;
    public void SetInput(Vector2 v) { input = v; }
    void Update()
    {
        Vector3 d = new Vector3(input.x, input.y, 0f) * moveSpeed * Time.deltaTime;
        transform.position += d;
        if (d.sqrMagnitude > 0.0001f) transform.up = d.normalized;
    }
}
