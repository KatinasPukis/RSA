using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace RSA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            

        }
        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }
        private void CheckNumbers(int number)
        {
            
            if(IsPrime(number)== false)
            {
                MessageBox.Show(number + " " + "is not a prime number");
            }
        }
        private int CommonDivisor(int a, int b)
        {
            int remainder;
            while(b!=0)
            {
                remainder = a % b;
                a = b;
                b = remainder;
            }
            return a;

        }
        static List<int> TexttoASCII(string text)
        {
            List<int> number = new List<int>();
            foreach (char words in text)
            {
                number.Add(words);
            }
            return number;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Decryption();
            }
            string text = richTextBox1.Text;
            int exponent = 0;
            int p = int.Parse(TextboxP.Text);
            int q = int.Parse(TextBoxQ.Text);
            CheckNumbers(p);
            CheckNumbers(q);
            int n = p * q;
            Console.WriteLine("n="+ " "+n);
            int Fn = (p - 1) * (q - 1);
            Console.WriteLine("F(n)="+" "+Fn);
            for(int i =2; i<Fn; i++)
            {
                if(CommonDivisor(i,Fn)==1)
                {
                    exponent = i;
                    if(exponent>1)
                    {
                        break;
                    }
                }
            }
            Console.WriteLine("Public key="+" "+ "e="+" "+exponent+" "+ "n="+" "+n);
            string publicKey = exponent + "\n" + n;
            File.WriteAllText(@"C:\Users\domin\source\repos\RSA\RSA\publickey.txt", publicKey);

            if (checkBox1.Checked)
            {
                Encryption(text,exponent,n);
            }
            
        }
        public int PrivateKey(int fn, int exponent)
        {
            int d = 2;
            while (d* exponent %fn !=1)
            {
                d++;
            }
            return d;
        }
        private void Encryption(string text, int exponent, int n)
        {
            List<int> textNumber = TexttoASCII(text);
            List<double> encryptedNumber= new List<double>();
            string encryptedText = "";
            for (int i = 0; i < text.Length; i++)
            {
                double number = Math.Pow(textNumber[i], exponent);
                double finalnumber = number % n;
                encryptedNumber.Add(finalnumber);
                encryptedText += (char)(finalnumber);


            }

            richTextBox1.Text = encryptedText;
            File.WriteAllText(@"C:\Users\domin\source\repos\RSA\RSA\encryptedText.txt", encryptedText);

        }
        public void Decryption()
        {
            string text = richTextBox1.Text;
            string line;
            List<string> allLinesText = File.ReadAllLines(@"C:\Users\domin\source\repos\RSA\RSA\publickey.txt").ToList();
            int n = Int32.Parse(allLinesText[1]);
            int p = 2;
            int q = 0;
            while(n % p > 0)
            {
                p++;
            }
            q = n / p;
            TextboxP.Text = q.ToString();
            TextBoxQ.Text = p.ToString();
            int fn = (p - 1) * (q - 1);
            int d = PrivateKey(fn, Int32.Parse(allLinesText[0]));
            List<int> EncryptedNumber = TexttoASCII(text);
            List<BigInteger> decryptedNumber = new List<BigInteger>();
            string decryptedText = "";
            for (int i = 0; i < text.Length; i++)
            {

                BigInteger bigInteger = BigInteger.Pow(EncryptedNumber[i], d);
                BigInteger finalnumber = bigInteger % n;
                decryptedNumber.Add(finalnumber);
                decryptedText += (char)(finalnumber);


            }
            richTextBox1.Text = decryptedText;




        }

        private void readTextButon_Click(object sender, EventArgs e)
        {
            string readText = File.ReadAllText(@"C:\Users\domin\source\repos\RSA\RSA\encryptedText.txt");
            richTextBox1.Text = readText;
        }
    }
}
