using DPA_Musicsheets.Managers;
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
using DPA_Musicsheets.Commands;
using DPA_Musicsheets.Commands.Handlers;
using System.Collections.Generic;
using DPA_Musicsheets.Commands.Actions;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase, IView<string>
    {
        private readonly IHandler _handler;
        private List<Key> _pressedKeys;
        private Invoker _invoker;
        private MusicLoader _musicLoader { get; set; }
        private EditorContext _context { get; set; }
        private MainViewModel _mainViewModel { get; set; }


        /// <summary>
        /// This text will be in the textbox.
        /// It can be filled either by typing or loading a file so we only want to set previoustext when it's caused by typing.
        /// </summary>
        public string LilypondText
        {
            get
            {
                return _context.CurrentEditorContent;
            }
            set
            {
                _context.CurrentEditorContent = value;
                RaisePropertyChanged(() => LilypondText);
            }
        }

        public LilypondViewModel(IViewManagerPool pool, EditorContext context, Invoker invoker)
        {
            _invoker = invoker;

            _context = context;
            _context.CurrentEditorContent = "Lilypond will appear here";

            _pressedKeys = new List<Key>();

            var viewManager = pool.GetInstance<LilypondViewManager>();
            viewManager.RegisterViewModel(this);

            _handler = new SaveToPdfHandler(_invoker, new List<Key> { Key.LeftCtrl, Key.P, Key.S }, _context);
            _handler.SetNext(new SaveToLilypondHandler(_invoker, new List<Key> { Key.LeftCtrl, Key.S }, _context));
        }

        public void Load(string data)
        {
            _context.CurrentEditorContent = data;

            if (!_context.IsRestored)
            {
                _context.Caretaker.Backup();
            }

            LilypondText = _context.CurrentEditorContent;
        }

        /// <summary>
        /// This occurs when the text in the textbox has changed. This can either be by loading or typing.
        /// </summary>
        public System.Windows.Input.ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            _context.CurrentState.Handle();
            _context.SetState(new IdleState(_context));
        });

        #region Commands for buttons like Undo, Redo and SaveAs
        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            _context.Caretaker.Undo();
            _context.SetState(new GeneratingState(_context));
            _context.CurrentState.Handle();
        }, () => _context.Caretaker.IsUndoable());

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            _context.Caretaker.Redo();
            _context.SetState(new GeneratingState(_context));
            _context.CurrentState.Handle();
        }, () => _context.Caretaker.IsRedoable());

        public System.Windows.Input.ICommand SaveAsCommand => new RelayCommand(() =>
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
                    // new SaveToLilypondCommand(_context.CurrentEditorContent); Command kunnen we helaas niet gebruiken, omdat het een filename verwacht
                    _musicLoader.SaveToLilypond(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".pdf"))
                {
                    // new SaveToPdfCommand(_context.CurrentEditorContent); Command kunnen we helaas niet gebruiken, omdat het een filename verwacht
                    _musicLoader.SaveToPDF(saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
            }
        });


        public System.Windows.Input.ICommand OnLostFocusCommand => new RelayCommand(() =>
        {
            Console.WriteLine("lilypondviewmodel Lost focus");
        });

        public System.Windows.Input.ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            Console.WriteLine($"lilypondviewmodel Key down: {e.Key}");
            _pressedKeys.Add(e.Key);

            _handler.Handle(new Request
            {
                PressedKeys = _pressedKeys
            });
        });

        public System.Windows.Input.ICommand OnKeyUpCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            Console.WriteLine("lilypondviewmodel Key Up");
            _pressedKeys.Remove(e.Key);
        });

        #endregion Commands for buttons like Undo, Redo and SaveAs
    }
}
