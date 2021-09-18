Shader "Custom/InkFluid"
{
    Properties
    {
        _frequency("frequency", Float) = 20
        _WaveSpeed("WaveSpeed", Float) = 10
        _WaveHeight("WaveHeight", Float) = 24
        _WaveOffset("WaveOffset", Float) = 21
        _Color("Color", Color) = (0.9339623, 0.355272, 0, 0)
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}

        // UGUIのマスクが使えるようにステンシルを定義する
        _Stencil("Stencil ID", Float) = 0
        _StencilComp("Stencil Comparison", Float) = 8
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
            // ステンシルここまで
    }
        SubShader
        {
            Tags
            {
                "RenderPipeline" = "UniversalPipeline"
                "RenderType" = "Transparent"
                "UniversalMaterialType" = "Lit"
                "Queue" = "Transparent"
            }

            // ステンシルを適用する
            Stencil{
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Pass
            {
                Name "Sprite Lit"
                Tags
                {
                    "LightMode" = "Universal2D"
                }

            // Render State
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            // ZTestをunity_GUIZTestModeに変える
            ZTest[unity_GUIZTestMode]
            ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_SCREENPOSITION
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITELIT
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float4 texCoord0;
            float4 color;
            float4 screenPosition;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float4 uv0;
            float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float4 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            float4 interp2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings(Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyzw = input.texCoord0;
            output.interp1.xyzw = input.color;
            output.interp2.xyzw = input.screenPosition;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings(PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            output.screenPosition = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

        // --------------------------------------------------
        // Graph

        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
    float _frequency;
    float _WaveSpeed;
    float _WaveHeight;
    float _WaveOffset;
    float4 _Color;
    CBUFFER_END

        // Object and Global properties
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;
        SAMPLER(SamplerState_Linear_Repeat);

        // Graph Functions

    void Unity_Multiply_float(float A, float B, out float Out)
    {
        Out = A * B;
    }

    void Unity_Fraction_float(float In, out float Out)
    {
        Out = frac(In);
    }

    void Unity_SampleGradient_float(Gradient Gradient, float Time, out float4 Out)
    {
        float3 color = Gradient.colors[0].rgb;
        [unroll]
        for (int c = 1; c < 8; c++)
        {
            float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
            color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
        }
    #ifndef UNITY_COLORSPACE_GAMMA
        color = SRGBToLinear(color);
    #endif
        float alpha = Gradient.alphas[0].x;
        [unroll]
        for (int a = 1; a < 8; a++)
        {
            float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
            alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
        }
        Out = float4(color, alpha);
    }

    void Unity_Add_float4(float4 A, float4 B, out float4 Out)
    {
        Out = A + B;
    }

    void Unity_OneMinus_float(float In, out float Out)
    {
        Out = 1 - In;
    }

    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
    {
        Out = UV * Tiling + Offset;
    }


    float2 Unity_GradientNoise_Dir_float(float2 p)
    {
        // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
        p = p % 289;
        // need full precision, otherwise half overflows when p > 1
        float x = float(34 * p.x + 1) * p.x % 289 + p.y;
        x = (34 * x + 1) * x % 289;
        x = frac(x / 41) * 2 - 1;
        return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
    }

    void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
    {
        float2 p = UV * Scale;
        float2 ip = floor(p);
        float2 fp = frac(p);
        float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
        float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
        float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
        float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
        fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
        Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
    }

    void Unity_Blend_Screen_float(float Base, float Blend, out float Out, float Opacity)
    {
        Out = 1.0 - (1.0 - Blend) * (1.0 - Base);
        Out = lerp(Base, Out, Opacity);
    }

    void Unity_Maximum_float(float A, float B, out float Out)
    {
        Out = max(A, B);
    }

    void Unity_Blend_Divide_float4(float4 Base, float4 Blend, out float4 Out, float Opacity)
    {
        Out = Base / (Blend + 0.000000000001);
        Out = lerp(Base, Out, Opacity);
    }

    void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
    {
        RGBA = float4(R, G, B, A);
        RGB = float3(R, G, B);
        RG = float2(R, G);
    }

    void Unity_Add_float(float A, float B, out float Out)
    {
        Out = A + B;
    }

    void Unity_Sine_float(float In, out float Out)
    {
        Out = sin(In);
    }

    void Unity_Step_float(float Edge, float In, out float Out)
    {
        Out = step(Edge, In);
    }

    // Graph Vertex
    struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float4 SpriteMask;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    Gradient _Gradient_dcfb27bd47c745ab892eaef1e55d14ad_Out_0 = NewGradient(0, 4, 2, float4(1, 0, 0, 0),float4(0, 1, 0, 0.3329976),float4(0, 0, 1, 0.6659952),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
    float _Multiply_a99a66b168cc412da3f5e8fae61294d6_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, 0.1, _Multiply_a99a66b168cc412da3f5e8fae61294d6_Out_2);
    float _Fraction_a64471f9119b4df3bdf23230f357381f_Out_1;
    Unity_Fraction_float(_Multiply_a99a66b168cc412da3f5e8fae61294d6_Out_2, _Fraction_a64471f9119b4df3bdf23230f357381f_Out_1);
    float4 _SampleGradient_304139d346b34fd8bfd90c323bfa4520_Out_2;
    Unity_SampleGradient_float(_Gradient_dcfb27bd47c745ab892eaef1e55d14ad_Out_0, _Fraction_a64471f9119b4df3bdf23230f357381f_Out_1, _SampleGradient_304139d346b34fd8bfd90c323bfa4520_Out_2);
    float4 _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0 = IN.uv0;
    float _Split_0f43ac5a5c904352be9acefe125fd048_R_1 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[0];
    float _Split_0f43ac5a5c904352be9acefe125fd048_G_2 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[1];
    float _Split_0f43ac5a5c904352be9acefe125fd048_B_3 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[2];
    float _Split_0f43ac5a5c904352be9acefe125fd048_A_4 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[3];
    float _Multiply_f49cd046518a4db7a094de40daf556f3_Out_2;
    Unity_Multiply_float(_Split_0f43ac5a5c904352be9acefe125fd048_G_2, 0.2, _Multiply_f49cd046518a4db7a094de40daf556f3_Out_2);
    float4 _Add_896e119c6cff4adf860349a3eccce683_Out_2;
    Unity_Add_float4(_SampleGradient_304139d346b34fd8bfd90c323bfa4520_Out_2, (_Multiply_f49cd046518a4db7a094de40daf556f3_Out_2.xxxx), _Add_896e119c6cff4adf860349a3eccce683_Out_2);
    float _OneMinus_d0326770ea144971a2f7dd60bad24e8c_Out_1;
    Unity_OneMinus_float(IN.TimeParameters.x, _OneMinus_d0326770ea144971a2f7dd60bad24e8c_Out_1);
    float2 _TilingAndOffset_1e6d7ab5e77f4a17a924ecdb2352f393_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_OneMinus_d0326770ea144971a2f7dd60bad24e8c_Out_1.xx), _TilingAndOffset_1e6d7ab5e77f4a17a924ecdb2352f393_Out_3);
    float _GradientNoise_c1f6a64254f14b26b04a168fd626a7c1_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_1e6d7ab5e77f4a17a924ecdb2352f393_Out_3, 2, _GradientNoise_c1f6a64254f14b26b04a168fd626a7c1_Out_2);
    float2 _TilingAndOffset_5716e7a66976403a9f903a1bbc496b31_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (IN.TimeParameters.x.xx), _TilingAndOffset_5716e7a66976403a9f903a1bbc496b31_Out_3);
    float _GradientNoise_8906cfaf61e4471997e0ac71165d8d52_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_5716e7a66976403a9f903a1bbc496b31_Out_3, 2, _GradientNoise_8906cfaf61e4471997e0ac71165d8d52_Out_2);
    float _Blend_990059ca2a794bd8ae051d6d23de0100_Out_2;
    Unity_Blend_Screen_float(_GradientNoise_c1f6a64254f14b26b04a168fd626a7c1_Out_2, _GradientNoise_8906cfaf61e4471997e0ac71165d8d52_Out_2, _Blend_990059ca2a794bd8ae051d6d23de0100_Out_2, 1);
    float _Maximum_0978eb690efd43dbb2e00867157c0c07_Out_2;
    Unity_Maximum_float(_Blend_990059ca2a794bd8ae051d6d23de0100_Out_2, 0.3, _Maximum_0978eb690efd43dbb2e00867157c0c07_Out_2);
    float4 _Blend_8a0cce33d63044d09eb0b9eccec9014b_Out_2;
    Unity_Blend_Divide_float4(_Add_896e119c6cff4adf860349a3eccce683_Out_2, (_Maximum_0978eb690efd43dbb2e00867157c0c07_Out_2.xxxx), _Blend_8a0cce33d63044d09eb0b9eccec9014b_Out_2, 1);
    float4 _Combine_14ef834db90841e593e29fa8791ca16b_RGBA_4;
    float3 _Combine_14ef834db90841e593e29fa8791ca16b_RGB_5;
    float2 _Combine_14ef834db90841e593e29fa8791ca16b_RG_6;
    Unity_Combine_float(IN.TimeParameters.x, 0, 0, 0, _Combine_14ef834db90841e593e29fa8791ca16b_RGBA_4, _Combine_14ef834db90841e593e29fa8791ca16b_RGB_5, _Combine_14ef834db90841e593e29fa8791ca16b_RG_6);
    float2 _TilingAndOffset_d7947460ce004fdb8b6264458505125b_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_14ef834db90841e593e29fa8791ca16b_RG_6, _TilingAndOffset_d7947460ce004fdb8b6264458505125b_Out_3);
    float _GradientNoise_fd985463eec444be93224ee44886da0b_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_d7947460ce004fdb8b6264458505125b_Out_3, 5, _GradientNoise_fd985463eec444be93224ee44886da0b_Out_2);
    float4 _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0 = IN.uv0;
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_R_1 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[0];
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_G_2 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[1];
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_B_3 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[2];
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_A_4 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[3];
    float _Property_ba2a3f0e900c42e7a78943147136d880_Out_0 = _frequency;
    float _Multiply_69fd945d41774357a9d2d705570d0a71_Out_2;
    Unity_Multiply_float(_Split_919a80cc5b8046ea8de51fbad2a3f6a9_R_1, _Property_ba2a3f0e900c42e7a78943147136d880_Out_0, _Multiply_69fd945d41774357a9d2d705570d0a71_Out_2);
    float _Property_eba1580a62ad44e59cfffd69b0cd27d0_Out_0 = _WaveSpeed;
    float _Multiply_d9d2b6c80d3d470194bbe2d0594aa8da_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, _Property_eba1580a62ad44e59cfffd69b0cd27d0_Out_0, _Multiply_d9d2b6c80d3d470194bbe2d0594aa8da_Out_2);
    float _Add_fa66d2f88db14e01b7e676701a22c7da_Out_2;
    Unity_Add_float(_Multiply_69fd945d41774357a9d2d705570d0a71_Out_2, _Multiply_d9d2b6c80d3d470194bbe2d0594aa8da_Out_2, _Add_fa66d2f88db14e01b7e676701a22c7da_Out_2);
    float _Sine_ce2ac6e5f9a64be88565bbf06e23a934_Out_1;
    Unity_Sine_float(_Add_fa66d2f88db14e01b7e676701a22c7da_Out_2, _Sine_ce2ac6e5f9a64be88565bbf06e23a934_Out_1);
    float _Multiply_4e0dd23ffa1d4fb6aa15c44b53571808_Out_2;
    Unity_Multiply_float(_GradientNoise_fd985463eec444be93224ee44886da0b_Out_2, _Sine_ce2ac6e5f9a64be88565bbf06e23a934_Out_1, _Multiply_4e0dd23ffa1d4fb6aa15c44b53571808_Out_2);
    float _Property_9ac35708677b4631922e87eeb38d1b33_Out_0 = _WaveHeight;
    float _Multiply_ba1f56678f1a4af7b493a64492bc1d27_Out_2;
    Unity_Multiply_float(_Split_919a80cc5b8046ea8de51fbad2a3f6a9_G_2, _Property_9ac35708677b4631922e87eeb38d1b33_Out_0, _Multiply_ba1f56678f1a4af7b493a64492bc1d27_Out_2);
    float _Add_ece30c85f12c4e21b2460f9b3bffa059_Out_2;
    Unity_Add_float(_Multiply_4e0dd23ffa1d4fb6aa15c44b53571808_Out_2, _Multiply_ba1f56678f1a4af7b493a64492bc1d27_Out_2, _Add_ece30c85f12c4e21b2460f9b3bffa059_Out_2);
    float _Property_d4452a37c81a4e5db27b7682d12d47d4_Out_0 = _WaveOffset;
    float _Step_67602ca94b6d4ba28fb446b54cc6afac_Out_2;
    Unity_Step_float(_Add_ece30c85f12c4e21b2460f9b3bffa059_Out_2, _Property_d4452a37c81a4e5db27b7682d12d47d4_Out_0, _Step_67602ca94b6d4ba28fb446b54cc6afac_Out_2);
    UnityTexture2D _Property_9083188972664c73927901759cf5dab9_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9083188972664c73927901759cf5dab9_Out_0.tex, _Property_9083188972664c73927901759cf5dab9_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_R_4 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.r;
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_G_5 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.g;
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_B_6 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.b;
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_A_7 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.a;
    float _Multiply_5ea4d834adca4c739ebc2beec9dc3f40_Out_2;
    Unity_Multiply_float(_Step_67602ca94b6d4ba28fb446b54cc6afac_Out_2, _SampleTexture2D_19a425feb2a843a1978e7680880bb194_A_7, _Multiply_5ea4d834adca4c739ebc2beec9dc3f40_Out_2);
    surface.BaseColor = (_Blend_8a0cce33d63044d09eb0b9eccec9014b_Out_2.xyz);
    surface.Alpha = _Multiply_5ea4d834adca4c739ebc2beec9dc3f40_Out_2;
    surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 0) : float4 (SRGBToLinear(float3(1, 1, 1)), 0);
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.uv0 = input.texCoord0;
    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteLitPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "Sprite Normal"
    Tags
    {
        "LightMode" = "NormalsRendering"
    }

        // Render State
        Cull Off
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
    #pragma exclude_renderers d3d11_9x
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITENORMAL
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        float4 uv0 : TEXCOORD0;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float3 normalWS;
        float4 tangentWS;
        float4 texCoord0;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float3 TangentSpaceNormal;
        float4 uv0;
        float3 TimeParameters;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float3 interp0 : TEXCOORD0;
        float4 interp1 : TEXCOORD1;
        float4 interp2 : TEXCOORD2;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.normalWS;
        output.interp1.xyzw = input.tangentWS;
        output.interp2.xyzw = input.texCoord0;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.normalWS = input.interp0.xyz;
        output.tangentWS = input.interp1.xyzw;
        output.texCoord0 = input.interp2.xyzw;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float _frequency;
