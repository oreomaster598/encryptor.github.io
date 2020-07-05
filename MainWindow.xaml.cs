using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Text.RegularExpressions;

namespace textencryptor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool inithelp = false;


        public static string HextoString(string InputText)
        {

            byte[] bb = Enumerable.Range(0, InputText.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(InputText.Substring(x, 2), 16))
                             .ToArray();
            return Encoding.ASCII.GetString(bb);
        }

        public static string TrimNonAscii(string value)
        {
            string pattern = "[^ -~]+";
            Regex reg_exp = new Regex(pattern);
            return reg_exp.Replace(value, "");
        }

        public static string[] GetAllHexVaules()
        {
            List<string> hex = new List<string>();
            string[] hexChars = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
            int pos = 0;
            int pos2 = 0;
            for (int i = 256; i != 0; i--)
            {
                if (pos == 16)
                {
                    pos = 0;
                    pos2++;
                }
                hex.Add($"{hexChars[pos2]}{hexChars[pos]}");
                pos++;
            }
            return hex.ToArray();
        }

        public static string encrypt(string input, int key)
        {
            string[] hex = GetAllHexVaules();
            string result = "";
            string resultingChars = "";

            foreach (string s in hex)
                result += s;

            result = HextoString(result);
            result = TrimNonAscii(result);
            result = result.Replace("?", string.Empty);
            result = result.Replace(" ", string.Empty);

            foreach (char c in result)
                resultingChars += $"{c},";
            string[] HexChars = resultingChars.Split(',');

            Random rnd = new Random(key);
            string[] rndHexChars = HexChars.OrderBy(x => rnd.Next()).ToArray();

            List<string> usedChars = new List<string>();

            string hexresult = input;

            foreach (char c in input)
            {
                if (!usedChars.Contains(c.ToString()))
                {
                    int index = Array.FindIndex(HexChars, hexc => hexc.Contains(c));
                    hexresult = hexresult.Replace(c.ToString(), rndHexChars[index]);
                    usedChars.Add(c.ToString());
                }
            }
            return hexresult;

        }
        public static string decrypt(string input, int key)
        {
            string[] hex = GetAllHexVaules();
            string result = "";
            string resultingChars = "";

            foreach (string s in hex)
                result += s;

            result = HextoString(result);
            result = TrimNonAscii(result);
            result = result.Replace("?", string.Empty);
            result = result.Replace(" ", string.Empty);

            foreach (char c in result)
                resultingChars += $"{c},";
            string[] HexChars = resultingChars.Split(',');

            Random rnd = new Random(key);
            string[] rndHexChars = HexChars.OrderBy(x => rnd.Next()).ToArray();

            List<string> usedChars = new List<string>();

            string hexresult = input;
            string conversionLog = "";

            foreach (char c in input)
            {
                if (1 == 1)
                {
                    int index = Array.FindIndex(rndHexChars, hexc => hexc.Contains(c));
                    conversionLog += HexChars[index];
                }
            }
            return conversionLog;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            eresult.Visibility = Visibility.Visible;
            eresult.Text = decrypt(textbox.Text, Convert.ToInt32(keybox.Text));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            eresult.Visibility = Visibility.Visible;
            eresult.Text = encrypt(textbox.Text, Convert.ToInt32(keybox.Text));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            inithelp = (inithelp) ? false : true;
            if (inithelp)
            {
                eresult.Visibility = Visibility.Visible;
                eresult.Text = "Enter a key (the key must be a number. some keys may not work)\nEnter text then click encrypt/decrypt";
            }
            if (!inithelp)
            {
                eresult.Visibility = Visibility.Hidden;
                eresult.Text = "";
            }
        }
    }
}
