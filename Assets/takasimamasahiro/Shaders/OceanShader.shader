Shader "Custom/URP/OceanShader"
{
    Properties
    {
        [Header(Wave Settings)]
        _WaveSpeed("Wave Speed", Float) = 1.0
        _WaveHeight("Wave Height", Float) = 0.1
        _WaveFrequency("Wave Frequency", Float) = 2.0
        _WaveDirection("Wave Direction", Vector) = (1, 0, 1, 0)
        
        [Header(Color Settings)]
        _ShallowColor("Shallow Water Color", Color) = (0.4, 0.8, 1.0, 0.8)
        _DeepColor("Deep Water Color", Color) = (0.1, 0.3, 0.6, 1.0)
        _FresnelPower("Fresnel Power", Range(0.1, 10.0)) = 2.0
        
        [Header(Surface Properties)]
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.95
        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _NormalStrength("Normal Strength", Range(0.0, 2.0)) = 1.0
        
        [Header(Textures)]
        _NormalMap("Normal Map", 2D) = "bump" {}
        _FoamTexture("Foam Texture", 2D) = "white" {}
        
        [Header(Foam Settings)]
        _FoamAmount("Foam Amount", Range(0.0, 1.0)) = 0.5
        _FoamCutoff("Foam Cutoff", Range(0.0, 1.0)) = 0.8
        
        [Header(Animation)]
        _NormalSpeed("Normal Animation Speed", Float) = 0.2
        _SecondaryWaveSpeed("Secondary Wave Speed", Float) = 0.7
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
        }
        
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // URP required keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile_fog
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 texcoord     : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS               : SV_POSITION;
                float2 uv                       : TEXCOORD0;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
                float3 positionWS               : TEXCOORD2;
                float3 normalWS                 : TEXCOORD3;
                float3 viewDirWS                : TEXCOORD4;
                half4 fogFactorAndVertexLight   : TEXCOORD5;
                float4 shadowCoord              : TEXCOORD6;
                float4 screenPos                : TEXCOORD7;
                float waveHeight                : TEXCOORD8;
            };

            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            TEXTURE2D(_FoamTexture);
            SAMPLER(sampler_FoamTexture);

            CBUFFER_START(UnityPerMaterial)
                // Wave properties
                float _WaveSpeed;
                float _WaveHeight;
                float _WaveFrequency;
                float4 _WaveDirection;
                float _SecondaryWaveSpeed;
                
                // Color properties
                half4 _ShallowColor;
                half4 _DeepColor;
                float _FresnelPower;
                
                // Surface properties
                float _Smoothness;
                float _Metallic;
                float _NormalStrength;
                
                // Texture properties
                float4 _NormalMap_ST;
                float4 _FoamTexture_ST;
                
                // Foam properties
                float _FoamAmount;
                float _FoamCutoff;
                
                // Animation properties
                float _NormalSpeed;
            CBUFFER_END

            // Wave function using Gerstner waves
            float3 GerstnerWave(float3 position, float amplitude, float frequency, float2 direction, float speed, float time)
            {
                float phase = dot(direction, position.xz) * frequency + time * speed;
                
                float3 wave;
                wave.x = direction.x * amplitude * sin(phase);
                wave.z = direction.y * amplitude * sin(phase);
                wave.y = amplitude * cos(phase);
                
                return wave;
            }

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                // Get time
                float time = _Time.y;
                
                // Get world position first (before wave modification)
                float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);
                
                // Calculate multiple waves using world coordinates
                float3 waveOffset = float3(0, 0, 0);
                
                // Primary wave - using world XZ coordinates
                float2 dir1 = normalize(_WaveDirection.xy);
                waveOffset += GerstnerWave(worldPos, _WaveHeight, _WaveFrequency, dir1, _WaveSpeed, time);
                
                // Secondary wave - using world XZ coordinates
                float2 dir2 = normalize(float2(_WaveDirection.z, _WaveDirection.w));
                waveOffset += GerstnerWave(worldPos, _WaveHeight * 0.5, _WaveFrequency * 0.8, dir2, _SecondaryWaveSpeed, time);
                
                // Apply wave offset directly in world space, then convert back
                worldPos += waveOffset;
                input.positionOS = mul(unity_WorldToObject, float4(worldPos, 1.0));
                
                // Standard vertex transformations
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                output.uv = TRANSFORM_TEX(input.texcoord, _NormalMap);
                output.positionWS = vertexInput.positionWS;
                output.positionCS = vertexInput.positionCS;
                output.normalWS = normalInput.normalWS;
                output.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
                output.screenPos = ComputeScreenPos(vertexInput.positionCS);
                output.waveHeight = length(waveOffset);

                // Lighting and fog
                half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
                half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
                output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

                OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
                OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

                output.shadowCoord = GetShadowCoord(vertexInput);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // Sample normal map with animation
                float2 normalUV1 = input.uv + _Time.y * _NormalSpeed * float2(0.1, 0.1);
                float2 normalUV2 = input.uv + _Time.y * _NormalSpeed * float2(-0.05, 0.08);
                
                half3 normal1 = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, normalUV1));
                half3 normal2 = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, normalUV2));
                half3 normal = normalize(normal1 + normal2) * _NormalStrength;

                // Prepare surface data
                SurfaceData surfaceData = (SurfaceData)0;
                
                // Calculate fresnel effect
                float3 viewDir = normalize(input.viewDirWS);
                float3 worldNormal = normalize(input.normalWS + normal);
                float fresnel = pow(1.0 - saturate(dot(worldNormal, viewDir)), _FresnelPower);

                // Calculate depth-based color
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                float depth = LinearEyeDepth(SampleSceneDepth(screenUV), _ZBufferParams);
                float surfaceDepth = LinearEyeDepth(input.screenPos.z / input.screenPos.w, _ZBufferParams);
                float waterDepth = saturate((depth - surfaceDepth) * 0.1);

                // Mix shallow and deep colors
                half4 waterColor = lerp(_ShallowColor, _DeepColor, waterDepth);

                // Add foam
                float foamMask = saturate(input.waveHeight * _FoamAmount);
                foamMask = step(_FoamCutoff, foamMask);
                half3 foam = SAMPLE_TEXTURE2D(_FoamTexture, sampler_FoamTexture, input.uv * 4).rgb;
                waterColor.rgb = lerp(waterColor.rgb, foam, foamMask);

                // Set surface properties
                surfaceData.albedo = waterColor.rgb;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness;
                surfaceData.normalTS = normal;
                surfaceData.alpha = lerp(waterColor.a, 1.0, fresnel * 0.5);

                // Lighting calculation
                InputData inputData = (InputData)0;
                inputData.positionWS = input.positionWS;
                inputData.normalWS = worldNormal;
                inputData.viewDirectionWS = viewDir;
                inputData.shadowCoord = input.shadowCoord;
                inputData.fogCoord = input.fogFactorAndVertexLight.x;
                inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
                inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);

                half4 color = UniversalFragmentPBR(inputData, surfaceData);
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                
                return color;
            }
            ENDHLSL
        }
    }
    
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
