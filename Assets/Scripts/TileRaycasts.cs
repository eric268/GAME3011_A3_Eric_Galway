using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRaycasts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D[] hitLeft = Physics2D.RaycastAll(transform.position, Vector2.left, 25.0f);
        RaycastHit2D[] hitRight = Physics2D.RaycastAll(transform.position, Vector2.right, 25.0f);
        RaycastHit2D[] hitUp = Physics2D.RaycastAll(transform.position, Vector2.up, 25.0f);
        RaycastHit2D[] hitDown = Physics2D.RaycastAll(transform.position, Vector2.down, 25.0f);

        Debug.DrawLine(transform.position, transform.position + (25.0f * Vector3.right), Color.red, Time.fixedDeltaTime);
        Debug.DrawLine(transform.position, transform.position + (50.0f * Vector3.left), Color.red, Time.fixedDeltaTime);
        Debug.DrawLine(transform.position, transform.position + (-50.0f * Vector3.up), Color.red, Time.fixedDeltaTime);
        Debug.DrawLine(transform.position, transform.position + (-50.0f * Vector3.down), Color.red, Time.fixedDeltaTime);

        foreach(RaycastHit2D hit in hitRight)
        {
            print(hit.collider.gameObject.name);
        }
    }
}
