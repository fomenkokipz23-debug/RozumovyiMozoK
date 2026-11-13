using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    // Коефіцієнт паралаксу від 0 до 1, який визначає, наскільки сильно шар реагує на рух камери.
    // Атрибут [Range] створює зручний повзунок в інспекторі Unity.
    // 0 = шар стоїть на місці (як дуже далекі зорі), 1 = шар рухається синхронно з камерою.
    [Range(0f, 1f)]
    public float parallaxFactor = 0.3f;

    // Посилання на компонент Transform головної камери
    private Transform cam;
    
    // Позиція камери у попередньому кадрі (потрібна для обчислення зміщення)
    private Vector3 lastCamPos;

    // Метод Start викликається один раз на самому початку гри
    void Start()
    {
        // Знаходимо головну камеру на сцені через Camera.main і беремо її Transform
        cam = Camera.main.transform;
        
        // Запам'ятовуємо стартову позицію камери
        lastCamPos = cam.position;
    }

    // Метод LateUpdate викликається кожного кадру, але строго ПІСЛЯ того, як відпрацювали всі звичайні Update.
    // Це ідеальне місце для логіки камер та фонів.
    void LateUpdate()
    {
        // Обчислюємо дельту (різницю / зміщення) між поточною позицією камери та її позицією у минулому кадрі
        Vector3 delta = cam.position - lastCamPos;

        // Зміщуємо наш шар фону відповідно до руху камери, але множимо це зміщення на коефіцієнт інерції (parallaxFactor).
        // По осі Z зміщення дорівнює 0, оскільки гра двовимірна (2D).
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);

        // Оновлюємо значення минулої позиції камери, записуючи туди поточні координати для наступного кадру
        lastCamPos = cam.position;
    }
}