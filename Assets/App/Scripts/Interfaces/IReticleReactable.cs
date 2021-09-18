using UnityEngine;

/// <summary>
/// レティクルが合わさった際に反応をするインターフェース
/// </summary>
interface IReticleReactable
{
    /// <summary>
    /// エイムが合わさった時の処理
    /// </summary>
    public void OnAimed(RaycastHit raycastHit);

    /// <summary>
    /// エイムが外れた時の処理
    /// </summary>
    public void OnUnAimed();
}
