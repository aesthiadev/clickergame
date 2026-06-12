using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MyFirstApp;

public partial class MainWindow : Window
{
    private int clicks = 0;
    private int totalClicks = 0;
    private int clickPerClick = 1;
    private int upgrades = 0;
    private int upgradeCost = 20;
    private int prestige = 1;
    private int autoClickers = 0;
    private int autoClickersCost = 100;

    private TimeSpan playTime = TimeSpan.Zero; // Timespan zegar
    private string formattedTime = "00:00:00"; // Format 

    private string path = $@"{AppDomain.CurrentDomain.BaseDirectory}\save.txt";

    private DispatcherTimer autoClickerTimer = new DispatcherTimer();

    public MainWindow()
    {

        InitializeComponent();

        autoClickerTimer.Interval = TimeSpan.FromSeconds(1); // Ustawienie tick
        autoClickerTimer.Tick += AutoClickerTick; // Funkcja jaka ma sie wydarzyć co sekunde

        autoClickerTimer.Start(); // Start Timer
    }

    private void AutoClickerTick(object? sender, EventArgs e)
    {
        clicks += autoClickers * prestige;
        totalClicks += autoClickers * prestige;

        playTime = playTime.Add(TimeSpan.FromSeconds(1)); // Dodawanie sekund co 1s
        formattedTime = playTime.ToString(@"hh\:mm\:ss"); // Formatowanie czasu

        var prestigeProgress = this.FindControl<ProgressBar>("PrestigeProgress");
        var scoreText = this.FindControl<TextBlock>("ScoreText");
        var totalText = this.FindControl<TextBlock>("TotalClicks");
        var playTimeText = this.FindControl<TextBlock>("PlayTime");

        totalText!.Text = $"Total Clicks: {totalClicks}";
        scoreText!.Text = $"{clicks}";
        prestigeProgress!.Value = clicks;

        playTimeText!.Text = $"Play Time: {formattedTime}";
    }

    private void Click_Button(object? sender, RoutedEventArgs e)
    {
        var clickValue = this.FindControl<TextBlock>("ScoreText");
        var infoText = this.FindControl<TextBlock>("InfoText");
        var prestigeProgress = this.FindControl<ProgressBar>("PrestigeProgress");
        var totalText = this.FindControl<TextBlock>("TotalClicks");
        

        clicks = clicks + clickPerClick * prestige;
        totalClicks = totalClicks + clickPerClick * prestige;

        totalText!.Text = $"Total Clicks: {totalClicks}";
        clickValue!.Text = $"{clicks}";
        prestigeProgress!.Value = clicks;
    }
    
    private void Upgrade_Button(object? sender, RoutedEventArgs e)
    {
        var clickValue = this.FindControl<TextBlock>("ScoreText");
        var upgradeText = this.FindControl<TextBlock>("UpgradeText");
        var infoText = this.FindControl<TextBlock>("InfoText");

        if (clicks >= upgradeCost)
        {
            upgrades++;
            clicks = clicks - upgradeCost;
            upgradeCost = upgradeCost * 2;
            clickPerClick++;
            upgradeText!.Text = $"Upgrade Cost: {upgradeCost}";
            infoText!.Text = $"Clicks per click: {clickPerClick}";
            clickValue!.Text = $"{clicks}";
        }
        else { return; }

    }

    private void Reset_Button(object? sender, RoutedEventArgs e)
    {
        var clickValue = this.FindControl<TextBlock>("ScoreText");
        var upgradeText = this.FindControl<TextBlock>("UpgradeText");
        var infoText = this.FindControl<TextBlock>("InfoText");
        var prestigeText = this.FindControl<TextBlock>("PrestigeText");
        var prestigeProgress = this.FindControl<ProgressBar>("PrestigeProgress");
        var clickerCost = this.FindControl<TextBlock>("AutoClickerCostText");
        var text = this.FindControl<TextBlock>("AutoClickerCountText");
        var prestigeStat = this.FindControl<TextBlock>("Prestiges");
        var totalText = this.FindControl<TextBlock>("TotalClicks");
        var playTimeText = this.FindControl<TextBlock>("PlayTime");

        clicks = 0;
        totalClicks = 0;
        clickPerClick = 1;
        upgrades = 0;
        upgradeCost = 20;
        prestige = 1;
        autoClickers = 0;
        autoClickersCost = 100;
        playTime = TimeSpan.Zero;

        prestigeProgress!.Value = clicks;
        prestigeStat!.Text = $"Prestiges: {prestige}";
        prestigeText!.Text = $"Multiplier: x{prestige}";
        upgradeText!.Text = $"Upgrade Cost: {upgradeCost}";
        infoText!.Text = $"Clicks per click: {clickPerClick}";
        clickValue!.Text = $"{clicks}";
        clickerCost!.Text = $"Cost: {autoClickersCost}";
        text!.Text = $"Owned: {autoClickers}";
        totalText!.Text = $"Total Clicks: {totalClicks}";

        formattedTime = playTime.ToString(@"hh\:mm\:ss");

        playTimeText!.Text = $"Play Time: {formattedTime}";
    }
     
