using LolPcl.Class;
using LolPcl.Class.enums;
using LolPcl.Class.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lol
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RootPlayerInfo PlayerInfo;
        private RootGameInfo MathInfo;
        private string AccImgUrl;

        private string RegionKey;
        private Dictionary<string, string> Reg;

        public MainWindow()
        {
            InitializeComponent();

            PlayerInfo = null;
            MathInfo = null;
            AccImgUrl = null;
            RegionKey = null;

            Reg = new Dictionary<string, string>();
            Reg.Add("RU", Regions.RU);
            Reg.Add("KR", Regions.KR);
            Reg.Add("BR1", Regions.BR1);
            Reg.Add("OC1", Regions.OC1);
            Reg.Add("JP1", Regions.JP1);
            Reg.Add("NA1", Regions.NA1);
            Reg.Add("EUN1", Regions.EUN1);
            Reg.Add("EUW1", Regions.EUW1);
            Reg.Add("TR1", Regions.TR1);
            Reg.Add("LA1", Regions.LA1);
            Reg.Add("LA2", Regions.LA2);

            foreach (var item in Reg)
            {
                Region.Items.Add(item.Key);
            }
        }

        private async void FindBtn(object sender, RoutedEventArgs e)
        {
            if (NickNameTxt.Text == "" || RegionKey == null)
            {
                MessageBox.Show("Enter Nick Name and check region");
            }
            else
            {
                VisibilityInfoElements(true);
                try
                {
                    PlayerInfo = await new Player("Enter Here api key").GetPlayerInfoAsync(NickNameTxt.Text, RegionKey);
                    MathInfo = await new Player("Enter Here api key").GetMarhInfoAsync(PlayerInfo.AccountId, RegionKey);
                    AccImgUrl = new Player("Enter Here api key").GetProfileImg(NickNameTxt.Text);
                    ConfigUi();
                }
                catch (Exception ex)
                {
                    if (ex.Message == "The remote server returned an error: (404) Not Found.")
                    {
                        MessageBox.Show("Network error Or Player Not Found!!!");
                        BackBtn(null, null);
                    }
                    if (ex.Message == "The remote server returned an error: (403) Forbidden.")
                    {
                        MessageBox.Show("Old Api Key");
                        BackBtn(null, null);
                    }
                }
            }
        }



        private void AccIDCopyBtn(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(AccIDLab.Content.ToString());
            MessageBox.Show("Copyed !");
        }

        private void PlayerIDCopyBtn(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(PlayerIDLab.Content.ToString());
            MessageBox.Show("Copyed !");
        }

        private void SummonerIDCopyBtn(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(SummonerIDLab.Content.ToString());
            MessageBox.Show("Copyed !");
        }

        private void PuuIDCopyBtn(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(PuuIDLab.Content.ToString());
            MessageBox.Show("Copyed !");
        }


        private void BackBtn(object sender, RoutedEventArgs e)
        {
            VisibilityInfoElements(false);
        }

        private void SelRegion(object sender, RoutedEventArgs e)
        {
            RegionKey = ((ComboBox)sender).SelectedItem.ToString();
            RegionKey = Reg[RegionKey].ToString();
        }

        #region Configs

        private void VisibilityInfoElements(bool vis)
        {
            EllipseForImg.Visibility = vis ? Visibility.Visible : Visibility.Hidden;
            NickGrd.Visibility = vis ? Visibility.Visible : Visibility.Hidden;
            LevelGrd.Visibility = vis ? Visibility.Visible : Visibility.Hidden;
            matchGrd.Visibility = vis ? Visibility.Visible : Visibility.Hidden;
            AccGrd.Visibility = vis ? Visibility.Visible : Visibility.Hidden;
            PlayerGrd.Visibility = vis ? Visibility.Visible : Visibility.Hidden;
            summonierGrd.Visibility = vis ? Visibility.Visible : Visibility.Hidden;
            BackBtnName.Visibility = vis ? Visibility.Visible : Visibility.Hidden;
            PuuIDGrid.Visibility = vis ? Visibility.Visible : Visibility.Hidden;

            SrcPlayer.Visibility = vis ? Visibility.Hidden : Visibility.Visible;
        }

        private void ConfigUi()
        {
            ProfileImg.ImageSource = new BitmapImage(new Uri(AccImgUrl));
            NickLab.Content = PlayerInfo.Name;
            LevelLab.Content = PlayerInfo.SummonerLevel;
            Match.Content = MathInfo.TotalGames;
            AccIDLab.Content = PlayerInfo.AccountId;
            PlayerIDLab.Content = PlayerInfo.ProfileIconId;
            SummonerIDLab.Content =  PlayerInfo.Id;
            PuuIDLab.Content = PlayerInfo.Puuid;
        }

        #endregion
    }
}
