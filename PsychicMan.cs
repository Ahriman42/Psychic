using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psychic
{
    /// <summary>
    /// Класс экстрасенса
    /// </summary>
    public class PsychicMan
    {
        // Имя экстрасенса
        public string Name { get; set; }
        // Достоверность экстрасенса
        public int Power { get; set; }
        // Количество правильных ответов экстрасенса
        public int correctAnswers { get; set; }
        // Текущий ответ экстрасенса
        public int Answer { get; set; }
        // Список ответов экстрасенса
        public List<int> AnswersList { get; set; } = new List<int>();
        public PsychicMan()
        {
        }
        public PsychicMan(string _name)
        {
            Name = _name;
            Power = 0;
        }

        /// <summary>
        /// Выбирает один из видов ответов для экстрасенса.
        /// 0 - диапазон от 10 до 99;
        /// 1 - диапазон от 10 до 49;
        /// 2 - диапазон от 50 до 99.
        /// </summary>
        /// <param name="_counter"></param>
        public void SetAnswer(int _counter)
        {
            Random rnd = new Random();
            int answer;
            switch (_counter)
            {
                case 0:
                    answer = rnd.Next(10, 100);
                    break;
                case 1:
                    answer = rnd.Next(10, 50);
                    break;
                case 2:
                    answer = rnd.Next(50, 100);
                    break;
                default:
                    answer = 0;
                    break;
            }
            AnswersList.Add(answer);
            Answer = answer;
        }

        /// <summary>
        /// Расчет достоверности экстрасенса
        /// </summary>
        /// <param name="_playerAnswers">Список загаданных игроком чисел</param>
        public void SetPower(List<int> _playerAnswers)
        {
            if(Answer == _playerAnswers.Last())
            {
                correctAnswers++;
            }
            Power = (int)(((double)correctAnswers / _playerAnswers.Count()) * 100);
        }
    }
}
