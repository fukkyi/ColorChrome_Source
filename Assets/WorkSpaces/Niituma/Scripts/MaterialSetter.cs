using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class MaterialSetter : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Shader _shader = null;
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private Texture _mainTex = null;
    [SerializeField] private Texture _disolveTex = null;
    [SerializeField] private bool _fadeTrigger = false;


    void Awake()
    {
        _image.material = GS_Parameter.GeneratMaterial(_shader, _color, _mainTex, _disolveTex, _fadeTrigger);
    }

}
