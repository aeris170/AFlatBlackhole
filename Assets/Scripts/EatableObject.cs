using UnityEngine;
using UnityEngine.UI;

public class EatableObject : MonoBehaviour
{
    public float AttractionDistance;
    public bool IsEvil;

    private Collider ownCollider;

    public bool IsEaten { get; private set; } = false;
    private bool vibrateTriggered = false;

    private void Start()
    {
        ownCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if(transform.position.y < 0)
        {
            IsEaten = true;
        }
        if(IsEaten && !vibrateTriggered)
        {
            if (IsEvil)
            {
                GameOver();
            }
            vibrateTriggered = true;
            Handheld.Vibrate();
        }
        if (transform.position.y < -3)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Blackhole"))
        {
            ownCollider.enabled = false;
        }
    }

    private void GameOver()
    {
        Level.IsGameOver = true;
        GameObject.Find("GameOverText").GetComponent<Text>().enabled = true;
        GameObject.Find("BlackholeParent").GetComponent<TouchMove>().enabled = false;
        GameObject.Find("Blackhole").GetComponent<Collider>().enabled = false;
    }
}
