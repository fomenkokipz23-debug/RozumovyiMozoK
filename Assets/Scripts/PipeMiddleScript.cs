using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMiddleScript : MonoBehaviour
{
    // Посилання на загальний скрипт логіки гри для додавання балів
    public LogicScript logic;
    
    // Текстовий тег, за яким ми будемо шукати об'єкт логіки на сцені
    private string logicTag = "Logic";
    
    // Метод Start викликається один раз на самому початку гри для цієї невидимої зони
    void Start()
    {
        // Динамічно шукаємо на всій сцені ігровий об'єкт із тегом "Logic" (GameObject.FindWithTag),
        // після чого витягуємо з нього компонент LogicScript і записуємо в наше посилання.
        logic = GameObject.FindWithTag(logicTag).GetComponent<LogicScript>();
    }

    // Метод Update викликається кожного кадру (зараз він порожній, оскільки постійна логіка тут не потрібна)
    void Update()
    {
        
    }
    
    // Вбудований метод Unity для відстеження перетину примарного (тригерного) колайдера.
    // Викликається автоматично в той момент, коли колайдер пташки входить в зону між трубами.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Звертаємося до нашого менеджера логіки і викликаємо публічний метод addScore,
        // передаючи туди 1 бал за успішне проходження перешкоди.
        logic.addScore(1);
    }
}