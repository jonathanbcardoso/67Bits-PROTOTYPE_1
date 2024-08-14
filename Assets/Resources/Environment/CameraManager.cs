using TMPro;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("                                    Camera Stuff")]
    public Vector3 CameraStartingPoint;
    public Transform Player;

    // Update is called once per frame

    private void Start()
    {
        Application.targetFrameRate = 100;
    }
    void FixedUpdate()
    {
        transform.position = Player.position + CameraStartingPoint;
    }
}
