using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using Windows.Globalization;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using System.Linq;

namespace LL
{
    
    sealed partial class App
    {

        public string ConStr;
        public string lang;
        //public Int16 lenpage;
        private readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            if (!(Window.Current.Content is Frame rootFrame)) {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false) {
                if (rootFrame.Content == null) {
                    // Если стек навигации не восстанавливается для перехода к первой странице,
                    // настройка новой страницы путем передачи необходимой информации в качестве параметра
                    // навигации
                    switch (ls.Values["LLang"])
                    {
                        case 1: ApplicationLanguages.PrimaryLanguageOverride = "en"; lang = "en"; break;
                        case 2: ApplicationLanguages.PrimaryLanguageOverride = "ru"; lang = "ru"; break;
                        default: lang = ApplicationLanguages.Languages.First().Substring(0,2); break;
                    }
                    rootFrame.Navigate(typeof(MainPage));
                    // Обеспечение активности текущего окна
                    Window.Current.Activate();
                }
            }
        }

        /// <summary>
        /// Вызывается в случае сбоя навигации на определенную страницу
        /// </summary>
        /// <param name="sender">Фрейм, для которого произошел сбой навигации</param>
        /// <param name="e">Сведения о сбое навигации</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
            deferral.Complete();
        }

    }
    public class Msg : Flyout
    {
        public Msg(string uid)
        {
            Content = new TextBlock() { Text = ResourceLoader.GetForCurrentView().GetString(uid) };
        }
    }
}
