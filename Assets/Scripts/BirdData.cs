using UnityEngine;

// Атрибут CreateAssetMenu дозволяє створювати нові файли цього типу прямо через контекстне меню Unity 
// (правий клік миші в папці Project -> Create -> Birds -> BirdData).
[CreateAssetMenu(fileName = "NewBird", menuName = "Birds/BirdData")]
public class BirdData : ScriptableObject
{
    // Текстове поле для збереження назви пташки, яка буде відображатися в магазині
    public string birdName;      // Назва пташки для магазину
    
    // Посилання на 2D-картинку (спрайт) цієї конкретної пташки
    public Sprite birdSprite;    // Картинка пташки
    
    // Кількість балів (рекорд), яку гравцю необхідно набрати, щоб розблокувати цю пташку
    public int scoreRequired;    // Скільки перешкод треба пролетіти (наприклад, 10)
    
    // Текстовий унікальний ідентифікатор пташки для збереження вибору гравця через PlayerPrefs
    public string birdId;        // Унікальний ID (наприклад, "bird_blue")
}