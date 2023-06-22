using UnityEngine;
using DG.Tweening;

public static class TransformExtensions
{
    public static void MoveTowardsUI(this Transform transform, Transform uiWorldPos, float duration)
    {
        transform.DOMove(uiWorldPos.position, duration);
    }
}