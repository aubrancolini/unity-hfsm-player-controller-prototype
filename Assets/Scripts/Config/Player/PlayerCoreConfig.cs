using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCoreConfig", menuName = "Scriptable Objects/PlayerCoreConfig")]
public class PlayerCoreConfig : ScriptableObject
{
    [Header("Horizontal movement")]
    public float horizontalSpeed = 7f;
    [Tooltip("Higher values -> Higher Jump.")]
    public float baseHorizontalSpeedMultiplier = 1f;
}