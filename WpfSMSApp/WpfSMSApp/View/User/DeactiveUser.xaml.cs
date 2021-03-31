using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace WpfSMSApp.View.User
{
    /// <summary>
    /// EditAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DeactiveUser : Page
    {
        public DeactiveUser()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Page));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
              
                // 그리드 데이터 바인딩
                List<Model.User> users = Logic.DataAccess.GetUsers();
                this.DataContext = users;
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 DeactiveUser Loaded: {ex}");
                throw ex;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); // 이전 화면으로 돌아감
        }

        private async void BtnDeactive_Click(object sender, RoutedEventArgs e)
        {
            // 입력된 값이 모두 만족하는지 판별하는 플래그
            bool isValid = true;

            if (GrdData.SelectedItem==null)
            {
                await Commons.ShowMessageAsync("오류", "비활성화할 사용자를 선택하세요.");
                //MessageBox.Show("비활성화 안됨");
                return;
            }

            // 유효성 체크
            if (isValid)
            {
                try
                {
                    var user = GrdData.SelectedItem as Model.User;
                    user.UserActivated = false;
                  
                    // DB에 적용
                    var result = Logic.DataAccess.SetUsers(user);

                    if (result == 0)
                    {
                        // 수정안됨
                await Commons.ShowMessageAsync("오류", "사용자 정보 수정에 실패했습니다.");
                        return;
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

        private void GrdData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            try
            {
                // TODO : 선택된 값이 입력창에 나오도록 처리
                var user = GrdData.SelectedItem as Model.User;

             
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 GrdData_SelectedCellsChanged: {ex}");
            }
        }

      
    }
}
