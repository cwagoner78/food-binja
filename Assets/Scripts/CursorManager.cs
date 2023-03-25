using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D _teethOpenTexture;
    [SerializeField] private Texture2D _teethClosedTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public float hotSpotX = 15f;
    public float hotSpotY = 15f;


    private void Start()
    {
        //Cursor.visible = false;
        Cursor.SetCursor(_teethOpenTexture, new Vector2(hotSpotX, hotSpotY), cursorMode);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) Cursor.SetCursor(_teethClosedTexture, new Vector2(hotSpotX, hotSpotY), cursorMode);
        if (Input.GetMouseButtonUp(0)) Cursor.SetCursor(_teethOpenTexture, new Vector2(hotSpotX, hotSpotY), cursorMode);
    }
}
