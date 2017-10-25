using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FractalManager : MonoBehaviour {

    public Mesh[] meshes; //In Unity, a mesh is a construct used the the GPU to draw complex objects.
    public Material material; // In Unity, materials are essentiually shaders or visual properties of objects.
    public int maxDepth; //Variable to determine exit out of recursive function
    public float childScale;
    public float spawnProbaility;
    public float maxRotationSpeed;
    public float maxTwist;

    private Material[,] materials;
    private int depth;
    private float rotationSpeed;

    /*
    When fractal object is called, add mesh and material 
    In this case, Start() is invoked before first frame, but Start() could be used when the script is invoked halfway through scene
    */
    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void Start()
    {
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);
        if (materials == null)
        {
            InitializeMaterials();
        }
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0,2)];
           
        //exit conditional to prevent infinite recursive loop
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    //Set fractal children to have same variables of parent, while incrementing depth variable by 1
    private void Initialize(FractalManager parent, int childIndex)
    {
        meshes = parent.meshes;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwist = parent.maxTwist;
        transform.parent = parent.transform; //so that subsequent fractals will properly nest in Unity.
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[childIndex] * (.5f + .5f * childScale);
        transform.localRotation = childOrientations[childIndex];
        spawnProbaility = parent.spawnProbaility;
    }

    //define what directions will the fractals grow
    private static Vector3[] childDirections =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back,
    };

    //rotations for growing children
    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f,0f,-90f),
        Quaternion.Euler(0f,0f,90f),
        Quaternion.Euler(90f,0f,0f),
        Quaternion.Euler(-90f,0f,0f)
    };
    
    //allow fractal to visually generate
    private IEnumerator CreateChildren ()
    {
        for (int i = 0; i < childDirections.Length; i++)
            if(Random.value < spawnProbaility)
            {
                yield return new WaitForSeconds(Random.Range(.1f, .5f));
                new GameObject("Fractal Child").AddComponent<FractalManager>().Initialize(this, i);
            }
    }

    //create new material on children, create color gradient as depth/children grow
    private void InitializeMaterials ()
    {
        materials = new Material[maxDepth + 1, 2];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;
            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, (float)i / maxDepth);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
        }
        materials[maxDepth, 0].color = Color.magenta;
        materials[maxDepth, 1].color = Color.red;
    }

    //Give functionality to the exit UI button
    public void exitApplication()
    {
        Application.Quit();
    }

    //Give functionality to the restart UI button
    public void restart()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        Time.timeScale = 1;
    }

}
