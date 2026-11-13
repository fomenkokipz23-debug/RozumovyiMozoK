using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    // Префаб труби (перешкоди), який ми будемо створювати на сцені
    public GameObject pipe;
    
    // Частота спавну (кожні 2 секунди з'являється нова труба)
    public float spawnRate = 2f;
    
    // Максимальне відхилення труби по вертикалі вгору і вниз від точки спавнера
    public float heightOffset = 7;
    
    // Затримка перед першим спавном після старту гри (5 секунд)
    public float startDelay = 5f;

    // Метод Start викликається один раз на самому початку гри
    void Start()
    {
        // Використовуємо вбудовану функцію Unity InvokeRepeating.
        // Вона автоматично викликає метод "spawnPipe" через час "startDelay"
        // і потім повторює цей виклик кожні "spawnRate" секунд.
        InvokeRepeating("spawnPipe", startDelay, spawnRate);
    }

    // Власний метод, який безпосередньо створює трубу на сцені
    void spawnPipe()
    {
        // Розраховуємо найнижчу можливу точку для спавну по осі Y
        float lowestPoint = transform.position.y - heightOffset;
        
        // Розраховуємо найвищу можливу точку для спавну по осі Y
        float highestPoint = transform.position.y + heightOffset;

        // Створюємо новий об'єкт (трубу) за допомогою Instantiate.
        // Позиція X залишається як у спавнера (transform.position.x).
        // Позиція Y обирається випадково (Random.Range) між найнижчою та найвищою точками.
        // Позиція Z дорівнює 0 (для 2D гри).
        // Обертання (rotation) копіюється з самого спавнера.
        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
    }
}