using DPA_Musicsheets.Commands;
using DPA_Musicsheets.Commands.Actions;
using DPA_Musicsheets.Commands.Handlers;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.States;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ICommand = System.Windows.Input.ICommand;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Invoker _invoker;
        private readonly IHandler _handler;
        private Shortcut _pressedKeys;
        private readonly EditorContext _editorContext;

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        /// <summary>
        /// The current state can be used to display some text.
        /// "Rendering..." is a text that will be displayed for example.
        /// </summary>
        private string _currentState;

        public string CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                RaisePropertyChanged(() => CurrentState);
            }
        }

        private readonly MusicLoader _musicLoader;

        public MainViewModel(MusicLoader musicLoader, Invoker invoker, EditorContext context)
        {
            _invoker = invoker;
            _pressedKeys = new Shortcut();
            _musicLoader = musicLoader;
            _editorContext = context;
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";

            _handler = new OpenFileHandler(_invoker, new Shortcut(Key.LeftCtrl, Key.O), SetFileName);
        }

        private void SetFileName(string name)
        {
            FileName = name;
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            var command = new OpenFileCommand(SetFileName);

            _invoker.SetCommand(command);
            _invoker.ExecuteCommand();
        });

        public ICommand LoadCommand => new RelayCommand(() =>
        {
            _musicLoader.Load(FileName);
            _editorContext.SavedState = _editorContext.CurrentEditorContent;
        });

        public ICommand OnLostFocusCommand => new RelayCommand(() => { Console.WriteLine("Maingrid Lost focus"); });

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            Console.WriteLine($"Key down: {e.Key}");
            _pressedKeys.Add(e.Key);

            var handled = _handler.Handle(new Request { Shortcut = _pressedKeys });
            _pressedKeys = handled?.Shortcut ?? _pressedKeys; // only set new pressed key if handled successfully
        });

        public ICommand OnKeyUpCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            Console.WriteLine("Key Up");
            _pressedKeys.Remove(e.Key);
        });

        public ICommand OnWindowClosingCommand => new RelayCommand<CancelEventArgs>((args) =>
        {
            if (_editorContext.SavedState != _editorContext.CurrentEditorContent)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    args.Cancel = true;
                    return;
                }
            }
            ViewModelLocator.Cleanup();
        });
    }
}