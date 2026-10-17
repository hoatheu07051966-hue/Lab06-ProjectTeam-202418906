/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

using System;

namespace Lab06
{
    /// <summary>Lớp trừu tượng cơ sở của mọi thiết bị.</summary>
    public abstract class Device
    {
        private string _deviceName;
        private decimal _purchasePrice;
        private int _yearInUse;

        // Mã thiết bị chỉ được thiết lập khi khởi tạo
        public string DeviceId { get; }

        public string DeviceName
        {
            get => _deviceName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên thiết bị không được rỗng.");
                _deviceName = value;
            }
        }

        public int YearInUse
        {
            get => _yearInUse;
            set
            {
                if (value > DateTime.Now.Year)
                    throw new ArgumentException("Năm đưa vào sử dụng không được lớn hơn năm hiện tại.");
                _yearInUse = value;
            }
        }

        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Giá mua phải lớn hơn 0.");
                _purchasePrice = value;
            }
        }

        public DeviceStatus Status { get; protected set; }

        protected Device(string deviceId, string deviceName, int yearInUse,
                         decimal purchasePrice, DeviceStatus status = DeviceStatus.Available)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
                throw new ArgumentException("Mã thiết bị không được rỗng.");

            DeviceId = deviceId;
            DeviceName = deviceName;
            YearInUse = yearInUse;
            PurchasePrice = purchasePrice;
            Status = status;
        }

        /// <summary>Chi phí bảo trì dự kiến trong một năm (đa hình).</summary>
        public abstract decimal CalculateAnnualMaintenanceCost();

        /// <summary>Số năm đã sử dụng.</summary>
        public int GetAge() => DateTime.Now.Year - YearInUse;

        public override string ToString()
        {
            return $"[{DeviceId}] {DeviceName} - Năm {YearInUse} - Giá {PurchasePrice:N0} VND - {Status}";
        }
    }
}
