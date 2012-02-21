using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace hts_prog_level2
{
  public partial class Form1 : Form
  {

    string[] morseLookup = new string[43];

    public Form1()
    {
      InitializeComponent();
    }

    public string EncodeToMorse(string input)
    {
      string output = "";

      for (int c = 0; c < input.Length; c++)
      {
        int ascii = (int)input[c];

        if ((ascii >= 48 && ascii <= 57) || (ascii >= 64 && ascii <= 90)) // able to convert into morse
          output += morseLookup[ascii - 48];

        if (ascii == 32) // space character
          output += " / "; 
      }

      return output;

    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      if (textBox1.Text.Length > 0)
      {
        textBox1.Text = textBox1.Text.ToUpper();
        textBox1.SelectionStart = textBox1.Text.Length;
        
        if ( radioButton1.Checked )
          textBox2.Text = EncodeToMorse(textBox1.Text);
        if (radioButton2.Checked)
          textBox2.Text = DecodeFromMorse(textBox1.Text);
      }
      else
        textBox2.Text = "";
    }

    public string DecodeFromMorse(string input)
    {
      string decoded = "";

      string[] data = input.Split(' ');

      for (int c = 0; c < data.Length; c++)
      {
        for (int morseCodes = 0; morseCodes < morseLookup.Length; morseCodes++)
        {
          if (data[c] == morseLookup[morseCodes])
          {
            char t = (char)(morseCodes + 48);
            decoded += t;  // convert from ascii code
            break;
          }
        }
      }

      return decoded;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      // setup morse lookup table
      // loaded with characters representing their ascii value - 48 to make a quicker lookup
      morseLookup[0] = "-----";   // 0 -ASCII code is 48, this table is setup as ASCII code - 48
      morseLookup[1] = ".----";   // 1
      morseLookup[2] = "..---";   // 2
      morseLookup[3] = "...--";   // 3
      morseLookup[4] = "....-";   // 4
      morseLookup[5] = ".....";   // 5
      morseLookup[6] = "-....";   // 6
      morseLookup[7] = "--...";   // 7
      morseLookup[8] = "---..";   // 8
      morseLookup[9] = "----.";   // 9

      // characters here in ascii table not used in morse

      morseLookup[17] = ".-";       // A
      morseLookup[18] = "-...";     // B
      morseLookup[19] = "-.-.";     // C
      morseLookup[20] = "-..";      // D
      morseLookup[21] = ".";        // E
      morseLookup[22] = "..-.";     // F
      morseLookup[23] = "--.";      // G
      morseLookup[24] = "....";     // H
      morseLookup[25] = "..";       // I
      morseLookup[26] = ".---";     // J
      morseLookup[27] = "-.-";      // K
      morseLookup[28] = ".-..";     // L
      morseLookup[29] = "--";       // M
      morseLookup[30] = "-.";       // N
      morseLookup[31] = "---";      // O
      morseLookup[32] = ".--.";     // P
      morseLookup[33] = "--.-";     // Q
      morseLookup[34] = ".-.";      // R
      morseLookup[35] = "...";      // S
      morseLookup[36] = "-";        // T
      morseLookup[37] = "..-";      // U
      morseLookup[38] = "...-";     // V
      morseLookup[39] = ".--";      // W
      morseLookup[40] = "-..-";     // X
      morseLookup[41] = "-.--";     // Y
      morseLookup[42] = "--..";     // Z



    }

    /// <summary>
    /// Function to download Image from website
    /// </summary>
    /// <param name="_URL">URL address to download image</param>
    /// <returns>Image</returns>
    public Image DownloadImage(string _URL)
    {
      Image _tmpImage = null;

      try
      {
        // Open a connection
        System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(_URL);

        _HttpWebRequest.AllowWriteStreamBuffering = true;

        // You can also specify additional header values like the user agent or the referer: (Optional)
        _HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
        _HttpWebRequest.Referer = "http://www.google.com/";

        // set timeout for 20 seconds (Optional)
        _HttpWebRequest.Timeout = 20000;

        // Request response:
        System.Net.WebResponse _WebResponse = _HttpWebRequest.GetResponse();

        // Open data stream:
        System.IO.Stream _WebStream = _WebResponse.GetResponseStream();

        // convert webstream to image
        _tmpImage = Image.FromStream(_WebStream);

        // Cleanup
        _WebResponse.Close();
        _WebResponse.Close();
      }
      catch (Exception _Exception)
      {
        // Error
        MessageBox.Show("Exception caught in process: {0}", _Exception.ToString());
        return null;
      }

      return _tmpImage;
    }

    public Image getImageFromURL(string URL)
    {
      HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(URL);
      Request.Method = "GET";
      HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

      //MessageBox.Show(Response.ContentType);
      //Response.ContentType = "image/jpeg";
      ImageConverter imageConverter = new System.Drawing.ImageConverter();

      System.Drawing.Image bmp = Bitmap.FromStream(Response.GetResponseStream(),false,false);

      Response.Close();

      return bmp;
    }

    public void debug(string input)
    {
      listBox1.Items.Add("[*] " + input);
      listBox1.TopIndex = listBox1.Items.Count - 1;
    }

    public string processWhitePixels(Image input)
    {
      Bitmap bmp = new Bitmap(input);

      int count = 0;

      int prevPix = -1;

      string data = "";

      for ( int y = 0; y < bmp.Height; y++ )
        for (int x = 0; x < bmp.Width; x++)
        {
          // debug( bmp.GetPixel(x, y).ToString() );
          Color test = bmp.GetPixel(x, y);

          if ((test.R == 255) && (test.G == 255) && (test.B == 255)) // found white pixel
          {
            // debug("pixel at pos " + count.ToString());
            if (prevPix == -1)
            {
              char t = (char)count;
              data += t;
              prevPix = count;
            }
            else
            {
              char t = (char)(count - prevPix);
              data += t;
              prevPix = count;
            }
          }

          count++;
        }

      debug(data);

      return data;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      // grab image
      if (Clipboard.ContainsImage())
      {
        pictureBox1.Image = Clipboard.GetImage();
        debug("Image Dimensions : (" + pictureBox1.Image.Width.ToString() + "," + pictureBox1.Image.Height.ToString() + ")");
        string morseCodedString = processWhitePixels(pictureBox1.Image);
        if (morseCodedString.Length > 0)
          textBox1.Text = morseCodedString;
      }
      else
      {
        debug("No Image Found on Clipboard");
      }
    }
  }
}
