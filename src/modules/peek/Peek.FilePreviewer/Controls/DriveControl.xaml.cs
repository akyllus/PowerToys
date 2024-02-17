// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Peek.Common.Helpers;
using Peek.FilePreviewer.Previewers.Drive.Models;

namespace Peek.FilePreviewer.Controls
{
    public sealed partial class DriveControl : UserControl
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(DrivePreviewData),
            typeof(DriveControl),
            new PropertyMetadata(null, new PropertyChangedCallback((d, e) => ((DriveControl)d).UpdateSizeBar())));

        public DrivePreviewData? Source
        {
            get { return (DrivePreviewData)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public DriveControl()
        {
            this.InitializeComponent();
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

        private void SizeChanged_Handler(object sender, SizeChangedEventArgs e)
        {
            UpdateSizeBar();
        }

        private void UpdateSizeBar()
        {
            if (Source != null && Source.Capacity > 0 && Source.UsedSpace > 0)
            {
                var usedRatio = ((double)Source.UsedSpace / Source.Capacity) * 100;
                var usedWidth = (CapacityBar.ActualWidth * usedRatio) / 100;
                UsedSpaceBar.Width = usedWidth < 10 ? 10 : usedWidth;
                UsedSpaceBar.Visibility = Visibility.Visible;
            }
            else
            {
                UsedSpaceBar.Visibility = Visibility.Collapsed;
            }
        }
    }
}
