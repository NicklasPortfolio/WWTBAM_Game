using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Question = WhoWantsToBeAMillionaire.XMLEditor.Question;

namespace WhoWantsToBeAMillionaire
{
    public partial class WWTBAM : Form
    {
        Random rand = new Random();

        private List<Button> buttons;
        private List<Label> labels;
        private List<int> usedQuestions = new List<int>();
      
        private int questionCounter = 1;
        private int secondsCounter = 0;
        private string date = DateTime.Now.Date.ToShortDateString();

        private Question GetItems(int requestedDifficulty)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Question>));
            List<Question> items;

            using (Stream reader = new FileStream("questions.xml", FileMode.Open))
            {
                items = (List<Question>)serializer.Deserialize(reader);
            }

            List<Question> questions = items.FindAll(item => item.difficulty == requestedDifficulty);

            if (questions.Count > 0)
            {
                int randomIndex;
                Question selectedQuestion;
                do
                {
                    randomIndex = rand.Next(0, questions.Count);
                    selectedQuestion = questions[randomIndex];
                }
                while (usedQuestions.Contains(selectedQuestion.questionIndex));

                usedQuestions.Add(selectedQuestion.questionIndex);
                return selectedQuestion;
            }
            
            return null;
        }

        private void ChangeButtons(Question question)
        {
            lblQst.Text = question.questionText;

            HashSet<int> generatedNumbers = new HashSet<int>(); 

            for (int i = 0; i < 4; i++)
            {
                int randomNumber;
                do
                {
                    randomNumber = rand.Next(0, 4);
                }
                while (generatedNumbers.Contains(randomNumber)); 

                generatedNumbers.Add(randomNumber); 
                buttons[randomNumber].Text = question.options[i];
                if (question.correctIndex == i)
                {
                    buttons[randomNumber].Tag = "correct";
                }
            }
        }

        private void NextLabel()
        {
            labels[questionCounter - 2].Text = labels[questionCounter - 2].Text.Trim(new char[] { '<' });
            labels[questionCounter - 1].Text += " <";
        }

        private void PlayAudio(string fileName, bool loop)
        {
            SoundPlayer audio = new SoundPlayer(fileName);

            if (loop)
            {
                audio.PlayLooping();
            }
            else
            {
                audio.Play();
            }
        }
        public WWTBAM()
        {
            InitializeComponent();

            buttons = new List<Button>
            {
                btnAns1,
                btnAns2, 
                btnAns3,
                btnAns4,
            };

            labels = new List<Label>
            {
                lbl1,
                lbl2,
                lbl3,
                lbl4,
                lbl5,
                lbl6,
                lbl7,
                lbl8,
                lbl9,
                lbl10,
                lbl11,
                lbl12,
                lbl13,
                lbl14,
                lbl15,
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XMLEditor program = new XMLEditor();
            program.SetItems();
            ChangeButtons(GetItems(1));
            PlayAudio(@"Audio\To1000.wav", true);
        }

        private void AnsButtonClicked(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            Question question;

            button.BackColor = Color.Gold;
            button.Update();

            PlayAudio(@"Audio\FinalAnswer.wav", false);
            System.Threading.Thread.Sleep(4000);

            if ((string)button.Tag == "correct")
            {
                button.BackColor = Color.ForestGreen;
                button.Update();

                PlayAudio(@"Audio\Correct.wav", false);
                System.Threading.Thread.Sleep(4000);

                if (questionCounter == 15)
                {
                    PlayAudio(@"Audio\Win.wav", false);
                    MessageBox.Show($"Congratulations! You've won the million!", "You won!");
                    this.Close();
                }
                else
                {
                    questionCounter++;
                    if (questionCounter < 5)
                    {
                        question = GetItems(1);
                        PlayAudio(@"Audio\To1000.wav", true);
                    }
                    else if (questionCounter > 4 && questionCounter < 10)
                    {
                        question = GetItems(2);
                        PlayAudio(@"Audio\To32000.wav", true);
                    }
                    else
                    {
                        question = GetItems(3);
                        PlayAudio(@"Audio\To1M.wav", true);
                    }

                    button.Tag = null;
                    ChangeButtons(question);
                    NextLabel();
                }
            }
            else
            {
                button.BackColor = Color.Firebrick;
                button.Update();

                PlayAudio(@"Audio\Incorrect.wav", false);
                System.Threading.Thread.Sleep(4100);

                MessageBox.Show($"You lost! See you next time!", "You lost!");
                this.Close();
            }

            button.BackColor = Color.MidnightBlue;
            button.Update();

            foreach (Button buttonObject in buttons)
            {
                buttonObject.Enabled = true;
            }
        }
        private void btn5050_Click(object sender, EventArgs e)
        {
            List<Button> tobeDisabled = new List<Button>();

            foreach (Button button in buttons)
            {
                if (tobeDisabled.Count == 2)
                {
                    break;
                }
                else if ((string)button.Tag != "correct")
                {
                    tobeDisabled.Add(button);
                }
            }

            foreach (Button button in tobeDisabled)
            {
                button.Enabled = false;

            }
            SoundPlayer sound = new SoundPlayer(@"Audio\5050Sound.wav");
            sound.Play();

            btn5050.Enabled = false;
            btn5050.BackgroundImage = null;
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            secondsCounter++;
            lblDate.Text = $"{date} | {secondsCounter} seconds";
        }
    }
}
