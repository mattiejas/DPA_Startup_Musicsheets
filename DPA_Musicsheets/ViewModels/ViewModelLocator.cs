using System.Collections.Generic;
using Common.Interfaces;
using DPA_Musicsheets.Commands;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Managers.View;
using DPA_Musicsheets.States;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace DPA_Musicsheets.ViewModels
{
    /// <summary>
    /// This is the MVVM-Light implementation of dependency injection.
    /// </summary>
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IViewManagerPool, ViewManagerPool>();
            SimpleIoc.Default.Register<MusicLoader>();
            SimpleIoc.Default.Register<Invoker>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LilypondViewModel>();
            SimpleIoc.Default.Register<StaffsViewModel>();
            SimpleIoc.Default.Register<MidiPlayerViewModel>();
            SimpleIoc.Default.Register<EditorContext>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public LilypondViewModel LilypondViewModel => ServiceLocator.Current.GetInstance<LilypondViewModel>();
        public StaffsViewModel StaffsViewModel => ServiceLocator.Current.GetInstance<StaffsViewModel>();
        public MidiPlayerViewModel MidiPlayerViewModel => ServiceLocator.Current.GetInstance<MidiPlayerViewModel>();

        public static void Cleanup()
        {
            ServiceLocator.Current.GetInstance<MidiPlayerViewModel>().Cleanup();
        }
    }
}