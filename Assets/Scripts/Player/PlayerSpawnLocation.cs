using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnLocation : Singleton<PlayerSpawnLocation>
{
    [SerializeField]
    private Vector3 homeLocation = new Vector3(30, 51, 0);

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            SpwanPlayerAtLocation(homeLocation);
        }
    }

    public void SpwanPlayerAtLocation(Vector3 location)
    {
        transform.position = location;
    }
}