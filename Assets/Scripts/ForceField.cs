using UnityEngine;

public class ForceField : MonoBehaviour
{
    enum ForceFieldDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    [SerializeField] ForceFieldDirection pushDirection;
    [SerializeField] private float pushStrength = 10f;
    [SerializeField] private float maxForce = 10f;
    [SerializeField] Vector3 centerPoint;
    


    private Vector3 GetDirectionVector()
    {
        switch (pushDirection) 
        {
            case ForceFieldDirection.Left: return Vector3.left;
            case ForceFieldDirection.Right: return Vector3.right;
            case ForceFieldDirection.Up: return Vector3.up;
            case ForceFieldDirection.Down: return Vector3.down;
            default: return Vector3.zero;
        }

    }




    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Rigidbody2D rb = collision.attachedRigidbody;
            if(rb != null)
            {
                Vector3 direction = GetDirectionVector();
                float distance = Vector3.Dot(direction, transform.position - collision.transform.position);
                float strength = Mathf.Clamp(pushStrength * distance, 0, maxForce);

                rb.AddForce(direction * strength, ForceMode2D.Impulse);
            }
        }
    }
}
