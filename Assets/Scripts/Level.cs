using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public static bool IsGameOver { get; set; }

    public GameObject[] SubLevels;
    public int CurrentSubLevel;

    private GameObject levelLoader;
    private LevelLoader loader;

    public GameObject blackholeParent;
    public GameObject blackhole;
    private MeshCollider blackholeCollider;
    private PositionClamper blackholePositionClamper;
    private TouchMove blackholeTouchMove;

    private Text levelCompleteText;
    private Text nextLevelInText;

    private bool isCountingDown;

    private void Start()
    {
        Camera.main.transform.position = CalculateCameraLocationOfSubLevel(CurrentSubLevel);

        levelLoader = GameObject.Find("LevelLoader");
        loader = levelLoader.GetComponent<LevelLoader>();

        blackholeParent = GameObject.Find("BlackholeParent");
        blackhole = blackholeParent.transform.GetChild(0).gameObject;
        blackholeParent.transform.position = CalculateBlackholeLocationOfSubLevel(CurrentSubLevel);
        blackholeCollider = blackhole.GetComponent<MeshCollider>();
        blackholePositionClamper = blackholeParent.GetComponent<PositionClamper>();
        blackholePositionClamper.SetClampValueX(new Vector2(-2, 2));
        blackholePositionClamper.SetClampValueZ(CalculateZBoundsOfSubLevel(CurrentSubLevel));
        blackholeTouchMove = blackholeParent.GetComponent<TouchMove>();

        levelCompleteText = GameObject.Find("LevelCompleteText").GetComponent<Text>();
        nextLevelInText = GameObject.Find("NextLevelInText").GetComponent<Text>();
    }

    void Update()
    {
        if (IsGameOver) return;
        if (IsSubLevelOver(CurrentSubLevel))
        {
            if (CurrentSubLevel + 1 == SubLevels.Length)
            {
                if(!isCountingDown)
                {
                    isCountingDown = true;
                    levelCompleteText.enabled = true;
                    nextLevelInText.enabled = true;
                    StartCoroutine(LoadNextLevelAfterSeconds(5));
                }
            }
            else
            {
                SubLevelTransition();
            }
        }
    }

    #region Implementation Details
    private bool IsSubLevelOver(int i)
    {
        GameObject sublevel = SubLevels[i];
        if (sublevel.transform.childCount == 0)
        {
            return true;
        }
        foreach(Transform child in sublevel.transform)
        {
            EatableObject scriptHandle = child.GetComponent<EatableObject>();
            if (!scriptHandle.IsEvil && !scriptHandle.IsEaten)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Disable the blackhole
    /// Try to center the blackhole
    ///
    /// If centered,
    /// Try to move the camera and the blackhole to the next sublevel
    ///
    /// When movement ends,
    /// Reenable blackhole and advance the sublevel index.
    /// </summary>
    private void SubLevelTransition()
    {
        DisableBlackholeInteractivity();
        if (blackholeParent.transform.position.x != 0)
        {
            blackholeParent.transform.position = Vector3.MoveTowards(blackholeParent.transform.position, new Vector3(0, blackholeParent.transform.position.y, blackholeParent.transform.position.z), Time.deltaTime);
        }
        else
        {
            bool isCameraMovementDone = TryToMoveCameraTo(CalculateCameraLocationOfSubLevel(CurrentSubLevel + 1));
            bool isBlackholeMovementDone = TryToMoveBlackholeTo(CalculateBlackholeLocationOfSubLevel(CurrentSubLevel + 1));
            if(isCameraMovementDone && isBlackholeMovementDone)
            {
                ++CurrentSubLevel;
                EnableBlackholeInteractivity();
                blackholePositionClamper.SetClampValueZ(CalculateZBoundsOfSubLevel(CurrentSubLevel));
            }
        }
    }

    #region Utility Functions
    /// <summary>
    /// Tries to move the camera.
    /// If the camera is succesfully moved to the destination, returns true.
    /// Else, returns false.
    /// </summary>
    ///
    /// <returns>
    /// Is camera at the position passed as input?
    /// </returns>
    private bool TryToMoveCameraTo(Vector3 position)
    {
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, position, 10 * Time.deltaTime);
        return Camera.main.transform.position.Equals(position);
    }

    /// <summary>
    /// Takes a sublevel index and computes the coordinates for the camera.
    /// </summary>
    private Vector3 CalculateCameraLocationOfSubLevel(int i)
    {
        Vector3 cameraDefaultPosition = Quaternion.Euler(Camera.main.GetComponent<CameraRotator>().Tilt, 0, 0) * (10 * Vector3.back);
        cameraDefaultPosition.z += 24 * i;
        return cameraDefaultPosition;
    }

    /// <summary>
    /// Tries to move the blackhole.
    /// If the blackhole is succesfully moved to the destination, returns true.
    /// Else, returns false.
    /// </summary>
    ///
    /// <returns>
    /// Is camera at the position passed as input?
    /// </returns>
    private bool TryToMoveBlackholeTo(Vector3 position)
    {
        blackholeParent.transform.position = Vector3.MoveTowards(blackholeParent.transform.position, position, 10 * Time.deltaTime);
        return blackholeParent.transform.position.Equals(position);
    }

    /// <summary>
    /// Takes a sublevel index and computes the coordinates for the blackhole.
    /// </summary>
    private Vector3 CalculateBlackholeLocationOfSubLevel(int i)
    {
        Vector3 blackholeDefaultPosition = new Vector3(0, -0.99f, -2.5f);
        blackholeDefaultPosition.z += 24 * i;
        return blackholeDefaultPosition;
    }

    /// <summary>
    /// Takes a sublevel index and computes the z coordinate bounds for blackhole.
    /// </summary>
    private Vector2 CalculateZBoundsOfSubLevel(int i)
    {
        return new Vector2(-2.82f + 24 * i, 6.18f + 24 * i);
    }

    /// <summary>
    /// Enables blackhole's collider, position clamper and touch move scripts.
    /// </summary>
    private void EnableBlackholeInteractivity()
    {
        blackholeCollider.enabled = true;
        blackholePositionClamper.enabled = true;
        blackholeTouchMove.enabled = true;
    }

    /// <summary>
    /// Disables blackhole's collider, position clamper and touch move scripts.
    /// </summary>
    private void DisableBlackholeInteractivity()
    {
        blackholeCollider.enabled = false;
        blackholePositionClamper.enabled = false;
        blackholeTouchMove.enabled = false;
    }

    IEnumerator LoadNextLevelAfterSeconds(int countdownValue)
    {
        int remainingSeconds = countdownValue;
        while (remainingSeconds > 0)
        {
            blackholeCollider.enabled = false;
            nextLevelInText.text = "Next Level\nIn " + remainingSeconds;
            yield return new WaitForSeconds(1.0f);
            remainingSeconds--;
        }
        blackholeCollider.enabled = true;
        levelCompleteText.enabled = false;
        nextLevelInText.enabled = false;
        isCountingDown = false;
        loader.NextLevel();
    }
    #endregion
    #endregion
}
