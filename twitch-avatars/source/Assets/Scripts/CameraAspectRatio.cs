using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
    public float targetAspect = 16.0f / 9.0f;

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        camera.aspect = targetAspect;
        //setCameraAspectRatio(targetAspect);
    }

    public void setCameraAspectRatio(float targetAspect)
    {
        // determine the game window's current aspect ratio
        float windowaspect = Screen.width / Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetAspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
