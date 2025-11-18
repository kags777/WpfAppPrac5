using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace WpfAppPrac5
{
    public partial class MainWindow : Window
    {
        private List<DateTime> dates = new List<DateTime>();
        private Random rnd = new Random();
        private string file = "dates.xml";

        public MainWindow()
        {
            InitializeComponent();
            InfoWindow info = new InfoWindow();
            info.ShowDialog();
        }

        private DateTime RandomDate()
        {
            int year = rnd.Next(1400, 2026);
            int month = rnd.Next(1, 13);
            int day = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);
            return new DateTime(year, month, day);
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            dates.Clear();
            for (int i = 0; i < 20; i++)
                dates.Add(RandomDate());

            RefreshList();
            MessageBox.Show("Сгенерировано 20 дат!");
        }

        private void RefreshList()
        {
            DatesView.ItemsSource = null;
            DatesView.ItemsSource = dates
                .Select(d => d.ToString("dd.MM.yyyy"))
                .ToList();
        }

        private void SaveXML_Click(object sender, RoutedEventArgs e)
        {
            XDocument doc = new XDocument(
                new XElement("Dates",
                    dates.Select(d => new XElement("Date", d.ToString("yyyy-MM-dd")))
                )
            );
            doc.Save(file);
            MessageBox.Show("Файл сохранён.");
        }

        private void LoadXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XDocument doc = XDocument.Load(file);
                dates = doc.Root.Elements("Date")
                    .Select(x => DateTime.Parse(x.Value))
                    .ToList();
                RefreshList();
                MessageBox.Show("XML загружен.");
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки XML.");
            }
        }

        private void FilterYear_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(YearBox.Text, out int year)) return;

            var res = dates.Where(d => d.Year == year).ToList();
            if (!res.Any())
                MessageBox.Show($"Нет дат для {year} года.");
            else
                DatesView.ItemsSource = res.Select(d => d.ToString("dd.MM.yyyy"));
        }

        private void FilterMonth_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(MonthBox.Text, out int month)) return;

            var res = dates.Where(d => d.Month == month).ToList();
            if (!res.Any())
                MessageBox.Show($"Нет дат для {month} месяца.");
            else
                DatesView.ItemsSource = res.Select(d => d.ToString("dd.MM.yyyy"));
        }

        private void FilterDay_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(DayBox.Text, out int day)) return;

            var res = dates.Where(d => d.Day == day).ToList();
            if (!res.Any())
                MessageBox.Show($"Нет дат для {day} дня.");
            else
                DatesView.ItemsSource = res.Select(d => d.ToString("dd.MM.yyyy"));
        }

        private void Range_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(RangeStartBox.Text, out int start)) return;
            if (!int.TryParse(RangeEndBox.Text, out int end)) return;

            var count = dates.Count(d => d.Year >= start && d.Year <= end);
            MessageBox.Show($"Количество дат в диапазоне {start}-{end}: {count}");
        }

        private void MaxDate_Click(object sender, RoutedEventArgs e)
        {
            if (dates.Count == 0) return;

            var max = dates.Max();
            MessageBox.Show($"Максимальная дата: {max:dd.MM.yyyy}");
        }

        private void SortAsc_Click(object sender, RoutedEventArgs e)
        {
            dates = dates.OrderBy(d => d).ToList();
            RefreshList();
        }

        private void SortDesc_Click(object sender, RoutedEventArgs e)
        {
            dates = dates.OrderByDescending(d => d).ToList();
            RefreshList();
        }

        private void AddDate_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new InputBox("Введите дату (дд.ММ.гггг):");
            if (dlg.ShowDialog() == true)
            {
                if (DateTime.TryParse(dlg.Value, out DateTime dt))
                {
                    dates.Add(dt);
                    RefreshList();
                }
                else MessageBox.Show("Неверный формат.");
            }
        }

        private void EditDate_Click(object sender, RoutedEventArgs e)
        {
            if (DatesView.SelectedItem == null)
            {
                MessageBox.Show("Выберите дату.");
                return;
            }

            string selected = DatesView.SelectedItem.ToString();
            DateTime old = DateTime.Parse(selected);

            var dlg = new InputBox("Введите новую дату (дд.MM.гггг):", selected);

            if (dlg.ShowDialog() == true)
            {
                if (DateTime.TryParse(dlg.Value, out DateTime newDate))
                {
                    dates[dates.IndexOf(old)] = newDate;
                    RefreshList();
                }
                else MessageBox.Show("Неверный формат.");
            }
        }
    }
}
