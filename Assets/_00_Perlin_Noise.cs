using UnityEngine;

public class _00_Perlin_Noise : MonoBehaviour
{
    [SerializeField]
    GameObject textureSample;

    [SerializeField]
    int textureSize = 32;

    float sceneSize = 128;


    Texture2D noiseTexture;


    [SerializeField]
    GameObject sampleCube;


    [SerializeField, Range(1, 200)]
    float heightMultiplier = 5;


    [SerializeField, Range(0, 10)]
    float zoomFactor = 1;


    float xOffset = 0, zOffset = 0;



    float cubeSize = 1f;

    void findCubeSize ()
    {
        cubeSize = (float)sceneSize / (float)textureSize;
    }


    void Start()
    {
        findCubeSize();

        xOffset = (float)(textureSize / 2);
        zOffset = (float)(textureSize / 2);



        noiseTexture = new Texture2D(textureSize, textureSize);


        InvokeRepeating("runBlocks", 0, 0.25f);
    }

    void runBlocks()
    {
        // delete all possible children
        foreach (Transform any in gameObject.transform)
            Destroy(any.gameObject);

        makeTiles();
    }

    void makeTiles()
    {
        for (int x = 0; x < textureSize; x++)
        {
            for (int z = 0; z < textureSize; z++)
            {

                float texX = (float)x / (float)textureSize * zoomFactor + Time.time; // 0, .1, .2, .3
                float texZ = (float)z / (float)textureSize * zoomFactor;

                // this is the value of noise in that spot
                float grayCol = Mathf.PerlinNoise(texX, texZ);
                Color color = new Color(grayCol, grayCol, grayCol);

                noiseTexture.SetPixel(x, z, color);


                Vector3 copyPos = new Vector3((x - xOffset) * cubeSize,
                                                grayCol * heightMultiplier,
                                                (z - zOffset) * cubeSize);

                GameObject copy = Instantiate(sampleCube);
                copy.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
                copy.transform.position = copyPos;
                copy.transform.parent = gameObject.transform; // gameOBject = manager
            }
        }

        noiseTexture.Apply();
        textureSample.GetComponent<Renderer>().material.mainTexture = noiseTexture;
    }
}
