using System;
using System.Windows.Forms;

namespace AmberMeet.Test.DataSimulation.Models
{
    internal class MessageHelper
    {
        public static void Show(string msg)
        {
            MessageBox.Show(msg);
            //new MessagePrompt(msg).ShowDialog();
        }

        public static void Show(Exception ex)
        {
            MessageBox.Show($"出现异常：{ex.Message}");
            //new MessagePrompt($"出现异常：{ex.Message}").ShowDialog();
        }
    }
}