float _WaveSpeed;
float _WaveHeight;
float _WaveOffset;
float4 _Color;
CBUFFER_END

// Object and Global properties
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
float4 _MainTex_TexelSize;
SAMPLER(SamplerState_Linear_Repeat);

// Graph Functions

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Fraction_float(float In, out float Out)
{
    Out = frac(In);
}

void Unity_SampleGradient_float(Gradient Gradient, float Time, out float4 Out)
{
    float3 color = Gradient.colors[0].rgb;
    [unroll]
    for (int c = 1; c < 8; c++)
    {
        float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
        color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
    }
#ifndef UNITY_COLORSPACE_GAMMA
    color = SRGBToLinear(color);
#endif
    float alpha = Gradient.alphas[0].x;
    [unroll]
    for (int a = 1; a < 8; a++)
    {
        float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
        alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
    }
    Out = float4(color, alpha);
}

void Unity_Add_float4(float4 A, float4 B, out float4 Out)
{
    Out = A + B;
}

void Unity_OneMinus_float(float In, out float Out)
{
    Out = 1 - In;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}


float2 Unity_GradientNoise_Dir_float(float2 p)
{
    // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
    p = p % 289;
    // need full precision, otherwise half overflows when p > 1
    float x = float(34 * p.x + 1) * p.x % 289 + p.y;
    x = (34 * x + 1) * x % 289;
    x = frac(x / 41) * 2 - 1;
    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}

