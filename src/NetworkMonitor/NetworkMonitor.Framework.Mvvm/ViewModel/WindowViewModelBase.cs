using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using NetworkMonitor.Framework.Extensions;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.Environment;
using NetworkMonitor.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity;
using NetworkMonitor.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NetworkMonitor.Framework.Mvvm.Abstraction.ViewModel;
using NetworkMonitor.Framework.Mvvm.Integration.ViewMapping;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace NetworkMonitor.Framework.Mvvm.ViewModel
{
	public abstract class WindowViewModelBase : InteractiveViewModel, IServiceProviderHolder, IWindowViewModel, IDefaultBehaviorProvider
	{
		protected WindowViewModelBase([NotNull] IWindowContentViewModel content)
		{
			Content = content ?? throw new ArgumentNullException(nameof(content));
			if (content is IWindowSetter setter)
				setter.Set(this);
			if (content is IConfigureWindow configure)
				configure.Configure(this);
		}

		protected static readonly ILogger Log = LogManager.GetLogger(nameof(WindowViewModelBase));

		private string _title;

		public string Title
		{
			get => _title ?? GetWindowTitle();
			set => SetValue(ref _title, value, nameof(Title));
		}

		private double _width = 800;

		public double Width
		{
			get => _width;
			set => SetValue(ref _width, value, nameof(Width));
		}

		private double _minWidth = 100;

		public double MinWidth
		{
			get => _minWidth;
			set => SetValue(ref _minWidth, value, nameof(MinWidth));
		}

		private double _maxWidth = int.MaxValue;

		public double MaxWidth
		{
			get => _maxWidth;
			set => SetValue(ref _maxWidth, value, nameof(MaxWidth));
		}

		private double _height = 450;

		public double Height
		{
			get => _height;
			set => SetValue(ref _height, value, nameof(Height));
		}

		private double _minHeight = 100;

		public double MinHeight
		{
			get => _minHeight;
			set => SetValue(ref _minHeight, value, nameof(MinHeight));
		}

		private double _maxHeight = int.MaxValue;

		public double MaxHeight
		{
			get => _maxHeight;
			set => SetValue(ref _maxHeight, value, nameof(MaxHeight));
		}

		private double _top;

		public double Top
		{
			get => _top;
			set => SetValue(ref _top, value, nameof(Top));
		}

		private double _left;

		public double Left
		{
			get => _left;
			set => SetValue(ref _left, value, nameof(Left));
		}

		private bool _resizeable = true;

		public bool Resizeable
		{
			get => _resizeable;
			set
			{
				if (SetValue(ref _resizeable, value, nameof(Resizeable)))
					OnPropertyChanged(nameof(ResizeMode));
			}
		}

		private bool _minimizable = true;

		public bool Minimizable
		{
			get => _minimizable;
			set
			{
				if (SetValue(ref _minimizable, value, nameof(Minimizable)))
					OnPropertyChanged(nameof(ResizeMode));
			}
		}

		public ResizeMode ResizeMode
		{
			get
			{
				var mode = ResizeMode.CanMinimize | ResizeMode.CanResize;
				if (!Minimizable)
					mode = mode & ~ResizeMode.CanMinimize;
				if (!Resizeable)
					mode = mode & ~ResizeMode.CanResize;
				return mode;
			}
			set => throw new NotSupportedException();
		}

		private IWindowContentViewModel _content;

		public IWindowContentViewModel Content
		{
			get => _content;
			set => SetValue(ref _content, value, nameof(Content));
		}

		private SizeToContent _sizeToContent = SizeToContent.Manual;

		public SizeToContent SizeToContent
		{
			get => _sizeToContent;
			set => SetValue(ref _sizeToContent, value, nameof(SizeToContent));
		}

		private bool _showInTaskbar = true;

		public bool ShowInTaskbar
		{
			get => _showInTaskbar;
			set => SetValue(ref _showInTaskbar, value, nameof(ShowInTaskbar));
		}

		public virtual bool ClaimMainWindowOnOpen => false;

		/// <inheritdoc />
		public void Focus()
		{
			Log.Trace($"{nameof(Focus)} requested.");
			_whenFocusRequested?.TryOnNext(null);
		}

		/// <inheritdoc />
		public void Normalize()
		{
			Log.Trace($"{nameof(Normalize)} requested.");
			_whenNormalizeRequested?.TryOnNext(null);
		}

		/// <inheritdoc />
		public void Maximize()
		{
			Log.Trace($"{nameof(Maximize)} requested.");
			_whenMaximizeRequested?.TryOnNext(null);
		}

		/// <inheritdoc />
		public void Minimize()
		{
			Log.Trace($"{nameof(Minimize)} requested.");
			_whenMinimizeRequested?.TryOnNext(null);
		}

		/// <inheritdoc />
		public void Close()
		{
			Log.Trace($"{nameof(Close)} requested.");
			_whenClosingRequested?.TryOnNext(null);
		}

		private Subject<object> _whenFocusRequested = new Subject<object>();
		/// <inheritdoc />
		public IObservable<object> WhenFocusRequested => _whenFocusRequested;

		private Subject<object> _whenClosingRequested = new Subject<object>();
		/// <inheritdoc />
		public IObservable<object> WhenClosingRequested => _whenClosingRequested;

		private Subject<object> _whenNormalizeRequested = new Subject<object>();
		/// <inheritdoc />
		public IObservable<object> WhenNormalizeRequested => _whenNormalizeRequested;

		private Subject<object> _whenMinimizeRequested = new Subject<object>();
		/// <inheritdoc />
		public IObservable<object> WhenMinimizeRequested => _whenMinimizeRequested;

		private Subject<object> _whenMaximizeRequested = new Subject<object>();
		/// <inheritdoc />
		public IObservable<object> WhenMaximizeRequested => _whenMaximizeRequested;

		private Subject<object> _whenClosed = new Subject<object>();
		public IObservable<object> WhenClosed => _whenClosed;

		private Subject<CancelEventArgs> _whenClosing = new Subject<CancelEventArgs>();
		public IObservable<CancelEventArgs> WhenClosing => _whenClosing;

		private Subject<IActivationContext> _whenActivated = new Subject<IActivationContext>();
		public IObservable<IActivationContext> WhenActivated => _whenActivated;

		private Subject<WindowState> _whenStateChanged = new Subject<WindowState>();
		public IObservable<WindowState> WhenStateChanged => _whenStateChanged;

		private Subject<Point> _whenLocationChanged = new Subject<Point>();
		public IObservable<Point> WhenLocationChanged => _whenLocationChanged;

		private Subject<SizeChangedEventArgs> _whenSizeChanged = new Subject<SizeChangedEventArgs>();
		public IObservable<SizeChangedEventArgs> WhenSizeChanged => _whenSizeChanged;

		/// <inheritdoc />
		public async Task ActivateAsync(IActivationContext context)
		{
			await OnActivateAsync(context);
			await Content.ActivateAsync(context);
			_whenActivated.TryOnNext(context);
		}

		protected virtual Task OnActivateAsync(IActivationContext context) => Task.CompletedTask;

		protected virtual string GetWindowTitle()
		{
			return Content.GetTitle();
		}

		protected async Task<bool> UpdateRegionAsync(IContentViewModel content, string regionName)
		{
			Log.Debug($"Updating region [{regionName}] with [{content}]");
			using (LoadingState.Session())
			{
				var visualizerFactory = ServiceProvider.GetRequiredService<IDisplayCoordinatorFactory>();
				var visualizer = visualizerFactory.Create(content);
				return await visualizer.DisplayAsync(content, new RegionArguments(this, regionName));
			}
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; set; }

		/// <inheritdoc />
		protected override void Dispose(bool managedDispose)
		{
			if (managedDispose)
			{
				_whenFocusRequested.Dispose();
				_whenMaximizeRequested.Dispose();
				_whenMinimizeRequested.Dispose();
				_whenClosingRequested.Dispose();
				_whenNormalizeRequested.Dispose();

				_whenClosed.Dispose();
				_whenClosing.Dispose();
				_whenActivated.Dispose();
				_whenSizeChanged.Dispose();
				_whenStateChanged.Dispose();
				_whenLocationChanged.Dispose();
			}

			base.Dispose(managedDispose);
		}

		/// <inheritdoc />
		public void NotifyClosed()
		{
			Log.Trace(nameof(NotifyClosed));
			_whenClosed.TryOnNext(null);
		}

		/// <inheritdoc />
		public void NotifyClosing(CancelEventArgs args)
		{
			Log.Trace(nameof(NotifyClosing));
			_whenClosing.TryOnNext(args);
		}

		/// <inheritdoc />
		public void NotifyWindowStateChange(WindowState args)
		{
			Log.Trace($"{nameof(NotifyWindowStateChange)} {args}");
			_whenStateChanged.TryOnNext(args);
		}

		/// <inheritdoc />
		public void NotifyLocationChanged(Point args)
		{
			Log.Trace($"{nameof(NotifyLocationChanged)} {args}");
			_whenLocationChanged.TryOnNext(args);
		}

		/// <inheritdoc />
		public void NotifySizeChanged(SizeChangedEventArgs args)
		{
			Log.Trace($"{nameof(NotifySizeChanged)} {args.NewSize}");
			_whenSizeChanged.TryOnNext(args);
		}

		/// <inheritdoc />
		public IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			return Content.GetDefaultBehaviors();
		}
	}
}