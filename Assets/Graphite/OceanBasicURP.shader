Shader "Iska/URP/OceanSimple"
{
    Properties
    {
        _BaseColor   ("Base Color", Color) = (0.02, 0.18, 0.35, 0.85)
        _FresnelPower("Fresnel Power", Range(0.5, 8)) = 4
        _FresnelStr  ("Fresnel Strength", Range(0, 2)) = 0.6

        _NormalMap   ("Normal Map", 2D) = "bump" {}
        _NormalScale ("Normal Strength", Range(0, 2)) = 0.6
        _NormalSpeed ("Normal Scroll Speed", Vector) = (0.05, -0.03, -0.04, 0.02)

        _WaveDir1 ("Wave1 Dir (xz)", Vector) = (1, 0, 0, 0)
        _WaveLen1 ("Wave1 Length (m)", Range(0.5, 100)) = 12
        _WaveAmp1 ("Wave1 Amp (m)", Range(0, 2)) = 0.15
        _WaveSpd1 ("Wave1 Speed (m/s)", Range(0, 10)) = 1.2

        _WaveDir2 ("Wave2 Dir (xz)", Vector) = (0.6, 0, 0.8, 0)
        _WaveLen2 ("Wave2 Length (m)", Range(0.5, 100)) = 7
        _WaveAmp2 ("Wave2 Amp (m)", Range(0, 2)) = 0.08
        _WaveSpd2 ("Wave2 Speed (m/s)", Range(0, 10)) = 0.8
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            Name "OceanSimple"
            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target 3.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_NormalMap); SAMPLER(sampler_NormalMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float  _FresnelPower;
                float  _FresnelStr;

                float4 _NormalMap_ST;
                float  _NormalScale;
                float4 _NormalSpeed;

                float4 _WaveDir1; float _WaveLen1; float _WaveAmp1; float _WaveSpd1;
                float4 _WaveDir2; float _WaveLen2; float _WaveAmp2; float _WaveSpd2;
            CBUFFER_END

            struct Attributes { float4 positionOS:POSITION; float3 normalOS:NORMAL; float4 tangentOS:TANGENT; float2 uv:TEXCOORD0; };
            struct Varyings { float4 positionCS:SV_POSITION; float3 positionWS:TEXCOORD0; float3 normalWS:TEXCOORD1; float3 tangentWS:TEXCOORD2; float3 bitanWS:TEXCOORD3; float2 uv0:TEXCOORD4; };

            float gerstner(float2 dirXZ,float len,float amp,float spd,float t,float2 xz){
                float2 k = normalize(dirXZ) * (2.0*PI/max(0.001,len));
                return amp * sin(dot(k,xz) + spd*t);
            }

            Varyings vert(Attributes v){
                Varyings o;
                float3 posWS = TransformObjectToWorld(v.positionOS.xyz);
                float t = _Time.y*20.0;
                float h = gerstner(_WaveDir1.xz,_WaveLen1,_WaveAmp1,_WaveSpd1,t,posWS.xz)
                        + gerstner(_WaveDir2.xz,_WaveLen2,_WaveAmp2,_WaveSpd2,t,posWS.xz);
                posWS.y += h;

                o.positionCS = TransformWorldToHClip(posWS);
                o.positionWS = posWS;

                float3 nWS = TransformObjectToWorldNormal(v.normalOS);
                float3 tWS = TransformObjectToWorldDir(v.tangentOS.xyz);
                float  sign = v.tangentOS.w * GetOddNegativeScale();
                float3 bWS = cross(nWS,tWS)*sign;

                o.normalWS = normalize(nWS);
                o.tangentWS= normalize(tWS);
                o.bitanWS  = normalize(bWS);
                o.uv0 = TRANSFORM_TEX(v.uv,_NormalMap);
                return o;
            }

            float3 normalFromMapWS(float2 uv,float3 nWS,float3 tWS,float3 bWS){
                float2 uv1 = uv + _NormalSpeed.xy*(_Time.y*20.0);
                float2 uv2 = uv + _NormalSpeed.zw*(_Time.y*20.0);
                float3 n1 = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap,sampler_NormalMap,uv1));
                float3 n2 = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap,sampler_NormalMap,uv2));
                float3 nT = normalize(float3(n1.xy+n2.xy, n1.z*n2.z));
                nT.xy *= _NormalScale;
                float3x3 TBN = float3x3(tWS,bWS,nWS);
                return normalize(mul(nT,TBN));
            }

            half4 frag(Varyings i):SV_Target{
                float3 viewWS = normalize(_WorldSpaceCameraPos - i.positionWS);
                float3 nWS    = normalFromMapWS(i.uv0, normalize(i.normalWS), i.tangentWS, i.bitanWS);
                float f = pow(saturate(1.0 - dot(nWS,viewWS)), _FresnelPower) * _FresnelStr;
                float3 col = _BaseColor.rgb + f.xxx;
                return half4(col, _BaseColor.a);
            }
            ENDHLSL
        }
    }
}