void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
{
    float2 p = UV * Scale;
    float2 ip = floor(p);
    float2 fp = frac(p);
    float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
    float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
    float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
    float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
}

void Unity_Blend_Screen_float(float Base, float Blend, out float Out, float Opacity)
{
    Out = 1.0 - (1.0 - Blend) * (1.0 - Base);
    Out = lerp(Base, Out, Opacity);
}

void Unity_Maximum_float(float A, float B, out float Out)
{
    Out = max(A, B);
}

void Unity_Blend_Divide_float4(float4 Base, float4 Blend, out float4 Out, float Opacity)
{
    Out = Base / (Blend + 0.000000000001);
    Out = lerp(Base, Out, Opacity);
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Sine_float(float In, out float Out)
{
    Out = sin(In);
}

void Unity_Step_float(float Edge, float In, out float Out)
{
    Out = step(Edge, In);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float3 NormalTS;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    Gradient _Gradient_dcfb27bd47c745ab892eaef1e55d14ad_Out_0 = NewGradient(0, 4, 2, float4(1, 0, 0, 0),float4(0, 1, 0, 0.3329976),float4(0, 0, 1, 0.6659952),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
    float _Multiply_a99a66b168cc412da3f5e8fae61294d6_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, 0.1, _Multiply_a99a66b168cc412da3f5e8fae61294d6_Out_2);
    float _Fraction_a64471f9119b4df3bdf23230f357381f_Out_1;
    Unity_Fraction_float(_Multiply_a99a66b168cc412da3f5e8fae61294d6_Out_2, _Fraction_a64471f9119b4df3bdf23230f357381f_Out_1);
    float4 _SampleGradient_304139d346b34fd8bfd90c323bfa4520_Out_2;
    Unity_SampleGradient_float(_Gradient_dcfb27bd47c745ab892eaef1e55d14ad_Out_0, _Fraction_a64471f9119b4df3bdf23230f357381f_Out_1, _SampleGradient_304139d346b34fd8bfd90c323bfa4520_Out_2);
    float4 _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0 = IN.uv0;
    float _Split_0f43ac5a5c904352be9acefe125fd048_R_1 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[0];
    float _Split_0f43ac5a5c904352be9acefe125fd048_G_2 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[1];
    float _Split_0f43ac5a5c904352be9acefe125fd048_B_3 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[2];
    float _Split_0f43ac5a5c904352be9acefe125fd048_A_4 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[3];
    float _Multiply_f49cd046518a4db7a094de40daf556f3_Out_2;
    Unity_Multiply_float(_Split_0f43ac5a5c904352be9acefe125fd048_G_2, 0.2, _Multiply_f49cd046518a4db7a094de40daf556f3_Out_2);
    float4 _Add_896e119c6cff4adf860349a3eccce683_Out_2;
    Unity_Add_float4(_SampleGradient_304139d346b34fd8bfd90c323bfa4520_Out_2, (_Multiply_f49cd046518a4db7a094de40daf556f3_Out_2.xxxx), _Add_896e119c6cff4adf860349a3eccce683_Out_2);
    float _OneMinus_d0326770ea144971a2f7dd60bad24e8c_Out_1;
    Unity_OneMinus_float(IN.TimeParameters.x, _OneMinus_d0326770ea144971a2f7dd60bad24e8c_Out_1);
    float2 _TilingAndOffset_1e6d7ab5e77f4a17a924ecdb2352f393_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_OneMinus_d0326770ea144971a2f7dd60bad24e8c_Out_1.xx), _TilingAndOffset_1e6d7ab5e77f4a17a924ecdb2352f393_Out_3);
    float _GradientNoise_c1f6a64254f14b26b04a168fd626a7c1_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_1e6d7ab5e77f4a17a924ecdb2352f393_Out_3, 2, _GradientNoise_c1f6a64254f14b26b04a168fd626a7c1_Out_2);
    float2 _TilingAndOffset_5716e7a66976403a9f903a1bbc496b31_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (IN.TimeParameters.x.xx), _TilingAndOffset_5716e7a66976403a9f903a1bbc496b31_Out_3);
    float _GradientNoise_8906cfaf61e4471997e0ac71165d8d52_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_5716e7a66976403a9f903a1bbc496b31_Out_3, 2, _GradientNoise_8906cfaf61e4471997e0ac71165d8d52_Out_2);
    float _Blend_990059ca2a794bd8ae051d6d23de0100_Out_2;
    Unity_Blend_Screen_float(_GradientNoise_c1f6a64254f14b26b04a168fd626a7c1_Out_2, _GradientNoise_8906cfaf61e4471997e0ac71165d8d52_Out_2, _Blend_990059ca2a794bd8ae051d6d23de0100_Out_2, 1);
    float _Maximum_0978eb690efd43dbb2e00867157c0c07_Out_2;
    Unity_Maximum_float(_Blend_990059ca2a794bd8ae051d6d23de0100_Out_2, 0.3, _Maximum_0978eb690efd43dbb2e00867157c0c07_Out_2);
    float4 _Blend_8a0cce33d63044d09eb0b9eccec9014b_Out_2;
    Unity_Blend_Divide_float4(_Add_896e119c6cff4adf860349a3eccce683_Out_2, (_Maximum_0978eb690efd43dbb2e00867157c0c07_Out_2.xxxx), _Blend_8a0cce33d63044d09eb0b9eccec9014b_Out_2, 1);
    float4 _Combine_14ef834db90841e593e29fa8791ca16b_RGBA_4;
    float3 _Combine_14ef834db90841e593e29fa8791ca16b_RGB_5;
    float2 _Combine_14ef834db90841e593e29fa8791ca16b_RG_6;
    Unity_Combine_float(IN.TimeParameters.x, 0, 0, 0, _Combine_14ef834db90841e593e29fa8791ca16b_RGBA_4, _Combine_14ef834db90841e593e29fa8791ca16b_RGB_5, _Combine_14ef834db90841e593e29fa8791ca16b_RG_6);
    float2 _TilingAndOffset_d7947460ce004fdb8b6264458505125b_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_14ef834db90841e593e29fa8791ca16b_RG_6, _TilingAndOffset_d7947460ce004fdb8b6264458505125b_Out_3);
    float _GradientNoise_fd985463eec444be93224ee44886da0b_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_d7947460ce004fdb8b6264458505125b_Out_3, 5, _GradientNoise_fd985463eec444be93224ee44886da0b_Out_2);
    float4 _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0 = IN.uv0;
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_R_1 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[0];
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_G_2 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[1];
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_B_3 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[2];
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_A_4 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[3];
    float _Property_ba2a3f0e900c42e7a78943147136d880_Out_0 = _frequency;
    float _Multiply_69fd945d41774357a9d2d705570d0a71_Out_2;
    Unity_Multiply_float(_Split_919a80cc5b8046ea8de51fbad2a3f6a9_R_1, _Property_ba2a3f0e900c42e7a78943147136d880_Out_0, _Multiply_69fd945d41774357a9d2d705570d0a71_Out_2);
    float _Property_eba1580a62ad44e59cfffd69b0cd27d0_Out_0 = _WaveSpeed;
    float _Multiply_d9d2b6c80d3d470194bbe2d0594aa8da_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, _Property_eba1580a62ad44e59cfffd69b0cd27d0_Out_0, _Multiply_d9d2b6c80d3d470194bbe2d0594aa8da_Out_2);
    float _Add_fa66d2f88db14e01b7e676701a22c7da_Out_2;
    Unity_Add_float(_Multiply_69fd945d41774357a9d2d705570d0a71_Out_2, _Multiply_d9d2b6c80d3d470194bbe2d0594aa8da_Out_2, _Add_fa66d2f88db14e01b7e676701a22c7da_Out_2);
    float _Sine_ce2ac6e5f9a64be88565bbf06e23a934_Out_1;
    Unity_Sine_float(_Add_fa66d2f88db14e01b7e676701a22c7da_Out_2, _Sine_ce2ac6e5f9a64be88565bbf06e23a934_Out_1);
    float _Multiply_4e0dd23ffa1d4fb6aa15c44b53571808_Out_2;
    Unity_Multiply_float(_GradientNoise_fd985463eec444be93224ee44886da0b_Out_2, _Sine_ce2ac6e5f9a64be88565bbf06e23a934_Out_1, _Multiply_4e0dd23ffa1d4fb6aa15c44b53571808_Out_2);
    float _Property_9ac35708677b4631922e87eeb38d1b33_Out_0 = _WaveHeight;
    float _Multiply_ba1f56678f1a4af7b493a64492bc1d27_Out_2;
    Unity_Multiply_float(_Split_919a80cc5b8046ea8de51fbad2a3f6a9_G_2, _Property_9ac35708677b4631922e87eeb38d1b33_Out_0, _Multiply_ba1f56678f1a4af7b493a64492bc1d27_Out_2);
    float _Add_ece30c85f12c4e21b2460f9b3bffa059_Out_2;
    Unity_Add_float(_Multiply_4e0dd23ffa1d4fb6aa15c44b53571808_Out_2, _Multiply_ba1f56678f1a4af7b493a64492bc1d27_Out_2, _Add_ece30c85f12c4e21b2460f9b3bffa059_Out_2);
    float _Property_d4452a37c81a4e5db27b7682d12d47d4_Out_0 = _WaveOffset;
    float _Step_67602ca94b6d4ba28fb446b54cc6afac_Out_2;
    Unity_Step_float(_Add_ece30c85f12c4e21b2460f9b3bffa059_Out_2, _Property_d4452a37c81a4e5db27b7682d12d47d4_Out_0, _Step_67602ca94b6d4ba28fb446b54cc6afac_Out_2);
    UnityTexture2D _Property_9083188972664c73927901759cf5dab9_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9083188972664c73927901759cf5dab9_Out_0.tex, _Property_9083188972664c73927901759cf5dab9_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_R_4 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.r;
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_G_5 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.g;
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_B_6 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.b;
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_A_7 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.a;
    float _Multiply_5ea4d834adca4c739ebc2beec9dc3f40_Out_2;
    Unity_Multiply_float(_Step_67602ca94b6d4ba28fb446b54cc6afac_Out_2, _SampleTexture2D_19a425feb2a843a1978e7680880bb194_A_7, _Multiply_5ea4d834adca4c739ebc2beec9dc3f40_Out_2);
    surface.BaseColor = (_Blend_8a0cce33d63044d09eb0b9eccec9014b_Out_2.xyz);
    surface.Alpha = _Multiply_5ea4d834adca4c739ebc2beec9dc3f40_Out_2;
    surface.NormalTS = IN.TangentSpaceNormal;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



    output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


    output.uv0 = input.texCoord0;
    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteNormalPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "Sprite Forward"
    Tags
    {
        "LightMode" = "UniversalForward"
    }

        // Render State
        Cull Off
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
    #pragma exclude_renderers d3d11_9x
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEFORWARD
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        float4 uv0 : TEXCOORD0;
        float4 color : COLOR;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float4 texCoord0;
        float4 color;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float3 TangentSpaceNormal;
        float4 uv0;
        float3 TimeParameters;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float4 interp0 : TEXCOORD0;
        float4 interp1 : TEXCOORD1;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyzw = input.texCoord0;
        output.interp1.xyzw = input.color;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.texCoord0 = input.interp0.xyzw;
        output.color = input.interp1.xyzw;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float _frequency;
