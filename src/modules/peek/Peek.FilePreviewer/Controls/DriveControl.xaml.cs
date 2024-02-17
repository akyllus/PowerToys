// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Peek.Common.Helpers;
using Peek.FilePreviewer.Previewers.Drive.Models;
using Windows.UI.ViewManagement;

namespace Peek.FilePreviewer.Controls
{
    public sealed partial class DriveControl : UserControl, IDisposable
    {
        private readonly UISettings _uiSettings = new();
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(DrivePreviewData),
            typeof(DriveControl),
            new PropertyMetadata(null, new PropertyChangedCallback((d, e) => ((DriveControl)d).SetSizeBarColor())));

        public DrivePreviewData? Source
        {
            get { return (DrivePreviewData)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public DriveControl()
        {
            ActualThemeChanged += ActualThemeChanged_Handler;
            _uiSettings.ColorValuesChanged += ColorValuesChanged_Handler;
            InitializeComponent();
        }

        public string FormatType(string type)
        {
            return string.Format(CultureInfo.CurrentCulture, ResourceLoaderInstance.ResourceLoader.GetString("Drive_Type"), type);
        }

        public string FormatFileSystem(string fileSystem)
        {
            return string.Format(CultureInfo.CurrentCulture, ResourceLoaderInstance.ResourceLoader.GetString("Drive_FileSystem"), fileSystem);
        }

        public string FormatCapacity(ulong capacity)
        {
            return string.Format(CultureInfo.CurrentCulture, ResourceLoaderInstance.ResourceLoader.GetString("Drive_Capacity"), ReadableStringHelper.BytesToReadableString(capacity, false));
        }

        public string FormatFreeSpace(ulong freeSpace)
        {
            return string.Format(CultureInfo.CurrentCulture, ResourceLoaderInstance.ResourceLoader.GetString("Drive_FreeSpace"), ReadableStringHelper.BytesToReadableString(freeSpace, false));
        }

        public string FormatUsedSpace(ulong usedSpace)
        {
            return string.Format(CultureInfo.CurrentCulture, ResourceLoaderInstance.ResourceLoader.GetString("Drive_UsedSpace"), ReadableStringHelper.BytesToReadableString(usedSpace, false));
        }

        public void Dispose()
        {
            ActualThemeChanged -= ActualThemeChanged_Handler;
            _uiSettings.ColorValuesChanged -= ColorValuesChanged_Handler;
        }

        private void SetSizeBarColor()
        {
            var accentDefaultColor = ((SolidColorBrush)Application.Current.Resources["AccentFillColorDefaultBrush"]).Color;
            UsedBarStart.Color = accentDefaultColor;
            UsedBarEnd.Color = accentDefaultColor;

            var accentDisabledColor = ((SolidColorBrush)Application.Current.Resources["AccentFillColorDisabledBrush"]).Color;
            FreeBarStart.Color = accentDisabledColor;
            FreeBarEnd.Color = accentDisabledColor;
        }

        private void ActualThemeChanged_Handler(FrameworkElement sender, object args)
        {
            SetSizeBarColor();
        }

        private void ColorValuesChanged_Handler(UISettings sender, object args)
        {
            _dispatcherQueue.TryEnqueue(SetSizeBarColor);
        }
    }
}
