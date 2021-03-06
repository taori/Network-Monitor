using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.UI;
using NetworkMonitor.Framework.Mvvm.Commands;
using NetworkMonitor.Framework.Mvvm.Interactivity;
using NetworkMonitor.Framework.Mvvm.Interactivity.Behaviors;
using NetworkMonitor.Framework.Mvvm.Interactivity.ViewModelBehaviors;
using ActivationContext = NetworkMonitor.Framework.Mvvm.Interactivity.ActivationContext;

namespace NetworkMonitor.Application.Dependencies
{
	public class MetroTabControllerAdapter : ITabController
	{
		private readonly IServiceProvider _serviceProvider;

		private readonly WeakReference<MetroTabControl> _metroTabControlReference;
		private MetroTabControl MetroTabControl =>
			_metroTabControlReference.TryGetTarget(out var control) 
				? control 
				: null;

		public MetroTabControllerAdapter(MetroTabControl metroTabControl, IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			metroTabControl.Unloaded += MetroTabControlOnUnloaded;
			_metroTabControlReference = new WeakReference<MetroTabControl>(metroTabControl);
		}

		private void MetroTabControlOnUnloaded(object sender, RoutedEventArgs e)
		{
			if (sender is MetroTabControl control)
			{
				control.Unloaded -= MetroTabControlOnUnloaded;
			}
		}

		public bool IsOperational => _metroTabControlReference.TryGetTarget(out _);

		public bool IsFocused(ITab model)
		{
			if (MetroTabControl.SelectedContent is MetroContentControl contentControl)
			{
				if (contentControl.Content.Equals(model))
					return true;
			}

			return false;
		}

		public void Insert(int index, ITab model)
		{
			if (!IsOperational)
				return;

			var tab = new MetroTabItem();
			if(System.Windows.Application.Current.TryFindResource("DefaultMetroTabItem") is var style && style != null)
				tab.Style = style as Style;

			var contentControl = new MetroContentControl();
			contentControl.Content = model;
			tab.Loaded += TabOnLoaded;
			tab.Content = contentControl;
			tab.Header = model.Title ?? "New Tab";
			tab.CloseButtonEnabled = model.Closable;
			tab.CloseButtonMargin = new Thickness(0, 8, 0, 0);
			tab.CloseTabCommand = new TaskCommand(o => Task.CompletedTask);

			model.WhenTitleChanged
				.ObserveOn(System.Windows.Application.Current.Dispatcher)
				.Subscribe(d => tab.Header = d);

			model.WhenCloseRequested
				.ObserveOn(System.Windows.Application.Current.Dispatcher)
				.Subscribe(d => MetroTabControl.Items.Remove(tab));

			MetroTabControl.Items.Insert(index, tab);
		}

		private async void TabOnLoaded(object sender, RoutedEventArgs e)
		{
			if (sender is MetroTabItem tabItem)
			{
				tabItem.Loaded -= TabOnLoaded;

				if (tabItem.Content is MetroContentControl contentControl)
				{
					if (contentControl.Content is ITab tab)
					{
						if (!string.IsNullOrWhiteSpace(tab.Title))
							tabItem.Header = tab.Title;

						tab.WhenTitleChanged
							.ObserveOn(SynchronizationContext.Current)
							.Subscribe(newTitle =>
							{
								if (!string.IsNullOrWhiteSpace(newTitle))
									tabItem.Header = newTitle;
							});

						tab.WhenClosableChanged
							.ObserveOn(SynchronizationContext.Current)
							.Subscribe(canClose => { tabItem.CloseButtonEnabled = canClose; });
					}

					if (contentControl.Content is IActivateable activate)
					{
						await activate.ActivateAsync(new ActivationContext(_serviceProvider));
					}
				}
			}
		}

		public int Add(ITab model)
		{
			if (!IsOperational)
				return -1;

			var count = MetroTabControl.Items.Count;
			Insert(count, model);
			return count;
		}

		public void Remove(ITab model)
		{
			if (!IsOperational)
				return;

			var index = FindIndex(model);
			if (index >= 0)
			{
				MetroTabControl.Items.RemoveAt(index);
			}
		}

		public int FindIndex(ITab model)
		{
			bool ContainsModel(object theModel, object control)
			{
				if (control is MetroTabItem tabItem)
					return ContainsModel(theModel, tabItem.Content);
				if (control is MetroContentControl contentControl)
					return object.ReferenceEquals(theModel, contentControl.Content);

				return false;
			}

			for (int i = 0; i < MetroTabControl.Items.Count; i++)
			{
				var controlItem = MetroTabControl.Items[i];
				if (ContainsModel(model, controlItem))
					return i;
			}

			return -1;
		}

		public void RemoveAt(int index)
		{
			if (!IsOperational)
				return;
			
			MetroTabControl.Items.RemoveAt(index);
		}

		public void Focus(ITab model)
		{
			if (!IsOperational)
				return;

			var index = FindIndex(model);
			if (index >= 0)
			{
				if (MetroTabControl.Items[index] is MetroTabItem tabItem)
				{
					tabItem.Focus();
				}
			}
		}


		public void FocusAt(int index)
		{
			if (!IsOperational)
				return;

			if (index < 0)
				return;

			if (MetroTabControl.Items[index] is MetroTabItem tabItem)
			{
				tabItem.Focus();
			}
		}

		public void FocusLast()
		{
			if (!IsOperational)
				return;

			if (MetroTabControl.Items.Count == 0)
				return;

			if (MetroTabControl.Items[MetroTabControl.Items.Count-1] is MetroTabItem tabItem)
			{
				tabItem.Focus();
			}
		}

		public int TabCount => MetroTabControl?.Items?.Count ?? 0;
	}
}