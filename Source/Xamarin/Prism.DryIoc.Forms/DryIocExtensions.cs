using DryIoc;
using Prism.Common;
using Prism.Mvvm;
using System;
using Xamarin.Forms;

namespace Prism.DryIoc
{
    /// <summary>
    /// Extension methods to register a <see cref="Xamarin.Forms.Page"/> with <see cref="PageNavigationRegistry"/> using <see cref="IContainer"/>
    /// </summary>
    public static class DryIocExtensions
    {
        /// <summary>
        /// Registers a Page for navigation.
        /// </summary>
        /// <typeparam name="TView">The Type of Page to register</typeparam>
        /// <param name="name">The unique name to register with the Page</param>
        public static void RegisterTypeForNavigation<TView>(this IContainer container, string name = null) where TView : Page
        {
            var viewType = typeof(TView);

            if (string.IsNullOrWhiteSpace(name))
                name = viewType.Name;

            container.RegisterTypeForNavigation(viewType, name);
        }

        /// <summary>
        /// Registers a Page for navigation
        /// </summary>
        /// <param name="container"><see cref="IUnityContainer"/> used to register type for Navigation.</param>
        /// <param name="viewType">The type of Page to register</param>
        /// <param name="name">The unique name to register with the Page</param>
        /// <returns><see cref="IUnityContainer"/></returns>
        public static void RegisterTypeForNavigation(this IContainer container, Type viewType, string name)
        {
            PageNavigationRegistry.Register(name, viewType);
            container.Register(typeof(object), viewType, serviceKey: name);
        }

        /// <summary>
        /// Registers a Page for navigation.
        /// </summary>
        /// <typeparam name="TView">The Type of Page to register</typeparam>
        /// <typeparam name="TViewModel">The ViewModel to use as the unique name for the Page</typeparam>
        public static void RegisterTypeForNavigation<TView, TViewModel>(this IContainer container, string name = null) 
            where TView : Page
            where TViewModel : class
        {
            container.RegisterTypeForNavigationWithViewModel<TViewModel>(typeof(TView), name);
        }

        /// <summary>
        /// Registers a Page for navigation based on the current Device OS using a shared ViewModel
        /// </summary>
        /// <typeparam name="TView">Default View Type to be shared across multiple Device Operating Systems if they are not specified directly.</typeparam>
        /// <typeparam name="TViewModel">Shared ViewModel Type</typeparam>
        /// <param name="container"><see cref="IContainer"/> used to register type for Navigation.</param>
        /// <param name="name">The unique name to register with the Page. If left empty or null will default to the ViewModel root name. i.e. MyPageViewModel => MyPage</param>
        /// <param name="androidView">Android Specific View Type</param>
        /// <param name="iOSView">iOS Specific View Type</param>
        /// <param name="otherView">Other Platform Specific View Type</param>
        /// <param name="windowsView">Windows Specific View Type</param>
        /// <param name="winPhoneView">Windows Phone Specific View Type</param>
        public static void RegisterTypeForNavigationOnPlatform<TView, TViewModel>(this IContainer container, string name = null, Type androidView = null, Type iOSView = null, Type otherView = null, Type windowsView = null, Type winPhoneView = null)
            where TView : Page
            where TViewModel : class
        {
            if (string.IsNullOrWhiteSpace(name))
                name = typeof(TView).Name;

            if (Device.OS == TargetPlatform.Android && androidView != null)
            {
                container.RegisterTypeForNavigationWithViewModel<TViewModel>(androidView, name);
            }
            else if (Device.OS == TargetPlatform.iOS && iOSView != null)
            {
                container.RegisterTypeForNavigationWithViewModel<TViewModel>(iOSView, name);
            }
            else if (Device.OS == TargetPlatform.Other && otherView != null)
            {
                container.RegisterTypeForNavigationWithViewModel<TViewModel>(otherView, name);
            }
            else if (Device.OS == TargetPlatform.Windows && windowsView != null)
            {
                container.RegisterTypeForNavigationWithViewModel<TViewModel>(windowsView, name);
            }
            else if (Device.OS == TargetPlatform.WinPhone && winPhoneView != null)
            {
                container.RegisterTypeForNavigationWithViewModel<TViewModel>(winPhoneView, name);
            }
            else
            {
                container.RegisterTypeForNavigation<TView, TViewModel>(name);
            }
        }

