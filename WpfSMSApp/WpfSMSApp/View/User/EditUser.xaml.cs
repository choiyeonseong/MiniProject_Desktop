using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;

namespace WpfSMSApp.View.User
{
    /// <summary>
    /// EditAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditUser : Page
    {
        public EditUser()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Page));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // label 숨김
                LblUserIdentityNumber.Visibility = LblUserSurName.Visibility
                    = LblUserName.Visibility = LblUserEmail.Visibility
                    = LblUserPassword.Visibility = LblUserAdmin.Visibility
                    = LblUserActivated.Visibility = Visibility.Hidden;

                // 콤보박스 초기화
                List<string> comboValues = new List<string> { "False", "True" };   // false : 0 / true : 1

                CboUserAdmin.ItemsSource = comboValues;
                CboUserActivated.ItemsSource = comboValues;

                TxtUserID.Text = TxtUserIdentityNumber.Text = "";

                // 그리드 데이터 바인딩
                List<Model.User> users = Logic.DataAccess.GetUsers();
                this.DataContext = users;
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 EditUser Loaded: {ex}");
                throw ex;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); // 이전 화면으로 돌아감
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // 입력된 값이 모두 만족하는지 판별하는 플래그
            bool isValid = true;

            // label 숨김
            LblUserIdentityNumber.Visibility = LblUserSurName.Visibility
                = LblUserName.Visibility = LblUserEmail.Visibility
                = LblUserPassword.Visibility = LblUserAdmin.Visibility
                = LblUserActivated.Visibility = Visibility.Hidden;

            // textbox 비었는지 확인 (유효성 체크 Validation check)
            var user = GrdData.SelectedItem as Model.User;   

            isValid = IsValidInput();

            // 유효성 체크
            if (isValid)
            {
                //MessageBox.Show("DB 수정 처리");
                user.UserIdentityNumber = TxtUserIdentityNumber.Text;
                user.UserSurname = TxtUserSurName.Text;
                user.UserName = TxtUserName.Text;
                user.UserEmail = TxtUserEmail.Text;
                user.UserPassword = TxtUserPassword.Password;
                user.UserAdmin = bool.Parse(CboUserAdmin.SelectedValue.ToString());
                user.UserActivated = bool.Parse(CboUserActivated.SelectedValue.ToString());

                try
                {
                    // UserPassword 암호화
                    var mdHash = MD5.Create();
                    user.UserPassword = Commons.GetMd5Hash(mdHash, user.UserPassword);

                    // DB에 적용
                    var result = Logic.DataAccess.SetUsers(user);

                    if (result == 0)
                    {
                        // 수정안됨
                        LblResult.Text = "사용자 입력에 문제가 발생했습니다. 관리자에게 문의바랍니다.";
                        LblResult.Foreground = Brushes.OrangeRed;
                    }
                    else
                    {
                        // 정상적 수정됨
                        NavigationService.Navigate(new UserList());
                    }
                }
                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외 발생 : {ex}");
                }
            }
        }

        private bool IsValidInput()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(TxtUserIdentityNumber.Text))
            {
                LblUserIdentityNumber.Visibility = Visibility.Visible;
                LblUserIdentityNumber.Text = "사번을 입력하세요.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(TxtUserSurName.Text))
            {
                LblUserSurName.Visibility = Visibility.Visible;
                LblUserSurName.Text = "이름(성)을 입력하세요.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(TxtUserName.Text))
            {
                LblUserName.Visibility = Visibility.Visible;
                LblUserName.Text = "이름을 입력하세요.";
                isValid = false;
            }


            if (string.IsNullOrEmpty(TxtUserEmail.Text))
            {
                LblUserEmail.Visibility = Visibility.Visible;
                LblUserEmail.Text = "이메일을 입력하세요.";
                isValid = false;
            }

            if (!Commons.IsValidEmail(TxtUserEmail.Text))
            {
                LblUserEmail.Visibility = Visibility.Visible;
                LblUserEmail.Text = "이메일 형식이 올바르지 않습니다.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(TxtUserPassword.Password))
            {
                LblUserPassword.Visibility = Visibility.Visible;
                LblUserPassword.Text = "패스워드를 입력하세요.";
                isValid = false;
            }

            if (CboUserAdmin.SelectedIndex < 0)
            {
                LblUserAdmin.Visibility = Visibility.Visible;
                LblUserAdmin.Text = "관리자 여부를 선택하세요.";
                isValid = false;
            }

            if (CboUserActivated.SelectedIndex < 0)
            {
                LblUserActivated.Visibility = Visibility.Visible;
                LblUserActivated.Text = "활성화 여부를 선택하세요.";
                isValid = false;
            }

            return isValid;
        }

        private void GrdData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            try
            {
                // TODO : 선택된 값이 입력창에 나오도록 처리
                var user = GrdData.SelectedItem as Model.User;

                TxtUserID.Text = user.UserID.ToString();
                TxtUserIdentityNumber.Text = user.UserIdentityNumber;
                TxtUserSurName.Text = user.UserSurname;
                TxtUserName.Text = user.UserName;
                TxtUserEmail.Text = user.UserEmail;
                CboUserAdmin.SelectedIndex = user.UserAdmin == false ? 0 : 1;
                CboUserActivated.SelectedIndex = user.UserActivated == false ? 0 : 1;
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 GrdData_SelectedCellsChanged: {ex}");
            }
        }
    }
}
