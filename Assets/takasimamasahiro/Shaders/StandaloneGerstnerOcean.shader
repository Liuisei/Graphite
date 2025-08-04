Properties
    {
        [Header(Realistic Wave System)]
        [Space(10)]
        _LargeWave1("Large Wave 1 (Amp, Freq, Speed, Steep)", Vector) = (1.2, 0.3, 0.8, 0.95)
        _LargeWave1Dir("Large Wave 1 Direction", Vector) = (1, 0, 0, 0)
        
        _LargeWave2("Large Wave 2 (Amp, Freq, Speed, Steep)", Vector) = (0.9, 0.35, 0.6, 0.9)
        _LargeWave2Dir("Large Wave 2 Direction", Vector) = (0.6, 0.8, 0, 0)
        
        _MediumWave1("Medium Wave 1 (Amp, Freq, Speed, Steep)", Vector) = (0.6, 0.8, 1.2, 0.8)
        _MediumWave1Dir("Medium Wave 1 Direction", Vector) = (-0.3, 0.95, 0, 0)
        
        _MediumWave2("Medium Wave 2 (Amp, Freq, Speed, Steep)", Vector) = (0.4, 1.1, 1.0, 0.7)
        _MediumWave2Dir("Medium Wave 2 Direction", Vector) = (0.8, -0.6, 0, 0)
        
        _SmallWave1("Small Wave 1 (Amp, Freq, Speed, Steep)", Vector) = (0.25, 2.2, 1.8, 0.6)
        _SmallWave1Dir("Small Wave 1 Direction", Vector) = (0.4, 0.9, 0, 0)
        
        _SmallWave2("Small Wave 2 (Amp, Freq, Speed, Steep)", Vector) = (0.15, 3.5, 2.2, 0.5)
        _SmallWave2Dir("Small Wave 2 Direction", Vector) = (-0.7, 0.7, 0, 0)
        
        _DetailWave1("Detail Wave 1 (Amp, Freq, Speed, Steep)", Vector) = (0.08, 6.0, 3.0, 0.4)
        _DetailWave1Dir("Detail Wave 1 Direction", Vector) = (0.9, 0.44, 0, 0)
        
        _DetailWave2("Detail Wave 2 (Amp, Freq, Speed, Steep)", Vector) = (0.05, 9.5, 4.5, 0.3)
        _DetailWave2Dir("Detail Wave 2 Direction", Vector) = (-0.5, 0.87, 0, 0)
        
        [Header(Wave Interaction)]
        _WaveInteraction("Wave Interaction Strength", Range(0.0, 2.0)) = 1.2
        _WaveBlending("Wave Blending Factor", Range(0.0, 2.0)) = 1.0
        _CrossWaveStrength("Cross Wave Strength", Range(0.0, 1.0)) = 0.6
        
        [Header(Global Wave Control)]
        _GlobalAmplitude("Global Amplitude", Range(0.0, 3.0)) = 1.0
        _GlobalSpeed("Global Speed", Range(0.0, 3.0)) = 1.0
        _GlobalSteepness("Global Steepness", Range(0.0, 2.0)) = 1.0
        _WindStrength("Wind Strength", Range(0.0, 2.0)) = 1.0
        _WindDirection("Wind Direction", Vector) = (1, 0, 0, 0)
        
        [Header(Advanced Wave Control)]
        _WaveControlMap("Wave Control Map (R:Height G:Chop B:Wind)", 2D) = "gray" {}
        _ControlMapScale("Control Map Scale", Float) = 0.1
        _ControlMapStrength("Control Map Influence", Range(0.0, 2.0)) = 1.0
        
        [Header(Scale Adaptation)]
        _ScaleCompensation("Scale Compensation", Range(0.0, 2.0)) = 1.0
        _WorldSpaceWaves("Use World Space Waves", Range(0.0, 1.0)) = 1.0
        _TilingScale("Texture Tiling Scale", Float) = 1.0
        
        [Header(Realistic Ocean Colors)]
        _DeepWaterColor("Deep Water Color", Color) = (0.02, 0.08, 0.15, 1.0)
        _ShallowWaterColor("Shallow Water Color", Color) = (0.1, 0.3, 0.4, 0.9)
        _SurfaceColor("Surface Highlight", Color) = (0.3, 0.6, 0.8, 0.8)
        _FoamColor("Foam Color", Color) = (0.9, 0.95, 1.0, 1.0)
        _SubsurfaceColor("Subsurface Color", Color) = (0.0, 0.2, 0.3, 1.0)
        
        [Header(Advanced Optics)]
        _FresnelStrength("Fresnel Strength", Range(0.0, 3.0)) = 1.8
        _FresnelPower("Fresnel Power", Range(0.1, 10.0)) = 5.0
        _FresnelBias("Fresnel Bias", Range(0.0, 0.3)) = 0.02
        _Transparency("Transparency", Range(0.0, 1.0)) = 0.9
        _RefractionStrength("Refraction Strength", Range(0.0, 0.15)) = 0.05
        _Scattering("Scattering Strength", Range(0.0, 2.0)) = 0.8
        
        [Header(Surface Detail)]
        _Roughness("Surface Roughness", Range(0.0, 0.3)) = 0.01
        _Metallic("Metallic", Range(0.0, 0.1)) = 0.0
        _NormalStrength("Normal Strength", Range(0.0, 5.0)) = 3.0
        _DetailNormalStrength("Detail Normal Strength", Range(0.0, 3.0)) = 2.0
        _MicroDetailStrength("Micro Detail Strength", Range(0.0, 1.0)) = 0.8
        
        [Header(Surface Textures)]
        _NormalMap("Wave Normal Map", 2D) = "bump" {}
        _DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
        _MicroNormalMap("Micro Normal Map", 2D) = "bump" {}
        _FoamTexture("Foam Texture", 2D) = "white" {}
        
        [Header(Advanced Foam System)]
        _FoamAmount("Foam Amount", Range(0.0, 4.0)) = 2.0
        _FoamCutoff("Foam Cutoff", Range(0.0, 1.0)) = 0.4
        _FoamSoftness("Foam Softness", Range(0.01, 0.8)) = 0.2
        _FoamPersistence("Foam Persistence", Range(0.0, 2.0)) = 1.2
        _FoamSpeed("Foam Animation Speed", Float) = 1.5
        _FoamScale("Foam Scale", Float) = 8.0
        
        [Header(Lighting Enhancement)]
        _SpecularStrength("Specular Strength", Range(0.0, 3.0)) = 2.0
        _SpecularPower("Specular Power", Range(1.0, 512.0)) = 128.0
        _SubsurfaceStrength("Subsurface Scattering", Range(0.0, 3.0)) = 1.2
        _RimLighting("Rim Lighting", Range(0.0, 2.0)) = 0.8
        
        [Header(Animation Speeds)]
        _NormalSpeed("Normal Speed", Float) = 0.05
        _DetailNormalSpeed("Detail Normal Speed", Float) = 0.2
        _MicroNormalSpeed("Micro Normal Speed", Float) = 0.8
        
        [Header(Depth Effects)]
        _DepthFade("Depth Fade Distance", Range(0.1, 50.0)) = 15.0
        _ShallowDepth("Shallow Water Depth", Range(0.1, 8.0)) = 3.0
        _DepthDarkening("Depth Darkening", Range(0.0, 3.0)) = 1.8
        _DepthScattering("Depth Scattering", Range(0.0, 2.0)) = 1.0
        
        [Header(Time Control)]
        _TimeScale("Time Scale", Float) = 1.0
        _TimeOffset("Time Offset", Float) = 0.0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
        }
        
        LOD 500
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
                float4 waveData                 : TEXCOORD8;
                float3 worldTangent             : TEXCOORD9;
                float3 worldBitangent           : TEXCOORD10;
                float4 waveInteraction          : TEXCOORD11;
            };

            TEXTURE2D(_WaveControlMap);
            SAMPLER(sampler_WaveControlMap);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            TEXTURE2D(_DetailNormalMap);
            SAMPLER(sampler_DetailNormalMap);
            TEXTURE2D(_MicroNormalMap);
            SAMPLER(sampler_MicroNormalMap);
            TEXTURE2D(_FoamTexture);
            SAMPLER(sampler_FoamTexture);

            CBUFFER_START(UnityPerMaterial)
                // Realistic wave parameters
                float4 _LargeWave1, _LargeWave1Dir;
                float4 _LargeWave2, _LargeWave2Dir;
                float4 _MediumWave1, _MediumWave1Dir;
                float4 _MediumWave2, _MediumWave2Dir;
                float4 _SmallWave1, _SmallWave1Dir;
                float4 _SmallWave2, _SmallWave2Dir;
                float4 _DetailWave1, _DetailWave1Dir;
                float4 _DetailWave2, _DetailWave2Dir;
                
                // Wave interaction
                float _WaveInteraction, _WaveBlending, _CrossWaveStrength;
                
                // Global controls
                float _GlobalAmplitude, _GlobalSpeed, _GlobalSteepness, _WindStrength;
                float4 _WindDirection;
                
                // Control map
                float4 _WaveControlMap_ST;
                float _ControlMapScale, _ControlMapStrength;
                
                // Scale adaptation
                float _ScaleCompensation, _WorldSpaceWaves, _TilingScale;
                
                // Colors
                half4 _DeepWaterColor, _ShallowWaterColor, _SurfaceColor, _FoamColor, _SubsurfaceColor;
                
                // Optics
                float _FresnelStrength, _FresnelPower, _FresnelBias, _Transparency, _RefractionStrength, _Scattering;
                
                // Surface
                float _Roughness, _Metallic;
                float _NormalStrength, _DetailNormalStrength, _MicroDetailStrength;
                
                // Textures
                float4 _NormalMap_ST, _DetailNormalMap_ST, _MicroNormalMap_ST, _FoamTexture_ST;
                
                // Foam
                float _FoamAmount, _FoamCutoff, _FoamSoftness, _FoamPersistence, _FoamSpeed, _FoamScale;
                
                // Lighting
                float _SpecularStrength, _SpecularPower, _SubsurfaceStrength, _RimLighting;
                
                // Animation
                float _NormalSpeed, _DetailNormalSpeed, _MicroNormalSpeed;
                
                // Depth
                float _DepthFade, _ShallowDepth, _DepthDarkening, _DepthScattering;
                
                // Time
                float _TimeScale, _TimeOffset;
            CBUFFER_END

            // リアルなGerstner波計算（風の影響含む）
            float4 CalculateRealisticGerstnerWave(float3 pos, float4 waveParams, float2 direction, float time, float4 control, float3 objectScale, float windInfluence)
            {
                float amplitude = waveParams.x * _GlobalAmplitude;
                float frequency = waveParams.y;
                float speed = waveParams.z * _GlobalSpeed;
                float steepness = waveParams.w * _GlobalSteepness;
                
                // スケール補正
                float avgScale = (objectScale.x + objectScale.z) * 0.5;
                amplitude *= lerp(1.0, avgScale * _ScaleCompensation, 0.8);
                frequency *= lerp(1.0, 1.0 / avgScale, 0.6);
                
                // コントロールマップとの相互作用
                amplitude *= lerp(1.0, control.r * 1.5, _ControlMapStrength);
                steepness *= lerp(1.0, control.g * 1.2, _ControlMapStrength);
                
                // 風の影響
                float2 windDir = normalize(_WindDirection.xy);
                float windAlignment = dot(direction, windDir);
                amplitude *= 1.0 + windAlignment * _WindStrength * windInfluence * 0.3;
                speed *= 1.0 + windAlignment * _WindStrength * windInfluence * 0.2;
                
                // 位相計算
                float phase = dot(direction, pos.xz) * frequency + time * speed;
                
                // 非線形波形のための調整
                float sharpness = steepness / max(frequency * amplitude, 0.001);
                sharpness *= 1.0 + windInfluence * 0.2;
                
                float sinPhase = sin(phase);
                float cosPhase = cos(phase);
                
                // より現実的な波形
                float nonlinearFactor = 1.0 + steepness * 0.5;
                cosPhase = pow(abs(cosPhase), nonlinearFactor) * sign(cosPhase);
                
                float3 wave = float3(0, 0, 0);
                wave.x = direction.x * amplitude * sharpness * sinPhase;
                wave.z = direction.y * amplitude * sharpness * sinPhase;
                wave.y = amplitude * cosPhase;
                
                float slope = amplitude * frequency * sin(phase) * nonlinearFactor;
                return float4(wave, slope);
            }

            // 波の相互作用計算
            float CalculateWaveInteraction(float3 pos, float time)
            {
                float interaction = 0.0;
                
                // 大きな波同士の相互作用
                float2 dir1 = normalize(_LargeWave1Dir.xy);
                float2 dir2 = normalize(_LargeWave2Dir.xy);
                float phase1 = dot(dir1, pos.xz) * _LargeWave1.y + time * _LargeWave1.z;
                float phase2 = dot(dir2, pos.xz) * _LargeWave2.y + time * _LargeWave2.z;
                
                // 波の干渉パターン
                float interference = sin(phase1) * sin(phase2) * _WaveInteraction;
                
                // クロス波効果
                float crossPhase = dot(normalize(dir1 + dir2), pos.xz) * 1.5 + time * 0.8;
                float crossWave = sin(crossPhase) * _CrossWaveStrength;
                
                return interference + crossWave;
            }

            // コントロールマップをサンプリング
            float4 SampleWaveControl(float3 worldPos, float time, float3 objectScale)
            {
                float2 controlUV;
                if (_WorldSpaceWaves > 0.5)
                {
                    controlUV = worldPos.xz * _ControlMapScale;
                }
                else
                {
                    float avgScale = (objectScale.x + objectScale.z) * 0.5;
                    controlUV = worldPos.xz * _ControlMapScale / avgScale;
                }
                
                // 時間による変化
                float2 animOffset = float2(
                    sin(time * 0.02) * 0.01,
                    cos(time * 0.03) * 0.01
                );
                controlUV += animOffset;
                
                return SAMPLE_TEXTURE2D_LOD(_WaveControlMap, sampler_WaveControlMap, controlUV, 0);
            }

            Varyings vert(Attributes input)
            {
                Varyings output;
                ZERO_INITIALIZE(Varyings, output);

                float time = (_Time.y + _TimeOffset) * _TimeScale;
                float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);
                
                // オブジェクトスケール取得
                float3 objectScale = float3(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22)
                );
                
                // コントロールマップサンプリング
                float4 waveControl = SampleWaveControl(worldPos, time, objectScale);
                
                // 波の相互作用計算
                float waveInteractionValue = CalculateWaveInteraction(worldPos, time);
                
                // 全ての波を計算して合成
                float3 totalWaveOffset = float3(0, 0, 0);
                float totalSlope = 0;
                float waveIntensity = 0;
                
                // 大きな波（主要）
                float4 largeWave1 = CalculateRealisticGerstnerWave(worldPos, _LargeWave1, _LargeWave1Dir.xy, time, waveControl, objectScale, 1.0);
                float4 largeWave2 = CalculateRealisticGerstnerWave(worldPos, _LargeWave2, _LargeWave2Dir.xy, time, waveControl, objectScale, 0.9);
                
                // 中程度の波
                float4 mediumWave1 = CalculateRealisticGerstnerWave(worldPos, _MediumWave1, _MediumWave1Dir.xy, time, waveControl, objectScale, 0.8);
                float4 mediumWave2 = CalculateRealisticGerstnerWave(worldPos, _MediumWave2, _MediumWave2Dir.xy, time, waveControl, objectScale, 0.7);
                
                // 小さな波
                float4 smallWave1 = CalculateRealisticGerstnerWave(worldPos, _SmallWave1, _SmallWave1Dir.xy, time, waveControl, objectScale, 0.6);
                float4 smallWave2 = CalculateRealisticGerstnerWave(worldPos, _SmallWave2, _SmallWave2Dir.xy, time, waveControl, objectScale, 0.5);
                
                // 詳細波
                float4 detailWave1 = CalculateRealisticGerstnerWave(worldPos, _DetailWave1, _DetailWave1Dir.xy, time, waveControl, objectScale, 0.4);
                float4 detailWave2 = CalculateRealisticGerstnerWave(worldPos, _DetailWave2, _DetailWave2Dir.xy, time, waveControl, objectScale, 0.3);
                
                // 波の合成（重み付き）
                totalWaveOffset += largeWave1.xyz * 1.0;
                totalWaveOffset += largeWave2.xyz * 0.9;
                totalWaveOffset += mediumWave1.xyz * 0.7;
                totalWaveOffset += mediumWave2.xyz * 0.6;
                totalWaveOffset += smallWave1.xyz * 0.4;
                totalWaveOffset += smallWave2.xyz * 0.3;
                totalWaveOffset += detailWave1.xyz * 0.2;
                totalWaveOffset += detailWave2.xyz * 0.15;
                
                // 波の相互作用を追加
                totalWaveOffset.y += waveInteractionValue * 0.1;
                
                totalSlope = largeWave1.w + largeWave2.w * 0.9 + mediumWave1.w * 0.7 + mediumWave2.w * 0.6;
                waveIntensity = length(totalWaveOffset);
                
                // ワールド位置更新
                worldPos += totalWaveOffset;
                input.positionOS = mul(unity_WorldToObject, float4(worldPos, 1.0));
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                output.uv = TRANSFORM_TEX(input.texcoord, _NormalMap);
                output.positionWS = vertexInput.positionWS;
                output.positionCS = vertexInput.positionCS;
                output.normalWS = normalInput.normalWS;
                output.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
                output.screenPos = ComputeScreenPos(vertexInput.positionCS);
                
                // 波データ保存
                output.waveData.x = waveIntensity;
                output.waveData.y = totalSlope;
                output.waveData.z = saturate(totalSlope * 1.5 * (1.0 + waveControl.g * 0.5));
                output.waveData.w = waveControl.g;
                
                // 波の相互作用データ
                output.waveInteraction.x = waveInteractionValue;
                output.waveInteraction.y = waveControl.r;
                output.waveInteraction.z = waveControl.b;
                output.waveInteraction.w = totalSlope;
                
                // タンジェント空間
                output.worldTangent = normalInput.tangentWS;
                output.worldBitangent = cross(normalInput.normalWS, normalInput.tangentWS) * input.tangentOS.w;

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
                float time = (_Time.y + _TimeOffset) * _TimeScale;
                
                // オブジェクトスケール
                float3 objectScale = float3(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22)
                );
                float avgScale = (objectScale.x + objectScale.z) * 0.5;
                float tilingMultiplier = lerp(1.0, avgScale * _TilingScale, 0.8);
                
                // 多層法線マッピング（より詳細）
                float2 normalUV1 = input.uv * tilingMultiplier + time * _NormalSpeed * float2(0.02, 0.03);
                float2 normalUV2 = input.uv * (1.3 * tilingMultiplier) + time * _NormalSpeed * float2(-0.01, 0.025);
                float2 normalUV3 = input.uv * (0.7 * tilingMultiplier) + time * _NormalSpeed * float2(0.015, -0.02);
                
                float2 detailUV1 = input.uv * (3.5 * tilingMultiplier) + time * _DetailNormalSpeed * float2(0.04, 0.06);
                float2 detailUV2 = input.uv * (5.1 * tilingMultiplier) + time * _DetailNormalSpeed * float2(-0.03, 0.05);
                
                float2 microUV1 = input.uv * (12.0 * tilingMultiplier) + time * _MicroNormalSpeed * float2(0.08, 0.1);
                float2 microUV2 = input.uv * (18.5 * tilingMultiplier) + time * _MicroNormalSpeed * float2(-0.06, 0.09);
                
                // 法線サンプリング
                half3 normal1 = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, normalUV1));
                half3 normal2 = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, normalUV2));
                half3 normal3 = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, normalUV3));
                
                half3 detailNormal1 = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUV1));
                half3 detailNormal2 = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUV2));
                
                half3 microNormal1 = UnpackNormal(SAMPLE_TEXTURE2D(_MicroNormalMap, sampler_MicroNormalMap, microUV1));
                half3 microNormal2 = UnpackNormal(SAMPLE_TEXTURE2D(_MicroNormalMap, sampler_MicroNormalMap, microUV2));
                
                // 法線ブレンド
                half3 baseNormal = normalize((normal1 + normal2 + normal3) / 3.0) * _NormalStrength;
                half3 detailCombined = normalize((detailNormal1 + detailNormal2) / 2.0) * _DetailNormalStrength;
                half3 microCombined = normalize((microNormal1 + microNormal2) / 2.0) * _MicroDetailStrength;
                
                // 波の相互作用による法線調整
                float interactionFactor = input.waveInteraction.x * _WaveBlending;
                baseNormal *= 1.0 + interactionFactor * 0.3;
                
                half3 finalNormal = normalize(baseNormal + detailCombined + microCombined);

                // ワールド空間法線
                float3x3 tangentToWorld = float3x3(input.worldTangent, input.worldBitangent, input.normalWS);
                float3 worldNormal = normalize(mul(finalNormal, tangentToWorld));
                
                float3 viewDir = normalize(input.viewDirWS);
                
                // 高度なフレネル計算
                float NdotV = saturate(dot(worldNormal, viewDir));
                float fresnel = _FresnelBias + (1.0 - _FresnelBias) * pow(1.0 - NdotV, _FresnelPower);
                fresnel *= _FresnelStrength;
                
                // 波の相互作用による反射強化
                fresnel *= 1.0 + input.waveInteraction.x * 0.2;

                // 深度とリフラクション
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                float2 refractionOffset = finalNormal.xy * _RefractionStrength * (1.0 + input.waveData.x * 0.5);
                screenUV += refractionOffset;
                
                float depth = LinearEyeDepth(SampleSceneDepth(screenUV), _ZBufferParams);
                float surfaceDepth = LinearEyeDepth(input.screenPos.z / input.screenPos.w, _ZBufferParams);
                float waterDepth = max(0, (depth - surfaceDepth));
                
                // リアルな深度ベース色計算
                float depthFactor = saturate(waterDepth / _DepthFade);
                float shallowFactor = saturate(waterDepth / _ShallowDepth);
                
                // 散乱効果
                float scatteringFactor = saturate(waterDepth * _DepthScattering / _DepthFade);
                
                // 複雑な色のブレンド
                half4 deepWater = _DeepWaterColor;
                half4 shallowWater = lerp(_ShallowWaterColor, _SurfaceColor, shallowFactor * 0.6);
                half4 waterColor = lerp(shallowWater, deepWater, depthFactor);
                
                // 散乱による色の変化
                waterColor.rgb = lerp(waterColor.rgb, _SubsurfaceColor.rgb, scatteringFactor * _Scattering);
                
                // 波の高さによる色調整
                float heightFactor = saturate(input.waveData.x * 2.0);
                waterColor.rgb = lerp(waterColor.rgb, _SurfaceColor.rgb, heightFactor * 0.3);
                
                // フレネル反射の適用
                waterColor.rgb = lerp(waterColor.rgb, waterColor.rgb * 1.5, fresnel * 0.7);
                
                // 深度による暗化（より自然）
                float depthDarkening = 1.0 - depthFactor * _DepthDarkening * 0.8;
                waterColor.rgb *= depthDarkening;

                // 高度な泡システム
                float foamBase = input.waveData.z;
                
                // 複数のノイズレイヤーで泡を生成
                float foamNoise1 = sin(time * _FoamSpeed * 0.8 + input.positionWS.x * (2.5 / avgScale) + input.positionWS.z * (3.1 / avgScale)) * 0.5 + 0.5;
                float foamNoise2 = sin(time * _FoamSpeed * 1.3 + input.positionWS.x * (4.2 / avgScale) + input.positionWS.z * (1.8 / avgScale)) * 0.5 + 0.5;
                float foamNoise3 = sin(time * _FoamSpeed * 0.6 + input.positionWS.x * (6.7 / avgScale) + input.positionWS.z * (5.3 / avgScale)) * 0.5 + 0.5;
                
                float combinedFoamNoise = (foamNoise1 + foamNoise2 * 0.7 + foamNoise3 * 0.4) / 2.1;
                
                // 波の相互作用による泡の強化
                float interactionFoam = input.waveInteraction.x * 0.5;
                foamBase += interactionFoam;
                
                float foamMask = saturate((foamBase + combinedFoamNoise * 0.4) * _FoamAmount);
                foamMask = smoothstep(_FoamCutoff - _FoamSoftness, _FoamCutoff + _FoamSoftness, foamMask);
                
                // 泡の持続性
                foamMask *= _FoamPersistence;
                
                // 詳細な泡テクスチャ
                float2 foamUV1 = input.uv * (_FoamScale * tilingMultiplier) + time * _FoamSpeed * 0.1 * float2(0.08, 0.12);
                float2 foamUV2 = input.uv * (_FoamScale * 1.7 * tilingMultiplier) + time * _FoamSpeed * 0.15 * float2(-0.06, 0.09);
                
                half3 foam1 = SAMPLE_TEXTURE2D(_FoamTexture, sampler_FoamTexture, foamUV1).rgb;
                half3 foam2 = SAMPLE_TEXTURE2D(_FoamTexture, sampler_FoamTexture, foamUV2).rgb;
                half3 combinedFoam = normalize(foam1 + foam2 * 0.8) * _FoamColor.rgb;
                
                // 泡のブレンド
                waterColor.rgb = lerp(waterColor.rgb, combinedFoam, foamMask);

                // サーフェスデータ設定
                SurfaceData surfaceData;
                ZERO_INITIALIZE(SurfaceData, surfaceData);
                surfaceData.albedo = waterColor.rgb;
                surfaceData.metallic = _Metallic;
                
                // 動的な粗さ（波の状態による）
                float dynamicRoughness = _Roughness * (1.0 + input.waveData.x * 0.5 + interactionFactor * 0.3);
                surfaceData.smoothness = 1.0 - dynamicRoughness;
                surfaceData.normalTS = finalNormal;
                
                // 動的な透明度
                float dynamicAlpha = lerp(_Transparency, 1.0, fresnel * 0.9 + foamMask * 0.8);
                surfaceData.alpha = dynamicAlpha;

                // ライティング計算
                InputData inputData;
                ZERO_INITIALIZE(InputData, inputData);
                inputData.positionWS = input.positionWS;
                inputData.normalWS = worldNormal;
                inputData.viewDirectionWS = viewDir;
                inputData.shadowCoord = input.shadowCoord;
                inputData.fogCoord = input.fogFactorAndVertexLight.x;
                inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
                inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);

                half4 color = UniversalFragmentPBR(inputData, surfaceData);
                
                // 高度なスペキュラハイライト
                Light mainLight = GetMainLight(inputData.shadowCoord);
                float3 halfVector = normalize(mainLight.direction + viewDir);
                float NdotH = saturate(dot(worldNormal, halfVector));
                float NdotL = saturate(dot(worldNormal, mainLight.direction));
                
                // 波の状態に応じたスペキュラ
                float dynamicSpecularPower = _SpecularPower * (1.0 + input.waveData.x * 0.3);
                float specular = pow(NdotH, dynamicSpecularPower) * _SpecularStrength;
                specular *= NdotL * mainLight.shadowAttenuation;
                
                // 波の相互作用による追加のきらめき
                float sparkle = input.waveInteraction.x * 0.5 + input.waveData.z * 0.3;
                specular *= 1.0 + sparkle;
                
                color.rgb += specular * mainLight.color;
                
                // 高度なサブサーフェススキャタリング
                float3 lightDir = mainLight.direction;
                float subsurfaceNdotL = saturate(dot(-worldNormal, lightDir));
                float subsurfaceVdotL = saturate(dot(-viewDir, lightDir));
                
                float subsurface = subsurfaceNdotL * subsurfaceVdotL * _SubsurfaceStrength;
                subsurface *= (1.0 - depthFactor) * (1.0 - foamMask);
                subsurface *= 1.0 + scatteringFactor * 0.5;
                
                color.rgb += subsurface * _SubsurfaceColor.rgb * mainLight.color;
                
                // リムライティング（波の縁での光）
                float rimFactor = 1.0 - NdotV;
                float rim = pow(rimFactor, 3.0) * _RimLighting;
                rim *= NdotL * mainLight.shadowAttenuation;
                rim *= 1.0 + input.waveData.x * 0.4; // 波の高さで強化
                
                color.rgb += rim * mainLight.color * _SurfaceColor.rgb;
                
                // 追加ライト（複数光源対応）
                #ifdef _ADDITIONAL_LIGHTS
                uint pixelLightCount = GetAdditionalLightsCount();
                for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
                {
                    Light light = GetAdditionalLight(lightIndex, inputData.positionWS);
                    float3 lightHalfVector = normalize(light.direction + viewDir);
                    float lightNdotH = saturate(dot(worldNormal, lightHalfVector));
                    float lightNdotL = saturate(dot(worldNormal, light.direction));
                    
                    float additionalSpecular = pow(lightNdotH, dynamicSpecularPower * 0.8) * _SpecularStrength * 0.5;
                    additionalSpecular *= lightNdotL * light.shadowAttenuation * light.distanceAttenuation;
                    
                    color.rgb += additionalSpecular * light.color;
                }
                #endif
                
                // フォグ適用
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                
                return color;
            }
            ENDHLSL
        }
    }
    
    FallBack "Hidden/Universal Render Pipeline/FallbackError"