using UnityEngine;

/// <summary>
/// ���e�B�N�������킳�����ۂɔ���������C���^�[�t�F�[�X
/// </summary>
interface IReticleReactable
{
    /// <summary>
    /// �G�C�������킳�������̏���
    /// </summary>
    public void OnAimed(RaycastHit raycastHit);

    /// <summary>
    /// �G�C�����O�ꂽ���̏���
    /// </summary>
    public void OnUnAimed();
}
