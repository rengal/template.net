using System;
using System.Collections.Generic;
using Resto.Common.Properties;
using Resto.Data;
using Resto.Configuration;
using Resto.Framework.Data;
using System.Linq;

namespace Resto.Configuration
{
    #region IDeviceListCreator Interface

    public interface IDeviceListCreator
    {
        void Create(PrinterDeviceType deviceType);
        void Create(CameraDeviceType deviceType);
        void Create(CashRegisterDeviceType deviceType);
        void Create(CardProcessingDeviceType deviceType);
        void Create(CustomerDisplayDeviceType deviceType);
        void Create(ScaleDeviceType deviceType);
        void Create(PowerDeviceType deviceType);
        void Create(CashDrawerDeviceType deviceType);
    }

    #endregion IDeviceListCreator Interface

    #region IEntityListenerSubscriber Interface

    public interface IEntityListenerSubscriber
    {
        void Subscribe(CameraDeviceType deviceType);
        void Subscribe(CardProcessingDeviceType deviceType);
        void Subscribe(CashRegisterDeviceType deviceType);
        void Subscribe(CustomerDisplayDeviceType deviceType);
        void Subscribe(PrinterDeviceType deviceType);
        void Subscribe(ScaleDeviceType deviceType);
        void Subscribe(PowerDeviceType deviceType);
        void Subscribe(CashDrawerDeviceType deviceType);
    }

    #endregion IEntityListenerSubscriber Interface
}

namespace Resto.Data
{
    #region DriverSettings Class

    public delegate AgentDriver CreateDriverDelegate();
    public delegate DeviceSettings CreateDriverSettingsDelegate();

    public sealed class DriverSettings
    {
        #region Constructors

        public DriverSettings(string javaDriver, CreateDriverDelegate createDriverDelegate, CreateDriverSettingsDelegate createDriverSettingsDelegate)
        {
            m_javaDriver = javaDriver;
            m_createDriverDelegate = createDriverDelegate;
            m_createDriverSettingsDelegate = createDriverSettingsDelegate;
        }

        #endregion

        #region Properties

        public string JavaDriver
        {
            get { return m_javaDriver; }
        }

        public CreateDriverDelegate CreateDriverDelegate
        {
            get { return m_createDriverDelegate; }
        }

        public CreateDriverSettingsDelegate CreateDriverSettingsDelegate
        {
            get { return m_createDriverSettingsDelegate; }
        }

        #endregion

        #region Data Members

        private string m_javaDriver;
        private CreateDriverDelegate m_createDriverDelegate;
        private CreateDriverSettingsDelegate m_createDriverSettingsDelegate;

        #endregion
    }

    #endregion DriverSettings Class

    #region DeviceType Class

    /// <summary>
    /// Provides the abstract base class for some device type.
    /// </summary>
    public abstract class DeviceTypeBase
    {
        #region Constructors

        protected DeviceTypeBase(DriverSettings[] driverSettings)
        {
            m_driverSettings = driverSettings;
        }

        #endregion

        #region Methods

        public abstract void CreateDeviceList(IDeviceListCreator deviceListCreator);

        public abstract AgentDevice CreateDevice(Guid id, Guid? agentId, AgentDriver driver, string friendlyName);

        public abstract AgentDevice[] GetRegisteredDevices();

        public abstract void SubscribeEntityListener(IEntityListenerSubscriber subscriber);

        #endregion

        #region Properties

        public abstract string JavaDevice { get; }

        public abstract bool AllowsVirtualDevices { get; }

        public abstract string DeviceTypeName { get; }

        public DriverSettings[] DriverSettings
        {
            get { return m_driverSettings; }
        }

        public virtual DriverSettings NullDeviceDriverSettings => null;

        public abstract string Prompt { get; }

        #endregion

        #region Implementation

        protected AgentDevice[] GetRegisteredDevicesImpl<T>() where T : AgentDevice
        {
            var list = EntityManager.INSTANCE.GetAll<T>();

            var devices = new AgentDevice[list.Count];
            if (list.Count > 0)
            {
                for (var i = 0; i < list.Count; i++)
                    devices[i] = list[i];
            }

            return devices;
        }

