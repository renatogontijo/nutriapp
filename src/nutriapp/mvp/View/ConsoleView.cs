using Presenter;
using Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace View
{
    public enum EnScreenType
    {
        eString,
        eNumberInt, // Integer
        eNumberDbl, // Double
        eDate
    }

    public class ConsoleView
    {
        private IDictionary<ConsoleKey, ScreenAction> m_keyAction = new Dictionary<ConsoleKey, ScreenAction>();
        private IDictionary<string, ScreenItem> m_screenItem = new Dictionary<string, ScreenItem>();
        private KeyValuePair<string, ScreenItem>? m_currentItem = null;
        private IEnumerator<KeyValuePair<string, ScreenItem>> m_cursor = null;
        public bool EnableSave { get; set; }
        public bool EnableSearch { get; set; }
        public bool EnableSelectItem { get; set; }

        private static ConsoleView m_instance = null;

        private IPresenter m_presenter;

        public static ConsoleView Instance()
        {
            if (m_instance == null)
                m_instance = new ConsoleView();
            return m_instance;
        }

        private ConsoleView()
        {
        }

        public void StartScreen(IPresenter presenter)
        {
            m_presenter = presenter;
            m_currentItem = null;
            m_cursor = null;
            m_screenItem.Clear();
            EnableSave = false;
            ClearScreen();
        }

        public void StartAction()
        {
            m_keyAction.Clear();
        }

        public void CreateAction(ConsoleKey key, IPresenter presenter)
        {
            if (!m_keyAction.ContainsKey(key))
                m_keyAction.Add(key, null);
            m_keyAction[key] = new ScreenAction() { Presenter = presenter };
        }

        public void CreateAction(ConsoleKey key, IPresenter presenter, string description)
        {
            if (!m_keyAction.ContainsKey(key))
                m_keyAction.Add(key, null);
            m_keyAction[key] = new ScreenAction() { Presenter = presenter, Description = description };
        }

        private static void ClearScreen()
        {
            Console.Clear();
        }

        private static void SetWindowsSize()
        {
            Console.SetWindowSize(80, 40);
        }

        private void WriteTitle(string title)
        {
            int col = (80 - title.Length) / 2;
            Console.SetCursorPosition(col, 0);
            Console.Write(title);
        }

        private void CreateMenuScreen()
        {
            bool f2key = false;
            bool f3key = false;
            bool f5key = false;

            string menu = "Menu:  ";
            foreach (KeyValuePair<ConsoleKey, ScreenAction> item in m_keyAction)
            {
                if (item.Key > ConsoleKey.F2 && !f2key && EnableSave)
                {
                    f2key = true;
                    menu += "F2:Salvar e Sair  |  ";
                }
                else if (item.Key > ConsoleKey.F3 && !f3key && EnableSearch)
                {
                    f3key = true;
                    menu += "F3:Pesquisar  |  ";
                }
                else if (item.Key > ConsoleKey.F5 && !f5key && EnableSelectItem)
                {
                    f5key = true;
                    menu += "F5:Digite a posição e pressione F5 para selecionar  |  ";
                }

                ScreenAction act = item.Value;
                string value = act.Description;
                if (string.IsNullOrEmpty(value) && act.Presenter != null)
                    value = act.Presenter.Name;

                menu += string.Format("{0}:{1}  |  ", Convert.ToString(item.Key), value);
            }

            menu += "F12:Sair  |";

            Console.SetCursorPosition(0, 25);
            Console.Write(menu);
        }

        private void ActivateKeyboard()
        {
            bool end = false;

            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                ScreenAction action = null;
                if (m_keyAction.TryGetValue(key.Key, out action) && action.Presenter != null)
                    action.Presenter.Show(m_instance);
                else
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.F1:
                        case ConsoleKey.F4:
                        case ConsoleKey.F6:
                        case ConsoleKey.F7:
                        case ConsoleKey.F8:
                        case ConsoleKey.F9:
                        case ConsoleKey.F10:
                        case ConsoleKey.F11:
                            break;

                        case ConsoleKey.F2:
                            if (m_presenter != null)
                            {
                                string msgErr = null;
                                if (!m_presenter.Save(out msgErr))
                                    WriteMessageOnFooter(msgErr);
                                else if (m_presenter.ParentPresenter != null)
                                    m_presenter.ParentPresenter.Show(m_instance);
                            }
                            break;
                        case ConsoleKey.F3:
                            if (m_presenter != null)
                                m_presenter.Search(m_instance);
                            break;
                        case ConsoleKey.F5:
                            if (m_presenter != null)
                            {
                                int position = 0;
                                string value = GetCurrentScreenItemValue();
                                if (!string.IsNullOrEmpty(value) && int.TryParse(value, out position))
                                    m_presenter.SelectedItem(m_instance, position);
                            }
                            break;
                        case ConsoleKey.F12:
                            end = true;
                            break;
                        case ConsoleKey.UpArrow:
                            MoveToPreviousScreenItem();
                            break;
                        case ConsoleKey.Enter:
                        case ConsoleKey.DownArrow:
                            MoveToNextScreenItem();
                            break;
                        default:
                            ReadData(key);
                            break;
                    }
                }
            } while (!end);
        }

        public void CreateScreen()
        {
            SetWindowsSize();
            ClearScreen();

            if (m_presenter != null)
                WriteTitle(m_presenter.Name);

            DefColor(ConsoleColor.Gray, ConsoleColor.Black);
            CreateMenuScreen();

            DefColor(ConsoleColor.Black, ConsoleColor.Gray);
            DrawScreen();

            SetCursorScreenItem();
            MoveToNextScreenItem();

            ActivateKeyboard();
        }

        private static void DefColor(ConsoleColor backcolor, ConsoleColor forecolor)
        {
            Console.BackgroundColor = backcolor;
            Console.ForegroundColor = forecolor;
        }

        private void DrawScreen()
        {
            IEnumerator<KeyValuePair<string, ScreenItem>> cursor = m_screenItem.GetEnumerator();
            ScreenItem scr;

            while (cursor.MoveNext())
            {
                scr = cursor.Current.Value;
                Console.SetCursorPosition(scr.Column, scr.Row);
                Console.Write(GetScreenItemValue(scr));
            }

            cursor = null;
        }

        private static string GetScreenItemValue(ScreenItem scr)
        {
            object value = null;
            if (scr.ObjRef == null)
                value = Convert.ToString(scr.Value);
            else
                value = GetPropertyValue(scr.ObjRef, scr.PropertyName);

            string valueStr = Convert.ToString(value);
            return valueStr;
        }

        private void ReadData(ConsoleKeyInfo key)
        {
            int colData = -1;
            int colLength = 0;
            int rowData = -1;
            string valueStr = null;

            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    if (m_currentItem != null)
                    {
                        ScreenItem scr = m_currentItem.Value.Value;
                        if (scr.ColumnLength > 0)
                        {
                            scr.ColumnLength--;
                            scr.Value = scr.Value.Substring(0, scr.ColumnLength);
                            SetPropertyValue(scr.ObjRef, scr.PropertyName, scr.Value);
                            colData = scr.ColumnStart;
                            colLength = scr.ColumnLength;
                            rowData = scr.Row;
                            valueStr = scr.Value;
                            //
                            Console.SetCursorPosition(colData + colLength, rowData);
                            Console.Write(" ");
                        }
                    }
                    break;
                default:
                    if (m_currentItem != null && IsValidKeyChar(key.KeyChar))
                    {
                        ScreenItem scr = m_currentItem.Value.Value;
                        if (scr.ColumnLength < scr.ColumnEnd)
                        {
                            scr.Value += key.KeyChar;
                            SetPropertyValue(scr.ObjRef, scr.PropertyName, scr.Value);
                            scr.ColumnLength = scr.Value.Length;
                            colData = scr.ColumnStart;
                            colLength = scr.ColumnLength;
                            rowData = scr.Row;
                            valueStr = scr.Value;
                        }
                    }
                    break;
            }

            if (colData >= 0 && rowData >= 0 && !string.IsNullOrEmpty(valueStr))
            {
                Console.SetCursorPosition(colData, rowData);
                Console.Write(valueStr);
            }
        }

        private string GetCurrentScreenItemValue()
        {
            string val = null;

            if (m_currentItem != null)
            {
                ScreenItem scr = m_currentItem.Value.Value;
                val = scr.Value;
            }

            return val;
        }

        private static bool IsValidKeyChar(char keyChar)
        {
            return (char.IsLetterOrDigit(keyChar) ||
                    char.IsWhiteSpace(keyChar) ||
                    char.IsPunctuation(keyChar) ||
                    char.IsSeparator(keyChar));
        }

        private static object GetPropertyValue(object objectReference, string propertyName)
        {
            object value = null;

            PropertyInfo prop = objectReference.GetType().GetProperty(propertyName);
            if (prop != null)
                value = prop.GetValue(objectReference);

            return value;
        }

        private static void SetPropertyValue(object objectReference, string propertyName, object value)
        {
            PropertyInfo prop = objectReference.GetType().GetProperty(propertyName);
            if (prop != null)
                prop.SetValue(objectReference, value);
        }

        private void AddScreenItem(int column, int row, ScreenItem scrItem)
        {
            string scrKey = string.Format("{0:00}-{1:00}", column, row);
            m_screenItem.Add(scrKey, scrItem);
        }

        public void ScreenSection(int column, int row, string value)
        {
            int length = 0;
            if (!string.IsNullOrEmpty(value))
                length = value.Length;

            ScreenItem scrItem = new ScreenItem()
            {
                Column = column,
                Row = row,
                Value = value,
                ScreenType = null,
                ColumnStart = column,
                ColumnEnd = (column + length)
            };
            AddScreenItem(column, row, scrItem);
        }

        public void ScreenSection(int column, int row, object objectReference, string propertyName,
            EnScreenType screenType, int length)
        {
            ScreenItem scrItem = new ScreenItem()
            {
                Column = column,
                Row = row,
                Value = Convert.ToString(GetPropertyValue(objectReference, propertyName)),
                ObjRef = objectReference,
                PropertyName = propertyName,
                ScreenType = screenType,
                ColumnStart = column,
                ColumnEnd = (column + length)
            };
            AddScreenItem(column, row, scrItem);
        }

        public void WriteMessageOnFooter(string message)
        {
            int curColPosition = Console.CursorLeft;
            int curRowPosition = Console.CursorTop;

            DefColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.SetCursorPosition(0, 24);
            Console.Write(message);

            DefColor(ConsoleColor.Black, ConsoleColor.Gray);
            Console.SetCursorPosition(curColPosition, curRowPosition);
        }

        private void SetCursorScreenItem()
        {
            m_cursor = null;
            m_cursor = m_screenItem.GetEnumerator();
            m_currentItem = null;
        }

        private void MoveToNextScreenItem(bool goToBegin = true)
        {
            bool eof = true;
            IEnumerator<KeyValuePair<string, ScreenItem>> nextCursor = m_cursor;
            KeyValuePair<string, ScreenItem> nextItem = nextCursor.Current;

            while (nextCursor.MoveNext())
            {
                eof = false;
                nextItem = nextCursor.Current;
                ScreenItem scrItem = nextItem.Value;
                if (scrItem.ScreenType != null)
                    break;
            }

            if (eof)
            {
                SetCursorScreenItem();
                if (goToBegin)
                    MoveToNextScreenItem(false);
            }
            else
            {
                m_cursor = nextCursor;
                m_currentItem = nextItem;
                ScreenItem scr = m_currentItem.Value.Value;
                int col = (scr.ColumnStart + scr.ColumnLength);
                int row = scr.Row;
                Console.SetCursorPosition(col, row);
            }
        }

        private void MoveToPreviousScreenItem()
        {
            bool bof = true;
            IEnumerator<KeyValuePair<string, ScreenItem>> cursor = m_screenItem.GetEnumerator();
            IEnumerator<KeyValuePair<string, ScreenItem>> previousCursor = null;
            KeyValuePair<string, ScreenItem> previousItem = cursor.Current;

            while (cursor.MoveNext())
            {
                bof = false;

                if (cursor.Current.Key == m_currentItem.Value.Key)
                    break;

                ScreenItem scr = cursor.Current.Value;
                if (scr.ScreenType != null)
                {
                    previousCursor = cursor;
                    previousItem = previousCursor.Current;
                }
            }

            if (!bof && previousCursor != null)
            {
                m_cursor = previousCursor;
                m_currentItem = previousItem;
                ScreenItem scr = m_currentItem.Value.Value;
                int col = (scr.ColumnStart + scr.ColumnLength);
                int row = scr.Row;
                Console.SetCursorPosition(col, row);
            }
        }

        #region ScreenItem

        private class ScreenItem
        {
            public int Row { get; set; }
            public int Column { get; set; }
            public string Value { get; set; }
            public object ObjRef { get; set; }
            public string PropertyName { get; set; }
            public EnScreenType? ScreenType { get; set; }
            public int ColumnStart { get; set; }
            public int ColumnLength { get; set; }
            public int ColumnEnd { get; set; }
        }

        #endregion

        #region ScreenAction

        private class ScreenAction
        {
            public IPresenter Presenter { get; set; }
            public string Description { get; set; }
        }

        #endregion

    }
}
