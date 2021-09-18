using UnityEngine;

public class GS_Parameter
{
    #region Parameter Name  
    static readonly string color        = "_Color";
    static readonly string mainTex      = "_MainTex";
    static readonly string disolveTex   = "_DisolveTex";
    static readonly string fadeTrigger  = "_FadeTrigger";
    static readonly string threshold    = "_Threshold";
    #endregion

    /// <summary>
    /// GS�p�}�e���A���Z�b�g����
    /// </summary>
    /// <param name="shader">�Z�b�g����V�F�[�_�[</param> TODO: "shader" ����͕ς���K�v���Ȃ��̂ň����ɂ���Ӗ��Ȃ�����
    /// <param name="col">�J���[</param>
    /// <param name="mTex">���C���̃e�N�X�`��</param>
    /// <param name="dTex">���[���摜</param>
    /// <param name="fade">�F�������t�F�[�h���̕���</param>
    /// <returns></returns>
    public static Material GeneratMaterial(Shader shader, Color col, Texture mTex = null, Texture dTex = null, bool fade = false)
    {
        Material m = new Material(shader);
        if (mTex != null) m.SetTexture(mainTex, mTex);
        if (dTex != null) m.SetTexture(disolveTex, dTex);
        var f = fade == true ? 1 : 0;
        m.SetFloat(fadeTrigger, f);
        if (f == 1) m.SetColor(color, col * new Color(1, 1, 1, 0));
        else m.SetColor(color, col);
        return m;

    }
    /// <summary>
    /// �t�F�[�h�����̉��o��ς��邽�߂̏���
    /// </summary>
    /// <param name="m">�t�F�[�h�̃}�e���A��</param>
    /// <param name="tex">�ύX���鉉�o�悤�e�N�X�`��</param>
    /// <param name="col">�t�F�[�h���̐F</param>
    public static void FadeTextureChange(Material m, Texture tex, Color col)
    {
        m.SetTexture(disolveTex, tex);
        m.SetColor(color, col * new Color(1,1,1,0));
    }
    /// <summary>
    /// �F����/�t�F�[�h�� �������l�ݒ�p
    /// </summary>
    /// <param name="m">�ύX����}�e���A��</param>
    /// <param name="v">�ݒ肷�� �������l</param>
    public static void SetThreshold(Material m, float v)
    {
        m.SetFloat(threshold, v);
    }
}
