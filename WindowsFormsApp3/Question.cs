using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CSharp
{
    class Answer
    {
        public string AnswerText;
        private bool IsRight;

        public Answer(string text, bool isRight)
        {
            IsRight = isRight;
            for (int i = 0; i < text.Length; i++)
            {
                if(i % 150 == 0 && i != 0)
                    AnswerText += "\n";
                AnswerText += text[i];
            }
        }

        public bool GetIsRight()
        {
            return IsRight;
        }
    }

    class Question
    {
        public string QestionText;
        public List<Answer> Answers;
        public int RightAnswersCount;

        public Question(string text, List<Answer> answers)
        {
            QestionText = text;
            Answers = answers;
            RightAnswersCount = 0;
            for (int i = 0; i < Answers.Count; i++)
            {
                if (Answers[i].GetIsRight())
                    RightAnswersCount++;
            }
        }
    }
}
