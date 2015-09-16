using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Security.Cryptography;

namespace DigitalSignature
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OutageDocument outageDocument = new OutageDocument();
                outageDocument.ApplicantName = textBox1.Text;
                outageDocument.AreaOfResponsibility = Convert.ToString(comboBox1.SelectedItem);
                outageDocument.Substation = Convert.ToString(comboBox2.SelectedItem);
                outageDocument.StartDateTime = dateTimePicker1.Value;
                outageDocument.EndDateTime = dateTimePicker2.Value;
                textBox2.Text = JsonConvert.SerializeObject(outageDocument);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private string generateSHA256Hash(string jsonString)
        {
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(jsonString);           
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashedBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashedBytes);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text = generateSHA256Hash(textBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            string publicXMLKey = rsa.ToXmlString(false);
            string privateXMLKey = rsa.ToXmlString(true);
            textBox5.Text = privateXMLKey;
            textBox6.Text = publicXMLKey;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
                rsa.FromXmlString(textBox5.Text);
                byte[] data = Encoding.UTF8.GetBytes(textBox3.Text);
                var encryptedData = rsa.SignData(data, new SHA256CryptoServiceProvider());
                textBox4.Text = Convert.ToBase64String(encryptedData);
            }
            catch (Exception exception)
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                textBox10.Text = generateSHA256Hash(textBox7.Text);

                RSACryptoServiceProvider client = new RSACryptoServiceProvider(2048);
                client.FromXmlString(textBox11.Text);
                var signedBytes = Convert.FromBase64String(textBox8.Text);
                byte[] fingerprintBytes = Encoding.UTF8.GetBytes(textBox10.Text);
                //MessageBox.Show(client.VerifyData(fingerprintBytes, new SHA256CryptoServiceProvider(), signedBytes).ToString());
                if (client.VerifyData(fingerprintBytes, new SHA256CryptoServiceProvider(), signedBytes) == true)
                {
                    label7.Visible = true;
                    label9.Visible = false;
                }
                else
                {
                    label7.Visible = false;
                    label9.Visible = true;
                }

            }
            catch (Exception exception)
            {
                label7.Visible = false;
                label9.Visible = true;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.button4_Click(this, new EventArgs());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox8.Text = textBox4.Text;
            textBox7.Text = textBox2.Text;
            textBox11.Text = textBox6.Text;
            label7.Visible = false;
            label9.Visible = false;
        }
    }
}
