using Monitor.EntityModels;
using Monitor.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Monitor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int minFont = Properties.Settings.Default.GridFontMin;
        private int maxFont = Properties.Settings.Default.GridFontMax;
        private int fontSize = Properties.Settings.Default.GridFont;

        public bool IsFullScreen = false;

        private Thread _updatingThread;

        private ObservableCollection<MonitorCrew> crews = new ObservableCollection<MonitorCrew>();

        private delegate void CrewsParameterDelegate(List<MonitorCrew> crews);
        private delegate void KomParameterDelegate(List<kom> data);

        private CrewsComparer comparer = new CrewsComparer();

        public MainWindow()
        {
            InitializeComponent();

            //Init();

            this.Width = 1360;
            this.Height = 768;

            //this.Width = System.Windows.SystemParameters.VirtualScreenWidth - System.Windows.SystemParameters.FullPrimaryScreenWidth;
            //this.Height = this.Width * 9 / 16;

            //this.Height = System.Windows.SystemParameters.FullPrimaryScreenHeight;

            //CrewDataGrid.ItemsSource = crews;
            //Binding myBinding = new Binding();
            //myBinding.Source = crews;
            //CrewDataGrid.SetBinding(DataGrid.ItemsSourceProperty, myBinding);
            //Monitor.Properties.

            Init();
            CrewDataGrid.ItemsSource = crews;

            _updatingThread = new Thread(new ThreadStart(Update)) { IsBackground = true };
            _updatingThread.Start();
        }

        private void Update()
        {
            while (true)
            {
                Thread.Sleep(5000);
                Init();

            }
        }

        private void Init()
        {
            CurrentSituationTextBlock.Dispatcher.BeginInvoke(new Action(UpdateCurrentSituationTextBlock));

            using (EntityContext ctx = new EntityContext())
            {
                var data = ctx.kom
                    .Where(k => k.FL_DISPLAY == 1 && k.FL_UB != 1)
                    .OrderBy(k => k.D_OTPR)
                    .ThenBy(k => k.N_KOM)
                    .ToList();

                //crews.Clear();

                CrewDataGrid.Dispatcher.BeginInvoke(new KomParameterDelegate(UpdateGrid), data);
                
                //CrewDataGrid.ItemsSource = data;
                //CrewDataGrid.ItemsSource = crews;

                //Binding myBinding = new Binding();
                //myBinding.Source = crews;

                //CrewDataGrid.SetBinding(DataGrid.ItemsSourceProperty, myBinding);
                //CrewDataGrid.Dispatcher.BeginInvoke(new CrewsParameterDelegate(BindCrews), crews);
                //BindingExpression seasonsBindingExpression = CrewDataGrid.GetBindingExpression(ListView.ItemsSourceProperty);

                //CrewDataGrid.Dispatcher.BeginInvoke(new Action(UpdateGrid));
            }
        }

        //private void BindCrews(List<MonitorCrew> cs)
        //{
        //    Binding myBinding = new Binding();
        //    myBinding.Source = crews;

        //    CrewDataGrid.SetBinding(DataGrid.ItemsSourceProperty, myBinding);
        //}

        private void UpdateCurrentSituationTextBlock()
        {
            CurrentSituationTextBlock.Text = DateTime.Now.ToString("dd MMMM yyyy г, dddd, HH:mm");
        }

        private void UpdateGrid(List<kom> data)
        {
            for (int i = 0; i < crews.Count; i++)
            {
                if (!data.Any(d => d.N_KOM == crews[i].CrewNumber))
                {
                    crews.RemoveAt(i);
                    i--;
                }
                else
                {
                    var kom = data.First(d => d.N_KOM == crews[i].CrewNumber);
                    crews[i].UpdateWithKom(kom);
                    data.Remove(kom);
                }
            }
            if (data.Count > 0)
            {
                foreach (kom kom in data)
                {
                    //crews.Add(new MonitorCrew(kom));
                    crews.AddSorted<MonitorCrew>(new MonitorCrew(kom), comparer);
                }
                
                //crews
                //    .OrderBy(c => c.DepartureDate)
                //    .ThenBy(c => c.CrewNumber);

            }
        }

        /*private void UpdateGrid()
        {
            CrewDataGrid.ItemsSource = null;

            //Binding myBinding = new Binding();
            //myBinding.Source = crews;

            //CrewDataGrid.SetBinding(ListView.ItemsSourceProperty, myBinding);

            Binding myBinding = new Binding() { Source = crews };
            CrewDataGrid.SetBinding(DataGrid.ItemsSourceProperty, myBinding);
            
            BindingExpression seasonsBindingExpression = CrewDataGrid.GetBindingExpression(ListView.ItemsSourceProperty);
            seasonsBindingExpression.UpdateTarget();
        }*/

        private void CrewDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);

            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }

        }

        public static string GetPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;

            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;

                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                var pi = descriptor as PropertyInfo;

                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        var displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }

            return null;
        }

        public void EnterFullScreenMode()
        {
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.WindowState = System.Windows.WindowState.Maximized;

            IsFullScreen = true;
        }

        public void ExitFullScreenMode()
        {
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            this.WindowState = System.Windows.WindowState.Normal;

            IsFullScreen = false;
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsFullScreen)
                ExitFullScreenMode();
            else
                EnterFullScreenMode();
        }

        private void CrewDataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) &&
                e.Delta != 0)
            {
                double f = CrewDataGrid.FontSize + (double)e.Delta / 240;
                if (f > maxFont)
                    f = maxFont;
                if (f < minFont)
                    f = minFont;
                CrewDataGrid.FontSize = f;

                e.Handled = true;
            }
        }

    }
}
