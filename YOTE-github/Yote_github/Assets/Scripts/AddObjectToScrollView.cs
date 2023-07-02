using UnityEngine;
using UnityEngine.UI;

public class AddObjectToScrollView : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject objectPrefab;

    public void AddObject()
    {
        GameObject newObj = Instantiate(objectPrefab, scrollViewContent.transform);
        // Customize the properties of the new object if needed
    }
}
