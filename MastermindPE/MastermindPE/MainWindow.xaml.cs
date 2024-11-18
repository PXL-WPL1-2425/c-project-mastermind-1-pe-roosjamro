using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MastermindPE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> availableColors = new List<string> { "Red", "Yellow", "Orange", "White", "Green", "Blue" };
        private List<string> secretCode = new List<string>();
        private int score;

        private int attempts;

        DispatcherTimer timer;
        DateTime clicked;
        TimeSpan interval = new TimeSpan(0, 0, 10);

        public MainWindow()
        {
            InitializeComponent();
            secretCode = GenerateRandomCode();
            PopulateComboBoxes();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;

            StartCountDown();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            interval = DateTime.Now - clicked;

            timerTextBox.Text = $"{interval.Seconds}:{interval.Milliseconds.ToString().PadLeft(2, '0')}";
        }

        private void CheckCode_Click(object sender, RoutedEventArgs e)
        {
            List<string> playerGuess = new List<string>
            {
                ComboBox1.SelectedItem?.ToString(),
                ComboBox2.SelectedItem?.ToString(),
                ComboBox3.SelectedItem?.ToString(),
                ComboBox4.SelectedItem?.ToString()
            };

            if (playerGuess.Contains(null))
            {
                MessageBox.Show("Vul alle kleuren in.");
                return;
            }

            int roundScore = 0;
            bool[] codeMatched = new bool[4];
            bool[] guessMatched = new bool[4];

            for (int i = 0; i < 4; i++)
            {
                if (playerGuess[i] == secretCode[i])
                {
                    (FindName($"Label{i + 1}") as Label).BorderBrush = Brushes.DarkRed;
                    roundScore += 2;
                    codeMatched[i] = guessMatched[i] = true;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (!guessMatched[i])
                {
                    bool colorMatchFound = false;
                    for (int j = 0; j < 4; j++)
                    {
                        if (!codeMatched[j] && playerGuess[i] == secretCode[j])
                        {
                            colorMatchFound = true;
                            codeMatched[j] = true;
                            break;
                        }
                    }

                    var label = (Label)FindName($"Label{i + 1}");
                    label.BorderBrush = colorMatchFound ? Brushes.Wheat : Brushes.Gray;
                    if (colorMatchFound) roundScore += 1;
                }
            }

            score += roundScore;
            ScoreText.Text = $"Score: {score}";


            if (roundScore == 8)
            {
                MessageBox.Show("Gefeliciteerd! Je hebt de code gekraakt!");
                StartGame();
            }

            attempts++;

            if (CheckCode == clicked)
            {
                timer.Start();
            }

        }

        private List<string> GenerateRandomCode()
        {
            Random random = new Random();
            List<string> code = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                string color = availableColors[random.Next(availableColors.Count)];
                code.Add(color);
            }
            return code;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = $"Poging: { string.Join(",", attempts)}";

            codeTextBox.Text = $"{ string.Join(",", secretCode)}";

           
        }

        private void PopulateComboBoxes()
        {
            ComboBox[] comboBoxes = { ComboBox1, ComboBox2, ComboBox3, ComboBox4 };
            for (int i = 0; i < comboBoxes.Length; i++)
            {
                comboBoxes[i].ItemsSource = availableColors;
            }
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem != null)
            {
                int index = int.Parse(comboBox.Name.Substring(comboBox.Name.Length - 1)) - 1;
                Label label = (Label)FindName($"Label{index + 1}");
                label.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(comboBox.SelectedItem.ToString());
            }
        }

        private void StartGame()
        {
            secretCode = GenerateRandomCode();
            PopulateComboBoxes();
            score = 0;
            ScoreText.Text = $"Score: {score}";
            ResetLabelBorders();

           


        }

        private void ResetLabelBorders()
        {
            for (int i = 1; i <= 4; i++)
            {
                Label label = (Label)FindName($"Label{i}");
                label.BorderBrush = Brushes.Transparent;
            }
        }

        private void toggledebug()
        {

        }
        
        //start de timer
        private void StartCountDown()
        {
            timer.Start();
        }

        //stopt de timer
        private void StopCountDown()
        {
            
        }

    }
}
