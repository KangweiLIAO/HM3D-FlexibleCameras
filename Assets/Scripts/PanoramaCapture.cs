using UnityEngine;
using UnityEditor;

public class PanoramaCapture : MonoBehaviour {

    public Camera targetCam;
    public Material cubemapMaterial;

    // Start is called before the first frame update
    void Start() {
        targetCam = gameObject.GetComponent<Camera>();
        RenderTexture cubemapRT;
        // find asset with name Cubemap, type of renderTexture in folder "Assets/Textures":
        string[] guids = AssetDatabase.FindAssets("Cubemap t:renderTexture", new[] { "Assets/Textures" });
        foreach (var asset in guids) {
            // delete the old cubemap textures
            var path = AssetDatabase.GUIDToAssetPath(asset);
            AssetDatabase.DeleteAsset(path);
        }
        // if cubemap texture not exist, create:
        cubemapRT = CaptureCubemapTexture(targetCam);
        cubemapMaterial.SetTexture("_CubeMap", cubemapRT);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {
            // Press U to update the cubemap
            CaptureCubemapTexture(targetCam);
            Debug.Log("Current cubemap is updated with camera: " + targetCam.name);
        } else if (Input.GetKeyDown(KeyCode.S)) {
            // Press S to create a panorama jpg file
            RenderTexture cubemapRT = CaptureCubemapTexture(targetCam);
            RenderTexture equirectRT = new RenderTexture(4096, 2048, 16);
            if (equirectRT.Create()) {
                cubemapRT.ConvertToEquirect(equirectRT);
                UpdateCubemap(cubemapRT);
                Texture2D t2d = new Texture2D(equirectRT.width, equirectRT.height); // texture2D to store image data
                RenderTexture.active = equirectRT;
                // store equirectRT image in a Texture2D:
                t2d.ReadPixels(new Rect(0, 0, equirectRT.width, equirectRT.height), 0, 0);
                RenderTexture.active = null;
                byte[] bytes = t2d.EncodeToJPG();
                string path = Application.dataPath + "/Generated/Panorama" + ".jpg"; // save as panorama jpg file
                System.IO.File.WriteAllBytes(path, bytes);
                Debug.Log("Cubemap successfully captured");
            }
        }
    }

    RenderTexture CaptureCubemapTexture(Camera camera) {
        RenderTexture cubemap = new RenderTexture(4096, 4096, 32);
        cubemap.dimension = UnityEngine.Rendering.TextureDimension.Cube;
        camera.RenderToCubemap(cubemap);
        if (cubemap.Create()) {
            AssetDatabase.CreateAsset(cubemap, "Assets/Textures/Cubemap.asset");
        }
        return cubemap;
    }

    void UpdateCubemap(RenderTexture texture) {
        cubemapMaterial.SetTexture("_CubeMap", texture);
    }
}
