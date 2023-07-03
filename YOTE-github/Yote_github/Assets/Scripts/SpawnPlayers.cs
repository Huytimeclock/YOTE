using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject objectPrefab;
    public GameObject scrollViewContent;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newObj = PhotonNetwork.Instantiate(objectPrefab.name, Vector3.zero, Quaternion.identity);
        newObj.transform.SetParent(scrollViewContent.transform, false);
        // Customize the properties of the new object if needed
    }
}
