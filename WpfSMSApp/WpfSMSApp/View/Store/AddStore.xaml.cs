using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;

namespace WpfSMSApp.View.Store
{
    /// <summary>
    /// EditAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddStore : Page
    {
        public AddStore()
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
                  = Visibility.Hidden;

              
                TxtUserID.Text = TxtUserIdentityNumber.Text = "";
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 AddStore Loaded: {ex}");
                throw ex;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); // 이전 화면으로 돌아감
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // 입력된 값이 모두 만족하는지 판별하는 플래그
            bool isValid = true;

            // label 숨김
            LblUserIdentityNumber.Visibility = LblUserSurName.Visibility
                = Visibility.Hidden;

            // textbox 비었는지 확인 (유효성 체크 Validation check)
            var user = new Model.User();    // 새로운 User 생성

            isValid = IsValidInput();

            // 유효성 체크
            if (isValid)
            {
                //MessageBox.Show("DB 수정 처리");
                user.UserIdentityNumber = TxtUserIdentityNumber.Text;
                user.UserSurname = TxtUserSurName.Text;
               

                try
                {
                    // UserPassword 암호화
                    var mdHash = MD5.Create();
                    user.UserPassword = Commons.GetMd5Hash(mdHash, user.UserPassword);

                    // DB에 적용
                    var result = Logic.DataAccess.SetUsers(user);

                    if (result == 0)
                    {
                      
                    }
                    else
                    {
                        // 정상적 수정됨
                        NavigationService.Navigate(new StoreList());
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
            else
            {
                var cnt = Logic.DataAccess.GetUsers().Where(u => u.UserIdentityNumber.Equals(TxtUserIdentityNumber.Text)).Count();
                if (cnt > 0)
                {
                    LblUserIdentityNumber.Visibility = Visibility.Visible;
                    LblUserIdentityNumber.Text = "중복된 사번이 존재합니다.";
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(TxtUserSurName.Text))
            {
                LblUserSurName.Visibility = Visibility.Visible;
                LblUserSurName.Text = "이름(성)을 입력하세요.";
                isValid = false;
            }

           
            return isValid;
        }
    }
}
