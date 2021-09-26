using System.Collections.Generic;
using UnityEngine;

public class LayerMaskUtil
{
    /// <summary>
    /// �f�t�H���g�̃��C���[
    /// </summary>
    public static readonly int DefaultLayerNumber = 0;
    public static readonly string DefaultLayerName = "Default";
    /// <summary>
    /// ���ʐF�ɏo����I�u�W�F�N�g�̃��C���[
    /// </summary>
    public static readonly int GrayableLayerNumber = 6;
    public static readonly string GrayableLayerName = "Grayable";
    /// <summary>
    /// �v���C���[�̃��C���[
    /// </summary>
    public static readonly int PlayerLayerNumber = 7;
    public static readonly string PlayerLayerName = "Player";
    /// <summary>
    /// �G�̃��C���[
    /// </summary>
    public static readonly int EnemyLayerNumber = 8;
    public static readonly string EnemyLayerName = "Enemy";
    /// <summary>
    /// ���ʐF�ɏo����I�u�W�F�N�g�̃��C���[
    /// </summary>
    public static readonly int GrayableGrassLayerNumber = 11;
    public static readonly string GrayableGrassLayerName = "GrayableGrass";
    /// <summary>
    /// �v���C���[�U������̃��C���[
    /// </summary>
    public static readonly int PlayerAttackLayerNumber = 12;
    public static readonly string PlayerAttackLayerName = "PlayerAttackZone";
    /// <summary>
    /// �G�U������̃��C���[
    /// </summary>
    public static readonly int EnemyAttackLayerNumber = 13;
    public static readonly string EnemyAttackLayerName = "EnemyAttackZone";
    /// <summary>
    /// �Փ˂��Ȃ����ʐF�ɏo����I�u�W�F�N�g�̃��C���[
    /// </summary>
    public static readonly int GrayableNonCollisionLayerNumber = 14;
    public static readonly string GrayableNonCollisionLayerName = "Grayable_NonCollision";
    /// <summary>
    /// Terrain�̃��C���[
    /// </summary>
    public static readonly int TerrainLayerNumber = 17;
    public static readonly string TerrainLayerName = "Terrain";
    /// <summary>
    /// ���S�����G�̃��C���[
    /// </summary>
    public static readonly int DeadEnemyLayerNumber = 18;
    public static readonly string DeadEnemyLayerName = "DeadEnemy";

    private static readonly string[] GroundLayers = { "Default", GrayableLayerName };

    /// <summary>
    /// �n�`�̃��C���[�ɔ������郌�C���[�}�X�N���擾����
    /// </summary>
    /// <returns></returns>
    public static int GetLayerMaskGrounds()
    {
        return LayerMask.GetMask(GroundLayers);
    }

    /// <summary>
    /// �n�`�̃��C���[�ƓG�ɔ������郌�C���[�}�X�N���擾����
    /// </summary>
    /// <returns></returns>
    public static int GetLayerMaskGroundsAndEnemy()
    {
        return ~LayerMask.GetMask(PlayerLayerName, PlayerAttackLayerName, EnemyAttackLayerName, GrayableGrassLayerName);
    }

    /// <summary>
    /// ������炷�I�u�W�F�N�g�݂̂ɔ������郌�C���[�}�X�N���擾����
    /// </summary>
    /// <returns></returns>
    public static int GetLayerMaskFootstepsObject()
    {
        return LayerMask.GetMask(TerrainLayerName, DefaultLayerName);
    }
}