        /// <summary>
        /// Registers a Page for navigation based on the Device Idiom using a shared ViewModel
        /// </summary>
        /// <typeparam name="TView">Default View Type to be used across multiple Idioms if they are not specified directly.</typeparam>
        /// <typeparam name="TViewModel">The shared ViewModel</typeparam>
        /// <param name="container"><see cref="IContainer"/> used to register type for Navigation.</param>
        /// <param name="name">The common name used for Navigation. If left empty or null will default to the ViewModel root name. i.e. MyPageViewModel => MyPage</param>
        /// <param name="desktopView">Desktop Specific View Type</param>
        /// <param name="tabletView">Tablet Specific View Type</param>
        /// <param name="phoneView">Phone Specific View Type</param>
        public static void RegisterTypeForNavigationOnIdiom<TView, TViewModel>(this IContainer container, string name = null, Type desktopView = null, Type tabletView = null, Type phoneView = null)
            where TView : Page
            where TViewModel : class
        {
            if (string.IsNullOrWhiteSpace(name))
                name = typeof(TView).Name;

            if (Device.Idiom == TargetIdiom.Desktop && desktopView != null)
            {
                container.RegisterTypeForNavigationWithViewModel<TViewModel>(desktopView, name);
            }
            else if (Device.Idiom == TargetIdiom.Phone && phoneView != null)
            {
                container.RegisterTypeForNavigationWithViewModel<TViewModel>(phoneView, name);
            }
            else if (Device.Idiom == TargetIdiom.Tablet && tabletView != null)
            {
                container.RegisterTypeForNavigationWithViewModel<TViewModel>(tabletView, name);
            }
            else
            {
                container.RegisterTypeForNavigation<TView, TViewModel>(name);
            }
        }

        //TODO: decide if Prism will continue to support navigating via ViewModels, or should we stick to just a single approach to navigation
        //public static void RegisterTypeForViewModelNavigation<TView, TViewModel>(this IContainer container)
        //    where TView : Page
        //    where TViewModel : class
        //{
        //    container.RegisterTypeForNavigationWithViewModel<TViewModel>(typeof(TView), typeof(TViewModel).FullName);
        //}

        //public static void RegisterTypeForViewModelNavigationOnPlatform<TView, TViewModel>(this IContainer container, Type AndroidView = null, Type iOSView = null, Type OtherView = null, Type WindowsView = null, Type WinPhoneView = null)
        //    where TView : Page
        //    where TViewModel : class
        //{
        //    container.RegisterTypeForNavigationOnPlatform<TView, TViewModel>(typeof(TViewModel).FullName, AndroidView, iOSView, OtherView, WindowsView, WinPhoneView);
        //}

        //public static void RegisterTypeForViewModelNavigationOnIdiom<TView, TViewModel>(this IContainer container, Type DesktopView = null, Type TabletView = null, Type PhoneView = null)
        //    where TView : Page
        //    where TViewModel : class
        //{
        //    container.RegisterTypeForNavigationOnIdiom<TView, TViewModel>(typeof(TViewModel).FullName, DesktopView, TabletView, PhoneView);
        //}

        private static void RegisterTypeForNavigationWithViewModel<TViewModel>(this IContainer container, Type viewType, string name = null)
            where TViewModel : class
        {
            if (string.IsNullOrWhiteSpace(name))
                name = viewType.Name;

            ViewModelLocationProvider.Register(viewType.ToString(), typeof(TViewModel));

            container.RegisterTypeForNavigation(viewType, name);
        }
    }
}