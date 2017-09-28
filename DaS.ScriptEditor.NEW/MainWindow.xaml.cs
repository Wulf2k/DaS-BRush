using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DbgRow = System.Tuple<System.DateTime, DaS.ScriptLib.LuaScripting.Dbg.DbgPrintType, string>;
using MK = System.Windows.Input.ModifierKeys;

namespace DaS.ScriptEditor.NEW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Queue<DbgRow> DbgRowQueue = new Queue<DbgRow>();
        private Thread DbgRowThread;
        private int DbgRowThreadUpdateInterval = 20;
        private object DbgPrintLOCK = new object();

        private Dictionary<ICommand, SeButton> SeCommands = new Dictionary<ICommand, SeButton>();

        public MenuItemIndexer SeMenu;

        public class MenuItemIndexer
        {
            private readonly MainWindow main;
            public MenuItemIndexer(MainWindow m)
            {
                main = m;
            }

            public MenuItem this[SeButton b]
            {
                get
                {
                    switch (b)
                    {
                        case SeButton.NewDoc: return main.MenuNew;
                        case SeButton.OpenFile: return main.MenuOpen;
                        case SeButton.SaveAllFiles: return main.MenuSaveAll;
                        case SeButton.SaveFile: return main.MenuSave;
                        case SeButton.Start: return main.MenuStartScript;
                        case SeButton.Stop: return main.MenuStopScript;
                        case SeButton.Refresh: return main.MenuRefreshHook;
                        default: return null;
                    }
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            ScriptLib.LuaScripting.DSLua.Init();

            SeMenu = new MenuItemIndexer(this);

            MainLuaContainer.AutoComplete.InitDefaultEntries(this);

            seToolbar.SeButtonClicked += SeToolbar_SeButtonClicked;
            seToolbar.SeButtonEnabledChanged += SeToolbar_SeButtonEnabledChanged;

            TryAttachAndUpdateStr(true);

            ScriptLib.LuaScripting.Dbg.OnPrint += Dbg_OnPrint;

            MainLuaContainer.ScriptStart += MainLuaContainer_ScriptStart;
            MainLuaContainer.ScriptStop += MainLuaContainer_ScriptStop;

            InitCommandBindings();
        }

        private void SeToolbar_SeButtonEnabledChanged(object sender, SeButtonEventArgs e)
        {
            SeMenu[e.ButtonType]?.SeSetEnabled(seToolbar[e.ButtonType]?.IsHitTestVisible ?? false);
        }

        private void RegisterNewCommand(SeButton btn, MK modifierKey, Key key, MenuItem menuItem = null)
        {
            var cmd = new RoutedCommand();
            var gesture = new KeyGesture(key, modifierKey);
            cmd.InputGestures.Add(gesture);
            MainLuaContainer.LuaEditor.CommandBindings.Add(new CommandBinding(cmd, OnSeCommand, SeCommandCanExecute));
            SeCommands.Add(cmd, btn);

            if (menuItem != null)
            {
                menuItem.InputGestureText = gesture.GetDisplayStringForCulture(System.Globalization.CultureInfo.CurrentCulture);
            }
        }

        private void InitCommandBindings()
        {
            RegisterNewCommand(SeButton.SaveFile, MK.Control, Key.S, MenuSave);
            RegisterNewCommand(SeButton.SaveAllFiles, MK.Control | MK.Shift, Key.S, MenuSaveAll);
            RegisterNewCommand(SeButton.SaveAs, MK.Control | MK.Alt, Key.S, MenuSaveAs);

            RegisterNewCommand(SeButton.NewDoc, MK.Control, Key.N, MenuNew);
            RegisterNewCommand(SeButton.OpenFile, MK.Control, Key.O, MenuOpen);

            RegisterNewCommand(SeButton.Refresh, MK.None, Key.F7, MenuRefreshHook);

            //Since the script start and stop buttons are never both enabled at the same time, 
            //they can be bound to the same key.
            RegisterNewCommand(SeButton.Start, MK.None, Key.F5);
            RegisterNewCommand(SeButton.Stop, MK.None, Key.F5);
        }

        private void OnSeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SeButtonAction(SeCommands[e.Command]);
        }

        private void SeCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = seToolbar.GetButtonEnabled(SeCommands[e.Command]);
        }

        private void MainLuaContainer_ScriptStop(object sender, LuaTabEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                seToolbar.ButtonStart.SeSetEnabled(true);
                seToolbar.ButtonStop.SeSetEnabled(false);
            }));
        }

        private void MainLuaContainer_ScriptStart(object sender, LuaTabEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                seToolbar.ButtonStart.SeSetEnabled(false);
                seToolbar.ButtonStop.SeSetEnabled(true);
            }));
        }

        private void DbgRowUpdateThread()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            if (DbgRowQueue.Count > 0)
                            {
                                var nextRow = DbgRowQueue.Dequeue();

                                if (nextRow.Item2 == ScriptLib.LuaScripting.Dbg.DbgPrintType.ClearAll)
                                {
                                    ScriptOutputBox.Clear();
                                }

                                var longTime = nextRow.Item1.ToLongTimeString();
                                var a = longTime.Substring(0, longTime.Length - 3);
                                var b = nextRow.Item1.Millisecond.ToString("D3");
                                var c = longTime.Substring(longTime.Length - 2, 2);

                                ScriptOutputBox.AppendText($"[{a}.{b} {c}][{nextRow.Item2}] > {nextRow.Item3}\n");

                                ScriptOutputBox.ScrollToEnd(); 
                            }

                            OutputPanel.Title = $"Output ({DbgRowQueue.Count} Print() calls queued)";
                        }));
                    }
                    catch
                    {

                    }

                    Thread.Sleep(DbgRowThreadUpdateInterval);
                }
            }
            catch (ThreadAbortException)
            {

            }
        }

        //private void FinishOutputOnStop()
        //{
        //    while (DbgRowQueue.Count > 0)
        //    {
        //        var nextRow = DbgRowQueue.Dequeue();

        //        //TODO: Add a fancier component for the output

        //        var longTime = nextRow.Item1.ToLongTimeString();
        //        var a = longTime.Substring(0, longTime.Length - 3);
        //        var b = nextRow.Item1.Millisecond.ToString("D3");
        //        var c = longTime.Substring(longTime.Length - 2, 2);

        //        ScriptOutputBox.AppendText($"[{a}.{b} {c}][{nextRow.Item2}] > {nextRow.Item3}\n\n");

        //        ScriptOutputBox.ScrollToEnd();
        //    }
        //}

        private void Dbg_OnPrint(DateTime time, ScriptLib.LuaScripting.Dbg.DbgPrintType type, string text)
        {
            lock (DbgPrintLOCK)
            {
                if (DbgRowQueue != null)
                {
                    if (type == ScriptLib.LuaScripting.Dbg.DbgPrintType.ClearAll)
                    {
                        DbgRowQueue.Clear();
                    }

                    DbgRowQueue.Enqueue(new DbgRow(time, type, text));
                }
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

        private void SeButtonAction(SeButton seButton)
        {
            seToolbar.SetButtonEnabled(seButton, false);
            ForceCursor = true;
            Cursor = Cursors.Wait;
            new Thread(new ParameterizedThreadStart((b) =>
            {
                Action<bool> loading = (isLoad) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (isLoad)
                        {
                            ForceCursor = true;
                            Cursor = Cursors.Wait;
                        }
                        else
                        {
                            Cursor = Cursors.Arrow;
                            ForceCursor = false;
                        }
                    });
                };

                var btn = b as SeButton? ?? SeButton.None;

                switch (b as SeButton? ?? SeButton.None)
                {
                    case SeButton.ExitEntireProgram:
                        Close();
                        break;
                    case SeButton.NewDoc:
                        Application.Current.Dispatcher.Invoke(() => MainLuaContainer.AddNewTab(loading));
                        break;
                    case SeButton.OpenFile:
                        if (MainLuaContainer.SelectedLuaScript != null)
                            MainLuaContainer.SelectedLuaScript.SeOpenFile(loading);
                        else
                            MainLuaContainer.SeOpenFile(loading);
                        break;
                    case SeButton.Refresh:
                        Application.Current.Dispatcher.Invoke(() => TryAttachAndUpdateStr());
                        break;
                    case SeButton.SaveAllFiles:
                        MainLuaContainer.SaveAll(loading);
                        break;
                    case SeButton.SaveAs:
                        MainLuaContainer.SelectedLuaScript.SeSaveAs(loading);
                        break;
                    case SeButton.SaveFile:
                        MainLuaContainer.SelectedLuaScript.SeSave(loading);
                        break;
                    case SeButton.Start:
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MainLuaContainer.SelectedLuaScript.StartExecution(loading);
                            if (DbgRowThread != null)
                            {
                                DbgRowThread.Abort();
                            }
                            DbgRowThread = new Thread(new ThreadStart(DbgRowUpdateThread));
                            DbgRowThread.Start();
                        });
                        break;
                    case SeButton.Stop:
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MainLuaContainer.SelectedLuaScript.StopExecution();
                            //FinishOutputOnStop();
                        });
                        break;
                }

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (!(btn == SeButton.Start || btn == SeButton.Stop))
                    {
                        seToolbar.SetButtonEnabled(btn, true);
                    }
                    loading(false);
                }));
            })).Start(seButton);
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

        private void MenuSaveAll_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.SaveAllFiles);
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

        private void LayoutAnchorable_IsActiveChanged(object sender, EventArgs e)
        {
            (sender as Xceed.Wpf.AvalonDock.Layout.LayoutAnchorable).IsActive = false;
        }

        private void LayoutAnchorable_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void MenuRefreshHook_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.Refresh);
        }

        private void MenuStartScript_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.Start);
        }

        private void MenuStopScript_Click(object sender, RoutedEventArgs e)
        {
            SeButtonAction(SeButton.Stop);
        }

        
    }
}
