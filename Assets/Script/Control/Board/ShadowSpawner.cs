using UnityEngine;

public class ShadowSpawner : MonoBehaviour
{
    private GameObject shadow;

    public void SpawnShadow(GameObject Monster, GameObject Grid)
    {
        if (!shadow)
        {
            shadow = Instantiate(Monster, Grid.transform.position, Quaternion.Euler(0, 0, 0));
            //Destroy script
            MonoBehaviour[] components;
            components = shadow.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour c in components)
            {
                Destroy(c);
            }
        }
    }

    public void DestroyShadow()
    {
        if (shadow)
            Destroy(shadow);
    }
}
