/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

namespace Lab06
{
    /// <summary>Micro không dây.</summary>
    public class Micro : TeachingDevice
    {
        public Micro(string id, string name, int yearInUse, decimal price,
                     int maxBorrowPeriods = 1, DeviceStatus status = DeviceStatus.Available)
            : base(id, name, yearInUse, price, maxBorrowPeriods, status) { }

        // Micro: 10% giá mua mỗi năm (thay pin, vệ sinh)
        public override decimal CalculateAnnualMaintenanceCost() => PurchasePrice * 0.10m;

        public override string ToString() => "Micro " + base.ToString();
    }

    /// <summary>Điều khiển điều hòa.</summary>
    public class AirConditionerRemoteControl : TeachingDevice
    {
        public AirConditionerRemoteControl(string id, string name, int yearInUse, decimal price,
                     int maxBorrowPeriods = 3, DeviceStatus status = DeviceStatus.Available)
            : base(id, name, yearInUse, price, maxBorrowPeriods, status) { }

        // Điều khiển điều hòa: 5% giá mua mỗi năm
        public override decimal CalculateAnnualMaintenanceCost() => PurchasePrice * 0.05m;

        public override string ToString() => "Điều khiển điều hòa " + base.ToString();
    }

    /// <summary>Điều khiển máy chiếu.</summary>
    public class ProjectorRemoteControl : TeachingDevice
    {
        public ProjectorRemoteControl(string id, string name, int yearInUse, decimal price,
                     int maxBorrowPeriods = 5, DeviceStatus status = DeviceStatus.Available)
            : base(id, name, yearInUse, price, maxBorrowPeriods, status) { }

        // Điều khiển máy chiếu: 8% giá mua mỗi năm
        public override decimal CalculateAnnualMaintenanceCost() => PurchasePrice * 0.08m;

        public override string ToString() => "Điều khiển máy chiếu " + base.ToString();
    }
}
