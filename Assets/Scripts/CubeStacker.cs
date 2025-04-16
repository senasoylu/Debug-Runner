using System.Collections.Generic;
using UnityEngine;

public class CubeStacker : MonoBehaviour
{
   
    [SerializeField] private Transform cubeStackParent;    

    // Sallanma (sway) ve ge�i� (lerp) efektleri i�in:
    [SerializeField] private float swayAmplitude = 0.1f;       // Sal�n�m mesafesi (x ekseninde)
    [SerializeField] private float swayFrequency = 2f;         // Sal�n�m h�z�
    [SerializeField] private float smoothSpeed = 5f;           // Hedef pozisyona ge�i� h�z� (Lerp katsay�s�)

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

            // Her k�pe hafif bir sal�n�m (sway) efekti verelim; i ekleyerek her k�pe farkl� faz kazand�r�yoruz.
            float swayOffset = Mathf.Sin(Time.time * swayFrequency + i) * swayAmplitude;
            targetPos.x += swayOffset;

            // Mevcut local pozisyondan hedef pozisyona, Time.deltaTime ve smoothSpeed kullanarak yumu�ak ge�i� yap�n.
            collectedCubes[i].transform.localPosition = Vector3.Lerp(
                collectedCubes[i].transform.localPosition,
                targetPos,
                Time.deltaTime * smoothSpeed
            );
        }
    }

    // Yeni bir k�p topland���nda �a�r�lacak metot:
    public void CollectCube(GameObject cube)
    {
        // Collider varsa kapatarak sonraki �arp��malar� �nleyelim.
        Collider col = cube.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        // Objeyi, �nceden Inspector �zerinden atad���n�z stacking container'�n alt�na ta��yoruz.
        cube.transform.SetParent(cubeStackParent);
        // Yeni k�p i�in ba�lang��ta taban pozisyonuna (en altta, yani firstCubeYOffset) yerle�tirelim.
        cube.transform.localPosition = new Vector3(0, firstCubeYOffset, firstCubeZOffset);
        // Listenin sonuna ekleyin: bu sayede toplama s�ras�, ilk toplanan�n index 0�da kalmas�n� sa�lar.
        collectedCubes.Add(cube);
    }
}
