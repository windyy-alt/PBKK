using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        TextBox txtDisplay;

        double angka1 = 0;
        double angka2 = 0;
        string operasi = "";

        public Form1()
        {
            InitializeComponent();

            BuatTampilan();
        }


        private void BuatTampilan()
        {
            this.Text = "Kalkulator Sederhana";
            this.Size = new Size(350, 450);
            this.StartPosition = FormStartPosition.CenterScreen;


            // Display
            txtDisplay = new TextBox();
            txtDisplay.Location = new Point(20, 20);
            txtDisplay.Size = new Size(290, 50);
            txtDisplay.Font = new Font("Arial", 20);
            txtDisplay.TextAlign = HorizontalAlignment.Right;
            txtDisplay.ReadOnly = true;
            txtDisplay.Text = "0";

            this.Controls.Add(txtDisplay);


            string[] tombol =
            {
                "7","8","9","/",
                "4","5","6","*",
                "1","2","3","-",
                "0","C","=","+"
            };


            int x = 20;
            int y = 90;


            foreach (string teks in tombol)
            {
                Button btn = new Button();

                btn.Text = teks;
                btn.Size = new Size(65, 50);
                btn.Location = new Point(x, y);
                btn.Font = new Font("Arial", 14);

                btn.Click += Tombol_Click;

                this.Controls.Add(btn);


                x += 70;

                if (x > 230)
                {
                    x = 20;
                    y += 60;
                }
            }
        }


        private void Tombol_Click(object sender, EventArgs e)
        {
            Button tombol = (Button)sender;


            string nilai = tombol.Text;


            // angka
            if (char.IsDigit(nilai[0]))
            {
                if (txtDisplay.Text == "0")
                    txtDisplay.Text = nilai;
                else
                    txtDisplay.Text += nilai;

                return;
            }


            // clear
            if (nilai == "C")
            {
                txtDisplay.Text = "0";
                angka1 = 0;
                angka2 = 0;
                operasi = "";
                return;
            }


            // operator
            if (nilai == "+" ||
               nilai == "-" ||
               nilai == "*" ||
               nilai == "/")
            {
                angka1 = double.Parse(txtDisplay.Text);
                operasi = nilai;
                txtDisplay.Text = "0";

                return;
            }


            // hasil
            if (nilai == "=")
            {
                angka2 = double.Parse(txtDisplay.Text);

                double hasil = 0;


                switch (operasi)
                {
                    case "+":
                        hasil = angka1 + angka2;
                        break;

                    case "-":
                        hasil = angka1 - angka2;
                        break;

                    case "*":
                        hasil = angka1 * angka2;
                        break;

                    case "/":

                        if (angka2 == 0)
                        {
                            MessageBox.Show(
                                "Tidak dapat membagi dengan nol",
                                "Error"
                            );

                            return;
                        }

                        hasil = angka1 / angka2;
                        break;
                }


                txtDisplay.Text = hasil.ToString();
            }
        }
    }
}