        #endregion

        #region Data Members

        private DriverSettings[] m_driverSettings;

        #endregion
    }

    #endregion DeviceTypeBase Class

    #region PrinterDeviceType Class

    public class PrinterDeviceType : DeviceTypeBase
    {
        #region Constructors

        public PrinterDeviceType(DriverSettings[] driverSettings)
            : base(driverSettings)
        {
        }

        #endregion

        #region DeviceType Members

        public override AgentDevice CreateDevice(Guid id, Guid? agentId, AgentDriver driver, string friendlyName)
        {
            return new PrinterDevice(
                id,
                new LocalizableValue(friendlyName),
                agentId,
                driver);
        }

        public override void CreateDeviceList(IDeviceListCreator deviceListCreator)
        {
            deviceListCreator.Create(this);
        }

        public override AgentDevice[] GetRegisteredDevices()
        {
            return GetRegisteredDevicesImpl<PrinterDevice>();
        }

        public override void SubscribeEntityListener(IEntityListenerSubscriber subscriber)
        {
            subscriber.Subscribe(this);
        }

        public override string JavaDevice
        {
            get { return "PrinterDevice"; }
        }

        public override bool AllowsVirtualDevices
        {
            get { return false; }
        }

        public override string DeviceTypeName
        {
            get { return Resources.PrinterDeviceTypeDeviceTypeName; }
        }

        public override DriverSettings NullDeviceDriverSettings
        {
            get
            {
                return DriverSettings.Single(x => x.JavaDriver == nameof(PrinterNullDriver));
            }
        }

        public override string Prompt
        {
            get { return Resources.PrinterDeviceTypePrompt; }
        }

        #endregion
    }

    #endregion PrinterDeviceType Class

    #region CameraDeviceType Class

    public class CameraDeviceType : DeviceTypeBase
    {
        #region Constructors

        public CameraDeviceType(DriverSettings[] driverSettings)
            : base(driverSettings)
        {
        }

        #endregion

        #region DeviceType Members

        public override AgentDevice CreateDevice(Guid id, Guid? agentId, AgentDriver driver, string friendlyName)
        {
            return new Camera(
                id,
                new LocalizableValue(friendlyName),
                agentId,
                driver);
        }

        public override void CreateDeviceList(IDeviceListCreator deviceListCreator)
        {
            deviceListCreator.Create(this);
        }

        public override AgentDevice[] GetRegisteredDevices()
        {
            return GetRegisteredDevicesImpl<Camera>();
        }

        public override void SubscribeEntityListener(IEntityListenerSubscriber subscriber)
        {
            subscriber.Subscribe(this);
        }

        public override string JavaDevice
        {
            get { return "CameraDevice"; }
        }

        public override bool AllowsVirtualDevices
        {
            get { return false; }
        }

        public override string DeviceTypeName
        {
            get { return Resources.CameraDeviceTypeDeviceTypeName; }
        }

        public override string Prompt
        {
            get { return Resources.CameraDeviceTypePrompt; }
        }

        #endregion
    }

    #endregion CameraDeviceType Class

    #region CashRegisterDeviceType Class

    public class CashRegisterDeviceType : DeviceTypeBase
    {
        #region Constructors

        public CashRegisterDeviceType(DriverSettings[] driverSettings)
            : base(driverSettings)
        {
        }

        #endregion

        #region DeviceType Members

        public override AgentDevice CreateDevice(Guid id, Guid? agentId, AgentDriver driver, string friendlyName)
        {
            return new CashRegister(id, new LocalizableValue(friendlyName), agentId, driver, 1, -1,
                 CashRegisterConnectingStatus.DISCONNECTED, CashRegisterCloseEncashmentSetup.FULLANDPAYIN, 0)
                 {
                     PrintNds = true
                 };
        }

        public override void CreateDeviceList(IDeviceListCreator deviceListCreator)
        {
            deviceListCreator.Create(this);
        }

        public override AgentDevice[] GetRegisteredDevices()
        {
            return GetRegisteredDevicesImpl<CashRegister>();
        }

