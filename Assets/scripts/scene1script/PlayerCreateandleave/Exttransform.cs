
using UnityEngine;

public static class  Transforms
{
    

    public static void Destroychildren(this Transform t,bool destryimediate=false)
    {
        foreach (Transform child in t)
        {
            if(destryimediate)

            {
                MonoBehaviour.DestroyImmediate(child.gameObject);
            }
            else
            {
                MonoBehaviour.Destroy(child.gameObject);
            }
        }
    }
}