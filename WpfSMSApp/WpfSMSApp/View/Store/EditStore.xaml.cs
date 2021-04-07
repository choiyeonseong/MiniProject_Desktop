using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;

namespace WpfSMSApp.View.Store
{
    /// <summary>
    /// EditAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditStore : Page
    {
        private int StoreID { get; set; }
        private Model.Store CurrentStore;
        public EditStore()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Page));
        }

        /// <summary>
        /// 추가 생성자. StoreList에서 storeId를 받아옴
        /// </summary>
        /// <param name="storeId"></param>
        public EditStore(int storeId) : this()
        {
            StoreID = storeId;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // label 숨김
            LblStoreName.Visibility = LblStoreLocation.Visibility
                = Visibility.Hidden;

            TxtStoreID.Text = TxtStoreName.Text
                = TxtStoreLocation.Text = "";

            try
            {
                // Store 테이블에서 내용 읽어옴
                CurrentStore = Logic.DataAccess.GetStores().Where(s => s.StoreID.Equals(StoreID)).FirstOrDefault();
                TxtStoreID.Text = CurrentStore.StoreID.ToString();
                TxtStoreName.Text = CurrentStore.StoreName;
                TxtStoreLocation.Text = CurrentStore.StoreLocation;
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 EditStore.xaml.cs Page_Loaded: {ex}");
                Commons.ShowMessageAsync($"예외", "예외발생 : {ex}");
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
            LblStoreName.Visibility = LblStoreLocation.Visibility
                = Visibility.Hidden;

           CurrentStore = new Model.Store();    // 새로운 Store 생성

            // textbox 비었는지 확인 (유효성 체크 Validation check)
            isValid = IsValidInput();

            if (isValid)
            {
                //MessageBox.Show("DB 수정 처리");
                CurrentStore.StoreName = TxtStoreName.Text;
                CurrentStore.StoreLocation = TxtStoreLocation.Text;

                try
                {
                    // DB에 적용
                    var result = Logic.DataAccess.SetStores(CurrentStore);

                    if (result == 0)
                    {
                        Commons.LOGGER.Error($"EditStore.xaml.cs 창고정보 수정오류 발생");
                        Commons.ShowMessageAsync("오류", "수정시 오류가 발생했습니다.");
                        return;
                    }
                    else
                    {
                        // 정상적 수정됨
                        Commons.ShowMessageAsync("수정", "데이터 베이스에 수정되었습니다.");
                        NavigationService.Navigate(new StoreList());    // 화면 refresh
                    }
                }
                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외 발생 : {ex}");
                }
            }
        }

        bool IsValid = true;

        private bool IsValidInput()
        {
            IsValid = true;
            if (string.IsNullOrEmpty(TxtStoreName.Text))
            {
                LblStoreName.Visibility = Visibility.Visible;
                LblStoreName.Text = "창고명을 입력하세요.";
                IsValid = false;
            }
            //else
            //{
            //    var cnt = Logic.DataAccess.GetStores().Where(u => u.StoreName.Equals(TxtStoreName.Text)).Count();
            //    if (cnt > 0)
            //    {
            //        LblStoreName.Visibility = Visibility.Visible;
            //        LblStoreName.Text = "중복된 창고명이 존재합니다.";
            //        IsValid = false;
            //    }
            //}

            if (string.IsNullOrEmpty(TxtStoreLocation.Text))
            {
                LblStoreLocation.Visibility = Visibility.Visible;
                LblStoreLocation.Text = "창고 위치를 입력하세요.";
                IsValid = false;
            }

            return IsValid;
        }

        private void TxtStoreName_LostFocus(object sender, RoutedEventArgs e)
        {
            IsValidInput();
        }

        private void TxtStoreLocation_LostFocus(object sender, RoutedEventArgs e)
        {
            IsValidInput();
        }

        private void TxtStoreLocation_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnEdit_Click(sender, e);
            }
        }
    }
}
