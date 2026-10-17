/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

using System;

namespace Lab06
{
    /// <summary>Thiết bị phục vụ giảng dạy, có giới hạn thời gian mượn.</summary>
    public abstract class TeachingDevice : Device
    {
        private int _maxBorrowPeriods;

        public int MaxBorrowPeriods
        {
            get => _maxBorrowPeriods;
            protected set
            {
                if (value <= 0)
                    throw new ArgumentException("Giới hạn mượn phải lớn hơn 0.");
                _maxBorrowPeriods = value;
            }
        }

        protected TeachingDevice(string deviceId, string deviceName, int yearInUse,
                                 decimal purchasePrice, int maxBorrowPeriods,
                                 DeviceStatus status = DeviceStatus.Available)
            : base(deviceId, deviceName, yearInUse, purchasePrice, status)
        {
            MaxBorrowPeriods = maxBorrowPeriods;
        }

        public void MarkAsBorrowed() => Status = DeviceStatus.Borrowed;

        public void MarkAsAvailable() => Status = DeviceStatus.Available;
    }
}
