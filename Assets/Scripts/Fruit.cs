using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] GameObject[] Prefab; // Array prefab buah yang akan di-spawn
    [SerializeField] float secondSpawn = 10f; // Waktu jeda antar spawn (default: 10 detik)
    [SerializeField] float minTras; // Posisi X minimum untuk spawn
    [SerializeField] float maxTras; // Posisi X maksimum untuk spawn

    void Start()
    {
        Debug.Log($"Starting fruit spawn with interval: {secondSpawn} seconds."); // Debugging nilai awal
        StartCoroutine(FruitSpawn());
    }

    IEnumerator FruitSpawn()
    { 
        while (true)
        {
            Debug.Log($"Spawning buah dalam {secondSpawn} detik."); // Debugging setiap spawn

            // Tentukan posisi spawn acak 
            var wanted = Random.Range(minTras, maxTras);
            var position = new Vector3(wanted, transform.position.y, transform.position.z);

            // Pilih buah pisang spawn di grid
            GameObject spawnedFruit = Instantiate(
                Prefab[Random.Range(0, Prefab.Length)],
                position,
                Quaternion.identity
            );

            // Tambahkan komponen FruitCollect jika belum ada
            if (spawnedFruit.GetComponent<FruitCollect>() == null)
            {
                spawnedFruit.AddComponent<FruitCollect>();
            }

            // Tunggu beberapa detik sebelum spawn berikutnya
            yield return new WaitForSeconds(secondSpawn);

            // Hancurkan objek setelah 5 detik
            Destroy(spawnedFruit, 5f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MonkeyController>() != null)
        {
            Debug.Log("Pisang diambil!");

            // Tambahkan skor menggunakan GameManager
            GameManager.Instance.AddScore(10); // Menambah 10 poin untuk setiap buah

            // Hancurkan buah setelah bersentuhan
            Destroy(gameObject);
        }
    }


}


