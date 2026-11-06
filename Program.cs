using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfTest.Entities
{
    // Сутність, що представляє варіант відповіді на питання
    [Table("Answers")]
    public class AnswerEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = string.Empty; // Текст відповіді

        public bool IsCorrect { get; set; } // Прапорець: чи є ця відповідь правильною

        [Required]
        [ForeignKey(nameof(Question))]
        public int QuestionId { get; set; } // Зовнішній ключ для зв'язку з питанням

        public QuestionEntity? Question { get; set; } // Навігаційна властивість на питання
    }

    // Сутність, що представляє питання в тесті
    public class QuestionEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(1000)]
        public string Text { get; set; } = null!; // Текст питання

        [Required, ForeignKey(nameof(Test))]
        public int TestId { get; set; } // Зовнішній ключ для зв'язку з тестом

        public TestEntity Test { get; set; } = null!; // Навігаційна властивість на тест

        public ICollection<AnswerEntity> Answers { get; set; } = null!; // Колекція варіантів відповідей для цього питання
    }

    // Сутність, що представляє тест загалом
    [Table("Tests")]
    public class TestEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = null!; // Назва тесту

        [StringLength(1000)]
        public string? Description { get; set; } // Необов'язковий опис тесту

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc); // Дата та час створення тесту

        [Range(1, 60)]
        public int? TimeLimitMinutes { get; set; } // Обмеження часу на проходження (від 1 до 60 хвилин)

        public ICollection<QuestionEntity> Questions { get; set; } = null!; // Колекція питань, що належать до цього тесту
    }
}

namespace WpfTest
{
    // Головний клас контексту бази даних для Entity Framework Core
    public class MyDataContext : DbContext
    {
        // Таблиці бази даних
        public DbSet<WpfTest.Entities.TestEntity> Tests { get; set; }
        public DbSet<WpfTest.Entities.QuestionEntity> Questions { get; set; }
        public DbSet<WpfTest.Entities.AnswerEntity> Answers { get; set; }

        // Конфігурація підключення до бази даних
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Назва файлу, де зберігається локальна база даних SQLite (зберігає тести, питання та відповіді)
            optionsBuilder.UseSqlite("Data Source=MyTest.db");
        }
    }
}