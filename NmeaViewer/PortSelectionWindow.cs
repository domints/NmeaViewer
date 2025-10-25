using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Ports;
using Terminal.Gui.Views;
using System.Collections.ObjectModel;
using Serilog;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Drawing;
using Terminal.Gui.Input;
using Terminal.Gui.App;

namespace NmeaViewer
{
    public class PortSelectionWindow : Window
    {
        private readonly ObservableCollection<string> _ports;
        private readonly ListView _portsList;
        private readonly ListView _baudList;
        private readonly Button _scanBtn;
        private readonly Button _connectBtn;

        private readonly NmeaReceiver _nmea = NmeaReceiver.Instance;

        public PortSelectionWindow()
        {
            Title = "Select COM Port and baud";
            _ports = new();
            RefreshPorts();

            _portsList = new ListView();
            _baudList = new ListView();
            _scanBtn = new Button();
            _connectBtn = new Button();

            SetupUI();
        }

        private void SetupUI()
        {
            _portsList.Title = "_Port";
            _portsList.BorderStyle = LineStyle.Rounded;
            _portsList.SetSource(_ports);
            _portsList.Width = Dim.Percent(50);
            _portsList.Height = Height - 2;
            _portsList.Accepting += Connect;

            _baudList.Title = "_Baud rate";
            _baudList.BorderStyle = LineStyle.Rounded;
            _baudList.Source = new ListWrapper<int>(new ObservableCollection<int> { 4800, 9600, 38400, 115200 });
            _baudList.X = Pos.Right(_portsList);
            _baudList.Width = Dim.Fill();
            _baudList.Height = Height - 2;
            _baudList.SelectedItem = 1;
            _baudList.Accepting += Connect;

            _scanBtn.Text = "_Scan...";
            _scanBtn.Y = Pos.AnchorEnd();

            _connectBtn.Text = "_Connect";
            _connectBtn.X = Pos.AnchorEnd();
            _connectBtn.Y = Pos.AnchorEnd();
            _connectBtn.Accepting += Connect;

            Add(_portsList, _baudList, _scanBtn, _connectBtn);
        }

        private void Connect(object? sender, CommandEventArgs e)
        {
            e.Handled = true;
            if (_portsList.SelectedItem < 0)
            {
                return;
            }
            var port = (string)_portsList.Source.ToList()[_portsList.SelectedItem]!;
            var baud = (int)_baudList.Source.ToList()[_baudList.SelectedItem]!;
            Log.Debug("Connecting {port}, {baud}...", port, baud);
            
            var openResult = false;
            var cts = new CancellationTokenSource();
            var ct = cts.Token;
            
            var (showAction, finishAction) = ShowConnectingDialog();
            Task.Run(() => { openResult = _nmea.OpenPort(port, baud); Application.Invoke(() => finishAction(openResult)); });
            showAction();

            var dataDisplay = new DataDisplayWindow();
            if (openResult)
                Application.Run(dataDisplay);

            Log.Debug("Did it went well? {success}", openResult);
        }

        private void RefreshPorts()
        {
            _ports.Clear();
            var ports = SerialPort.GetPortNames();
            Log.Information("Found ports: {ports}", (object)ports);
            foreach (var port in ports)
                _ports.Add(port);
        }

        private (Action, Action<bool>) ShowConnectingDialog()
        {
            var d = new Dialog();
            var spinner = new SpinnerView
            {
                Style = new SpinnerStyle.BouncingBar(),
                X = Pos.Center(),
                Y = Pos.Center()
            };
            var spinnerLabel = new Label
            {
                Text = "Connecting",
                X = Pos.Center(),
                Y = Pos.Bottom(spinner)
            };
            d.Add(spinner);
            d.Add(spinnerLabel);
            d.KeyDown += (_, s) =>
            {
                Log.Debug("Key captured, {}", s.KeyCode);
                s.Handled = true;

                if (s.KeyCode == Terminal.Gui.Drivers.KeyCode.Space)
                    d.RequestStop();
            };
            var dialogCloseBtn = new Button()
            {
                Text = "Close",
            };
            dialogCloseBtn.Accepting += (_, _) => d.RequestStop();

            var finishAction = (bool isOk) =>
            {
                Log.Information("Finishing action");
                d.Text = isOk ? "Connected!" : "Failed to connect.";
                d.AddButton(dialogCloseBtn);
                var label = new Label
                {
                    Text = d.Text,
                    X = Pos.Center(),
                    Y = Pos.Center()
                };
                d.Remove(spinnerLabel);
                d.Remove(spinner);
                d.Add(label);
            };

            return (() => Application.Run(d), finishAction);
        }
    }
}