using UnityEngine;

public class PositionClamper : MonoBehaviour
{
    private Vector2 xClamp;
    private Vector2 zClamp;

    private void Awake()
    {
        xClamp = new Vector2();
        zClamp = new Vector2();
    }

    void Update()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, xClamp.x, xClamp.y);
        transform.position = pos;

        pos = transform.position;
        pos.z = Mathf.Clamp(transform.position.z, zClamp.x, zClamp.y);
        transform.position = pos;
    }

    public void SetClampValueX(Vector2 xClamp)
    {
        this.xClamp = xClamp;
    }

    public void SetClampValueZ(Vector2 zClamp)
    {
        this.zClamp = zClamp;
    }
}