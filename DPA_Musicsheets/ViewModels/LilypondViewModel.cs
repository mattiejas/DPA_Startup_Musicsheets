﻿using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Interfaces;
using DPA_Musicsheets.Managers.View;
using DPA_Musicsheets.States;
using DPA_Musicsheets.Mementos;
using System.Diagnostics;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase, IView<string>
    {
        private EditorContext _context { get; set; }
        private MusicLoader _musicLoader;
        private MainViewModel _mainViewModel { get; set; }

        private string _text;
        private string _previousText;
        private string _nextText;

        /// <summary>
        /// This text will be in the textbox.
        /// It can be filled either by typing or loading a file so we only want to set previoustext when it's caused by typing.
        /// </summary>
        public string LilypondText
        {
            get
            {
                return _text;
            }
            set
            {
                if (!_waitingForRender && !_textChangedByLoad)
                {
                    _previousText = _text;
                }
                _text = value;
                _context.CurrentEditorContent = value;
                RaisePropertyChanged(() => LilypondText);
            }
        }

        private bool _textChangedByLoad = false;
        private DateTime _lastChange;
        // private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private bool _waitingForRender = false;

        public LilypondViewModel(IViewManagerPool pool, EditorContext context)
        {
            _context = context;

            var viewManager = pool.GetInstance<LilypondViewManager>();
            viewManager.RegisterViewModel(this);

            _text = "Lilypond will appear here";
        }

        public void Load(string data)
        {
            _text = data;
            _context.CurrentEditorContent = _text;

            if (!_context.IsRestored)
            {
                _context.Caretaker.Backup();
            }

            LilypondTextLoaded(_text);
        }

        public void LilypondTextLoaded(string text)
        {
            _textChangedByLoad = true;
            LilypondText = _previousText = text;
            _textChangedByLoad = false;
        }

        /// <summary>
        /// This occurs when the text in the textbox has changed. This can either be by loading or typing.
        /// </summary>
        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            // If we were typing, we need to do things.
            if (!_textChangedByLoad)
            {
                _context.CurrentState.Handle();
                _context.CurrentEditorContent = _text;

                _waitingForRender = true;
                _lastChange = DateTime.Now;

                //_mainViewModel.CurrentState = "Rendering...";
                /*
                Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
                {
                    if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                    {
                        _waitingForRender = false;
                        UndoCommand.RaiseCanExecuteChanged();

                        // _musicLoader.LoadLilypondIntoWpfStaffsAndMidi(LilypondText);
                        //_mainViewModel.CurrentState = "";
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
                */

                _context.SetState(new IdleState(_context));
            }
        });

        #region Commands for buttons like Undo, Redo and SaveAs
        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            _context.Caretaker.Undo();
            _context.SetState(new GeneratingState(_context));
            _context.CurrentState.Handle();

            /*
            _nextText = LilypondText;
            LilypondText = _previousText;
            _previousText = null;
            */
        }, () => _context.Caretaker.IsUndoable());

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            _previousText = LilypondText;
            LilypondText = _nextText;
            _nextText = null;
            RedoCommand.RaiseCanExecuteChanged();
        }, () => _nextText != null && _nextText != LilypondText);

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            // TODO: In the application a lot of classes know which filetypes are supported. Lots and lots of repeated code here...
            // Can this be done better?
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    _musicLoader.SaveToMidi(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".ly"))
                {
                    _musicLoader.SaveToLilypond(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".pdf"))
                {
                    _musicLoader.SaveToPDF(saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
            }
        });

        #endregion Commands for buttons like Undo, Redo and SaveAs
    }
}
