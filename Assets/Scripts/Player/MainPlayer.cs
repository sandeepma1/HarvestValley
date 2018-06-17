using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPlayer : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            transform.position = new Vector3(30, 51, 0);
        }
    }
}