float _WaveSpeed;
float _WaveHeight;
float _WaveOffset;
float4 _Color;
CBUFFER_END

// Object and Global properties
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
float4 _MainTex_TexelSize;
SAMPLER(SamplerState_Linear_Repeat);

// Graph Functions

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Fraction_float(float In, out float Out)
{
    Out = frac(In);
}

void Unity_SampleGradient_float(Gradient Gradient, float Time, out float4 Out)
{
    float3 color = Gradient.colors[0].rgb;
    [unroll]
    for (int c = 1; c < 8; c++)
    {
        float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
        color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
    }
#ifndef UNITY_COLORSPACE_GAMMA
    color = SRGBToLinear(color);
#endif
    float alpha = Gradient.alphas[0].x;
    [unroll]
    for (int a = 1; a < 8; a++)
    {
        float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
        alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
    }
    Out = float4(color, alpha);
}

void Unity_Add_float4(float4 A, float4 B, out float4 Out)
{
    Out = A + B;
}

void Unity_OneMinus_float(float In, out float Out)
{
    Out = 1 - In;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}


float2 Unity_GradientNoise_Dir_float(float2 p)
{
    // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
    p = p % 289;
    // need full precision, otherwise half overflows when p > 1
    float x = float(34 * p.x + 1) * p.x % 289 + p.y;
    x = (34 * x + 1) * x % 289;
    x = frac(x / 41) * 2 - 1;
    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}

