using UnityEngine;

public class LightColorController : MonoBehaviour
{
    void Start()
    {
        Light l = GetComponent<Light>();
        Color old = l.color;
        Color.RGBToHSV(old, out float H, out float S, out float V);
        l.color = Color.HSVToRGB(H, S, V / 2);
    }
}
