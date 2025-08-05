using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class OceanMeshGenerator : MonoBehaviour
{
    [Header("Mesh Settings")]
    [SerializeField] private int meshResolution = 100;
    [SerializeField] private float meshSize = 100f;
    [SerializeField] private Material oceanMaterial;

    [Header("LOD Settings")]
    [SerializeField] private bool useLOD = true;
    [SerializeField] private float[] lodDistances = { 50f, 100f, 200f };
    [SerializeField] private int[] lodResolutions = { 100, 50, 25, 10 };

    [Header("Runtime Controls")]
    [SerializeField] private bool regenerateOnStart = true;
    [SerializeField] private bool showDebugInfo = true;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh oceanMesh;
    private Camera playerCamera;
    private int currentLOD = -1;

    void Start()
    {
        InitializeComponents();

        if (regenerateOnStart)
        {
            GenerateOceanMesh();
            SetupMaterial();
        }

        if (useLOD)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
                playerCamera = FindObjectOfType<Camera>();

            InvokeRepeating(nameof(UpdateLOD), 1f, 0.5f);
        }

        if (showDebugInfo)
        {
            Debug.Log($"Ocean initialized - Resolution: {meshResolution}, Size: {meshSize}");
        }
    }

    void InitializeComponents()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshFilter == null)
            meshFilter = gameObject.AddComponent<MeshFilter>();
        if (meshRenderer == null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
    }

    void GenerateOceanMesh()
    {
        GenerateOceanMesh(meshResolution);
    }

    void GenerateOceanMesh(int resolution)
    {
        if (oceanMesh != null)
            DestroyImmediate(oceanMesh);

        oceanMesh = new Mesh();
        oceanMesh.name = "Ocean Mesh";

        // 頂点配列のサイズ
        int vertexCount = (resolution + 1) * (resolution + 1);
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uvs = new Vector2[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Vector4[] tangents = new Vector4[vertexCount];

        // 三角形配列のサイズ
        int triangleCount = resolution * resolution * 6;
        int[] triangles = new int[triangleCount];

        // 頂点、UV、法線、タンジェントの生成
        float stepSize = meshSize / resolution;
        for (int i = 0, y = 0; y <= resolution; y++)
        {
            for (int x = 0; x <= resolution; x++, i++)
            {
                float xPos = (x - resolution * 0.5f) * stepSize;
                float zPos = (y - resolution * 0.5f) * stepSize;

                vertices[i] = new Vector3(xPos, 0, zPos);
                uvs[i] = new Vector2((float)x / resolution, (float)y / resolution);
                normals[i] = Vector3.up;
                tangents[i] = new Vector4(1, 0, 0, 1); // タンジェント追加
            }
        }

        // 三角形の生成
        for (int ti = 0, vi = 0, y = 0; y < resolution; y++, vi++)
        {
            for (int x = 0; x < resolution; x++, ti += 6, vi++)
            {
                // 各四角形を2つの三角形に分割
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + resolution + 1;
                triangles[ti + 5] = vi + resolution + 2;
            }
        }

        // メッシュに設定
        oceanMesh.vertices = vertices;
        oceanMesh.uv = uvs;
        oceanMesh.triangles = triangles;
        oceanMesh.normals = normals;
        oceanMesh.tangents = tangents;

        // メッシュの最適化
        oceanMesh.RecalculateBounds();
        oceanMesh.Optimize();

        // 大きなメッシュの場合はインデックス形式を32bitに
        if (vertexCount > 65535)
        {
            oceanMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }

        meshFilter.mesh = oceanMesh;

        if (showDebugInfo)
        {
            Debug.Log($"Ocean mesh generated - Vertices: {vertexCount}, Triangles: {triangleCount / 3}, Resolution: {resolution}");
        }
    }

    void SetupMaterial()
    {
        if (oceanMaterial != null && meshRenderer != null)
        {
            meshRenderer.material = oceanMaterial;

            // レンダリング設定
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.receiveShadows = true;
            meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
        }
        else
        {
            Debug.LogWarning("Ocean material not assigned!");
        }
    }

    void UpdateLOD()
    {
        if (playerCamera == null || !useLOD) return;

        float distance = Vector3.Distance(transform.position, playerCamera.transform.position);
        int newLOD = GetLODIndex(distance);

        if (newLOD != currentLOD)
        {
            currentLOD = newLOD;
            int newResolution = lodResolutions[newLOD];

            if (newResolution != meshResolution)
            {
                meshResolution = newResolution;
                GenerateOceanMesh(meshResolution);

                if (showDebugInfo)
                {
                    Debug.Log($"LOD changed - Distance: {distance:F1}, Resolution: {meshResolution}");
                }
            }
        }
    }

    int GetLODIndex(float distance)
    {
        for (int i = 0; i < lodDistances.Length; i++)
        {
            if (distance < lodDistances[i])
            {
                return i;
            }
        }
        return lodResolutions.Length - 1;
    }

    // パブリックメソッド - 外部から制御可能
    public void SetMeshResolution(int resolution)
    {
        meshResolution = Mathf.Clamp(resolution, 10, 500);
        GenerateOceanMesh();
    }

    public void SetMeshSize(float size)
    {
        meshSize = Mathf.Max(1f, size);
        GenerateOceanMesh();
    }

    public void SetOceanMaterial(Material material)
    {
        oceanMaterial = material;
        SetupMaterial();
    }

    // インスペクター用メソッド
    [ContextMenu("Regenerate Mesh")]
    void RegenerateMesh()
    {
        GenerateOceanMesh();
        Debug.Log("Mesh regenerated manually");
    }

    [ContextMenu("Reset to Default Settings")]
    void ResetToDefaults()
    {
        meshResolution = 100;
        meshSize = 100f;
        useLOD = true;
        GenerateOceanMesh();
        Debug.Log("Ocean reset to default settings");
    }

    // Gizmosで範囲とLODを表示
    void OnDrawGizmosSelected()
    {
        // メッシュ範囲
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(meshSize, 0, meshSize));

        // LOD距離
        if (useLOD && lodDistances != null)
        {
            for (int i = 0; i < lodDistances.Length; i++)
            {
                Gizmos.color = Color.Lerp(Color.green, Color.red, (float)i / lodDistances.Length);
                Gizmos.DrawWireSphere(transform.position, lodDistances[i]);
            }
        }

        // 現在のLOD情報
        if (Application.isPlaying && playerCamera != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, playerCamera.transform.position);
        }
    }

    // GUI表示（デバッグ用）
    //void OnGUI()
    //{
    //    if (!showDebugInfo || !Application.isPlaying) return;

    //    GUILayout.BeginArea(new Rect(10, 10, 250, 150));
    //    GUILayout.BeginVertical("box");

    //    GUILayout.Label("Ocean Debug Info", EditorGUIUtility.isProSkin ? GUI.skin.label : GUI.skin.box);
    //    GUILayout.Label($"Resolution: {meshResolution}");
    //    GUILayout.Label($"Vertices: {(meshResolution + 1) * (meshResolution + 1)}");
    //    GUILayout.Label($"Size: {meshSize}m");

    //    if (useLOD && playerCamera != null)
    //    {
    //        float distance = Vector3.Distance(transform.position, playerCamera.transform.position);
    //        GUILayout.Label($"Camera Distance: {distance:F1}m");
    //        GUILayout.Label($"Current LOD: {currentLOD}");
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}

    void OnDestroy()
    {
        if (oceanMesh != null)
            DestroyImmediate(oceanMesh);
    }

    void OnValidate()
    {
        // インスペクターでの値変更を検証
        meshResolution = Mathf.Clamp(meshResolution, 10, 500);
        meshSize = Mathf.Max(1f, meshSize);

        if (lodResolutions != null && lodDistances != null)
        {
            if (lodResolutions.Length != lodDistances.Length + 1)
            {
                Debug.LogWarning("LOD arrays size mismatch! lodResolutions should have one more element than lodDistances.");
            }
        }
    }
}