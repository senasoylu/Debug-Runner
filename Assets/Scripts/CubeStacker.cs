using System.Collections.Generic;
using UnityEngine;

public class CubeStacker : MonoBehaviour
{
   
    [SerializeField] private Transform cubeStackParent;    

    // Sallanma (sway) ve geçiþ (lerp) efektleri için:
    [SerializeField] private float swayAmplitude = 0.1f;       // Salýným mesafesi (x ekseninde)
    [SerializeField] private float swayFrequency = 2f;         // Salýným hýzý
    [SerializeField] private float smoothSpeed = 5f;           // Hedef pozisyona geçiþ hýzý (Lerp katsayýsý)

    [SerializeField] private float cubeHeight = 0.5f;        
    [SerializeField] private float firstCubeYOffset = 1.0f;    
    [SerializeField] private float firstCubeZOffset = 0.5f;    

   
    private List<GameObject> collectedCubes = new List<GameObject>();

    private void Update()
    {
        int count = collectedCubes.Count;
        for (int i = 0; i < count; i++)
        {
            Vector3 targetPos = new Vector3(0, firstCubeYOffset + (count - 1 - i) * cubeHeight, firstCubeZOffset);

            // Her küpe hafif bir salýným (sway) efekti verelim; i ekleyerek her küpe farklý faz kazandýrýyoruz.
            float swayOffset = Mathf.Sin(Time.time * swayFrequency + i) * swayAmplitude;
            targetPos.x += swayOffset;

            // Mevcut local pozisyondan hedef pozisyona, Time.deltaTime ve smoothSpeed kullanarak yumuþak geçiþ yapýn.
            collectedCubes[i].transform.localPosition = Vector3.Lerp(
                collectedCubes[i].transform.localPosition,
                targetPos,
                Time.deltaTime * smoothSpeed
            );
        }
    }

    // Yeni bir küp toplandýðýnda çaðrýlacak metot:
    public void CollectCube(GameObject cube)
    {
        // Collider varsa kapatarak sonraki çarpýþmalarý önleyelim.
        Collider col = cube.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        // Objeyi, önceden Inspector üzerinden atadýðýnýz stacking container'ýn altýna taþýyoruz.
        cube.transform.SetParent(cubeStackParent);
        // Yeni küp için baþlangýçta taban pozisyonuna (en altta, yani firstCubeYOffset) yerleþtirelim.
        cube.transform.localPosition = new Vector3(0, firstCubeYOffset, firstCubeZOffset);
        // Listenin sonuna ekleyin: bu sayede toplama sýrasý, ilk toplananýn index 0’da kalmasýný saðlar.
        collectedCubes.Add(cube);
    }
}
