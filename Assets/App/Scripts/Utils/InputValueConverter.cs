using UnityEngine;
using UnityEngine.InputSystem;

public class InputValueConverter
{
    /// <summary>
    /// InputValueからRotateCamera用の値を取り出す
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector2 GetRotateCameraValue(InputAction.CallbackContext context)
    {
        return context.ReadValue<Vector2>();
    }

    /// <summary>
    /// InputValueからMove用の値を取り出す
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector2 GetMoveValue(InputAction.CallbackContext context)
    {
        return context.ReadValue<Vector2>();
    }
}