void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
{
    float2 p = UV * Scale;
    float2 ip = floor(p);
    float2 fp = frac(p);
    float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
    float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
    float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
    float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
}

void Unity_Blend_Screen_float(float Base, float Blend, out float Out, float Opacity)
{
    Out = 1.0 - (1.0 - Blend) * (1.0 - Base);
    Out = lerp(Base, Out, Opacity);
}

void Unity_Maximum_float(float A, float B, out float Out)
{
    Out = max(A, B);
}

void Unity_Blend_Divide_float4(float4 Base, float4 Blend, out float4 Out, float Opacity)
{
    Out = Base / (Blend + 0.000000000001);
    Out = lerp(Base, Out, Opacity);
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Sine_float(float In, out float Out)
{
    Out = sin(In);
}

void Unity_Step_float(float Edge, float In, out float Out)
{
    Out = step(Edge, In);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float3 NormalTS;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    Gradient _Gradient_dcfb27bd47c745ab892eaef1e55d14ad_Out_0 = NewGradient(0, 4, 2, float4(1, 0, 0, 0),float4(0, 1, 0, 0.3329976),float4(0, 0, 1, 0.6659952),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
    float _Multiply_a99a66b168cc412da3f5e8fae61294d6_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, 0.1, _Multiply_a99a66b168cc412da3f5e8fae61294d6_Out_2);
    float _Fraction_a64471f9119b4df3bdf23230f357381f_Out_1;
    Unity_Fraction_float(_Multiply_a99a66b168cc412da3f5e8fae61294d6_Out_2, _Fraction_a64471f9119b4df3bdf23230f357381f_Out_1);
    float4 _SampleGradient_304139d346b34fd8bfd90c323bfa4520_Out_2;
    Unity_SampleGradient_float(_Gradient_dcfb27bd47c745ab892eaef1e55d14ad_Out_0, _Fraction_a64471f9119b4df3bdf23230f357381f_Out_1, _SampleGradient_304139d346b34fd8bfd90c323bfa4520_Out_2);
    float4 _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0 = IN.uv0;
    float _Split_0f43ac5a5c904352be9acefe125fd048_R_1 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[0];
    float _Split_0f43ac5a5c904352be9acefe125fd048_G_2 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[1];
    float _Split_0f43ac5a5c904352be9acefe125fd048_B_3 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[2];
    float _Split_0f43ac5a5c904352be9acefe125fd048_A_4 = _UV_83a87905a2cb473cbeaaab490cf0022c_Out_0[3];
    float _Multiply_f49cd046518a4db7a094de40daf556f3_Out_2;
    Unity_Multiply_float(_Split_0f43ac5a5c904352be9acefe125fd048_G_2, 0.2, _Multiply_f49cd046518a4db7a094de40daf556f3_Out_2);
    float4 _Add_896e119c6cff4adf860349a3eccce683_Out_2;
    Unity_Add_float4(_SampleGradient_304139d346b34fd8bfd90c323bfa4520_Out_2, (_Multiply_f49cd046518a4db7a094de40daf556f3_Out_2.xxxx), _Add_896e119c6cff4adf860349a3eccce683_Out_2);
    float _OneMinus_d0326770ea144971a2f7dd60bad24e8c_Out_1;
    Unity_OneMinus_float(IN.TimeParameters.x, _OneMinus_d0326770ea144971a2f7dd60bad24e8c_Out_1);
    float2 _TilingAndOffset_1e6d7ab5e77f4a17a924ecdb2352f393_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_OneMinus_d0326770ea144971a2f7dd60bad24e8c_Out_1.xx), _TilingAndOffset_1e6d7ab5e77f4a17a924ecdb2352f393_Out_3);
    float _GradientNoise_c1f6a64254f14b26b04a168fd626a7c1_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_1e6d7ab5e77f4a17a924ecdb2352f393_Out_3, 2, _GradientNoise_c1f6a64254f14b26b04a168fd626a7c1_Out_2);
    float2 _TilingAndOffset_5716e7a66976403a9f903a1bbc496b31_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (IN.TimeParameters.x.xx), _TilingAndOffset_5716e7a66976403a9f903a1bbc496b31_Out_3);
    float _GradientNoise_8906cfaf61e4471997e0ac71165d8d52_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_5716e7a66976403a9f903a1bbc496b31_Out_3, 2, _GradientNoise_8906cfaf61e4471997e0ac71165d8d52_Out_2);
    float _Blend_990059ca2a794bd8ae051d6d23de0100_Out_2;
    Unity_Blend_Screen_float(_GradientNoise_c1f6a64254f14b26b04a168fd626a7c1_Out_2, _GradientNoise_8906cfaf61e4471997e0ac71165d8d52_Out_2, _Blend_990059ca2a794bd8ae051d6d23de0100_Out_2, 1);
    float _Maximum_0978eb690efd43dbb2e00867157c0c07_Out_2;
    Unity_Maximum_float(_Blend_990059ca2a794bd8ae051d6d23de0100_Out_2, 0.3, _Maximum_0978eb690efd43dbb2e00867157c0c07_Out_2);
    float4 _Blend_8a0cce33d63044d09eb0b9eccec9014b_Out_2;
    Unity_Blend_Divide_float4(_Add_896e119c6cff4adf860349a3eccce683_Out_2, (_Maximum_0978eb690efd43dbb2e00867157c0c07_Out_2.xxxx), _Blend_8a0cce33d63044d09eb0b9eccec9014b_Out_2, 1);
    float4 _Combine_14ef834db90841e593e29fa8791ca16b_RGBA_4;
    float3 _Combine_14ef834db90841e593e29fa8791ca16b_RGB_5;
    float2 _Combine_14ef834db90841e593e29fa8791ca16b_RG_6;
    Unity_Combine_float(IN.TimeParameters.x, 0, 0, 0, _Combine_14ef834db90841e593e29fa8791ca16b_RGBA_4, _Combine_14ef834db90841e593e29fa8791ca16b_RGB_5, _Combine_14ef834db90841e593e29fa8791ca16b_RG_6);
    float2 _TilingAndOffset_d7947460ce004fdb8b6264458505125b_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_14ef834db90841e593e29fa8791ca16b_RG_6, _TilingAndOffset_d7947460ce004fdb8b6264458505125b_Out_3);
    float _GradientNoise_fd985463eec444be93224ee44886da0b_Out_2;
    Unity_GradientNoise_float(_TilingAndOffset_d7947460ce004fdb8b6264458505125b_Out_3, 5, _GradientNoise_fd985463eec444be93224ee44886da0b_Out_2);
    float4 _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0 = IN.uv0;
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_R_1 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[0];
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_G_2 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[1];
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_B_3 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[2];
    float _Split_919a80cc5b8046ea8de51fbad2a3f6a9_A_4 = _UV_bc3840b2abe946daaf0d29f72f7a867e_Out_0[3];
    float _Property_ba2a3f0e900c42e7a78943147136d880_Out_0 = _frequency;
    float _Multiply_69fd945d41774357a9d2d705570d0a71_Out_2;
    Unity_Multiply_float(_Split_919a80cc5b8046ea8de51fbad2a3f6a9_R_1, _Property_ba2a3f0e900c42e7a78943147136d880_Out_0, _Multiply_69fd945d41774357a9d2d705570d0a71_Out_2);
    float _Property_eba1580a62ad44e59cfffd69b0cd27d0_Out_0 = _WaveSpeed;
    float _Multiply_d9d2b6c80d3d470194bbe2d0594aa8da_Out_2;
    Unity_Multiply_float(IN.TimeParameters.x, _Property_eba1580a62ad44e59cfffd69b0cd27d0_Out_0, _Multiply_d9d2b6c80d3d470194bbe2d0594aa8da_Out_2);
    float _Add_fa66d2f88db14e01b7e676701a22c7da_Out_2;
    Unity_Add_float(_Multiply_69fd945d41774357a9d2d705570d0a71_Out_2, _Multiply_d9d2b6c80d3d470194bbe2d0594aa8da_Out_2, _Add_fa66d2f88db14e01b7e676701a22c7da_Out_2);
    float _Sine_ce2ac6e5f9a64be88565bbf06e23a934_Out_1;
    Unity_Sine_float(_Add_fa66d2f88db14e01b7e676701a22c7da_Out_2, _Sine_ce2ac6e5f9a64be88565bbf06e23a934_Out_1);
    float _Multiply_4e0dd23ffa1d4fb6aa15c44b53571808_Out_2;
    Unity_Multiply_float(_GradientNoise_fd985463eec444be93224ee44886da0b_Out_2, _Sine_ce2ac6e5f9a64be88565bbf06e23a934_Out_1, _Multiply_4e0dd23ffa1d4fb6aa15c44b53571808_Out_2);
    float _Property_9ac35708677b4631922e87eeb38d1b33_Out_0 = _WaveHeight;
    float _Multiply_ba1f56678f1a4af7b493a64492bc1d27_Out_2;
    Unity_Multiply_float(_Split_919a80cc5b8046ea8de51fbad2a3f6a9_G_2, _Property_9ac35708677b4631922e87eeb38d1b33_Out_0, _Multiply_ba1f56678f1a4af7b493a64492bc1d27_Out_2);
    float _Add_ece30c85f12c4e21b2460f9b3bffa059_Out_2;
    Unity_Add_float(_Multiply_4e0dd23ffa1d4fb6aa15c44b53571808_Out_2, _Multiply_ba1f56678f1a4af7b493a64492bc1d27_Out_2, _Add_ece30c85f12c4e21b2460f9b3bffa059_Out_2);
    float _Property_d4452a37c81a4e5db27b7682d12d47d4_Out_0 = _WaveOffset;
    float _Step_67602ca94b6d4ba28fb446b54cc6afac_Out_2;
    Unity_Step_float(_Add_ece30c85f12c4e21b2460f9b3bffa059_Out_2, _Property_d4452a37c81a4e5db27b7682d12d47d4_Out_0, _Step_67602ca94b6d4ba28fb446b54cc6afac_Out_2);
    UnityTexture2D _Property_9083188972664c73927901759cf5dab9_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9083188972664c73927901759cf5dab9_Out_0.tex, _Property_9083188972664c73927901759cf5dab9_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_R_4 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.r;
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_G_5 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.g;
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_B_6 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.b;
    float _SampleTexture2D_19a425feb2a843a1978e7680880bb194_A_7 = _SampleTexture2D_19a425feb2a843a1978e7680880bb194_RGBA_0.a;
    float _Multiply_5ea4d834adca4c739ebc2beec9dc3f40_Out_2;
    Unity_Multiply_float(_Step_67602ca94b6d4ba28fb446b54cc6afac_Out_2, _SampleTexture2D_19a425feb2a843a1978e7680880bb194_A_7, _Multiply_5ea4d834adca4c739ebc2beec9dc3f40_Out_2);
    surface.BaseColor = (_Blend_8a0cce33d63044d09eb0b9eccec9014b_Out_2.xyz);
    surface.Alpha = _Multiply_5ea4d834adca4c739ebc2beec9dc3f40_Out_2;
    surface.NormalTS = IN.TangentSpaceNormal;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



    output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


    output.uv0 = input.texCoord0;
    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteForwardPass.hlsl"

    ENDHLSL
}
        }
            FallBack "Hidden/Shader Graph/FallbackError"
}