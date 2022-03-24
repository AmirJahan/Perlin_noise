using UnityEngine;

public class _01_Texture_Height : MonoBehaviour
{
    [SerializeField] GameObject textureSample;
    [SerializeField] GameObject sampleCube;

    int textureSize = 1080; // important to change

    [SerializeField, Range(1, 200)] float heightMultiplier = 5;

    [SerializeField] float gridSize = 32;

    float xOffset = 0, zOffset = 0;
    float cubeSize = 1f;


    Object[] textureSequence;
    int currentIndex = 0;


    void findCubeSize() {

        cubeSize = (float)textureSize / (float)gridSize; // 1080 / 128
    }

    void Start() {

        textureSequence = Resources.LoadAll("Face_Archive", typeof(Texture2D));

        findCubeSize();

        xOffset = (float)(gridSize / 2);
        zOffset = (float)(gridSize / 2);

        InvokeRepeating("runBlocks", 0, 0.25f);
    }

    void runBlocks() {
        // delete all possible children
        foreach (Transform any in gameObject.transform)
            Destroy(any.gameObject);

        getNewFace();
    }

    void getNewFace() {

        currentIndex++;
        currentIndex %= textureSequence.Length;
        Texture2D myTexture = (Texture2D)textureSequence[currentIndex];

        for (int x = 0; x < gridSize; x++) { // 32
            for (int z = 0; z < gridSize; z++) { // 


                // 8.4375
                Color thisPixel = myTexture.GetPixel(  Mathf.FloorToInt(x * cubeSize),
                                                       Mathf.FloorToInt(z * cubeSize)); // 0, 32
                float height = thisPixel.g;

                float xPos = (x - xOffset) * cubeSize;
                float zPos = (z - xOffset) * cubeSize;

                Vector3 copyPos = new Vector3(xPos,
                                              height * heightMultiplier,
                                              zPos);

                GameObject copy = Instantiate(sampleCube);
                copy.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
                copy.transform.position = copyPos;
                copy.transform.parent = gameObject.transform; // gameOBject = manager
            }
        }

        textureSample.GetComponent<Renderer>().material.mainTexture = myTexture;
    }
}
