using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // Швидкість автоматичного прокручування цього шару фону ліворуч
    public float scrollSpeed = 0.5f;
    
    // Ширина картинки (спрайту) в одиницях Unity
    private float width;
    
    // Початкова стартова позиція об'єкта на сцені
    private Vector3 startPos;

    // Метод Start викликається один раз на самому початку гри
    void Start()
    {
        // Запам'ятовуємо початкові координати фону
        startPos = transform.position;
        
        // Автоматично вираховуємо точну ширину картинки по осі X.
        // GetComponent<SpriteRenderer>().bounds.size.x повертає фізичний розмір спрайту у світових одиницях.
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Метод Update викликається кожного кадру гри
    void Update()
    {
        // Постійно рухаємо шар фону ліворуч (Vector3.left) із заданою швидкістю.
        // Time.deltaTime забезпечує стабільну швидкість незалежно від кількості кадрів в секунду (FPS).
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // Перевіряємо, чи змістився фон ліворуч більше, ніж на власну ширину від початкової позиції
        if (transform.position.x <= startPos.x - width)
        {
            // Якщо картинка повністю пролетіла екран, миттєво телепортуємо її праворуч.
            // Зміщення на width * 2 використовується для безшовного з'єднання двох однакових картинок фону,
            // які рухаються одна за одною, створюючи ілюзію нескінченного полотна.
            transform.position += new Vector3(width * 2, 0, 0);
        }
    }
}