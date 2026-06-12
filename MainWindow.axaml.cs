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
    private int clickPerClick = 1;
    private int upgrades = 0;
    private int upgradeCost = 20;
    private int prestige = 1;
    private int autoClickers = 0;
    private int autoClickersCost = 100;

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

        var prestigeProgress = this.FindControl<ProgressBar>("PrestigeProgress");
        var scoreText = this.FindControl<TextBlock>("ScoreText");
        scoreText!.Text = $"{clicks}";
        prestigeProgress!.Value = clicks;
    }

    private void Click_Button(object? sender, RoutedEventArgs e)
    {
        var clickValue = this.FindControl<TextBlock>("ScoreText");
        var infoText = this.FindControl<TextBlock>("InfoText");
        var prestigeProgress = this.FindControl<ProgressBar>("PrestigeProgress");

        clicks = clicks + clickPerClick * prestige;

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

        clicks = 0;
        clickPerClick = 1;
        upgrades = 0;
        upgradeCost = 20;
        prestige = 1;
        autoClickers = 0;
        autoClickersCost = 100;

        prestigeProgress!.Value = clicks;
        prestigeText!.Text = $"Multiplier: x{prestige}";
        upgradeText!.Text = $"Upgrade Cost: {upgradeCost}";
        infoText!.Text = $"Clicks per click: {clickPerClick}";
        clickValue!.Text = $"{clicks}";
        clickerCost!.Text = $"Cost: {autoClickersCost}";
        text!.Text = $"Owned: {autoClickers}";
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

        if (clicks >= prestigeProgress!.Maximum)
        {
            clicks = 0;
            clickPerClick = 1;
            upgrades = 0;
            upgradeCost = 20;
            autoClickers = 0;
            autoClickersCost = 100;
            prestige++;

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
        string createText = $"clicks={clicks}\nclickPerClick={clickPerClick}\nupgrades={upgrades}\nupgradeCost={upgradeCost}\nprestige={prestige}\nautoClicker={autoClickers}\nautoClickersCost={autoClickersCost}\nprestigeMaxValue={prestigeProgress!.Maximum}";
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

            clicks = int.Parse(s[1]);
            clickPerClick = int.Parse(s2[1]);
            upgrades = int.Parse(s3[1]);
            upgradeCost = int.Parse(s4[1]);
            prestige = int.Parse(s5[1]);
            autoClickers = int.Parse(s6[1]);
            autoClickersCost = int.Parse(s7[1]);

            var clickValue = this.FindControl<TextBlock>("ScoreText");
            var infoText = this.FindControl<TextBlock>("InfoText");
            var prestigeText = this.FindControl<TextBlock>("PrestigeText");
            var prestigeProgress = this.FindControl<ProgressBar>("PrestigeProgress");
            var clickerCost = this.FindControl<TextBlock>("AutoClickerCostText");
            var autoClickerCount = this.FindControl<TextBlock>("AutoClickerCountText");
            var upgradeText = this.FindControl<TextBlock>("UpgradeText");

            clickValue!.Text = $"{clicks}";
            infoText!.Text = $"Clicks per click: {clickPerClick}";
            upgradeText!.Text = $"Upgrade Cost: {upgradeCost}";
            prestigeText!.Text = $"Multiplier: x{prestige}";
            prestigeProgress!.Value = clicks;
            prestigeProgress!.Maximum = int.Parse(s8[1]);
            clickerCost!.Text = $"Cost: {autoClickersCost}";
            autoClickerCount!.Text = $"Owned: {autoClickers}";
        }
    }
}