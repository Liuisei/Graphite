using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class OceanSettings
{
    [Header("Mesh Generation")]
    public int resolution = 200;
    public float size = 300f;
    public bool enableLOD = true;

    [Header("Wave Parameters")]
    public float waveSpeed = 0.4f;
    public float waveHeight = 1.5f;
    public float waveFrequency = 0.5f;
    public Vector4 waveDirection = new Vector4(1f, 0.2f, 0.3f, 0.8f);
    public float waveSteepness = 0.75f;

    [Header("Colors")]
    public Color surfaceColor = new Color(0.42f, 0.68f, 0.76f, 0.85f);
    public Color shallowColor = new Color(0.48f, 0.72f, 0.82f, 0.78f);
    public Color deepColor = new Color(0.06f, 0.16f, 0.32f, 1.0f);
    public Color horizonColor = new Color(0.22f, 0.52f, 0.74f, 1.0f);

    [Header("Surface Properties")]
    public float fresnelStrength = 1.3f;
    public float transparency = 0.8f;
    public float normalStrength = 2.8f;
    public float foamAmount = 1.8f;

    [Header("Performance")]
    [Range(0, 3)]
    public int qualityLevel = 2; // 0=Low, 1=Medium, 2=High, 3=Ultra
    public bool autoQualityAdjust = true;
    public float targetFPS = 60f;
}

public class PhotorealisticOcean : MonoBehaviour
{
    [SerializeField] private OceanSettings settings = new OceanSettings();
    [SerializeField] private Shader oceanShader;
    [SerializeField] private Texture2D normalMap;
    [SerializeField] private Texture2D detailNormalMap;
    [SerializeField] private Texture2D foamTexture;
    [SerializeField] private Texture2D causticTexture;

    private Material oceanMaterial;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Mesh oceanMesh;

    // Post-processing
    private Volume postProcessVolume;
    private VolumeProfile volumeProfile;

    // Performance monitoring
    private float frameTimeAverage;
    private float lastQualityCheck;

    // LOD system
    private Camera mainCamera;
    private float[] lodDistances = { 30f, 60f, 120f, 250f, 500f };
    private int[] lodResolutions = { 250, 180, 120, 80, 40 };

    void Start()
    {
        InitializeComponents();
        CreateOceanMesh();
        SetupMaterial();
        SetupLighting();
        SetupPostProcessing();
        ApplySettings();
    }

    void InitializeComponents()
    {
        // メッシュコンポーネントの追加
        if (!meshRenderer) meshRenderer = gameObject.AddComponent<MeshRenderer>();
        if (!meshFilter) meshFilter = gameObject.AddComponent<MeshFilter>();

        // カメラの取得
        mainCamera = Camera.main ?? FindObjectOfType<Camera>();

        // シェーダーの自動検索
        if (!oceanShader)
            oceanShader = Shader.Find("Custom/URP/PhotorealisticOcean");
    }

    void CreateOceanMesh()
    {
        int res = settings.resolution;
        float size = settings.size;

        oceanMesh = new Mesh();
        oceanMesh.name = "Ocean Mesh";

        // 頂点生成
        Vector3[] vertices = new Vector3[(res + 1) * (res + 1)];
        Vector2[] uvs = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];

        for (int y = 0; y <= res; y++)
        {
            for (int x = 0; x <= res; x++)
            {
                int index = y * (res + 1) + x;

                float xPos = ((float)x / res - 0.5f) * size;
                float zPos = ((float)y / res - 0.5f) * size;

                vertices[index] = new Vector3(xPos, 0, zPos);
                uvs[index] = new Vector2((float)x / res, (float)y / res);
                normals[index] = Vector3.up;
                tangents[index] = new Vector4(1, 0, 0, 1);
            }
        }

        // インデックス生成
        int[] triangles = new int[res * res * 6];
        int triangleIndex = 0;

        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                int topLeft = y * (res + 1) + x;
                int topRight = topLeft + 1;
                int bottomLeft = (y + 1) * (res + 1) + x;
                int bottomRight = bottomLeft + 1;

                // 第1三角形
                triangles[triangleIndex++] = topLeft;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = topRight;

