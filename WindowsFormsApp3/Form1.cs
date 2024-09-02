using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2CSharp
{
    public partial class Form1 : Form
    {
        private Question question;
        private Button nextButton = new Button();
        Label errorLable = new Label();
        private int numberOfQuestion = 0;
        private List<CheckBox> checkBoxes = new List<CheckBox>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            DialogResult dialogResult = openFileDialog1.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void NextQuestion()
        {
            Controls.Clear();
            StreamReader streamReader = new StreamReader(textBox1.Text);
            string readedString;
            string questionText = " ";
            int questionsCounter = 0;
            bool isQuestionRead = false;
            List<Answer> answers = new List<Answer>();

            while ((readedString = streamReader.ReadLine()) != null)
            {
                if (answers.Count == 4 || questionsCounter == 0)
                {
                    questionsCounter++;
                    if (questionsCounter - 1 == numberOfQuestion)
                    {
                        question = new Question(questionText, answers);
                        isQuestionRead = true;
                        DisplayQuestion();
                        break;
                    }
                    answers.Clear();
                    questionText = readedString;
                }
                else
                {
                    if (readedString[readedString.Length - 1] == '*')
                        answers.Add(new Answer(readedString.Substring(0, readedString.Length - 1), true));
                    else
                        answers.Add(new Answer(readedString, false));
                }
            }
            streamReader.Close();

            if (!isQuestionRead)
            {
                DisplayResults();
            }
        }

        private void DisplayQuestion()
        {
            Label questionLabel = new Label();
            checkBoxes.Clear();

            questionLabel.Location = new Point(10, 5);
            questionLabel.AutoSize = true;
            questionLabel.MaximumSize = new Size(1000, 125);
            questionLabel.Text = "Задание " + numberOfQuestion + ": " + question.QestionText;
            Controls.Add(questionLabel);

            for (int i = 0; i < question.Answers.Count; i++)
            {
                checkBoxes.Add(new CheckBox());
                checkBoxes[i].Location = new Point(10, questionLabel.Size.Height + 10 + 400 / question.Answers.Count * i);
                checkBoxes[i].AutoSize = true;
                checkBoxes[i].Text = question.Answers[i].AnswerText;
                Controls.Add(checkBoxes[i]);
            }

            errorLable.Text = "";
            Controls.Add(errorLable);
            Controls.Add(nextButton);
        }

        private void WriteResult(int rightChoisedAnswers, int choisedAnswers)
        {
            if (numberOfQuestion == 1)
            {
                StreamWriter streamWriter = new StreamWriter("Results.txt", false);
                streamWriter.WriteLine("1) " + rightChoisedAnswers.ToString() + " из " + question.RightAnswersCount.ToString());
                streamWriter.Close();
            }
            else
            {
                StreamWriter streamWriter = new StreamWriter("Results.txt", true);
                streamWriter.WriteLine(numberOfQuestion.ToString() + ") " + rightChoisedAnswers.ToString() + " из " + question.RightAnswersCount.ToString());
                streamWriter.Close();
            }
        }

        private void DisplayResults()
        {
            StreamReader streamReader = new StreamReader("Results.txt");
            string readedString;
            int i = 0;

            Label resultLable = new Label();
            resultLable.Location = new Point(25, 25);
            resultLable.Font = new Font(resultLable.Font.Name, 12, resultLable.Font.Style);
            resultLable.Text = "Результаты";
            Controls.Add(resultLable);

            while ((readedString = streamReader.ReadLine()) != null)
            {
                Label label = new Label();
                label.MaximumSize = new Size(125, 13);
                label.Location = new Point(25 + 150 * (i / 15), 50 + 25 * (i % 15));
                label.Text = readedString;
                Controls.Add(label);
                i++;
            }
            streamReader.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                nextButton.Text = "ПРОДОЛЖИТЬ";
                nextButton.Location = new Point(865, 470);
                nextButton.Size = new Size(150, 50);
                nextButton.Click += button3_Click;

                errorLable.ForeColor = Color.Red;
                errorLable.Location = new Point(865, 425);
                errorLable.AutoSize = true;
                errorLable.MaximumSize = new Size(150, 50);

                numberOfQuestion++;
                NextQuestion();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int rightChoiseCount = 0;
            int answerChoisedCount = 0;

            for (int i = 0; i < question.Answers.Count; i++)
            {
                if (checkBoxes[i].Checked)
                {
                    answerChoisedCount++;
                    if (question.Answers[i].GetIsRight())
                    {
                        rightChoiseCount++;
                    }
                }
            }

            if (answerChoisedCount > 0)
            {
                WriteResult(rightChoiseCount, answerChoisedCount);
                numberOfQuestion++;
                NextQuestion();
                errorLable.Text = "";
            }
            else
            {
                errorLable.Text = "Выберите вариант ответа!";
            }
        }
    }
}


