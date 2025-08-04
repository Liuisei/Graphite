Shader "Custom/URP/PhotorealisticOcean"
{
    Properties
    {
        [Header(Primary Wave System)]
        _WaveSpeed("Wave Speed", Float) = 0.6
        _WaveHeight("Wave Height", Float) = 0.8
        _WaveFrequency("Wave Frequency", Float) = 0.8
        _WaveDirection("Wave Direction", Vector) = (1, 0, 0.3, 0.7)
        _WaveSteepness("Wave Steepness", Range(0.0, 1.0)) = 0.9
        
        [Header(Secondary Wave Layers)]
        _Wave2Speed("Wave 2 Speed", Float) = 0.4
        _Wave2Height("Wave 2 Height", Float) = 0.4
        _Wave2Frequency("Wave 2 Frequency", Float) = 1.2
        
        _Wave3Speed("Wave 3 Speed", Float) = 1.1
        _Wave3Height("Wave 3 Height", Float) = 0.15
        _Wave3Frequency("Wave 3 Frequency", Float) = 2.8
        
        [Header(Detail Waves)]
        _DetailWaveSpeed("Detail Wave Speed", Float) = 1.8
        _DetailWaveHeight("Detail Wave Height", Float) = 0.05
        _DetailWaveFreq("Detail Wave Frequency", Float) = 6.0
        
        [Header(Ocean Colors)]
        _SurfaceColor("Surface Color", Color) = (0.4, 0.65, 0.75, 0.8)
        _ShallowColor("Shallow Color", Color) = (0.45, 0.75, 0.85, 0.75)
        _DeepColor("Deep Color", Color) = (0.05, 0.15, 0.3, 1.0)
        _HorizonColor("Horizon Color", Color) = (0.2, 0.5, 0.7, 1.0)
        _SubsurfaceColor("Subsurface Color", Color) = (0.1, 0.4, 0.6, 1.0)
        
        [Header(Optical Properties)]
        _FresnelStrength("Fresnel Strength", Range(0.0, 2.0)) = 1.2
        _FresnelPower("Fresnel Power", Range(0.1, 8.0)) = 4.0
        _FresnelBias("Fresnel Bias", Range(0.0, 0.5)) = 0.02
        _Transparency("Transparency", Range(0.0, 1.0)) = 0.85
        _RefractionStrength("Refraction Strength", Range(0.0, 0.1)) = 0.03
        
        [Header(Surface Detail)]
        _Roughness("Surface Roughness", Range(0.0, 1.0)) = 0.02
        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _NormalStrength("Normal Strength", Range(0.0, 4.0)) = 2.5
        _DetailNormalStrength("Detail Normal Strength", Range(0.0, 2.0)) = 1.8
        _MicroNormalStrength("Micro Normal Strength", Range(0.0, 1.0)) = 0.6
        
        [Header(Textures)]
        _NormalMap("Wave Normal Map", 2D) = "bump" {}
        _DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
        _FoamTexture("Foam Texture", 2D) = "white" {}
        _CausticTexture("Caustic Texture", 2D) = "black" {}
        
        [Header(Foam System)]
        _FoamAmount("Foam Amount", Range(0.0, 3.0)) = 1.5
        _FoamCutoff("Foam Cutoff", Range(0.0, 1.0)) = 0.5
        _FoamSoftness("Foam Softness", Range(0.01, 0.5)) = 0.1
        _FoamColor("Foam Color", Color) = (0.9, 0.95, 1.0, 1.0)
        _FoamSpeed("Foam Animation Speed", Float) = 2.2
        
        [Header(Lighting Enhancement)]
        _SpecularStrength("Specular Strength", Range(0.0, 2.0)) = 1.5
        _SubsurfaceStrength("Subsurface Scattering", Range(0.0, 2.0)) = 0.8
        _CausticStrength("Caustic Strength", Range(0.0, 2.0)) = 0.4
        
        [Header(Animation Speeds)]
        _NormalSpeed("Normal Speed", Float) = 0.08
        _DetailNormalSpeed("Detail Normal Speed", Float) = 0.3
        _CausticSpeed("Caustic Speed", Float) = 0.5
        
        [Header(Depth Effects)]
        _DepthFade("Depth Fade Distance", Range(0.1, 20.0)) = 8.0
        _ShallowDepth("Shallow Water Depth", Range(0.1, 5.0)) = 1.5
        _DepthDarkening("Depth Darkening", Range(0.0, 2.0)) = 1.2
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
        }
        
        LOD 400
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
                float3 waveData                 : TEXCOORD8; // x: height, y: slope, z: foam
                float3 worldTangent             : TEXCOORD9;
                float3 worldBitangent           : TEXCOORD10;
            };

            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            TEXTURE2D(_DetailNormalMap);
            SAMPLER(sampler_DetailNormalMap);
            TEXTURE2D(_FoamTexture);
            SAMPLER(sampler_FoamTexture);
            TEXTURE2D(_CausticTexture);
            SAMPLER(sampler_CausticTexture);

            CBUFFER_START(UnityPerMaterial)
                // Wave properties
                float _WaveSpeed, _WaveHeight, _WaveFrequency, _WaveSteepness;
                float4 _WaveDirection;
                float _Wave2Speed, _Wave2Height, _Wave2Frequency;
                float _Wave3Speed, _Wave3Height, _Wave3Frequency;
                float _DetailWaveSpeed, _DetailWaveHeight, _DetailWaveFreq;
                
                // Colors
                half4 _SurfaceColor, _ShallowColor, _DeepColor, _HorizonColor, _SubsurfaceColor;
                
                // Optical
                float _FresnelStrength, _FresnelPower, _FresnelBias, _Transparency, _RefractionStrength;
                
                // Surface
                float _Roughness, _Metallic;
                float _NormalStrength, _DetailNormalStrength, _MicroNormalStrength;
                
                // Textures
                float4 _NormalMap_ST, _DetailNormalMap_ST, _FoamTexture_ST, _CausticTexture_ST;
                
                // Foam
                float _FoamAmount, _FoamCutoff, _FoamSoftness, _FoamSpeed;
                half4 _FoamColor;
                
                // Lighting
                float _SpecularStrength, _SubsurfaceStrength, _CausticStrength;
                
                // Animation
                float _NormalSpeed, _DetailNormalSpeed, _CausticSpeed;
                
                // Depth
                float _DepthFade, _ShallowDepth, _DepthDarkening;
            CBUFFER_END

            // Advanced Gerstner wave with better control
            float4 GerstnerWave(float3 pos, float amp, float freq, float2 dir, float speed, float steep, float time)
            {
                float phase = dot(dir, pos.xz) * freq + time * speed;
                float sharpness = steep / max(freq * amp, 0.001);
                
                float sinPhase = sin(phase);
                float cosPhase = cos(phase);
                
                float3 wave = float3(0, 0, 0);
                wave.x = dir.x * amp * sharpness * sinPhase;
                wave.z = dir.y * amp * sharpness * sinPhase;
                wave.y = amp * cosPhase;
                
                // Return wave displacement and slope information
                float slope = amp * freq * cosPhase;
                return float4(wave, slope);
            }

            Varyings vert(Attributes input)
            {
                Varyings output;
                ZERO_INITIALIZE(Varyings, output);

                float time = _Time.y;
                float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);
                
                float3 waveOffset = float3(0, 0, 0);
                float totalSlope = 0;
                
                // Large primary waves
                float2 dir1 = normalize(_WaveDirection.xy);
                float4 wave1 = GerstnerWave(worldPos, _WaveHeight, _WaveFrequency, dir1, _WaveSpeed, _WaveSteepness, time);
                waveOffset += wave1.xyz;
                totalSlope += wave1.w;
                
                // Medium secondary waves
                float2 dir2 = normalize(_WaveDirection.zw);
                float4 wave2 = GerstnerWave(worldPos, _Wave2Height, _Wave2Frequency, dir2, _Wave2Speed, _WaveSteepness * 0.8, time);
                waveOffset += wave2.xyz;
                totalSlope += wave2.w * 0.8;
                
                // Smaller tertiary waves
                float2 dir3 = normalize(float2(0.6, 0.8));
                float4 wave3 = GerstnerWave(worldPos, _Wave3Height, _Wave3Frequency, dir3, _Wave3Speed, _WaveSteepness * 0.6, time);
                waveOffset += wave3.xyz;
                totalSlope += wave3.w * 0.6;
                
                // Detail waves
                float2 dir4 = normalize(float2(-0.4, 0.9));
                float4 wave4 = GerstnerWave(worldPos, _DetailWaveHeight, _DetailWaveFreq, dir4, _DetailWaveSpeed, 0.4, time);
                waveOffset += wave4.xyz;
                totalSlope += wave4.w * 0.3;
                
                // Additional micro waves for surface detail
                float2 dir5 = normalize(float2(0.8, -0.3));
                float4 wave5 = GerstnerWave(worldPos, _DetailWaveHeight * 0.5, _DetailWaveFreq * 1.7, dir5, _DetailWaveSpeed * 1.3, 0.2, time);
                waveOffset += wave5.xyz;
                
                worldPos += waveOffset;
                input.positionOS = mul(unity_WorldToObject, float4(worldPos, 1.0));
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                output.uv = TRANSFORM_TEX(input.texcoord, _NormalMap);
                output.positionWS = vertexInput.positionWS;
                output.positionCS = vertexInput.positionCS;
                output.normalWS = normalInput.normalWS;
                output.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
                output.screenPos = ComputeScreenPos(vertexInput.positionCS);
                
                // Store wave data for fragment shader
                output.waveData.x = length(waveOffset); // wave height
                output.waveData.y = totalSlope; // slope for foam
                output.waveData.z = saturate(totalSlope * 2.0); // foam factor
                
                // Calculate tangent space for normal mapping
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
                float time = _Time.y;
                
                // Multi-scale normal mapping for realistic surface detail
                float2 normalUV1 = input.uv + time * _NormalSpeed * float2(0.02, 0.04);
                float2 normalUV2 = input.uv * 1.7 + time * _NormalSpeed * float2(-0.015, 0.025);
                float2 detailUV1 = input.uv * 4.3 + time * _DetailNormalSpeed * float2(0.05, 0.08);
                float2 detailUV2 = input.uv * 7.1 + time * _DetailNormalSpeed * float2(-0.03, 0.06);
                float2 microUV = input.uv * 16.0 + time * _DetailNormalSpeed * 1.5 * float2(0.08, 0.12);
                
                half3 normal1 = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, normalUV1));
                half3 normal2 = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, normalUV2));
                half3 detailNormal1 = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUV1));
                half3 detailNormal2 = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUV2));
                half3 microNormal = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, microUV));
                
                // Blend normals with different intensities
                half3 baseNormal = normalize(normal1 + normal2) * _NormalStrength;
                half3 detailCombined = normalize(detailNormal1 + detailNormal2) * _DetailNormalStrength;
                half3 finalNormal = normalize(baseNormal + detailCombined + microNormal * _MicroNormalStrength);

                // Transform normal to world space
                float3x3 tangentToWorld = float3x3(input.worldTangent, input.worldBitangent, input.normalWS);
                float3 worldNormal = normalize(mul(finalNormal, tangentToWorld));
                
                float3 viewDir = normalize(input.viewDirWS);
                
                // Enhanced fresnel calculation
                float NdotV = saturate(dot(worldNormal, viewDir));
                float fresnel = _FresnelBias + (1.0 - _FresnelBias) * pow(1.0 - NdotV, _FresnelPower);
                fresnel *= _FresnelStrength;

                // Depth and transparency calculations
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                screenUV += finalNormal.xy * _RefractionStrength; // Refraction
                
                float depth = LinearEyeDepth(SampleSceneDepth(screenUV), _ZBufferParams);
                float surfaceDepth = LinearEyeDepth(input.screenPos.z / input.screenPos.w, _ZBufferParams);
                float waterDepth = max(0, (depth - surfaceDepth));
                
                // Depth-based color mixing
                float depthFactor = saturate(waterDepth / _DepthFade);
                float shallowFactor = saturate(waterDepth / _ShallowDepth);
                
                half4 surfaceWater = lerp(_SurfaceColor, _ShallowColor, shallowFactor * 0.5);
                half4 deepWater = lerp(surfaceWater, _DeepColor, depthFactor);
                half4 waterColor = lerp(deepWater, _HorizonColor, fresnel * 0.6);
                
                // Darken with depth
                waterColor.rgb *= lerp(1.0, 1.0 - _DepthDarkening * 0.5, depthFactor);

                // Advanced foam system
                float foamNoise = sin(time * _FoamSpeed + input.positionWS.x * 3.2 + input.positionWS.z * 2.7) * 0.5 + 0.5;
                float foamMask = saturate((input.waveData.z + foamNoise * 0.3) * _FoamAmount);
                foamMask = smoothstep(_FoamCutoff - _FoamSoftness, _FoamCutoff + _FoamSoftness, foamMask);
                
                float2 foamUV = input.uv * 6.0 + time * 0.4 * float2(0.1, 0.15);
                half3 foam = SAMPLE_TEXTURE2D(_FoamTexture, sampler_FoamTexture, foamUV).rgb * _FoamColor.rgb;
                
                // Caustics effect
                float2 causticUV = input.uv * 3.0 + time * _CausticSpeed * float2(0.05, 0.08);
                half caustics = SAMPLE_TEXTURE2D(_CausticTexture, sampler_CausticTexture, causticUV).r;
                caustics *= _CausticStrength * (1.0 - depthFactor) * (1.0 - fresnel);
                
                waterColor.rgb += caustics * _SubsurfaceColor.rgb;
                waterColor.rgb = lerp(waterColor.rgb, foam, foamMask);

                // Setup surface data
                SurfaceData surfaceData;
                ZERO_INITIALIZE(SurfaceData, surfaceData);
                surfaceData.albedo = waterColor.rgb;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = 1.0 - _Roughness;
                surfaceData.normalTS = finalNormal;
                surfaceData.alpha = lerp(_Transparency, 1.0, fresnel * 0.8 + foamMask);

                // Lighting
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
                
                // Enhanced specular highlights
                Light mainLight = GetMainLight(inputData.shadowCoord);
                float3 halfVector = normalize(mainLight.direction + viewDir);
                float NdotH = saturate(dot(worldNormal, halfVector));
                float specular = pow(NdotH, (1.0 - _Roughness) * 128.0) * _SpecularStrength;
                color.rgb += specular * mainLight.color * mainLight.shadowAttenuation;
                
                // Subsurface scattering
                float subsurface = saturate(dot(-worldNormal, mainLight.direction)) * _SubsurfaceStrength;
                color.rgb += subsurface * _SubsurfaceColor.rgb * mainLight.color * (1.0 - depthFactor);
                
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                
                return color;
            }
            ENDHLSL
        }
    }
    
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