        public override void SubscribeEntityListener(IEntityListenerSubscriber subscriber)
        {
            subscriber.Subscribe(this);
        }

        public override string JavaDevice
        {
            get { return "CashRegister"; }
        }

        public override bool AllowsVirtualDevices
        {
            get { return true; }
        }

        public override string DeviceTypeName
        {
            get { return Resources.CashRegisterDeviceTypeDeviceTypeName; }
        }

        public override DriverSettings NullDeviceDriverSettings
        {
            get
            {
                return DriverSettings.Single(x => x.JavaDriver == nameof(ChequePrinterNullDriver));
            }
        }

        public override string Prompt
        {
            get { return Resources.CashRegisterDeviceTypePrompt; }
        }

        #endregion
    }

    #endregion CashRegisterDeviceType Class

    #region CardProcessingDeviceType Class

    public class CardProcessingDeviceType : DeviceTypeBase
    {
        #region Constructors

        public CardProcessingDeviceType(DriverSettings[] driverSettings)
            : base(driverSettings)
        {
        }

        #endregion

        #region DeviceType Members

        public override AgentDevice CreateDevice(Guid id, Guid? agentId, AgentDriver driver, string friendlyName)
        {
            if (driver is DiscountDriver)
            {
                return new DiscountDevice(id, new LocalizableValue(friendlyName), agentId, driver);
            }
            else
            {
                return new CardProcessingDevice(id, new LocalizableValue(friendlyName), agentId, driver);
            }
        }

        public override void CreateDeviceList(IDeviceListCreator deviceListCreator)
        {
            deviceListCreator.Create(this);
        }

        public override AgentDevice[] GetRegisteredDevices()
        {
            List<AgentDevice> result = new List<AgentDevice>();
            result.AddRange(GetRegisteredDevicesImpl<CardProcessingDevice>());
            result.AddRange(GetRegisteredDevicesImpl<DiscountDevice>());
            return result.ToArray();
        }

        public override void SubscribeEntityListener(IEntityListenerSubscriber subscriber)
        {
            subscriber.Subscribe(this);
        }

        public override string JavaDevice
        {
            get { return "CardProcessing"; }
        }

        public override bool AllowsVirtualDevices
        {
            get { return true; }
        }

        public override string DeviceTypeName
        {
            get { return Resources.CardProcessingDeviceTypeDeviceTypeName; }
        }

        public override string Prompt
        {
            get { return Resources.CardProcessingDeviceTypePrompt; }
        }

        #endregion
    }

    #endregion CardProcessingDeviceType Class

    #region CustomerDisplayDeviceType Class

    public class CustomerDisplayDeviceType : DeviceTypeBase
    {
        #region Constructors

        public CustomerDisplayDeviceType(DriverSettings[] driverSettings)
            : base(driverSettings)
        {
        }

        #endregion

        #region DeviceType Members

        public override AgentDevice CreateDevice(Guid id, Guid? agentId, AgentDriver driver, string friendlyName)
        {
            return new CustomerDisplayDevice(id, new LocalizableValue(friendlyName), agentId, driver, null);
        }

        public override void CreateDeviceList(IDeviceListCreator deviceListCreator)
        {
            deviceListCreator.Create(this);
        }

        public override AgentDevice[] GetRegisteredDevices()
        {
            return GetRegisteredDevicesImpl<CustomerDisplayDevice>();
        }

        public override void SubscribeEntityListener(IEntityListenerSubscriber subscriber)
        {
            subscriber.Subscribe(this);
        }

        public override string JavaDevice
        {
            get { return "CustomerDisplay"; }
        }

        public override bool AllowsVirtualDevices
        {
            get { return false; }
        }

        public override string DeviceTypeName
        {
            get { return Resources.CustomerDisplayDeviceTypeDeviceTypeName; }
        }

        public override string Prompt
        {
            get { return Resources.CustomerDisplayDeviceTypePrompt; }
        }

        #endregion
    }

    #endregion CustomerDisplayDeviceType Class

    #region ScaleDeviceType Class

    public class ScaleDeviceType : DeviceTypeBase
    {
        #region Constructors

