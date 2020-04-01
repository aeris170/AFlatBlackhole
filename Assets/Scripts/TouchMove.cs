using UnityEngine;

public class TouchMove : MonoBehaviour
{
    private const float COEFF = 0.006f;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved)
            {
                transform.position += new Vector3(touch.deltaPosition.x * COEFF, 0, touch.deltaPosition.y * COEFF);
            }
        }
    }
}