                // 第2三角形
                triangles[triangleIndex++] = topRight;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = bottomRight;
            }
        }

        oceanMesh.vertices = vertices;
        oceanMesh.uv = uvs;
        oceanMesh.normals = normals;
        oceanMesh.tangents = tangents;
        oceanMesh.triangles = triangles;

        oceanMesh.RecalculateBounds();
        meshFilter.mesh = oceanMesh;
    }

    void SetupMaterial()
    {
        if (!oceanShader)
        {
            Debug.LogError("Ocean shader not found!");
            return;
        }

        oceanMaterial = new Material(oceanShader);

        // テクスチャの自動検索と割り当て
        if (!normalMap) normalMap = Resources.Load<Texture2D>("Ocean/Textures/water_normal_map");
        if (!detailNormalMap) detailNormalMap = Resources.Load<Texture2D>("Ocean/Textures/water_detail_normal");
        if (!foamTexture) foamTexture = Resources.Load<Texture2D>("Ocean/Textures/foam_texture");
        if (!causticTexture) causticTexture = Resources.Load<Texture2D>("Ocean/Textures/caustic_texture");

        if (normalMap) oceanMaterial.SetTexture("_NormalMap", normalMap);
        if (detailNormalMap) oceanMaterial.SetTexture("_DetailNormalMap", detailNormalMap);
        if (foamTexture) oceanMaterial.SetTexture("_FoamTexture", foamTexture);
        if (causticTexture) oceanMaterial.SetTexture("_CausticTexture", causticTexture);

        meshRenderer.material = oceanMaterial;
    }

    void SetupLighting()
    {
        // メインライトの設定
        Light sunLight = RenderSettings.sun;
        if (!sunLight)
        {
            GameObject lightGO = new GameObject("Ocean Sun Light");
            lightGO.transform.SetParent(transform);
            sunLight = lightGO.AddComponent<Light>();
        }

        sunLight.type = LightType.Directional;
        sunLight.intensity = 2.0f;
        sunLight.color = new Color(1.0f, 0.98f, 0.92f, 1.0f);
        sunLight.shadows = LightShadows.Soft;
        sunLight.transform.rotation = Quaternion.Euler(40f, -30f, 0f);

        // 環境光の設定
        RenderSettings.ambientMode = AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.52f, 0.72f, 1.0f);
        RenderSettings.ambientEquatorColor = new Color(0.45f, 0.65f, 0.85f);
        RenderSettings.ambientGroundColor = new Color(0.25f, 0.35f, 0.45f);
        RenderSettings.ambientIntensity = 0.7f;
    }

    void SetupPostProcessing()
    {
        // Post-processing Volumeの作成
        GameObject volumeGO = new GameObject("Ocean Post-Process Volume");
        volumeGO.transform.SetParent(transform);

        postProcessVolume = volumeGO.AddComponent<Volume>();
        postProcessVolume.isGlobal = true;

        // VolumeProfileの作成
        volumeProfile = ScriptableObject.CreateInstance<VolumeProfile>();
        postProcessVolume.profile = volumeProfile;

        // Bloom効果の追加
        if (volumeProfile.TryGet<Bloom>(out var bloom))
        {
            bloom.threshold.value = 1.0f;
            bloom.intensity.value = 0.35f;
            bloom.scatter.value = 0.6f;
            bloom.tint.value = new Color(1.0f, 0.98f, 0.95f);
        }
        else
        {
            bloom = volumeProfile.Add<Bloom>();
            bloom.threshold.Override(1.0f);
            bloom.intensity.Override(0.35f);
            bloom.scatter.Override(0.6f);
            bloom.tint.Override(new Color(1.0f, 0.98f, 0.95f));
        }

        // Color Adjustmentsの追加
        if (volumeProfile.TryGet<ColorAdjustments>(out var colorAdj))
        {
            colorAdj.postExposure.value = 0.08f;
            colorAdj.contrast.value = 10f;
            colorAdj.saturation.value = 12f;
        }
        else
        {
            colorAdj = volumeProfile.Add<ColorAdjustments>();
            colorAdj.postExposure.Override(0.08f);
            colorAdj.contrast.Override(10f);
            colorAdj.saturation.Override(12f);
        }
    }

    void ApplySettings()
    {
        if (!oceanMaterial) return;

        // Wave parameters
        oceanMaterial.SetFloat("_WaveSpeed", settings.waveSpeed);
        oceanMaterial.SetFloat("_WaveHeight", settings.waveHeight);
        oceanMaterial.SetFloat("_WaveFrequency", settings.waveFrequency);
        oceanMaterial.SetVector("_WaveDirection", settings.waveDirection);
        oceanMaterial.SetFloat("_WaveSteepness", settings.waveSteepness);

        // Colors
        oceanMaterial.SetColor("_SurfaceColor", settings.surfaceColor);
        oceanMaterial.SetColor("_ShallowColor", settings.shallowColor);
        oceanMaterial.SetColor("_DeepColor", settings.deepColor);
        oceanMaterial.SetColor("_HorizonColor", settings.horizonColor);

        // Surface properties
        oceanMaterial.SetFloat("_FresnelStrength", settings.fresnelStrength);
        oceanMaterial.SetFloat("_Transparency", settings.transparency);
        oceanMaterial.SetFloat("_NormalStrength", settings.normalStrength);
        oceanMaterial.SetFloat("_FoamAmount", settings.foamAmount);

        // Quality-based settings
        ApplyQualitySettings(settings.qualityLevel);
    }

    void ApplyQualitySettings(int quality)
    {
        if (!oceanMaterial) return;

        switch (quality)
        {
            case 0: // Low
                oceanMaterial.SetFloat("_DetailWaveHeight", 0.02f);
                oceanMaterial.SetFloat("_DetailNormalStrength", 1.0f);
                oceanMaterial.SetFloat("_CausticStrength", 0.2f);
                break;
            case 1: // Medium
                oceanMaterial.SetFloat("_DetailWaveHeight", 0.06f);
                oceanMaterial.SetFloat("_DetailNormalStrength", 1.5f);
                oceanMaterial.SetFloat("_CausticStrength", 0.3f);
                break;
            case 2: // High
                oceanMaterial.SetFloat("_DetailWaveHeight", 0.10f);
                oceanMaterial.SetFloat("_DetailNormalStrength", 2.0f);
                oceanMaterial.SetFloat("_CausticStrength", 0.5f);
                break;
            case 3: // Ultra
                oceanMaterial.SetFloat("_DetailWaveHeight", 0.12f);
                oceanMaterial.SetFloat("_DetailNormalStrength", 2.5f);
                oceanMaterial.SetFloat("_CausticStrength", 0.6f);
                break;
        }
    }

    void Update()
    {
        if (settings.enableLOD && mainCamera)
        {
            UpdateLOD();
        }

        if (settings.autoQualityAdjust)
        {
            MonitorPerformance();
        }
    }

    void UpdateLOD()
    {
        float distance = Vector3.Distance(transform.position, mainCamera.transform.position);

        int newResolution = settings.resolution;
        for (int i = 0; i < lodDistances.Length; i++)
        {
            if (distance > lodDistances[i])
            {
                newResolution = lodResolutions[i];
            }
            else
            {
                break;
            }
        }

        if (newResolution != settings.resolution)
        {
            settings.resolution = newResolution;
            CreateOceanMesh();
        }
    }

    void MonitorPerformance()
    {
        frameTimeAverage = Mathf.Lerp(frameTimeAverage, Time.deltaTime, 0.1f);

        if (Time.time - lastQualityCheck > 2f)
        {
            float currentFPS = 1f / frameTimeAverage;

            if (currentFPS < settings.targetFPS * 0.8f && settings.qualityLevel > 0)
            {
                settings.qualityLevel--;
                ApplyQualitySettings(settings.qualityLevel);
            }
            else if (currentFPS > settings.targetFPS * 1.1f && settings.qualityLevel < 3)
            {
                settings.qualityLevel++;
                ApplyQualitySettings(settings.qualityLevel);
            }

            lastQualityCheck = Time.time;
        }
    }

    // インスペクター用の設定更新
    void OnValidate()
    {
        if (Application.isPlaying)
        {
            ApplySettings();
        }
    }

    // ギズモ表示
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(settings.size, 0, settings.size));
    }
}