        public ScaleDeviceType(DriverSettings[] driverSettings)
            : base(driverSettings)
        {
        }

        #endregion

        #region DeviceType Members

        public override AgentDevice CreateDevice(Guid id, Guid? agentId, AgentDriver driver, string friendlyName)
        {
            return new ScaleDevice(id, new LocalizableValue(friendlyName), agentId, driver, null);
        }

        public override void CreateDeviceList(IDeviceListCreator deviceListCreator)
        {
            deviceListCreator.Create(this);
        }

        public override AgentDevice[] GetRegisteredDevices()
        {
            return GetRegisteredDevicesImpl<ScaleDevice>();
        }

        public override void SubscribeEntityListener(IEntityListenerSubscriber subscriber)
        {
            subscriber.Subscribe(this);
        }

        public override string JavaDevice
        {
            get { return "ScaleDevice"; }
        }

        public override bool AllowsVirtualDevices
        {
            get { return false; }
        }

        public override string DeviceTypeName
        {
            get { return Resources.ScaleDeviceTypeDeviceTypeName; }
        }

        public override DriverSettings NullDeviceDriverSettings
        {
            get
            {
                return DriverSettings.Single(x => x.JavaDriver == nameof(ScaleNullDriver));
            }
        }

        public override string Prompt
        {
            get { return Resources.ScaleDeviceTypePrompt; }
        }
        #endregion
    }
    #endregion

    #region PowerDeviceType Class

    public class PowerDeviceType : DeviceTypeBase
    {
        #region Constructors

        public PowerDeviceType(DriverSettings[] driverSettings)
            : base(driverSettings)
        {
        }

        #endregion

        #region DeviceType Members

        public override AgentDevice CreateDevice(Guid id, Guid? agentId, AgentDriver driver, string friendlyName)
        {
            return new PowerDevice(id, new LocalizableValue(friendlyName), agentId, driver);
        }

        public override void CreateDeviceList(IDeviceListCreator deviceListCreator)
        {
            deviceListCreator.Create(this);
        }

        public override AgentDevice[] GetRegisteredDevices()
        {
            return GetRegisteredDevicesImpl<PowerDevice>();
        }

        public override void SubscribeEntityListener(IEntityListenerSubscriber subscriber)
        {
            subscriber.Subscribe(this);
        }

        public override string JavaDevice
        {
            get { return "PowerDevice"; }
        }

        public override bool AllowsVirtualDevices
        {
            get { return false; }
        }

        public override string DeviceTypeName
        {
            get { return Resources.PowerDeviceTypeDeviceTypeName; }
        }

        public override string Prompt
        {
            get { return Resources.PowerDeviceTypePrompt; }
        }

        #endregion
    }

    #endregion PowerDeviceType Class

    #region CashDrawerDeviceType Class

    public class CashDrawerDeviceType : DeviceTypeBase
    {
        #region Constructors

        public CashDrawerDeviceType(DriverSettings[] driverSettings)
            : base(driverSettings)
        {
        }

        #endregion

        #region DeviceType Members

        public override AgentDevice CreateDevice(Guid id, Guid? agentId, AgentDriver driver, string friendlyName)
        {
            return new CashDrawerDevice(
                id,
                new LocalizableValue(friendlyName),
                agentId,
                driver
                );
        }

        public override void CreateDeviceList(IDeviceListCreator deviceListCreator)
        {
            deviceListCreator.Create(this);
        }

        public override AgentDevice[] GetRegisteredDevices()
        {
            return GetRegisteredDevicesImpl<CashDrawerDevice>();
        }

        public override void SubscribeEntityListener(IEntityListenerSubscriber subscriber)
        {
            subscriber.Subscribe(this);
        }

        public override string JavaDevice
        {
            get { return "CashDrawerDevice"; }
        }

        public override bool AllowsVirtualDevices
        {
            get { return false; }
        }

        public override string DeviceTypeName
        {
            get { return Resources.CashDrawerDeviceTypeDeviceTypeName; }
        }

        public override string Prompt
        {
            get { return Resources.CashDrawerDeviceTypePrompt; }
        }
        #endregion
    }
    #endregion CashDrawerDeviceType
}