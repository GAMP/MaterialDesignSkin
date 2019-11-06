using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel.Composition;
using System.Threading;
using Client;
using System.Windows.Threading;
using Client.ViewModels;
using System.Windows.Controls;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using SkinInterfaces;
using MaterialDesignSkin.ViewModels;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export(typeof(IShellWindow))]
    [Export(typeof(IDialogService))]
    [InheritedExport()]
    public partial class MainWindow : Window, IDialogService,
        IShellWindow
    {
        #region CONSTRUCTOR
        public MainWindow()
        {
            InitializeComponent();
            EventManager.RegisterClassHandler(typeof(FrameworkElement), GotFocusEvent, new RoutedEventHandler(OnRemoveFocusVisualStyle), true);
        }
        #endregion

        #region FIELDS

        private SemaphoreSlim DIALOG_LOCK = new SemaphoreSlim(1, 1);
        private SemaphoreSlim OVERLAY_LOCK = new SemaphoreSlim(1, 1);
        private CancellationTokenSource
            DIALOG_CTS,
            OVERLAY_CTS;
        private ConcurrentStack<ContentLock> OVERLAY_CONTENT_STACK = new ConcurrentStack<ContentLock>();

        #endregion

        #region NATIVE

        public enum HWND : int
        {
            HWND_TOP = 0,
            HWND_BOTTOM = 1,
            HWND_TOPMOST = -1,
            HWND_NOTOPMOST = -2,
        }

        public enum SWP : uint
        {
            SWP_NOSIZE = 0x0001,
            SWP_NOMOVE = 0x0002,
            SWP_NOZORDER = 0x0004,
            SWP_NOREDRAW = 0x0008,
            SWP_NOACTIVATE = 0x0010,
            SWP_FRAMECHANGED = 0x0020,
            SWP_SHOWWINDOW = 0x0040,
            SWP_HIDEWINDOW = 0x0080,
            SWP_NOCOPYBITS = 0x0100,
            SWP_NOOWNERZORDER = 0x0200,
            SWP_NOSENDCHANGING = 0x0400,
            SWP_DRAWFRAME = SWP_FRAMECHANGED,
            SWP_NOREPOSITION = SWP_NOOWNERZORDER,
            SWP_DEFERERASE = 0x2000,
            SWP_ASYNCWINDOWPOS = 0x4000,
            SWP_TOPMPOST = SWP_NOMOVE | SWP_NOSIZE,
        }

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order.</param>
        /// <param name="X">The new position of the left side of the window, in client coordinates. </param>
        /// <param name="Y">The new position of the top of the window, in client coordinates. </param>
        /// <param name="cx">The new width of the window, in pixels. </param>
        /// <param name="cy">The new height of the window, in pixels. </param>
        /// <param name="uFlags"></param>
        /// <returns>The window sizing and positioning flags.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos([In()] IntPtr hWnd,
            [In()][Optional()] HWND hWndInsertAfter,
            [In()] int X,
            [In()] int Y,
            [In()] int cx,
            [In()] int cy,
            [In()] SWP uFlags);

        /// <summary>
        /// The GetForegroundWindow function returns a handle to the foreground window (the window with which the user is currently working). The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads. 
        /// </summary>
        /// <returns>The return value is a handle to the foreground window. The foreground window can be NULL in certain circumstances, such as when a window is losing activation. </returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        #endregion

        #region IMPORTS

        [Import()]
        private IClinetCompositionService CompositionService
        {
            get; set;
        }

        #endregion

        #region INTERFACES

        public Task ShowDialogAsync(object content)
        {
            return ShowDialogAsync(content, CancellationToken.None);
        }

        public async Task ShowDialogAsync(object content, CancellationToken ct)
        {
            await ShowDialogInternalAsync(content, Timeout.InfiniteTimeSpan, ct);
        }

        public Task<bool> TryShowDialogAsync(object content)
        {
            return TryShowDialogAsync(content, CancellationToken.None);
        }

        public async Task<bool> TryShowDialogAsync(object content, CancellationToken ct)
        {
            try
            {
                await ShowDialogInternalAsync(content, TimeSpan.Zero, ct);
                return true;
            }
            catch (DialogLockException)
            {
                return false;
            }
            catch
            {
                throw;
            }
        }

        public Task<bool> ShowAcceptDialogAsync(string message)
        {
            return ShowAcceptDialogAsync(message, CancellationToken.None);
        }

        public Task<bool> ShowAcceptDialogAsync(string message, CancellationToken ct)
        {
            return ShowAcceptDialogAsync(message, MessageDialogButtons.Accept, ct);
        }

        public Task<bool> ShowAcceptDialogAsync(string message, MessageDialogButtons buttons)
        {
            return ShowAcceptDialogAsync(message, buttons, CancellationToken.None);
        }

        public async Task<bool> ShowAcceptDialogAsync(string message, MessageDialogButtons buttons, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            ////create view model
            //var viewModel = CompositionService.GetExportedValue<IMessageDialogViewModel>();
            //viewModel.Message = message;
            //viewModel.Buttons = buttons;

            ////create view on UI thread
            //var view = await Dispatcher.InvokeAsync(() => CompositionService.GetExportedValue<AcceptDialogView>());

            ////set data context on UI thread
            //await Dispatcher.InvokeAsync(() => view.DataContext = viewModel);

            //return await ShowDialogInternalAsync(view, Timeout.InfiniteTimeSpan, ct) is true;

            bool result = false;

            //create view model
            var viewModel = CompositionService.GetExportedValue<MessageDialogViewModel>();
            viewModel.Message = message;
            viewModel.Buttons = buttons;

            #region CREATE VIEWS

            //create view on UI thread
            var CONTENT = await Dispatcher.InvokeAsync(() => CompositionService.GetExportedValue<AcceptDialogViewEx>());

            //set data context on UI thread
            await Dispatcher.InvokeAsync(() => CONTENT.DataContext = viewModel); 

            #endregion

            var LINKED_CTS = CreateOverlayLinkedTokenSource(ct);
            var LINKED_CT = LINKED_CTS.Token;

            if(await OVERLAY_LOCK.WaitAsync(TimeSpan.Zero))
            {
                try
                {
                    viewModel.AcceptCommand = new SimpleCommand<object, object>((o) => true, (o) =>
                    {
                        HideCurrentOverlay(false);
                        result = true;
                    });
                    viewModel.CancelCommand = new SimpleCommand<object, object>((o) => true, (o) =>
                    {
                        HideCurrentOverlay(false);
                        result = false;
                    });

                    await ShowOverlayWithLockAsync(CONTENT, false, Timeout.InfiniteTimeSpan, LINKED_CT);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    OVERLAY_LOCK.Release();
                }
            }
            else
            {
                var WAIT_HANDLE = new SemaphoreSlim(0, 1);

                var CONTENT_LOCK = new ContentLock()
                {
                    Content = CONTENT,
                    WaitHandle = WAIT_HANDLE
                };

                OVERLAY_CONTENT_STACK.Push(CONTENT_LOCK);

                viewModel.AcceptCommand = new SimpleCommand<object, object>((o) => true, (o) =>
                {
                    try
                    {
                        WAIT_HANDLE.Release();
                    }
                    catch (SemaphoreFullException)
                    {
                        //happens sometimes when multiple calls to release are made
                    }

                    result = true;
                });
                viewModel.CancelCommand = new SimpleCommand<object, object>((o) => true, (o) =>
                {
                    try
                    {
                        WAIT_HANDLE.Release();
                    }
                    catch (SemaphoreFullException)
                    {
                        //happens sometimes when multiple calls to release are made
                    }

                    result = false;
                });

                await Dispatcher.InvokeAsync(() => 
                {
                    _OVERLAY_CONTENT_HOST.Content = CONTENT;
                });

                await WAIT_HANDLE.WaitAsync(Timeout.InfiniteTimeSpan, LINKED_CT);

                OVERLAY_CONTENT_STACK.TryPop(out _);

                if (OVERLAY_CONTENT_STACK.TryPeek(out var CURRENT_LOCK))
                {
                    await Dispatcher.InvokeAsync(() => _OVERLAY_CONTENT_HOST.Content = CURRENT_LOCK.Content);
                }
            }

            return result;
        }

        private Task<object> ShowDialogInternalAsync(object content, TimeSpan waitSpan, CancellationToken ct)
        {
            throw new NotSupportedException();
            //if (content == null)
            //    throw new ArgumentNullException(nameof(content));

            //var CANCELATION_TOKEN_SOURCE = CreateDialogLinkedTokenSource(ct);
            //var CANCELATION_TOKEN = CANCELATION_TOKEN_SOURCE.Token;

            //if (await DIALOG_LOCK.WaitAsync(waitSpan, CANCELATION_TOKEN) == true)
            //{
            //    try
            //    {
            //        return await Dispatcher.Invoke(async () =>
            //        {
            //            return await _MAIN_DIALOG.ShowDialog(content, delegate (object sender, DialogOpenedEventArgs args)
            //            {
            //                if (CANCELATION_TOKEN.IsCancellationRequested)
            //                {
            //                    Dispatcher.InvokeAsync(() => args.Session.Close(false));
            //                }
            //                else
            //                {
            //                    CANCELATION_TOKEN.Register(() => Dispatcher.Invoke(() =>
            //                    {
            //                        try
            //                        {
            //                            if (!args.Session.IsEnded)
            //                                args.Session.Close(false);
            //                        }
            //                        catch
            //                        {
            //                            throw;
            //                        }
            //                    }));
            //                }
            //            });
            //        });
            //    }
            //    catch
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        DIALOG_LOCK.Release();
            //    }
            //}
            //else
            //{
            //    throw new DialogLockException();
            //}
        }

        public void HideCurrentDialog()
        {
            DIALOG_CTS?.Cancel();
            DIALOG_CTS?.Dispose();
            DIALOG_CTS = null;
        }

        private CancellationTokenSource CreateDialogLinkedTokenSource(CancellationToken ct)
        {
            DIALOG_CTS = DIALOG_CTS ?? new CancellationTokenSource();
            return CancellationTokenSource.CreateLinkedTokenSource(DIALOG_CTS.Token, ct);
        }
        private CancellationTokenSource CreateOverlayLinkedTokenSource(CancellationToken ct)
        {
            OVERLAY_CTS = OVERLAY_CTS ?? new CancellationTokenSource();
            return CancellationTokenSource.CreateLinkedTokenSource(OVERLAY_CTS.Token, ct);
        }

        #region DIALOGLOCKEXCEPTION
        private class DialogLockException : Exception
        {
        } 
        #endregion

        #region OVERLAY

        private class ContentLock
        {
            public object Content
            {
                get; set;
            }

            public SemaphoreSlim WaitHandle
            {
                get; set;
            }
        }

        public event EventHandler<OverlayEventArgs> OverlayEvent;

        public Task<bool> TryShowOverlayAsync(object content)
        {
            return TryShowOverlayAsync(content, CancellationToken.None);
        }
        public Task<bool> TryShowOverlayAsync(object content, CancellationToken ct)
        {
            return ShowOverlayInternalAsync(content, true, TimeSpan.Zero, ct);
        }

        public Task ShowOverlayAsync(IMediaViewModel mediaModel)
        {
            return ShowOverlayAsync(mediaModel, true, CancellationToken.None);
        }
        public Task ShowOverlayAsync(IMediaViewModel mediaModel, bool allowClosing)
        {
            return ShowOverlayAsync(mediaModel, allowClosing, CancellationToken.None);
        }
        public Task ShowOverlayAsync(IMediaViewModel mediaModel, CancellationToken ct)
        {
            return ShowOverlayAsync(mediaModel, true, ct);
        }
        public async Task ShowOverlayAsync(IMediaViewModel mediaModel, bool allowClosing, CancellationToken ct)
        {
            if (mediaModel == null)
                throw new ArgumentNullException(nameof(mediaModel));

            var WINDOW_HANDLE = new WindowInteropHelper(this).Handle;
            var PRIMARY_SCREEN = System.Windows.Forms.Screen.PrimaryScreen;
            IntPtr PREVIOUS_WINDOW_HANDLE = IntPtr.Zero;
            try
            {
                var CANCELATION_TOKEN_SOURCE = CreateOverlayLinkedTokenSource(ct);
                var CANCELATION_TOKEN = CANCELATION_TOKEN_SOURCE.Token;

                var CONTENT = await Dispatcher.InvokeAsync(() =>
                {
                    var CC = new ContentControl()
                    {
                        Content = mediaModel,
                        ContentTemplate = FindResource("VIDEO_PLAYER_TEMPLATE") as DataTemplate,
                    };
                    return CC;
                });

                PREVIOUS_WINDOW_HANDLE = GetForegroundWindow();

                if (PREVIOUS_WINDOW_HANDLE == WINDOW_HANDLE)
                {
                    PREVIOUS_WINDOW_HANDLE = IntPtr.Zero;
                }

                SWP SHOW_FLAGS = SWP.SWP_NOREDRAW | SWP.SWP_ASYNCWINDOWPOS;
                HWND SHOW_HWND = PREVIOUS_WINDOW_HANDLE != IntPtr.Zero ? (HWND)PREVIOUS_WINDOW_HANDLE : HWND.HWND_NOTOPMOST;

                SetWindowPos(WINDOW_HANDLE,
                    SHOW_HWND,
                    0,
                    0,
                    PRIMARY_SCREEN.Bounds.Width,
                    PRIMARY_SCREEN.Bounds.Height,
                    SHOW_FLAGS);

                if (await OVERLAY_LOCK.WaitAsync(TimeSpan.Zero) == true)
                {
                    try
                    {
                        await ShowOverlayWithLockAsync(CONTENT, allowClosing, Timeout.InfiniteTimeSpan, CANCELATION_TOKEN);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        OVERLAY_LOCK.Release();
                    }
                }
                else
                {
                    var WAIT_HANDLE = new SemaphoreSlim(0, 1);
                    var CONTENT_LOCK = new ContentLock() { Content = CONTENT, WaitHandle = WAIT_HANDLE };
                    OVERLAY_CONTENT_STACK.Push(CONTENT_LOCK);

                    ct.Register(async () =>
                    {
                        var NEW_STACK = OVERLAY_CONTENT_STACK.Where(o => o != CONTENT_LOCK);
                        OVERLAY_CONTENT_STACK = new ConcurrentStack<ContentLock>(NEW_STACK);

                        if (OVERLAY_CONTENT_STACK.TryPeek(out var CURRENT_LOCK))
                        {
                            await Dispatcher.InvokeAsync(() => _OVERLAY_CONTENT_HOST.Content = CURRENT_LOCK.Content);
                        }
                    });

                    await Dispatcher.InvokeAsync(() => _OVERLAY_CONTENT_HOST.Content = CONTENT);
                    await WAIT_HANDLE.WaitAsync(Timeout.InfiniteTimeSpan, CANCELATION_TOKEN);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                var INSERT_AFTER = PREVIOUS_WINDOW_HANDLE != IntPtr.Zero ? (HWND)PREVIOUS_WINDOW_HANDLE : HWND.HWND_NOTOPMOST;
                SetWindowPos(WINDOW_HANDLE,
                    INSERT_AFTER,
                    0,
                    0,
                    PRIMARY_SCREEN.WorkingArea.Width,
                    PRIMARY_SCREEN.WorkingArea.Height,
                    SWP.SWP_NOREDRAW | SWP.SWP_ASYNCWINDOWPOS);
            }
        }

        public Task ShowOverlayAsync(object content)
        {
            return ShowOverlayAsync(content, CancellationToken.None);
        }
        public Task ShowOverlayAsync(object content, bool allowClosing)
        {
            return ShowOverlayInternalAsync(content, allowClosing, Timeout.InfiniteTimeSpan, CancellationToken.None);
        }
        public Task ShowOverlayAsync(object content, CancellationToken ct)
        {
            return ShowOverlayInternalAsync(content, true, Timeout.InfiniteTimeSpan, ct);
        }
        public Task ShowOverlayAsync(object content, bool allowClosing, CancellationToken ct)
        {
            return ShowOverlayInternalAsync(content, allowClosing, Timeout.InfiniteTimeSpan, ct);
        }

        private async Task<bool> ShowOverlayInternalAsync(object content, bool allowClosing, TimeSpan waitSpan, CancellationToken ct)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var CANCELATION_TOKEN_SOURCE = CreateOverlayLinkedTokenSource(ct);
            var CANCELATION_TOKEN = CANCELATION_TOKEN_SOURCE.Token;

            if (await OVERLAY_LOCK.WaitAsync(waitSpan, CANCELATION_TOKEN))
            {
                try
                {
                    await ShowOverlayWithLockAsync(content, allowClosing, waitSpan, CANCELATION_TOKEN);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    OVERLAY_LOCK.Release();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task ShowOverlayWithLockAsync(object content, bool allowClosing, TimeSpan waitSpan, CancellationToken ct)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            try
            {
                var WAIT_HANDLE = new SemaphoreSlim(0, 1);

                void TryPop()
                {
                    var CURRENT_OVERLAY_CONTENT = _OVERLAY_CONTENT_HOST.Content;
                    if (CURRENT_OVERLAY_CONTENT == null)
                        return;

                    if (OVERLAY_CONTENT_STACK.TryPop(out var TOP_CONTENT))
                    {
                        TOP_CONTENT.WaitHandle?.Release();
                    }

                    if (OVERLAY_CONTENT_STACK.IsEmpty)
                    {
                        WAIT_HANDLE.Release();
                    }
                    else
                    {
                        OVERLAY_CONTENT_STACK.TryPeek(out var NEW_CONTENT);
                        _OVERLAY_CONTENT_HOST.Content = NEW_CONTENT.Content;
                    }
                }

                void mouseButtonEventHandler(object sender, MouseButtonEventArgs args)
                {
                    if (!allowClosing)
                        return;

                    if (args.Source == this)
                        return;

                    if (args.Source != _OVERLAY_CONTENT_HOST)
                    {
                        TryPop();
                    }
                }

                void keyEventHandler(object sender, KeyEventArgs args)
                {
                    if (!allowClosing)
                        return;

                    if (args.Key != Key.Escape)
                        return;

                    TryPop();
                }

                MouseDown += mouseButtonEventHandler;
                KeyDown += keyEventHandler;

                OVERLAY_CONTENT_STACK.Push(new ContentLock() { Content = content });

                await Dispatcher.InvokeAsync(() =>
                {
                    //make overlay visible
                    OVERLAY_HOST.IsHitTestVisible = true;

                    //set desired content
                    _OVERLAY_CONTENT_HOST.Content = content;

                    if (!_OVERLAY_CONTENT_HOST.Focus())
                        return;
                    _OVERLAY_CONTENT_HOST.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                });

                OverlayEvent?.Invoke(this, new OverlayEventArgs(true));

                try
                {
                    await WAIT_HANDLE.WaitAsync(ct);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    MouseDown -= mouseButtonEventHandler;
                    KeyDown -= keyEventHandler;

                    await Dispatcher.InvokeAsync(() =>
                    {
                        //make overlay visible
                        OVERLAY_HOST.IsHitTestVisible = false;
                        _OVERLAY_CONTENT_HOST.Content = null;
                    });
                }
            }
            catch (OperationCanceledException)
            {
                if (IsOverlayGracefullClosed == false)
                    throw;
            }
            catch
            {
                throw;
            }
            finally
            {
                //release all locks
                OVERLAY_CONTENT_STACK?.ToList().ForEach(CL => CL.WaitHandle?.Release());

                //content stack is no longer valid
                OVERLAY_CONTENT_STACK?.Clear();

                //reset the gacefull close flag
                IsOverlayGracefullClosed = null;

                //raise event
                OverlayEvent?.Invoke(this, new OverlayEventArgs(false));
            }
        }

        public void HideCurrentOverlay()
        {
            HideCurrentOverlay(false);
        }

        public void HideCurrentOverlay(bool cancel)
        {
            IsOverlayGracefullClosed = OVERLAY_CTS != null ? !cancel : (bool?)null;

            OVERLAY_CTS?.Cancel();
            OVERLAY_CTS?.Dispose();
            OVERLAY_CTS = null;
        }

        private bool? IsOverlayGracefullClosed
        {
            get; set;
        }

        #endregion

        #endregion

        #region EVENT HANDLERS

        private static void OnRemoveFocusVisualStyle(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.FocusVisualStyle = null;
        }

        #endregion
    }
}
