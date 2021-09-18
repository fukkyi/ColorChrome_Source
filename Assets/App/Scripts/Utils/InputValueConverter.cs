using UnityEngine;
using UnityEngine.InputSystem;

public class InputValueConverter
{
    /// <summary>
    /// InputValue����RotateCamera�p�̒l�����o��
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector2 GetRotateCameraValue(InputAction.CallbackContext context)
    {
        return context.ReadValue<Vector2>();
    }

    /// <summary>
    /// InputValue����Move�p�̒l�����o��
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector2 GetMoveValue(InputAction.CallbackContext context)
    {
        return context.ReadValue<Vector2>();
    }
}
