using System;
using System.Collections.Generic;
using System.Windows;
using MahApps.Metro.Controls;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NLog;

namespace NetworkMonitor.Application.Dependencies
{
	public class TabManager : ITabControllerManager
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(TabManager));

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.RegisterAttached(
			"ViewModel", typeof(ITabController), typeof(TabManager), new PropertyMetadata(default(ITabController), ControllerChanged));

		public static void SetViewModel(DependencyObject element, object value)
		{
			if (value != null || GetViewModel(element) == null)
				element.SetValue(ViewModelProperty, value);
		}

		public static object GetViewModel(DependencyObject element)
		{
			return (object) element.GetValue(ViewModelProperty);
		}

		public static readonly DependencyProperty ControllerNameProperty = DependencyProperty.RegisterAttached(
			"ControllerName", typeof(string), typeof(TabManager), new PropertyMetadata(default(string), ControllerNameChanged));

		public static void SetControllerName(DependencyObject element, string value)
		{
			element.SetValue(ControllerNameProperty, value);
		}

		public static string GetControllerName(DependencyObject element)
		{
			return (string) element.GetValue(ControllerNameProperty);
		}

		private static ITabController CreateTabController(object tabControl)
		{
			if (tabControl is MetroTabControl metroTabControl)
			{ 
				Log.Debug("Creating Metro Tab Controller adapter.");
				return new MetroTabControllerAdapter(metroTabControl);
			}

			throw new NotSupportedException();
		}

		private static readonly Dictionary<(string name, object viewModel), WeakReference<ITabController>> ControllerRegister = new Dictionary<(string name, object viewModel), WeakReference<ITabController>>();

		private static void RegisterTabController(DependencyObject control, object viewModel, string controllerName)
		{
			var tabController = CreateTabController(control);
			Log.Debug($"Registering tab controller with the name {controllerName}.");
			if (tabController != null)
				ControllerRegister.Add((controllerName, viewModel), new WeakReference<ITabController>(tabController));
		}

		private static void ControllerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RegisterTabController(d, GetViewModel(d), GetControllerName(d));
		}

		private static void ControllerNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RegisterTabController(d, GetViewModel(d), GetControllerName(d));
		}

		public ITabController GetController(object viewModel, string name)
		{
			if (ControllerRegister.TryGetValue((name, viewModel), out var reference))
			{
				if (reference.TryGetTarget(out var controller))
					return controller;
			}

			return null;
		}
	}

	public class MetroTabControllerAdapter : ITabController
	{
		private readonly MetroTabControl _metroTabControl;

		public MetroTabControllerAdapter(MetroTabControl metroTabControl)
		{
			_metroTabControl = metroTabControl;
		}

		public void InsertAt(int index, object model)
		{
			_metroTabControl.Items.Insert(index, model);
		}

		public void Insert(object model)
		{
			_metroTabControl.Items.Add(model);
		}

		public void Remove(object model)
		{
//			_metroTabControl.Items.Insert(index, model);
		}

		public void RemoveAt(int index)
		{
			_metroTabControl.Items.RemoveAt(index);
		}

		public void Focus(object model)
		{
//			_metroTabControl.Items.Insert(index, model);
		}

		public void FocusAt(int index)
		{
			var item = _metroTabControl.Items[index];
		}

		public int TabCount => _metroTabControl?.Items?.Count ?? 0;
	}
}