using System;
using System.Windows.Forms;
using AmberMeet.Infrastructure.Utilities;
using AmberMeet.Test.DataSimulation.Models;

namespace AmberMeet.Test.DataSimulation
{
    public partial class BuildTestData : Form
    {
        private readonly AppDataBuilder _appDataBuilder;

        public BuildTestData()
        {
            InitializeComponent();
            _appDataBuilder = new AppDataBuilder();
        }

        /// <summary>
        ///     虚构用户按钮点击
        /// </summary>
        private void FictitiouUsersButton_Click(object sender, EventArgs e)
        {
            try
            {
                FictitiouUsersButton.Text = @"正在建立测试用户......";
                FictitiouUsersButton.Enabled = false;
                ControlBox = false;
                var testUsers = _appDataBuilder.BuildTestUsers();
                foreach (var testUser in testUsers)
                {
                    ServiceFactory.OrgUserService.AddUser(testUser);
                }
                MessageHelper.Show("需求用户完成，用户密码为系统默认密码");
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                MessageHelper.Show(ex);
            }
            finally
            {
                FictitiouUsersButton.Text = @"虚构用户";
                FictitiouUsersButton.Enabled = true;
                ControlBox = true;
            }
        }
    }
}