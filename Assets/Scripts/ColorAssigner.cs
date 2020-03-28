using UnityEngine;

public class ColorAssigner : MonoBehaviour
{
    [Rename("Color Order (For XML only)")]
    public int ColorOrder;

    [Rename("Current Color (readonly)")]
    public Color CurrentColor;

    private void OnValidate()
    {
        AssignColors();
    }

    public void OnLevelLoad()
    {
        CurrentColor = GameObject.Find("ColorManager").GetComponent<Colors>().GetRandomColorOfOrder(ColorOrder);
        AssignColors();
    }

    private void AssignColors()
    {
        GameObject[] gameAreaChildren = GameObject.FindGameObjectsWithTag("GameAreaChildren");
        foreach (GameObject child in gameAreaChildren)
        {
            SetColorOf(child.GetComponent<MeshRenderer>(), CurrentColor);
            foreach (Transform childOfChild in child.transform)
            {
                SetColorOf(childOfChild.GetComponent<MeshRenderer>(), CurrentColor);
            }
        }

        Color.RGBToHSV(CurrentColor, out float H, out float S, out float V);
        Color complement = Color.HSVToRGB((H + 0.5f) % 1f, .8f - S, V);

        GameObject[] eatables = GameObject.FindGameObjectsWithTag("Eatable");
        foreach (GameObject eatable in eatables)
        {
            if (eatable.GetComponent<EatableObject>().IsEvil)
            {
                SetColorOf(eatable.GetComponent<MeshRenderer>(), complement);
            }
            else
            {
                SetColorOf(eatable.GetComponent<MeshRenderer>(), Color.white);
            }
        }
    }

    private void SetColorOf(MeshRenderer m, Color c)
    {
        m.sharedMaterial = new Material(m.sharedMaterial)
        {
            color = c
        };
    }
}
