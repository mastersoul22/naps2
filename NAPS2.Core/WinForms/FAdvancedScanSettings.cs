/*
    NAPS2 (Not Another PDF Scanner 2)
    http://sourceforge.net/projects/naps2/
    
    Copyright (C) 2009       Pavel Sorejs
    Copyright (C) 2012       Michael Adams
    Copyright (C) 2013       Peter De Leeuw
    Copyright (C) 2012-2015  Ben Olden-Cooligan

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using NAPS2.Scan;
using NAPS2.Scan.Twain;

namespace NAPS2.WinForms
{
    public partial class FAdvancedScanSettings : FormBase
    {
        public FAdvancedScanSettings()
        {
            InitializeComponent();

            cmbTwainImpl.Format += (sender, e) => e.Value = ((Enum) e.ListItem).Description();
            cmbTwainImpl.Items.Add(TwainImpl.Default);
            cmbTwainImpl.Items.Add(TwainImpl.Modern);
            cmbTwainImpl.Items.Add(TwainImpl.Legacy);
            if (Environment.Is64BitProcess)
            {
                cmbTwainImpl.Items.Add(TwainImpl.X64);
            }
        }

        protected override void OnLoad(object sender, EventArgs e)
        {
            cbHighQuality.Checked = ScanProfile.MaxQuality;
            tbImageQuality.Value = ScanProfile.Quality;
            txtImageQuality.Text = ScanProfile.Quality.ToString("G");
            cbBrightnessContrastAfterScan.Checked = ScanProfile.BrightnessContrastAfterScan;
            cbForcePageSize.Checked = ScanProfile.ForcePageSize;
            if (ScanProfile.TwainImpl != TwainImpl.X64 || Environment.Is64BitProcess)
            {
                cmbTwainImpl.SelectedIndex = (int) ScanProfile.TwainImpl;
            }
            cbExcludeBlankPages.Checked = ScanProfile.ExcludeBlankPages;
            tbWhiteThreshold.Value = ScanProfile.BlankPageWhiteThreshold;
            txtWhiteThreshold.Text = ScanProfile.BlankPageWhiteThreshold.ToString("G");
            tbCoverageThreshold.Value = ScanProfile.BlankPageCoverageThreshold;
            txtCoverageThreshold.Text = ScanProfile.BlankPageCoverageThreshold.ToString("G");

            UpdateEnabled();

            new LayoutManager(this)
                .Bind(groupBox1, groupBox2, groupBox3, tbImageQuality, tbWhiteThreshold, tbCoverageThreshold)
                    .WidthToForm()
                .Bind(txtImageQuality, txtWhiteThreshold, txtCoverageThreshold, btnOK, btnCancel)
                    .RightToForm()
                .Activate();
        }

        private void UpdateEnabled()
        {
            cmbTwainImpl.Enabled = ScanProfile.DriverName == TwainScanDriver.DRIVER_NAME;
            tbImageQuality.Enabled = !cbHighQuality.Checked;
            txtImageQuality.Enabled = !cbHighQuality.Checked;
            tbWhiteThreshold.Enabled = cbExcludeBlankPages.Checked && ScanProfile.BitDepth != ScanBitDepth.BlackWhite;
            txtWhiteThreshold.Enabled = cbExcludeBlankPages.Checked && ScanProfile.BitDepth != ScanBitDepth.BlackWhite;
            tbCoverageThreshold.Enabled = cbExcludeBlankPages.Checked;
            txtCoverageThreshold.Enabled = cbExcludeBlankPages.Checked;
        }

        public ScanProfile ScanProfile { get; set; }

        private void SaveSettings()
        {
            ScanProfile.Quality = tbImageQuality.Value;
            ScanProfile.MaxQuality = cbHighQuality.Checked;
            ScanProfile.BrightnessContrastAfterScan = cbBrightnessContrastAfterScan.Checked;
            ScanProfile.ForcePageSize = cbForcePageSize.Checked;
            if (cmbTwainImpl.SelectedIndex != -1)
            {
                ScanProfile.TwainImpl = (TwainImpl) cmbTwainImpl.SelectedIndex;
            }
            ScanProfile.ExcludeBlankPages = cbExcludeBlankPages.Checked;
            ScanProfile.BlankPageWhiteThreshold = tbWhiteThreshold.Value;
            ScanProfile.BlankPageCoverageThreshold = tbCoverageThreshold.Value;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbImageQuality_Scroll(object sender, EventArgs e)
        {
            txtImageQuality.Text = tbImageQuality.Value.ToString("G");
        }

        private void txtImageQuality_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(txtImageQuality.Text, out value))
            {
                if (value >= tbImageQuality.Minimum && value <= tbImageQuality.Maximum)
                {
                    tbImageQuality.Value = value;
                }
            }
        }

        private void cbHighQuality_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void cbExcludeBlankPages_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void tbWhiteThreshold_Scroll(object sender, EventArgs e)
        {
            txtWhiteThreshold.Text = tbWhiteThreshold.Value.ToString("G");
        }

        private void txtWhiteThreshold_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(txtWhiteThreshold.Text, out value))
            {
                if (value >= tbWhiteThreshold.Minimum && value <= tbWhiteThreshold.Maximum)
                {
                    tbWhiteThreshold.Value = value;
                }
            }
        }

        private void tbCoverageThreshold_Scroll(object sender, EventArgs e)
        {
            txtCoverageThreshold.Text = tbCoverageThreshold.Value.ToString("G");
        }

        private void txtCoverageThreshold_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(txtCoverageThreshold.Text, out value))
            {
                if (value >= tbCoverageThreshold.Minimum && value <= tbCoverageThreshold.Maximum)
                {
                    tbCoverageThreshold.Value = value;
                }
            }
        }
    }
}
