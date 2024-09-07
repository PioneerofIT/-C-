using System.ComponentModel;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MakeWordApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged //INotifyPropertyChanged 인터페이스
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string Name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) handler(this, new PropertyChangedEventArgs(Name));
        }
        private string wrongStatus;
        private string selEng;
        private string selKor;
        private string message;
        private List<char> btns = new List<char>();

        public string WrongStatus
        {
            get => wrongStatus;
            set
            {
                wrongStatus = value;
                OnPropertyChanged("WrongStatus");
            }
        }
        public string SelEng
        {
            get => selEng;
            set
            {
                selEng = value;
                OnPropertyChanged("selEng");
            }
        }
        public string SelKor
        {
            get => selKor;
            set
            {
                selKor = value;
                OnPropertyChanged("selKor");
            }
        }
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged("selKor");
            }
        }
        List<char> SelWord = new List<char>();
        List<string> words = new List<string>()
        {
            "boy,소년",
            "school, 학교",
            "car,자동차",
            "book,책"
        };
        int wrong = 0;
        int maxWrong = 3;
        string compareWord = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this; // MainWindow의 모든 데이터 바인딩
            btns.AddRange("abcdefghijklmnopqrstuvwxyz");
            ic.ItemsSource = btns;
            RandomWord();
            ChangeWorad(SelEng, SelWord);
            WrongStatus = $"틀린횟수: {wrong} of {maxWrong}";
        }

        private void ChangeWorad(string selEng, List<char> selWord)
        {
            char[] result = selEng.Select(x => (selWord.IndexOf(x) >= 0 ? x : '*')).ToArray();
            SelEng = string.Join(' ', result);
        }

        private void RandomWord()
        {
            string[] selChar = words[new Random().Next(0, words.Count)].Split(",");
            SelEng = selChar[0];
            SelKor = selChar[1];
            compareWord= SelEng;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var result = btn.Content.ToString();
                CheckWord(result[0]);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            wrong = 0;
            SelWord = new List<char>();
            RandomWord();
            ChangeWorad(SelEng, SelWord);
            Message = "알파벳을 선택하여 주세요";
            foreach (var a in ic.Items)
            {
                var btn = (UIElement)ic.ItemContainerGenerator.ContainerFromItem(a); //지정된 항목에 해당하는 UI 속성 반환
                if (btn != null)
                {
                    btn.IsEnabled = true;
                }
            }

        }

        private void CheckWord(char v)
        {
            if(SelWord.IndexOf(v)  == -1)
            {
                SelWord.Add(v);
            }
            if(compareWord.IndexOf(v) >= 0)
            {
                ChangeWorad(compareWord, SelWord);
                CheckWin();
            }
            else if(compareWord.IndexOf(v) == -1)
            {
                wrong++;
                Status();
                CheckLost();
            }
        }

        private void Status()
        {
            WrongStatus = $"틀린횟수: {wrong} of {maxWrong}";
        }

        private void CheckWin()
        {
            if (compareWord == SelEng.Replace(" ", " "))
            {
                Message = " You win!";
            }
        }
        private void    CheckLost()
        {
            if(wrong == maxWrong)
            {
                Message = "You Lost!";
                foreach(var a in ic.Items)
                {
                    var btn = (UIElement) ic.ItemContainerGenerator.ContainerFromItem(a);
                    if(btn != null)
                    {
                        btn.IsEnabled = false;
                    }
                }
            }
        }
    }
}