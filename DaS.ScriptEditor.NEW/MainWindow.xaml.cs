using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using DbgRow = System.Tuple<System.DateTime, DaS.ScriptLib.Lua.Dbg.DbgPrintType, string>;

namespace DaS.ScriptEditor.NEW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Queue<DbgRow> DbgRowQueue = new Queue<DbgRow>();

        private Thread DbgRowThread;

        private int DbgRowThreadUpdateInterval = 33;
        private int DbgRowQueueAmountPerCycle = 10;

        private object DbgPrintLOCK = new object();

        public MainWindow()
        {
            InitializeComponent();

            MainLuaContainer.AutoComplete.InitDefaultEntries(this);

            seToolbar.SeButtonClicked += SeToolbar_SeButtonClicked;

            TryAttachAndUpdateStr(true);

            ScriptLib.Lua.Dbg.OnPrint += Dbg_OnPrint;

            MainLuaContainer.ScriptStart += MainLuaContainer_ScriptStart;
            MainLuaContainer.ScriptStop += MainLuaContainer_ScriptStop;
        }

        private void MainLuaContainer_ScriptStop(object sender, LuaTabSwitchEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                seToolbar.ButtonStart.IsEnabled = true;
                seToolbar.ButtonStart.Opacity = 1.0;
                seToolbar.ButtonStop.IsEnabled = false;
                seToolbar.ButtonStop.Opacity = 0.5;
            }));
        }

        private void MainLuaContainer_ScriptStart(object sender, LuaTabSwitchEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                seToolbar.ButtonStart.IsEnabled = false;
                seToolbar.ButtonStart.Opacity = 0.5;
                seToolbar.ButtonStop.IsEnabled = true;
                seToolbar.ButtonStop.Opacity = 1.0;
            }));
        }

        private void DbgRowUpdateThread()
        {
            try
            {
                while (true)
                {
                    int counter = 0;

                    while (DbgRowQueue.Count > 0 && counter++ < DbgRowQueueAmountPerCycle)
                    {
                        var nextRow = DbgRowQueue.Dequeue();

                        //TODO: Add a fancier component for the output


                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            if (nextRow.Item2 == ScriptLib.Lua.Dbg.DbgPrintType.ClearAll)
                            {
                                ScriptOutputBox.Clear();
                            }

                            var longTime = nextRow.Item1.ToLongTimeString();
                            var a = longTime.Substring(0, longTime.Length - 3);
                            var b = nextRow.Item1.Millisecond.ToString("D3");
                            var c = longTime.Substring(longTime.Length - 2, 2);

                            ScriptOutputBox.AppendText($"[{a}.{b} {c}][{nextRow.Item2}] > {nextRow.Item3}\n");

                            ScriptOutputBox.ScrollToEnd();
                        }));
                    }

                    Thread.Sleep(DbgRowThreadUpdateInterval);
                }
            }
            catch (ThreadAbortException)
            {

            }
        }

        private void FinishOutputOnStop()
        {
            while (DbgRowQueue.Count > 0)
            {
                var nextRow = DbgRowQueue.Dequeue();

                //TODO: Add a fancier component for the output

                var longTime = nextRow.Item1.ToLongTimeString();
                var a = longTime.Substring(0, longTime.Length - 3);
                var b = nextRow.Item1.Millisecond.ToString("D3");
                var c = longTime.Substring(longTime.Length - 2, 2);

                ScriptOutputBox.AppendText($"[{a}.{b} {c}][{nextRow.Item2}] > {nextRow.Item3}\n\n");

                ScriptOutputBox.ScrollToEnd();
            }
        }

        private void Dbg_OnPrint(DateTime time, ScriptLib.Lua.Dbg.DbgPrintType type, string text)
        {
            if (DbgRowQueue != null)
            {
                DbgRowQueue.Enqueue(new DbgRow(time, type, text));
            }

            //new Thread(new ThreadStart(() =>
            //{
            //    lock (DbgPrintLOCK)
            //    {
            //        if (DbgRowQueue != null)
            //        {
            //            DbgRowQueue.Enqueue(new DbgRow(time, type, text));
            //        }
            //    }
            //})).Start();
        }

        public bool IsStartButtonEnabled
        {
            get
            {
                return !IsCurrentDocumentRunning;
            }
        }

        public bool IsStopButtonEnabled
        {
            get
            {
                return IsCurrentDocumentRunning;
            }
        }

        public bool IsCurrentDocumentRunning
        {
            get
            {
                return MainLuaContainer.SelectedLuaScript.IsRunning;
            }
        }

        private void TryAttachAndUpdateStr(bool suppressMessageBox = false)
        {
            ScriptLib.Injection.Hook.DARKSOULS.TryAttachToDarkSouls(suppressMessageBox);
            seToolbar.UpdateDarkSoulsVersionText(ScriptLib.Injection.Hook.DARKSOULS.VersionDisplayName);
        }

        private void SeButtonAction(SeButton btn)
        {
            switch (btn)
            {
                case SeButton.ExitEntireProgram:
                    Close();
                    break;
                case SeButton.NewDoc:
                    MainLuaContainer.AddNewTab();
                    break;
                case SeButton.OpenFile:
                    if (MainLuaContainer.SelectedLuaScript != null)
                        MainLuaContainer.SelectedLuaScript.SeOpenFile();
                    else
                        MainLuaContainer.SeOpenFile();
                    break;
                case SeButton.Refresh:
                    TryAttachAndUpdateStr();
                    break;
                case SeButton.SaveAllFiles:
                    MainLuaContainer.SaveAll();
                    break;
                case SeButton.SaveAs:
                    MainLuaContainer.SelectedLuaScript.SeSaveAs();
                    break;
                case SeButton.SaveFile:
                    MainLuaContainer.SelectedLuaScript.SeSave();
                    break;
                case SeButton.Start:
                    MainLuaContainer.SelectedLuaScript.StartExecution();
                    DbgRowThread = new Thread(new ThreadStart(DbgRowUpdateThread));
                    DbgRowThread.Start();
                    break;
                case SeButton.Stop:
                    MainLuaContainer.SelectedLuaScript.StopExecution();
                    if (DbgRowThread != null)
                    {
                        DbgRowThread.Abort();
                    }
                    //FinishOutputOnStop();
                    break;
            }
        }

        private void SeToolbar_SeButtonClicked(object sender, SeButtonEventArgs e)
        {
            SeButtonAction(e.ButtonType);
        }

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.NewDoc);
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.OpenFile);
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.SaveFile);
        }

        private void MenuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.SaveAs);
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.ExitEntireProgram);
        }

        private void OutputWindow_Hiding(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (DbgRowThread != null && DbgRowThread.IsAlive)
            {
                DbgRowThread.Abort();
                DbgRowThread = null;
            }
        }

        private void ScriptOutputBox_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            e.Handled = true;
        }
    }
}
