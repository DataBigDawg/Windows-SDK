/** 
  * Copyright 2014 Accela, Inc. 
  * 
  * You are hereby granted a non-exclusive, worldwide, royalty-free license to 
  * use, copy, modify, and distribute this software in source code or binary 
  * form for use in connection with the web services and APIs provided by 
  * Accela. 
  * 
  * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
  * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
  * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
  * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
  * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
  * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
  * DEALINGS IN THE SOFTWARE. 
  * 
  */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration.Pnp;
using Windows.System;
using Windows.System.Profile;

namespace Accela.WindowsStoreSDK
{
    internal class DeviceInfoHelper
    {
        public async static Task<DeviceInfo> GetDeviceInfoAsync()
        {
            DeviceInfo info = new DeviceInfo();
            info.DeviceID = GetDeviceID();
            info.WindowsVersion = await GetWindowsVersionAsync();
            info.DeviceManufacturer = await GetDeviceManufacturerAsync();
            info.DeviceModel = await GetDeviceModelAsync();
            info.DeviceCategory = await GetDeviceCategoryAsync();
            info.DeviceOS = info.WindowsVersion + "-" +
                info.DeviceManufacturer + "-" +
                info.DeviceModel + "-" +
                info.DeviceCategory;

            return info;
        }

        public static string GetDeviceID()
        {
            HardwareToken packageSpecificToken = HardwareIdentification.GetPackageSpecificToken(null);

            var hardwareId = packageSpecificToken.Id;

            var _internalId = "";
            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);
            var array = new byte[hardwareId.Length];
            dataReader.ReadBytes(array);
            for (var i = 0; i < array.Length; i++)
            {
                _internalId += array[i].ToString();
            }
            return _internalId;
        }

        const string ItemNameKey = "System.ItemNameDisplay";
        const string ModelNameKey = "System.Devices.ModelName";
        const string ManufacturerKey = "System.Devices.Manufacturer";
        const string DeviceClassKey = "{A45C254E-DF1C-4EFD-8020-67D146A850E0},10";
        const string PrimaryCategoryKey = "{78C34FC8-104A-4ACA-9EA4-524D52996E57},97";
        const string DeviceDriverVersionKey = "{A8B865DD-2E3D-4094-AD97-E593A70C75D6},3";
        const string RootContainer = "{00000000-0000-0000-FFFF-FFFFFFFFFFFF}";
        const string RootQuery = "System.Devices.ContainerId:=\"" + RootContainer + "\"";
        const string HalDeviceClass = "4d36e966-e325-11ce-bfc1-08002be10318";

        public async static Task<ProcessorArchitecture> GetProcessorArchitectureAsync()
        {
            var halDevice = await GetHalDevice(ItemNameKey);
            if (halDevice != null && halDevice.Properties[ItemNameKey] != null)
            {
                var halName = halDevice.Properties[ItemNameKey].ToString();
                if (halName.Contains("x64")) return ProcessorArchitecture.X64;
                if (halName.Contains("ARM")) return ProcessorArchitecture.Arm;
                return ProcessorArchitecture.X86;
            }
            return ProcessorArchitecture.Unknown;
        }

        public static Task<string> GetDeviceManufacturerAsync()
        {
            return GetRootDeviceInfoAsync(ManufacturerKey);
        }

        public static Task<string> GetDeviceModelAsync()
        {
            return GetRootDeviceInfoAsync(ModelNameKey);
        }

        public static Task<string> GetDeviceCategoryAsync()
        {
            return GetRootDeviceInfoAsync(PrimaryCategoryKey);
        }

        public static async Task<string> GetWindowsVersionAsync()
        {
            // There is no good place to get this.
            // The HAL driver version number should work unless you're using a custom HAL... 
            var hal = await GetHalDevice(DeviceDriverVersionKey);
            if (hal == null || !hal.Properties.ContainsKey(DeviceDriverVersionKey))
                return null;

            var versionParts = hal.Properties[DeviceDriverVersionKey].ToString().Split('.');
            return string.Join(".", versionParts.Take(2).ToArray());
        }

        private static async Task<string> GetRootDeviceInfoAsync(string propertyKey)
        {
            var pnp = await PnpObject.CreateFromIdAsync(PnpObjectType.DeviceContainer,
                      RootContainer, new[] { propertyKey });
            return (string)pnp.Properties[propertyKey];
        }

        private static async Task<PnpObject> GetHalDevice(params string[] properties)
        {
            var actualProperties = properties.Concat(new[] { DeviceClassKey });
            var rootDevices = await PnpObject.FindAllAsync(PnpObjectType.Device,
                actualProperties, RootQuery);

            foreach (var rootDevice in rootDevices.Where(d => d.Properties != null && d.Properties.Any()))
            {
                var lastProperty = rootDevice.Properties.Last();
                if (lastProperty.Value != null)
                    if (lastProperty.Value.ToString().Equals(HalDeviceClass))
                        return rootDevice;
            }
            return null;
        }
    }

    internal class DeviceInfo
    {
        /// <summary>
        /// Device Id 
        /// like: 307416080***************************************************
        /// </summary
        public string DeviceID { get; set; }
        /// <summary>
        /// Device OS 
        /// like: 6.2-Microsoft Corporation-Surface with Windows RT-Computer.Tablet
        /// </summary>
        public string DeviceOS { get; set; }
        /// <summary>
        /// Windows Version 
        /// like: 6.2
        /// </summary>
        public string WindowsVersion { get; set; }
        /// <summary>
        /// Device Model 
        /// like: Surface with Windows RT
        /// </summary>
        public string DeviceModel { get; set; }
        /// <summary>
        /// Device Category 
        /// like: Computer.Tablet
        /// </summary>
        public string DeviceCategory { get; set; }
        /// <summary>
        /// Device Manufacturer
        /// like: Microsoft Corporation
        /// </summary>
        public string DeviceManufacturer { get; set; }
    }
}