    private void AutoClicker_Button(object? sender, RoutedEventArgs e)
    {
        if (clicks >= autoClickersCost)
        {
            clicks = clicks - autoClickersCost;
            autoClickers++;
            autoClickersCost = autoClickersCost * 3;

            var clicksText = this.FindControl<TextBlock>("ScoreText");
            var clickerCost = this.FindControl<TextBlock>("AutoClickerCostText");
            var text = this.FindControl<TextBlock>("AutoClickerCountText");

            clicksText!.Text = $"{clicks}";
            clickerCost!.Text = $"Cost: {autoClickersCost}";
            text!.Text = $"Owned: {autoClickers}";
        }
    }

    private void Prestige_Button(object? sender, RoutedEventArgs e)
    {
        var clickValue = this.FindControl<TextBlock>("ScoreText");
        var upgradeText = this.FindControl<TextBlock>("UpgradeText");
        var infoText = this.FindControl<TextBlock>("InfoText");
        var prestigeText = this.FindControl<TextBlock>("PrestigeText");
        var prestigeProgress = this.FindControl<ProgressBar>("PrestigeProgress");
        var clickerCost = this.FindControl<TextBlock>("AutoClickerCostText");
        var text = this.FindControl<TextBlock>("AutoClickerCountText");
        var prestigeStat = this.FindControl<TextBlock>("Prestiges");

        if (clicks >= prestigeProgress!.Maximum)
        {
            clicks = 0;
            clickPerClick = 1;
            upgrades = 0;
            upgradeCost = 20;
            autoClickers = 0;
            autoClickersCost = 100;
            prestige++;

            prestigeStat!.Text = $"Prestiges: {prestige}";
            prestigeProgress!.Value = clicks;
            prestigeProgress!.Maximum = prestigeProgress.Maximum * 2;
            prestigeText!.Text = $"Multiplier: x{prestige}";
            upgradeText!.Text = $"Upgrade Cost: {upgradeCost}";
            infoText!.Text = $"Click per click: {clickPerClick}";
            clickValue!.Text = $"{clicks}";
            clickerCost!.Text = $"Cost: {autoClickersCost}";
            text!.Text = $"Owned: {autoClickers}";
        }
    }

    private void Save_Button(object? sender, RoutedEventArgs e)
    {
        var prestigeProgress = this.FindControl<ProgressBar>("PrestigeProgress");
        string createText = $"clicks={clicks}\nclickPerClick={clickPerClick}\nupgrades={upgrades}\nupgradeCost={upgradeCost}\nprestige={prestige}\nautoClicker={autoClickers}\nautoClickersCost={autoClickersCost}\nprestigeMaxValue={prestigeProgress!.Maximum}\nplayTime={playTime.TotalSeconds}\ntotalClicks={totalClicks}";
        File.WriteAllText(path, createText);
    }

    private void Load_Button(object? sender, RoutedEventArgs e)
    {
        if (Path.Exists(path)) 
        {
            string[] readText = File.ReadAllLines(path);

            string[] s = readText[0].Split("="); // clicks
            string[] s2 = readText[1].Split("="); // clickPerClick
            string[] s3 = readText[2].Split("="); // upgrades
            string[] s4 = readText[3].Split("="); // upgradeCost
            string[] s5 = readText[4].Split("="); // prestige
            string[] s6 = readText[5].Split("="); // autoClickers
            string[] s7 = readText[6].Split("="); // autoClickersCost
            string[] s8 = readText[7].Split("="); // prestigeProgressMaximum
            string[] s9 = readText[8].Split("="); // playTime
            string[] s10 = readText[9].Split("="); // totalClicks

            clicks = int.Parse(s[1]);
            clickPerClick = int.Parse(s2[1]);
            upgrades = int.Parse(s3[1]);
            upgradeCost = int.Parse(s4[1]);
            prestige = int.Parse(s5[1]);
            autoClickers = int.Parse(s6[1]);
            autoClickersCost = int.Parse(s7[1]);
            playTime = TimeSpan.FromSeconds(int.Parse(s9[1]));
            totalClicks = int.Parse(s10[1]);

            var clickValue = this.FindControl<TextBlock>("ScoreText");
            var infoText = this.FindControl<TextBlock>("InfoText");
            var prestigeText = this.FindControl<TextBlock>("PrestigeText");
            var prestigeProgress = this.FindControl<ProgressBar>("PrestigeProgress");
            var clickerCost = this.FindControl<TextBlock>("AutoClickerCostText");
            var autoClickerCount = this.FindControl<TextBlock>("AutoClickerCountText");
            var upgradeText = this.FindControl<TextBlock>("UpgradeText");
            var playTimeText = this.FindControl<TextBlock>("PlayTime");
            var prestigeStat = this.FindControl<TextBlock>("Prestiges");
            var totalClicksText = this.FindControl<TextBlock>("TotalClicks");

            prestigeStat!.Text = $"Prestiges: {prestige}";
            totalClicksText!.Text = $"Total Clicks: {totalClicks}";
            clickValue!.Text = $"{clicks}";
            infoText!.Text = $"Clicks per click: {clickPerClick}";
            upgradeText!.Text = $"Upgrade Cost: {upgradeCost}";
            prestigeText!.Text = $"Multiplier: x{prestige}";
            prestigeProgress!.Value = clicks;
            prestigeProgress!.Maximum = int.Parse(s8[1]);
            clickerCost!.Text = $"Cost: {autoClickersCost}";
            autoClickerCount!.Text = $"Owned: {autoClickers}";

            formattedTime = playTime.ToString(@"hh\:mm\:ss");
            playTimeText!.Text = $"Play Time: {formattedTime}";
        }
    }
}