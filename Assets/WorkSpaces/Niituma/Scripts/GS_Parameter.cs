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
    /// GS用マテリアルセット処理
    /// </summary>
    /// <param name="shader">セットするシェーダー</param> TODO: "shader" これは変える必要がないので引数にする意味ないかも
    /// <param name="col">カラー</param>
    /// <param name="mTex">メインのテクスチャ</param>
    /// <param name="dTex">ルール画像</param>
    /// <param name="fade">色抜きかフェードかの分岐</param>
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
    /// フェード処理の演出を変えるための処理
    /// </summary>
    /// <param name="m">フェードのマテリアル</param>
    /// <param name="tex">変更する演出ようテクスチャ</param>
    /// <param name="col">フェード時の色</param>
    public static void FadeTextureChange(Material m, Texture tex, Color col)
    {
        m.SetTexture(disolveTex, tex);
        m.SetColor(color, col * new Color(1,1,1,0));
    }
    /// <summary>
    /// 色抜き/フェードの しきい値設定用
    /// </summary>
    /// <param name="m">変更するマテリアル</param>
    /// <param name="v">設定する しきい値</param>
    public static void SetThreshold(Material m, float v)
    {
        m.SetFloat(threshold, v);
    }
}
