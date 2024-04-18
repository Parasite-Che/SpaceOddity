using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector2 GetScreenSizeInUnits()
    {
        return Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) - Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
    }

    public static void PullByGravity(Rigidbody2D movingBody, Transform staticBody, float intensity)
    {
        float distance = Vector2.Distance(movingBody.position, staticBody.position);
        Vector2 pullForce = (staticBody.position - (Vector3)movingBody.position).normalized / distance * intensity;
        movingBody.AddForce(pullForce, ForceMode2D.Force);
    }
}